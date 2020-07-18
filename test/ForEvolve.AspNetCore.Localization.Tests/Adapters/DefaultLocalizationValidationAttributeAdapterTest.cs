using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Xunit;

namespace ForEvolve.AspNetCore.Localization.Adapters
{
    public class DefaultLocalizationValidationAttributeAdapterTest
    {
        private DefaultLocalizationValidationAttributeAdapter AdapterUnderTest { get; }

        public DefaultLocalizationValidationAttributeAdapterTest()
        {
            AdapterUnderTest = new DefaultLocalizationValidationAttributeAdapter();
        }

        public class CanHandle : DefaultLocalizationValidationAttributeAdapterTest
        {
            [Fact]
            public void Should_return_true_when_Type_is_supported()
            {
                // Arrange
                var requiredAttribute = new RequiredAttribute();

                // Act
                var result = AdapterUnderTest.CanHandle(requiredAttribute);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Should_return_false_when_Type_is_not_supported()
            {
                // Arrange
                var someValidationAttribute = new UnsupportedValidationAttribute();

                // Act
                var result = AdapterUnderTest.CanHandle(someValidationAttribute);

                // Assert
                Assert.False(result);
            }
        }

        public class GetErrorMessageResourceName : DefaultLocalizationValidationAttributeAdapterTest
        {
            [Fact]
            public void Should_return_TypeName_suffixed_by_ErrorMessage()
            {
                // Arrange
                var someValidationAttribute = new UnsupportedValidationAttribute();
                var expectedResourceName = "UnsupportedValidationAttribute_ErrorMessage";

                // Act
                var result = AdapterUnderTest.GetErrorMessageResourceName(someValidationAttribute);

                // Assert
                Assert.Equal(expectedResourceName, result);
            }
        }

        private class UnsupportedValidationAttribute : ValidationAttribute { }
    }
}
