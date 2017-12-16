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
            T defaultValue) {

            return this.MaybeDefaultComparable<T>(inner, defaultValue);
        }

        protected sealed override MaybeDefaultDistinctGenerator<T> MaybeDefaultDistinct<T>(
            IComparableGenerator<T> inner,
            decimal percentage) {

            return this.MaybeDefaultComparable<T>(inner, percentage);
        }

        protected sealed override MaybeDefaultDistinctGenerator<T> MaybeDefaultDistinct<T>(
            IComparableGenerator<T> inner,
            T defaultValue,
            decimal percentage) {

            return this.MaybeDefaultComparable<T>(inner, defaultValue, percentage);
        }

        protected virtual MaybeDefaultComparableGenerator<T> MaybeDefaultComparable<T>(
            IComparableGenerator<T> inner) {

            var generator = new MaybeDefaultComparableGenerator<T>(inner);
            Assert.Equal(default(T), generator.DefaultValue);
            return generator;
        }

        protected virtual MaybeDefaultComparableGenerator<T> MaybeDefaultComparable<T>(
            IComparableGenerator<T> inner,
            T defaultValue) {

            var generator = new MaybeDefaultComparableGenerator<T>(inner, defaultValue);
            Assert.Equal(defaultValue, generator.DefaultValue);
            return generator;
        }

        protected virtual MaybeDefaultComparableGenerator<T> MaybeDefaultComparable<T>(
            IComparableGenerator<T> inner,
            decimal percentage) {

            var generator = new MaybeDefaultComparableGenerator<T>(inner, percentage);
            Assert.Equal(default(T), generator.DefaultValue);
            return generator;
        }

        protected virtual MaybeDefaultComparableGenerator<T> MaybeDefaultComparable<T>(
            IComparableGenerator<T> inner,
            T defaultValue,
            decimal percentage) {

            var generator = new MaybeDefaultComparableGenerator<T>(inner, defaultValue, percentage);
            Assert.Equal(defaultValue, generator.DefaultValue);
            return generator;
        }

        [Theory]
        [MemberData(nameof(DefaultReturningGenerators))]
        public void NextLessThan_InnerReturnsDefault(Object inner) {
            this.InvokeGenericMethod(
                nameof(NextLessThan_InnerReturnsDefaultImpl),
                inner
            );
        }

        protected void NextLessThan_InnerReturnsDefaultImpl<T>(DefaultGenerator<T> inner) {
            var generator = this.MaybeDefaultComparable<T>(inner, inner.DefaultValue);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextLessThan(inner.DefaultValue)
            );
        }

        [Theory]
        [MemberData(nameof(IgnoredPercentages))]
        public void NextLessThan_InnerFailsButDefaultOk_Struct(decimal percentage) {
            var inner = new FakeStructGenerator(new Int32Generator(5, 10));

            this.NextLessThan_InnerFailsButDefaultOk_StructImpl(
                this.MaybeDefaultComparable(
                    inner,
                    percentage
                )
            );

            this.NextLessThan_InnerFailsButDefaultOk_StructImpl(
                this.MaybeDefaultComparable(
                    inner,
                    new FakeStruct { Value = -100 },
                    percentage
                )
            );
        }

        private void NextLessThan_InnerFailsButDefaultOk_StructImpl(
            MaybeDefaultComparableGenerator<FakeStruct> generator) {

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {

                // 2 is less than the range of FakeStructGenerator,
                // but greater then default (which is 0).

                var value = generator.NextLessThan(new FakeStruct { Value = 2 });

                Assert.Equal(generator.DefaultValue, value);
                Assert.True(generator.EqualityComparer.Equals(generator.DefaultValue, value));
                Assert.Equal(0, generator.Comparer.Compare(generator.DefaultValue, value));
            }
        }

        [Theory]
        [MemberData(nameof(IgnoredPercentages))]
        public void NextLessThan_InnerFailsButDefaultOk_Class(decimal percentage) {
            var inner = new FakeClassGenerator(new Int32Generator(5, 10));

            this.NextLessThan_InnerFailsButDefaultOk_ClassImpl(
                this.MaybeDefaultComparable(
                    inner,
                    percentage
                )
            );

            this.NextLessThan_InnerFailsButDefaultOk_ClassImpl(
                this.MaybeDefaultComparable(
                    inner,
                    new FakeClass(-100),
                    percentage
                )
            );
        }

        private void NextLessThan_InnerFailsButDefaultOk_ClassImpl(
            MaybeDefaultComparableGenerator<FakeClass> generator) {

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {

                // 2 is less than the range of FakeClassGenerator,
                // but greater then default (which is null).
                // null is always considered "less" than non-null values.

                var value = generator.NextLessThan(new FakeClass(2));

                Assert.Equal(generator.DefaultValue, value);
                Assert.True(generator.EqualityComparer.Equals(generator.DefaultValue, value));
                Assert.Equal(0, generator.Comparer.Compare(generator.DefaultValue, value));
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

        protected void NextLessThan_FiftyPercentOfDefaultImpl<T>(
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

        protected void NextLessThanOrEqualTo_InnerReturnsDefaultImpl<T>(
            DefaultGenerator<T> inner) {

            var generator = this.MaybeDefaultComparable<T>(inner, inner.DefaultValue);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextLessThanOrEqualTo(inner.DefaultValue);

                Assert.Equal(inner.DefaultValue, value);
                Assert.True(generator.EqualityComparer.Equals(inner.DefaultValue, value));
                Assert.Equal(0, generator.Comparer.Compare(inner.DefaultValue, value));
            }
        }

        [Theory]
        [MemberData(nameof(IgnoredPercentages))]
        public void NextLessThanOrEqualTo_InnerFailsButDefaultOk_Struct(decimal percentage) {
            var inner = new FakeStructGenerator(new Int32Generator(5, 10));

            this.NextLessThanOrEqualTo_InnerFailsButDefaultOk_StructImpl(
                this.MaybeDefaultComparable(
                    inner,
                    percentage
                )
            );

            this.NextLessThanOrEqualTo_InnerFailsButDefaultOk_StructImpl(
                this.MaybeDefaultComparable(
                    inner,
                    new FakeStruct { Value = -100 },
                    percentage
                )
            );
        }

        private void NextLessThanOrEqualTo_InnerFailsButDefaultOk_StructImpl(
            MaybeDefaultComparableGenerator<FakeStruct> generator) {

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {

                // 2 is less than the range of FakeStructGenerator,
                // but greater then default (which is 0).

                var value = generator.NextLessThanOrEqualTo(new FakeStruct { Value = 2 });

                Assert.Equal(generator.DefaultValue, value);
                Assert.True(generator.EqualityComparer.Equals(generator.DefaultValue, value));
                Assert.Equal(0, generator.Comparer.Compare(generator.DefaultValue, value));

                // the default is consider equal to itself, so should return default(T)

                value = generator.NextLessThanOrEqualTo(generator.DefaultValue);

                Assert.Equal(generator.DefaultValue, value);
                Assert.True(generator.EqualityComparer.Equals(generator.DefaultValue, value));
                Assert.Equal(0, generator.Comparer.Compare(generator.DefaultValue, value));
            }
        }

        [Theory]
        [MemberData(nameof(IgnoredPercentages))]
        public void NextLessThanOrEqualTo_InnerFailsButDefaultOk_Class(decimal percentage) {
            var inner = new FakeClassGenerator(new Int32Generator(5, 10));

            this.NextLessThanOrEqualTo_InnerFailsButDefaultOk_ClassImpl(
                this.MaybeDefaultComparable(
                    inner,
                    percentage
                )
            );

            this.NextLessThanOrEqualTo_InnerFailsButDefaultOk_ClassImpl(
                this.MaybeDefaultComparable(
                    inner,
                    new FakeClass(-100),
                    percentage
                )
            );
        }

        protected void NextLessThanOrEqualTo_InnerFailsButDefaultOk_ClassImpl(
            MaybeDefaultComparableGenerator<FakeClass> generator) {

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {

                // 2 is less than the range of FakeClassGenerator,
                // but greater then default (which is null).
                // null is always considered "less" than non-null values.

                var value = generator.NextLessThanOrEqualTo(new FakeClass(2));

                Assert.Equal(generator.DefaultValue, value);
                Assert.True(generator.EqualityComparer.Equals(generator.DefaultValue, value));
                Assert.Equal(0, generator.Comparer.Compare(generator.DefaultValue, value));

                // the default is consider equal to itself, so should return default(T)

                value = generator.NextLessThanOrEqualTo(generator.DefaultValue);

                Assert.Equal(generator.DefaultValue, value);
                Assert.True(generator.EqualityComparer.Equals(generator.DefaultValue, value));
                Assert.Equal(0, generator.Comparer.Compare(generator.DefaultValue, value));
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

        protected void NextLessThanOrEqualTo_FiftyPercentOfDefaultImpl<T>(
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

        protected void NextGreaterThanOrEqualTo_InnerReturnsDefaultImpl<T>(
            DefaultGenerator<T> inner) {

            var generator = this.MaybeDefaultComparable<T>(inner, inner.DefaultValue);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextGreaterThanOrEqualTo(inner.DefaultValue);

                Assert.Equal(inner.DefaultValue, value);
                Assert.True(generator.EqualityComparer.Equals(inner.DefaultValue, value));
                Assert.Equal(0, generator.Comparer.Compare(inner.DefaultValue, value));
            }
        }

        [Theory]
        [MemberData(nameof(IgnoredPercentages))]
        public void NextGreaterThanOrEqualTo_InnerFailsButDefaultOk(decimal percentage) {
            var inner = new FakeStructGenerator(new Int32Generator(-10, -5));

            this.NextGreaterThanOrEqualTo_InnerFailsButDefaultOkImpl(
                this.MaybeDefaultComparable(
                    inner,
                    percentage
                )
            );

            this.NextGreaterThanOrEqualTo_InnerFailsButDefaultOkImpl(
                this.MaybeDefaultComparable(
                    inner,
                    new FakeStruct { Value = 100 },
                    percentage
                )
            );
        }

        protected void NextGreaterThanOrEqualTo_InnerFailsButDefaultOkImpl(
            MaybeDefaultComparableGenerator<FakeStruct> generator) {

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {

                // -5 is less than the range of FakeStructGenerator,
                // but greater then default (which is 0).

                var value = generator.NextGreaterThanOrEqualTo(new FakeStruct { Value = -2 });

                Assert.Equal(generator.DefaultValue, value);
                Assert.True(generator.EqualityComparer.Equals(generator.DefaultValue, value));
                Assert.Equal(0, generator.Comparer.Compare(generator.DefaultValue, value));

                // the default is consider equal to itself, so should return default(T)

                value = generator.NextGreaterThanOrEqualTo(generator.DefaultValue);

                Assert.Equal(generator.DefaultValue, value);
                Assert.True(generator.EqualityComparer.Equals(generator.DefaultValue, value));
                Assert.Equal(0, generator.Comparer.Compare(generator.DefaultValue, value));
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

        protected void NextGreaterThanOrEqualTo_FiftyPercentOfDefaultImpl<T>(
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

        protected void NextGreaterThan_InnerReturnsDefaultImpl<T>(DefaultGenerator<T> inner) {
            var generator = this.MaybeDefaultComparable<T>(inner, inner.DefaultValue);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextGreaterThan(inner.DefaultValue)
            );
        }

        [Theory]
        [MemberData(nameof(IgnoredPercentages))]
        public void NextGreaterThan_InnerFailsButDefaultOk(decimal percentage) {
            var inner = new FakeStructGenerator(new Int32Generator(-10, -5));
            var generator = this.MaybeDefaultComparable(inner, percentage);

            this.NextGreaterThan_InnerFailsButDefaultOkImpl(
                this.MaybeDefaultComparable(
                    inner,
                    percentage
                )
            );

            this.NextGreaterThan_InnerFailsButDefaultOkImpl(
                this.MaybeDefaultComparable(
                    inner,
                    new FakeStruct { Value = 5 },
                    percentage
                )
            );
        }

        private void NextGreaterThan_InnerFailsButDefaultOkImpl(
            MaybeDefaultComparableGenerator<FakeStruct> generator) {

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {

                // -5 is less than the range of FakeStructGenerator,
                // but greater then default (which is 0).

                var value = generator.NextGreaterThan(new FakeStruct { Value = -2 });

                Assert.Equal(generator.DefaultValue, value);
                Assert.True(generator.EqualityComparer.Equals(generator.DefaultValue, value));
                Assert.Equal(0, generator.Comparer.Compare(generator.DefaultValue, value));
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

        protected void NextGreaterThan_FiftyPercentOfDefaultImpl<T>(
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
                    hasDefault = inner.EqualityComparer.Equals(value, generator.DefaultValue);
                }

                if (!hasNonDefault) {
                    hasNonDefault = !inner.EqualityComparer.Equals(value, generator.DefaultValue);
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
