using System;
using Xunit;

namespace Peddler {
    public class SingleGeneratorTests {

        private const int numberOfAttempts = 1000;

        [Fact]
        public void Next_ValidValue() {
            var generator = new SingleGenerator();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.False(Single.IsInfinity(value));
                Assert.False(Single.IsNaN(value));
            }
        }

        [Fact]
        public void NextDistinct_ValidDistinctValues() {
            var generator = new SingleGenerator();
            var original = generator.Next();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var distinct = generator.NextDistinct(original);

                Assert.False(Single.IsInfinity(distinct));
                Assert.False(Single.IsNaN(distinct));
                Assert.NotEqual(original, distinct, generator.EqualityComparer);
            }
        }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(0.5f)]
        [InlineData(1.0f)]
        [InlineData(10.0f)]
        [InlineData(-0.5f)]
        [InlineData(-1.0f)]
        [InlineData(-10.0f)]
        [InlineData(Single.MinValue)]
        [InlineData(5E+30f)]
        [InlineData(3.40282245E+38f)] // Largest value less than MaxValue
        public void Next_GreaterThan(Single min) {
            var generator = new SingleGenerator();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextGreaterThan(min);

                Assert.False(Single.IsInfinity(value));
                Assert.False(Single.IsNaN(value));
                Assert.Equal(1, generator.Comparer.Compare(value, min));
            }
        }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(0.5f)]
        [InlineData(1.0f)]
        [InlineData(10.0f)]
        [InlineData(-0.5f)]
        [InlineData(-1.0f)]
        [InlineData(-10.0f)]
        [InlineData(Single.MinValue)]
        [InlineData(5E+30f)]
        [InlineData(3.40282245E+38f)] // Largest value less than MaxValue
        public void Next_GreaterThanOrEqual(Single min) {
            var generator = new SingleGenerator();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextGreaterThanOrEqualTo(min);

                Assert.False(Single.IsInfinity(value));
                Assert.False(Single.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(value, min), 0, 1);
            }
        }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(0.5f)]
        [InlineData(1.0f)]
        [InlineData(10.0f)]
        [InlineData(Single.MaxValue)]
        [InlineData(-5E+30f)]
        [InlineData(-3.40282245E+38f)] // Smallest value greater than MinValue
        public void Next_LessThan(Single max) {
            var generator = new SingleGenerator();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextLessThan(max);

                Assert.False(Single.IsInfinity(value));
                Assert.False(Single.IsNaN(value));
                Assert.Equal(-1, generator.Comparer.Compare(value, max));
            }
        }

        [Theory]
        [InlineData(0.0f)]
        [InlineData(0.5f)]
        [InlineData(1.0f)]
        [InlineData(10.0f)]
        [InlineData(Single.MaxValue)]
        [InlineData(-5E+30f)]
        [InlineData(-3.40282245E+38f)] // Smallest value greater than MinValue
        public void Next_LessThanOrEqual(Single max) {
            var generator = new SingleGenerator();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextLessThanOrEqualTo(max);

                Assert.False(Single.IsInfinity(value));
                Assert.False(Single.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(value, max), -1, 0);
            }
        }

        [Theory]
        [InlineData(0f, 0.1f)]
        [InlineData(0f, 10f)]
        [InlineData(0f, Single.MaxValue)]
        [InlineData(1f, 10f)]
        [InlineData(-0.1f, 0.1f)]
        [InlineData(-0.1f, Single.MaxValue)]
        [InlineData(-10f, 0f)]
        [InlineData(-10f, -1f)]
        public void Next_FixedRange(Single min, Single max) {
            var generator = new SingleGenerator(min, max);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.False(Single.IsInfinity(value));
                Assert.False(Single.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(value, max), -1, 0);
                Assert.InRange(generator.Comparer.Compare(value, min), 0, 1);
            }
        }

        [Theory]
        [InlineData(0f, 0.1f, 0.05f)]
        [InlineData(0f, 10f, 5f)]
        [InlineData(0f, Single.MaxValue, 5f)]
        [InlineData(1f, 10f, 5f)]
        [InlineData(-0.1f, 0.1f, 0f)]
        [InlineData(-0.1f, Single.MaxValue, 5f)]
        [InlineData(-10f, 0f, -5f)]
        [InlineData(-10f, -1f, -5f)]
        public void NextGreaterThan_FixedRange(Single min, Single max, Single other) {
            var generator = new SingleGenerator(min, max);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextGreaterThan(other);

                Assert.False(Single.IsInfinity(value));
                Assert.False(Single.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(value, max), -1, 0);
                Assert.Equal(1, generator.Comparer.Compare(value, other));
            }
        }

        [Theory]
        [InlineData(0f, 0.1f, 0.05f)]
        [InlineData(0f, 10f, 5f)]
        [InlineData(0f, Single.MaxValue, 5f)]
        [InlineData(1f, 10f, 5f)]
        [InlineData(-0.1f, 0.1f, 0f)]
        [InlineData(-0.1f, Single.MaxValue, 5f)]
        [InlineData(-10f, 0f, -5f)]
        [InlineData(-10f, -1f, -5f)]
        public void NextGreaterThanOrEqualTo_FixedRange(Single min, Single max, Single other) {
            var generator = new SingleGenerator(min, max);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextGreaterThanOrEqualTo(other);

                Assert.False(Single.IsInfinity(value));
                Assert.False(Single.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(value, max), -1, 0);
                Assert.InRange(generator.Comparer.Compare(value, other), 0, 1);
            }
        }

        [Theory]
        [InlineData(0f, 0.1f, 0.05f)]
        [InlineData(0f, 10f, 5f)]
        [InlineData(0f, Single.MaxValue, 5f)]
        [InlineData(1f, 10f, 5f)]
        [InlineData(-0.1f, 0.1f, 0f)]
        [InlineData(-0.1f, Single.MaxValue, 5f)]
        [InlineData(-10f, 0f, -5f)]
        [InlineData(-10f, -1f, -5f)]
        public void NextLessThan_FixedRange(Single min, Single max, Single other) {
            var generator = new SingleGenerator(min, max);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextLessThan(other);

                Assert.False(Single.IsInfinity(value));
                Assert.False(Single.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(value, min), 0, 1);
                Assert.Equal(-1, generator.Comparer.Compare(value, other));
            }
        }

        [Theory]
        [InlineData(0f, 0.1f, 0.05f)]
        [InlineData(0f, 10f, 5f)]
        [InlineData(0f, Single.MaxValue, 5f)]
        [InlineData(1f, 10f, 5f)]
        [InlineData(-0.1f, 0.1f, 0f)]
        [InlineData(-0.1f, Single.MaxValue, 5f)]
        [InlineData(-10f, 0f, -5f)]
        [InlineData(-10f, -1f, -5f)]
        public void NextLessThanOrEqualTo_FixedRange(Single min, Single max, Single other) {
            var generator = new SingleGenerator(min, max);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextLessThanOrEqualTo(other);

                Assert.False(Single.IsInfinity(value));
                Assert.False(Single.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(value, min), 0, 1);
                Assert.InRange(generator.Comparer.Compare(value, other), -1, 0);
            }
        }
    }
}