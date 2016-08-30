using System;

namespace Peddler {

    public class IntegerGenerator : IDistinctGenerator<int>, IComparableGenerator<int> {

        private Random random { get; } = new Random();
        public int Low { get; }
        public int High { get; }

        public IntegerGenerator() :
            this(0, Int32.MaxValue) {}

        public IntegerGenerator(int low) :
            this(low, Int32.MaxValue) {}

        public IntegerGenerator(int low, int high) {
            if (low >= high) {
                throw new ArgumentException(
                    $"The '{nameof(low)}' argument ({low:N0}) must be lower " +
                    $"than the '{nameof(high)}' argument ({high:N0}).",
                    nameof(low)
                );
            }

            this.Low = low;
            this.High = high;
        }

        public int Next() {
            return this.random.Next(this.Low, this.High);
        }

        public int NextDistinct(int other) {
            if (other < this.Low || other >= this.High) {
                return this.Next();
            }

            if (this.Low == this.High - 1) {
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

            var nextValue = this.random.Next(this.Low, this.High - 1);

            if (nextValue >= other) {
                nextValue++;
            }

            return nextValue;
        }

        public int NextGreaterThan(int other) {
            if (other < this.Low) {
                return this.Next();
            }

            if (other >= this.High - 1) {
                throw new UnableToGenerateValueException(
                    $"Since '{nameof(this.High)}' is {this.High:N0}, the maximum value that " +
                    $"can be generated is {(this.High - 1):N0}. Since the value provided for " +
                    $"'{nameof(other)}' was {other:N0}, a value greater than it cannot be " +
                    $"provided by this {this.GetType().Name}.",
                    nameof(other)
                );
            }

            return this.random.Next(other + 1, this.High);
        }

        public int NextGreaterThanOrEqualTo(int other) {
            if (other < this.Low) {
                return this.Next();
            }

            if (other >= this.High) {
                throw new UnableToGenerateValueException(
                    $"Since '{nameof(this.High)}' is {this.High:N0}, the maximum value that " +
                    $"can be generated is {(this.High - 1):N0}. Since the value provided for " +
                    $"'{nameof(other)}' was {other:N0}, a value greater than or equal to it " +
                    $"cannot be provided by this {this.GetType().Name}.",
                    nameof(other)
                );
            }

            return this.random.Next(other, this.High);
        }

        public int NextLessThan(int other) {
            if (other >= this.High) {
                return this.Next();
            }

            if (other <= this.Low) {
                throw new UnableToGenerateValueException(
                    $"Since '{nameof(this.Low)}' is {this.Low:N0}, the minimum value that " +
                    $"can be generated is {this.Low:N0}. Since the value provided for " +
                    $"for '{nameof(other)}' was {other:N0}, a value less than it cannot " +
                    $"be provided by this {this.GetType().Name}.",
                    nameof(other)
                );
            }

            return this.random.Next(this.Low, other);
        }

        public int NextLessThanOrEqualTo(int other) {
            if (other >= this.High) {
                return this.Next();
            }

            if (other < this.Low) {
                throw new UnableToGenerateValueException(
                    $"Since '{nameof(this.Low)}' is {this.Low:N0}, the minimum value that " +
                    $"can be generated is {this.Low:N0}. Since the value provided for " +
                    $"for '{nameof(other)}' was {other:N0}, a value less than or equal " +
                    $"to it cannot be provided by this {this.GetType().Name}.",
                    nameof(other)
                );
            }

            return this.random.Next(this.Low, other + 1);
        }

    }

}
