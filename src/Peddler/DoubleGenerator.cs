using System;
using System.Collections.Generic;
using System.Threading;

namespace Peddler {
    /// <summary>
    ///   An implementation of <see cref="IDistinctGenerator{Double}" />.
    /// </summary>
    /// <para>
    ///   Rather than generating values naively distributed from the lower and upper boundary by
    ///   multiplying a randomly generated value from 0.0 to 1.0 by the total range, this generator
    ///   attempts to evenly distribute randomly generated values across the orders of magnitude
    ///   covered by a range. For example, if <see cref="Low" /> is 0.1 and <see cref="High" /> is
    ///   1,000 the generated value should have an equal probability of falling between 0.1 and 1,
    ///   1 and 10, 10 and 100, or 100 and 1000; where as the naive algorithm would predominantly
    ///   generate numbers between 100 and 1000. This difference is especially noticeable when
    ///   generating numbers with a very large upper bound such as <see cref="Double.MaxValue" />.
    /// </para>
    /// <para>
    ///   The reason for choosing to implement random number generation in this way is to ensure
    ///   that tests are equally likely to detect bugs resulting from very small input values
    ///   compared to those resulting from very large input values.
    /// </para>
    public class DoubleGenerator : IComparableGenerator<Double> {
        private static ThreadLocal<Random> random { get; } =
            new ThreadLocal<Random>(() => new Random());

        private const Int32 EpsilonExponent = -323;
        private const Int32 MinimumIntervalExponentDelta = 16;

        /// <summary>
        ///   The minimum value to generate in calls to <see cref="Next()" />. Defaults to
        ///   <see cref="Double.MinValue" />
        /// </summary>
        public Double Low { get; }

        /// <summary>
        ///   The maximum value to generate in calls to <see cref="Next()" />. Defaults to
        ///   <see cref="Double.MaxValue" />.
        /// </summary>
        public Double High { get; }

        private Int32 defaultMinimumExponent { get; }
        private Int32 defaultMaximumExponent { get; }
        private List<(Int32, Int32)> defaultScales { get; }

        /// <inheritdoc />
        public IEqualityComparer<Double> EqualityComparer => EqualityComparer<Double>.Default;

        /// <inheritdoc />
        public IComparer<Double> Comparer => Comparer<Double>.Default;

        /// <summary>
        ///   Creates a default instance of <see cref="DoubleGenerator" /> that produces values from
        ///   <see cref="Double.MinValue" /> to <see cref="Double.MaxValue" />.
        /// </summary>
        public DoubleGenerator() : this(Double.MinValue, Double.MaxValue) {
        }

        /// <summary>
        ///   Creates an instance of <see cref="DoubleGenerator" /> that produces values between
        ///   <paramref name="low" /> and <paramref name="high" />.
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <see cref="Double" /> boundary for this generator.
        /// </param>
        /// <param name="high">
        ///   The inclusive, upper <see cref="Double" /> boundary for this generator.
        /// </param>
        public DoubleGenerator(Double low, Double high) {
            this.Low = low;
            this.High = high;

            (this.defaultMinimumExponent, this.defaultMaximumExponent, this.defaultScales) =
                CalculateScales(low, high);
        }

        /// <inheritdoc />
        public Double Next() {
            return Next(
                Low,
                High,
                this.defaultScales,
                this.defaultMinimumExponent,
                this.defaultMaximumExponent
            );
        }

        /// <inheritdoc />
        public Double NextDistinct(Double other) {
            Double result;
            do {
                result = this.Next();
            } while (EqualityComparer.Equals(other, result));

            return result;
        }

        /// <inheritdoc />
        public Double NextGreaterThan(Double other) {
            // The minimum interval between floating point numbers gets larger as the values get
            // larger.
            var minInterval =
                (Double)Math.Pow(
                    10,
                    Math.Max(
                        EpsilonExponent,
                        Math.Log10(Math.Abs(other)) - MinimumIntervalExponentDelta)
                );

            var result = Next(other + minInterval, this.High);

            return result;
        }

        /// <inheritdoc />
        public Double NextGreaterThanOrEqualTo(Double other) {
            return Next(other, this.High);
        }

        /// <inheritdoc />
        public Double NextLessThan(Double other) {
            // The minimum interval between floating point numbers gets larger as the values get
            // larger.
            var minInterval =
                (Double)Math.Pow(
                    10,
                    Math.Max(
                        EpsilonExponent,
                        Math.Log10(Math.Abs(other)) - MinimumIntervalExponentDelta)
                );

            return Next(this.Low, other - minInterval);
        }

        /// <inheritdoc />
        public Double NextLessThanOrEqualTo(Double other) {
            return Next(this.Low, other);
        }

        /// <summary>
        /// Generates a random value between <paramref name="low" /> (inclusive) and
        /// <paramref name="high" /> (exclusive). Assumes that <paramref name="low" /> is less than
        /// or equal to <paramref name="high" />.
        /// </summary>
        /// <param name="low">The smallest value to generate.</param>
        /// <param name="high">The largest value to generate.</param>
        /// <returns></returns>
        private static Double Next(Double low, Double high) {
            var (minExp, maxExp, scales) = CalculateScales(low, high);

            return Next(low, high, scales, minExp, maxExp);
        }

        private static (Int32 minExponent, Int32 maxExponent, List<(Int32 scale, Int32 sign)> scales) CalculateScales(
            Double low,
            Double high) {

            if (low > high) {
                throw new ArgumentException(
                    $"The '{nameof(high)}' value must be greater than or equal to the " +
                    $"'{nameof(low)}' value."
                );
            } else if (low >= Double.MaxValue) {
                throw new ArgumentException(
                    "Unable to generate a value above the specified limit.",
                    nameof(low)
                );
            } else if (high <= Double.MinValue) {
                throw new ArgumentException(
                    "Unable to generate a value below the specified limit.",
                    nameof(high)
                );
            }

            var minExp =
                Math.Max(
                    EpsilonExponent,
                    low >= 0 ?
                        (Int32)Math.Floor(Math.Log10(Math.Abs(low))) :
                        (Int32)Math.Ceiling(Math.Log10(Math.Abs(low))) - 1
                );
            var maxExp =
                Math.Max(
                    EpsilonExponent,
                    high >= 0 ?
                        (Int32)Math.Ceiling(Math.Log10(Math.Abs(high))) - 1 :
                        (Int32)Math.Floor(Math.Log10(Math.Abs(high)))
                );
            // scales contains all possible exponents are and signs from the largest possible
            // negative number to the largest possible positive number. (i.e. for -10 to +10 it
            // would contain [(1, -1), (0, -1), (0, 1), (1, 1).
            var scales = new List<(Int32, Int32)>();

            if (low < 0) {
                if (high < 0) {
                    // since both numbers are negative, the magnitude of the minimum value should
                    // be less than or equal to the magnitude of the maximum value.
                    for (var e = minExp; e >= maxExp; e--) {
                        scales.Add((e, -1));
                    }
                } else {
                    for (var e = minExp; e >= EpsilonExponent; e--) {
                        scales.Add((e, -1));
                    }

                    if (high > 0) {
                        for (var e = EpsilonExponent; e <= maxExp; e++) {
                            scales.Add((e, 1));
                        }
                    }
                }
            } else {
                for (var e = minExp; e <= maxExp; e++) {
                    scales.Add((e, 1));
                }
            }

            return (minExp, maxExp, scales);
        }

        private static Double Next(
            Double low,
            Double high,
            List<(Int32 scale, Int32 sign)> scales,
            Int32 minExp,
            Int32 maxExp) {

            var magnitudeIx = random.Value.Next(0, scales.Count);
            var magnitude = scales[magnitudeIx];

            Double fractionMin = 1.0f, fractionMax = 10.0f;

            if (magnitudeIx == 0) {
                if (low < 0) {
                    fractionMax = (Math.Abs(low) / Math.Pow(10, minExp));
                } else {
                    fractionMin = (low / Math.Pow(10, minExp));
                }
            }

            if (magnitudeIx == scales.Count - 1) {
                if (high < 0) {
                    fractionMin = (Math.Abs(high) / Math.Pow(10, maxExp));
                } else {
                    fractionMax = (high / Math.Pow(10, maxExp));
                }
            }

            if (fractionMax < fractionMin) {
                // normally we avoid generating fraction parts less than 1 because it
                // changes the magnitude. However in some cases the maximum fraction is less
                // than 1.0, in which case we're either targeting a fraction between n and
                // max where n is less than max and greater than or equal to 0
                fractionMin = 0f;
            }

            var fraction = fractionMin + random.Value.NextDouble() * (fractionMax - fractionMin);
            return magnitude.sign * fraction * Math.Pow(10, magnitude.scale);
        }
    }
}