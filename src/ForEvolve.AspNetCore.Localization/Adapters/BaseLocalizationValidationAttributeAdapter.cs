using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForEvolve.AspNetCore.Localization.Adapters
{
    public abstract class BaseLocalizationValidationAttributeAdapter<TValidationAttribute> : ILocalizationValidationAttributeAdapter
        where TValidationAttribute : ValidationAttribute
    {
        public IList<string> SupportedAttributes { get; } = new List<string>(new[] { typeof(TValidationAttribute).Name });

        public virtual bool CanHandle(ValidationAttribute attribute)
        {
            return attribute is TValidationAttribute;
        }

        public virtual string GetErrorMessageResourceName(ValidationAttribute attribute)
        {
            return InternalGetErrorMessageResourceName(attribute as TValidationAttribute);
        }

        protected abstract string InternalGetErrorMessageResourceName(TValidationAttribute attr);
    }
}
