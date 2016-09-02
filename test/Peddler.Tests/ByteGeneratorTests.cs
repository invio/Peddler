using System;

namespace Peddler {

    public class ByteGeneratorTests : IntegralGeneratorTests<Byte> {

        protected override IIntegralGenerator<Byte> CreateGenerator() {
            return new ByteGenerator();
        }

        protected override IIntegralGenerator<Byte> CreateGenerator(Byte low) {
            return new ByteGenerator(low);
        }

        protected override IIntegralGenerator<Byte> CreateGenerator(Byte low, Byte high) {
            return new ByteGenerator(low, high);
        }

    }

}
