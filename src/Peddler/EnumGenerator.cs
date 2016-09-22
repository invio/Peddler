using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Peddler {

    /// <summary>
    ///   A generator for values of the enum type <typeparamref name="TEnum" />.
    /// </summary>
    public class EnumGenerator<TEnum> : IDistinctGenerator<TEnum> where TEnum : struct {

        private static Lazy<ISet<TEnum>> defaultValues { get; }
        private static IEqualityComparer<TEnum> defaultEqualityComparer { get; }

        static EnumGenerator() {
            defaultValues = new Lazy<ISet<TEnum>>(GetEnumValues);
            defaultEqualityComparer = EqualityComparer<TEnum>.Default;
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

        /// <inheritdoc />
        public IEqualityComparer<TEnum> EqualityComparer { get; } = defaultEqualityComparer;

        private Random random { get; } = new Random();
        private TEnum[] valuesLookup { get; }
        private IDictionary<TEnum, int> valuesReverseLookup { get; }

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
            this.valuesReverseLookup =
                this.valuesLookup
                    .Select((TEnum value, int index) => Tuple.Create(value, index))
                    .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
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
                    $"'{nameof(values)}' contains no enum values.",
                    nameof(values)
                );
            }

            this.Values = values.ToImmutableHashSet();

            this.valuesLookup = this.Values.ToArray();
            this.valuesReverseLookup =
                this.valuesLookup
                    .Select((TEnum value, int index) => Tuple.Create(value, index))
                    .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
        }

        /// <inheritdoc />
        public virtual TEnum Next() {
            return this.valuesLookup[this.random.Next(this.valuesLookup.Length)];
        }

        /// <inheritdoc />
        public virtual TEnum NextDistinct(TEnum other) {
            int index;

            if (!this.valuesReverseLookup.TryGetValue(other, out index)) {
                return this.Next();
            }

            if (this.valuesLookup.Length == 1) {
                throw new UnableToGenerateValueException(
                    $"This EnumGenerator<{typeof(TEnum).Name}> can only generate " +
                    $"{typeof(TEnum).Name} values of '{this.Values.Single():G}'. " +
                    $"Since the value provided for '{nameof(other)}' was " +
                    $"'{this.Values.Single():G}', a distinct value cannot be generated.",
                    nameof(other)
                );
            }

            var nextIndex = this.random.Next(this.valuesLookup.Length - 1);

            if (nextIndex >= index) {
                nextIndex++;
            }

            return this.valuesLookup[nextIndex];
        }

    }

}
