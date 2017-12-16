using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Peddler {

    public class MaybeDefaultGeneratorTests {

        protected const int numberOfAttempts = 100;
        protected const int extendedNumberOfAttempts = 10 * numberOfAttempts;

        protected virtual MaybeDefaultGenerator<T> MaybeDefault<T>(
            IComparableGenerator<T> inner) {

            var generator = new MaybeDefaultGenerator<T>(inner);
            Assert.Equal(default(T), generator.DefaultValue);
            return generator;
        }

        protected virtual MaybeDefaultGenerator<T> MaybeDefault<T>(
            IComparableGenerator<T> inner,
            T defaultValue) {

            var generator = new MaybeDefaultGenerator<T>(inner, defaultValue);
            Assert.Equal(defaultValue, generator.DefaultValue);
            return generator;
        }

        protected virtual MaybeDefaultGenerator<T> MaybeDefault<T>(
            IComparableGenerator<T> inner,
            decimal percentage) {

            var generator = new MaybeDefaultGenerator<T>(inner, percentage);
            Assert.Equal(default(T), generator.DefaultValue);
            return generator;
        }

        protected virtual MaybeDefaultGenerator<T> MaybeDefault<T>(
            IComparableGenerator<T> inner,
            T defaultValue,
            decimal percentage) {

            var generator = new MaybeDefaultGenerator<T>(inner, defaultValue, percentage);
            Assert.Equal(defaultValue, generator.DefaultValue);
            return generator;
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

        [Fact]
        public void Constructor_WithoutDefault_DefaultValueIsDefaultForType() {
            var inner = new Int32Generator();

            Assert.Equal(
                default(Int32),
                this.MaybeDefault(inner).DefaultValue
            );

            Assert.Equal(
                default(Int32),
                this.MaybeDefault(inner, 0.5m).DefaultValue
            );
        }

        [Fact]
        public void Constructor_WithDefault_DefaultValueRespectsInjectedValue() {
            var inner = new Int32Generator();
            var defaultValue = inner.Next();

            Assert.Equal(
                defaultValue,
                this.MaybeDefault(inner, defaultValue).DefaultValue
            );

            Assert.Equal(
                defaultValue,
                this.MaybeDefault(inner, defaultValue, 0.5m).DefaultValue
            );
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

        protected void Next_ZeroPercentageOfDefaultImpl<T>(
            IComparableGenerator<T> inner,
            T otherDefault) {

            var autoDefaultGenerator = this.MaybeDefault<T>(inner, 0m);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.NotEqual(default(T), autoDefaultGenerator.Next());
            }

            var specificDefaultGenerator = this.MaybeDefault<T>(inner, otherDefault, 0m);

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

        protected void Next_OneHundredPercentageOfDefaultImpl<T>(
            IComparableGenerator<T> inner,
            T defaultValue) {

            var autoDefaultGenerator = this.MaybeDefault<T>(inner, 1m);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.Equal(default(T), autoDefaultGenerator.Next());
            }

            var specificDefaultGenerator = this.MaybeDefault<T>(inner, defaultValue, 1m);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.Equal(defaultValue, specificDefaultGenerator.Next());
            }
        }

        [Theory]
        [MemberData(nameof(NonDefaultReturningGenerators))]
        public void Next_GeneratesDefaultAndNonDefault(Object inner, Object defaultValue) {
            this.InvokeGenericMethod(
                nameof(Next_WithFiftyPercentChangeOfDefault),
                inner,
                defaultValue
            );
        }

        protected void Next_WithFiftyPercentChangeOfDefault<T>(
            IComparableGenerator<T> inner,
            T defaultValue) {

            const decimal percentage = 0.5m;

            this.Next_WithFiftyPercentChangeOfDefaultImpl<T>(
                this.MaybeDefault<T>(inner, percentage),
                inner.EqualityComparer,
                percentage
            );

            this.Next_WithFiftyPercentChangeOfDefaultImpl<T>(
                this.MaybeDefault<T>(inner, defaultValue, percentage),
                inner.EqualityComparer,
                percentage
            );
        }

        private void Next_WithFiftyPercentChangeOfDefaultImpl<T>(
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

        [Fact]
        public async Task ThreadSafety() {

            // Arrange

            var generator = this.MaybeDefault(new Int32Generator(1, Int32.MaxValue), 0.5m);
            var consecutiveDefaults = new ConcurrentStack<Int32>();

            Action createThread =
                () => this.ThreadSafetyImpl(generator, consecutiveDefaults);

            // Act

            var threads =
                Enumerable
                    .Range(0, 10)
                    .Select(_ => Task.Run(createThread))
                    .ToArray();

            await Task.WhenAll(threads);

            // Assert

            Assert.True(
                consecutiveDefaults.Count < 50,
                $"System.Random is not thread safe. If one of its .Next() " +
                $"implementations is called simultaneously on several " +
                $"threads, it breaks and starts returning zero exclusively. " +
                $"The last {consecutiveDefaults.Count:N0} values were " +
                $"the null, signifying its internal System.Random is in " +
                $"a broken state."
            );
        }

        private void ThreadSafetyImpl<T>(
            IGenerator<T> generator,
            ConcurrentStack<T> consecutiveDefaults) {

            var count = 0;

            while (count++ < 10000) {
                if (generator.Next().Equals(default(T))) {
                    consecutiveDefaults.Push(default(T));
                } else {
                    consecutiveDefaults.Clear();
                }
            }
        }

        protected void InvokeGenericMethod(
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

            const BindingFlags flags =
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Static | BindingFlags.Instance;

            var method = this.GetType().GetMethod(methodName, flags);

            if (method == null) {
                throw new ArgumentException($"Unable to find method '{methodName}'");
            }

            method
                .MakeGenericMethod(new Type[] { fakeType })
                .Invoke(this, parameters);
        }

    }

}
