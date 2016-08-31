using System;
using Xunit;

namespace Peddler {

    public class RandomExtensionsTests {

        private const int numberOfAttempts = 100;

        // --- NextSByte ---

        [Fact]
        public void NextSByte_WithDefaults() {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextSByte();

                Assert.True(value >= 0);
                Assert.True(value < SByte.MaxValue);
            }
        }

        [Fact]
        public void NextSByte_WithDefaults_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextSByte()
            );
        }

        [Theory]
        [InlineData((SByte)0)]
        [InlineData((SByte)1)]
        [InlineData((SByte)10)]
        [InlineData(SByte.MaxValue)]
        public void NextSByte_WithMaxValue(SByte maxValue) {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextSByte(maxValue);

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
        public void NextSByte_WithMaxValue_LessThanZero() {
            var random = new Random();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => random.NextSByte(-1)
            );
        }

        [Fact]
        public void NextSByte_WithMaxValue_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextSByte(1)
            );
        }

        [Theory]
        [InlineData((SByte)0, (SByte)0)]
        [InlineData((SByte)1, (SByte)1)]
        [InlineData(SByte.MaxValue, SByte.MaxValue)]
        [InlineData(SByte.MinValue, SByte.MaxValue)]
        [InlineData(SByte.MinValue, (SByte)(-1))]
        [InlineData(SByte.MinValue, (SByte)0)]
        [InlineData(SByte.MinValue, (SByte)1)]
        [InlineData((SByte)(-1), (SByte)100)]
        [InlineData((SByte)0, (SByte)1)]
        [InlineData((SByte)0, (SByte)100)]
        [InlineData((SByte)1, (SByte)100)]
        [InlineData((SByte)0, SByte.MaxValue)]
        [InlineData((SByte)1, SByte.MaxValue)]
        public void NextSByte_WithRange(SByte minValue, SByte maxValue) {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextSByte(minValue, maxValue);

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
        public void NextSByte_WithRange_MinValueLessThanMaxValue() {
            var random = new Random();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => random.NextSByte(10, 1)
            );
        }

        [Fact]
        public void NextSByte_WithRange_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextSByte(1, 10)
            );
        }

        // --- NextByte ---

        [Fact]
        public void NextByte_WithDefaults() {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextByte();

                Assert.True(value >= 0);
                Assert.True(value < Byte.MaxValue);
            }
        }

        [Fact]
        public void NextByte_WithDefaults_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextByte()
            );
        }

        [Theory]
        [InlineData((Byte)0)]
        [InlineData((Byte)1)]
        [InlineData((Byte)10)]
        [InlineData(Byte.MaxValue)]
        public void NextByte_WithMaxValue(Byte maxValue) {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextByte(maxValue);

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
        public void NextByte_WithMaxValue_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextByte(1)
            );
        }

        [Theory]
        [InlineData((Byte)0, (Byte)0)]
        [InlineData((Byte)1, (Byte)1)]
        [InlineData(Byte.MaxValue, Byte.MaxValue)]
        [InlineData((Byte)0, (Byte)1)]
        [InlineData((Byte)0, (Byte)100)]
        [InlineData((Byte)1, (Byte)100)]
        [InlineData((Byte)0, Byte.MaxValue)]
        [InlineData((Byte)1, Byte.MaxValue)]
        public void NextByte_WithRange(Byte minValue, Byte maxValue) {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextByte(minValue, maxValue);

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
        public void NextByte_WithRange_MinValueLessThanMaxValue() {
            var random = new Random();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => random.NextByte(10, 1)
            );
        }

        [Fact]
        public void NextByte_WithRange_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextByte(1, 10)
            );
        }

        // --- NextInt16 ---

        [Fact]
        public void NextInt16_WithDefaults() {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextInt16();

                Assert.True(value >= 0);
                Assert.True(value < Int16.MaxValue);
            }
        }

        [Fact]
        public void NextInt16_WithDefaults_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextInt16()
            );
        }

        [Theory]
        [InlineData((Int16)0)]
        [InlineData((Int16)1)]
        [InlineData((Int16)10)]
        [InlineData(Int16.MaxValue)]
        public void NextInt16_WithMaxValue(Int16 maxValue) {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextInt16(maxValue);

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
        public void NextInt16_WithMaxValue_LessThanZero() {
            var random = new Random();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => random.NextInt16(-1)
            );
        }

        [Fact]
        public void NextInt16_WithMaxValue_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextInt16(1)
            );
        }

        [Theory]
        [InlineData((Int16)0, (Int16)0)]
        [InlineData((Int16)1, (Int16)1)]
        [InlineData(Int16.MaxValue, Int16.MaxValue)]
        [InlineData(Int16.MinValue, Int16.MaxValue)]
        [InlineData(Int16.MinValue, (Int16)(-1))]
        [InlineData(Int16.MinValue, (Int16)0)]
        [InlineData(Int16.MinValue, (Int16)1)]
        [InlineData((Int16)(-1), (Int16)100)]
        [InlineData((Int16)0, (Int16)1)]
        [InlineData((Int16)0, (Int16)100)]
        [InlineData((Int16)1, (Int16)100)]
        [InlineData((Int16)0, Int16.MaxValue)]
        [InlineData((Int16)1, Int16.MaxValue)]
        public void NextInt16_WithRange(Int16 minValue, Int16 maxValue) {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextInt16(minValue, maxValue);

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
        public void NextInt16_WithRange_MinValueLessThanMaxValue() {
            var random = new Random();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => random.NextInt16(10, 1)
            );
        }

        [Fact]
        public void NextInt16_WithRange_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextInt16(1, 10)
            );
        }

        // --- NextUInt16 ---

        [Fact]
        public void NextUInt16_WithDefaults() {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextUInt16();

                Assert.True(value >= 0);
                Assert.True(value < UInt16.MaxValue);
            }
        }

        [Fact]
        public void NextUInt16_WithDefaults_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextUInt16()
            );
        }

        [Theory]
        [InlineData((UInt16)0)]
        [InlineData((UInt16)1)]
        [InlineData((UInt16)10)]
        [InlineData(UInt16.MaxValue)]
        public void NextUInt16_WithMaxValue(UInt16 maxValue) {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextUInt16(maxValue);

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
        public void NextUInt16_WithMaxValue_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextUInt16(1)
            );
        }

        [Theory]
        [InlineData((UInt16)0, (UInt16)0)]
        [InlineData((UInt16)1, (UInt16)1)]
        [InlineData(UInt16.MaxValue, UInt16.MaxValue)]
        [InlineData((UInt16)0, (UInt16)100)]
        [InlineData((UInt16)1, (UInt16)100)]
        [InlineData((UInt16)0, UInt16.MaxValue)]
        [InlineData((UInt16)1, UInt16.MaxValue)]
        [InlineData((UInt16)0x0000, (UInt16)0x0001)]
        public void NextUInt16_WithRange(UInt16 minValue, UInt16 maxValue) {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextUInt16(minValue, maxValue);

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
        public void NextUInt16_WithRange_MinValueLessThanMaxValue() {
            var random = new Random();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => random.NextUInt16(10, 1)
            );
        }

        [Fact]
        public void NextUInt16_WithRange_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextUInt16(1, 10)
            );
        }

        // --- NextUInt32 ---

        [Fact]
        public void NextUInt32_WithDefaults() {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextUInt32();

                Assert.True(value >= 0);
                Assert.True(value < UInt32.MaxValue);
            }
        }

        [Fact]
        public void NextUInt32_WithDefaults_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextUInt32()
            );
        }

        [Theory]
        [InlineData((UInt32)0)]
        [InlineData((UInt32)1)]
        [InlineData((UInt32)10)]
        [InlineData(UInt32.MaxValue)]
        public void NextUInt32_WithMaxValue(UInt32 maxValue) {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextUInt32(maxValue);

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
        public void NextUInt32_WithMaxValue_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextUInt32(1)
            );
        }

        [Theory]
        [InlineData((UInt32)0, (UInt32)0)]
        [InlineData((UInt32)1, (UInt32)1)]
        [InlineData(UInt32.MaxValue, UInt32.MaxValue)]
        [InlineData((UInt32)0, (UInt32)100)]
        [InlineData((UInt32)1, (UInt32)100)]
        [InlineData((UInt32)0, UInt32.MaxValue)]
        [InlineData((UInt32)1, UInt32.MaxValue)]
        [InlineData((UInt32)0x00000000, (UInt32)0x00010000)]
        [InlineData((UInt32)0x00000000, (UInt32)0x00000001)]
        public void NextUInt32_WithRange(UInt32 minValue, UInt32 maxValue) {
            var random = new Random();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = random.NextUInt32(minValue, maxValue);

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
        public void NextUInt32_WithRange_MinValueLessThanMaxValue() {
            var random = new Random();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => random.NextUInt32(10, 1)
            );
        }

        [Fact]
        public void NextUInt32_WithRange_NullRandom() {
            Random random = null;

            Assert.Throws<ArgumentNullException>(
                () => random.NextUInt32(1, 10)
            );
        }

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
