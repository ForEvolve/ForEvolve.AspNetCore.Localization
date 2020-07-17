using ForEvolve.AspNetCore.Localization;
using ForEvolve.AspNetCore.Localization.Adapters;
using ForEvolve.AspNetCore.Localization.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ForEvolveLocalizationStartupExtensions
    {
        public const string DefaultResourcesPath = "Resources";

        /// <summary>
        /// Adds services required for application localization.
        /// The Asp.Net Core AddLocalization() method will be called.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the services to.</param>
        /// <returns>The Microsoft.Extensions.DependencyInjection.IServiceCollection so that additional calls can be chained.</returns>
        [Obsolete(@"You can delete the call to this extension method. 
Use the IMvcBuilder.AddForEvolveMvcLocalization() extension method instead.

This method will be removed in a future major release.")]
        public static IServiceCollection AddForEvolveLocalization(this IServiceCollection services)
        {
            return services;
        }

        /// <summary>
        /// Adds services required for application localization.
        /// The Asp.Net Core AddLocalization() method will be called.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the services to.</param>
        /// <param name="setupAction">An System.Action&lt;ForEvolveLocalizationOptions&gt; to configure the ForEvolve.AspNetCore.Localization.ForEvolveLocalizationOptions.</param>
        /// <returns>The Microsoft.Extensions.DependencyInjection.IServiceCollection so that additional calls can be chained.</returns>
        [Obsolete(@"You can delete the call to this extension method. 
Use the IMvcBuilder.AddForEvolveMvcLocalization() extension method instead.

This method will be removed in a future major release.")]
        public static IServiceCollection AddForEvolveLocalization(this IServiceCollection services, Action<ForEvolveLocalizationOptions> setupAction)
        {

            return services;
        }

        [Obsolete("Call IMvcBuilder.AddForEvolveLocalization() instead; this method will be removed in a future major release.")]
        public static IMvcBuilder AddForEvolveMvcLocalization(this IMvcBuilder mvcBuilder)
        {
            return AddForEvolveLocalization(mvcBuilder);
        }

        /// <summary>
        /// Adds services required for application localization.
        /// Calls the IServiceCollection.AddLocalization() method.
        /// Registers an IMetadataDetailsProvider that handles validation attributes to Microsoft.AspNetCore.Mvc.MvcOptions.
        /// Calls AddViewLocalization() and AddDataAnnotationsLocalization() on the IMvcBuilder (you can opt-out by configuring the options).
        /// </summary>
        /// <param name="mvcBuilder">The Microsoft.Extensions.DependencyInjection.IMvcBuilder.</param>
        /// <returns>The Microsoft.Extensions.DependencyInjection.IMvcBuilder so that additional calls can be chained.</returns>
        public static IMvcBuilder AddForEvolveLocalization(this IMvcBuilder mvcBuilder)
        {
            // New registration
            var services = mvcBuilder.Services;
            services
                .AddSingleton<IConfigureOptions<RequestLocalizationOptions>, RequestLocalizationOptionsInitializer>()
                .AddSingleton<ISupportedCulturesCollection, SupportedCulturesCollection>()
            ;

            // Localization Options
            var localizationOptions = new ForEvolveLocalizationOptions
            {
                ResourcesPath = DefaultResourcesPath,
                EnableDataAnnotationsLocalization = true,
                EnableViewLocalization = true,
                ConfigureValidationMetadataProvider = provider => { },
                DefaultAdapterOptions = new ForEvolveMvcDefaultLocalizationAdapterOptions(),
            };

            // Create and configure the LocalizationValidationMetadataProvider
            var defaultValidationMetadataProvider = new ForEvolveLocalizationValidationMetadataProvider<DataAnnotationSharedResource>(
                // Custom multi-messages adapter that duplicate the attribute logic
                // A better solution is welcome :)
                new StringLengthLocalizationValidationAttributeAdapter(),

                // Keep this one last
                new DefaultLocalizationValidationAttributeAdapter(localizationOptions.DefaultAdapterOptions)
            );
            localizationOptions
                .ConfigureValidationMetadataProvider(defaultValidationMetadataProvider);

            // Regiter services
            mvcBuilder.Services
                .AddSingleton(localizationOptions)
                //.AddSingleton<ILocalizationValidationMetadataProvider>(defaultValidationMetadataProvider)
                .AddLocalization(options =>
                {
                    options.ResourcesPath = localizationOptions.ResourcesPath;
                });

            // Add the ValidationMetadataProvider to MVC
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

        /// <summary>
        /// Adds the Microsoft.AspNetCore.Localization.RequestLocalizationMiddleware to automatically set culture information for requests based on information provided by the client.
        /// This calls UseRequestLocalization().
        /// </summary>
        /// <param name="app">The Microsoft.AspNetCore.Builder.IApplicationBuilder.</param>
        /// <returns>The Microsoft.AspNetCore.Builder.IApplicationBuilder so that additional calls can be chained..</returns>
        public static IApplicationBuilder UseForEvolveRequestLocalization(this IApplicationBuilder app)
        {
            var locOptions = app.ApplicationServices.GetService<ForEvolveLocalizationOptions>();
            if (locOptions == null)
            {
                throw new NullReferenceException($"{nameof(ForEvolveLocalizationOptions)} was not found. Please make sure that `services.AddForEvolveLocalization()` has been called in the `ConfigureServices(IServiceCollection services)` method.");
            }
            app.UseRequestLocalization();
            return app;
        }
    }
}
