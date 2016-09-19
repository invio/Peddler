using System;

namespace Peddler {

    /// <summary>
    ///   A catch-all interface for generators of data types that can be
    ///   represented as an integer, such as <see cref="Int32" />, <see cref="Byte" />,
    ///   <see cref="DateTime" /> (via ticks), or <see cref="System.Char" />.
    /// </summary>
    public interface IIntegralGenerator<TIntegral> : IComparableGenerator<TIntegral> {

        /// <summary>
        ///   The inclusive, lower <typeparamref name="TIntegral" /> boundary for this generator.
        /// </summary>
        TIntegral Low { get; }

        /// <summary>
        ///   The exclusive, upper <typeparamref name="TIntegral" /> boundary for this generator.
        /// </summary>
        TIntegral High { get; }

    }

}
