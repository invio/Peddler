using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Peddler {

    internal static class DateTimeUtilities {

        private static IDictionary<DateTimeUnit, long> ticksPerUnitCache { get; }

        static DateTimeUtilities() {
            ticksPerUnitCache =
                ImmutableDictionary<DateTimeUnit, long>
                    .Empty
                    .Add(DateTimeUnit.Tick, 1L)
                    .Add(DateTimeUnit.Millisecond, TimeSpan.TicksPerMillisecond)
                    .Add(DateTimeUnit.Second, TimeSpan.TicksPerSecond)
                    .Add(DateTimeUnit.Minute, TimeSpan.TicksPerMinute)
                    .Add(DateTimeUnit.Hour, TimeSpan.TicksPerHour)
                    .Add(DateTimeUnit.Day, TimeSpan.TicksPerDay);
        }

        public static long GetTicksPerUnit(DateTimeUnit unit) {
            return ticksPerUnitCache[unit];
        }

    }

}