using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Peddler {

    public class MaybeDefaultGeneratorTests {

        protected const int numberOfAttempts = 100;
        protected const int extendedNumberOfAttempts = 10 * numberOfAttempts;

        protected virtual MaybeDefaultGenerator<T> MaybeDefault<T>(
            IComparableGenerator<T> inner) {

            return new MaybeDefaultGenerator<T>(inner);
        }

        protected virtual MaybeDefaultGenerator<T> MaybeDefault<T>(
            IComparableGenerator<T> inner,
            T defaultValue) {

            return new MaybeDefaultGenerator<T>(inner, defaultValue);
        }

        protected virtual MaybeDefaultGenerator<T> MaybeDefault<T>(
            IComparableGenerator<T> inner,
            decimal percentage) {

            return new MaybeDefaultGenerator<T>(inner, percentage);
        }

        protected virtual MaybeDefaultGenerator<T> MaybeDefault<T>(
            IComparableGenerator<T> inner,
            T defaultValue,
            decimal percentage) {

            return new MaybeDefaultGenerator<T>(inner, defaultValue, percentage);
        }

        public static IEnumerable<object[]> NonDefaultReturningGenerators {
            get {
                return new List<object[]> {
                    new object[] {
                        new FakeClassGenerator(new Int32Generator(-10, 10)),
                        new FakeClass(-100)
                    },
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(1, 10)),
                        new FakeStruct { Value = -100 }
                    },
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(-10, -1)),
                        new FakeStruct { Value = -100 }
                    }
                };
            }
        }

        [Fact]
        public void Constructor_NullInnerGenerator() {
            IComparableGenerator<Object> inner = null;

            Assert.Throws<ArgumentNullException>(
                () => this.MaybeDefault(inner)
            );

            Assert.Throws<ArgumentNullException>(
                () => this.MaybeDefault(inner, default(Object))
            );

            Assert.Throws<ArgumentNullException>(
                () => this.MaybeDefault(inner, 0.5m)
            );

            Assert.Throws<ArgumentNullException>(
                () => this.MaybeDefault(inner, default(Object), 0.5m)
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
            var inner = new FakeClassGenerator();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => this.MaybeDefault(inner, percentage)
            );

            Assert.Throws<ArgumentOutOfRangeException>(
                () => this.MaybeDefault(inner, new FakeClass(-100), percentage)
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
            var inner = new FakeClassGenerator();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => this.MaybeDefault(inner, percentage)
            );

            Assert.Throws<ArgumentOutOfRangeException>(
                () => this.MaybeDefault(inner, new FakeClass(-100), percentage)
            );
        }

        [Theory]
        [MemberData(nameof(NonDefaultReturningGenerators))]
        public void Next_ZeroPercentageOfDefault(Object inner, Object defaultValue) {
            this.InvokeGenericMethod(
                nameof(Next_ZeroPercentageOfDefaultImpl),
                inner,
                defaultValue
            );
        }

        public void Next_ZeroPercentageOfDefaultImpl<T>(
            IComparableGenerator<T> inner,
            T otherDefault) {

            var autoDefaultGenerator = this.MaybeDefault<T>(inner, 0m);
            Assert.Equal(default(T), autoDefaultGenerator.DefaultValue);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.NotEqual(default(T), autoDefaultGenerator.Next());
            }

            var specificDefaultGenerator = this.MaybeDefault<T>(inner, otherDefault, 0m);
            Assert.Equal(otherDefault, specificDefaultGenerator.DefaultValue);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.NotEqual(otherDefault, specificDefaultGenerator.Next());
            }
        }

        [Theory]
        [MemberData(nameof(NonDefaultReturningGenerators))]
        public void Next_OneHundredPercentageOfDefault(Object inner, Object defaultValue) {
            this.InvokeGenericMethod(
                nameof(Next_OneHundredPercentageOfDefaultImpl),
                inner,
                defaultValue
            );
        }

        public void Next_OneHundredPercentageOfDefaultImpl<T>(
            IComparableGenerator<T> inner,
            T defaultValue) {

            var autoDefaultGenerator = this.MaybeDefault<T>(inner, 1m);
            Assert.Equal(default(T), autoDefaultGenerator.DefaultValue);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.Equal(default(T), autoDefaultGenerator.Next());
            }

            var specificDefaultGenerator = this.MaybeDefault<T>(inner, defaultValue, 1m);
            Assert.Equal(defaultValue, specificDefaultGenerator.DefaultValue);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.Equal(defaultValue, specificDefaultGenerator.Next());
            }
        }

        [Theory]
        [MemberData(nameof(NonDefaultReturningGenerators))]
        public void Next_FiftyPercentageOfDefault(Object inner, Object defaultValue) {
            this.InvokeGenericMethod(
                nameof(Next_FiftyPercentageOfDefaultImpl),
                inner,
                defaultValue
            );
        }

        public void Next_FiftyPercentageOfDefaultImpl<T>(
            IComparableGenerator<T> inner,
            T defaultValue) {

            const decimal percentage = 0.5m;

            Next_FiftyPercentageOfDefaultImpl(
                this.MaybeDefault<T>(inner, percentage),
                inner.EqualityComparer,
                percentage
            );

            Next_FiftyPercentageOfDefaultImpl(
                this.MaybeDefault<T>(inner, defaultValue, percentage),
                inner.EqualityComparer,
                percentage
            );
        }

        private void Next_FiftyPercentageOfDefaultImpl<T>(
            MaybeDefaultGenerator<T> generator,
            IEqualityComparer<T> innerComparer,
            decimal percentage) {

            var hasDefault = false;
            var hasNonDefault = false;

            for (var attempt = 0; attempt < extendedNumberOfAttempts; attempt++) {
                var value = generator.Next();

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

        public void InvokeGenericMethod(
            String methodName,
            params Object[] parameters) {

            if (methodName == null) {
                throw new ArgumentNullException(nameof(methodName));
            }

            if (parameters == null) {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Length == 0) {
                throw new ArgumentException(
                    $"The first argument must be of type {typeof(IComparableGenerator<>).Name}.",
                    nameof(parameters)
                );
            }

            var interfaceType =
                parameters[0]
                    .GetType()
                    .GetInterfaces()
                    .SingleOrDefault(type => type.Name.StartsWith("IComparableGenerator"));

            if (interfaceType == null) {
                throw new ArgumentException(
                    $"The first argument must be of type {typeof(IComparableGenerator<>).Name}.",
                    nameof(parameters)
                );
            }

            var fakeType = interfaceType.GetGenericArguments().Single();

            var method = this.GetType().GetMethod(methodName);

            if (method == null) {
                throw new ArgumentException($"Unable to find method '{methodName}'");
            }

            method
                .MakeGenericMethod(new Type[] { fakeType })
                .Invoke(this, parameters);
        }

    }

}
