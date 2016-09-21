using System;
using System.Collections.Generic;

namespace Peddler {

    /// <summary>
    ///   Converts all structs of type <typeparamref name="T" /> that are returned from an
    ///   inner <see cref="IDistinctGenerator{T}" /> into <see cref="Nullable{T}" />.
    /// </summary>
    public class NullableDistinctGenerator<T> :
        NullableGenerator<T>, IDistinctGenerator<Nullable<T>> where T : struct {

        private IDistinctGenerator<T> inner { get; }

        /// <inheritdoc />
        public IEqualityComparer<Nullable<T>> EqualityComparer {
            get { return EqualityComparer<Nullable<T>>.Default; }
        }

        /// <summary>
        ///   Instantiates a <see cref="NullableDistinctGenerator{T}" /> that will wrap a
        ///   <see cref="Nullable{T}" /> around any struct <typeparamref name="T" /> that is
        ///   created by the <paramref name="inner" /> <see cref="IDistinctGenerator{T}" />.
        /// </summary>
        /// <param name="inner">
        ///   A struct-based <see cref="IDistinctGenerator{T}" /> implementation that will
        ///   have its results converted into a <see cref="Nullable{T}" /> with the same value.
        /// </param>
        public NullableDistinctGenerator(IDistinctGenerator<T> inner) : base(inner) {
            this.inner = inner;
        }

        /// <inheritdoc />
        public Nullable<T> NextDistinct(Nullable<T> other) {
            if (this.EqualityComparer.Equals(other, default(Nullable<T>))) {
                return this.Next();
            }

            return new Nullable<T>(this.inner.NextDistinct(other.Value));
        }

    }

}
