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
        public IList<string> SupportedAttributes { get; }

        public DefaultLocalizationValidationAttributeAdapter(ForEvolveMvcDefaultLocalizationAdapterOptions options)
        {
            if (options == null) { throw new ArgumentNullException(nameof(options)); }
            SupportedAttributes = options.SupportedAttributes;
        }

        public bool CanHandle(ValidationAttribute attribute)
        {
            return SupportedAttributes.Contains(attribute.GetType().Name);
        }

        public string GetErrorMessageResourceName(ValidationAttribute attribute)
        {
            return $"{attribute.GetType().Name}_ErrorMessage";
        }
    }
}
