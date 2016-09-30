using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Peddler {

    public class KindSensitiveDateTimeEqualityComparerTests {

        private static IList<DateTimeKind> kinds { get; }
        private static IGenerator<Int64> tickGenerator { get; }

        static KindSensitiveDateTimeEqualityComparerTests() {
            kinds = new DateTimeKind[] {
                DateTimeKind.Unspecified,
                DateTimeKind.Local,
                DateTimeKind.Utc
            };

            tickGenerator = new Int64Generator(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks);
        }

        public static IEnumerable<object[]> Kinds {
            get {
                foreach (var kind in kinds) {
                    yield return new object[] { kind };
                }
            }
        }

        public static IEnumerable<object[]> MismatchedKinds {
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
        [MemberData(nameof(MismatchedKinds))]
        public void Equals_KindMismatch(DateTimeKind leftKind, DateTimeKind rightKind) {
            var ticks = tickGenerator.Next();
            var comparer = new KindSensitiveDateTimeEqualityComparer();

            var left = new DateTime(ticks, leftKind);
            var right = new DateTime(ticks, rightKind);

            Assert.False(comparer.Equals(left, right));
        }

        [Theory]
        [MemberData(nameof(Kinds))]
        public void Equals_MatchingKinds(DateTimeKind kind) {
            var comparer = new KindSensitiveDateTimeEqualityComparer();
            var original = new DateTime(DateTime.MinValue.Ticks, kind);

            var same = new DateTime(DateTime.MinValue.Ticks, kind);
            Assert.True(comparer.Equals(original, same));

            var different = new DateTime(DateTime.MinValue.Ticks + 1, kind);
            Assert.False(comparer.Equals(original, different));
        }

        [Theory]
        [MemberData(nameof(Kinds))]
        public void GetHashCode_MatchingKinds(DateTimeKind kind) {
            var ticks = tickGenerator.Next();
            var comparer = new KindSensitiveDateTimeEqualityComparer();

            var left = new DateTime(ticks, kind);
            var right = new DateTime(ticks, kind);

            Assert.Equal(comparer.GetHashCode(left), comparer.GetHashCode(right));
        }

    }

}
