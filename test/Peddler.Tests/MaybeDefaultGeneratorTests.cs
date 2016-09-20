using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Peddler {

    public class MaybeDefaultGeneratorTests {

        protected const int numberOfAttempts = 100;
        protected const int extendedNumberOfAttempts = 10 * numberOfAttempts;

        protected virtual IGenerator<T> MaybeDefault<T>(IComparableGenerator<T> inner) {
            return new MaybeDefaultGenerator<T>(inner);
        }

        protected virtual IGenerator<T> MaybeDefault<T>(
            IComparableGenerator<T> inner,
            decimal percentage) {

            return new MaybeDefaultGenerator<T>(inner, percentage);
        }

        public static IEnumerable<object[]> NonDefaultReturningGenerators {
            get {
                return new List<object[]> {
                    new object[] { new FakeClassGenerator() },
                    new object[] { new FakeStructGenerator(new Int32Generator(1, 10)) },
                    new object[] { new FakeStructGenerator(new Int32Generator(-10, -1)) }
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
            var inner = new FakeClassGenerator();

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
            var inner = new FakeClassGenerator();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => this.MaybeDefault(inner, percentage)
            );
        }

        [Theory]
        [MemberData(nameof(NonDefaultReturningGenerators))]
        public void Next_ZeroPercentageOfDefault(Object inner) {
            this.InvokeGenericMethod(nameof(Next_ZeroPercentageOfDefaultImpl), inner);
        }

        public void Next_ZeroPercentageOfDefaultImpl<T>(IComparableGenerator<T> inner) {
            var generator = this.MaybeDefault<T>(inner, 0m);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.NotEqual(default(T), generator.Next());
            }
        }

        [Theory]
        [MemberData(nameof(NonDefaultReturningGenerators))]
        public void Next_OneHundredPercentageOfDefault(Object inner) {
            this.InvokeGenericMethod(nameof(Next_OneHundredPercentageOfDefaultImpl), inner);
        }

        public void Next_OneHundredPercentageOfDefaultImpl<T>(FakeGeneratorBase<T> inner) {
            var generator = this.MaybeDefault<T>(inner, 1m);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.Equal(default(T), generator.Next());
            }
        }

        [Theory]
        [MemberData(nameof(NonDefaultReturningGenerators))]
        public void Next_FiftyPercentageOfDefault(Object inner) {
            this.InvokeGenericMethod(nameof(Next_FiftyPercentageOfDefaultImpl), inner);
        }

        public void Next_FiftyPercentageOfDefaultImpl<T>(IComparableGenerator<T> inner) {
            const decimal percentage = 0.5m;

            var generator = this.MaybeDefault<T>(inner, percentage);
            var hasDefault = false;
            var hasNonDefault = false;

            for (var attempt = 0; attempt < extendedNumberOfAttempts; attempt++) {
                var value = generator.Next();

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
