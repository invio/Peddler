using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Invio.Xunit;
using Xunit;

namespace Peddler {

    [UnitTest]
    public sealed class KindSensitiveDateTimeEqualityComparerTests {

        private static IList<DateTimeUnit> units { get; }
        private static IList<DateTimeKind> kinds { get; }
        private static IGenerator<Int64> tickGenerator { get; }

        static KindSensitiveDateTimeEqualityComparerTests() {
            units = ImmutableList.Create<DateTimeUnit>(
                DateTimeUnit.Tick,
                DateTimeUnit.Millisecond,
                DateTimeUnit.Second,
                DateTimeUnit.Minute,
                DateTimeUnit.Hour,
                DateTimeUnit.Day
            );

            kinds = ImmutableList.Create<DateTimeKind>(
                DateTimeKind.Unspecified,
                DateTimeKind.Local,
                DateTimeKind.Utc
            );

            tickGenerator =
                new Int64Generator(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks);
        }

        public static IEnumerable<object[]> MismatchedKinds_MemberData {
            get {
                foreach (var leftKind in kinds) {
                    foreach (var rightKind in kinds) {
                        if (leftKind == rightKind) {
                            continue;
                        }

                        yield return new object[] { leftKind, rightKind };
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(MismatchedKinds_MemberData))]
        public void Equals_KindMismatch(DateTimeKind leftKind, DateTimeKind rightKind) {
            var ticks = tickGenerator.Next();
            var comparer = new KindSensitiveDateTimeEqualityComparer();

            var left = new DateTime(ticks, leftKind);
            var right = new DateTime(ticks, rightKind);

            Assert.False(comparer.Equals(left, right));
        }

        public static IEnumerable<object[]> MatchingKinds_MemberData {
            get {
                foreach (var unit in units) {
                    foreach (var kind in kinds) {
                        yield return new object[] { kind, unit };
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(MatchingKinds_MemberData))]
        public void Equals_MatchingKinds(DateTimeKind kind, DateTimeUnit granularity) {

            // Arrange

            var comparer = new KindSensitiveDateTimeEqualityComparer(granularity);
            var original = new DateTime(DateTime.Now.Ticks, kind);
            var ticksPerUnit = DateTimeUtilities.GetTicksPerUnit(granularity);

            // Act

            var same = new DateTime(original.Ticks, kind);
            var different = new DateTime(original.Ticks + ticksPerUnit, kind);

            // Assert

            Assert.True(comparer.Equals(original, same));
            Assert.False(comparer.Equals(original, different));
        }

        [Theory]
        [MemberData(nameof(MatchingKinds_MemberData))]
        public void GetHashCode_MatchingKinds(DateTimeKind kind, DateTimeUnit granularity) {

            // Arrange

            var ticks = tickGenerator.Next();
            var comparer = new KindSensitiveDateTimeEqualityComparer(granularity);

            // Act

            var left = comparer.GetHashCode(new DateTime(ticks, kind));
            var right = comparer.GetHashCode(new DateTime(ticks, kind));

            // Assert

            Assert.Equal(left, right);
        }

        public static IEnumerable<object[]> UnitsCauseValuesToBeFlooredToPreviousInterval_MemberData {
            get {
                foreach (var kind in kinds) {
                    yield return new object[] {
                        new DateTime(2016, 10, 11, 01, 23, 56, 111, kind),
                        new DateTime(2016, 10, 11, 01, 23, 56, 111, kind).AddTicks(111),
                        new DateTime(2016, 10, 11, 01, 23, 56, 111, kind).AddTicks(999),
                        DateTimeUnit.Millisecond
                    };

                    yield return new object[] {
                        new DateTime(2016, 10, 11, 01, 23, 56, 000, kind),
                        new DateTime(2016, 10, 11, 01, 23, 56, 111, kind),
                        new DateTime(2016, 10, 11, 01, 23, 56, 999, kind),
                        DateTimeUnit.Second
                    };

                    yield return new object[] {
                        new DateTime(2016, 10, 11, 01, 23, 00, kind),
                        new DateTime(2016, 10, 11, 01, 23, 59, kind),
                        new DateTime(2016, 10, 11, 01, 23, 01, kind),
                        DateTimeUnit.Minute
                    };

                    yield return new object[] {
                        new DateTime(2016, 10, 11, 01, 00, 00, kind),
                        new DateTime(2016, 10, 11, 01, 11, 11, kind),
                        new DateTime(2016, 10, 11, 01, 59, 59, kind),
                        DateTimeUnit.Hour
                    };

                    yield return new object[] {
                        new DateTime(2016, 10, 11, 00, 00, 00, kind),
                        new DateTime(2016, 10, 11, 01, 23, 45, kind),
                        new DateTime(2016, 10, 11, 23, 59, 59, kind),
                        DateTimeUnit.Day
                    };
                }
            }
        }

        [Theory]
        [MemberData(nameof(UnitsCauseValuesToBeFlooredToPreviousInterval_MemberData))]
        public void Equals_UnitsCauseValuesToBeFlooredToPreviousInterval(
            DateTime onInterval,
            DateTime highInInterval,
            DateTime lowInInterval,
            DateTimeUnit granularity) {

            // Arrange

            var comparer = new KindSensitiveDateTimeEqualityComparer(granularity);
            var ticksPerUnit = DateTimeUtilities.GetTicksPerUnit(granularity);

            var nextInternal = onInterval.AddTicks(ticksPerUnit);
            var previousIntervalMaximum = onInterval.AddTicks(-1L);

            // Act

            var highToLow = comparer.Equals(highInInterval, lowInInterval);
            var highToInterval = comparer.Equals(highInInterval, onInterval);
            var intervalToLow = comparer.Equals(onInterval, lowInInterval);

            var currentIntervalToNextInterval = comparer.Equals(onInterval, nextInternal);
            var currentToPreviousInterval = comparer.Equals(onInterval, previousIntervalMaximum);

            // Assert

            Assert.True(highToLow);
            Assert.True(highToInterval);
            Assert.True(intervalToLow);

            Assert.False(currentIntervalToNextInterval);
            Assert.False(currentToPreviousInterval);
        }

        [Theory]
        [MemberData(nameof(UnitsCauseValuesToBeFlooredToPreviousInterval_MemberData))]
        public void GetHashCode_UnitsCauseValuesToBeFlooredToPreviousInterval(
            DateTime onInterval,
            DateTime highInInterval,
            DateTime lowInInterval,
            DateTimeUnit granularity) {

            // Arrange

            var comparer = new KindSensitiveDateTimeEqualityComparer(granularity);
            var ticksPerUnit = DateTimeUtilities.GetTicksPerUnit(granularity);

            var nextInternal = onInterval.AddTicks(ticksPerUnit);
            var previousIntervalMaximum = onInterval.AddTicks(-1L);

            // Act

            var onIntervalHashCode = comparer.GetHashCode(lowInInterval);
            var highInIntervalHashCode = comparer.GetHashCode(highInInterval);
            var lowInIntervalHashCode = comparer.GetHashCode(lowInInterval);

            // Assert

            Assert.Equal(onIntervalHashCode, highInIntervalHashCode);
            Assert.Equal(onIntervalHashCode, lowInIntervalHashCode);
        }

    }

}
