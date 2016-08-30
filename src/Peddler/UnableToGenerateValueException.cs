using System;

namespace Peddler {

    public class UnableToGenerateValueException : ArgumentException {

        public UnableToGenerateValueException() :
            base() {}

        public UnableToGenerateValueException(String message) :
            base(message) {}

        public UnableToGenerateValueException(String message, String paramName) :
            base(message, paramName) {}

        public UnableToGenerateValueException(String message, Exception innerException) :
            base(message, innerException) {}

        public UnableToGenerateValueException(String message, String paramName, Exception innerException) :
            base(message, paramName, innerException) {}

    }

}
