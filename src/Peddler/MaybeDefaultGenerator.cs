using System;
using System.Threading;

namespace Peddler {

    /// <summary>
    ///   An <see cref="IGenerator{T}" /> that may provide a default
    ///   value for <typeparamref name="T" /> in place of a value an inner
    ///   <see cref="IGenerator{T}" /> might otherwise provide.
    /// </summary>
    /// <remarks>
    ///   This is most useful for periodically generating 'null' for
    ///   <see cref="Nullable{T}" /> instances or for reference tyes.
    /// </remarks>
    public class MaybeDefaultGenerator<T> : IGenerator<T> {

        private static ThreadLocal<Random> random { get; } =
            new ThreadLocal<Random>(() => new Random());

        /// <summary>
        ///   The value that is considered "the default value" that will
        ///   be occasionally be returned by this generator.
        /// </summary>
        public T DefaultValue { get; }

        private IGenerator<T> inner { get; }
        private decimal percentage { get; }

        /// <summary>
        ///   Instantiates a <see cref="MaybeDefaultGenerator{T}" /> that will
        ///   provide the default value for <typeparamref name="T" /> 15% of the time.
        ///   The other 85% of the time, it provides a value for <typeparamref name="T" />
        ///   based upon the <paramref name="inner" /> <see cref="IGenerator{T}" />.
        /// </summary>
        /// <param name="inner">
        ///   An <see cref="IGenerator{T}" /> implementation that will be used
        ///   when the <see cref="MaybeDefaultGenerator{T}" /> opts not to inject
        ///   the default value for <typeparamref name="T" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        public MaybeDefaultGenerator(IGenerator<T> inner) :
            this(inner, default(T), 0.15m) {}

        /// <summary>
        ///   Instantiates a <see cref="MaybeDefaultGenerator{T}" /> that will use the value
        ///   provided by the <paramref name="defaultValue" /> parameter 15% of the time.
        ///   The other 85% of the time, it provides a value for <typeparamref name="T" />
        ///   based upon the <paramref name="inner" /> <see cref="IGenerator{T}" />.
        /// </summary>
        /// <param name="inner">
        ///   An <see cref="IGenerator{T}" /> implementation that will be used
        ///   when the <see cref="MaybeDefaultGenerator{T}" /> opts not to inject
        ///   the value provided via the <paramref name="defaultValue" /> parameter.
        /// </param>
        /// <param name="defaultValue">
        ///   The value of type <typeparamref name="T" /> that the consumer
        ///   would consider "the default value" for this generator.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        public MaybeDefaultGenerator(IGenerator<T> inner, T defaultValue) :
            this(inner, defaultValue, 0.15m) {}

        /// <summary>
        ///   Instantiates a <see cref="MaybeDefaultGenerator{T}" /> that will
        ///   provide the default value for <typeparamref name="T" /> by the
        ///   percentage defined via the <paramname ref="percentage" /> parameter.
        ///   If the <see cref="MaybeDefaultGenerator{T}" /> opts not to provide
        ///   the default value, it will provide a value for <typeparamref name="T" />
        ///   based upon the <paramref name="inner" /> generator passed in instead.
        /// </summary>
        /// <param name="inner">
        ///   An <see cref="IGenerator{T}" /> implementation that will be used
        ///   when the <see cref="MaybeDefaultGenerator{T}" /> opts not to inject
        ///   the default value for <typeparamref name="T" />.
        /// </param>
        /// <param name="percentage">
        ///   A <see cref="Decimal" /> between 0.0 and 1.0 that represents a
        ///   percentage (0% to 100%, respectively) of the time that the default
        ///   value for <typeparamref name="T" /> will be used instead of a value
        ///   created by the <paramref name="inner" /> <see cref="IGenerator{T}" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="percentage" /> is less than 0.0 or
        ///   or greater than 1.0.
        /// </exception>
        public MaybeDefaultGenerator(IGenerator<T> inner, decimal percentage) :
            this(inner, default(T), percentage) {}

        /// <summary>
        ///   Instantiates a <see cref="MaybeDefaultGenerator{T}" /> that will use the
        ///   value provided by the <paramref name="defaultValue" /> parameter by the
        ///   percentage defined via the <paramname ref="percentage" /> parameter.
        ///   If the <see cref="MaybeDefaultGenerator{T}" /> opts not to provide
        ///   the default value, it will provide a value for <typeparamref name="T" />
        ///   based upon the <paramref name="inner" /> generator passed in instead.
        /// </summary>
        /// <param name="inner">
        ///   An <see cref="IGenerator{T}" /> implementation that will be used
        ///   when the <see cref="MaybeDefaultGenerator{T}" /> opts not to inject
        ///   the value provided by the <paramref name="defaultValue" /> parameter.
        /// </param>
        /// <param name="defaultValue">
        ///   The value of type <typeparamref name="T" /> that the consumer
        ///   would consider "the default value" for this generator.
        /// </param>
        /// <param name="percentage">
        ///   A <see cref="Decimal" /> between 0.0 and 1.0 that represents a
        ///   percentage (0% to 100%, respectively) of the time that the value provided by
        ///   the <paramref name="defaultValue" /> parameter will be used instead of a
        ///   value created by the <paramref name="inner" /> <see cref="IGenerator{T}" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="inner" /> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Thrown when <paramref name="percentage" /> is less than 0.0 or
        ///   or greater than 1.0.
        /// </exception>
        public MaybeDefaultGenerator(IGenerator<T> inner, T defaultValue, decimal percentage) {
            if (inner == null) {
                throw new ArgumentNullException(nameof(inner));
            }

            if (percentage < 0m || percentage > 1.0m) {
                throw new ArgumentOutOfRangeException(
                    nameof(percentage),
                    $"'{nameof(percentage)}' must be between 0.0 and 1.0"
                );
            }

            this.DefaultValue = defaultValue;

            this.inner = inner;
            this.percentage = percentage;
        }

        /// <summary>
        ///   Determines whether or not the default value of <typeparamref name="T" />
        ///   should be returned instead of a value from the inner <see cref="IGenerator{T}" />.
        /// </summary>
        protected bool MaybeDefault() {
            return (percentage - ((decimal)random.Value.NextDouble())) >= 0m;
        }

        /// <inheritdoc />
        public T Next() {
            if (this.MaybeDefault()) {
                return this.DefaultValue;
            }

            return this.inner.Next();
        }

    }

}
