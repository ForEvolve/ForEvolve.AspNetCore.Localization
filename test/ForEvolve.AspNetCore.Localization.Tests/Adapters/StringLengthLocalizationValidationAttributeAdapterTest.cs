using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xunit;

namespace ForEvolve.AspNetCore.Localization.Adapters
{
    public class StringLengthLocalizationValidationAttributeAdapterTest
    {
        private StringLengthLocalizationValidationAttributeAdapter AdapterUnderTest = new StringLengthLocalizationValidationAttributeAdapter();

        public class InternalGetErrorMessageResourceName : StringLengthLocalizationValidationAttributeAdapterTest
        {
            [Fact]
            public void Should_return_DefaultResourceName()
            {
                // Arrange
                var expectedResourceName = StringLengthLocalizationValidationAttributeAdapter.DefaultResourceName;
                var attribute = new StringLengthAttribute(50);

                // Act
                var result = AdapterUnderTest.GetErrorMessageResourceName(attribute);

                // Assert
                Assert.Equal(expectedResourceName, result);
            }

            [Fact]
            public void Should_return_ResourceNameIncludingMinimum_when_MinimumLength_is_greater_than_zero()
            {
                // Arrange
                var expectedResourceName = StringLengthLocalizationValidationAttributeAdapter.ResourceNameIncludingMinimum;
                var attribute = new StringLengthAttribute(50)
                {
                    MinimumLength = 1
                };

                // Act
                var result = AdapterUnderTest.GetErrorMessageResourceName(attribute);

                // Assert
                Assert.Equal(expectedResourceName, result);
            }
        }
    }
}
