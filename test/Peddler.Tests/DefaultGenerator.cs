using System;
using System.Collections.Generic;

namespace Peddler {

    public class DefaultGenerator<T> : IComparableGenerator<T> {

        public T DefaultValue { get; }

        public DefaultGenerator() :
            this(default(T)) {}

        public DefaultGenerator(T defaultValue) {
            this.DefaultValue = defaultValue;
        }

        public IEqualityComparer<T> EqualityComparer {
            get { return EqualityComparer<T>.Default; }
        }

        public IComparer<T> Comparer {
            get { return Comparer<T>.Default; }
        }

        public T Next() {
            return this.DefaultValue;
        }

        public T NextDistinct(T other) {
            return this.NextImpl(other, comparison => comparison != 0);
        }

        public T NextGreaterThan(T other) {
            return this.NextImpl(other, comparison => comparison > 0);
        }

        public T NextGreaterThanOrEqualTo(T other) {
            return this.NextImpl(other, comparison => comparison >= 0);
        }

        public T NextLessThan(T other) {
            return this.NextImpl(other, comparison => comparison < 0);
        }

        public T NextLessThanOrEqualTo(T other) {
            return this.NextImpl(other, comparison => comparison <= 0);
        }

        private T NextImpl(T other, Func<int, bool> isDefaultOk) {
            if (isDefaultOk(this.Comparer.Compare(this.DefaultValue, other))) {
                return this.DefaultValue;
            }

            throw new UnableToGenerateValueException();
        }

    }

}
