using System;

namespace Peddler {

    public abstract class IntegralGenerator<TIntegral> :
        IGenerator<TIntegral>, IDistinctGenerator<TIntegral>, IComparableGenerator<TIntegral>
        where TIntegral : struct, IEquatable<TIntegral>, IComparable<TIntegral> {

        public TIntegral Low { get; }
        public TIntegral High { get; }

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

        protected abstract TIntegral Next(TIntegral low, TIntegral high);
        protected abstract TIntegral AddOne(TIntegral value);
        protected abstract TIntegral SubtractOne(TIntegral value);

        public TIntegral Next() {
            return this.Next(this.Low, this.High);
        }

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

        public TIntegral NextGreaterThan(TIntegral other) {
            if (other.CompareTo(this.Low) < 0) {
                return this.Next();
            }

            if (other.CompareTo(this.SubtractOne(this.High)) >= 0) {
                throw new UnableToGenerateValueException(
                    $"Since '{nameof(this.High)}' is {this.High:N0}, the maximum value that " +
                    $"can be generated is {this.SubtractOne(this.High):N0}. Since the value provided for " +
                    $"'{nameof(other)}' was {other:N0}, a value greater than it cannot be " +
                    $"provided by this {this.GetType().Name}.",
                    nameof(other)
                );
            }

            return this.Next(this.AddOne(other), this.High);
        }

        public TIntegral NextGreaterThanOrEqualTo(TIntegral other) {
            if (other.CompareTo(this.Low) <= 0) {
                return this.Next();
            }

            if (other.CompareTo(this.High) >= 0) {
                throw new UnableToGenerateValueException(
                    $"Since '{nameof(this.High)}' is {this.High:N0}, the maximum value that " +
                    $"can be generated is {this.SubtractOne(this.High):N0}. Since the value provided for " +
                    $"'{nameof(other)}' was {other:N0}, a value greater than or equal to it " +
                    $"cannot be provided by this {this.GetType().Name}.",
                    nameof(other)
                );
            }

            return this.Next(other, this.High);
        }

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
