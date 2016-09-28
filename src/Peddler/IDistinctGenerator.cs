using System;
using System.Collections.Generic;

namespace Peddler {

    /// <summary>
    ///   A common interface for generating values of type <typeparamref name="T"/>
    ///   that are distinct (i.e. "not equal to") values of <typeparamref name="T"/>.
    /// </summary>
    public interface IDistinctGenerator<T> : IGenerator<T> {

        /// <summary>
        ///   An <see cref="IEqualityComparer{T}" /> that contains the evaluation used
        ///   to determine if two instances of <typeparamref name="T" /> are considered
        ///   distinct from the perspective of this generator.
        /// </summary>
        IEqualityComparer<T> EqualityComparer { get; }

        /// <summary>
        ///   Creates a new instance of <typeparamref name="T"/> that is
        ///   distinct from the value of <paramref name="other" />.
        /// </summary>
        /// <param name="other">
        ///   An instance of <typeparamref name="T"/> that will be distinct from
        ///   (i.e. "not equal to") the resultant value.
        /// </param>
        /// <returns>
        ///   An instance of <typeparamref name="T"/> that is distinct (i.e. "not equal to")
        ///   <paramref name="other" />.
        /// </returns>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this <see cref="IDistinctGenerator{T}"/> is unable to
        ///   generate a value that is distinct from <paramref name="other" />.
        /// </exception>
        T NextDistinct(T other);

    }


}
