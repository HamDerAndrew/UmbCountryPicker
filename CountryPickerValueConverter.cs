using EarthCountriesInfo;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace UmbCountryPicker;

[DefaultPropertyValueConverter]
public class CountryPickerValueConverter : PropertyValueConverterBase
{
    public override bool IsConverter(IPublishedPropertyType propertyType)
    {
        // Umbraco's v13 -> v14 migration moves the old package.manifest alias onto the Data Type's
        // EditorUiAlias and rewrites EditorAlias to the core schema (Umbraco.Plain.String), so the
        // UI alias is what identifies this property editor from v14 onwards.
        return propertyType.EditorUiAlias == "UmbCountryPicker";
    }

    public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
    {
        return typeof(string);
    }

    public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
    {
        return PropertyCacheLevel.Element;
    }

    public override object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType,
        object? source, bool preview)
    {
        // normalise the stored value
        var str = source?.ToString()?.Trim('"').Trim();
        if (string.IsNullOrWhiteSpace(str))
            return null;

        // Try enum for standard ISO codes
        if (Enum.TryParse<CountryIsoCode>(str, true, out var parsed))
            // intermediate value is enum
            return parsed;

        // Fallback: custom or unknown ISO codes (e.g. CI, ME, HR, RS)
        return str;
    }

    public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType,
        PropertyCacheLevel referenceCacheLevel, object? inter, bool preview)
    {
        // If enum, return code string
        if (inter is CountryIsoCode enumValue)
            return enumValue.ToString();

        // If string, return as is
        return inter?.ToString();
    }
}
