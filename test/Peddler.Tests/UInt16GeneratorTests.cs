using System;

namespace Peddler {

    public class UInt16GeneratorTests : IntegralGeneratorTests<UInt16> {

        protected override IIntegralGenerator<UInt16> CreateGenerator() {
            return new UInt16Generator();
        }

        protected override IIntegralGenerator<UInt16> CreateGenerator(UInt16 low) {
            return new UInt16Generator(low);
        }

        protected override IIntegralGenerator<UInt16> CreateGenerator(UInt16 low, UInt16 high) {
            return new UInt16Generator(low, high);
        }

    }

}
