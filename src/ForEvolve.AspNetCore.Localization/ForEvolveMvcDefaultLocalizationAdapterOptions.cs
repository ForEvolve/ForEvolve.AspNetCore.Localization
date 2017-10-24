using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ForEvolve.AspNetCore.Localization
{
    public class ForEvolveMvcDefaultLocalizationAdapterOptions
    {
        private static string[] InternalDefaultSupportedAttributes = new string[]
        {
            nameof(CompareAttribute),
            nameof(EmailAddressAttribute),
            nameof(RequiredAttribute),
            nameof(CreditCardAttribute),
            nameof(FileExtensionsAttribute),
            nameof(MaxLengthAttribute),
            nameof(MinLengthAttribute),
            nameof(PhoneAttribute),
            nameof(RangeAttribute),
            nameof(RegularExpressionAttribute),
            nameof(UrlAttribute)
        };

        public IList<string> SupportedAttributes { get; }

        public ForEvolveMvcDefaultLocalizationAdapterOptions()
            : this(InternalDefaultSupportedAttributes)
        {
        }

        public ForEvolveMvcDefaultLocalizationAdapterOptions(IEnumerable<string> supportedAttributes)
        {
            if (supportedAttributes == null) { throw new ArgumentNullException(nameof(supportedAttributes)); }
            SupportedAttributes = new List<string>(supportedAttributes);
        }

        public IReadOnlyCollection<string> DefaultSupportedAttributes => new ReadOnlyCollection<string>(InternalDefaultSupportedAttributes);
    }
}
