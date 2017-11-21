using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ForEvolve.AspNetCore.Localization
{
    public class SupportedCulturesRepositoryTest
    {
        private readonly SupportedCulturesRepository _repositoryUnderTest;
        private readonly List<CultureInfo> _cultures;

        private const string FirstCultureName = "en";
        private const string SecondCultureName = "fr";

        public SupportedCulturesRepositoryTest()
        {
            _cultures = new List<CultureInfo>
            {
                new CultureInfo(FirstCultureName),
                new CultureInfo(SecondCultureName)
            };
            _repositoryUnderTest = new SupportedCulturesRepository(_cultures);
        }

        public class Ctor
        {
            [Fact]
            public void Should_guard_against_null()
            {
                // Arrange
                IEnumerable<CultureInfo> nullCultures = null;

                // Act & Assert
                Assert.Throws<ArgumentNullException>("cultures", () => new SupportedCulturesRepository(nullCultures));
            }
        }

        public class FindDefaultAsync : SupportedCulturesRepositoryTest
        {
            [Fact]
            public async Task Should_return_the_first_culture()
            {
                // Arrange
                var expectedCulture = _cultures.First();

                // Act
                var result = await _repositoryUnderTest.FindDefaultAsync();

                // Assert
                Assert.Same(expectedCulture, result);
            }
        }

        public class ReadAllAsync : SupportedCulturesRepositoryTest
        {
            [Fact]
            public async Task Should_return_all_cultures()
            {
                // Act
                var result = await _repositoryUnderTest.ReadAllAsync();

                // Assert
                Assert.Equal(_cultures, result);
            }
        }

        public class ReadOneAsync : SupportedCulturesRepositoryTest
        {
            [Fact]
            public async Task Should_return_all_cultures()
            {
                // Arrange
                var expectedCulture = _cultures[1];

                // Act
                var result = await _repositoryUnderTest.ReadOneAsync(SecondCultureName);

                // Assert
                Assert.Equal(expectedCulture, result);
            }
        }
    }
}
