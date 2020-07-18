using ForEvolve.AspNetCore.Localization.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace ForEvolve.AspNetCore.Localization
{

    public class MvcOptionsInitializer : IConfigureOptions<MvcOptions>
    {
        private readonly ILocalizationValidationMetadataProvider _defaultValidationMetadataProvider;
        public MvcOptionsInitializer(ILocalizationValidationMetadataProvider defaultValidationMetadataProvider)
        {
            _defaultValidationMetadataProvider = defaultValidationMetadataProvider ?? throw new ArgumentNullException(nameof(defaultValidationMetadataProvider));
        }

        public void Configure(MvcOptions options)
        {
            options.ModelMetadataDetailsProviders.Add(_defaultValidationMetadataProvider);
        }
    }
}
