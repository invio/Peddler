using System;
using System.Collections.Generic;
using Xunit;

namespace Peddler {

    public class MaybeDefaultGeneratorTests {

        private const int numberOfAttempts = 100;

        protected virtual IGenerator<T> MaybeDefault<T>(IGenerator<T> inner) {
            return new MaybeDefaultGenerator<T>(inner);
        }

        protected virtual IGenerator<T> MaybeDefault<T>(IGenerator<T> inner, decimal percentage) {
            return new MaybeDefaultGenerator<T>(inner, percentage);
        }

        [Fact]
        public void Constructor_NullInnerGenerator() {
            IGenerator<Object> inner = null;

            Assert.Throws<ArgumentNullException>(
                () => this.MaybeDefault(inner)
            );

            Assert.Throws<ArgumentNullException>(
                () => this.MaybeDefault(inner, 0.5m)
            );
        }

        public static IEnumerable<object[]> Constructor_PercentageBelowZero_Data {
            get {
                yield return new object[] { -0.1m };
                yield return new object[] { -1m };
                yield return new object[] { Decimal.MinValue };
            }
        }

        [Theory]
        [MemberData(nameof(Constructor_PercentageBelowZero_Data))]
        public void Constructor_PercentageBelowZero(decimal percentage) {
            var inner = new ObjectGenerator();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => this.MaybeDefault(inner, percentage)
            );
        }

        public static IEnumerable<object[]> Constructor_PercentageAboveOne_Data {
            get {
                yield return new object[] { 1.1m };
                yield return new object[] { 2m };
                yield return new object[] { Decimal.MaxValue };
            }
        }

        [Theory]
        [MemberData(nameof(Constructor_PercentageAboveOne_Data))]
        public void Constructor_PercentageAboveOne(decimal percentage) {
            var inner = new ObjectGenerator();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => this.MaybeDefault(inner, percentage)
            );
        }

        [Fact]
        public void Next_ZeroPercentageOfDefault() {
            var generator = this.MaybeDefault(new ObjectGenerator(), 0m);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.NotEqual(null, generator.Next());
            }
        }

        [Fact]
        public void Next_OneHundredPercentageOfDefault() {
            var generator = this.MaybeDefault(new ObjectGenerator(), 1m);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.Equal(null, generator.Next());
            }
        }

        [Fact]
        public void Next_FiftyPercentageOfDefault() {
            const int maximumNumberOfAttempts = 1000;
            const decimal percentage = 0.5m;

            var generator = this.MaybeDefault(new ObjectGenerator(), percentage);
            var hasNull = false;
            var hasNonNull = false;

            for (var attempt = 0; attempt < maximumNumberOfAttempts; attempt++) {
                var value = generator.Next();

                hasNull = hasNull || (value == null);
                hasNonNull = hasNonNull || (value != null);

                if (hasNull && hasNonNull) {
                    break;
                }
            }

            Assert.True(
                hasNull && hasNonNull,
                $"After {maximumNumberOfAttempts:N0} attempts with a {percentage * 100}% " +
                $"percentage chance of generating default values, the generator did not " +
                $"generate both a default and non-default value. The randomization approach " +
                $"is unbalanced."
            );
        }

        public class ObjectGenerator : IGenerator<Object> {

            public Object Next() {
                return new Object();
            }

        }

    }

}
