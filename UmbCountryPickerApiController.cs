using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.BackOffice.Filters;
using Umbraco.Cms.Web.Common.Attributes;
using EarthCountriesInfo;
using HumanLanguages;

namespace UmbCountryPicker_v13;

[ValidateAngularAntiForgeryToken]
[PluginController("UmbCountryPicker")]
public sealed class UmbCountryPickerApiController : UmbracoAuthorizedJsonController
{
    public IEnumerable<DropdownItemDTO> GetKeyValueList(string languageIsoCodeString = "en")
    {
        LanguageIsoCode languageIsoCode = HumanHelper.CreateLanguageIsoCode(languageIsoCodeString);

        return Countries.CountryPropertiesDictionary.Select(kvp =>
        {
            // ReSharper disable once HeapView.BoxingAllocation
            var isoCode = kvp.Key.ToString();
            var props = kvp.Value;

            // Fallback to English if the requested language is not found
            var name = props.CountryNames.TryGetValue(languageIsoCode.LanguageId, out var translatedName) && !string.IsNullOrWhiteSpace(translatedName)
                ? translatedName
                : props.CountryNames.TryGetValue(LanguageId.en, out var englishName)
                    ? englishName
                    : isoCode; // fallback fallback

            return new DropdownItemDTO
            {
                Id = isoCode,
                CountryName = name
            };
        });
    }
}