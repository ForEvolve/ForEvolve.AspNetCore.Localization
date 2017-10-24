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
        private readonly string[] _internalSupportedAttributes = new string[]
        {
            "CompareAttribute",
            "EmailAddressAttribute",
            "RequiredAttribute",
            "CreditCardAttribute",
            "FileExtensionsAttribute",
            "MaxLengthAttribute",
            "MinLengthAttribute",
            "PhoneAttribute",
            "RangeAttribute",
            "RegexAttribute",
            "UrlAttribute"
        };

        public IReadOnlyCollection<string> SupportedAttributes => new ReadOnlyCollection<string>(_internalSupportedAttributes);

        public bool CanHandle(ValidationAttribute attribute)
        {
            return _internalSupportedAttributes.Contains(attribute.GetType().Name);
        }

        public string GetErrorMessageResourceName(ValidationAttribute attribute)
        {
            return $"{attribute.GetType().Name}_ErrorMessage";
        }
    }
}
