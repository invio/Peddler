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
                foreach (var comparerKind in kinds) {
                    foreach (var leftKind in kinds) {
                        foreach (var rightKind in kinds) {
                            if (leftKind == comparerKind && rightKind == comparerKind) {
                                continue;
                            }

                            yield return new object[] { comparerKind, leftKind, rightKind };
                        }
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(MismatchedKinds))]
        public void Equals_KindMismatch(
            DateTimeKind comparerKind,
            DateTimeKind leftKind,
            DateTimeKind rightKind) {

            var comparer = new KindSensitiveDateTimeComparer(comparerKind);
            var left = new DateTime(tickGenerator.Next(), leftKind);
            var right = new DateTime(tickGenerator.Next(), rightKind);

            var exception = Assert.Throws<ArgumentException>(
                () => comparer.Equals(left, right)
            );

            if (leftKind != comparerKind) {
                Assert.Equal(
                    $"The DateTimeKind of 'left' ({leftKind:G}) " +
                    $"does not match the DateTimeKind of this " +
                    $"KindSensitiveDateTimeComparer ({comparerKind:G})." +
                    $"{Environment.NewLine}Parameter name: left",
                    exception.Message
                );
            } else {
                Assert.Equal(
                    $"The DateTimeKind of 'right' ({rightKind:G}) " +
                    $"does not match the DateTimeKind of this " +
                    $"KindSensitiveDateTimeComparer ({comparerKind:G})." +
                    $"{Environment.NewLine}Parameter name: right",
                    exception.Message
                );
            }
        }

        [Theory]
        [MemberData(nameof(Kinds))]
        public void Equals_MatchingKinds(DateTimeKind kind) {
            var comparer = new KindSensitiveDateTimeComparer(kind);
            var original = new DateTime(DateTime.MinValue.Ticks, kind);

            var same = new DateTime(DateTime.MinValue.Ticks, kind);
            Assert.True(comparer.Equals(original, same));

            var different = new DateTime(DateTime.MinValue.Ticks + 1, kind);
            Assert.False(comparer.Equals(original, different));
        }

        [Theory]
        [InlineData(DateTimeKind.Unspecified, DateTimeKind.Utc)]
        [InlineData(DateTimeKind.Unspecified, DateTimeKind.Local)]
        [InlineData(DateTimeKind.Utc, DateTimeKind.Unspecified)]
        [InlineData(DateTimeKind.Utc, DateTimeKind.Local)]
        [InlineData(DateTimeKind.Local, DateTimeKind.Unspecified)]
        [InlineData(DateTimeKind.Local, DateTimeKind.Utc)]
        public void GetHashCode_KindMismatch(DateTimeKind comparerKind, DateTimeKind inputKind) {
            var comparer = new KindSensitiveDateTimeComparer(comparerKind);
            var input = new DateTime(tickGenerator.Next(), inputKind);

            var exception = Assert.Throws<ArgumentException>(
                () => comparer.GetHashCode(input)
            );

            Assert.Equal(
                $"The DateTimeKind of 'input' ({inputKind:G}) " +
                $"does not match the DateTimeKind of this " +
                $"KindSensitiveDateTimeComparer ({comparerKind:G})." +
                $"{Environment.NewLine}Parameter name: input",
                exception.Message
            );
        }

        [Theory]
        [MemberData(nameof(Kinds))]
        public void GetHashCode_MatchingKinds(DateTimeKind kind) {
            var comparer = new KindSensitiveDateTimeComparer(kind);
            var hashed = new DateTime(tickGenerator.Next(), kind);

            Assert.Equal(comparer.GetHashCode(hashed), hashed.GetHashCode());
        }


        [Theory]
        [MemberData(nameof(MismatchedKinds))]
        public void Compare_KindMismatch(
            DateTimeKind comparerKind,
            DateTimeKind leftKind,
            DateTimeKind rightKind) {

            var comparer = new KindSensitiveDateTimeComparer(comparerKind);
            var left = new DateTime(tickGenerator.Next(), leftKind);
            var right = new DateTime(tickGenerator.Next(), rightKind);

            var exception = Assert.Throws<ArgumentException>(
                () => comparer.Compare(left, right)
            );

            if (leftKind != comparerKind) {
                Assert.Equal(
                    $"The DateTimeKind of 'left' ({leftKind:G}) " +
                    $"does not match the DateTimeKind of this " +
                    $"KindSensitiveDateTimeComparer ({comparerKind:G})." +
                    $"{Environment.NewLine}Parameter name: left",
                    exception.Message
                );
            } else {
                Assert.Equal(
                    $"The DateTimeKind of 'right' ({rightKind:G}) " +
                    $"does not match the DateTimeKind of this " +
                    $"KindSensitiveDateTimeComparer ({comparerKind:G})." +
                    $"{Environment.NewLine}Parameter name: right",
                    exception.Message
                );
            }
        }

        [Theory]
        [MemberData(nameof(Kinds))]
        public void Compare_MatchingKinds(DateTimeKind kind) {
            var comparer = new KindSensitiveDateTimeComparer(kind);
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
