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
        private ForEvolveMvcDefaultLocalizationAdapterOptions Options { get; }

        public DefaultLocalizationValidationAttributeAdapterTest()
        {
            Options = new ForEvolveMvcDefaultLocalizationAdapterOptions();
            AdapterUnderTest = new DefaultLocalizationValidationAttributeAdapter(Options);
        }

        public class Ctor
        {
            [Fact]
            public void Should_guard_against_null_options()
            {
                // Arrange
                ForEvolveMvcDefaultLocalizationAdapterOptions options = null;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new DefaultLocalizationValidationAttributeAdapter(options));
            }

            [Fact]
            public void Should_set_SupportedAttributes()
            {
                // Arrange
                var supportedAttributes = new string[] { "asdf", "sdfg" };
                var options = new ForEvolveMvcDefaultLocalizationAdapterOptions(supportedAttributes);

                // Act
                var adapter = new DefaultLocalizationValidationAttributeAdapter(options);

                // Assert
                Assert.Equal(adapter.SupportedAttributes, supportedAttributes);
            }
        }

        public class CanHandle : DefaultLocalizationValidationAttributeAdapterTest
        {
            [Fact]
            public void Should_return_true_when_Type_is_supported()
            {
                // Arrange
                var requiredAttribute = CreateAttribute<RequiredAttribute>(shouldBeSupported: true);

                // Act
                var result = AdapterUnderTest.CanHandle(requiredAttribute);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Should_return_false_when_Type_is_not_supported()
            {
                // Arrange
                var someValidationAttribute = CreateAttribute<SomeValidationAttribute>(shouldBeSupported: false);

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
                var someValidationAttribute = CreateAttribute<SomeValidationAttribute>(shouldBeSupported: false);
                var expectedResourceName = "SomeValidationAttribute_ErrorMessage";

                // Act
                var result = AdapterUnderTest.GetErrorMessageResourceName(someValidationAttribute);

                // Assert
                Assert.Equal(expectedResourceName, result);
            }
        }

        private TAttribute CreateAttribute<TAttribute>(bool shouldBeSupported)
            where TAttribute : ValidationAttribute, new()
        {
            var requiredAttribute = new TAttribute();
            var typeName = requiredAttribute.GetType().Name;
            if (shouldBeSupported)
            {
                var requiredAttributeTypeName = Options
                    .DefaultSupportedAttributes
                    .Single(x => x == typeName);
            }
            else
            {
                Assert.Throws<InvalidOperationException>(() => Options
                    .DefaultSupportedAttributes
                    .Single(x => x == typeName));
            }
            return requiredAttribute;
        }

        private class SomeValidationAttribute : ValidationAttribute
        {

        }
    }
}
