using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForEvolve.AspNetCore.Localization
{
    public interface ILocalizationValidationAttributeAdapter
    {
        string GetErrorMessageResourceName(ValidationAttribute attribute);
        bool CanHandle(ValidationAttribute attribute);
    }
}
