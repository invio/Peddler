using System;
using Xunit;

namespace Peddler {

    public class RandomExtensionsTests {

        private const int numberOfAttempts = 100;

        // --- NextInt64 ---

        [Fact]
        public void NextInt64_WithDefaults() {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextInt64();

                Assert.True(value >= 0);
                Assert.True(value < Int64.MaxValue);
            }
        }

        [Fact]
        public void NextInt64_WithDefaults_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextInt64()
            );
        }

        [Theory]
        [InlineData((Int64)0)]
        [InlineData((Int64)1)]
        [InlineData((Int64)10)]
        [InlineData(Int64.MaxValue)]
        public void NextInt64_WithMaxValue(Int64 maxValue) {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextInt64(maxValue);

                if (maxValue == 0) {
                    // The only situation when 'value' can be equal to 'maxValue'
                    // is when 'maxValue' is 0. This is identical to how
                    // Random.Next(int maxValue) operates.
                    Assert.Equal(maxValue, value);
                } else {
                    Assert.True(value >= 0);
                    Assert.True(value < maxValue);
                }
            }
        }

        [Fact]
        public void NextInt64_WithMaxValue_LessThanZero() {
            var random = new Random();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => random.NextInt64(-1)
            );
        }

        [Fact]
        public void NextInt64_WithMaxValue_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextInt64(1)
            );
        }

        [Theory]
        [InlineData((Int64)0, (Int64)0)]
        [InlineData((Int64)1, (Int64)1)]
        [InlineData(Int64.MaxValue, Int64.MaxValue)]
        [InlineData(Int64.MinValue, Int64.MaxValue)]
        [InlineData(Int64.MinValue, (Int64)(-1))]
        [InlineData(Int64.MinValue, (Int64)0)]
        [InlineData(Int64.MinValue, (Int64)1)]
        [InlineData((Int64)(-1), (Int64)100)]
        [InlineData((Int64)0, (Int64)100)]
        [InlineData((Int64)1, (Int64)100)]
        [InlineData((Int64)0, Int64.MaxValue)]
        [InlineData((Int64)1, Int64.MaxValue)]
        [InlineData((Int64)0x0000000000000000, (Int64)0x0001000000000000)]
        [InlineData((Int64)0x0000000000000000, (Int64)0x0000000100000000)]
        [InlineData((Int64)0x0000000000000000, (Int64)0x0000000000010000)]
        [InlineData((Int64)0x0000000000000000, (Int64)0x0000000000000001)]
        public void NextInt64_WithRange(Int64 minValue, Int64 maxValue) {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextInt64(minValue, maxValue);

                if (minValue == maxValue) {
                    // The only situation when 'value' can be equal to 'maxValue'
                    // is when 'maxValue' is equal to 'minValue'. This is identical
                    // to how Random.Next(int minValue, maxValue) operates.
                    Assert.Equal(minValue, value);
                } else {
                    Assert.True(value >= minValue);
                    Assert.True(value < maxValue);
                }
            }
        }

        [Fact]
        public void NextInt64_WithRange_MinValueLessThanMaxValue() {
            var random = new Random();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => random.NextInt64(10, 1)
            );
        }

        [Fact]
        public void NextInt64_WithRange_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextInt64(1, 10)
            );
        }

        // --- NextUInt64 ---

        [Fact]
        public void NextUInt64_WithDefaults() {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextUInt64();

                Assert.True(value >= 0);
                Assert.True(value < UInt64.MaxValue);
            }
        }

        [Fact]
        public void NextUInt64_WithDefaults_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextUInt64()
            );
        }

        [Theory]
        [InlineData((UInt64)0)]
        [InlineData((UInt64)1)]
        [InlineData((UInt64)10)]
        [InlineData((UInt64)Int64.MaxValue)]
        [InlineData(UInt64.MaxValue)]
        public void NextUInt64_WithMaxValue(UInt64 maxValue) {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextUInt64(maxValue);

                if (maxValue == 0) {
                    // The only situation when 'value' can be equal to 'maxValue'
                    // is when 'maxValue' is 0. This is identical to how
                    // Random.Next(int maxValue) operates.
                    Assert.Equal(maxValue, value);
                } else {
                    Assert.True(value >= 0);
                    Assert.True(value < maxValue);
                }
            }
        }

        [Fact]
        public void NextUInt64_WithMaxValue_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextUInt64(1)
            );
        }

        [Theory]
        [InlineData((UInt64)0, (UInt64)0)]
        [InlineData((UInt64)1, (UInt64)1)]
        [InlineData(UInt64.MaxValue, UInt64.MaxValue)]
        [InlineData((UInt64)0, (UInt64)100)]
        [InlineData((UInt64)1, (UInt64)100)]
        [InlineData((UInt64)0, UInt64.MaxValue)]
        [InlineData((UInt64)1, UInt64.MaxValue)]
        [InlineData((UInt64)0x0000000000000000, (UInt64)0x0001000000000000)]
        [InlineData((UInt64)0x0000000000000000, (UInt64)0x0000000100000000)]
        [InlineData((UInt64)0x0000000000000000, (UInt64)0x0000000000010000)]
        [InlineData((UInt64)0x0000000000000000, (UInt64)0x0000000000000001)]
        public void NextUInt64_WithRange(UInt64 minValue, UInt64 maxValue) {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextUInt64(minValue, maxValue);

                if (minValue == maxValue) {
                    // The only situation when 'value' can be equal to 'maxValue'
                    // is when 'maxValue' is equal to 'minValue'. This is identical
                    // to how Random.Next(int minValue, maxValue) operates.
                    Assert.Equal(minValue, value);
                } else {
                    Assert.True(value >= minValue);
                    Assert.True(value < maxValue);
                }
            }
        }

        [Fact]
        public void NextUInt64_WithRange_MinValueLessThanMaxValue() {
            var random = new Random();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => random.NextUInt64(10, 1)
            );
        }

        [Fact]
        public void NextUInt64_WithRange_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextUInt64(1, 10)
            );
        }

    }

}
