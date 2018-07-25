using System;
using Xunit;

namespace Peddler {
    public class DoubleGeneratorTests {

        private const int numberOfAttempts = 1000;

        [Fact]
        public void Next_ValidValue() {
            var generator = new DoubleGenerator();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.False(Double.IsInfinity(value));
                Assert.False(Double.IsNaN(value));
            }
        }

        [Fact]
        public void NextDistinct_ValidDistinctValues() {
            var generator = new DoubleGenerator();
            var original = generator.Next();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var distinct = generator.NextDistinct(original);

                Assert.False(Double.IsInfinity(distinct));
                Assert.False(Double.IsNaN(distinct));
                Assert.NotEqual(original, distinct, generator.EqualityComparer);
            }
        }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(0.5d)]
        [InlineData(1.0d)]
        [InlineData(10.0d)]
        [InlineData(-0.5d)]
        [InlineData(-1.0d)]
        [InlineData(-10.0d)]
        [InlineData(Double.MinValue)]
        [InlineData(3.40282245E+38d)] // Largest value less than MaxValue
        public void Next_GreaterThan(Double min) {
            var generator = new DoubleGenerator();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextGreaterThan(min);

                Assert.False(Double.IsInfinity(value));
                Assert.False(Double.IsNaN(value));
                Assert.Equal(1, generator.Comparer.Compare(value, min));
            }
        }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(0.5d)]
        [InlineData(1.0d)]
        [InlineData(10.0d)]
        [InlineData(-0.5d)]
        [InlineData(-1.0d)]
        [InlineData(-10.0d)]
        [InlineData(Double.MinValue)]
        [InlineData(3.40282245E+38d)] // Largest value less than MaxValue
        public void Next_GreaterThanOrEqual(Double min) {
            var generator = new DoubleGenerator();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextGreaterThanOrEqualTo(min);

                Assert.False(Double.IsInfinity(value));
                Assert.False(Double.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(value, min), 0, 1);
            }
        }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(0.5d)]
        [InlineData(1.0d)]
        [InlineData(10.0d)]
        [InlineData(Double.MaxValue)]
        [InlineData(-3.40282245E+38d)] // Smallest value greater than MinValue
        public void Next_LessThan(Double max) {
            var generator = new DoubleGenerator();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextLessThan(max);

                Assert.False(Double.IsInfinity(value));
                Assert.False(Double.IsNaN(value));
                Assert.Equal(-1, generator.Comparer.Compare(value, max));
            }
        }

        [Theory]
        [InlineData(0.0d)]
        [InlineData(0.5d)]
        [InlineData(1.0d)]
        [InlineData(10.0d)]
        [InlineData(Double.MaxValue)]
        [InlineData(-3.40282245E+38d)] // Smallest value greater than MinValue
        public void Next_LessThanOrEqual(Double max) {
            var generator = new DoubleGenerator();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextLessThanOrEqualTo(max);

                Assert.False(Double.IsInfinity(value));
                Assert.False(Double.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(value, max), -1, 0);
            }
        }

        [Theory]
        [InlineData(0d, 0.1d)]
        [InlineData(0d, 10d)]
        [InlineData(0d, Double.MaxValue)]
        [InlineData(1d, 10d)]
        [InlineData(-0.1d, 0.1d)]
        [InlineData(-0.1d, Double.MaxValue)]
        [InlineData(-10d, 0d)]
        [InlineData(-10d, -1d)]
        public void Next_FixedRange(Double min, Double max) {
            var generator = new DoubleGenerator(min, max);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.False(Double.IsInfinity(value));
                Assert.False(Double.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(value, max), -1, 0);
                Assert.InRange(generator.Comparer.Compare(value, min), 0, 1);
            }
        }

        [Theory]
        [InlineData(0d, 0.1d, 0.05d)]
        [InlineData(0d, 10d, 5d)]
        [InlineData(0d, Double.MaxValue, 5d)]
        [InlineData(1d, 10d, 5d)]
        [InlineData(-0.1d, 0.1d, 0d)]
        [InlineData(-0.1d, Double.MaxValue, 5d)]
        [InlineData(-10d, 0d, -5d)]
        [InlineData(-10d, -1d, -5d)]
        public void NextGreaterThan_FixedRange(Double min, Double max, Double other) {
            var generator = new DoubleGenerator(min, max);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextGreaterThan(other);

                Assert.False(Double.IsInfinity(value));
                Assert.False(Double.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(value, max), -1, 0);
                Assert.Equal(1, generator.Comparer.Compare(value, other));
            }
        }

        [Theory]
        [InlineData(0d, 0.1d, 0.05d)]
        [InlineData(0d, 10d, 5d)]
        [InlineData(0d, Double.MaxValue, 5d)]
        [InlineData(1d, 10d, 5d)]
        [InlineData(-0.1d, 0.1d, 0d)]
        [InlineData(-0.1d, Double.MaxValue, 5d)]
        [InlineData(-10d, 0d, -5d)]
        [InlineData(-10d, -1d, -5d)]
        public void NextGreaterThanOrEqualTo_FixedRange(Double min, Double max, Double other) {
            var generator = new DoubleGenerator(min, max);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextGreaterThanOrEqualTo(other);

                Assert.False(Double.IsInfinity(value));
                Assert.False(Double.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(value, max), -1, 0);
                Assert.InRange(generator.Comparer.Compare(value, other), 0, 1);
            }
        }

        [Theory]
        [InlineData(0d, 0.1d, 0.05d)]
        [InlineData(0d, 10d, 5d)]
        [InlineData(0d, Double.MaxValue, 5d)]
        [InlineData(1d, 10d, 5d)]
        [InlineData(-0.1d, 0.1d, 0d)]
        [InlineData(-0.1d, Double.MaxValue, 5d)]
        [InlineData(-10d, 0d, -5d)]
        [InlineData(-10d, -1d, -5d)]
        public void NextLessThan_FixedRange(Double min, Double max, Double other) {
            var generator = new DoubleGenerator(min, max);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextLessThan(other);

                Assert.False(Double.IsInfinity(value));
                Assert.False(Double.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(value, min), 0, 1);
                Assert.Equal(-1, generator.Comparer.Compare(value, other));
            }
        }

        [Theory]
        [InlineData(0d, 0.1d, 0.05d)]
        [InlineData(0d, 10d, 5d)]
        [InlineData(0d, Double.MaxValue, 5d)]
        [InlineData(1d, 10d, 5d)]
        [InlineData(-0.1d, 0.1d, 0d)]
        [InlineData(-0.1d, Double.MaxValue, 5d)]
        [InlineData(-10d, 0d, -5d)]
        [InlineData(-10d, -1d, -5d)]
        public void NextLessThanOrEqualTo_FixedRange(Double min, Double max, Double other) {
            var generator = new DoubleGenerator(min, max);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextLessThanOrEqualTo(other);

                Assert.False(Double.IsInfinity(value));
                Assert.False(Double.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(value, min), 0, 1);
                Assert.InRange(generator.Comparer.Compare(value, other), -1, 0);
            }
        }
    }
}