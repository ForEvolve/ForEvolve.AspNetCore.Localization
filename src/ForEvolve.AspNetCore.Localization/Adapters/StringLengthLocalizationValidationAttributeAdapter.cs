using System.ComponentModel.DataAnnotations;

namespace ForEvolve.AspNetCore.Localization.Adapters
{
    public class StringLengthLocalizationValidationAttributeAdapter : BaseLocalizationValidationAttributeAdapter<StringLengthAttribute>
    {
        protected override string InternalGetErrorMessageResourceName(StringLengthAttribute attr)
        {
            if (attr.MinimumLength > 0)
            {
                return "StringLengthAttribute_ErrorMessageIncludingMinimum";
            }
            else
            {
                return "StringLengthAttribute_ErrorMessage";
            }
        }
    }
}
