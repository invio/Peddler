using System;
using System.Collections.Generic;

namespace Peddler {

    /// <summary>
    ///   An <see cref="IDistinctGenerator{T}" /> that may provide a default
    ///   value for <typeparamref name="T" /> in place of a value an inner
    ///   <see cref="IDistinctGenerator{T}" /> might otherwise provide.
    /// </summary>
    /// <remarks>
    ///   This is most useful for periodically generating 'null' for
    ///   <see cref="Nullable{T}" /> instances or for reference tyes.
    /// </remarks>
    public class MaybeDefaultDistinctGenerator<T> :
        MaybeDefaultGenerator<T>, IDistinctGenerator<T> {

        /// <inheritdoc />
        public IEqualityComparer<T> EqualityComparer {
            get { return this.inner.EqualityComparer; }
        }

        private IDistinctGenerator<T> inner { get; }

        /// <summary>
        ///   Instantiates a <see cref="MaybeDefaultDistinctGenerator{T}" /> that will
        ///   provide the default value for <typeparamref name="T" /> 15% of the time.
        ///   The other 85% of the time, it provides a value for <typeparamref name="T" />
        ///   based upon the <paramref name="inner" /> <see cref="IDistinctGenerator{T}" />.
        /// </summary>
        /// <remarks>
        ///   When the default value for <typeparamref name="T" /> is not a valid value
        ///   to return (for example, when <see cref="NextDistinct" /> is called with
        ///   the default value of <typeparamref name="T" />), the percentage liklihood
        ///   of returning the default value is ignored.
        /// </remarks>
        /// <param name="inner">
        ///   An <see cref="IDistinctGenerator{T}" /> implementation that will be used
        ///   when the <see cref="MaybeDefaultDistinctGenerator{T}" /> opts not to inject
        ///   the default value for <typeparamref name="T" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        public MaybeDefaultDistinctGenerator(IDistinctGenerator<T> inner) :
            this(inner, default(T), 0.15m) {}

        /// <summary>
        ///   Instantiates a <see cref="MaybeDefaultDistinctGenerator{T}" /> that will use the
        ///   value provided by the <paramref name="defaultValue" /> parameter 15% of the time.
        ///   The other 85% of the time, it provides a value for <typeparamref name="T" />
        ///   based upon the <paramref name="inner" /> <see cref="IDistinctGenerator{T}" />.
        /// </summary>
        /// <remarks>
        ///   When the value provided by the <paramref name="defaultValue" /> parameter
        ///   is not a valid value for this <see cref="MaybeDefaultDistinctGenerator{T}" />
        ///   to return (for example, when <see cref="NextDistinct" /> is called with
        ///   the value provided for the <paramref name="defaultValue" /> parameter), the
        ///   percentage liklihood of returning the default value is ignored.
        /// </remarks>
        /// <param name="inner">
        ///   An <see cref="IDistinctGenerator{T}" /> implementation that will be used
        ///   when the <see cref="MaybeDefaultDistinctGenerator{T}" /> opts not to inject
        ///   the value provided via the <paramref name="defaultValue" /> parameter.
        /// </param>
        /// <param name="defaultValue">
        ///   The value of type <typeparamref name="T" /> that the consumer
        ///   would consider "the default value" for this generator.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        public MaybeDefaultDistinctGenerator(IDistinctGenerator<T> inner, T defaultValue) :
            this(inner, defaultValue, 0.15m) {}

        /// <summary>
        ///   Instantiates a <see cref="MaybeDefaultDistinctGenerator{T}" /> that will
        ///   provide the default value for <typeparamref name="T" /> by the
        ///   percentage defined via the <paramref name="percentage" /> parameter.
        ///   If the <see cref="MaybeDefaultDistinctGenerator{T}" /> opts not to provide
        ///   the default value, it will provide a value for <typeparamref name="T" />
        ///   based upon the <paramref name="inner" /> generator passed in instead.
        /// </summary>
        /// <remarks>
        ///   When the default value for <typeparamref name="T" /> is not a valid value
        ///   to return (for example, when <see cref="NextDistinct" /> is called with
        ///   the default value of <typeparamref name="T" />), the
        ///   <paramref name="percentage" /> liklihood of returning the default value is
        ///   ignored.
        /// </remarks>
        /// <param name="inner">
        ///   An <see cref="IDistinctGenerator{T}" /> implementation that will be used
        ///   when the <see cref="MaybeDefaultDistinctGenerator{T}" /> opts not to inject
        ///   the default value for <typeparamref name="T" />.
        /// </param>
        /// <param name="percentage">
        ///   A <see cref="Decimal" /> between 0.0 and 1.0 that represents a
        ///   percentage (0% to 100%, respectively) of the time that the default
        ///   value for <typeparamref name="T" /> will be used instead of a value
        ///   created by the <paramref name="inner" /> <see cref="IDistinctGenerator{T}" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="percentage" /> is less than 0.0 or
        ///   or greater than 1.0.
        /// </exception>
        public MaybeDefaultDistinctGenerator(IDistinctGenerator<T> inner, decimal percentage) :
            this(inner, default(T), percentage) {}

        /// <summary>
        ///   Instantiates a <see cref="MaybeDefaultDistinctGenerator{T}" /> that will use
        ///   the value provided by the <paramref name="defaultValue" /> parameter by the
        ///   percentage defined via the <paramname ref="percentage" /> parameter.
        ///   If the <see cref="MaybeDefaultDistinctGenerator{T}" /> opts not to provide
        ///   the default value, it will provide a value for <typeparamref name="T" />
        ///   based upon the <paramref name="inner" /> generator passed in instead.
        /// </summary>
        /// <remarks>
        ///   When the value provided by the <paramref name="defaultValue" /> parameter
        ///   is not a valid value for this <see cref="MaybeDefaultDistinctGenerator{T}" />
        ///   to return (for example, when <see cref="NextDistinct" /> is called with
        ///   the value provided for the <paramref name="defaultValue" /> parameter), the
        ///   percentage liklihood of returning the default value is ignored.
        /// </remarks>
        /// <param name="inner">
        ///   An <see cref="IDistinctGenerator{T}" /> implementation that will be used
        ///   when the <see cref="MaybeDefaultDistinctGenerator{T}" /> opts not to inject
        ///   the value provided via the <paramref name="defaultValue" /> parameter.
        /// </param>
        /// <param name="defaultValue">
        ///   The value of type <typeparamref name="T" /> that the consumer
        ///   would consider "the default value" for this generator.
        /// </param>
        /// <param name="percentage">
        ///   A <see cref="Decimal" /> between 0.0 and 1.0 that represents a
        ///   percentage (0% to 100%, respectively) of the time that the value provided by
        ///   the <paramref name="defaultValue" /> parameter will be used instead of a value
        ///   created by the <paramref name="inner" /> <see cref="IDistinctGenerator{T}" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="percentage" /> is less than 0.0 or
        ///   or greater than 1.0.
        /// </exception>
        public MaybeDefaultDistinctGenerator(
            IDistinctGenerator<T> inner,
            T defaultValue,
            decimal percentage) :
                base(inner, defaultValue, percentage) {

            this.inner = inner;
        }

        /// <summary>
        ///   Creates a new instance of <typeparamref name="T"/> that is
        ///   distinct from the value of <paramref name="other" />.
        /// </summary>
        /// <remarks>
        ///   The percentage distribution of when to use the
        ///   <see cref="MaybeDefaultGenerator{T}.DefaultValue" /> will be
        ///   ignored if the value of <paramref name="other" /> is equal to
        ///   <see cref="MaybeDefaultGenerator{T}.DefaultValue" />, or if
        ///   the inner generator is unable to provide a value and
        ///   <paramref name="other" /> is not equal to
        ///   <see cref="MaybeDefaultGenerator{T}.DefaultValue" />.
        /// </remarks>
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
        public T NextDistinct(T other) {
            if (this.inner.EqualityComparer.Equals(other, this.DefaultValue)) {
                return this.inner.NextDistinct(other);
            }

            if (this.MaybeDefault()) {
                return this.DefaultValue;
            }

            try {

                // If the inner IDistinctGenerator<T> is unable to
                // generate a distinct value, but the value of "other"
                // is not 'DefaultValue', we can still fulfill the contract
                // of the interface if by returning this.DefaultValue.

                return this.inner.NextDistinct(other);
            } catch (UnableToGenerateValueException) {
                return this.DefaultValue;
            }
        }

    }

}
