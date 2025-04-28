# Lapidary

A .NET adapter for connecting to GemStone/S 64 databases, version 3.7 or newer.

## Disclaimer

While this adapter is a "usable" for basic workloads, it's subject to ongoing development and doesn't come with important guarantees like a stabilised API.

## Methodology

Aside from "just working", this library aims to be approachable for both .NET and GemStone developers alike. From the .NET side it should be idiomatic enough to easily focus on working with OODBMS patterns, while the GemStone side should be able to easily grep the code despite the syntax differences.

## Current Functionality

- Connect to the database using plaintext, encrypted or X509 credentials
- Execute arbitrary Smalltalk code
- Call parameterised selectors on objects in your Gem session
- Use Begin-Commit/Abort transaction workflow
- Create GemStone to CLR class mappers
- Async support for non-blocking calls (Experimental)
- Basic DI support

## Planned Work

- Example docker environment
- Proper error handling
- Documentation
- User-friend extension methods
- Port more trivial conversion to managed code
- Authentication re-work, including Kerberos
- Plus more...


## Can I use this with an older GemStone database?

In theory yes, back to version 3.3 for the moment. While 3.7 saw significant improvements to the GciTs interface, a majority of the core functionality has remained stable. Removing the `BoolType* executedSessionInit` parameter from the `GciTsLogin` call should be enough to get going.

## Trivial Example

```csharp
// Injected "GemContext", similar to an EF DbContext or Topaz gem

public GemObject BasicUsage()
{
	var twoObject = context.PerformSmalltalkRaw("1 + 1"u8);
	var numberTwo = twoObject.GetNumber<int>();
	
	var twoStringObject = twoObject.Perform("printString"u8);
	var stringTwo = twoStringObject.GetString();

	return twoObject;
}
```
