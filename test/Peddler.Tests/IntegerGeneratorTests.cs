using System;
using Xunit;

namespace Peddler {

    public class IntegerGeneratorTests {

        private const int NUMBER_OF_ATTEMPTS = 100;

        [Fact]
        public void Constructor_WithLow_LowCannotBeInt32MaxValue() {
            Assert.Throws<ArgumentException>(
                () => new IntegerGenerator(Int32.MaxValue)
            );
        }

        [Theory]
        [InlineData(Int32.MinValue + 1, Int32.MinValue)]
        [InlineData(Int32.MaxValue, Int32.MinValue)]
        [InlineData(Int32.MinValue, Int32.MinValue)]
        [InlineData(0, 0)]
        [InlineData(Int32.MaxValue, Int32.MaxValue)]
        public void Constructor_WithLowAndHigh_LowMustBeLessThanHigh(int low, int high) {
            Assert.Throws<ArgumentException>(
                () => new IntegerGenerator(low, high)
            );
        }

        [Fact]
        public void Next_WithDefaults_RangeIsZeroToInt32MaxValue() {
            var generator = new IntegerGenerator();

            for (var attempt = 0; attempt < NUMBER_OF_ATTEMPTS; attempt++) {
                var value = generator.Next();

                Assert.True(value >= 0);
                Assert.True(value < Int32.MaxValue);
            }
        }

        [Theory]
        [InlineData(Int32.MinValue)]
        [InlineData(0)]
        [InlineData(Int32.MaxValue - 1)]
        public void Next_WithLowDefined_RangeIsLowToInt32MaxValue(int low) {
            var generator = new IntegerGenerator(low);

            for (var attempt = 0; attempt < NUMBER_OF_ATTEMPTS; attempt++) {
                var value = generator.Next();

                Assert.True(value >= low);
                Assert.True(value < Int32.MaxValue);
            }
        }

        [Theory]
        [InlineData(Int32.MinValue, Int32.MaxValue)]
        [InlineData(Int32.MinValue, 0)]
        [InlineData(0, Int32.MaxValue)]
        public void Next_WithLowAndHighDefinted_RangeIsBetweenLowAndHigh(int low, int high) {
            var generator = new IntegerGenerator(low, high);

            for (var attempt = 0; attempt < NUMBER_OF_ATTEMPTS; attempt++) {
                var value = generator.Next();

                Assert.True(value >= low);
                Assert.True(value < high);
            }
        }

        [Theory]
        [InlineData(Int32.MinValue, Int32.MaxValue)]
        [InlineData(0, 2)]
        public void NextDistinct_NeverGetSameValue(int low, int high) {
            var generator = new IntegerGenerator(low, high);
            var previousValue = generator.Next();

            for (var attempt = 0; attempt < NUMBER_OF_ATTEMPTS; attempt++) {
                var nextValue = generator.NextDistinct(previousValue);
                Assert.NotEqual(previousValue, nextValue);
                previousValue = nextValue;
            }
        }

        [Fact]
        public void NextDistinct_ThrowOnConstantGenerator() {
            // With these arguments, IntegerGenerator can only generate '0'
            var generator = new IntegerGenerator(0, 1);
            var value = generator.Next();

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextDistinct(value)
            );
        }


    }


}
