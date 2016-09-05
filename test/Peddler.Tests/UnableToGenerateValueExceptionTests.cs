using System;
using Xunit;

namespace Peddler {

    public class UnableToGenerateValueExceptionTests {

        [Fact]
        public void WithDefaults() {

            // Act
            var exception = Throw<UnableToGenerateValueException>(
                () => { throw new UnableToGenerateValueException(); }
            );

            // Assert
            Assert.Equal("Value does not fall within the expected range.", exception.Message);
            Assert.Null(exception.ParamName);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void WithMessage() {

            // Arrange
            var message = Guid.NewGuid().ToString();

            // Act
            var exception = Throw<UnableToGenerateValueException>(
                () => { throw new UnableToGenerateValueException(message); }
            );

            Assert.Equal(message, exception.Message);
            Assert.Null(exception.ParamName);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void WithMessageAndParamName() {

            // Arrange
            var message = Guid.NewGuid().ToString();
            var paramName = Guid.NewGuid().ToString();

            // Act
            var exception = Throw<UnableToGenerateValueException>(
                () => { throw new UnableToGenerateValueException(message, paramName); }
            );

            // Assert
            Assert.Equal(message + GetParameterNameSuffix(paramName), exception.Message);
            Assert.Equal(paramName, exception.ParamName);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void WithMessageAndInnerException() {

            // Arrange
            var message = Guid.NewGuid().ToString();
            var inner = new Exception();

            // Act
            var exception = Throw<UnableToGenerateValueException>(
                () => { throw new UnableToGenerateValueException(message, inner); }
            );

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Null(exception.ParamName);
            Assert.Equal(inner, exception.InnerException);
        }

        [Fact]
        public void WithMessageAndParamNameAndInnerException() {

            // Arrange
            var message = Guid.NewGuid().ToString();
            var paramName = Guid.NewGuid().ToString();
            var inner = new Exception();

            // Act
            var exception = Throw<UnableToGenerateValueException>(
                () => { throw new UnableToGenerateValueException(message, paramName, inner); }
            );

            // Assert
            Assert.Equal(message + GetParameterNameSuffix(paramName), exception.Message);
            Assert.Equal(paramName, exception.ParamName);
            Assert.Equal(inner, exception.InnerException);
        }

        private static T Throw<T>(Action action) where T : Exception {
            return (T)Record.Exception(action);
        }

        private static string GetParameterNameSuffix(String paramName) {
            if (paramName == null) {
                throw new ArgumentNullException(nameof(paramName));
            }

            return Environment.NewLine + $"Parameter name: {paramName}";
        }

    }

}
