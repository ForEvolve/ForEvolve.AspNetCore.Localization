using ForEvolve.AspNetCore.Localization;
using ForEvolve.AspNetCore.Localization.Adapters;
using ForEvolve.AspNetCore.Localization.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ForEvolveLocalizationStartupExtensions
    {
        public const string DefaultResourcesPath = "Resources";

        public static IServiceCollection AddForEvolveLocalization(this IServiceCollection services)
        {
            return services
                .AddForEvolveLocalization(options => { });
        }

        public static IServiceCollection AddForEvolveLocalization(this IServiceCollection services, Action<ForEvolveLocalizationOptions> setupAction)
        {
            var localizationOptions = new ForEvolveLocalizationOptions
            {
                ResourcesPath = DefaultResourcesPath
            };
            setupAction(localizationOptions);
            return services
                .AddLocalization(options =>
                {
                    options.ResourcesPath = localizationOptions.ResourcesPath;
                });
        }

        public static IMvcBuilder AddForEvolveMvcLocalization(this IMvcBuilder mvcBuilder)
        {
            return mvcBuilder
                .AddForEvolveMvcLocalization(options => { });
        }

        public static IMvcBuilder AddForEvolveMvcLocalization(this IMvcBuilder mvcBuilder, Action<ForEvolveMvcLocalizationOptions> setupAction)
        {
            // Setup the options
            var localizationOptions = new ForEvolveMvcLocalizationOptions
            {
                EnableDataAnnotationsLocalization = true,
                EnableViewLocalization = true,
                ConfigureValidationMetadataProvider = provider => { },
                DefaultAdapterOptions = new ForEvolveMvcDefaultLocalizationAdapterOptions()
            };
            setupAction(localizationOptions);

            // Create and configure the LocalizationValidationMetadataProvider
            var defaultValidationMetadataProvider = new ForEvolveLocalizationValidationMetadataProvider<DataAnnotationSharedResource>(
                // Custom multi-messages adapter that duplicate the attribute logic
                // A better solution is welcome :)
                new StringLengthLocalizationValidationAttributeAdapter(),
                
                // Keep this one last
                new DefaultLocalizationValidationAttributeAdapter(localizationOptions.DefaultAdapterOptions)
            );
            localizationOptions.ConfigureValidationMetadataProvider(defaultValidationMetadataProvider);

            // Add the new ValidationMetadataProvider to MVC
            mvcBuilder
                 .AddMvcOptions(options =>
                {
                    options.ModelMetadataDetailsProviders.Add(defaultValidationMetadataProvider);
                });

            // Add the default ViewLocalization (opt-out basis)
            if (localizationOptions.EnableViewLocalization)
            {
                mvcBuilder.AddViewLocalization();
            }

            // Add the default DataAnnotationsLocalization (opt-out basis)
            if (localizationOptions.EnableDataAnnotationsLocalization)
            {
                mvcBuilder.AddDataAnnotationsLocalization();
            }

            return mvcBuilder;
        }
    }
}
