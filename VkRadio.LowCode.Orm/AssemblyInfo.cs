using System;

// CA1014: Mark assemblies with CLSCompliantAttribute
// According to https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1014
// I need either to mark my assembly as CLS compliant and then explicitly exclude non-compliant types,
// or to mark the whole assembly as non-compliant. Because I have no time to research the nuances of
// CLS compliance, I use the latter variant.
[assembly: CLSCompliant(false)]
