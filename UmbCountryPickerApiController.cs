using EarthCountriesInfo;
using HumanLanguages;
using UmbCountryPicker.Models;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.BackOffice.Filters;
using Umbraco.Cms.Web.Common.Attributes;

namespace UmbCountryPicker;

[ValidateAngularAntiForgeryToken]
[PluginController("UmbCountryPicker")]
public sealed class UmbCountryPickerApiController : UmbracoAuthorizedJsonController
{
    private readonly UmbCountryPickerConfig _config;

    public UmbCountryPickerApiController(UmbCountryPickerConfig config)
    {
        _config = config;
    }

    public IEnumerable<DropdownItemDTO> GetKeyValueList(string languageIsoCodeString = "en")
    {
        var languageIsoCode = HumanHelper.CreateLanguageIsoCode(languageIsoCodeString);

        var list = Countries.CountryPropertiesDictionary.Select(kvp =>
        {
            var isoCode = kvp.Key.ToString();
            var props = kvp.Value;

            var name = props.CountryNames.TryGetValue(languageIsoCode.LanguageId, out var translated)
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
}