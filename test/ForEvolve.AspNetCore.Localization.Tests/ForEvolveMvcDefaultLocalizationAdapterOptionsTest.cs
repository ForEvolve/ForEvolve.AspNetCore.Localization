using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ForEvolve.AspNetCore.Localization
{
    public class ForEvolveMvcDefaultLocalizationAdapterOptionsTest
    {
        public class Ctor
        {
            [Fact]
            public void Should_assign_DefaultSupportedAttributes_to_SupportedAttributes()
            {
                // Act
                var sut = new ForEvolveMvcDefaultLocalizationAdapterOptions();

                // Assert
                Assert.Equal(sut.DefaultSupportedAttributes, sut.SupportedAttributes);
            }

            [Fact]
            public void Should_assign_specified_supportedAttributes_to_SupportedAttributes()
            {
                // Arrange
                var supportedAttributes = new string[] {
                    "SomeAttribute",
                    "SomeOtherAttribute"
                };

                // Act
                var sut = new ForEvolveMvcDefaultLocalizationAdapterOptions(supportedAttributes);

                // Assert
                Assert.Equal(supportedAttributes, sut.SupportedAttributes);
            }

            [Fact]
            public void Should_throw_ArgumentNullException_when_specified_supportedAttributes_are_null()
            {
                // Act & Assert
                var ex = Assert.Throws<ArgumentNullException>(() => new ForEvolveMvcDefaultLocalizationAdapterOptions(null));
                Assert.Equal("supportedAttributes", ex.ParamName);
            }
        }
    }
}
