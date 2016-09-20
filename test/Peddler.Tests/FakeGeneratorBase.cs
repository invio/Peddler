using System;
using System.Collections.Generic;

namespace Peddler {

    public abstract class FakeGeneratorBase<TFake> : IComparableGenerator<TFake> {

        public IEqualityComparer<TFake> EqualityComparer {
            get { return EqualityComparer<TFake>.Default; }
        }

        public IComparer<TFake> Comparer {
            get { return Comparer<TFake>.Default; }
        }

        private IComparableGenerator<int> generator { get; }

        protected FakeGeneratorBase() :
            this(new Int32Generator(-10, 10)) {}

        protected FakeGeneratorBase(IComparableGenerator<int> generator) {
            if (generator == null) {
                throw new ArgumentNullException(nameof(generator));
            }

            this.generator = generator;
        }

        protected abstract TFake CreateFake(int value);
        protected abstract int GetValue(TFake fake);

        public TFake Next() {
            return this.CreateFake(generator.Next());
        }

        public TFake NextDistinct(TFake other) {
            return NextImpl(
                other,
                comparison => comparison != 0,
                generator.NextDistinct
            );
        }

        public TFake NextLessThan(TFake other) {
            return NextImpl(
                other,
                comparison => comparison < 0,
                generator.NextLessThan
            );
        }

        public TFake NextLessThanOrEqualTo(TFake other) {
            return NextImpl(
                other,
                comparison => comparison <= 0,
                generator.NextLessThanOrEqualTo
            );
        }

        public TFake NextGreaterThan(TFake other) {
            return NextImpl(
                other,
                comparison => comparison > 0,
                generator.NextGreaterThan
            );
        }

        public TFake NextGreaterThanOrEqualTo(TFake other) {
            return NextImpl(
                other,
                comparison => comparison >= 0,
                generator.NextGreaterThanOrEqualTo
            );
        }

        private TFake NextImpl(
            TFake other,
            Func<int, bool> isNonNullValueOk,
            Func<int, int> nextImpl) {

            if (other == null) {
                var fake = this.Next();
                var comparison = this.Comparer.Compare(fake, other);

                if (!isNonNullValueOk(comparison)) {
                    throw new UnableToGenerateValueException(
                        $"Cannot generate {typeof(TFake).Name} when {nameof(other)} is null.",
                        nameof(other)
                    );
                }

                return fake;
            }

            return this.CreateFake(nextImpl(this.GetValue(other)));
        }

    }
}
