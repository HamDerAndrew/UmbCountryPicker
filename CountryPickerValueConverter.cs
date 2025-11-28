using EarthCountriesInfo;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace UmbCountryPicker;

[DefaultPropertyValueConverter]
public class CountryPickerValueConverter : PropertyValueConverterBase
{
    public bool IsConverter(IPublishedPropertyType propertyType)
    {
        return propertyType.EditorAlias == "UmbCountryPicker";
    }

    public Type GetPropertyValueType(IPublishedPropertyType propertyType)
    {
        return typeof(string);
    }

    public PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
    {
        return PropertyCacheLevel.Element;
    }

    public object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType,
        object source, bool preview)
    {
        // normalise the stored value
        var str = source.ToString()?.Trim('"').Trim();
        if (string.IsNullOrWhiteSpace(str))
            return null;

        // Try enum for standard ISO codes
        if (Enum.TryParse<CountryIsoCode>(str, true, out var parsed))
            // intermediate value is enum
            return parsed;

        // Fallback: custom or unknown ISO codes (e.g. CI, ME, HR, RS)
        return str;
    }

    public object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType,
        PropertyCacheLevel referenceCacheLevel, object intermediate, bool preview)
    {
        // If enum, return code string
        if (intermediate is CountryIsoCode enumValue)
            return enumValue.ToString();

        // If string, return as is
        return intermediate.ToString();
    }

    public object? ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType,
        PropertyCacheLevel referenceCacheLevel, object intermediate, bool preview)
    {
        // XPath gets the same value as the frontend: string ISO code
        return ConvertIntermediateToObject(owner, propertyType, referenceCacheLevel, intermediate, preview);
    }
}