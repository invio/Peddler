using System;
using System.Collections.Generic;
using Xunit;

namespace Peddler {

    public class MaybeDefaultDistinctGeneratorTests : MaybeDefaultGeneratorTests {

        protected sealed override IGenerator<T> MaybeDefault<T>(
            IComparableGenerator<T> inner) {

            return this.MaybeDefaultDistinct<T>(inner);
        }

        protected sealed override IGenerator<T> MaybeDefault<T>(
            IComparableGenerator<T> inner,
            decimal percentage) {

            return this.MaybeDefaultDistinct<T>(inner, percentage);
        }

        protected virtual IDistinctGenerator<T> MaybeDefaultDistinct<T>(
            IComparableGenerator<T> inner) {

            return new MaybeDefaultDistinctGenerator<T>(inner);
        }

        protected virtual IDistinctGenerator<T> MaybeDefaultDistinct<T>(
            IComparableGenerator<T> inner,
            decimal percentage) {

            return new MaybeDefaultDistinctGenerator<T>(inner, percentage);
        }

        public static IEnumerable<object[]> FakeGenerators {
            get {
                return new List<object[]> {
                    new object[] { new FakeStructGenerator(new Int32Generator(-10, -1)) },
                    new object[] { new FakeStructGenerator(new Int32Generator(-10, 10)) },
                    new object[] { new FakeStructGenerator(new Int32Generator(1, 10)) },
                    new object[] { new FakeStructGenerator(new Int32Generator(0, 2)) },
                    new object[] { new FakeClassGenerator() }
                };
            }
        }

        [Theory]
        [MemberData(nameof(FakeGenerators))]
        public void NextDistinct_InnerCanGenerateNonDefault(Object inner) {
            this.InvokeGenericMethod(
                nameof(NextDistinct_InnerCanGenerateNonDefaultImpl),
                inner
            );
        }

        public void NextDistinct_InnerCanGenerateNonDefaultImpl<T>(
            IComparableGenerator<T> inner) {

            var generator = this.MaybeDefaultDistinct<T>(inner);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(default(T));

                Assert.NotEqual(default(T), value);
                Assert.False(generator.EqualityComparer.Equals(default(T), value));
            }
        }

        public static IEnumerable<object[]> DefaultReturningGenerators {
            get {
                return new List<object[]> {
                    new object[] { new DefaultGenerator<Object>() },
                    new object[] { new DefaultGenerator<int>() },
                    new object[] { new DefaultGenerator<FakeClass>() },
                    new object[] { new DefaultGenerator<FakeStruct>() }
                };
            }
        }

        [Theory]
        [MemberData(nameof(DefaultReturningGenerators))]
        public void NextDistinct_InnerOnlyReturnsDefault(Object inner) {
            this.InvokeGenericMethod(
                nameof(NextDistinct_InnerOnlyReturnsDefaultImpl),
                inner
            );
        }

        public void NextDistinct_InnerOnlyReturnsDefaultImpl<T>(IComparableGenerator<T> inner) {
            var generator = this.MaybeDefaultDistinct<T>(inner);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextDistinct(default(T))
            );
        }

        public static IEnumerable<object[]> IgnoredPercentages {
            get {
                yield return new object[] { 0m };
                yield return new object[] { 0.5m };
                yield return new object[] { 1m };
            }
        }

        [Theory]
        [MemberData(nameof(IgnoredPercentages))]
        public void NextDistinct_InnerFailsButDefaultOk(decimal percentage) {
            var inner = new FakeStructGenerator(new Int32Generator(2, 3));
            var generator = this.MaybeDefaultDistinct(inner, percentage);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {

                // 2 is the only thing that can be returned by FakeStructGenerator,
                // but default (which is 0) is ok.

                var value = generator.NextDistinct(new FakeStruct { Value = 2 });

                Assert.Equal(default(FakeStruct), value);
                Assert.True(generator.EqualityComparer.Equals(default(FakeStruct), value));
            }
        }

        [Theory]
        [MemberData(nameof(FakeGenerators))]
        public void NextDistinct_FiftyPercentageOfDefault(Object inner) {
            this.InvokeGenericMethod(
                nameof(NextDistinct_FiftyPercentageOfDefaultImpl),
                inner
            );
        }

        public void NextDistinct_FiftyPercentageOfDefaultImpl<T>(
            IComparableGenerator<T> inner) {

            const decimal percentage = 0.5m;

            var generator = this.MaybeDefaultDistinct<T>(inner, percentage);
            var hasDefault = false;
            var hasNonDefault = false;

            for (var attempt = 0; attempt < extendedNumberOfAttempts; attempt++) {
                var value = generator.NextDistinct(generator.Next());

                if (!hasDefault) {
                    hasDefault = inner.EqualityComparer.Equals(value, default(T));
                }

                if (!hasNonDefault) {
                    hasNonDefault = !inner.EqualityComparer.Equals(value, default(T));
                }

                if (hasDefault && hasNonDefault) {
                    break;
                }
            }

            Assert.True(
                hasDefault && hasNonDefault,
                $"After {extendedNumberOfAttempts:N0} attempts with a {percentage * 100}% " +
                $"percentage chance of generating default values, the generator did not " +
                $"generate both a default and non-default value. The randomization approach " +
                $"is unbalanced."
            );
        }

    }

}
