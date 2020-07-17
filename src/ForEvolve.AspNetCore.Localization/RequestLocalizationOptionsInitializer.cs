using ForEvolve.AspNetCore.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;

namespace Microsoft.Extensions.DependencyInjection
{
    public class RequestLocalizationOptionsInitializer : IConfigureOptions<RequestLocalizationOptions>
    {
        private readonly ISupportedCulturesCollection _supportedCultures;
        private readonly CultureInfo _defaultCulture;
        public RequestLocalizationOptionsInitializer(ISupportedCulturesCollection supportedCultures)
        {
            _supportedCultures = supportedCultures ?? throw new ArgumentNullException(nameof(supportedCultures));
            _defaultCulture = _supportedCultures.First();
        }

        public void Configure(RequestLocalizationOptions options)
        {
            // State what the default culture for your application is. This will be used if no specific culture
            // can be determined for a given request.
            options.DefaultRequestCulture = new RequestCulture(_defaultCulture, _defaultCulture);

            // You must explicitly state which cultures your application supports.
            // These are the cultures the app supports for formatting numbers, dates, etc.
            options.SupportedCultures = _supportedCultures.ToList();

            // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
            options.SupportedUICultures = _supportedCultures.ToList();
        }
    }
}
