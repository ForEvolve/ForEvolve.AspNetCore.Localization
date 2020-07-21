using ForEvolve.AspNetCore.Localization;
using ForEvolve.AspNetCore.Localization.Adapters;
using ForEvolve.AspNetCore.Localization.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ForEvolveLocalizationStartupExtensions
    {
        /// <summary>
        /// Adds services required for application localization.
        /// The Asp.Net Core AddLocalization() method will be called.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the services to.</param>
        /// <returns>The Microsoft.Extensions.DependencyInjection.IServiceCollection so that additional calls can be chained.</returns>
        [Obsolete(@"You can delete the call to this extension method. 
Use the IMvcBuilder.AddForEvolveMvcLocalization() extension method instead.

This method will be removed in a future major release.", error: false)]
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
The ForEvolveLocalizationOptions class has also been removed.

This method will be removed in a future major release.", error: true)]
        public static IServiceCollection AddForEvolveLocalization(this IServiceCollection services, Action<object> setupAction)
        {
            return services;
        }

        [Obsolete("Call IMvcBuilder.AddForEvolveLocalization() instead; this method will be removed in a future major release.", error: false)]
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
            return AddForEvolveLocalization(
                mvcBuilder,
                enableViewLocalization: true,
                enableDataAnnotationsLocalization: true
            );
        }

        /// <summary>
        /// Adds services required for application localization.
        /// Calls the IServiceCollection.AddLocalization() method.
        /// Registers an IMetadataDetailsProvider that handles validation attributes to Microsoft.AspNetCore.Mvc.MvcOptions.
        /// </summary>
        /// <param name="mvcBuilder">The Microsoft.Extensions.DependencyInjection.IMvcBuilder.</param>
        /// <param name="enableDataAnnotationsLocalization"><c>true</c> to call <see cref="MvcLocalizationMvcBuilderExtensions.AddViewLocalization"/> on <paramref name="mvcBuilder"/>, otherwise false.</param>
        /// <param name="enableViewLocalization"><c>true</c> to call <see cref="MvcDataAnnotationsMvcBuilderExtensions.AddDataAnnotationsLocalization"/> on <paramref name="mvcBuilder"/>, otherwise false.</param>
        /// <returns>The <see cref="IMvcBuilder"/> so that additional calls can be chained.</returns>
        public static IMvcBuilder AddForEvolveLocalization(this IMvcBuilder mvcBuilder, bool enableViewLocalization, bool enableDataAnnotationsLocalization)
        {
            // New registration
            var services = mvcBuilder.Services;
            services
                // Configure ForEvolve.AspNetCore.Localization
                .AddSingleton<ISupportedCulturesCollection, SupportedCulturesCollection>()
                .AddSingleton<ILocalizationValidationMetadataProvider, ForEvolveLocalizationValidationMetadataProvider<DataAnnotationSharedResource>>()
                .AddSingleton<ILocalizationValidationAttributeAdapter, StringLengthLocalizationValidationAttributeAdapter>()
                .AddSingleton<ILocalizationValidationAttributeAdapter, DefaultLocalizationValidationAttributeAdapter>()

                // Add custom options initializers to configures Asp.Net Core
                .AddSingleton<IConfigureOptions<RequestLocalizationOptions>, RequestLocalizationOptionsInitializer>()
                .AddSingleton<IConfigureOptions<LocalizationOptions>, LocalizationOptionsInitializer>()
                .AddSingleton<IConfigureOptions<MvcOptions>, MvcOptionsInitializer>()

                // Adds services required for application localization.
                .AddLocalization()
            ;

            if (enableViewLocalization) { mvcBuilder.AddViewLocalization(); }
            if (enableDataAnnotationsLocalization) { mvcBuilder.AddDataAnnotationsLocalization(); }

            return mvcBuilder;
        }

        /// <summary>
        /// Adds the Microsoft.AspNetCore.Localization.RequestLocalizationMiddleware to automatically set culture information for requests based on information provided by the client.
        /// This calls UseRequestLocalization().
        /// </summary>
        /// <param name="app">The Microsoft.AspNetCore.Builder.IApplicationBuilder.</param>
        /// <returns>The Microsoft.AspNetCore.Builder.IApplicationBuilder so that additional calls can be chained..</returns>
        [Obsolete("Call the IApplicationBuilder.UseRequestLocalization() extension method instead. This will be deleted in a next major version.", error: false)]
        public static IApplicationBuilder UseForEvolveRequestLocalization(this IApplicationBuilder app)
        {
            return app.UseRequestLocalization();
        }
    }
}
