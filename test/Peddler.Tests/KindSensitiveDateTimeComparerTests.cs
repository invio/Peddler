using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Invio.Xunit;
using Xunit;

namespace Peddler {

    [UnitTest]
    public sealed class KindSensitiveDateTimeComparerTests {

        private static IList<DateTimeUnit> units { get; }
        private static IList<DateTimeKind> kinds { get; }
        private static IGenerator<Int64> tickGenerator { get; }

        static KindSensitiveDateTimeComparerTests() {
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

            tickGenerator = new Int64Generator(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks);
        }

        public static IEnumerable<object[]> Compare_MismatchedKinds_MemberData {
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
        [MemberData(nameof(Compare_MismatchedKinds_MemberData))]
        public void Compare_KindMismatch(DateTimeKind leftKind, DateTimeKind rightKind) {

            // Arrange

            var comparer = new KindSensitiveDateTimeComparer();
            var left = new DateTime(tickGenerator.Next(), leftKind);
            var right = new DateTime(tickGenerator.Next(), rightKind);

            // Act

            var exception = Record.Exception(
                () => comparer.Compare(left, right)
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);

            Assert.Equal(
                $"The DateTimeKind of 'left' ({left.Kind:G}) " +
                $"does not match the DateTimeKind of 'right' " +
                $"({right.Kind:G}), so they cannot be compared.",
                exception.Message
            );
        }

        public static IEnumerable<object[]> Compare_MatchingKinds_MemberData {
            get {
                foreach (var kind in kinds) {
                    foreach (var unit in units) {
                        yield return new object[] { kind, unit };
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(Compare_MatchingKinds_MemberData))]
        public void Compare_MatchingKinds(
            DateTimeKind kind,
            DateTimeUnit granularity) {

            // Arrange

            var comparer = new KindSensitiveDateTimeComparer(granularity);
            var original = new DateTime(DateTime.UtcNow.Ticks - 100, kind);
            var ticksPerUnit = DateTimeUtilities.GetTicksPerUnit(granularity);

            // Act

            var same = new DateTime(original.Ticks, kind);
            var later = new DateTime(original.Ticks + ticksPerUnit, kind);
            var earlier = new DateTime(original.Ticks - ticksPerUnit, kind);

            // Assert

            Assert.Equal(0, comparer.Compare(original, same));
            Assert.Equal(-1, comparer.Compare(original, later));
            Assert.Equal(1, comparer.Compare(original, earlier));
        }

        public static IEnumerable<object[]> Compare_UnitsCauseValuesToBeFlooredToPreviousInterval_MemberData {
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
        [MemberData(nameof(Compare_UnitsCauseValuesToBeFlooredToPreviousInterval_MemberData))]
        public void Compare_UnitsCauseValuesToBeFlooredToPreviousInterval(
            DateTime onInterval,
            DateTime highInInterval,
            DateTime lowInInterval,
            DateTimeUnit granularity) {

            // Arrange

            var comparer = new KindSensitiveDateTimeComparer(granularity);
            var ticksPerUnit = DateTimeUtilities.GetTicksPerUnit(granularity);

            var nextInternal = onInterval.AddTicks(ticksPerUnit);
            var previousIntervalMaximum = onInterval.AddTicks(-1L);

            // Act

            var highToLow = comparer.Compare(highInInterval, lowInInterval);
            var highToInterval = comparer.Compare(highInInterval, onInterval);
            var intervalToLow = comparer.Compare(onInterval, lowInInterval);

            var currentIntervalToNextInterval = comparer.Compare(onInterval, nextInternal);
            var currentToPreviousInterval = comparer.Compare(onInterval, previousIntervalMaximum);

            // Assert

            Assert.Equal(0, highToLow);
            Assert.Equal(0, highToInterval);
            Assert.Equal(0, intervalToLow);

            Assert.Equal(-1, currentIntervalToNextInterval);
            Assert.Equal(1, currentToPreviousInterval);
        }

    }

}
