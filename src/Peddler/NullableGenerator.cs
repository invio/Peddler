using System;

namespace Peddler {

    /// <summary>
    ///   Converts all structs of type <typeparamref name="T" /> that are returned from an
    ///   inner <see cref="IGenerator{T}" /> into <see cref="Nullable{T}" />.
    /// </summary>
    public class NullableGenerator<T> : IGenerator<Nullable<T>> where T : struct {

        private IGenerator<T> inner { get; }

        /// <summary>
        ///   Instantiates a <see cref="NullableGenerator{T}" /> that will wrap
        ///   a <see cref="Nullable{T}" /> around any struct <typeparamref name="T" />
        ///   that is created by the <paramref name="inner" /> <see cref="IGenerator{T}" />.
        /// </summary>
        /// <param name="inner">
        ///   A struct-based <see cref="IGenerator{T}" /> implementation that will have its
        ///   results converted into a <see cref="Nullable{T}" /> with the same value.
        /// </param>
        public NullableGenerator(IGenerator<T> inner) {
            if (inner == null) {
                throw new ArgumentNullException(nameof(inner));
            }

            this.inner = inner;
        }

        /// <inheritdoc />
        public Nullable<T> Next() {
            return new Nullable<T>(this.inner.Next());
        }

    }

}
