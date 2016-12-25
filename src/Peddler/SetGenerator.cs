using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Peddler {

    /// <summary>
    ///   An <see cref="IDistinctGenerator{T}" /> that provides values from an
    ///   injected set of instances of type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">
    ///   The C#/.NET Type of value generated.
    /// </typeparam>
    public class SetGenerator<T> : IDistinctGenerator<T> {

        private static ThreadLocal<Random> random { get; } =
            new ThreadLocal<Random>(() => new Random());

        /// <inheritdoc />
        public virtual IEqualityComparer<T> EqualityComparer { get; }

        private ImmutableArray<T> valuesLookup { get; }

        /// <summary>
        ///   Instantiates a <see cref="SetGenerator{T}" /> that will
        ///   provide values of type <typeparamref name="T" /> from the
        ///   provided <see cref="ISet{T}" /> of <paramref name="values" />.
        /// </summary>
        /// <remarks>
        ///   Equality of values of <typeparamref name="T" /> is performed
        ///   using <see cref="EqualityComparer{T}.Default" />.
        /// </remarks>
        /// <param name="values">
        ///   A set of values the generator will choose from with equal
        ///   weight when providing values to the caller. Duplicates,
        ///   as determined by <see cref="EqualityComparer{T}.Default" />,
        ///   are removed.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="values" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   Thrown when <paramref name="values" /> is empty.
        /// </exception>
        public SetGenerator(ISet<T> values) :
            this(values, EqualityComparer<T>.Default) {}

        /// <summary>
        ///   Instantiates a <see cref="SetGenerator{T}" /> that will
        ///   provide values of type <typeparamref name="T" /> from the
        ///   provided <see cref="ISet{T}" /> of <paramref name="values" />.
        /// </summary>
        /// <remarks>
        ///   Equality of values of <typeparamref name="T" /> is performed
        ///   using <paramref name="comparer" />.
        /// </remarks>
        /// <param name="values">
        ///   A set of values the generator will choose from with equal
        ///   weight when providing values to the caller. Duplicates,
        ///   as determined by <paramref name="comparer" />, are removed.
        /// </param>
        /// <param name="comparer">
        ///   The comparison used to determine if two instances of
        ///   <typeparamref name="T" /> are equal.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="values" /> or
        ///   <paramref name="comparer" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   Thrown when <paramref name="values" /> is empty.
        /// </exception>
        public SetGenerator(ISet<T> values, IEqualityComparer<T> comparer) {
            if (values == null) {
                throw new ArgumentNullException(nameof(values));
            }

            if (comparer == null) {
                throw new ArgumentNullException(nameof(comparer));
            }

            if (!values.Any()) {
                throw new ArgumentException(
                    $"The '{nameof(values)}' argument must be non-empty.",
                    nameof(values)
                );
            }

            this.EqualityComparer = comparer;

            this.valuesLookup =
                values
                    .Distinct(this.EqualityComparer)
                    .ToImmutableArray();
        }

        /// <inheritdoc />
        public T Next() {
            return this.valuesLookup[random.Value.Next(0, this.valuesLookup.Length)];
        }

        /// <inheritdoc />
        public T NextDistinct(T other) {
            var currentIndex = this.valuesLookup.IndexOf(other, 0, this.EqualityComparer);

            if (currentIndex == -1) {
                return this.Next();
            }

            if (this.valuesLookup.Length == 1) {
                throw new UnableToGenerateValueException(
                    $"The only value this SetGenerator<{typeof(T).Name}> can generate is " +
                    $"'{other}'. Since '{other}' was passed in via the '{nameof(other)}' " +
                    $"argument, this SetGenerator<{typeof(T).Name} is unable to generate " +
                    $"a distinct value.",
                    nameof(other)
                );
            }

            var nextIndex = random.Value.Next(0, this.valuesLookup.Length - 1);

            if (currentIndex == nextIndex) {
                nextIndex++;
            }

            return this.valuesLookup[nextIndex];
        }

    }

}
