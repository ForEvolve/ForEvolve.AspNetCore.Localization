using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ForEvolve.AspNetCore.Localization
{
    public class ForEvolveLocalizationValidationMetadataProvider<TErrorMessageResource> : ILocalizationValidationMetadataProvider
    {
        public Type ErrorMessageResourceType { get; }
        public IList<ILocalizationValidationAttributeAdapter> Adapters { get; }

        public ForEvolveLocalizationValidationMetadataProvider(params ILocalizationValidationAttributeAdapter[] adapters)
        {
            Adapters = new List<ILocalizationValidationAttributeAdapter>(adapters ?? Enumerable.Empty<ILocalizationValidationAttributeAdapter>());
            ErrorMessageResourceType = typeof(TErrorMessageResource);
        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            foreach (var attr in context.Attributes)
            {
                if (attr is ValidationAttribute validationAttr)
                {
                    // If there is no custom message, try to find an adapter to set
                    // the ErrorMessageResourceName property automatically.
                    if (string.IsNullOrWhiteSpace(validationAttr.ErrorMessage))
                    {
                        foreach (var adapter in Adapters)
                        {
                            if (adapter.CanHandle(validationAttr))
                            {
                                validationAttr.ErrorMessageResourceType = ErrorMessageResourceType;
                                validationAttr.ErrorMessageResourceName = adapter.GetErrorMessageResourceName(validationAttr);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
