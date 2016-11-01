using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Peddler {

    public class ListOfGeneratorTests {

        private const int numberOfAttempts = 100;

        [Fact]
        public void Constructor_DefaultSizes_NullInnerGenerator() {

            // Arrange

            IGenerator<Object> inner = null;

            // Act

            var exception = Record.Exception(
                () => new ListOfGenerator<Object>(inner)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Constructor_ConstantSize_NullInnerGenerator() {

            // Arrange

            IGenerator<Object> inner = null;

            // Act

            var exception = Record.Exception(
                () => new ListOfGenerator<Object>(inner, 10)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Constructor_ConfiguredSizes_NullInnerGenerator() {

            // Arrange

            IGenerator<Object> inner = null;

            // Act

            var exception = Record.Exception(
                () => new ListOfGenerator<Object>(inner, 5, 15)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(Int32.MinValue)]
        public void Constructor_NumberOfValues_LessThanZero(int numberOfValues) {

            // Arrange

            var inner = new StringGenerator();

            // Act

            var exception = Record.Exception(
                () => new ListOfGenerator<String>(inner, numberOfValues)
            );

            // Assert

            Assert.IsType<ArgumentOutOfRangeException>(exception);
            Assert.Equal(
                $"'numberOfValues' ({numberOfValues:N0}) must be greater " +
                $"than or equal to zero, and less than Int32.MaxValue." +
                Environment.NewLine + "Parameter name: numberOfValues",
                exception.Message
            );
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(Int32.MinValue)]
        [InlineData(Int32.MaxValue)]
        public void Constructor_ConfiguredSizes_InvalidMinimumSize(int minimumSize) {

            // Arrange

            var inner = new StringGenerator();

            // Act

            var exception = Record.Exception(
                () => new ListOfGenerator<String>(inner, minimumSize, 100)
            );

            // Assert

            Assert.IsType<ArgumentOutOfRangeException>(exception);
            Assert.Equal(
                $"'minimumSize' ({minimumSize:N0}) must be greater " +
                $"than or equal to zero, and less than Int32.MaxValue." +
                Environment.NewLine + "Parameter name: minimumSize",
                exception.Message
            );
        }

        [Fact]
        public void Constructor_ConfiguredSizes_BothAreInt32MaxValue() {

            // Arrange

            var inner = new StringGenerator();

            // Act

            var exception = Record.Exception(
                () => new ListOfGenerator<String>(inner, Int32.MaxValue, Int32.MaxValue)
            );

            // Assert

            Assert.IsType<ArgumentOutOfRangeException>(exception);
            Assert.Equal(
                $"'minimumSize' ({Int32.MaxValue:N0}) must be greater " +
                $"than or equal to zero, and less than Int32.MaxValue." +
                Environment.NewLine + "Parameter name: minimumSize",
                exception.Message
            );
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(Int32.MinValue)]
        [InlineData(Int32.MaxValue)]
        public void Constructor_ConfiguredSizes_InvalidMaximumSize(int maximumSize) {

            // Arrange

            var inner = new StringGenerator();

            // Act

            var exception = Record.Exception(
                () => new ListOfGenerator<String>(inner, 1, maximumSize)
            );

            // Assert

            Assert.IsType<ArgumentOutOfRangeException>(exception);
            Assert.Equal(
                $"'maximumSize' ({maximumSize:N0}) must be greater " +
                $"than or equal to zero, and less than Int32.MaxValue." +
                Environment.NewLine + "Parameter name: maximumSize",
                exception.Message
            );
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(10, 5)]
        [InlineData(5, 2)]
        public void Constructor_ConfiguredSizes_MinimumGreaterThanMaximum(
            int minimumSize,
            int maximumSize) {

            // Arrange

            var inner = new StringGenerator();

            // Act

            var exception = Record.Exception(
                () => new ListOfGenerator<String>(inner, minimumSize, maximumSize)
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);
            Assert.Equal(
                $"The 'minimumSize' argument ({minimumSize:N0}) must not be " +
                $"greater than the 'maximumSize' argument ({maximumSize:N0}).",
                exception.Message
            );
        }

        [Fact]
        public void Next_DefaultsSizes() {
            var generator =
                new ListOfGenerator<String>(new StringGenerator());

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.NotNull(value);
                AssertCountBetween(value, 1, 10);
                AssertImmutable(value);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public void Next_ConstantSize(int numberOfValues) {
            var generator =
                new ListOfGenerator<String>(new StringGenerator(), numberOfValues);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.NotNull(value);
                AssertCountBetween(value, numberOfValues, numberOfValues);
                AssertImmutable(value);
            }
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(2, 4)]
        [InlineData(1, 100)]
        public void Next_ConfiguredSizes(int minimumSize, int maximumSize) {
            var generator =
                new ListOfGenerator<String>(new StringGenerator(), minimumSize, maximumSize);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.Next();

                Assert.NotNull(value);
                AssertCountBetween(value, minimumSize, maximumSize);
                AssertImmutable(value);
            }
        }

        private void AssertCountBetween<T>(IList<T> list, int low, int high) {
            if (list == null) {
                throw new ArgumentNullException(nameof(list));
            }

            Assert.True(
                list.Count >= low,
                $"Expected list to have a count that is greater than " +
                $"or equal to {low:N0}, but it was {list.Count:N0}."
            );

            Assert.True(
                list.Count <= high,
                $"Expected list to have a count that is less than " +
                $"or equal to {high:N0}, but it was {list.Count:N0}."
            );
        }

        private void AssertImmutable(IList<String> list) {
            if (list == null) {
                throw new ArgumentNullException(nameof(list));
            }

            Assert.Throws<NotSupportedException>(
                () => list.Add("Foo")
            );
        }

    }

}
