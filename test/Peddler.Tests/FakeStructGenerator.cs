using System;

namespace Peddler {

    public class FakeStructGenerator : FakeGeneratorBase<FakeStruct> {

        public FakeStructGenerator() :
            base() {}

        public FakeStructGenerator(IComparableGenerator<int> generator) :
            base(generator) {}

        protected override FakeStruct CreateFake(int value) {
            return new FakeStruct { Value = value };
        }

        protected override int GetValue(FakeStruct fake) {
            return fake.Value;
        }

    }

}
