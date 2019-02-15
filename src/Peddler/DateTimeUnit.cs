using System;

namespace Peddler {

    /// <summary>
    ///   The various types of units that make up a <see cref="DateTime" />.
    /// </summary>
    /// <remarks>
    ///   These can be used to control the level of granularity a
    ///   <see cref="DateTimeGenerator" /> uses when generating and differentiating
    ///   various <see cref="DateTime" /> values.
    /// </remarks>
    public enum DateTimeUnit {

        /// <summary>
        ///   Represents a single "tick", the smallest unit of time that is able to be
        ///   represented in a <see cref="DateTime" /> value.
        /// </summary>
        Tick,

        /// <summary>
        ///   Represents a millisecond, or 10000 ticks.
        ///   This is one of the units of measurement used for a <see cref="DateTime" />.
        /// </summary>
        Millisecond,

        /// <summary>
        ///   Represents a second, or 10000000 ticks.
        ///   This is one of the units of measurement used for a <see cref="DateTime" />.
        /// </summary>
        Second,

        /// <summary>
        ///   Represents a minute, or 600000000 ticks.
        ///   This is one of the units of measurement used for a <see cref="DateTime" />.
        /// </summary>
        Minute,

        /// <summary>
        ///   Represents a hour, or 36000000000 ticks.
        ///   This is one of the units of measurement used for a <see cref="DateTime" />.
        /// </summary>
        Hour,

        /// <summary>
        ///   Represents a day, or 864000000000 ticks.
        ///   This is one of the units of measurement used for a <see cref="DateTime" />.
        /// </summary>
        Day

    }

}