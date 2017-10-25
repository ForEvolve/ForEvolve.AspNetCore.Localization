using System.ComponentModel.DataAnnotations;

namespace ForEvolve.AspNetCore.Localization.Adapters
{
    public class StringLengthLocalizationValidationAttributeAdapter : BaseLocalizationValidationAttributeAdapter<StringLengthAttribute>
    {
        public const string DefaultResourceName = "StringLengthAttribute_ErrorMessage";
        public const string ResourceNameIncludingMinimum = "StringLengthAttribute_ErrorMessageIncludingMinimum";

        protected override string InternalGetErrorMessageResourceName(StringLengthAttribute attr)
        {
            if (attr.MinimumLength > 0)
            {
                return ResourceNameIncludingMinimum;
            }
            else
            {
                return DefaultResourceName;
            }
        }
    }
}
