using System;
using System.Collections.Generic;

namespace Peddler {

    /// <summary>
    ///  A generator for creating <see cref="Boolean" /> values.
    /// </summary>
    public class BooleanGenerator : IDistinctGenerator<Boolean> {

        private Random random { get; }

        /// <inheritdoc />
        public IEqualityComparer<Boolean> EqualityComparer { get; }

        /// <summary>
        ///   Instantiates a <see cref="BooleanGenerator" /> that will
        ///   create <see cref="Boolean" /> values with equal liklihood
        ///   of <c>true</c> and <c>false</c>.
        /// </summary>
        public BooleanGenerator() {
            this.random = new Random();
            this.EqualityComparer = EqualityComparer<Boolean>.Default;
        }

        /// <inheritdoc />
        public Boolean Next() {
            return this.random.NextBoolean();
        }

        /// <inheritdoc />
        public Boolean NextDistinct(Boolean other) {
            return !other;
        }

    }

}
