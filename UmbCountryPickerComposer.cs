using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using UmbCountryPicker.Models;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace UmbCountryPicker;

public class UmbCountryPickerComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddSingleton(provider =>
        {
            var env = provider.GetService<IWebHostEnvironment>();
            var configPath = Path.Combine(
                env.ContentRootPath,
                "App_Plugins",
                "UmbCountryPicker",
                "defaultOverrides.json");

            if (!File.Exists(configPath))
                return new UmbCountryPickerConfig(); // empty config, safe fallback

            var json = File.ReadAllText(configPath);

            return JsonSerializer.Deserialize<UmbCountryPickerConfig>(json)
                   ?? new UmbCountryPickerConfig();
        });
    }
}