using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Peddler {

    public class EnumGeneratorTests {

        private const int numberOfAttempts = 100;

        [Fact]
        public void Constructor_NoValues_NotAnEnum() {
            var exception = Assert.Throws<NotSupportedException>(
                () => new EnumGenerator<DateTime>()
            );

            Assert.Equal("'DateTime' is not an enum.", exception.Message);
        }

        [Fact]
        public void Constructor_NoValues_EnumLacksValues() {
            var exception = Assert.Throws<NotSupportedException>(
                () => new EnumGenerator<EmptyEnum>()
            );

            Assert.Equal("'EmptyEnum' lacks enum values.", exception.Message);
        }

        [Fact]
        public void Constructor_WithValues_NotAnEnum() {
            var values = new HashSet<DateTime> { DateTime.Now, DateTime.UtcNow };

            var exception = Assert.Throws<NotSupportedException>(
                () => new EnumGenerator<DateTime>(values)
            );

            Assert.Equal("'DateTime' is not an enum.", exception.Message);
        }

        [Fact]
        public void Constructor_WithValues_NullValues() {
            Assert.Throws<ArgumentNullException>(
                () => new EnumGenerator<ValidEnum>(null)
            );
        }

        [Fact]
        public void Constructor_WithValues_EmptyValues() {
            var values = new HashSet<ValidEnum>();

            Assert.Throws<ArgumentException>(
                () => new EnumGenerator<ValidEnum>(values)
            );
        }

        [Fact]
        public void ValuesProperty_FromDefaultValues() {
            var values = new HashSet<ValidEnum> {
                ValidEnum.Default,
                ValidEnum.One,
                ValidEnum.Two,
                ValidEnum.Three,
                ValidEnum.Four,
                ValidEnum.Five,
            };

            var generator = new EnumGenerator<ValidEnum>();

            Assert.Empty(values.Except(generator.Values));
            Assert.Empty(generator.Values.Except(values));
        }

        public static IEnumerable<object[]> ValuesProperty_FromSpecificValues_Data {
            get {
                return new List<object[]> {
                    new object[] {
                        new HashSet<ValidEnum> { ValidEnum.Default, ValidEnum.One }
                    },
                    new object[] {
                        new HashSet<ValidEnum> { ValidEnum.Default }
                    },
                    new object[] {
                        new HashSet<ValidEnum> { ValidEnum.One, ValidEnum.Two, ValidEnum.Four }
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(ValuesProperty_FromSpecificValues_Data))]
        public void ValuesProperty_FromSpecificValues(ISet<ValidEnum> values) {
            var generator = new EnumGenerator<ValidEnum>(values);

            Assert.Empty(values.Except(generator.Values));
            Assert.Empty(generator.Values.Except(values));
        }

        [Fact]
        public void Next_DefaultValues() {
            var values = new HashSet<ValidEnum> {
                ValidEnum.Default,
                ValidEnum.One,
                ValidEnum.Two,
                ValidEnum.Three,
                ValidEnum.Four,
                ValidEnum.Five,
            };

            var generator = new EnumGenerator<ValidEnum>();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.True(
                    values.Contains(value),
                    $"Unexpected value '{value:G}' was returned from generator."
                );
            }
        }

        public static IEnumerable<object[]> Next_SpecificValues_Data {
            get {
                return new List<object[]> {
                    new object[] {
                        new HashSet<ValidEnum> { ValidEnum.Default, ValidEnum.Five }
                    },
                    new object[] {
                        new HashSet<ValidEnum> { ValidEnum.One, ValidEnum.Two }
                    },
                    new object[] {
                        new HashSet<ValidEnum> { ValidEnum.Default }
                    },
                    new object[] {
                        new HashSet<ValidEnum> { ValidEnum.Four }
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Next_SpecificValues_Data))]
        public void Next_SpecificValues(ISet<ValidEnum> values) {
            var generator = new EnumGenerator<ValidEnum>(values);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.True(
                    values.Contains(value),
                    $"Unexpected value '{value:G}' was returned from generator."
                );
            }
        }

        [Fact]
        public void NextDistinct_DefaultValues_OneValueAllowed() {
            var generator = new EnumGenerator<OneEnum>();

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextDistinct(OneEnum.One)
            );
        }

        [Theory]
        [InlineData(ValidEnum.Default, ValidEnum.One)]
        [InlineData(ValidEnum.One, ValidEnum.Default)]
        [InlineData(ValidEnum.One, ValidEnum.Two)]
        public void NextDistinct_SpecificValues_OneValueAllowed_DifferentValue(
            ValidEnum allowed,
            ValidEnum different) {

            var values = new HashSet<ValidEnum> { allowed };
            var generator = new EnumGenerator<ValidEnum>(values);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(different);

                Assert.NotEqual(different, value);
                Assert.False(generator.EqualityComparer.Equals(different, value));

                Assert.Equal(allowed, value);
                Assert.True(generator.EqualityComparer.Equals(allowed, value));
            }
        }

        [Theory]
        [InlineData(ValidEnum.Default)]
        [InlineData(ValidEnum.One)]
        public void NextDistinct_SpecificValues_OneValueAllowed_SameValue(ValidEnum exclusive) {
            var values = new HashSet<ValidEnum> { exclusive };
            var generator = new EnumGenerator<ValidEnum>(values);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextDistinct(exclusive)
            );
        }

        public static IEnumerable<object[]> NextDistinct_SpecificValues_Data {
            get {
                return new List<object[]> {
                    new object[] {
                        new HashSet<ValidEnum> { ValidEnum.Default, ValidEnum.Five }
                    },
                    new object[] {
                        new HashSet<ValidEnum> { ValidEnum.One, ValidEnum.Two }
                    },
                    new object[] {
                        new HashSet<ValidEnum> {
                            ValidEnum.Default,
                            ValidEnum.One,
                            ValidEnum.Two,
                            ValidEnum.Three,
                            ValidEnum.Four,
                            ValidEnum.Five
                        }
                    }
                };
            }
        }

        [Fact]
        public void NextDistinct_DefaultValues() {
            var generator = new EnumGenerator<ValidEnum>();
            var previousValue = generator.Next();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(previousValue);

                Assert.NotEqual(previousValue, value);
                Assert.False(generator.EqualityComparer.Equals(previousValue, value));

                previousValue = value;
            }
        }

        [Theory]
        [MemberData(nameof(NextDistinct_SpecificValues_Data))]
        public void NextDistinct_SpecificValues(ISet<ValidEnum> values) {
            var generator = new EnumGenerator<ValidEnum>(values);
            var previousValue = generator.Next();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(previousValue);

                Assert.NotEqual(previousValue, value);
                Assert.False(generator.EqualityComparer.Equals(previousValue, value));

                previousValue = value;
            }
        }

        public enum EmptyEnum {}

        public enum OneEnum {

            One = 1

        }

        public enum ValidEnum {

            Default = 0,
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5

        }

    }

}
