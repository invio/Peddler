using System;

namespace Peddler {

    public class Int16GeneratorTests : IntegralGeneratorTests<Int16> {

        protected override IIntegralGenerator<Int16> CreateGenerator() {
            return new Int16Generator();
        }

        protected override IIntegralGenerator<Int16> CreateGenerator(Int16 low) {
            return new Int16Generator(low);
        }

        protected override IIntegralGenerator<Int16> CreateGenerator(Int16 low, Int16 high) {
            return new Int16Generator(low, high);
        }

    }

}
