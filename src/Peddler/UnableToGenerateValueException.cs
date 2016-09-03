using System;

namespace Peddler {

    /// <summary>
    ///   A base exception for when an <see cref="IComparableGenerator{T}" />
    ///   or <see cref="IDistinctGenerator{T}" /> is unable to generate values
    ///   because their internal constraints are incompatible with the value provided.
    /// </summary>
    /// <remarks>
    ///   For example, this would be raised if an <see cref="IComparableGenerator{Int32}" />
    ///   was asked to generate a value that is less than <see cref="Int32.MinValue" />.
    ///   The generator could not fulfill that request as no <see cref="Int32" /> can be
    ///   less than the type's minimum value, so this exception would be thrown.
    /// </remarks>
    public class UnableToGenerateValueException : ArgumentException {

        /// <summary>
        ///   Instantiates an <see cref="UnableToGenerateValueException" /> for use when
        ///   a generator cannot create a value that fulfills the caller's request.
        /// </summary>
        public UnableToGenerateValueException() :
            base() {}

        /// <summary>
        ///   Instantiates an <see cref="UnableToGenerateValueException" /> for use when
        ///   a generator cannot create a value that fulfills the caller's request.
        /// </summary>
        /// <param name="message">
        ///   An explanation as to why the generator could not create a value.
        /// </param>
        public UnableToGenerateValueException(String message) :
            base(message) {}

        /// <summary>
        ///   Instantiates an <see cref="UnableToGenerateValueException" /> for use when
        ///   a generator cannot create a value that fulfills the caller's request.
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
        ///   Instantiates an <see cref="UnableToGenerateValueException" /> for use when
        ///   a generator cannot create a value that fulfills the caller's request.
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
        ///   Instantiates an <see cref="UnableToGenerateValueException" /> for use when
        ///   a generator cannot create a value that fulfills the caller's request.
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
