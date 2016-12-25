using System;
using System.Collections.Generic;
using System.Threading;

namespace Peddler {

    /// <summary>
    ///  A generator for creating <see cref="Boolean" /> values.
    /// </summary>
    public class BooleanGenerator : IDistinctGenerator<Boolean> {

        private static ThreadLocal<Random> random { get; } =
            new ThreadLocal<Random>(() => new Random());

        /// <inheritdoc />
        public IEqualityComparer<Boolean> EqualityComparer { get; }

        /// <summary>
        ///   Instantiates a <see cref="BooleanGenerator" /> that will
        ///   create <see cref="Boolean" /> values with equal liklihood
        ///   of <c>true</c> and <c>false</c>.
        /// </summary>
        public BooleanGenerator() {
            this.EqualityComparer = EqualityComparer<Boolean>.Default;
        }

        /// <inheritdoc />
        public Boolean Next() {
            return random.Value.NextBoolean();
        }

        /// <inheritdoc />
        public Boolean NextDistinct(Boolean other) {
            return !other;
        }

    }

}
