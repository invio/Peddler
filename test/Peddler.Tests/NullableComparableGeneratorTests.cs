using System;
using Xunit;

namespace Peddler {

    // A word on principle as to why these tests enforce the behavior as it is:

    // IComparableGenerator<Nullable<T>> must never _randomly_ decide to inject a
    // Nullable<T> without a value. It can return a Nullable<T> it if that is the
    // only way to avoid an UnableToGenerateValueException, such as when a
    // Nullable<T> with a value is passed to LessThanOrEqualTo(), but it should
    // never be by chance.

    // If someone wishes to modify behavior such that null is randomly injected
    // from time to time, that is exactly what the MaybeDefault*Generators are for.
    // Wrap a NullableGenerator with that. Don't modify this implementation.

    public class NullableComparableGeneratorTests : NullableDistinctGeneratorTests {

        private static Nullable<Int32> defaultValue { get; } = default(Nullable<Int32>);

        protected sealed override IDistinctGenerator<Nullable<T>> ToNullableDistinct<T>(
            IComparableGenerator<T> inner) {

            return this.ToNullableComparable<T>(inner);
        }

        protected virtual IComparableGenerator<Nullable<T>> ToNullableComparable<T>(
            IComparableGenerator<T> inner) where T : struct {

            return new NullableComparableGenerator<T>(inner);
        }

        [Theory]
        [InlineData(Int32.MinValue)]
        [InlineData(0)]
        [InlineData(1)]
        public void NextLessThan_AtMinimumOfInner(int low) {
            var inner = new Int32Generator(low);
            Assert.Equal(low, inner.Low);

            var generator = this.ToNullableComparable(inner);
            var value = generator.NextLessThan(low);

            Assert.Equal(defaultValue, value);
            Assert.True(generator.EqualityComparer.Equals(defaultValue, value));
            Assert.Equal(0, generator.Comparer.Compare(defaultValue, value));
        }

        [Theory]
        [InlineData(Int32.MinValue + 1)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void NextLessThan_BelowMinimumOfInner(int low) {
            var inner = new Int32Generator(low);
            var generator = this.ToNullableComparable(inner);
            var value = generator.NextLessThan(new Nullable<Int32>(Int32.MinValue));

            Assert.Equal(defaultValue, value);
            Assert.True(generator.EqualityComparer.Equals(defaultValue, value));
            Assert.Equal(0, generator.Comparer.Compare(defaultValue, value));
        }

        [Fact]
        public void NextLessThan_Null() {
            var inner = new Int32Generator();
            var generator = this.ToNullableComparable(inner);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextLessThan(defaultValue)
            );
        }

        [Theory]
        [InlineData(200)]
        [InlineData(100)]
        [InlineData(0)]
        [InlineData(-100)]
        [InlineData(-199)]
        public void NextLessThan(int value) {
            var inner = new Int32Generator(-200, 200);
            var generator = this.ToNullableComparable(inner);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var original = new Nullable<Int32>(value);
                var result = generator.NextLessThan(original);

                Assert.False(generator.EqualityComparer.Equals(original, result));
                Assert.Equal(1, generator.Comparer.Compare(original, result));
                Assert.NotEqual(defaultValue, original);
                Assert.True(result.Value < original.Value);
                Assert.True(result.Value >= inner.Low);
            }
        }

        [Theory]
        [InlineData(Int32.MinValue)]
        [InlineData(0)]
        [InlineData(1)]
        public void NextLessThanOrEqualTo_AtMinimumOfInner(int low) {
            var inner = new Int32Generator(low);
            var generator = this.ToNullableComparable(inner);

            var original = new Nullable<Int32>(low);
            var result = generator.NextLessThanOrEqualTo(original);

            Assert.NotEqual(defaultValue, result);
            Assert.Equal(original, result);
            Assert.True(generator.EqualityComparer.Equals(original, result));
            Assert.Equal(0, generator.Comparer.Compare(original, result));
        }

        [Theory]
        [InlineData(Int32.MinValue + 1)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void NextLessThanOrEqualTo_BelowMinimumOfInner(int low) {
            var inner = new Int32Generator(low);
            var generator = this.ToNullableComparable(inner);
            var value = generator.NextLessThanOrEqualTo(new Nullable<Int32>(low - 1));

            Assert.Equal(defaultValue, value);
            Assert.True(generator.EqualityComparer.Equals(defaultValue, value));
            Assert.Equal(0, generator.Comparer.Compare(defaultValue, value));
        }

        [Fact]
        public void NextLessThanOrEqualTo_Null() {
            var generator = this.ToNullableComparable(new Int32Generator());
            var value = generator.NextLessThanOrEqualTo(defaultValue);

            Assert.Equal(defaultValue, value);
            Assert.True(generator.EqualityComparer.Equals(defaultValue, value));
            Assert.Equal(0, generator.Comparer.Compare(defaultValue, value));
        }

        [Theory]
        [InlineData(200)]
        [InlineData(100)]
        [InlineData(0)]
        [InlineData(-100)]
        [InlineData(-200)]
        public void NextLessThanOrEqualTo(int value) {
            var inner = new Int32Generator(-200, 200);
            var generator = this.ToNullableComparable(inner);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var original = new Nullable<Int32>(value);
                var result = generator.NextLessThanOrEqualTo(original);

                Assert.True(generator.Comparer.Compare(original, result) >= 0);
                Assert.NotEqual(defaultValue, original);
                Assert.True(result.Value <= original.Value);
                Assert.True(result.Value >= inner.Low);
            }
        }

        [Theory]
        [InlineData(Int32.MinValue, Int32.MaxValue)]
        [InlineData(Int32.MinValue, 0)]
        [InlineData(0, Int32.MaxValue)]
        public void NextGreaterThan_AtMaximumOfInner(int low, int high) {
            var inner = new Int32Generator(low, high);
            Assert.Equal(high, inner.High);

            var generator = this.ToNullableComparable(inner);
            var value = new Nullable<Int32>(high - 1);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextGreaterThan(value)
            );
        }

        [Theory]
        [InlineData(Int32.MinValue, Int32.MaxValue)]
        [InlineData(Int32.MinValue, 0)]
        [InlineData(0, Int32.MaxValue)]
        public void NextGreaterThan_AboveMaximumOfInner(int low, int high) {
            var inner = new Int32Generator(low, high);
            Assert.Equal(high, inner.High);

            var generator = this.ToNullableComparable(inner);
            var value = new Nullable<Int32>(high);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextGreaterThan(value)
            );
        }

        [Fact]
        public void NextGreaterThan_Null() {
            var generator = this.ToNullableComparable(new Int32Generator());
            var value = generator.NextGreaterThan(defaultValue);

            Assert.NotEqual(defaultValue, value);
            Assert.False(generator.EqualityComparer.Equals(defaultValue, value));
            Assert.Equal(-1, generator.Comparer.Compare(defaultValue, value));
        }

        [Theory]
        [InlineData(-200)]
        [InlineData(-100)]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(198)]
        public void NextGreaterThan(int value) {
            var inner = new Int32Generator(-200, 200);
            var generator = this.ToNullableComparable(inner);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var original = new Nullable<Int32>(value);
                var result = generator.NextGreaterThan(original);

                Assert.False(generator.EqualityComparer.Equals(original, result));
                Assert.Equal(-1, generator.Comparer.Compare(original, result));
                Assert.NotEqual(defaultValue, original);
                Assert.True(result.Value > original.Value);
                Assert.True(result.Value < inner.High);
            }
        }

        [Theory]
        [InlineData(Int32.MinValue, Int32.MaxValue)]
        [InlineData(Int32.MinValue, 0)]
        [InlineData(0, Int32.MaxValue)]
        public void NextGreaterThanOrEqualTo_AtMaximumOfInner(int low, int high) {
            var inner = new Int32Generator(low, high);
            Assert.Equal(high, inner.High);

            var generator = this.ToNullableComparable(inner);
            var original = new Nullable<Int32>(high - 1);
            var result = generator.NextGreaterThanOrEqualTo(original);

            Assert.NotEqual(defaultValue, result);
            Assert.Equal(result, original);
            Assert.True(generator.EqualityComparer.Equals(original, result));
            Assert.Equal(0, generator.Comparer.Compare(original, result));
        }

        [Theory]
        [InlineData(Int32.MinValue, Int32.MaxValue)]
        [InlineData(Int32.MinValue, 0)]
        [InlineData(0, Int32.MaxValue)]
        public void NextGreaterThanOrEqualTo_AboveMaximumOfInner(int low, int high) {
            var inner = new Int32Generator(low, high);
            Assert.Equal(high, inner.High);

            var generator = this.ToNullableComparable(inner);
            var value = new Nullable<Int32>(high);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextGreaterThanOrEqualTo(value)
            );
        }

        [Fact]
        public void NextGreaterThanOrEqualTo_Null() {
            var generator = this.ToNullableComparable(new Int32Generator());
            var value = generator.NextGreaterThanOrEqualTo(defaultValue);

            Assert.NotEqual(defaultValue, value);
            Assert.False(generator.EqualityComparer.Equals(defaultValue, value));
            Assert.Equal(-1, generator.Comparer.Compare(defaultValue, value));
        }

        [Theory]
        [InlineData(-200)]
        [InlineData(-100)]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(198)]
        [InlineData(199)]
        public void NextGreaterThanOrEqualTo(int value) {
            var inner = new Int32Generator(-200, 200);
            var generator = this.ToNullableComparable(inner);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var original = new Nullable<Int32>(value);
                var result = generator.NextGreaterThanOrEqualTo(original);

                Assert.True(generator.Comparer.Compare(original, result) <= 0);
                Assert.NotEqual(defaultValue, original);
                Assert.True(result.Value >= original.Value);
                Assert.True(result.Value < inner.High);
            }
        }

    }

}
