using System;

namespace Peddler {

    public class UInt32GeneratorTests : IntegralGeneratorTests<UInt32> {

        protected override IIntegralGenerator<UInt32> CreateGenerator() {
            return new UInt32Generator();
        }

        protected override IIntegralGenerator<UInt32> CreateGenerator(UInt32 low) {
            return new UInt32Generator(low);
        }

        protected override IIntegralGenerator<UInt32> CreateGenerator(UInt32 low, UInt32 high) {
            return new UInt32Generator(low, high);
        }

    }

}
