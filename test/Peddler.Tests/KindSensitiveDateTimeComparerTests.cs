using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Peddler {

    public class KindSensitiveDateTimeComparerTests {

        private static IList<DateTimeKind> kinds { get; }
        private static IGenerator<Int64> tickGenerator { get; }

        static KindSensitiveDateTimeComparerTests() {
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
        public void Compare_KindMismatch(DateTimeKind leftKind, DateTimeKind rightKind) {
            var comparer = new KindSensitiveDateTimeComparer();
            var left = new DateTime(tickGenerator.Next(), leftKind);
            var right = new DateTime(tickGenerator.Next(), rightKind);

            var exception = Assert.Throws<ArgumentException>(
                () => comparer.Compare(left, right)
            );

            Assert.Equal(
                $"The DateTimeKind of 'left' ({left.Kind:G}) " +
                $"does not match the DateTimeKind of 'right' " +
                $"({right.Kind:G}), so they cannot be compared.",
                exception.Message
            );
        }

        [Theory]
        [MemberData(nameof(Kinds))]
        public void Compare_MatchingKinds(DateTimeKind kind) {
            var comparer = new KindSensitiveDateTimeComparer();
            var original = new DateTime(DateTime.MinValue.Ticks + 100, kind);

            var same = new DateTime(original.Ticks, kind);
            Assert.Equal(0, comparer.Compare(original, same));

            var later = new DateTime(original.Ticks + 1, kind);
            Assert.Equal(-1, comparer.Compare(original, later));

            var earlier = new DateTime(original.Ticks - 1, kind);
            Assert.Equal(1, comparer.Compare(original, earlier));
        }

    }

}
