using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForEvolve.AspNetCore.Localization.Adapters
{
    public class DefaultLocalizationValidationAttributeAdapter : ILocalizationValidationAttributeAdapter
    {
        private readonly string[] _supportedAttributes = new string[]
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

        public bool CanHandle(ValidationAttribute attribute)
        {
            return _supportedAttributes.Contains(attribute.GetType().Name);
        }

        public string GetErrorMessageResourceName(ValidationAttribute attribute)
        {
            return $"{attribute.GetType().Name}_ErrorMessage";
        }
    }
}
