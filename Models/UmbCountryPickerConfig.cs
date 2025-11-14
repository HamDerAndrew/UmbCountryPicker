namespace UmbCountryPicker.Models;

public class UmbCountryPickerConfig
{
    public UmbCountryPickerOverrides Overrides { get; set; } = new();
}

public class UmbCountryPickerOverrides
{
    public List<UmbCountryRename> Renames { get; set; } = new();
    public List<UmbCountryAdd> Additions { get; set; } = new();
}

public class UmbCountryRename
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class UmbCountryAdd
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}