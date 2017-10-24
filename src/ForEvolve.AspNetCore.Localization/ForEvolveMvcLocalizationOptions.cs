using System;

namespace ForEvolve.AspNetCore.Localization
{
    public class ForEvolveMvcLocalizationOptions
    {
        public bool EnableViewLocalization { get; set; }
        public bool EnableDataAnnotationsLocalization { get; set; }
        public Action<ILocalizationValidationMetadataProvider> ConfigureValidationMetadataProvider { get; set; }
    }
}
