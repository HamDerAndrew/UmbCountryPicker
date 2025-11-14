using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UmbCountryPicker.Models;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace UmbCountryPicker.Composers;

public class UmbCountryPickerComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddSingleton(provider =>
        {
            var env = provider.GetRequiredService<IWebHostEnvironment>();

            // 1. Try appsettings.json
            var appsettingsConfig = provider.GetService<IOptions<UmbCountryPickerConfig>>()?.Value;
            if (appsettingsConfig != null &&
                (appsettingsConfig.Overrides.Renames.Any() || appsettingsConfig.Overrides.Additions.Any()))
            {
                return appsettingsConfig;
            }

            // 2. Try external override file 
            var overrideFile = Path.Combine(env.ContentRootPath, "umbraco-country-overrides.json");
            if (File.Exists(overrideFile))
            {
                var json = File.ReadAllText(overrideFile);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var fileConfig = JsonSerializer.Deserialize<UmbCountryPickerConfig>(json, options);
                if (fileConfig != null)
                    return fileConfig;
            }

            // 3. Fallback: no overrides
            return new UmbCountryPickerConfig();
        });
    }
}