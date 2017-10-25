using ForEvolve.AspNetCore.Localization;
using ForEvolve.AspNetCore.Localization.Adapters;
using ForEvolve.AspNetCore.Localization.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ForEvolveLocalizationStartupExtensions
    {
        public const string DefaultResourcesPath = "Resources";

        private static ForEvolveLocalizationOptions Options { get; set; }
        private static ILocalizationValidationMetadataProvider ValidationMetadataProvider { get; set; }

        public static IServiceCollection AddForEvolveLocalization(this IServiceCollection services)
        {
            return services
                .AddForEvolveLocalization(options => { });
        }

        public static IServiceCollection AddForEvolveLocalization(this IServiceCollection services, Action<ForEvolveLocalizationOptions> setupAction)
        {
            // Localization Options
            var supportedCultures = new List<CultureInfo>(new[]
            {
                new CultureInfo("en"),
                new CultureInfo("fr"),
            });
            var defaultCulture = supportedCultures.First();
            var localizationOptions = new ForEvolveLocalizationOptions
            {
                ResourcesPath = DefaultResourcesPath,
                MvcOptions = new ForEvolveMvcLocalizationOptions
                {
                    EnableDataAnnotationsLocalization = true,
                    EnableViewLocalization = true,
                    ConfigureValidationMetadataProvider = provider => { },
                    DefaultAdapterOptions = new ForEvolveMvcDefaultLocalizationAdapterOptions()
                },
                RequestLocalizationOptions = CreateDefaultRequestLocalizationOptions(defaultCulture, supportedCultures)
            };
            setupAction(localizationOptions);

            // Create and configure the LocalizationValidationMetadataProvider
            var defaultValidationMetadataProvider = new ForEvolveLocalizationValidationMetadataProvider<DataAnnotationSharedResource>(
                // Custom multi-messages adapter that duplicate the attribute logic
                // A better solution is welcome :)
                new StringLengthLocalizationValidationAttributeAdapter(),

                // Keep this one last
                new DefaultLocalizationValidationAttributeAdapter(localizationOptions.MvcOptions.DefaultAdapterOptions)
            );
            localizationOptions.MvcOptions
                .ConfigureValidationMetadataProvider(defaultValidationMetadataProvider);

            // Regiter services
            services
                .AddSingleton(localizationOptions)
                //.AddSingleton<ILocalizationValidationMetadataProvider>(defaultValidationMetadataProvider)
                .AddLocalization(options =>
                {
                    options.ResourcesPath = localizationOptions.ResourcesPath;
                });

            // Keep global references
            Options = localizationOptions;
            ValidationMetadataProvider = defaultValidationMetadataProvider;

            return services;
        }

        public static IMvcBuilder AddForEvolveMvcLocalization(this IMvcBuilder mvcBuilder)
        {
            // Add the ValidationMetadataProvider to MVC
            mvcBuilder
                 .AddMvcOptions(options =>
                {
                    options.ModelMetadataDetailsProviders.Add(ValidationMetadataProvider);
                });

            // Add the default ViewLocalization (opt-out basis)
            if (Options.MvcOptions.EnableViewLocalization)
            {
                mvcBuilder.AddViewLocalization();
            }

            // Add the default DataAnnotationsLocalization (opt-out basis)
            if (Options.MvcOptions.EnableDataAnnotationsLocalization)
            {
                mvcBuilder.AddDataAnnotationsLocalization();
            }

            return mvcBuilder;
        }

        public static IApplicationBuilder UseForEvolveRequestLocalization(this IApplicationBuilder app)
        {
            var locOptions = app.ApplicationServices.GetService<ForEvolveLocalizationOptions>();
            if (locOptions == null)
            {
                throw new NullReferenceException($"{nameof(ForEvolveLocalizationOptions)} was not found. Please make sure that `services.AddForEvolveLocalization()` has been called in the `ConfigureServices(IServiceCollection services)` method.");
            }
            app.UseRequestLocalization(locOptions.RequestLocalizationOptions);
            return app;
        }

        private static RequestLocalizationOptions CreateDefaultRequestLocalizationOptions(CultureInfo defaultCulture, IList<CultureInfo> supportedCultures)
        {
            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                DefaultRequestCulture = new RequestCulture(defaultCulture, defaultCulture),

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                SupportedUICultures = supportedCultures
            };

            // You can change which providers are configured to determine the culture for requests, or even add a custom
            // provider with your own logic. The providers will be asked in order to provide a culture for each request,
            // and the first to provide a non-null result that is in the configured supported cultures list will be used.
            // By default, the following built-in providers are configured:
            // - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
            // - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
            // - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
            //requestLocalizationOptions.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
            //{
            //  // My custom request culture logic
            //  return new ProviderCultureResult("en");
            //}));
            return requestLocalizationOptions;
        }
    }
}
