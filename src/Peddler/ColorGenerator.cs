using System;
using System.Collections.Generic;
using System.Drawing;

namespace Peddler {

    /// <summary>
    ///   A generator for <see cref="Color" /> values, with configurable
    ///   consideration for whether or not the the "alpha" channel is included
    ///   as part of the <see cref="Color" />.
    /// </summary>
    public sealed class ColorGenerator : IDistinctGenerator<Color> {

        /// <inheritdoc />
        public IEqualityComparer<Color> EqualityComparer { get; }

        private bool useAlpha { get; }
        private int mask { get; }
        private Int64Generator colorGenerator { get; }

        /// <summary>
        ///   Instantiates a <see cref="ColorGenerator" /> that has an equal liklihood of
        ///   of emiting any color that can be represented using the <see cref="Color" />
        ///   class provided with .NET Standard.
        /// </summary>
        /// <param name="useAlpha">
        ///   Controls whether or not the alpha channel is considered. If set to
        ///   <c>true</c> the alpha channel for each generate color will vary in value
        ///   from 0 to 255. If <c>false</c>, the alpha channel will be full opacity
        ///   (255) for all colors generated by the <see cref="ColorGenerator" />.
        /// </param>
        public ColorGenerator(bool useAlpha = false) {
            this.useAlpha = useAlpha;

            if (useAlpha) {
                this.mask = unchecked((int)0xFFFFFFFF);
            } else {
                this.mask = unchecked((int)0x00FFFFFF);
            }

            this.EqualityComparer = new ColorEqualityComparer(this.mask);

            // This needs to be using Int64 in order to generate values that
            // are 0 <-> 0xFFFFFFFF (Full Opacity White). The Int32Generator
            // can only generate values between 0 <-> (0xFFFFFFFE) because
            // the upper bounder is exclusive.

            this.colorGenerator = new Int64Generator(0L, ((long)(uint)mask) + 1L);
        }

        /// <summary>
        ///   Generates a random <see cref="Color" /> with arbitrary red, green, and blue
        ///   values that range from 0 to 255.
        /// </summary>
        /// <remarks>
        ///   If the "useAlpha" constructor parameter was set to <c>true</c>, then the alpha
        ///   channel will also range from 0 to 255. However, if it was set to <c>false</c>,
        ///   then the alpha channel will always be set to 255 for full opacity.
        /// </remarks>
        public Color Next() {
            var value = unchecked((int)this.colorGenerator.Next());

            if (!this.useAlpha) {
                value |= unchecked((int)0xFF000000); // Make full opacity
            }

            return Color.FromArgb(value);
        }

        /// <summary>
        ///   Generates a distinct <see cref="Color" /> with regard to red, green, blue
        ///   and, potentially, alpha channels.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     If this <see cref="ColorGenerator" /> was created WITHOUT regard for the
        ///     alpha channel (the "useAlpha" constructor parameter was set to <c>false</c>),
        ///     then the alpha of the generated color will always be 255, and one or more of
        ///     the provided red, green, or blue values will be distinct from the red, green,
        ///     or blue values provided via the <paramref name="color" /> parameter.
        ///   </para>
        ///   <para>
        ///     if this <see cref="ColorGenerator" /> was created WITH regard for the
        ///     alpha channel (the "useAlpha" constructor parameter set to <c>true</c>), then
        ///     one or more of the red, green, blue, and alpha channels will be distinct from
        ///     those that are defined in the <paramref name="color" /> parameter.
        ///   </para>
        /// </remarks>
        public Color NextDistinct(Color color) {
            var input = (color.ToArgb() & this.mask);
            var value = unchecked((int)this.colorGenerator.NextDistinct(input));

            if (!this.useAlpha) {
                value |= unchecked((int)0xFF000000); // Make full opacity
            }

            return Color.FromArgb(value);
        }

        private class ColorEqualityComparer : EqualityComparer<Color> {

            private int mask { get; }

            public ColorEqualityComparer(int mask) {
                this.mask = mask;
            }

            public override int GetHashCode(Color color) {
                return color.ToArgb() & this.mask;
            }

            public override bool Equals(Color x, Color y) {
                return (x.ToArgb() & this.mask) == (y.ToArgb() & this.mask);
            }

        }

    }

}