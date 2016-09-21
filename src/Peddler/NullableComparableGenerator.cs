using System;
using System.Collections.Generic;

namespace Peddler {

    /// <summary>
    ///   Converts all structs of type <typeparamref name="T" /> that are returned from an
    ///   inner <see cref="IComparableGenerator{T}" /> into <see cref="Nullable{T}" />.
    /// </summary>
    public class NullableComparableGenerator<T> :
        NullableDistinctGenerator<T>, IComparableGenerator<Nullable<T>> where T : struct {

        private IComparableGenerator<T> inner { get; }

        /// <inheritdoc />
        public IComparer<Nullable<T>> Comparer {
            get { return Comparer<Nullable<T>>.Default; }
        }

        /// <summary>
        ///   Instantiates a <see cref="NullableComparableGenerator{T}" /> that will wrap a
        ///   <see cref="Nullable{T}" /> around any struct <typeparamref name="T" /> that is
        ///   created by the <paramref name="inner" /> <see cref="IComparableGenerator{T}" />.
        /// </summary>
        /// <remarks>
        ///   For all implemented <see cref="IComparableGenerator{T}" /> methods, the
        ///   <see cref="NullableComparableGenerator{T}" /> will return a
        ///   <see cref="Nullable{T}" /> with a value of null rather than throw an
        ///   <see cref="UnableToGenerateValueException" /> if the inner
        ///   <see cref="IComparableGenerator{T}" /> cannot fulfill the request.
        /// </remarks>
        /// <param name="inner">
        ///   A struct-based <see cref="IComparableGenerator{T}" /> implementation that will
        ///   have its results converted into a <see cref="Nullable{T}" /> with the same value.
        /// </param>
        public NullableComparableGenerator(IComparableGenerator<T> inner) : base (inner) {
            this.inner = inner;
        }

        /// <inheritdoc />
        public Nullable<T> NextLessThan(Nullable<T> other) {
            if (this.Comparer.Compare(other, default(Nullable<T>)) == 0) {
                throw new UnableToGenerateValueException(
                    $"Since default(Nullable<{typeof(T).Name}>) was passed in via the " +
                    $"'{nameof(other)}' parameter, no lower value can be generated.",
                    nameof(other)
                );
            }

            try {
                return new Nullable<T>(this.inner.NextLessThan(other.Value));
            } catch (UnableToGenerateValueException) {
                return default(Nullable<T>);
            }
        }

        /// <inheritdoc />
        public Nullable<T> NextLessThanOrEqualTo(Nullable<T> other) {
            if (this.Comparer.Compare(other, default(Nullable<T>)) == 0) {
                return default(Nullable<T>);
            }

            try {
                return new Nullable<T>(this.inner.NextLessThanOrEqualTo(other.Value));
            } catch (UnableToGenerateValueException) {
                return default(Nullable<T>);
            }
        }

        /// <inheritdoc />
        public Nullable<T> NextGreaterThan(Nullable<T> other) {
            if (this.Comparer.Compare(other, default(Nullable<T>)) == 0) {
                return this.Next();
            }

            return new Nullable<T>(this.inner.NextGreaterThan(other.Value));
        }

        /// <inheritdoc />
        public Nullable<T> NextGreaterThanOrEqualTo(Nullable<T> other) {
            if (this.Comparer.Compare(other, default(Nullable<T>)) == 0) {
                return this.Next();
            }

            return new Nullable<T>(this.inner.NextGreaterThanOrEqualTo(other.Value));
        }

    }

}
