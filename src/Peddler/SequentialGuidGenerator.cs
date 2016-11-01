using System;
using System.Security.Cryptography;

namespace Peddler {

    /// <summary>
    ///   A COMB (standing for "combined guid with timestamp") algorithm used to create
    ///   "sequential" <see cref="Guid" /> instances.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///      Inspired by the examples Jeremy Todd's SequentialGuid GitHub project.
    ///      Check it out <a href="https://github.com/jhtodd/SequentialGuid">here</a>.
    ///   </para>
    /// </remarks>
    public class SequentialGuidGenerator : GuidGenerator {

        private static RandomNumberGenerator random { get; }

        static SequentialGuidGenerator() {
            random = RandomNumberGenerator.Create();
        }

        /// <summary>
        ///   Generates a new, non-empty <see cref="Guid" /> instance.
        /// </summary>
        /// <remarks>
        ///   This utilizes then a custom implementation for <see cref="Guid" />
        ///   that takes in to account the current timestamp in order to generate
        ///   values that will be considered sequentially close together.
        /// </remarks>
        public override Guid Next() {
            byte[] guidBytes = new byte[16];

            // (1) Put in 10 random bytes starting at index 6 into the array.

            var randomBytes = new byte[10];
            random.GetBytes(randomBytes);
            Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

            // (2) Put in 6 timestamp-baased bytes starting at index 0 into the array.
            // If the system is little-endian, flip it so the most significant
            // byte of the timestamp value is first.

            var timestampBytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks / 10000L);
            if (BitConverter.IsLittleEndian) {
                Array.Reverse(timestampBytes);
            }
            Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);

            // We have to compensate for the fact that .NET regards the Data1 and Data2
            // block as an Int32 and an Int16, respectively.
            // That means that it switches the order on little-endian systems.
            // So again, we have to reverse.

            if (BitConverter.IsLittleEndian) {
                Array.Reverse(guidBytes, 0, 4);
                Array.Reverse(guidBytes, 4, 2);
            }

            return new Guid(guidBytes);
        }

    }

}
