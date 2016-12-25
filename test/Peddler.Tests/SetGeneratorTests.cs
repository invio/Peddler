using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Peddler {

    public class SetGeneratorTests {

        private const int numberOfAttempts = 100;

        [Fact]
        public void Constructor_WithoutComparer_NullValues() {
            Assert.Throws<ArgumentNullException>(
                () => new SetGenerator<int>(null)
            );
        }

        [Fact]
        public void Constructor_WithComparer_NullValues() {
            var comparer = EqualityComparer<int>.Default;

            Assert.Throws<ArgumentNullException>(
                () => new SetGenerator<int>(null, comparer)
            );
        }

        [Fact]
        public void Constructor_WithComparer_NullComparer() {
            var values = new HashSet<int> { 1, 2, 3 };

            Assert.Throws<ArgumentNullException>(
                () => new SetGenerator<int>(values, null)
            );
        }

        [Fact]
        public void Constructor_WithoutComparer_EmptyValues() {
            var values = new HashSet<int>();

            var exception = Assert.Throws<ArgumentException>(
                () => new SetGenerator<int>(values)
            );

            Assert.Equal(
                "The 'values' argument must be non-empty." +
                Environment.NewLine + "Parameter name: values",
                exception.Message
            );
        }

        [Fact]
        public void Constructor_WithComparer_EmptyValues() {
            var values = new HashSet<int>();
            var comparer = EqualityComparer<int>.Default;

            var exception = Assert.Throws<ArgumentException>(
                () => new SetGenerator<int>(values, comparer)
            );

            Assert.Equal(
                "The 'values' argument must be non-empty." +
                Environment.NewLine + "Parameter name: values",
                exception.Message
            );
        }

        [Fact]
        public void EqualityComparerProperty_WithoutComparer() {
            var values = new HashSet<int> { 1, 2, 3 };
            var generator = new SetGenerator<int>(values);

            Assert.Equal(EqualityComparer<int>.Default, generator.EqualityComparer);
        }

        [Fact]
        public void EqualityComparerProperty_WithComparer() {
            var values = new HashSet<String> { "foo", "bar" };
            var comparer = StringComparer.OrdinalIgnoreCase;
            var generator = new SetGenerator<String>(values, comparer);

            Assert.Equal(comparer, generator.EqualityComparer);
        }

        [Fact]
        public void Next_WithoutComparer() {
            var values = new HashSet<int> { 1, 2, 3, 4, 5 };
            var generator = new SetGenerator<int>(values);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.Contains(generator.Next(), values, generator.EqualityComparer);
            }
        }

        [Fact]
        public void Next_WithComparer() {
            var values = new HashSet<String> { "a", "b", "foo", "bar", "biz" };
            var comparer = StringComparer.OrdinalIgnoreCase;
            var generator = new SetGenerator<String>(values, comparer);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.Contains(generator.Next(), values, generator.EqualityComparer);
            }
        }

        [Fact]
        public void Next_WithoutComparer_DuplicateValues() {
            var values = new HashSet<int> { 1, 1, 1, 1 };
            var generator = new SetGenerator<int>(values);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.Contains(generator.Next(), values, generator.EqualityComparer);
            }
        }

        [Fact]
        public void Next_WithComparer_DuplicateValues() {
            var values = new HashSet<String> { "foo", "FOO", "fOO", "Foo", "FoO", "fOo" };
            var comparer = StringComparer.OrdinalIgnoreCase;
            var generator = new SetGenerator<String>(values);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.Contains(generator.Next(), values, generator.EqualityComparer);
            }
        }

        [Fact]
        public void NextDistinct_WithoutComparer_SingleValue() {
            var values = new HashSet<int> { 1 };
            var generator = new SetGenerator<int>(values);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextDistinct(1)
            );
        }

        [Fact]
        public void NextDistinct_WithComparer_SingleValue() {
            var values = new HashSet<String> { "FOO" };
            var comparer = StringComparer.OrdinalIgnoreCase;
            var generator = new SetGenerator<String>(values, comparer);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextDistinct("foo")
            );
        }

        [Fact]
        public void NextDistinct_WithoutComparer_SingleDuplicateValue() {
            var values = new HashSet<int> { 1, 1 };
            var generator = new SetGenerator<int>(values);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextDistinct(1)
            );
        }

        [Fact]
        public void NextDistinct_WithComparer_SingleDuplicateValue() {
            var values = new HashSet<String> { "fOO", "Foo" };
            var comparer = StringComparer.OrdinalIgnoreCase;
            var generator = new SetGenerator<String>(values, comparer);

            Assert.Throws<UnableToGenerateValueException>(
                () => generator.NextDistinct("FOO")
            );
        }

        [Fact]
        public void NextDistinct_WithoutComparer_ValueOutsideSet() {
            const int outlier = 4;

            var values = new HashSet<int> { 1, 2, 3 };
            var generator = new SetGenerator<int>(values);

            Assert.DoesNotContain(outlier, values, generator.EqualityComparer);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.Contains(
                    generator.NextDistinct(outlier),
                    values,
                    generator.EqualityComparer
                );
            }
        }

        [Fact]
        public void NextDistinct_WithComparer_ValueOutsideSet() {
            const string outlier = "choo";

            var values = new HashSet<String> { "foo", "bar", "biz", "FOO", "Biz" };
            var comparer = StringComparer.OrdinalIgnoreCase;
            var generator = new SetGenerator<String>(values, comparer);

            Assert.DoesNotContain(outlier, values, generator.EqualityComparer);

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                Assert.Contains(
                    generator.NextDistinct(outlier),
                    values,
                    generator.EqualityComparer
                );
            }
        }

        [Fact]
        public void NextDistinct_WithoutComparer() {
            var values = new HashSet<int> { 1, 2, 3, 4, 5 };
            var generator = new SetGenerator<int>(values);

            var previousValue = generator.Next();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(previousValue);

                Assert.NotEqual(previousValue, value, generator.EqualityComparer);
                Assert.Contains(value, values, generator.EqualityComparer);

                previousValue = value;
            }
        }

        [Fact]
        public void NextDistinct_WithComparer() {
            var values = new HashSet<String> { "foo", "bar", "biz" };
            var comparer = StringComparer.OrdinalIgnoreCase;
            var generator = new SetGenerator<String>(values, comparer);

            var previousValue = generator.Next();

            for (var attempt = 0; attempt < numberOfAttempts; attempt++) {
                var value = generator.NextDistinct(previousValue);

                Assert.NotEqual(previousValue, value, generator.EqualityComparer);
                Assert.Contains(value, values, generator.EqualityComparer);

                previousValue = value;
            }
        }

        [Fact]
        public async Task ThreadSafety() {

            // Arrange

            var values = new HashSet<Int32> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var firstValue = values.ToArray()[0];
            var generator = new SetGenerator<Int32>(values);
            var consecutiveFirstValues = new ConcurrentStack<Int32>();

            Action createThread =
                () => this.ThreadSafetyImpl(firstValue, generator, consecutiveFirstValues);

            // Act

            var threads =
                Enumerable
                    .Range(0, 10)
                    .Select(_ => Task.Run(createThread))
                    .ToArray();

            await Task.WhenAll(threads);

            // Assert

            Assert.True(
                consecutiveFirstValues.Count < 50,
                $"System.Random is not thread safe. If one of its .Next() " +
                $"implementations is called simultaneously on several " +
                $"threads, it breaks and starts returning zero exclusively. " +
                $"The last {consecutiveFirstValues.Count:N0} sets " +
                $"returned the first value from the set, signifying its " +
                $"internal System.Random is in a broken state."
            );
        }

        private void ThreadSafetyImpl<T>(
            T firstValue,
            IGenerator<T> generator,
            ConcurrentStack<T> consecutiveFirstValues) {

            var count = 0;

            while (count++ < 10000) {
                if (generator.Next().Equals(firstValue)) {
                    consecutiveFirstValues.Push(firstValue);
                } else {
                    consecutiveFirstValues.Clear();
                }
            }
        }

    }

}
