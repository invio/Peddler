using System;
using System.Collections.Immutable;
using System.Linq;
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
        [InlineData(3.40282245E+38d)]
        [InlineData(1.79769313486231E+308d)]
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
        [InlineData(3.40282245E+38d)]
        [InlineData(1.79769313486231E+308d)]
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
        [InlineData(-3.40282245E+38d)]
        [InlineData(-1.79769313486231E+308d)]
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
        [InlineData(-3.40282245E+38d)]
        [InlineData(-1.79769313486231E+308d)]
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

        [Fact]
        public void EpsilonExponent_VerifyCompleteness() {
            // Generate numbers from 0 to 10 in intervals of 0.1 (effectively 101 possible
            // values: 0, 0.1, 0.2 ... 9.9, 10.0)
            //
            // After generating 10000 random values the probability of having failed to generate any
            // one of these values is 101 * ((100 / 101) ^ 10000) = 6.17 * (10 ^ -42) which seems
            // unlikely.
            var generator = new DoubleGenerator(0, 10, epsilonExponent: -1);

            var generatedValues =
                Enumerable.Range(0, 10000)
                    .Select(_ => (Int32)Math.Round(generator.Next() * 10))
                    .ToImmutableHashSet();

            Assert.True(generatedValues.All(value => value >= 0 && value <= 100));

            for (var value = 0; value <= 100; value++) {
                Assert.Contains(value, generatedValues);
            }
        }

        [Theory]
        [InlineData(0d)]
        [InlineData(0.1d)]
        [InlineData(5.5d)]
        [InlineData(9.9d)]
        [InlineData(10d)]
        public void EpsilonExponent_Distinct_VerifyCompleteness(Double other) {
            // Generate numbers from 0 to 10 in intervals of 0.1 (effectively 101 possible
            // values: 0, 0.1, 0.2 ... 9.9, 10.0) with the exception of one pre-selected value.
            //
            // After generating 10000 random values the probability of having failed to generate any
            // one of these values is 101 * ((100 / 101) ^ 10000) = 6.17 * (10 ^ -42) which seems
            // unlikely.
            var generator = new DoubleGenerator(0, 10, epsilonExponent: -1);

            var generatedValues =
                Enumerable.Range(0, 10000)
                    .Select(_ => (Int32)Math.Round(generator.NextDistinct(other) * 10))
                    .ToImmutableHashSet();

            Assert.True(generatedValues.All(value => value >= 0 && value <= 100));

            for (var value = 0; value <= 100; value++) {
                if (value == (Int32)Math.Round(other * 10)) {
                    Assert.DoesNotContain(value, generatedValues);
                } else {
                    Assert.Contains(value, generatedValues);
                }
            }
        }

        [Theory]
        [InlineData(0d)]
        [InlineData(0.1d)]
        [InlineData(5.5d)]
        [InlineData(9.9d)]
        [InlineData(10d)]
        [InlineData(20d)]
        [InlineData(50d)]
        [InlineData(100d)]
        public void SignificantFigures_Distinct(Double other) {
            // When generating values with a limited number of significant figures it is distinct
            // values must differ but a sufficient margin to be detectable when rounded to the
            // specified number of significant figures.
            //
            // For example when generating numbers from 0 to 100 with two significant figures and a
            // minimum exponent of -1, there are 101 values >= 0 and < 10 (0, 0.1, 0.2 ... 9.9, 10)
            // and there are 90 values > 10 and <= 100 (11, 12, .. 99, 100). Notably values such as
            // 12.4 are not representable and when generating a value that is distinct from 12 it is
            // important that a value that rounds to 12 is not generated.

            var generator = new DoubleGenerator(0, 100, epsilonExponent: -1, significantFigures: 2);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(other);

                if (other < 10) {
                    Assert.NotEqual(
                        (Int32)Math.Round(other * 10),
                        (Int32)Math.Round(value * 10)
                    );
                } else {
                    Assert.NotEqual(
                        (Int32)Math.Round(other),
                        (Int32)Math.Round(value)
                    );
                }
            }
        }

        [Fact]
        public void EpsilonExponent_MaxValue() {
            var generator =
                new DoubleGenerator(epsilonExponent: DoubleGenerator.MaximumEpsilonExponent);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.False(Double.IsInfinity(value));
                Assert.False(Double.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(Math.Abs(value), Double.MaxValue), -1, 0);
                Assert.InRange(
                    generator.Comparer.Compare(
                        Math.Abs(value),
                        Math.Pow(10, DoubleGenerator.MaximumEpsilonExponent)),
                    0,
                    1
                );
            }
        }
    }
}