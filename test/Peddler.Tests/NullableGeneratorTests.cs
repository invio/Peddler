using System;
using Xunit;

namespace Peddler {

    public class NullableGeneratorTests {

        protected const int numberOfAttempts = 100;

        protected virtual IGenerator<Nullable<T>> ToNullable<T>(IComparableGenerator<T> inner)
            where T : struct {

            return new NullableGenerator<T>(inner);
        }

        [Fact]
        public void Constructor_NullInnerGenerator() {
            IComparableGenerator<Int32> inner = null;

            Assert.Throws<ArgumentNullException>(
                () => this.ToNullable(inner)
            );
        }

        [Theory]
        [InlineData(-100, 0)]
        [InlineData(0, 100)]
        [InlineData(-100, 100)]
        public void Next(int low, int high) {
            Nullable<Int32> defaultValue = default(Nullable<Int32>);

            var inner = new Int32Generator(low, high);
            var generator = this.ToNullable(inner);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var nullable = generator.Next();

                Assert.NotEqual(nullable, defaultValue);
                Assert.True(nullable.Value >= inner.Low);
                Assert.True(nullable.Value < inner.High);
            }
        }

    }

}
