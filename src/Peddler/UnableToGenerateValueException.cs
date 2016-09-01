using System;

namespace Peddler {

    /// <summary>
    /// A base exception for when an <see cref="T:Peddler.IComparableGenerator`1"/> or an
    /// <see cref="T:Peddler.IDistinctGenerator`1"/> cannot generate value because their
    /// internal restaints are incompatible with the value provided.
    /// </summary>
    /// <remarks>
    /// For example, this would be raised is if an
    /// <see cref="T:Peddler.IComparableGenerator`1" />
    /// generated <see cref="T:System.Int32" />, but the caller asked for a value that
    /// was less than <see cref="M:System.Int32.MinValue" />. The generate could not
    /// fulfill that request as no <see cref="T:System.Int32" /> can be less than the
    /// minimum value, so it would throw this
    /// exception.
    /// </remarks>
    public class UnableToGenerateValueException : ArgumentException {

        /// <summary>
        ///   Instantiates an UnableToGenerateValueException for use when a generator
        ///   cannot create a value that fulfills the caller's request.
        /// </summary>
        public UnableToGenerateValueException() :
            base() {}

        /// <summary>
        ///   Instantiates an UnableToGenerateValueException for use when a generator
        ///   cannot create a value that fulfills the caller's request.
        /// </summary>
        /// <param name="message">
        ///   An explanation as to why the generator could not create a value.
        /// </param>
        public UnableToGenerateValueException(String message) :
            base(message) {}

        /// <summary>
        ///   Instantiates an UnableToGenerateValueException for use when a generator
        ///   cannot create a value that fulfills the caller's request.
        /// </summary>
        /// <param name="message">
        ///   An explanation as to why the generator could not create a value.
        /// </param>
        /// <param name="paramName">
        ///   The name of the parameter the caller provided which referenced a value
        ///   that was incompatible with the generator.
        /// </param>
        public UnableToGenerateValueException(String message, String paramName) :
            base(message, paramName) {}

        /// <summary>
        ///   Instantiates an UnableToGenerateValueException for use when a generator
        ///   cannot create a value that fulfills the caller's request.
        /// </summary>
        /// <param name="message">
        ///   An explanation as to why the generator could not create a value.
        /// </param>
        /// <param name="innerException">
        ///   A lower-level exception that caused this exception to occur.
        /// </param>
        public UnableToGenerateValueException(String message, Exception innerException) :
            base(message, innerException) {}

        /// <summary>
        ///   Instantiates an UnableToGenerateValueException for use when a generator
        ///   cannot create a value that fulfills the caller's request.
        /// </summary>
        /// <param name="message">
        ///   An explanation as to why the generator could not create a value.
        /// </param>
        /// <param name="paramName">
        ///   The name of the parameter the caller provided which referenced a value
        ///   that was incompatible with the generator.
        /// </param>
        /// <param name="innerException">
        ///   A lower-level exception that caused this exception to occur.
        /// </param>
        public UnableToGenerateValueException(
            String message,
            String paramName,
            Exception innerException) :
                base(message, paramName, innerException) {}

    }

}
