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

            Assert.Equal(generator.Low, 0);
            Assert.Equal(generator.High, Int32.MaxValue);

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

            Assert.Equal(generator.Low, low);
            Assert.Equal(generator.High, Int32.MaxValue);

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
        public void Next_WithLowAndHighDefined_RangeIsBetweenLowAndHigh(int low, int high) {
            var generator = new IntegerGenerator(low, high);

            Assert.Equal(generator.Low, low);
            Assert.Equal(generator.High, high);

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

        [Fact]
        public void NextDistinct_OtherGreaterThanRange() {
            var generator = new IntegerGenerator(0, 10);

            var other = 20;
            Assert.True(other > generator.High);

            for (var attempt = 0; attempt < NUMBER_OF_ATTEMPTS; attempt++) {
                var value = generator.NextDistinct(other);
                Assert.True(value >= generator.Low);
                Assert.True(value < generator.High);
            }
        }

        [Fact]
        public void NextDistinct_OtherLessThanRange() {
            var generator = new IntegerGenerator(0, 10);

            var other = -20;
            Assert.True(other < generator.Low);

            for (var attempt = 0; attempt < NUMBER_OF_ATTEMPTS; attempt++) {
                var value = generator.NextDistinct(other);
                Assert.True(value >= generator.Low);
                Assert.True(value < generator.High);
            }
        }

        [Theory]
        [InlineData(0, 10, 20)]
        [InlineData(0, 10, 10)]
        [InlineData(0, 10, 9)]
        [InlineData(0, Int32.MaxValue, Int32.MaxValue)]
        public void NextGreaterThan_ThrowOnMaxValueOrHigher(int low, int high, int other) {
            var generator = new IntegerGenerator(low, high);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextGreaterThan(other)
            );
        }

        [Theory]
        [InlineData(Int32.MinValue, Int32.MaxValue, Int32.MinValue)]
        [InlineData(0, 2, 0)]
        [InlineData(0, 10, -30)]
        public void NextGreaterThan(int low, int high, int other) {
            var generator = new IntegerGenerator(low, high);

            for (var attempt = 0; attempt < NUMBER_OF_ATTEMPTS; attempt++) {
                var value = generator.NextGreaterThan(other);

                Assert.True(value > other);
                Assert.True(value >= generator.Low);
                Assert.True(value < generator.High);
            }
        }

        [Theory]
        [InlineData(1, 2, 10)]
        [InlineData(0, 10, 10)]
        [InlineData(Int32.MinValue, Int32.MaxValue, Int32.MaxValue)]
        public void NextGreaterThanOrEqualTo_ThrowOnHigherThanMaxValue(int low, int high, int other) {
            var generator = new IntegerGenerator(low, high);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextGreaterThanOrEqualTo(other)
            );
        }

        [Theory]
        [InlineData(1, 2, 0)]
        [InlineData(0, 10, 9)]
        [InlineData(Int32.MinValue, Int32.MaxValue, Int32.MinValue)]
        [InlineData(0, 10, -30)]
        public void NextGreaterThanOrEqualTo(int low, int high, int other) {
            var generator = new IntegerGenerator(low, high);

            for (var attempt = 0; attempt < NUMBER_OF_ATTEMPTS; attempt++) {
                var value = generator.NextGreaterThanOrEqualTo(other);

                Assert.True(value >= other);
                Assert.True(value >= generator.Low);
                Assert.True(value < generator.High);
            }
        }

        [Theory]
        [InlineData(1, 2, 0)]
        [InlineData(0, 10, 0)]
        [InlineData(Int32.MinValue, Int32.MaxValue, Int32.MinValue)]
        public void NextLessThan_ThrowOnMinValueOrLower(int low, int high, int other) {
            var generator = new IntegerGenerator(low, high);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextLessThan(other)
            );
        }

        [Theory]
        [InlineData(Int32.MinValue, Int32.MaxValue, Int32.MaxValue)]
        [InlineData(0, 2, 1)]
        [InlineData(0, 10, 40)]
        public void NextLessThan(int low, int high, int other) {
            var generator = new IntegerGenerator(low, high);

            for (var attempt = 0; attempt < NUMBER_OF_ATTEMPTS; attempt++) {
                var value = generator.NextLessThan(other);

                Assert.True(value < other);
                Assert.True(value >= generator.Low);
                Assert.True(value < generator.High);
            }
        }

        [Theory]
        [InlineData(1, 2, -10)]
        [InlineData(0, 10, -1)]
        [InlineData(Int32.MinValue + 1, Int32.MaxValue, Int32.MinValue)]
        public void NextLessThanOrEqualTo_ThrowOnLessThanMinValue(int low, int high, int other) {
            var generator = new IntegerGenerator(low, high);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextLessThanOrEqualTo(other)
            );
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(0, 10, 0)]
        [InlineData(Int32.MinValue, Int32.MaxValue, Int32.MaxValue)]
        [InlineData(0, 10, 40)]
        public void NextLessThanOrEqualTo(int low, int high, int other) {
            var generator = new IntegerGenerator(low, high);

            for (var attempt = 0; attempt < NUMBER_OF_ATTEMPTS; attempt++) {
                var value = generator.NextLessThanOrEqualTo(other);

                Assert.True(value <= other);
                Assert.True(value >= generator.Low);
                Assert.True(value < generator.High);
            }
        }

    }


}
