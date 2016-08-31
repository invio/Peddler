using System;

namespace Peddler {

    public static class RandomExtensions {

        public static SByte NextSByte(this Random random) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextSByte(0, SByte.MaxValue);
        }

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

        public static Byte NextByte(this Random random) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextByte(0, Byte.MaxValue);
        }

        public static Byte NextByte(this Random random, Byte maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextByte(0, maxValue);
        }

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

        public static Int16 NextInt16(this Random random) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextInt16(0, Int16.MaxValue);
        }

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

        public static UInt16 NextUInt16(this Random random) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextUInt16(0, UInt16.MaxValue);
        }

        public static UInt16 NextUInt16(this Random random, UInt16 maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextUInt16(0, maxValue);
        }

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

        public static UInt32 NextUInt32(this Random random) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextUInt32(0, UInt32.MaxValue);
        }

        public static UInt32 NextUInt32(this Random random, UInt32 maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextUInt32(0, maxValue);
        }

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

        public static Int64 NextInt64(this Random random) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextInt64(0, Int64.MaxValue);
        }

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

        public static UInt64 NextUInt64(this Random random) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextUInt64(0, UInt64.MaxValue);
        }

        public static UInt64 NextUInt64(this Random random, UInt64 maxValue) {
            if (random == null) {
                throw new ArgumentNullException(nameof(random));
            }

            return random.NextUInt64(0, maxValue);
        }

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

            var isPreviousQuadrantBelowMaximum = false;
            var fullValue = 0ul;

            for (var quadrant = 1; quadrant <= 4; quadrant++) {
                UInt16 quadrantValue = 0;

                if (isPreviousQuadrantBelowMaximum) {
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
                            maximum++;
                        }

                        quadrantValue = (UInt16)random.Next((int)maximum);
                    }

                    if (quadrantValue < maximum) {
                        isPreviousQuadrantBelowMaximum = true;
                    }
                }

                fullValue = (fullValue << 16) | quadrantValue;
            }

            return minValue + fullValue;
        }

    }

}
