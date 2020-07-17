using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Localization;
using System;

namespace ForEvolve.AspNetCore.Localization
{
    public class ForEvolveLocalizationOptions : LocalizationOptions
    {
        public bool EnableViewLocalization { get; set; }
        public bool EnableDataAnnotationsLocalization { get; set; }
        public Action<ILocalizationValidationMetadataProvider> ConfigureValidationMetadataProvider { get; set; }
        public ForEvolveMvcDefaultLocalizationAdapterOptions DefaultAdapterOptions { get; set; }
    }
}
