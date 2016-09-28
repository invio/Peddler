# Peddler

[![Appveyor](https://ci.appveyor.com/api/projects/status/l8vetfa12dyu0q3b/branch/master?svg=true)](https://ci.appveyor.com/project/invio/peddler/branch/master)
[![Travis CI](https://img.shields.io/travis/invio/Peddler.svg?maxAge=3600&label=travis)](https://travis-ci.org/invio/Peddler)
[![NuGet](https://img.shields.io/nuget/v/Peddler.svg)](https://www.nuget.org/packages/Peddler/)
[![Coverage](https://coveralls.io/repos/github/invio/Peddler/badge.svg?branch=master)](https://coveralls.io/github/invio/Peddler?branch=master)

A collection of interfaces and base implementations that can be used to create data objects. These can be useful for creating objects for generative and performance testing, as well as empowering the test writer to focus the content of the test on what is actually being tested by randomly generating valid, irrelevant data on the object.

# Installation
The latest version of this package is available on NuGet. To install, run the following command:

```
PM> Install-Package Peddler
```

# Basic Usage

Peddler provides two things: Interfaces for how to design and compose your own data generation, as well as a collection of out-of-the-box generator implementations over the basic .NET types, such as DateTime, String, Guid, and Int32.

## Interfaces
There are three main interfaces. The out-of-the-box generators implement at least one (but generally all three) of them, depending on their data type.

### [`IGenerator<out T>`](src/Peddler/IGenerator.cs)
This generates random values of `T` when the caller invokes its `Next()` method. Example:
```cs
using Peddler;

public static void Main(string[] args) {
    GuidGenerator generator = new GuidGenerator();
    Guid randomGuid1 = generator.Next(); // A random GUID
    Guid randomGuid2 = generator.Next(); // Another random GUID

    GuidGenerator generator = new Int32Generator(1, 10);
    Int32 randomInt1 = generator.Next(); // A random Int32 between 1 (inclusively) and 10 (exclusively)
    Int32 randomInt2 = generator.Next(); // Possibly equal to randomInt1, possibly distinct.
}
```

### [`IDistinctGenerator<T>`](src/Peddler/IDistinctGenerator.cs)
This generates random values of `T` when the caller invokes its `NextDistinct(T)` method. The resultant value of `T` is guaranteed to be distinct from (or "not equal to") the provided value of `T`. This interface inherits from `IGenerator<T>`. Example:
```cs
using Peddler;

public static void Main(string[] args) {
    DateTimeGenerator generator = new DateTimeGenerator();
    DateTime dumped = new DateTime(2015, 1, 5, 20, 45, 13, 573, DateTimeKind.Utc);
    DateTime letMeForget = generator.NextDistinct(dumped);

    Int32Generator generator = new Int32Generator(1, 10);
    Int32 randomInt1 = generator.Next();
    Int32 randomInt2 = generator.NextDistinct(randomInt1); // Guaranteed to be distinct from randomInt1
    Int32 randomInt3 = generator.NextDistinct(15);         // 15 is out of the range, so this is functionally identical to generator.Next();
}
```

### [`IComparableGenerator<T>`](src/Peddler/IComparableGenerator.cs)
This generates random values of `T` when the caller invokes one of its `NextLessThan(T)`, `NextLessThanOrEqualTo(T)`, `NextGreaterThan(T)`, or `NextGreaterThanOrEqualTo(T)` methods. Depending upon which method is used, the resultant value of `T` is guaranteed to be greater than, less than, or equal to the provided value of `T`. This interface inherits from `IDistinctGenerator<T>`. Example:
```cs
using Peddler;

public static void Main(string[] args) {
    Int32Generator generator = new Int32Generator(1, 10);
    Int32 betweenFiveAndNine = generator.GreaterThan(5);   // 10 is an exclusive boundary
    Int32 mustBeOne = generator.LessThan(2);               // 1 is an inclusive boundary

    Int32Generator generator = new Int32Generator(1, 10);
    Int32 randomInt1 = generator.Next();
    Int32 randomInt2 = generator.NextDistinct(randomInt1); // Guaranteed to be distinct from randomInt1
    Int32 randomInt3 = generator.NextDistinct(15);         // 15 is out of the range, so functionally identical to generator.Next();
}
```

## Base Implementations
Peddler includes implementations for many of the basic data types of the .NET Framework. While you can always create (or submit a pull request with) your own variant of any of these data types, the flexibility provided by the various constructors on each of these implementations will fill most use cases for raw, randomized data generation.

| Type | `IGenerator<T>` | `IDistinctGenerator<T>` | `IComparableGenerator<T>` | Notes |
| ---- |:---------------:|:-----------------------:|:-------------------------:| ----- |
| [SByte](src/Peddler/SByteGenerator.cs) | X | X | X |   |
| [Byte](src/Peddler/ByteGenerator.cs) | X | X | X |   |
| [Int16](src/Peddler/Int16enerator.cs) | X | X | X |   |
| [UInt16](src/Peddler/UInt16Generator.cs) | X | X | X |   |
| [Int32](src/Peddler/Int32Generator.cs) | X | X | X |   |
| [UInt32](src/Peddler/UInt32Generator.cs) | X | X | X |   |
| [Int64](src/Peddler/Int64Generator.cs) | X | X | X |   |
| [UInt64](src/Peddler/UInt64Generator.cs) | X | X | X |   |
| [Guid](src/Peddler/GuidGenerator.cs) | X | X |   |   |
| [DateTime](src/Peddler/DateTimeGenerator.cs) | X | X | X | Enforces consistent use of [`DateTimeKind`](https://msdn.microsoft.com/en-us/library/shx7s921.aspx) |
| [String](src/Peddler/StringGenerator.cs) | X | X |   | Uses [`StringComparison.Ordinal`](https://msdn.microsoft.com/en-us/library/system.stringcomparison.aspx) rules |
| [Enum<T>](src/Peddler/EnumGenerator.cs) | X | X |   |   |
| [Boolean](src/Peddler/BooleanGenerator.cs) | X | X |   |   |

## Wrapper Implementations
Peddler also includes implementations that wrap around base implementations in order to mutate their functionality. For example, a caller may want to periodically produce null, or convert value type `T` into `Nullable<T>`, without having to write a whole new generator. These implementations can wrap around the base implementation types to mutate their underlying behavior.

| Type | `IGenerator<T>` | `IDistinctGenerator<T>` | `IComparableGenerator<T>` | Notes |
| ---- |:---------------:|:-----------------------:|:-------------------------:| ----- |
| [MaybeDefault<T>](src/Peddler/MaybeDefaultGenerator.cs) | X | | | Periodically returns `default(T)` based upon an injected percentage
| [MaybeDefaultDistinct<T>](src/Peddler/MaybeDefaultDistinctGenerator.cs) | X | X | | Periodically returns `default(T)` based upon an injected percentage
| [MaybeDefaultComparable<T>](src/Peddler/MaybeDefaultComparableGenerator.cs) | X | X | X | Periodically returns `default(T)` based upon an injected percentage
| [Nullable<T>](src/Peddler/NullableGenerator.cs) | X | | | Converts `<T>` to `Nullable<T>`
| [NullableDistinct<T>](src/Peddler/NullableDistinctGenerator.cs) | X | X | | Converts `<T>` to `Nullable<T>`
| [NullableComparable<T>](src/Peddler/NullableComparableGenerator.cs) | X | X | X | Converts `<T>` to `Nullable<T>`


## Constraints

If you ever ask a generator to return a value that is impossible (such as asking for a UInt32 that is less than 0), it will throw an [`UnableToGenerateValueException`](src/Peddler/UnableToGenerateValueException.cs). Example:
```cs
using Peddler;

public static void Main(string[] args) {
    Int32Generator generator = new Int32Generator(1, 10);

    try {
        generator.GreaterThan(15);
    } catch (UnableToGenerateValueException) {
        Console.WriteLine(
            "15 is greater than the 10, " +
            "so Int32Generator cannot create a compliant value."
        );
    }
}
```

That's it. <3
