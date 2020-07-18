using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace ForEvolve.AspNetCore.Localization
{
    public class LocalizationOptionsInitializer : IConfigureOptions<LocalizationOptions>
    {
        public const string DefaultResourcesPath = "Resources";

        public void Configure(LocalizationOptions options)
        {
            options.ResourcesPath = DefaultResourcesPath;
        }
    }
}
