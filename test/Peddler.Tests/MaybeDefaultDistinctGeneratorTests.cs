using System;
using System.Collections.Generic;
using Xunit;

namespace Peddler {

    public class MaybeDefaultDistinctGeneratorTests : MaybeDefaultGeneratorTests {

        protected sealed override MaybeDefaultGenerator<T> MaybeDefault<T>(
            IComparableGenerator<T> inner) {

            return this.MaybeDefaultDistinct<T>(inner);
        }

        protected sealed override MaybeDefaultGenerator<T> MaybeDefault<T>(
            IComparableGenerator<T> inner,
            T defaultValue) {

            return this.MaybeDefaultDistinct<T>(inner, defaultValue);
        }

        protected sealed override MaybeDefaultGenerator<T> MaybeDefault<T>(
            IComparableGenerator<T> inner,
            decimal percentage) {

            return this.MaybeDefaultDistinct<T>(inner, percentage);
        }

        protected sealed override MaybeDefaultGenerator<T> MaybeDefault<T>(
            IComparableGenerator<T> inner,
            T defaultValue,
            decimal percentage) {

            return this.MaybeDefaultDistinct<T>(inner, defaultValue, percentage);
        }

        protected virtual MaybeDefaultDistinctGenerator<T> MaybeDefaultDistinct<T>(
            IComparableGenerator<T> inner) {

            var generator = new MaybeDefaultDistinctGenerator<T>(inner);
            Assert.Equal(default(T), generator.DefaultValue);
            return generator;
        }

        protected virtual MaybeDefaultDistinctGenerator<T> MaybeDefaultDistinct<T>(
            IComparableGenerator<T> inner,
            T defaultValue) {

            var generator = new MaybeDefaultDistinctGenerator<T>(inner, defaultValue);
            Assert.Equal(defaultValue, generator.DefaultValue);
            return generator;
        }

        protected virtual MaybeDefaultDistinctGenerator<T> MaybeDefaultDistinct<T>(
            IComparableGenerator<T> inner,
            decimal percentage) {

            var generator = new MaybeDefaultDistinctGenerator<T>(inner, percentage);
            Assert.Equal(default(T), generator.DefaultValue);
            return generator;
        }

        protected virtual MaybeDefaultDistinctGenerator<T> MaybeDefaultDistinct<T>(
            IComparableGenerator<T> inner,
            T defaultValue,
            decimal percentage) {

            var generator = new MaybeDefaultDistinctGenerator<T>(inner, defaultValue, percentage);
            Assert.Equal(defaultValue, generator.DefaultValue);
            return generator;
        }

        public static IEnumerable<object[]> FakeGenerators {
            get {
                return new List<object[]> {
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(-10, -1)),
                        new FakeStruct { Value = -200 }
                    },
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(-10, 10)),
                        new FakeStruct { Value = -100 }
                    },
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(1, 10)),
                        new FakeStruct { Value = -50 }
                    },
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(0, 2)),
                        new FakeStruct { Value = -10 }
                    },
                    new object[] {
                        new FakeClassGenerator(),
                        new FakeClass(-100)
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(FakeGenerators))]
        public void NextDistinct_InnerCanGenerateNonDefault(Object inner, Object defaultValue) {
            this.InvokeGenericMethod(
                nameof(NextDistinct_InnerCanGenerateNonDefaultImpl),
                inner,
                defaultValue
            );
        }

        public void NextDistinct_InnerCanGenerateNonDefaultImpl<T>(
            IComparableGenerator<T> inner,
            T defaultValue) {

            var autoDefaultGenerator = this.MaybeDefaultDistinct<T>(inner);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = autoDefaultGenerator.NextDistinct(default(T));

                Assert.NotEqual(default(T), value);
                Assert.False(
                    autoDefaultGenerator.EqualityComparer.Equals(default(T), value)
                );
            }

            var specificDefaultGenerator = this.MaybeDefaultDistinct<T>(inner, defaultValue);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = specificDefaultGenerator.NextDistinct(default(T));

                Assert.NotEqual(default(T), value);
                Assert.False(
                    specificDefaultGenerator.EqualityComparer.Equals(default(T), value)
                );
            }
        }

        public static IEnumerable<object[]> DefaultReturningGenerators {
            get {
                return new List<object[]> {
                    new object[] { new DefaultGenerator<object>() },
                    new object[] { new DefaultGenerator<int>() },
                    new object[] { new DefaultGenerator<FakeClass>() },
                    new object[] { new DefaultGenerator<FakeStruct>() },
                    new object[] { new DefaultGenerator<object>(new object()) },
                    new object[] { new DefaultGenerator<int>(5) },
                    new object[] { new DefaultGenerator<FakeClass>(new FakeClass(-2)) }
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

        public void NextDistinct_InnerOnlyReturnsDefaultImpl<T>(
            DefaultGenerator<T> inner) {

            var generator = this.MaybeDefaultDistinct<T>(inner, inner.DefaultValue);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextDistinct(inner.DefaultValue)
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

            NextDistinct_InnerFailsButDefaultOkImpl(
                this.MaybeDefaultDistinct(inner, percentage)
            );

            NextDistinct_InnerFailsButDefaultOkImpl(
                this.MaybeDefaultDistinct(inner, new FakeStruct { Value = -100 }, percentage)
            );
        }

        private void NextDistinct_InnerFailsButDefaultOkImpl(
            MaybeDefaultDistinctGenerator<FakeStruct> generator) {

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {

                // 2 is the only thing that can be returned by FakeStructGenerator,
                // but default (which is 0) is ok.

                var value = generator.NextDistinct(new FakeStruct { Value = 2 });

                Assert.Equal(generator.DefaultValue, value);
                Assert.True(generator.EqualityComparer.Equals(generator.DefaultValue, value));
            }
        }

        [Theory]
        [MemberData(nameof(FakeGenerators))]
        public void NextDistinct_FiftyPercentageOfDefault(Object inner, Object defaultValue) {
            this.InvokeGenericMethod(
                nameof(NextDistinct_FiftyPercentageOfDefaultImpl),
                inner,
                defaultValue
            );
        }

        public void NextDistinct_FiftyPercentageOfDefaultImpl<T>(
            IComparableGenerator<T> inner,
            T defaultValue) {

            const decimal percentage = 0.5m;

            NextDistinct_FiftyPercentageOfDefaultImpl(
                this.MaybeDefaultDistinct<T>(inner, percentage),
                inner.EqualityComparer,
                percentage
            );

            NextDistinct_FiftyPercentageOfDefaultImpl(
                this.MaybeDefaultDistinct<T>(inner, defaultValue, percentage),
                inner.EqualityComparer,
                percentage
            );
        }

        private void NextDistinct_FiftyPercentageOfDefaultImpl<T>(
            MaybeDefaultDistinctGenerator<T> generator,
            IEqualityComparer<T> innerComparer,
            decimal percentage) {

            var hasDefault = false;
            var hasNonDefault = false;

            for (var attempt = 0; attempt < extendedNumberOfAttempts; attempt++) {
                var value = generator.NextDistinct(generator.Next());

                if (!hasDefault) {
                    hasDefault = innerComparer.Equals(value, generator.DefaultValue);
                }

                if (!hasNonDefault) {
                    hasNonDefault = !innerComparer.Equals(value, generator.DefaultValue);
                }

                if (hasDefault && hasNonDefault) {
                    break;
                }
            }

            Assert.True(
                hasDefault,
                $"After {extendedNumberOfAttempts:N0} attempts with a {percentage * 100}% " +
                $"percentage chance of generating default values, the generator did not " +
                $"generate a default value. The randomization approach is unbalanced."
            );

            Assert.True(
                hasNonDefault,
                $"After {extendedNumberOfAttempts:N0} attempts with a {percentage * 100}% " +
                $"percentage chance of generating default values, the generator did not " +
                $"generate a non-default value. The randomization approach is unbalanced."
            );
        }

    }

}
