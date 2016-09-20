using System;

namespace Peddler {

    public class FakeClassGenerator : FakeGeneratorBase<FakeClass> {

        public FakeClassGenerator() :
            base() {}

        public FakeClassGenerator(IComparableGenerator<int> generator) :
            base(generator) {}

        protected override FakeClass CreateFake(int value) {
            return new FakeClass(value);
        }

        protected override int GetValue(FakeClass fake) {
            if (fake == null) {
                throw new ArgumentNullException(nameof(fake));
            }

            return fake.Value;
        }

    }

}
