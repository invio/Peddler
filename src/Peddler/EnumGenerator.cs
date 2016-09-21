using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Peddler {

    /// <summary>
    ///   A generator for values of the enum type <typeparamref name="TEnum" />.
    /// </summary>
    public class EnumGenerator<TEnum> : IGenerator<TEnum> where TEnum : struct {

        private static Lazy<ISet<TEnum>> defaultValues { get; }

        static EnumGenerator() {
            defaultValues = new Lazy<ISet<TEnum>>(GetEnumValues);
        }

        private static ISet<TEnum> GetEnumValues() {
            if (!typeof(TEnum).GetTypeInfo().IsEnum) {
                throw new NotSupportedException($"'{typeof(TEnum).Name}' is not an enum.");
            }

            var values =
                typeof(TEnum)
                    .GetTypeInfo()
                    .GetEnumValues()
                    .Cast<TEnum>()
                    .ToImmutableHashSet();

            if (values.Count == 0) {
                throw new NotSupportedException($"'{typeof(TEnum).Name}' lacks enum values.");
            }

            return values;
        }

        /// <summary>
        ///   The list of <typeparamref name="TEnum" /> values that this
        ///   <see cref="EnumGenerator{TEnum}" /> will choose from when generating values.
        /// </summary>
        public ISet<TEnum> Values { get; }

        private Random random { get; } = new Random();
        private TEnum[] valuesLookup { get; }

        /// <summary>
        ///   Instantiates a <see cref="EnumGenerator{TEnum}" /> that has an equal liklihood
        ///   of emiting all enum values defined for <typeparamref name="TEnum" />.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///   Thrown when <typeparamref name="TEnum" /> is not an enum,
        ///   or if <typeparamref name="TEnum" /> is an enum but lacks enum values.
        /// </exception>
        public EnumGenerator() {
            this.Values = defaultValues.Value.ToImmutableHashSet();
            this.valuesLookup = this.Values.ToArray();
        }

        /// <summary>
        ///   Instantiates a <see cref="EnumGenerator{TEnum}" /> that has an equal liklihood
        ///   of emiting all enum values provided in the <paramref name="values" /> set.
        /// </summary>
        /// <remarks>
        ///   Each <typeparamref name="TEnum" /> value provided via the
        ///   <paramref name="values" /> parameter has an equal liklihood of being
        ///   returned by this <see cref="EnumGenerator{TEnum}" />.
        /// </remarks>
        /// <param name="values">
        ///   A set of <typeparamref name="TEnum" /> values that this
        ///   <see cref="EnumGenerator{TEnum}" /> will exclusively choose from when
        ///   providing <typeparamref name="TEnum" /> values.
        /// </param>
        /// <exception cref="NotSupportedException">
        ///   Thrown when <typeparamref name="TEnum" /> is not an enum.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///   Thrown when <paramref name="values" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   Thrown when <paramref name="values" /> is empty.
        /// </exception>
        public EnumGenerator(ISet<TEnum> values) {
            if (!typeof(TEnum).GetTypeInfo().IsEnum) {
                throw new NotSupportedException($"'{typeof(TEnum).Name}' is not an enum.");
            }

            if (values == null) {
                throw new ArgumentNullException("values");
            }

            if (values.Count == 0) {
                throw new ArgumentException(
                    $"'nameof(values)' contains no enum values.",
                    nameof(values)
                );
            }

            this.Values = values.ToImmutableHashSet();
            this.valuesLookup = this.Values.ToArray();
        }

        /// <inheritdoc />
        public virtual TEnum Next() {
            return this.valuesLookup[this.random.Next(this.Values.Count)];
        }

    }

}
