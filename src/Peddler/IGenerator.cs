using System;

namespace Peddler {

    /// <summary>
    ///   A common interface for generate values of type <typeparamref name="T"/>.
    /// </summary>
    public interface IGenerator<out T> {

        /// <summary>
        ///   Creates a new instance of <typeparamref name="T"/> based
        ///   upon the constraints injected into this implementation of
        ///   <see cref="Peddler.IGenerator{T}"/>.
        /// </summary>
        /// <returns>
        ///   An instance of <typeparamref name="T"/>.
        /// </returns>
        T Next();

    }

}
