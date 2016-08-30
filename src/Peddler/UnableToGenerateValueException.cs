using System;

namespace Peddler {

    public class UnableToGenerateValueException : InvalidOperationException {

        public UnableToGenerateValueException() :
            base() {}

        public UnableToGenerateValueException(String message) :
            base(message) {}

        public UnableToGenerateValueException(String message, Exception innerException) :
            base(message, innerException) {}

    }

}
