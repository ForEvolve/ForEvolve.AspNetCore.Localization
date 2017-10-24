using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.Collections.Generic;

namespace ForEvolve.AspNetCore.Localization
{
    public interface ILocalizationValidationMetadataProvider : IValidationMetadataProvider
    {
        IList<ILocalizationValidationAttributeAdapter> Adapters { get; }
    }
}
