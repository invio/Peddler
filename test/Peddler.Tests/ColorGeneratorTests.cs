using System;
using System.Collections.Generic;
using System.Drawing;
using Invio.Xunit;
using Xunit;

namespace Peddler {

    [UnitTest]
    public sealed class ColorGeneratorTests {

        private const int numberOfAttempts = 100;

        [Fact]
        public void EqualityComparer_AlphaDisabled_IgnoresAlphaChannel() {

            // Arrange

            var generator = new ColorGenerator(useAlpha: false);

            var light = Color.FromArgb(50, generator.Next());
            var dark = Color.FromArgb(200, light);

            // Act & Assert

            Assert.Equal(light, light, generator.EqualityComparer);
            Assert.Equal(dark, dark, generator.EqualityComparer);
            Assert.Equal(light, dark, generator.EqualityComparer);
        }

        [Fact]
        public void EqualityComparer_AlphaEnabled_ConsidersAlphaChannel() {

            // Arrange

            var generator = new ColorGenerator(useAlpha: true);

            var light = Color.FromArgb(50, generator.Next());
            var dark = Color.FromArgb(200, light);

            // Act & Assert

            Assert.Equal(light, light, generator.EqualityComparer);
            Assert.Equal(dark, dark, generator.EqualityComparer);
            Assert.NotEqual(light, dark, generator.EqualityComparer);
        }

        [Fact]
        public void Next_AlphaDisabled_ShouldAlwaysGenerateOpaqueColors() {

            // Arrange

            var generator = new ColorGenerator(useAlpha: false);

            // Act

            var values = new HashSet<Color>();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                values.Add(generator.Next());
            }

            // Assert

            Assert.All(values, value => Assert.Equal(255, value.A));
        }

        [Fact]
        public void Next_AlphaEnabled_ShouldGenerateSomeNonOpaqueColors() {

            // Arrange

            var generator = new ColorGenerator(useAlpha: true);

            // Act

            var colors = new HashSet<Color>();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                colors.Add(generator.Next());
            }

            // Assert

            Assert.Contains(colors, color => color.A != 255);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void NextDistinct_AlwaysGeneratesDistinctValues(bool useAlpha) {

            // Arrange

            var generator = new ColorGenerator(useAlpha);
            var original = generator.Next();

            // Act

            var colors = new HashSet<Color>();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                colors.Add(generator.NextDistinct(original));
            }

            // Assert

            Assert.DoesNotContain(original, colors, generator.EqualityComparer);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(255)]
        public void NextDistinct_AlphaDisabled_AlwaysGeneratesOpaqueColors(int initialAlpha) {

            // Arrange

            var generator = new ColorGenerator(useAlpha: false);
            var original = Color.FromArgb(initialAlpha, generator.Next());

            // Act

            var colors = new HashSet<Color>();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                colors.Add(generator.NextDistinct(original));
            }

            // Assert

            Assert.DoesNotContain(original, colors, generator.EqualityComparer);
            Assert.All(colors, color => Assert.Equal(255, color.A));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(255)]
        public void NextDistinct_AlphaEnabled_ShouldGenerateSomeNonOpaqueColors(int initialAlpha) {

            // Arrange

            var generator = new ColorGenerator(useAlpha: true);
            var original = Color.FromArgb(initialAlpha, generator.Next());

            // Act

            var colors = new HashSet<Color>();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                colors.Add(generator.NextDistinct(original));
            }

            // Assert

            Assert.DoesNotContain(original, colors, generator.EqualityComparer);
            Assert.Contains(colors, color => color.A != 255);
        }

    }

}