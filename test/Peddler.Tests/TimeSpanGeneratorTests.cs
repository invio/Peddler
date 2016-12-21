using System;
using System.Collections.Generic;
using Xunit;

namespace Peddler {

    public class TimeSpanGeneratorTests : IntegralGeneratorTests<TimeSpan> {

        protected override IIntegralGenerator<TimeSpan> CreateGenerator() {
            return new TimeSpanGenerator();
        }

        protected override IIntegralGenerator<TimeSpan> CreateGenerator(TimeSpan low) {
            return new TimeSpanGenerator(low);
        }

        protected override IIntegralGenerator<TimeSpan> CreateGenerator(
            TimeSpan low,
            TimeSpan high) {

            return new TimeSpanGenerator(low, high);
        }

        protected override String FormatValue(TimeSpan value) {
            return value.ToString();
        }

    }

}
