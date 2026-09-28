using EarthCountriesInfo;
using HumanLanguages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UmbCountryPicker.Models;
using Umbraco.Cms.Api.Management.Controllers;
using Umbraco.Cms.Api.Management.Routing;
using Umbraco.Cms.Web.Common.Authorization;

namespace UmbCountryPicker.Api;

/// <summary>
///     Supplies the country list to the backoffice property editor UI.
///     Served at /umbraco/management/api/v1/country-picker/countries.
/// </summary>
/// <remarks>
///     ManagementApiControllerBase already applies [ApiVersion], [MapToApi("management")] and
///     [Authorize(BackOfficeAccess)]; the BackOfficeAccess policy is restated here so the
///     authorization requirement is visible at the call site.
/// </remarks>
[VersionedApiBackOfficeRoute("country-picker")]
[ApiExplorerSettings(GroupName = "Country Picker")]
[Authorize(Policy = AuthorizationPolicies.BackOfficeAccess)]
public sealed class UmbCountryPickerController : ManagementApiControllerBase
{
    private readonly UmbCountryPickerConfig _config;

    public UmbCountryPickerController(UmbCountryPickerConfig config)
    {
        _config = config;
    }

    /// <summary>
    ///     Gets every known country as an id/name pair, localized to <paramref name="languageIsoCodeString" />
    ///     and with the configured renames and additions applied.
    /// </summary>
    [HttpGet("countries")]
    [ProducesResponseType(typeof(IEnumerable<DropdownItemDTO>), StatusCodes.Status200OK)]
    public IActionResult GetCountries(string languageIsoCodeString = "en")
    {
        return Ok(GetKeyValueList(languageIsoCodeString));
    }

    private IEnumerable<DropdownItemDTO> GetKeyValueList(string languageIsoCodeString)
    {
        var languageId = ResolveLanguageId(languageIsoCodeString);

        var list = Countries.CountryPropertiesDictionary.Select(kvp =>
        {
            var isoCode = kvp.Key.ToString();
            var props = kvp.Value;

            var name = props.CountryNames.TryGetValue(languageId, out var translated)
                       && !string.IsNullOrWhiteSpace(translated)
                ? translated
                : props.CountryNames.TryGetValue(LanguageId.en, out var english)
                    ? english
                    : isoCode;

            return new DropdownItemDTO
            {
                Id = isoCode,
                CountryName = name
            };
        }).ToList();

        // Apply renames
        foreach (var rename in _config.Overrides.Renames)
        {
            var item = list.FirstOrDefault(x => x.Id.Equals(rename.Code, StringComparison.OrdinalIgnoreCase));
            if (item != null) item.CountryName = rename.Name;
        }

        // Add missing countries
        foreach (var add in _config.Overrides.Additions)
            if (!list.Any(x => x.Id.Equals(add.Code, StringComparison.OrdinalIgnoreCase)))
                list.Add(new DropdownItemDTO
                {
                    Id = add.Code,
                    CountryName = add.Name
                });

        return list.OrderBy(x => x.CountryName);
    }

    /// <summary>
    ///     Resolves a backoffice culture (for example "da-DK") to a <see cref="LanguageId" />, falling back to
    ///     English when the culture is not one HumanLanguages recognises.
    /// </summary>
    private static LanguageId ResolveLanguageId(string languageIsoCodeString)
    {
        if (string.IsNullOrWhiteSpace(languageIsoCodeString))
            return LanguageId.en;

        return HumanHelper.TryCreateLanguageIsoCode(languageIsoCodeString, out var languageIsoCode)
               && languageIsoCode is not null
            ? languageIsoCode.LanguageId
            : LanguageId.en;
    }
}
