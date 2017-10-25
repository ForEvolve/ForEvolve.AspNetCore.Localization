using ForEvolve.AspNetCore.Localization.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Moq;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ForEvolve.AspNetCore.Localization
{
    public class ForEvolveLocalizationValidationMetadataProviderTest
    {
        public class Ctor
        {
            [Fact]
            public void Should_set_adapters()
            {
                // Arrange
                var expectedAdapter = new MyAdapter();
                var adapters = new ILocalizationValidationAttributeAdapter[]
                {
                    expectedAdapter
                };

                // Act
                var provider = new ForEvolveLocalizationValidationMetadataProvider<DataAnnotationTestResource>(adapters);

                // Assert
                Assert.Collection(provider.Adapters,
                    actualAdapter => Assert.Same(expectedAdapter, actualAdapter)
                );
            }

            [Fact]
            public void Should_set_adapters_to_empty_when_null()
            {
                // Act
                var provider = new ForEvolveLocalizationValidationMetadataProvider<DataAnnotationTestResource>();

                // Assert
                Assert.Empty(provider.Adapters);
            }

            private class MyAdapter : ILocalizationValidationAttributeAdapter
            {
                public bool CanHandle(ValidationAttribute attribute)
                {
                    throw new NotSupportedException();
                }

                public string GetErrorMessageResourceName(ValidationAttribute attribute)
                {
                    throw new NotSupportedException();
                }
            }
        }

        public class CreateValidationMetadata
        {
            private ForEvolveLocalizationValidationMetadataProvider<DataAnnotationTestResource> ProvierUnderTest;
            private Mock<ILocalizationValidationAttributeAdapter> MyMockAdapter { get; }
            public CreateValidationMetadata()
            {
                MyMockAdapter = new Mock<ILocalizationValidationAttributeAdapter>();
                ProvierUnderTest = new ForEvolveLocalizationValidationMetadataProvider<DataAnnotationTestResource>(MyMockAdapter.Object);
            }

            [Fact]
            public void Should_update_ErrorMessageResource_of_ValidationAttribute()
            {
                // Arrange
                var key = new ModelMetadataIdentity();
                var myAttribute = new MyValidationAttribute();
                var attributes = new ModelAttributes(new object[] { myAttribute });
                var context = new ValidationMetadataProviderContext(key, attributes);
                var expectedErrorMessageResourceName = "SomeResourceName";
                MyMockAdapter
                    .Setup(x => x.CanHandle(myAttribute))
                    .Returns(true);
                MyMockAdapter
                    .Setup(x => x.GetErrorMessageResourceName(myAttribute))
                    .Returns(expectedErrorMessageResourceName);

                // Act
                ProvierUnderTest.CreateValidationMetadata(context);

                // Assert
                Assert.Equal(
                    expectedErrorMessageResourceName, 
                    myAttribute.ErrorMessageResourceName
                );
                Assert.Equal(
                    ProvierUnderTest.ErrorMessageResourceType, 
                    myAttribute.ErrorMessageResourceType
                );
            }

            [Fact]
            public void Should_not_update_ErrorMessageResource_of_ValidationAttribute_with_custom_ErrorMessage()
            {
                // Arrange
                var key = new ModelMetadataIdentity();
                var myAttribute = new MyValidationAttribute
                {
                    ErrorMessage = "Some message"
                };
                var attributes = new ModelAttributes(new object[] { myAttribute });
                var context = new ValidationMetadataProviderContext(key, attributes);
                MyMockAdapter
                    .Setup(x => x.CanHandle(myAttribute))
                    .Returns(true);
                MyMockAdapter
                    .Setup(x => x.GetErrorMessageResourceName(myAttribute))
                    .Returns("Whatever")
                    .Verifiable();

                // Act
                ProvierUnderTest.CreateValidationMetadata(context);

                // Assert
                Assert.Null(myAttribute.ErrorMessageResourceName);
                Assert.Null(myAttribute.ErrorMessageResourceType);
                Assert.Equal("Some message", myAttribute.ErrorMessage);
                MyMockAdapter.Verify(
                    x => x.GetErrorMessageResourceName(myAttribute), 
                    Times.Never
                );
            }

            [Fact]
            public void Should_update_ErrorMessageResource_only_when_needed()
            {
                // Arrange
                var key = new ModelMetadataIdentity();
                var myAttributeNotToUpdate = new MyValidationAttribute
                {
                    ErrorMessage = "Some message"
                };
                var myAttributeToUpdate = new MyValidationAttribute();
                var someOtherNonValidationAttribute = new MyNonValidationAttribute();

                var attributes = new ModelAttributes(new object[] {
                    myAttributeNotToUpdate,
                    myAttributeToUpdate,
                    someOtherNonValidationAttribute
                });
                var context = new ValidationMetadataProviderContext(key, attributes);
                var expectedErrorMessageResourceName = "SomeResourceName";

                MyMockAdapter
                    .Setup(x => x.CanHandle(myAttributeNotToUpdate))
                    .Returns(true)
                    .Verifiable();
                MyMockAdapter
                    .Setup(x => x.GetErrorMessageResourceName(myAttributeNotToUpdate))
                    .Returns("Whatever")
                    .Verifiable();

                MyMockAdapter
                    .Setup(x => x.CanHandle(myAttributeToUpdate))
                    .Returns(true);
                MyMockAdapter
                    .Setup(x => x.GetErrorMessageResourceName(myAttributeToUpdate))
                    .Returns(expectedErrorMessageResourceName)
                    .Verifiable();


                // Act
                ProvierUnderTest.CreateValidationMetadata(context);

                // Assert
                Assert.Null(myAttributeNotToUpdate.ErrorMessageResourceName);
                Assert.Null(myAttributeNotToUpdate.ErrorMessageResourceType);
                Assert.Equal("Some message", myAttributeNotToUpdate.ErrorMessage);
                MyMockAdapter.Verify(
                    x => x.CanHandle(myAttributeNotToUpdate),
                    Times.Never
                );
                MyMockAdapter.Verify(
                    x => x.GetErrorMessageResourceName(myAttributeNotToUpdate),
                    Times.Never
                );

                Assert.Equal(
                    expectedErrorMessageResourceName, 
                    myAttributeToUpdate.ErrorMessageResourceName
                );
                Assert.Equal(
                    ProvierUnderTest.ErrorMessageResourceType,
                    myAttributeToUpdate.ErrorMessageResourceType
                );
                MyMockAdapter.Verify(
                    x => x.CanHandle(myAttributeToUpdate),
                    Times.Once
                );
                MyMockAdapter.Verify(
                    x => x.GetErrorMessageResourceName(myAttributeToUpdate),
                    Times.Once
                );

            }

            private class MyValidationAttribute : ValidationAttribute
            {

            }

            private class MyNonValidationAttribute : Attribute
            {

            }
        }
    }
}
