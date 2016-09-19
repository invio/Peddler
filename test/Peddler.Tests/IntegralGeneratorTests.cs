using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Peddler {

    public abstract class IntegralGeneratorTests<TIntegral>
        where TIntegral : struct, IEquatable<TIntegral>, IComparable<TIntegral> {

        private const int numberOfAttempts = 100;

        private static TIntegral minValue;
        private static TIntegral maxValue;

        static IntegralGeneratorTests() {
            minValue = (TIntegral)
                typeof(TIntegral)
                    .GetField("MinValue", BindingFlags.Public | BindingFlags.Static)
                    .GetValue(null);

            maxValue = (TIntegral)
                typeof(TIntegral)
                    .GetField("MaxValue", BindingFlags.Public | BindingFlags.Static)
                    .GetValue(null);
        }

        private static TIntegral ToIntegral(Object value) {
            if (typeof(TIntegral) == typeof(DateTime)) {
                // DateTime integral value is ticks', which isn't supported
                // Convert.ChangeType.
                value = new DateTime(
                    (Int64)Convert.ChangeType(value, typeof(Int64)),
                    DateTimeKind.Unspecified
                );
            }

            return (TIntegral)Convert.ChangeType(value, typeof(TIntegral));
        }

        private static TIntegral Add(TIntegral value, long amount) {
            checked {
                // Of all integrals, only UInt64 cannot be converted to Int64
                if (typeof(TIntegral) == typeof(UInt64)) {
                    if (amount < 0) {
                        return ToIntegral(Convert.ToUInt64(value) - (UInt64)Math.Abs(amount));
                    } else {
                        return ToIntegral(Convert.ToUInt64(value) + (UInt64)amount);
                    }
                }

                if (typeof(TIntegral) == typeof(DateTime)) {

                    return ToIntegral(((DateTime)(object)value).Ticks + amount);
                }

                return ToIntegral(Convert.ToInt64(value) + amount);
            }
        }

        protected abstract IIntegralGenerator<TIntegral> CreateGenerator();
        protected abstract IIntegralGenerator<TIntegral> CreateGenerator(TIntegral low);
        protected abstract IIntegralGenerator<TIntegral> CreateGenerator(TIntegral low, TIntegral high);

        [Fact]
        public void Constructor_WithLow_CannotBeMaxIntegralValue() {
            Assert.Throws<ArgumentException>(
                () => this.CreateGenerator(maxValue)
            );
        }

        public static IEnumerable<object[]> Constructor_WithLowAndHigh_LowMustBeLessThanHigh_Data {
            get {
                yield return new object[] { Add(minValue, 1), minValue };
                yield return new object[] { maxValue, minValue };
                yield return new object[] { minValue, minValue };
                yield return new object[] { ToIntegral(0), ToIntegral(0) };
                yield return new object[] { maxValue, maxValue };
            }
        }

        [Theory]
        [MemberData(nameof(Constructor_WithLowAndHigh_LowMustBeLessThanHigh_Data))]
        public void Constructor_WithLowAndHigh_LowMustBeLessThanHigh(
            TIntegral low,
            TIntegral high) {

            Assert.Throws<ArgumentException>(
                () => this.CreateGenerator(low, high)
            );
        }

        [Fact]
        public void Next_WithDefaults_RangeIsZeroToMaxIntegralValue() {
            var generator = this.CreateGenerator();

            Assert.Equal(generator.Low, ToIntegral(0));
            Assert.Equal(generator.High, maxValue);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                AssertGreaterThanOrEqualTo(value, generator.Low);
                AssertLessThan(value, generator.High);
                Assert.True(generator.EqualityComparer.Equals(value, value));
            }
        }

        public static IEnumerable<object[]> Next_WithLowDefined_RangeIsLowToMaxIntegralValue_Data {
            get {
                yield return new object[] { minValue };
                yield return new object[] { ToIntegral(0) };
                yield return new object[] { Add(maxValue, -1) };
            }
        }

        [Theory]
        [MemberData(nameof(Next_WithLowDefined_RangeIsLowToMaxIntegralValue_Data))]
        public void Next_WithLowDefined_RangeIsLowToMaxIntegralValue(TIntegral low) {
            var generator = this.CreateGenerator(low);

            Assert.Equal(generator.Low, low);
            Assert.Equal(generator.High, maxValue);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                AssertGreaterThanOrEqualTo(value, low);
                AssertLessThan(value, generator.High);
                Assert.True(generator.EqualityComparer.Equals(value, value));
            }
        }

        public static IEnumerable<object[]> Next_WithLowAndHighDefined_RangeIsBetweenLowAndHigh_Data {
            get {
                yield return new object[] { minValue, maxValue };
                yield return new object[] { minValue, ToIntegral(1) };
                yield return new object[] { ToIntegral(0), maxValue };
            }
        }

        [Theory]
        [MemberData(nameof(Next_WithLowAndHighDefined_RangeIsBetweenLowAndHigh_Data))]
        public void Next_WithLowAndHighDefined_RangeIsBetweenLowAndHigh(
            TIntegral low,
            TIntegral high) {

            var generator = this.CreateGenerator(low, high);

            Assert.Equal(generator.Low, low);
            Assert.Equal(generator.High, high);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                AssertGreaterThanOrEqualTo(value, low);
                AssertLessThan(value, high);
                Assert.True(generator.EqualityComparer.Equals(value, value));
            }
        }

        public static IEnumerable<object[]> NextDistinct_NeverGetSameValue_Data {
            get {
                yield return new object[] { minValue, maxValue };
                yield return new object[] { ToIntegral(0), ToIntegral(2) };
            }
        }

        [Theory]
        [MemberData(nameof(NextDistinct_NeverGetSameValue_Data))]
        public void NextDistinct_NeverGetSameValue(TIntegral low, TIntegral high) {
            var generator = this.CreateGenerator(low, high);
            var previousValue = generator.Next();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var nextValue = generator.NextDistinct(previousValue);
                Assert.NotEqual(previousValue, nextValue);
                Assert.False(generator.EqualityComparer.Equals(previousValue, nextValue));
                previousValue = nextValue;
            }
        }

        [Fact]
        public void NextDistinct_ThrowOnConstantGenerator() {
            // With these arguments, IntegralGenerator<TIntegral> can only generate '0'
            var generator = this.CreateGenerator(ToIntegral(0), ToIntegral(1));
            var value = generator.Next();

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextDistinct(value)
            );
        }

        [Fact]
        public void NextDistinct_OtherGreaterThanRange() {
            var generator = this.CreateGenerator(ToIntegral(0), ToIntegral(10));

            var other = ToIntegral(20);
            AssertGreaterThan(other, generator.High);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(other);

                AssertGreaterThanOrEqualTo(value, generator.Low);
                AssertLessThan(value, generator.High);
                Assert.False(generator.EqualityComparer.Equals(other, value));
            }
        }

        [Fact]
        public void NextDistinct_OtherLessThanRange() {
            var generator = this.CreateGenerator(ToIntegral(10), ToIntegral(20));

            var other = ToIntegral(5);
            AssertLessThan(other, generator.Low);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(other);

                AssertGreaterThanOrEqualTo(value, generator.Low);
                AssertLessThan(value, generator.High);
                Assert.False(generator.EqualityComparer.Equals(other, value));
            }
        }

        public static IEnumerable<object[]> NextGreaterThan_ThrowOnMaxValueOrHigher_Data {
            get {
                yield return new object[] { ToIntegral(0), ToIntegral(10), ToIntegral(20) };
                yield return new object[] { ToIntegral(0), ToIntegral(10), ToIntegral(10) };
                yield return new object[] { ToIntegral(0), ToIntegral(10), ToIntegral(9) };
                yield return new object[] { ToIntegral(0), maxValue, maxValue };
            }
        }

        [Theory]
        [MemberData(nameof(NextGreaterThan_ThrowOnMaxValueOrHigher_Data))]
        public void NextGreaterThan_ThrowOnMaxValueOrHigher(
            TIntegral low,
            TIntegral high,
            TIntegral other) {

            var generator = this.CreateGenerator(low, high);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextGreaterThan(other)
            );
        }

        public static IEnumerable<object[]> NextGreaterThan_Data {
            get {
                yield return new object[] { minValue, maxValue, minValue };
                yield return new object[] { ToIntegral(0), ToIntegral(2), ToIntegral(0) };
                yield return new object[] { ToIntegral(10), ToIntegral(40), ToIntegral(5) };
            }
        }

        [Theory]
        [MemberData(nameof(NextGreaterThan_Data))]
        public void NextGreaterThan(
            TIntegral low,
            TIntegral high,
            TIntegral other) {

            var generator = this.CreateGenerator(low, high);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextGreaterThan(other);

                AssertGreaterThan(value, other);
                AssertGreaterThanOrEqualTo(value, generator.Low);
                AssertLessThan(value, generator.High);
                Assert.True(generator.Comparer.Compare(other, value) < 0);
            }
        }

        public static IEnumerable<object[]> NextGreaterThanOrEqualTo_ThrowOnHigherThanMaxValue_Data {
            get {
                yield return new object[] { ToIntegral(1), ToIntegral(2), ToIntegral(10) };
                yield return new object[] { ToIntegral(0), ToIntegral(10), ToIntegral(10) };
                yield return new object[] { minValue, maxValue, maxValue };
            }
        }

        [Theory]
        [MemberData(nameof(NextGreaterThanOrEqualTo_ThrowOnHigherThanMaxValue_Data))]
        public void NextGreaterThanOrEqualTo_ThrowOnHigherThanMaxValue(
            TIntegral low,
            TIntegral high,
            TIntegral other) {

            var generator = this.CreateGenerator(low, high);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextGreaterThanOrEqualTo(other)
            );
        }

        public static IEnumerable<object[]> NextGreaterThanOrEqualTo_Data {
            get {
                yield return new object[] { ToIntegral(1), ToIntegral(2), ToIntegral(0) };
                yield return new object[] { ToIntegral(0), ToIntegral(10), ToIntegral(9) };
                yield return new object[] { minValue, maxValue, minValue };
                yield return new object[] { ToIntegral(50), ToIntegral(100), ToIntegral(30) };
            }
        }

        [Theory]
        [MemberData(nameof(NextGreaterThanOrEqualTo_Data))]
        public void NextGreaterThanOrEqualTo(
            TIntegral low,
            TIntegral high,
            TIntegral other) {

            var generator = this.CreateGenerator(low, high);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextGreaterThanOrEqualTo(other);

                AssertGreaterThanOrEqualTo(value, other);
                AssertGreaterThanOrEqualTo(value, generator.Low);
                AssertLessThan(value, generator.High);
                Assert.True(generator.Comparer.Compare(other, value) <= 0);
            }
        }

        public static IEnumerable<object[]> NextLessThan_ThrowOnMinValueOrLower_Data {
            get {
                yield return new object[] { ToIntegral(1), ToIntegral(2), ToIntegral(0) };
                yield return new object[] { ToIntegral(0), ToIntegral(10), ToIntegral(0) };
                yield return new object[] { minValue, maxValue, minValue };
            }
        }

        [Theory]
        [MemberData(nameof(NextLessThan_ThrowOnMinValueOrLower_Data))]
        public void NextLessThan_ThrowOnMinValueOrLower(
            TIntegral low,
            TIntegral high,
            TIntegral other) {

            var generator = this.CreateGenerator(low, high);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextLessThan(other)
            );
        }

        public static IEnumerable<object[]> NextLessThan_Data {
            get {
                yield return new object[] { minValue, maxValue, maxValue };
                yield return new object[] { ToIntegral(0), ToIntegral(2), ToIntegral(1) };
                yield return new object[] { ToIntegral(0), ToIntegral(10), ToIntegral(40) };
            }
        }

        [Theory]
        [MemberData(nameof(NextLessThan_Data))]
        public void NextLessThan(
            TIntegral low,
            TIntegral high,
            TIntegral other) {

            var generator = this.CreateGenerator(low, high);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextLessThan(other);

                AssertLessThan(value, other);
                AssertGreaterThanOrEqualTo(value, generator.Low);
                AssertLessThan(value, generator.High);
                Assert.True(generator.Comparer.Compare(other, value) > 0);
            }
        }

        public static IEnumerable<object[]> NextLessThanOrEqualTo_ThrowOnLessThanMinValue_Data {
            get {
                yield return new object[] { ToIntegral(10), ToIntegral(20), ToIntegral(0) };
                yield return new object[] { ToIntegral(30), ToIntegral(50), ToIntegral(10) };
                yield return new object[] { Add(minValue, 1), maxValue, minValue };
            }
        }

        [Theory]
        [MemberData(nameof(NextLessThanOrEqualTo_ThrowOnLessThanMinValue_Data))]
        public void NextLessThanOrEqualTo_ThrowOnLessThanMinValue(
            TIntegral low,
            TIntegral high,
            TIntegral other) {

            var generator = this.CreateGenerator(low, high);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextLessThanOrEqualTo(other)
            );
        }

        public static IEnumerable<object[]> NextLessThanOrEqualTo_Data {
            get {
                yield return new object[] { ToIntegral(1), ToIntegral(2), ToIntegral(3) };
                yield return new object[] { ToIntegral(0), ToIntegral(10), ToIntegral(0) };
                yield return new object[] { minValue, maxValue, maxValue };
                yield return new object[] { ToIntegral(0), ToIntegral(10), ToIntegral(40) };
            }
        }

        [Theory]
        [MemberData(nameof(NextLessThanOrEqualTo_Data))]
        public void NextLessThanOrEqualTo(
            TIntegral low,
            TIntegral high,
            TIntegral other) {

            var generator = this.CreateGenerator(low, high);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextLessThanOrEqualTo(other);

                AssertLessThanOrEqualTo(value, other);
                AssertGreaterThanOrEqualTo(value, generator.Low);
                AssertLessThan(value, generator.High);
                Assert.True(generator.Comparer.Compare(other, value) >= 0);
            }
        }

        private static void AssertLessThan(TIntegral low, TIntegral high) {
            Assert.True(
                low.CompareTo(high) < 0,
                $"Expected '{low:N0}' to be less than '{high:N0}'."
            );
        }

        private static void AssertLessThanOrEqualTo(TIntegral low, TIntegral high) {
            Assert.True(
                low.CompareTo(high) <= 0,
                $"Expected '{low:N0}' to be less than or equal to '{high:N0}'."
            );
        }

        private static void AssertGreaterThan(TIntegral low, TIntegral high) {
            Assert.True(
                low.CompareTo(high) > 0,
                $"Expected '{low:N0}' to be greater than '{high:N0}'."
            );
        }

        private static void AssertGreaterThanOrEqualTo(TIntegral low, TIntegral high) {
            Assert.True(
                low.CompareTo(high) >= 0,
                $"Expected '{low:N0}' to be greater than or equal to '{high:N0}'."
            );
        }

    }

}
