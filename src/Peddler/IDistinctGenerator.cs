using System;

namespace Peddler {

    /// <summary>
    ///   A common interface for generating values of type <typeparamref name="T"/>
    ///   that are distinct (i.e. "not equal to") values of <typeparamref name="T"/>.
    /// </summary>
    public interface IDistinctGenerator<T>{

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
        /// <exception cref="Peddler.UnableToGenerateValueException">
        ///   Thrown when this <see cref="Peddler.IComparableGenerator{T}"/> is unable to
        ///   generate a value that is distinct from <paramref name="other" />.
        /// </exception>
        T NextDistinct(T other);

    }


}
