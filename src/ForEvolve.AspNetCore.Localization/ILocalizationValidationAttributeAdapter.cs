using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForEvolve.AspNetCore.Localization
{
    public interface ILocalizationValidationAttributeAdapter
    {
        IList<string> SupportedAttributes { get; }
        string GetErrorMessageResourceName(ValidationAttribute attribute);
        bool CanHandle(ValidationAttribute attribute);
    }
}
