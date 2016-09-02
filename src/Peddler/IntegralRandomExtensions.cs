using System;

namespace Peddler {

    /// <summary>
    ///   A collection of extension methods for the <see cref="System.Random" /> class
    ///   that provide the ability to generate any integral type in using APIs that
    ///   are identical to <see cref="System.Random.Next(int, int)" />,
    ///   <see cref="System.Random.Next(int)" />, and <see cref="System.Random.Next()" />.
    /// </summary>
    public static class IntegralRandomExtensions {

        /// <summary>
        ///   Creates a random <see cref="System.SByte" /> with a value between
        ///   zero and <see cref="System.SByte.MaxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <returns>
        ///   The <see cref="System.SByte" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        public static SByte NextSByte(this Random random) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextSByte(0, SByte.MaxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.SByte" /> with a value between
        ///   zero and <paramref name="maxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <param name="maxValue">
        ///   The upper, exclusive boundary for the <see cref="System.SByte" />
        ///   that will be randomly generated.
        /// </param>
        /// <returns>
        ///   The <see cref="System.SByte" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="maxValue" /> is less than zero.
        /// </exception>
        public static SByte NextSByte(this Random random, SByte maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            if (maxValue < 0) {
                throw new ArgumentOutOfRangeException(
                    nameof(maxValue),
                    $"'{nameof(maxValue)}' must be greater than zero."
                );
            }

            return random.NextSByte(0, maxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.SByte" /> with a value between
        ///   <paramref name="minValue" /> and <paramref name="maxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <param name="minValue">
        ///   The lower, inclusive boundary for the <see cref="System.SByte" />
        ///   that will be randomly generated.
        /// </param>
        /// <param name="maxValue">
        ///   The upper, exclusive boundary for the <see cref="System.SByte" />
        ///   that will be randomly generated.
        /// </param>
        /// <returns>
        ///   The <see cref="System.SByte" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="minValue" /> is greater than
        ///   <paramref name="maxValue" />.
        /// </exception>
        public static SByte NextSByte(this Random random, SByte minValue, SByte maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            if (minValue > maxValue) {
                throw new ArgumentOutOfRangeException(
                    nameof(minValue),
                    $"'{nameof(minValue)}' cannot be greater than maxValue."
                );
            }

            return (SByte)random.Next((Int32)minValue, (Int32)maxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.Byte" /> with a value between
        ///   zero and <see cref="System.Byte.MaxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <returns>
        ///   The <see cref="System.Byte" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        public static Byte NextByte(this Random random) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextByte(0, Byte.MaxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.Byte" /> with a value between
        ///   zero and <paramref name="maxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <param name="maxValue">
        ///   The upper, exclusive boundary for the <see cref="System.Byte" />
        ///   that will be randomly generated.
        /// </param>
        /// <returns>
        ///   The <see cref="System.Byte" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        public static Byte NextByte(this Random random, Byte maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextByte(0, maxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.Byte" /> with a value between
        ///   <paramref name="minValue" /> and <paramref name="maxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <param name="minValue">
        ///   The lower, inclusive boundary for the <see cref="System.Byte" />
        ///   that will be randomly generated.
        /// </param>
        /// <param name="maxValue">
        ///   The upper, exclusive boundary for the <see cref="System.Byte" />
        ///   that will be randomly generated.
        /// </param>
        /// <returns>
        ///   The <see cref="System.Byte" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="minValue" /> is greater than
        ///   <paramref name="maxValue" />.
        /// </exception>
        public static Byte NextByte(this Random random, Byte minValue, Byte maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            if (minValue > maxValue) {
                throw new ArgumentOutOfRangeException(
                    nameof(minValue),
                    $"'{nameof(minValue)}' cannot be greater than maxValue."
                );
            }

            return (Byte)random.Next((Int32)minValue, (Int32)maxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.Int16" /> with a value between
        ///   zero and <see cref="System.Int16.MaxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <returns>
        ///   The <see cref="System.Int16" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        public static Int16 NextInt16(this Random random) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextInt16(0, Int16.MaxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.Int16" /> with a value between
        ///   zero and <paramref name="maxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <param name="maxValue">
        ///   The upper, exclusive boundary for the <see cref="System.Int16" />
        ///   that will be randomly generated.
        /// </param>
        /// <returns>
        ///   The <see cref="System.Int16" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="maxValue" /> is less than zero.
        /// </exception>
        public static Int16 NextInt16(this Random random, Int16 maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            if (maxValue < 0) {
                throw new ArgumentOutOfRangeException(
                    nameof(maxValue),
                    $"'{nameof(maxValue)}' must be greater than zero."
                );
            }

            return random.NextInt16(0, maxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.Int16" /> with a value between
        ///   <paramref name="minValue" /> and <paramref name="maxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <param name="minValue">
        ///   The lower, inclusive boundary for the <see cref="System.Int16" />
        ///   that will be randomly generated.
        /// </param>
        /// <param name="maxValue">
        ///   The upper, exclusive boundary for the <see cref="System.Int16" />
        ///   that will be randomly generated.
        /// </param>
        /// <returns>
        ///   The <see cref="System.Int16" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="minValue" /> is greater than
        ///   <paramref name="maxValue" />.
        /// </exception>
        public static Int16 NextInt16(this Random random, Int16 minValue, Int16 maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            if (minValue > maxValue) {
                throw new ArgumentOutOfRangeException(
                    nameof(minValue),
                    $"'{nameof(minValue)}' cannot be greater than maxValue."
                );
            }

            return (Int16)random.Next((Int32)minValue, (Int32)maxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.UInt16" /> with a value between
        ///   zero and <see cref="System.UInt16.MaxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <returns>
        ///   The <see cref="System.UInt16" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        public static UInt16 NextUInt16(this Random random) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextUInt16(0, UInt16.MaxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.UInt16" /> with a value between
        ///   zero and <paramref name="maxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <param name="maxValue">
        ///   The upper, exclusive boundary for the <see cref="System.UInt16" />
        ///   that will be randomly generated.
        /// </param>
        /// <returns>
        ///   The <see cref="System.UInt16" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        public static UInt16 NextUInt16(this Random random, UInt16 maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextUInt16(0, maxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.UInt16" /> with a value between
        ///   <paramref name="minValue" /> and <paramref name="maxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <param name="minValue">
        ///   The lower, inclusive boundary for the <see cref="System.UInt16" />
        ///   that will be randomly generated.
        /// </param>
        /// <param name="maxValue">
        ///   The upper, exclusive boundary for the <see cref="System.UInt16" />
        ///   that will be randomly generated.
        /// </param>
        /// <returns>
        ///   The <see cref="System.UInt16" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="minValue" /> is greater than
        ///   <paramref name="maxValue" />.
        /// </exception>
        public static UInt16 NextUInt16(this Random random, UInt16 minValue, UInt16 maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            if (maxValue < 0) {
                throw new ArgumentOutOfRangeException(
                    nameof(maxValue),
                    $"'{nameof(maxValue)}' must be greater than zero."
                );
            }

            return (UInt16)random.Next((Int32)minValue, (Int32)maxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.UInt32" /> with a value between
        ///   zero and <see cref="System.UInt32.MaxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <returns>
        ///   The <see cref="System.UInt32" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        public static UInt32 NextUInt32(this Random random) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextUInt32(0, UInt32.MaxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.UInt32" /> with a value between
        ///   zero and <paramref name="maxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <param name="maxValue">
        ///   The upper, exclusive boundary for the <see cref="System.UInt32" />
        ///   that will be randomly generated.
        /// </param>
        /// <returns>
        ///   The <see cref="System.UInt32" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        public static UInt32 NextUInt32(this Random random, UInt32 maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextUInt32(0, maxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.UInt32" /> with a value between
        ///   <paramref name="minValue" /> and <paramref name="maxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <param name="minValue">
        ///   The lower, inclusive boundary for the <see cref="System.UInt32" />
        ///   that will be randomly generated.
        /// </param>
        /// <param name="maxValue">
        ///   The upper, exclusive boundary for the <see cref="System.UInt32" />
        ///   that will be randomly generated.
        /// </param>
        /// <returns>
        ///   The <see cref="System.UInt32" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="minValue" /> is greater than
        ///   <paramref name="maxValue" />.
        /// </exception>
        public static UInt32 NextUInt32(this Random random, UInt32 minValue, UInt32 maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            if (maxValue < 0) {
                throw new ArgumentOutOfRangeException(
                    nameof(maxValue),
                    $"'{nameof(maxValue)}' must be greater than zero."
                );
            }

            return (UInt32)random.NextUInt64((UInt64)minValue, (UInt64)maxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.Int64" /> with a value between
        ///   zero and <see cref="System.Int64.MaxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <returns>
        ///   The <see cref="System.Int64" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        public static Int64 NextInt64(this Random random) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextInt64(0, Int64.MaxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.Int64" /> with a value between
        ///   zero and <paramref name="maxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <param name="maxValue">
        ///   The upper, exclusive boundary for the <see cref="System.Int64" />
        ///   that will be randomly generated.
        /// </param>
        /// <returns>
        ///   The <see cref="System.Int64" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="maxValue" /> is less than zero.
        /// </exception>
        public static Int64 NextInt64(this Random random, Int64 maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            if (maxValue < 0) {
                throw new ArgumentOutOfRangeException(
                    nameof(maxValue),
                    $"'{nameof(maxValue)}' must be greater than zero."
                );
            }

            return random.NextInt64(0, maxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.Int64" /> with a value between
        ///   <paramref name="minValue" /> and <paramref name="maxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <param name="minValue">
        ///   The lower, inclusive boundary for the <see cref="System.Int64" />
        ///   that will be randomly generated.
        /// </param>
        /// <param name="maxValue">
        ///   The upper, exclusive boundary for the <see cref="System.Int64" />
        ///   that will be randomly generated.
        /// </param>
        /// <returns>
        ///   The <see cref="System.Int64" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="minValue" /> is greater than
        ///   <paramref name="maxValue" />.
        /// </exception>
        public static Int64 NextInt64(this Random random, Int64 minValue, Int64 maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            if (minValue > maxValue) {
                throw new ArgumentOutOfRangeException(
                    nameof(minValue),
                    $"'{nameof(minValue)}' cannot be greater than maxValue."
                );
            }

            var range = (UInt64)(maxValue - minValue);

            return minValue + (Int64)random.NextUInt64(0, range);
        }

        /// <summary>
        ///   Creates a random <see cref="System.UInt64" /> with a value between
        ///   zero and <see cref="System.UInt64.MaxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <returns>
        ///   The <see cref="System.UInt64" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        public static UInt64 NextUInt64(this Random random) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextUInt64(0, UInt64.MaxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.UInt64" /> with a value between
        ///   zero and <paramref name="maxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <param name="maxValue">
        ///   The upper, exclusive boundary for the <see cref="System.UInt64" />
        ///   that will be randomly generated.
        /// </param>
        /// <returns>
        ///   The <see cref="System.UInt64" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        public static UInt64 NextUInt64(this Random random, UInt64 maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextUInt64(0, maxValue);
        }

        /// <summary>
        ///   Creates a random <see cref="System.UInt64" /> with a value between
        ///   <paramref name="minValue" /> and <paramref name="maxValue" />.
        /// </summary>
        /// <param name="random">
        ///   The <see cref="System.Random" /> instance being used via this extension method.
        /// </param>
        /// <param name="minValue">
        ///   The lower, inclusive boundary for the <see cref="System.UInt64" />
        ///   that will be randomly generated.
        /// </param>
        /// <param name="maxValue">
        ///   The upper, exclusive boundary for the <see cref="System.UInt64" />
        ///   that will be randomly generated.
        /// </param>
        /// <returns>
        ///   The <see cref="System.UInt64" /> that was randomly generated.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   Thrown when <paramref name="random" /> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="minValue" /> is greater than
        ///   <paramref name="maxValue" />.
        /// </exception>
        public static UInt64 NextUInt64(this Random random, UInt64 minValue, UInt64 maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            if (minValue > maxValue) {
                throw new ArgumentOutOfRangeException(
                    nameof(minValue),
                    $"'{nameof(minValue)}' cannot be greater than maxValue."
                );
            }

            // By subtracting low from high, our range is now normalized.
            // For example, a range of 2 to 10 is now 0 to 8.
            var range = maxValue - minValue;

            var isPreviousQuadrantBelowItsMaximum = false;
            var fullValue = 0ul;

            for (var quadrant = 1; quadrant <= 4; quadrant++) {
                UInt16 quadrantValue = 0;

                if (isPreviousQuadrantBelowItsMaximum) {
                    quadrantValue = (UInt16)random.Next(((int)UInt16.MaxValue) + 1);
                } else {
                    var maximum = (UInt16)(range >> (64 - (quadrant * 16)));

                    if (maximum > 0) {

                        // We use a mask to see if any bits in lower order quadrants are set.
                        // If they are, that means this quadrant can be inclusive with the
                        // current value when generating a random number for this quadrant.
                        // If it happens to get the maximum, the lower order quadrants
                        // have the flexibility to reign in the maximum as the range gets
                        // pinched.

                        UInt64 mask = 0xFFFFFFFFFFFFFFFF;

                        for (var shift = 0; shift < quadrant; shift++) {
                            mask = mask >> 16;
                        }

                        if ((range & mask) > 0) {
                            quadrantValue = (UInt16)random.Next(((int)maximum) + 1);
                        } else {
                            quadrantValue = random.NextUInt16(maximum);
                        }
                    }

                    if (quadrantValue < maximum) {
                        isPreviousQuadrantBelowItsMaximum = true;
                    }
                }

                fullValue = (fullValue << 16) | quadrantValue;
            }

            return minValue + fullValue;
        }

    }

}
