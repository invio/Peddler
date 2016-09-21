using System;
using Xunit;

namespace Peddler {

    public class NullableDistinctGeneratorTests : NullableGeneratorTests {

        protected sealed override IGenerator<Nullable<T>> ToNullable<T>(
            IComparableGenerator<T> inner) {

            return this.ToNullableDistinct<T>(inner);
        }

        protected virtual IDistinctGenerator<Nullable<T>> ToNullableDistinct<T>(
            IComparableGenerator<T> inner) where T : struct {

            return new NullableDistinctGenerator<T>(inner);
        }

        [Fact]
        public void NextDistinct_DefaultIsOK() {
            Nullable<Int32> defaultValue = default(Nullable<Int32>);

            var inner = new Int32Generator();
            var generator = this.ToNullableDistinct<Int32>(inner);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var nullable = generator.NextDistinct(default(Nullable<Int32>));

                Assert.NotEqual(nullable, defaultValue);
                Assert.False(generator.EqualityComparer.Equals(nullable, defaultValue));
                Assert.True(nullable.Value >= inner.Low);
                Assert.True(nullable.Value < inner.High);
            }
        }

        [Fact]
        public void NextDistinct_NeverDefaultOut() {
            Nullable<Int32> defaultValue = default(Nullable<Int32>);

            var inner = new Int32Generator();
            var generator = this.ToNullableDistinct<Int32>(inner);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var original = generator.Next();
                var nullable = generator.NextDistinct(original);

                Assert.NotEqual(nullable, defaultValue);
                Assert.False(generator.EqualityComparer.Equals(nullable, defaultValue));
                Assert.False(generator.EqualityComparer.Equals(original, nullable));
                Assert.True(nullable.Value >= inner.Low);
                Assert.True(nullable.Value < inner.High);
            }
        }

    }

}
