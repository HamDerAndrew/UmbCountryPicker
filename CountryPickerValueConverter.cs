using EarthCountriesInfo;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace UmbCountryPicker;

public class CountryPickerValueConverter : IPropertyValueConverter
{
    public bool IsConverter(IPublishedPropertyType propertyType)
        => propertyType.EditorAlias == "UmbCountryPicker";

    // 1️1. Convert the raw DB value (string "ISO") → intermediate CountryIsoCode enum
    public object? ConvertSourceToIntermediate(IPublishedElement owner,
        IPublishedPropertyType propertyType, object? source, bool preview)
    {
        if (source == null) return null;

        // source might come in as JValue or plain string; normalize to a C# string
        var str = source as string ?? source.ToString()!;
        str = str.Trim('"'); // in case it’s quoted JSON

        // TryParse into your enum; if it fails, return null
        if (Enum.TryParse<CountryIsoCode>(str, ignoreCase: true, out var parsed))
            return parsed;

        return null;
    }

    // 2️2. Umbraco will pass your intermediate (`parsed`) here.
    //    Since it's already the enum, just return it.
    public object? ConvertIntermediateToObject(IPublishedElement owner,
        IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel,
        object? inter, bool preview)
    {
        return inter;
    }

    public object? ConvertIntermediateToXPath(IPublishedElement owner,
        IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel,
        object? inter, bool preview)
        => throw new NotSupportedException();

    public PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
        => PropertyCacheLevel.Element;

    public Type GetPropertyValueType(IPublishedPropertyType propertyType)
        => typeof(CountryIsoCode);

    public bool? IsValue(object? value, PropertyValueLevel level)
        => value is CountryIsoCode;
}