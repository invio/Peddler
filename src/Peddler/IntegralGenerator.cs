using System;

namespace Peddler {

    /// <summary>
    ///   A base implementation of <see cref="IGenerator{T}" />,
    ///   <see cref="IDistinctGenerator{T}" /> and <see cref="IComparableGenerator{T}" />
    ///   for an integral types, such as <see cref="System.Int32" />,
    ///   <see cref="System.Byte" />, and <see cref="System.UInt64" />.
    /// </summary>
    public abstract class IntegralGenerator<TIntegral> :
        IGenerator<TIntegral>, IDistinctGenerator<TIntegral>, IComparableGenerator<TIntegral>
        where TIntegral : struct, IEquatable<TIntegral>, IComparable<TIntegral> {

        /// <summary>
        ///   The inclusive, lower <typeparamref name="TIntegral" /> boundary for this generator.
        /// </summary>
        public TIntegral Low { get; }

        /// <summary>
        ///   The exclusive, upper <typeparamref name="TIntegral" /> boundary for this generator.
        /// </summary>
        public TIntegral High { get; }

        /// <summary>
        ///   Instantiates an <see cref="IntegralGenerator{T}" /> that will create
        ///   <typeparamref name="TIntegral" /> values that range from <paramref name="low" />
        ///   (inclusively) to <paramref name="high" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <typeparamref name="TIntegral" /> boundary for this generator.
        /// </param>
        /// <param name="high">
        ///   The exclusive, upper <typeparamref name="TIntegral" /> boundary for this generator.
        /// </param>
        /// <exception cref="System.ArgumentException">
        ///   Thrown when <paramref name="low" /> is greater than or equal to
        ///   <paramref name="high" />.
        /// </exception>
        protected IntegralGenerator(TIntegral low, TIntegral high) {
            if (low.CompareTo(high) >= 0) {
                throw new ArgumentException(
                    $"The '{nameof(low)}' argument ({low:N0}) must be lower " +
                    $"than the '{nameof(high)}' argument ({high:N0}).",
                    nameof(low)
                );
            }

            this.Low = low;
            this.High = high;
        }

        /// <summary>
        ///   Gets a random <typeparamref name="TIntegral" /> value that exists between
        ///   <paramref name="low" /> (inclusively) and <paramref name="high" /> (exclusively).
        /// </summary>
        /// <param name="low">
        ///   The inclusive, lower <typeparamref name="TIntegral" /> boundary for value.
        /// </param>
        /// <param name="high">
        ///   The exclusive, upper <typeparamref name="TIntegral" /> boundary for value.
        /// </param>
        /// <returns>
        ///   A <typeparamref name="TIntegral" /> that conforms to the boundaries defined
        ///   via <paramref name="low" /> and <paramref name="high" />.
        /// </returns>
        protected abstract TIntegral Next(TIntegral low, TIntegral high);

        /// <summary>
        ///   Takes the provided <typeparamref name="TIntegral" /> value
        ///   and adds exactly one to it.
        /// </summary>
        /// <param name="value">
        ///   The <typeparamref name="TIntegral" /> value which will be incremented by one.
        /// </param>
        /// <returns>
        ///   The value of <paramref name="value" /> plus one.
        /// </returns>
        protected abstract TIntegral AddOne(TIntegral value);

        /// <summary>
        ///   Takes the provided <typeparamref name="TIntegral" /> value
        ///   and subtracts exactly one to it.
        /// </summary>
        /// <param name="value">
        ///   The <typeparamref name="TIntegral" /> value which will be decremented by one.
        /// </param>
        /// <returns>
        ///   The value of <paramref name="value" /> minus one.
        /// </returns>
        protected abstract TIntegral SubtractOne(TIntegral value);

        /// <summary>
        ///   Creates a new integral value of type <typeparamref name="TIntegral"/>
        ///   that will be between <see cref="Low" /> (inclusively) and
        ///   <see cref="High" /> (exclusively).
        /// </summary>
        /// <returns>
        ///   An instance of type <typeparamref name="TIntegral"/> that falls between
        ///   <see cref="Low" /> (inclusively) and <see cref="High" /> (exclusively).
        /// </returns>
        public TIntegral Next() {
            return this.Next(this.Low, this.High);
        }

        /// <summary>
        ///   Creates a new integral value of type <typeparamref name="TIntegral"/>
        ///   that will be between <see cref="Low" /> (inclusively) and
        ///   <see cref="High" /> (exclusively), but will not be equal to
        ///   <paramref name="other" />.
        /// </summary>
        /// <remarks>
        ///   The value provided for <paramref name="other" /> does not, itself,
        ///   need to be between <see cref="Low" /> and <see cref="High" />.
        /// </remarks>
        /// <param name="other">
        ///   A value that will never be equal to the <typeparamref name="TIntegral" />
        ///   that is returned.
        /// </param>
        /// <returns>
        ///   An instance of type <typeparamref name="TIntegral"/> that falls between
        ///   <see cref="Low" /> (inclusively) and <see cref="High" /> (exclusively).
        /// </returns>
        /// <exception cref="Peddler.UnableToGenerateValueException">
        ///   Thrown when this generator is unable to provide a value between
        ///   <see cref="Low" /> (inclusively) and <see cref="High" /> (exclusively).
        ///   This can happen if <see cref="Low" /> and <see cref="High" /> have an
        ///   effective range of one integral value, and that value is equal to the
        ///   one provided by <paramref name="other" />.
        /// </exception>
        public TIntegral NextDistinct(TIntegral other) {
            if (other.CompareTo(this.Low) < 0 || other.CompareTo(this.High) >= 0) {
                return this.Next();
            }

            if (this.Low.Equals(this.SubtractOne(this.High))) {
                throw new UnableToGenerateValueException(
                    $"Since '{nameof(this.Low)}' is {this.Low:N0} and '{nameof(this.High)}' " +
                    $"is {this.High:N0}, only {this.Low:N0} can be generated. The value " +
                    $"provided for '{nameof(other)}' was also {other:N0}, so a " +
                    $"distinct value cannot be generated.",
                    nameof(other)
                );
            }

            // Remove 1 value to represent the range of values minus a single
            // value to represent the one given by the caller as 'other'.
            // If we get a value equal to or greater than 'other', add one.

            // This is better with an example to explain why ...

            // Given the following values:
            //   low = 1
            //   high = 6
            //   other = 3
            //
            // Because the 'high' is an exclusive boundary, the integers we can generate
            // are within the range of [ 1, 2, 3, 4, 5 ]. We remove one to represent
            // the 'other' value, giving us a range of [ 1, 2, 3, 4 ]. One can see that
            // this includes our current value of '3'. However, if we add a one to the
            // result if it is equal to or greater to '3', our range becomes [ 1, 2, 4, 5 ].
            // This gives us a perfectly balanced range of possibilities.

            var nextValue = this.Next(this.Low, this.SubtractOne(this.High));

            if (nextValue.CompareTo(other) >= 0) {
                nextValue = this.AddOne(nextValue);
            }

            return nextValue;
        }

        /// <summary>
        ///   Creates a new integral value of type <typeparamref name="TIntegral"/>
        ///   that will be between <see cref="Low" /> (inclusively) and
        ///   <see cref="High" /> (exclusively) that will be greater than
        ///   <paramref name="other" />.
        /// </summary>
        /// <remarks>
        ///   The value provided for <paramref name="other" /> does not, itself,
        ///   need to be between <see cref="Low" /> and <see cref="High" />.
        /// </remarks>
        /// <param name="other">
        ///   A value that sets an exclusive, lower boundary for the
        ///   <typeparamref name="TIntegral" /> that is returned.
        /// </param>
        /// <returns>
        ///   An instance of type <typeparamref name="TIntegral"/> that falls between
        ///   <see cref="Low" /> (inclusively) and <see cref="High" /> (exclusively),
        ///   but is also greater than <paramref name="other" />.
        /// </returns>
        /// <exception cref="Peddler.UnableToGenerateValueException">
        ///   Thrown when this generator is unable to provide a value between
        ///   <see cref="Low" /> (inclusively) and <see cref="High" /> (exclusively).
        ///   This can happen if <see cref="High" /> is less than or equal to the
        ///   value provided by <paramref name="other" />.
        /// </exception>
        public TIntegral NextGreaterThan(TIntegral other) {
            if (other.CompareTo(this.Low) < 0) {
                return this.Next();
            }

            if (other.CompareTo(this.SubtractOne(this.High)) >= 0) {
                throw new UnableToGenerateValueException(
                    $"Since '{nameof(this.High)}' is {this.High:N0}, the maximum value that " +
                    $"can be generated is {this.SubtractOne(this.High):N0}. Since the value " +
                    $"provided for '{nameof(other)}' was {other:N0}, a value greater than " +
                    $"it cannot be provided by this {this.GetType().Name}.",
                    nameof(other)
                );
            }

            return this.Next(this.AddOne(other), this.High);
        }

        /// <summary>
        ///   Creates a new integral value of type <typeparamref name="TIntegral"/>
        ///   that will be between <see cref="Low" /> (inclusively) and
        ///   <see cref="High" /> (exclusively) that will be greater than or equal to
        ///   <paramref name="other" />.
        /// </summary>
        /// <remarks>
        ///   The value provided for <paramref name="other" /> does not, itself,
        ///   need to be between <see cref="Low" /> and <see cref="High" />.
        /// </remarks>
        /// <param name="other">
        ///   A value that sets an inclusive, lower boundary for the
        ///   <typeparamref name="TIntegral" /> that is returned.
        /// </param>
        /// <returns>
        ///   An instance of type <typeparamref name="TIntegral"/> that falls between
        ///   <see cref="Low" /> (inclusively) and <see cref="High" /> (exclusively),
        ///   but is also greater than or equal to <paramref name="other" />.
        /// </returns>
        /// <exception cref="Peddler.UnableToGenerateValueException">
        ///   Thrown when this generator is unable to provide a value between
        ///   <see cref="Low" /> (inclusively) and <see cref="High" /> (exclusively).
        ///   This can happen if <see cref="High" /> is less than or equal to the
        ///   value provided by <paramref name="other" />.
        /// </exception>
        public TIntegral NextGreaterThanOrEqualTo(TIntegral other) {
            if (other.CompareTo(this.Low) <= 0) {
                return this.Next();
            }

            if (other.CompareTo(this.High) >= 0) {
                throw new UnableToGenerateValueException(
                    $"Since '{nameof(this.High)}' is {this.High:N0}, the maximum value that " +
                    $"can be generated is {this.SubtractOne(this.High):N0}. Since the value " +
                    $"provided for '{nameof(other)}' was {other:N0}, a value greater than or " +
                    $"equal to it cannot be provided by this {this.GetType().Name}.",
                    nameof(other)
                );
            }

            return this.Next(other, this.High);
        }

        /// <summary>
        ///   Creates a new integral value of type <typeparamref name="TIntegral"/>
        ///   that will be between <see cref="Low" /> (inclusively) and
        ///   <see cref="High" /> (exclusively) that will be less than
        ///   <paramref name="other" />.
        /// </summary>
        /// <remarks>
        ///   The value provided for <paramref name="other" /> does not, itself,
        ///   need to be between <see cref="Low" /> and <see cref="High" />.
        /// </remarks>
        /// <param name="other">
        ///   A value that sets an exclusive, upper boundary for the
        ///   <typeparamref name="TIntegral" /> that is returned.
        /// </param>
        /// <returns>
        ///   An instance of type <typeparamref name="TIntegral"/> that falls between
        ///   <see cref="Low" /> (inclusively) and <see cref="High" /> (exclusively),
        ///   but is also less than <paramref name="other" />.
        /// </returns>
        /// <exception cref="Peddler.UnableToGenerateValueException">
        ///   Thrown when this generator is unable to provide a value between
        ///   <see cref="Low" /> (inclusively) and <see cref="High" /> (exclusively).
        ///   This can happen if <see cref="Low" /> is greater than or equal to the
        ///   value provided by <paramref name="other" />.
        /// </exception>
        public TIntegral NextLessThan(TIntegral other) {
            if (other.CompareTo(this.High) >= 0) {
                return this.Next();
            }

            if (other.CompareTo(this.Low) <= 0) {
                throw new UnableToGenerateValueException(
                    $"Since '{nameof(this.Low)}' is {this.Low:N0}, the minimum value that " +
                    $"can be generated is {this.Low:N0}. Since the value provided for " +
                    $"for '{nameof(other)}' was {other:N0}, a value less than it cannot " +
                    $"be provided by this {this.GetType().Name}.",
                    nameof(other)
                );
            }

            return this.Next(this.Low, other);
        }

        /// <summary>
        ///   Creates a new integral value of type <typeparamref name="TIntegral"/>
        ///   that will be between <see cref="Low" /> (inclusively) and
        ///   <see cref="High" /> (exclusively) that will be less than or equal to
        ///   <paramref name="other" />.
        /// </summary>
        /// <remarks>
        ///   The value provided for <paramref name="other" /> does not, itself,
        ///   need to be between <see cref="Low" /> and <see cref="High" />.
        /// </remarks>
        /// <param name="other">
        ///   A value that sets an inclusive, upper boundary for the
        ///   <typeparamref name="TIntegral" /> that is returned.
        /// </param>
        /// <returns>
        ///   An instance of type <typeparamref name="TIntegral"/> that falls between
        ///   <see cref="Low" /> (inclusively) and <see cref="High" /> (exclusively),
        ///   but is also less than or equal to <paramref name="other" />.
        /// </returns>
        /// <exception cref="Peddler.UnableToGenerateValueException">
        ///   Thrown when this generator is unable to provide a value between
        ///   <see cref="Low" /> (inclusively) and <see cref="High" /> (exclusively).
        ///   This can happen if <see cref="Low" /> is greater than the
        ///   value provided by <paramref name="other" />.
        /// </exception>
        public TIntegral NextLessThanOrEqualTo(TIntegral other) {
            if (other.CompareTo(this.SubtractOne(this.High)) >= 0) {
                return this.Next();
            }

            if (other.CompareTo(this.Low) < 0) {
                throw new UnableToGenerateValueException(
                    $"Since '{nameof(this.Low)}' is {this.Low:N0}, the minimum value that " +
                    $"can be generated is {this.Low:N0}. Since the value provided for " +
                    $"for '{nameof(other)}' was {other:N0}, a value less than or equal " +
                    $"to it cannot be provided by this {this.GetType().Name}.",
                    nameof(other)
                );
            }

            return this.Next(this.Low, this.AddOne(other));
        }


    }

}
