using System;
using System.Collections.Generic;

namespace Peddler {

    /// <summary>
    ///   An <see cref="IComparableGenerator{T}" /> that may provide a default
    ///   value for <typeparamref name="T" /> in place of a value an inner
    ///   <see cref="IComparableGenerator{T}" /> might otherwise provide.
    /// </summary>
    /// <remarks>
    ///   This is most useful for periodically generating 'null' for
    ///   <see cref="Nullable{T}" /> instances or for reference tyes.
    /// </remarks>
    public class MaybeDefaultComparableGenerator<T> :
        MaybeDefaultDistinctGenerator<T>, IComparableGenerator<T> {

        /// <inheritdoc />
        public IComparer<T> Comparer {
            get { return this.inner.Comparer; }
        }

        private IComparableGenerator<T> inner { get; }

        /// <summary>
        ///   Instantiates a <see cref="MaybeDefaultComparableGenerator{T}" /> that will
        ///   provide the default value for <typeparamref name="T" /> 15% of the time.
        ///   The other 85% of the time, it provides a value for <typeparamref name="T" />
        ///   based upon the <paramref name="inner" /> <see cref="IComparableGenerator{T}" />.
        /// </summary>
        /// <remarks>
        ///   When the default value for <typeparamref name="T" /> is not a valid value
        ///   to return (for example, when <see cref="NextLessThan" /> is called with
        ///   the default value of <typeparamref name="T" /> being null), the percentage
        ///   liklihood of returning the default value is ignored.
        /// </remarks>
        /// <param name="inner">
        ///   An <see cref="IComparableGenerator{T}" /> implementation that will be used
        ///   when the <see cref="MaybeDefaultComparableGenerator{T}" /> opts not to inject
        ///   the default value for <typeparamref name="T" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        public MaybeDefaultComparableGenerator(IComparableGenerator<T> inner) :
            this(inner, default(T), 0.15m) {}

        /// <summary>
        ///   Instantiates a <see cref="MaybeDefaultComparableGenerator{T}" /> that will use the
        ///   value provided by the <paramref name="defaultValue" /> parameter 15% of the time.
        ///   The other 85% of the time, it provides a value for <typeparamref name="T" />
        ///   based upon the <paramref name="inner" /> <see cref="IComparableGenerator{T}" />.
        /// </summary>
        /// <remarks>
        ///   When the value provided by the <paramref name="defaultValue" /> parameter
        ///   is not a valid value for this <see cref="MaybeDefaultComparableGenerator{T}" />
        ///   to return (for example, when <see cref="NextLessThan" /> is called with
        ///   the value provided for the <paramref name="defaultValue" /> parameter being null),
        ///   the percentage liklihood of returning the default value is ignored.
        /// </remarks>
        /// <param name="inner">
        ///   An <see cref="IComparableGenerator{T}" /> implementation that will be used
        ///   when the <see cref="MaybeDefaultComparableGenerator{T}" /> opts not to inject
        ///   the value provided via the <paramref name="defaultValue" /> parameter.
        /// </param>
        /// <param name="defaultValue">
        ///   The value of type <typeparamref name="T" /> that the consumer
        ///   would consider "the default value" for this generator.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        public MaybeDefaultComparableGenerator(IComparableGenerator<T> inner, T defaultValue) :
            this(inner, defaultValue, 0.15m) {}

        /// <summary>
        ///   Instantiates a <see cref="MaybeDefaultComparableGenerator{T}" /> that
        ///   will provide the default value for <typeparamref name="T" /> by the
        ///   percentage defined via the <paramname ref="percentage" /> parameter.
        ///   If the <see cref="MaybeDefaultComparableGenerator{T}" /> opts not to provide
        ///   the default value, it will provide a value for <typeparamref name="T" />
        ///   based upon the <paramref name="inner" /> generator passed in instead.
        /// </summary>
        /// <param name="inner">
        ///   An <see cref="IComparableGenerator{T}" /> implementation that will be used
        ///   when the <see cref="MaybeDefaultComparableGenerator{T}" /> opts not
        ///   to inject the default value for <typeparamref name="T" />.
        /// </param>
        /// <param name="percentage">
        ///   A <see cref="Decimal" /> between 0.0 and 1.0 that represents a percentage
        ///   (0% to 100%, respectively) of the time that the default value for
        ///   <typeparamref name="T" /> will be used instead of a value created
        ///   by the <paramref name="inner" /> <see cref="IComparableGenerator{T}" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="percentage" /> is less than 0.0 or
        ///   or greater than 1.0.
        /// </exception>
        public MaybeDefaultComparableGenerator(IComparableGenerator<T> inner, decimal percentage) :
            this(inner, default(T), percentage) {}

        /// <summary>
        ///   Instantiates a <see cref="MaybeDefaultComparableGenerator{T}" /> that will use
        ///   the value provided by the <paramref name="defaultValue" /> parameter by the
        ///   percentage defined via the <paramname ref="percentage" /> parameter.
        ///   If the <see cref="MaybeDefaultComparableGenerator{T}" /> opts not to provide
        ///   the default value, it will provide a value for <typeparamref name="T" />
        ///   based upon the <paramref name="inner" /> generator passed in instead.
        /// </summary>
        /// <remarks>
        ///   When the value provided by the <paramref name="defaultValue" /> parameter
        ///   is not a valid value for this <see cref="MaybeDefaultComparableGenerator{T}" />
        ///   to return (for example, when <see cref="NextLessThan" /> is called with
        ///   the value provided for the <paramref name="defaultValue" /> parameter being null),
        ///   the percentage liklihood of returning the default value is ignored.
        /// </remarks>
        /// <param name="inner">
        ///   An <see cref="IComparableGenerator{T}" /> implementation that will be used
        ///   when the <see cref="MaybeDefaultComparableGenerator{T}" /> opts not to inject
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
        ///   created by the <paramref name="inner" /> <see cref="IComparableGenerator{T}" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="percentage" /> is less than 0.0 or
        ///   or greater than 1.0.
        /// </exception>
        public MaybeDefaultComparableGenerator(
            IComparableGenerator<T> inner,
            T defaultValue,
            decimal percentage) :
                base(inner, defaultValue, percentage) {

            this.inner = inner;
        }

        /// <summary>
        ///   Creates a new instance of <typeparamref name="T"/> that is
        ///   greater than the value of <paramref name="other" />.
        /// </summary>
        /// <remarks>
        ///   The percentage liklihood of using
        ///   <see cref="MaybeDefaultGenerator{T}.DefaultValue" /> will not be used if
        ///   <paramref name="other" /> is already greater than
        ///   <see cref="MaybeDefaultGenerator{T}.DefaultValue" />, or if the inner generator
        ///   is unable to provide a value and <paramref name="other" /> is not already
        ///   greater than or equal to <see cref="MaybeDefaultGenerator{T}.DefaultValue" />.
        /// </remarks>
        /// <param name="other">
        ///   An instance of <typeparamref name="T"/> that is a lower, exclusive boundary
        ///   for any value that this <see cref="MaybeDefaultComparableGenerator{T}"/>
        ///   may generate.
        /// </param>
        /// <returns>
        ///   An instance of <typeparamref name="T"/> that is greater than
        ///   <paramref name="other" />.
        /// </returns>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this <see cref="MaybeDefaultComparableGenerator{T}"/> is
        ///   unable to generate a value that is greater than <paramref name="other" />.
        /// </exception>
        public T NextGreaterThan(T other) {
            return this.NextByComparison(
                other,
                comparison => comparison > 0,
                this.inner.NextGreaterThan
            );
        }

        /// <summary>
        ///   Creates a new instance of <typeparamref name="T"/> that is
        ///   greater than or equal to the value of <paramref name="other" />.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     The percentage liklihood of using
        ///     <see cref="MaybeDefaultGenerator{T}.DefaultValue" /> will not be used if
        ///     <paramref name="other" /> is already greater than
        ///     <see cref="MaybeDefaultGenerator{T}.DefaultValue" />, or if the inner
        ///     generator is unable to provide a value and <paramref name="other" /> is not
        ///     already greater than <see cref="MaybeDefaultGenerator{T}.DefaultValue" />.
        ///   </para>
        ///   <para>
        ///     The range of values generated by this
        ///     <see cref="MaybeDefaultComparableGenerator{T}"/> implementation may have
        ///     a lower boundary that is greater than <paramref name="other" />. In that
        ///     case, the value returned will always be greater than <paramref name="other" />.
        ///   </para>
        /// </remarks>
        /// <param name="other">
        ///   An instance of <typeparamref name="T"/> that is a lower, inclusive
        ///   boundary for any value that this <see cref="MaybeDefaultComparableGenerator{T}"/>
        ///   may generate.
        /// </param>
        /// <returns>
        ///   An instance of <typeparamref name="T"/> that is greater than
        ///   or equal to <paramref name="other" />.
        /// </returns>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this <see cref="MaybeDefaultComparableGenerator{T}"/> is unable
        ///   to generate a value that is greater than or equal to <paramref name="other" />.
        /// </exception>
        public T NextGreaterThanOrEqualTo(T other) {
            return this.NextByComparison(
                other,
                comparison => comparison >= 0,
                this.inner.NextGreaterThanOrEqualTo
            );
        }

        /// <summary>
        ///   Creates a new instance of <typeparamref name="T"/> that is
        ///   less than the value of <paramref name="other" />.
        /// </summary>
        /// <remarks>
        ///   The percentage liklihood of using
        ///   <see cref="MaybeDefaultGenerator{T}.DefaultValue" /> will not be used if
        ///   <paramref name="other" /> is already less than
        ///   <see cref="MaybeDefaultGenerator{T}.DefaultValue" />, or if the inner generator
        ///   is unable to provide a value and <paramref name="other" /> is not already
        ///   less than or equal to <see cref="MaybeDefaultGenerator{T}.DefaultValue" />.
        /// </remarks>
        /// <param name="other">
        ///   An instance of <typeparamref name="T"/> that is an upper, exclusive boundary
        ///   for any value that this <see cref="MaybeDefaultComparableGenerator{T}"/>
        ///   may generate.
        /// </param>
        /// <returns>
        ///   An instance of <typeparamref name="T"/> that is less than
        ///   <paramref name="other" />.
        /// </returns>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this <see cref="MaybeDefaultComparableGenerator{T}"/> is
        ///   unable to generate a value that is less than <paramref name="other" />.
        /// </exception>
        public T NextLessThan(T other) {
            return this.NextByComparison(
                other,
                comparison => comparison < 0,
                this.inner.NextLessThan
            );
        }

        /// <summary>
        ///   Creates a new instance of <typeparamref name="T"/> that is
        ///   less than or equal to the value of <paramref name="other" />.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     The percentage liklihood of using
        ///     <see cref="MaybeDefaultGenerator{T}.DefaultValue" /> will not be used
        ///     if <paramref name="other" /> is already less then
        ///     <see cref="MaybeDefaultGenerator{T}.DefaultValue" />, or if the inner
        ///     generator is unable to provide a value and <paramref name="other" /> is not
        ///     already less <see cref="MaybeDefaultGenerator{T}.DefaultValue" />.
        ///   </para>
        ///   <para>
        ///     The range of values generated by this
        ///     <see cref="MaybeDefaultComparableGenerator{T}"/> implementation may have
        ///     an upper boundary that is less than <paramref name="other" />. In that
        ///     case, the value returned will always be less than <paramref name="other" />.
        ///   </para>
        /// </remarks>
        /// <param name="other">
        ///   An instance of <typeparamref name="T"/> that is an upper, inclusive boundary
        ///   for any value that this <see cref="MaybeDefaultComparableGenerator{T}"/>
        ///   may generate.
        /// </param>
        /// <returns>
        ///   An instance of <typeparamref name="T" /> that is less than
        ///   or equal to <paramref name="other" />.
        /// </returns>
        /// <exception cref="UnableToGenerateValueException">
        ///   Thrown when this <see cref="MaybeDefaultComparableGenerator{T}"/> is unable
        ///   to generate a value that is less than or equal to <paramref name="other" />.
        /// </exception>
        public T NextLessThanOrEqualTo(T other) {
            return this.NextByComparison(
                other,
                comparison => comparison <= 0,
                this.inner.NextLessThanOrEqualTo
            );
        }

        private T NextByComparison(T other, Func<int, bool> isDefaultPossible, Func<T, T> next) {
            var comparison = this.inner.Comparer.Compare(this.DefaultValue, other);

            if (!isDefaultPossible(comparison)) {
                return next(other);
            }

            if (this.MaybeDefault()) {
                return this.DefaultValue;
            }

            try {

                // We have already established that 'this.DefaultValue' is a valid value
                // to return in this circumstance, so we will return it even though
                // we didn't happen to pick it randomly this time.

                // MaybeDefault() is a guideline based on the percentages.
                // We will still return a valid value if that is at all possible.

                return next(other);
            } catch (UnableToGenerateValueException) {
                return this.DefaultValue;
            }
        }

    }

}
