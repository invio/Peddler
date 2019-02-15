using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Invio.Xunit;
using Xunit;

namespace Peddler {

    [UnitTest]
    public sealed class DateTimeGeneratorTests : IntegralGeneratorTests<DateTime> {

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

        public static IEnumerable<object[]> Constructor_WithRange_ConsistentKinds_MemberData {
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
        [MemberData(nameof(Constructor_WithRange_ConsistentKinds_MemberData))]
        public void Constructor_WithRange_ConsistentKinds(DateTime low, DateTime high) {
            var generator = new DateTimeGenerator(low, high);

            Assert.Equal(low.Kind, generator.Kind);
            Assert.Equal(high.Kind, generator.Kind);
        }

        public static IEnumerable<object[]> Constructor_WithRange_MismatchedKinds_MemberData {
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
        [MemberData(nameof(Constructor_WithRange_MismatchedKinds_MemberData))]
        public void Constructor_WithRange_MismatchedKinds(DateTime low, DateTime high) {

            // Act

            var exception = Record.Exception(
                () => new DateTimeGenerator(low, high)
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);
        }

        public static IEnumerable<object[]> UnitsMemberData { get; } =
            Enumerable
                .Empty<DateTimeUnit>()
                .Append(DateTimeUnit.Tick)
                .Append(DateTimeUnit.Millisecond)
                .Append(DateTimeUnit.Second)
                .Append(DateTimeUnit.Minute)
                .Append(DateTimeUnit.Hour)
                .Append(DateTimeUnit.Day)
                .Select(unit => new object[] { unit })
                .ToImmutableList();

        [Theory]
        [MemberData(nameof(UnitsMemberData))]
        public void Constructor_WithGranularity_LowAndHighOnInternal(
            DateTimeUnit granularity) {

            // Act

            var generator = new DateTimeGenerator(granularity);

            // Assert

            AssertOnIntervalForGranularity(generator.Low, granularity);
            AssertOnIntervalForGranularity(generator.High, granularity);
        }

        [Theory]
        [MemberData(nameof(UnitsMemberData))]
        public void Next_AlwaysReturnsValuesOnGranularityUnitsInterval(
            DateTimeUnit granularity) {

            // Arrange

            var generator = new DateTimeGenerator(granularity);

            // Act

            var generated = generator.Next();

            // Assert

            AssertOnIntervalForGranularity(generated, granularity);
        }

        [Fact]
        public void NextDistinct_MismatchedKindStillReturnsCorrectKind() {

            // Arrange

            var date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            var generator = new DateTimeGenerator();

            // Act

            var generated = generator.NextDistinct(date);

            // Assert

            Assert.Equal(DateTimeKind.Local, date.Kind);
            Assert.Equal(DateTimeKind.Utc, generator.Kind);
            Assert.Equal(DateTimeKind.Utc, generated.Kind);
        }

        [Theory]
        [MemberData(nameof(UnitsMemberData))]
        public void NextDistinct_AlwaysReturnsValuesOnGranularityUnitsInterval(
            DateTimeUnit granularity) {

            // Arrange

            var generator = new DateTimeGenerator(granularity);

            // Act

            var generated = generator.NextDistinct(DateTime.UtcNow);

            // Assert

            AssertOnIntervalForGranularity(generated, granularity);
        }

        [Fact]
        public void NextLessThan_MismatchedKind() {

            // Arrange

            var date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            var generator = new DateTimeGenerator();

            // Act

            var exception = Record.Exception(
                () => generator.NextLessThan(date)
            );

            // Assert

            Assert.NotEqual(date.Kind, generator.Kind);
            Assert.IsType<ArgumentException>(exception);
        }

        [Theory]
        [MemberData(nameof(UnitsMemberData))]
        public void NextLessThan_AlwaysReturnsValuesOnGranularityUnitsInterval(
            DateTimeUnit granularity) {

            // Arrange

            var generator = new DateTimeGenerator(granularity);

            // Act

            var generated = generator.NextLessThan(DateTime.UtcNow);

            // Assert

            AssertOnIntervalForGranularity(generated, granularity);
        }

        [Fact]
        public void NextLessThanOrEqualTo_MismatchedKind() {

            // Arrange

            var generator = new DateTimeGenerator();
            var date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

            // Act

            var exception = Record.Exception(
                () => generator.NextLessThanOrEqualTo(date)
            );

            // Assert

            Assert.NotEqual(date.Kind, generator.Kind);
            Assert.IsType<ArgumentException>(exception);
        }

        [Theory]
        [MemberData(nameof(UnitsMemberData))]
        public void NextLessThanOrEqualTo_AlwaysReturnsValuesOnGranularityUnitsInterval(
            DateTimeUnit granularity) {

            // Arrange

            var generator = new DateTimeGenerator(granularity);

            // Act

            var generated = generator.NextLessThanOrEqualTo(DateTime.UtcNow);

            // Assert

            AssertOnIntervalForGranularity(generated, granularity);
        }

        [Fact]
        public void NextGreaterThan_MismatchedKind() {

            // Arrange

            var generator = new DateTimeGenerator();
            var date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

            // Act

            var exception = Record.Exception(
                () => generator.NextGreaterThan(date)
            );

            // Assert

            Assert.NotEqual(date.Kind, generator.Kind);
            Assert.IsType<ArgumentException>(exception);
        }

        [Theory]
        [MemberData(nameof(UnitsMemberData))]
        public void NextGreaterThan_AlwaysReturnsValuesOnGranularityUnitsInterval(
            DateTimeUnit granularity) {

            // Arrange

            var generator = new DateTimeGenerator(granularity);

            // Act

            var generated = generator.NextGreaterThan(DateTime.UtcNow);

            // Assert

            AssertOnIntervalForGranularity(generated, granularity);
        }

        [Fact]
        public void NextGreaterThanOrEqualTo_MismatchedKind() {

            // Arrange

            var date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            var generator = new DateTimeGenerator();

            // Act

            var exception = Record.Exception(
                () => generator.NextGreaterThanOrEqualTo(date)
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);
            Assert.NotEqual(date.Kind, generator.Kind);
        }

        [Theory]
        [MemberData(nameof(UnitsMemberData))]
        public void NextGreaterThanOrEqualTo_AlwaysReturnsValuesOnGranularityUnitsInterval(
            DateTimeUnit granularity) {

            // Arrange

            var generator = new DateTimeGenerator(granularity);

            // Act

            var generated = generator.NextGreaterThanOrEqualTo(DateTime.UtcNow);

            // Assert

            AssertOnIntervalForGranularity(generated, granularity);
        }

        [Fact]
        public void EqualityComparer_MismatchedKind() {

            // Arrange

            var now = DateTime.Now;
            var generator = new DateTimeGenerator();
            var comparer = generator.EqualityComparer;

            // Act

            var isEqual =
                comparer.Equals(
                    DateTime.SpecifyKind(now, DateTimeKind.Local),
                    DateTime.SpecifyKind(now, DateTimeKind.Utc)
                );

            // Assert

            Assert.False(isEqual);
        }

        [Theory]
        [InlineData(DateTimeKind.Utc)]
        public void EqualityComparer_RespectsGranularity(DateTimeKind kind) {

            // Arrange

            var value = new DateTime(2006, 07, 22, 05, 15, 45, kind);
            var generator = new DateTimeGenerator(DateTimeUnit.Hour);
            var comparer = generator.EqualityComparer;

            // Act

            var isEqual = comparer.Equals(value, new DateTime(2006, 07, 22, 05, 45, 05, kind));

            // Assert

            Assert.True(isEqual);
        }

        [Fact]
        public void Comparer_MismatchedKind() {

            // Arrange

            var now = DateTime.Now;
            var generator = new DateTimeGenerator();
            var comparer = generator.Comparer;

            // Act

            var exception = Record.Exception(
                () => comparer.Compare(
                    DateTime.SpecifyKind(now, DateTimeKind.Local),
                    DateTime.SpecifyKind(now, DateTimeKind.Utc)
                )
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);
        }

        [Theory]
        [InlineData(DateTimeKind.Utc)]
        public void Comparer_RespectsGranulatiry(DateTimeKind kind) {

            // Arrange

            var value = new DateTime(2006, 07, 22, 05, 15, 45, kind);
            var generator = new DateTimeGenerator(DateTimeUnit.Minute);
            var comparer = generator.Comparer;

            // Act

            var comparison = comparer.Compare(
                value,
                new DateTime(2006, 07, 22, 05, 15, 15, kind)
            );

            // Assert

            Assert.Equal(0, comparison);
        }

        private static void AssertOnIntervalForGranularity(
            DateTime generated,
            DateTimeUnit granularity) {

            var isHoursZeroed = false;
            var isMinutesZeroed = false;
            var isSecondsZeroed = false;
            var isMillisecondsZeroed = false;

            switch (granularity) {
                case DateTimeUnit.Day:
                    isHoursZeroed = true;
                    goto case DateTimeUnit.Hour;
                case DateTimeUnit.Hour:
                    isMinutesZeroed = true;
                    goto case DateTimeUnit.Minute;
                case DateTimeUnit.Minute:
                    isSecondsZeroed = true;
                    goto case DateTimeUnit.Second;
                case DateTimeUnit.Second:
                    isMillisecondsZeroed = true;
                    goto case DateTimeUnit.Millisecond;
                case DateTimeUnit.Millisecond:
                    break;
                case DateTimeUnit.Tick:
                    return;
                default:
                    throw new NotSupportedException(
                        $"The {nameof(DateTimeUnit)} '{granularity}' is not supported."
                    );
            }

            Assert.Equal(
                new DateTime(
                    generated.Year,
                    generated.Month,
                    generated.Day,
                    isHoursZeroed ? 0 : generated.Hour,
                    isMinutesZeroed ? 0 : generated.Minute,
                    isSecondsZeroed ? 0 : generated.Second,
                    isMillisecondsZeroed ? 0 : generated.Millisecond,
                    generated.Kind
                ),
                generated
            );
        }

    }

}
