using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Peddler {

    /// <summary>
    ///   An <see cref="IGenerator{TList}" /> that will provide
    ///   immutable lists of values of type <typeparamref name="T" />.
    /// </summary>
    public class ListOfGenerator<T> : IGenerator<IList<T>> {

        private static ThreadLocal<Random> random { get; } =
            new ThreadLocal<Random>(() => new Random());

        private IGenerator<T> inner { get; }
        private int minimumSize { get; }
        private int maximumSize { get; }

        /// <summary>
        ///   Instantiates a <see cref="ListOfGenerator{T}" /> that will
        ///   provide lists of values of type <typeparamref name="T" />
        ///   that range in size between 1 and 10 values.
        /// </summary>
        /// <param name="inner">
        ///   An <see cref="IGenerator{T}" /> implementation that will
        ///   be used to generate values of type <typeparamref name="T" />
        ///   that are ultimately put into an immutable list.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        public ListOfGenerator(IGenerator<T> inner) : this(inner, 1, 10) {}

        /// <summary>
        ///   Instantiates a <see cref="ListOfGenerator{T}" /> that will
        ///   provide lists of values of type <typeparamref name="T" />
        ///   that are of a size equal to <paramref name="numberOfValues" />.
        /// </summary>
        /// <param name="inner">
        ///   An <see cref="IGenerator{T}" /> implementation that will
        ///   be used to generate values of type <typeparamref name="T" />
        ///   that are ultimately put into an immutable list.
        /// </param>
        /// <param name="numberOfValues">
        ///   The size of the lists that will be generated.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="numberOfValues" /> is
        ///   less than zero or equal to Int32.MaxValue.
        /// </exception>
        public ListOfGenerator(IGenerator<T> inner, int numberOfValues) {
            if (inner == null) {
                throw new ArgumentNullException(nameof(inner));
            }

            if (numberOfValues < 0 || numberOfValues == Int32.MaxValue) {
                throw new ArgumentOutOfRangeException(
                    nameof(numberOfValues),
                    $"'{nameof(numberOfValues)}' ({numberOfValues:N0}) must be " +
                    $"greater than or equal to zero, and less than Int32.MaxValue."
                );
            }

            this.inner = inner;
            this.minimumSize = numberOfValues;
            this.maximumSize = numberOfValues;
        }

        /// <summary>
        ///   Instantiates a <see cref="ListOfGenerator{T}" /> that will
        ///   provide lists of values of type <typeparamref name="T" />
        ///   that are of a size that is greater than or equal to
        ///   <paramref name="minimumSize" />, but less than or equal to
        ///   <paramref name="maximumSize" />.
        /// </summary>
        /// <param name="inner">
        ///   An <see cref="IGenerator{T}" /> implementation that will
        ///   be used to generate values of type <typeparamref name="T" />
        ///   that are ultimately put into an immutable list.
        /// </param>
        /// <param name="minimumSize">
        ///   The inclusive, minimum boundary for the size of the lists
        ///   that will be generated.
        /// </param>
        /// <param name="maximumSize">
        ///   The inclusive, maximum boundary for the size of the lists
        ///   that will be generated.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="minimumSize" />
        ///   or <paramref name="maximumSize" /> is less
        ///   than zero or equal to Int32.MaxValue.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   Thrown when <paramref name="minimumSize" />
        ///   is greater than <paramref name="maximumSize" />.
        /// </exception>
        public ListOfGenerator(IGenerator<T> inner, int minimumSize, int maximumSize) {
            if (inner == null) {
                throw new ArgumentNullException(nameof(inner));
            }

            if (minimumSize < 0 || minimumSize == Int32.MaxValue) {
                throw new ArgumentOutOfRangeException(
                    nameof(minimumSize),
                    $"'{nameof(minimumSize)}' ({minimumSize:N0}) must be greater " +
                    $"than or equal to zero, and less than Int32.MaxValue."
                );
            }

            if (maximumSize < 0 || maximumSize == Int32.MaxValue) {
                throw new ArgumentOutOfRangeException(
                    nameof(maximumSize),
                    $"'{nameof(maximumSize)}' ({maximumSize:N0}) must be greater " +
                    $"than or equal to zero, and less than Int32.MaxValue."
                );
            }

            if (minimumSize > maximumSize) {
                throw new ArgumentException(
                    $"The '{nameof(minimumSize)}' argument ({minimumSize:N0}) must not be " +
                    $"greater than the '{nameof(maximumSize)}' argument ({maximumSize:N0})."
                );
            }

            this.inner = inner;
            this.minimumSize = minimumSize;
            this.maximumSize = maximumSize;
        }

        /// <inheritdoc />
        public IList<T> Next() {
            return
                Enumerable
                    .Range(0, random.Value.Next(this.minimumSize, this.maximumSize + 1))
                    .Select(_ => this.inner.Next())
                    .ToImmutableList();
        }

    }

}
