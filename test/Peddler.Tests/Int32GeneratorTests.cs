using System;

namespace Peddler {

    public class Int32GeneratorTests : IntegralGeneratorTests<Int32> {

        protected override IntegralGenerator<Int32> CreateGenerator() {
            return new Int32Generator();
        }

        protected override IntegralGenerator<Int32> CreateGenerator(Int32 low) {
            return new Int32Generator(low);
        }

        protected override IntegralGenerator<Int32> CreateGenerator(Int32 low, Int32 high) {
            return new Int32Generator(low, high);
        }

    }

}
