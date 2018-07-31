using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
        [InlineData(3.402822E+38f)] // Largest value less than MaxValue
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
        [InlineData(3.402822E+38f)] // Largest value less than MaxValue
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
        [InlineData(-3.402822E+38f)] // Smallest value greater than MinValue
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
        [InlineData(-3.402822E+38f)] // Smallest value greater than MinValue
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

        [Fact]
        public void EpsilonExponent_VerifyCompleteness() {
            // Generate numbers from 0 to 10 in intervals of 0.1 (effectively 101 possible
            // values: 0, 0.1, 0.2 ... 9.9, 10.0)
            //
            // After generating 10000 random values the probability of having failed to generate any
            // one of these values is 101 * ((100 / 101) ^ 10000) = 6.17 * (10 ^ -42) which seems
            // unlikely.
            var generator = new SingleGenerator(0, 10, epsilonExponent: -1);

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
        [InlineData(0f)]
        [InlineData(0.1f)]
        [InlineData(5.5f)]
        [InlineData(9.9f)]
        [InlineData(10f)]
        public void EpsilonExponent_Distinct_VerifyCompleteness(Single other) {
            // Generate numbers from 0 to 10 in intervals of 0.1 (effectively 101 possible
            // values: 0, 0.1, 0.2 ... 9.9, 10.0) with the exception of one pre-selected value.
            //
            // After generating 10000 random values the probability of having failed to generate any
            // one of these values is 101 * ((100 / 101) ^ 10000) = 6.17 * (10 ^ -42) which seems
            // unlikely.
            var generator = new SingleGenerator(0, 10, epsilonExponent: -1);

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
        [InlineData(0f)]
        [InlineData(0.1f)]
        [InlineData(5.5f)]
        [InlineData(9.9f)]
        [InlineData(10f)]
        [InlineData(20f)]
        [InlineData(50f)]
        [InlineData(100f)]
        public void SignificantFigures_Distinct(Single other) {
            // When generating values with a limited number of significant figures it is distinct
            // values must differ but a sufficient margin to be detectable when rounded to the
            // specified number of significant figures.
            //
            // For example when generating numbers from 0 to 100 with two significant figures and a
            // minimum exponent of -1, there are 101 values >= 0 and < 10 (0, 0.1, 0.2 ... 9.9, 10)
            // and there are 90 values > 10 and <= 100 (11, 12, .. 99, 100). Notably values such as
            // 12.4 are not representable and when generating a value that is distinct from 12 it is
            // important that a value that rounds to 12 is not generated.

            var generator = new SingleGenerator(0, 100, epsilonExponent: -1, significantFigures: 2);

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
                new SingleGenerator(epsilonExponent: SingleGenerator.MaximumEpsilonExponent);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.False(Single.IsInfinity(value));
                Assert.False(Single.IsNaN(value));
                Assert.InRange(generator.Comparer.Compare(Math.Abs(value), Single.MaxValue), -1, 0);
                Assert.InRange(
                    generator.Comparer.Compare(
                        Math.Abs(value),
                        (Single)Math.Pow(10, SingleGenerator.MaximumEpsilonExponent)),
                    0,
                    1
                );
            }
        }
    }
}