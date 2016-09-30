using System;
using System.Collections.Generic;
using Xunit;

namespace Peddler {

    public class MaybeDefaultComparableGeneratorTests : MaybeDefaultDistinctGeneratorTests {

        protected sealed override MaybeDefaultDistinctGenerator<T> MaybeDefaultDistinct<T>(
            IComparableGenerator<T> inner) {

            return this.MaybeDefaultComparable<T>(inner);
        }

        protected sealed override MaybeDefaultDistinctGenerator<T> MaybeDefaultDistinct<T>(
            IComparableGenerator<T> inner,
            decimal percentage) {

            return this.MaybeDefaultComparable<T>(inner, percentage);
        }

        protected virtual MaybeDefaultComparableGenerator<T> MaybeDefaultComparable<T>(
            IComparableGenerator<T> inner) {

            return new MaybeDefaultComparableGenerator<T>(inner);
        }

        protected virtual MaybeDefaultComparableGenerator<T> MaybeDefaultComparable<T>(
            IComparableGenerator<T> inner,
            decimal percentage) {

            return new MaybeDefaultComparableGenerator<T>(inner, percentage);
        }

        [Theory]
        [MemberData(nameof(DefaultReturningGenerators))]
        public void NextLessThan_InnerReturnsDefault(Object inner) {
            this.InvokeGenericMethod(
                nameof(NextLessThan_InnerReturnsDefaultImpl),
                inner
            );
        }

        public void NextLessThan_InnerReturnsDefaultImpl<T>(
            IComparableGenerator<T> inner) {

            var generator = this.MaybeDefaultComparable<T>(inner);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextLessThan(default(T))
            );
        }

        [Theory]
        [MemberData(nameof(IgnoredPercentages))]
        public void NextLessThan_InnerFailsButDefaultOk_Struct(decimal percentage) {
            var inner = new FakeStructGenerator(new Int32Generator(5, 10));
            var generator = this.MaybeDefaultComparable(inner, percentage);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {

                // 2 is less than the range of FakeStructGenerator,
                // but greater then default (which is 0).

                var value = generator.NextLessThan(new FakeStruct { Value = 2 });

                Assert.Equal(default(FakeStruct), value);
                Assert.True(generator.EqualityComparer.Equals(default(FakeStruct), value));
                Assert.Equal(0, generator.Comparer.Compare(default(FakeStruct), value));
            }
        }

        [Theory]
        [MemberData(nameof(IgnoredPercentages))]
        public void NextLessThan_InnerFailsButDefaultOk_Class(decimal percentage) {
            var inner = new FakeClassGenerator(new Int32Generator(5, 10));
            var generator = this.MaybeDefaultComparable(inner, percentage);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {

                // 2 is less than the range of FakeClassGenerator,
                // but greater then default (which is null).
                // null is always considered "less" than non-null values.

                var value = generator.NextLessThan(new FakeClass(2));

                Assert.Equal(default(FakeClass), value);
                Assert.True(generator.EqualityComparer.Equals(default(FakeClass), value));
                Assert.Equal(0, generator.Comparer.Compare(default(FakeClass), value));
            }
        }

        public static IEnumerable<object[]> NextLessThan_FiftyPercentOfDefault_Data {
            get {
                return new List<object[]> {
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(2, 5)),
                        new FakeStruct { Value = 3 }
                    },
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(2, 5)),
                        new FakeStruct { Value = 5 }
                    },
                    new object[] {
                        new FakeClassGenerator(new Int32Generator(2, 5)),
                        new FakeClass(3)
                    },
                    new object[] {
                        new FakeClassGenerator(new Int32Generator(2, 5)),
                        new FakeClass(5)
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(NextLessThan_FiftyPercentOfDefault_Data))]
        public void NextLessThan_FiftyPercentOfDefault(Object inner, Object otherValue) {
            this.InvokeGenericMethod(
                nameof(NextLessThan_FiftyPercentOfDefaultImpl),
                inner,
                otherValue
            );
        }

        public void NextLessThan_FiftyPercentOfDefaultImpl<T>(
            IComparableGenerator<T> inner,
            T otherValue) {

            this.NextImpl_FiftyPercentOfDefaultImpl<T>(
                inner,
                otherValue,
                generator => generator.NextLessThan
            );
        }

        [Theory]
        [MemberData(nameof(DefaultReturningGenerators))]
        public void NextLessThanOrEqualTo_InnerReturnsDefault(Object inner) {
            this.InvokeGenericMethod(
                nameof(NextLessThanOrEqualTo_InnerReturnsDefaultImpl),
                inner
            );
        }

        public void NextLessThanOrEqualTo_InnerReturnsDefaultImpl<T>(
            IComparableGenerator<T> inner) {

            var generator = this.MaybeDefaultComparable<T>(inner);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextLessThanOrEqualTo(default(T));

                Assert.Equal(default(T), value);
                Assert.True(generator.EqualityComparer.Equals(default(T), value));
                Assert.Equal(0, generator.Comparer.Compare(default(T), value));
            }
        }

        [Theory]
        [MemberData(nameof(IgnoredPercentages))]
        public void NextLessThanOrEqualTo_InnerFailsButDefaultOk_Struct(decimal percentage) {
            var inner = new FakeStructGenerator(new Int32Generator(5, 10));
            var generator = this.MaybeDefaultComparable(inner, percentage);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {

                // 2 is less than the range of FakeStructGenerator,
                // but greater then default (which is 0).

                var value = generator.NextLessThanOrEqualTo(new FakeStruct { Value = 2 });

                Assert.Equal(default(FakeStruct), value);
                Assert.True(generator.EqualityComparer.Equals(default(FakeStruct), value));
                Assert.Equal(0, generator.Comparer.Compare(default(FakeStruct), value));

                // the default is consider equal to itself, so should return default(T)

                value = generator.NextLessThanOrEqualTo(default(FakeStruct));

                Assert.Equal(default(FakeStruct), value);
                Assert.True(generator.EqualityComparer.Equals(default(FakeStruct), value));
                Assert.Equal(0, generator.Comparer.Compare(default(FakeStruct), value));
            }
        }

        [Theory]
        [MemberData(nameof(IgnoredPercentages))]
        public void NextLessThanOrEqualTo_InnerFailsButDefaultOk_Class(decimal percentage) {
            var inner = new FakeClassGenerator(new Int32Generator(5, 10));
            var generator = this.MaybeDefaultComparable(inner, percentage);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {

                // 2 is less than the range of FakeClassGenerator,
                // but greater then default (which is null).
                // null is always considered "less" than non-null values.

                var value = generator.NextLessThanOrEqualTo(new FakeClass(2));

                Assert.Equal(default(FakeClass), value);
                Assert.True(generator.EqualityComparer.Equals(default(FakeClass), value));
                Assert.Equal(0, generator.Comparer.Compare(default(FakeClass), value));

                // the default is consider equal to itself, so should return default(T)

                value = generator.NextLessThanOrEqualTo(default(FakeClass));

                Assert.Equal(default(FakeClass), value);
                Assert.True(generator.EqualityComparer.Equals(default(FakeClass), value));
                Assert.Equal(0, generator.Comparer.Compare(default(FakeClass), value));
            }
        }

        public static IEnumerable<object[]> NextLessThanOrEqualTo_FiftyPercentOfDefault_Data {
            get {
                return new List<object[]> {
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(2, 5)),
                        new FakeStruct { Value = 3 }
                    },
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(2, 5)),
                        new FakeStruct { Value = 5 }
                    },
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(2, 5)),
                        new FakeStruct { Value = 2 }
                    },
                    new object[] {
                        new FakeClassGenerator(new Int32Generator(2, 5)),
                        new FakeClass(3)
                    },
                    new object[] {
                        new FakeClassGenerator(new Int32Generator(2, 5)),
                        new FakeClass(5)
                    },
                    new object[] {
                        new FakeClassGenerator(new Int32Generator(2, 5)),
                        new FakeClass(2)
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(NextLessThanOrEqualTo_FiftyPercentOfDefault_Data))]
        public void NextLessThanOrEqualTo_FiftyPercentOfDefault(Object inner, Object otherValue) {
            this.InvokeGenericMethod(
                nameof(NextLessThanOrEqualTo_FiftyPercentOfDefaultImpl),
                inner,
                otherValue
            );
        }

        public void NextLessThanOrEqualTo_FiftyPercentOfDefaultImpl<T>(
            IComparableGenerator<T> inner,
            T otherValue) {

            this.NextImpl_FiftyPercentOfDefaultImpl<T>(
                inner,
                otherValue,
                generator => generator.NextLessThanOrEqualTo
            );
        }

        [Theory]
        [MemberData(nameof(DefaultReturningGenerators))]
        public void NextGreaterThanOrEqualTo_InnerReturnsDefault(Object inner) {
            this.InvokeGenericMethod(
                nameof(NextGreaterThanOrEqualTo_InnerReturnsDefaultImpl),
                inner
            );
        }

        public void NextGreaterThanOrEqualTo_InnerReturnsDefaultImpl<T>(
            IComparableGenerator<T> inner) {

            var generator = this.MaybeDefaultComparable<T>(inner);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextGreaterThanOrEqualTo(default(T));

                Assert.Equal(default(T), value);
                Assert.True(generator.EqualityComparer.Equals(default(T), value));
                Assert.Equal(0, generator.Comparer.Compare(default(T), value));
            }
        }

        [Theory]
        [MemberData(nameof(IgnoredPercentages))]
        public void NextGreaterThanOrEqualTo_InnerFailsButDefaultOk(decimal percentage) {
            var inner = new FakeStructGenerator(new Int32Generator(-10, -5));
            var generator = this.MaybeDefaultComparable(inner, percentage);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {

                // -5 is less than the range of FakeStructGenerator,
                // but greater then default (which is 0).

                var value = generator.NextGreaterThanOrEqualTo(new FakeStruct { Value = -2 });

                Assert.Equal(default(FakeStruct), value);
                Assert.True(generator.EqualityComparer.Equals(default(FakeStruct), value));
                Assert.Equal(0, generator.Comparer.Compare(default(FakeStruct), value));

                // the default is consider equal to itself, so should return default(T)

                value = generator.NextGreaterThanOrEqualTo(default(FakeStruct));

                Assert.Equal(default(FakeStruct), value);
                Assert.True(generator.EqualityComparer.Equals(default(FakeStruct), value));
                Assert.Equal(0, generator.Comparer.Compare(default(FakeStruct), value));
            }
        }

        public static IEnumerable<object[]> NextGreaterThanOrEqualTo_FiftyPercentOfDefault_Data {
            get {
                return new List<object[]> {
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(-4, -1)),
                        new FakeStruct { Value = -5 }
                    },
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(-4, -1)),
                        new FakeStruct { Value = -3 }
                    },
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(-4, -1)),
                        new FakeStruct { Value = -2 }
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(NextGreaterThanOrEqualTo_FiftyPercentOfDefault_Data))]
        public void NextGreaterThanOrEqualTo_FiftyPercentOfDefault(Object inner, Object otherValue) {
            this.InvokeGenericMethod(
                nameof(NextGreaterThanOrEqualTo_FiftyPercentOfDefaultImpl),
                inner,
                otherValue
            );
        }

        public void NextGreaterThanOrEqualTo_FiftyPercentOfDefaultImpl<T>(
            IComparableGenerator<T> inner,
            T otherValue) {

            this.NextImpl_FiftyPercentOfDefaultImpl<T>(
                inner,
                otherValue,
                generator => generator.NextGreaterThanOrEqualTo
            );
        }

        [Theory]
        [MemberData(nameof(DefaultReturningGenerators))]
        public void NextGreaterThan_InnerReturnsDefault(Object inner) {
            this.InvokeGenericMethod(
                nameof(NextGreaterThan_InnerReturnsDefaultImpl),
                inner
            );
        }

        public void NextGreaterThan_InnerReturnsDefaultImpl<T>(IComparableGenerator<T> inner) {
            var generator = this.MaybeDefaultComparable<T>(inner);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextGreaterThan(default(T))
            );
        }

        [Theory]
        [MemberData(nameof(IgnoredPercentages))]
        public void NextGreaterThan_InnerFailsButDefaultOk(decimal percentage) {
            var inner = new FakeStructGenerator(new Int32Generator(-10, -5));
            var generator = this.MaybeDefaultComparable(inner, percentage);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {

                // -5 is less than the range of FakeStructGenerator,
                // but greater then default (which is 0).

                var value = generator.NextGreaterThan(new FakeStruct { Value = -2 });

                Assert.Equal(default(FakeStruct), value);
                Assert.True(generator.EqualityComparer.Equals(default(FakeStruct), value));
                Assert.Equal(0, generator.Comparer.Compare(default(FakeStruct), value));
            }
        }

        public static IEnumerable<object[]> NextGreaterThan_FiftyPercentOfDefault_Data {
            get {
                return new List<object[]> {
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(-4, -1)),
                        new FakeStruct { Value = -5 }
                    },
                    new object[] {
                        new FakeStructGenerator(new Int32Generator(-4, -1)),
                        new FakeStruct { Value = -3 }
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(NextGreaterThan_FiftyPercentOfDefault_Data))]
        public void NextGreaterThan_FiftyPercentOfDefault(Object inner, Object otherValue) {
            this.InvokeGenericMethod(
                nameof(NextGreaterThan_FiftyPercentOfDefaultImpl),
                inner,
                otherValue
            );
        }

        public void NextGreaterThan_FiftyPercentOfDefaultImpl<T>(
            IComparableGenerator<T> inner,
            T otherValue) {

            this.NextImpl_FiftyPercentOfDefaultImpl<T>(
                inner,
                otherValue,
                generator => generator.NextGreaterThan
            );
        }

        private void NextImpl_FiftyPercentOfDefaultImpl<T>(
            IComparableGenerator<T> inner,
            T otherValue,
            Func<IComparableGenerator<T>, Func<T, T>> getNextImpl) {

            const decimal percentage = 0.5m;

            var generator = this.MaybeDefaultComparable<T>(inner, percentage);
            var nextImpl = getNextImpl(generator);

            var hasDefault = false;
            var hasNonDefault = false;

            for (var attempt = 0; attempt < extendedNumberOfAttempts; attempt++) {
                var value = nextImpl(otherValue);

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
