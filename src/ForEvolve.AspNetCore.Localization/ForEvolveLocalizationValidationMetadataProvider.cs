using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ForEvolve.AspNetCore.Localization
{
    public class ForEvolveLocalizationValidationMetadataProvider<TErrorMessageResource> : ILocalizationValidationMetadataProvider
    {
        private readonly List<ILocalizationValidationAttributeAdapter> _adapters;
        private readonly Type _errorMessageResourceType;

        public ForEvolveLocalizationValidationMetadataProvider(params ILocalizationValidationAttributeAdapter[] adapters)
        {
            _adapters = new List<ILocalizationValidationAttributeAdapter>(adapters ?? Enumerable.Empty<ILocalizationValidationAttributeAdapter>());
            _errorMessageResourceType = typeof(TErrorMessageResource);
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
                        foreach (var adapter in _adapters)
                        {
                            if (adapter.CanHandle(validationAttr))
                            {
                                validationAttr.ErrorMessageResourceType = _errorMessageResourceType;
                                validationAttr.ErrorMessageResourceName = adapter.GetErrorMessageResourceName(validationAttr);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public IList<ILocalizationValidationAttributeAdapter> Adapters => _adapters;
    }
}
