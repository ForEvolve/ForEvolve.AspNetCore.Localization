using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ForEvolve.AspNetCore.Localization.Adapters
{
    public class BaseLocalizationValidationAttributeAdapterTest
    {
        private TestLocalizationValidationAttributeAdapter AdapterUnderTest { get; }

        public BaseLocalizationValidationAttributeAdapterTest()
        {
            AdapterUnderTest = new TestLocalizationValidationAttributeAdapter();
        }

        public class CanHandle : BaseLocalizationValidationAttributeAdapterTest
        {
            [Fact]
            public void Should_return_true_when_attribute_is_of_type_TValidationAttribute()
            {
                // Arrange
                var attribute = new TestValidationAttribute();

                // Act
                var result = AdapterUnderTest.CanHandle(attribute);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Should_return_false_when_attribute_is_not_of_type_TValidationAttribute()
            {
                // Arrange
                var attribute = new AnotherTestValidationAttribute();

                // Act
                var result = AdapterUnderTest.CanHandle(attribute);

                // Assert
                Assert.False(result);
            }
        }
        public class GetErrorMessageResourceName : BaseLocalizationValidationAttributeAdapterTest
        {
            [Fact]
            public void Should_call_InternalGetErrorMessageResourceName_of_sub_class()
            {
                // Arrange
                var attribute = new TestValidationAttribute();
                var expectedAttribute = attribute;
                var expectedErrorMessageResourceName = AdapterUnderTest.ErrorMessageResourceName;

                // Act
                var result = AdapterUnderTest.GetErrorMessageResourceName(attribute);

                // Assert
                Assert.True(AdapterUnderTest.WasCalledOnce());
                Assert.Same(expectedAttribute, AdapterUnderTest.CallResults.First());
                Assert.Equal(expectedErrorMessageResourceName, result);
            }
        }

        private class TestValidationAttribute : ValidationAttribute
        {

        }

        private class AnotherTestValidationAttribute : ValidationAttribute
        {

        }

        private class TestLocalizationValidationAttributeAdapter : BaseLocalizationValidationAttributeAdapter<TestValidationAttribute>
        {
            public const string DefaultErrorMessageResourceName = "TestLocalizationValidationAttributeAdapter-ErrorMessageResourceName";

            public string ErrorMessageResourceName => DefaultErrorMessageResourceName;

            public int CallCount => CallResults.Count;

            public bool WasCalled()
            {
                return CallCount > 0;
            }

            public bool WasCalledOnce()
            {
                return CallCount == 1;
            }

            public List<TestValidationAttribute> CallResults { get; }

            public TestLocalizationValidationAttributeAdapter()
            {
                CallResults = new List<TestValidationAttribute>();
            }

            protected override string InternalGetErrorMessageResourceName(TestValidationAttribute attr)
            {
                CallResults.Add(attr);
                return DefaultErrorMessageResourceName;
            }
        }
    }
}
