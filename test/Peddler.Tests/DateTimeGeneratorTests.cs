using System;
using System.Collections.Generic;
using Xunit;

namespace Peddler {

    public class DateTimeGeneratorTests : IntegralGeneratorTests<DateTime> {

        protected override IIntegralGenerator<DateTime> CreateGenerator() {
            return new DateTimeGenerator();
        }

        protected override IIntegralGenerator<DateTime> CreateGenerator(DateTime low) {
            return new DateTimeGenerator(low);
        }

        protected override IIntegralGenerator<DateTime> CreateGenerator(
            DateTime low,
            DateTime high) {

            return new DateTimeGenerator(low, high);
        }

        [Fact]
        public void Constructor_WithDefaults_KindIsUtc() {
            var generator = new DateTimeGenerator();

            Assert.Equal(DateTimeKind.Utc, generator.Kind);
        }

        public static IEnumerable<object[]> Constructor_WithLow_KindIsInferred_Data {
            get {
                var ticks = DateTime.Now.Ticks;

                yield return new object[] { new DateTime(ticks, DateTimeKind.Unspecified) };
                yield return new object[] { new DateTime(ticks, DateTimeKind.Local) };
                yield return new object[] { new DateTime(ticks, DateTimeKind.Utc) };
            }
        }

        [Theory]
        [MemberData(nameof(Constructor_WithLow_KindIsInferred_Data))]
        public void Constructor_WithLow_KindIsInferred(DateTime date) {
            var generator = new DateTimeGenerator(date);

            Assert.Equal(date.Kind, generator.Kind);
        }

        public static IEnumerable<object[]> Constructor_WithRange_ConsistentKinds_Data {
            get {
                var ticks = DateTime.Now.Ticks;

                yield return new object[] {
                    new DateTime(ticks, DateTimeKind.Unspecified),
                    new DateTime(ticks + 1, DateTimeKind.Unspecified)
                };

                yield return new object[] {
                    new DateTime(ticks, DateTimeKind.Local),
                    new DateTime(ticks + 1, DateTimeKind.Local)
                };

                yield return new object[] {
                    new DateTime(ticks, DateTimeKind.Utc),
                    new DateTime(ticks + 1, DateTimeKind.Utc)
                };
            }
        }

        [Theory]
        [MemberData(nameof(Constructor_WithRange_ConsistentKinds_Data))]
        public void Constructor_WithRange_ConsistentKinds(DateTime low, DateTime high) {
            var generator = new DateTimeGenerator(low, high);

            Assert.Equal(low.Kind, generator.Kind);
            Assert.Equal(high.Kind, generator.Kind);
        }

        public static IEnumerable<object[]> Constructor_WithRange_MismatchedKinds_Data {
            get {
                var ticks = DateTime.Now.Ticks;

                yield return new object[] {
                    new DateTime(ticks, DateTimeKind.Unspecified),
                    new DateTime(ticks + 1, DateTimeKind.Local)
                };

                yield return new object[] {
                    new DateTime(ticks, DateTimeKind.Local),
                    new DateTime(ticks + 1, DateTimeKind.Utc)
                };

                yield return new object[] {
                    new DateTime(ticks, DateTimeKind.Utc),
                    new DateTime(ticks + 1, DateTimeKind.Unspecified)
                };
            }
        }

        [Theory]
        [MemberData(nameof(Constructor_WithRange_MismatchedKinds_Data))]
        public void Constructor_WithRange_MismatchedKinds(DateTime low, DateTime high) {
            Assert.Throws<ArgumentException>(
                () => new DateTimeGenerator(low, high)
            );
        }

        [Fact]
        public void NextDistinct_MismatchedKind() {
            var date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            var generator = new DateTimeGenerator();

            Assert.NotEqual(date.Kind, generator.Kind);
            Assert.Throws<ArgumentException>(
                () => generator.NextDistinct(date)
            );
        }

        [Fact]
        public void NextLessThan_MismatchedKind() {
            var date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            var generator = new DateTimeGenerator();

            Assert.NotEqual(date.Kind, generator.Kind);
            Assert.Throws<ArgumentException>(
                () => generator.NextLessThan(date)
            );
        }

        [Fact]
        public void NextLessThanOrEqualTo_MismatchedKind() {
            var date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            var generator = new DateTimeGenerator();

            Assert.NotEqual(date.Kind, generator.Kind);
            Assert.Throws<ArgumentException>(
                () => generator.NextLessThanOrEqualTo(date)
            );
        }

        [Fact]
        public void NextGreaterThan_MismatchedKind() {
            var date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            var generator = new DateTimeGenerator();

            Assert.NotEqual(date.Kind, generator.Kind);
            Assert.Throws<ArgumentException>(
                () => generator.NextGreaterThan(date)
            );
        }

        [Fact]
        public void NextGreaterThanOrEqualTo_MismatchedKind() {
            var date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            var generator = new DateTimeGenerator();

            Assert.NotEqual(date.Kind, generator.Kind);
            Assert.Throws<ArgumentException>(
                () => generator.NextGreaterThanOrEqualTo(date)
            );
        }

        [Fact]
        public void EqualityComparer_MismatchedKind() {
            var date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            var generator = new DateTimeGenerator();

            Assert.NotEqual(date.Kind, generator.Kind);

            var comparer = generator.EqualityComparer;
            Assert.Throws<ArgumentException>(
                () => generator.EqualityComparer.Equals(date, date)
            );

            Assert.Throws<ArgumentException>(
                () => generator.EqualityComparer.GetHashCode(date)
            );
        }

        [Fact]
        public void Comparer_MismatchedKind() {
            var date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            var generator = new DateTimeGenerator();

            Assert.NotEqual(date.Kind, generator.Kind);

            var comparer = generator.EqualityComparer;
            Assert.Throws<ArgumentException>(
                () => generator.Comparer.Compare(date, date)
            );
        }

    }

}
