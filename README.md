# TES3.NET (alpha)

TES3.NET is a .NET standard class library for reading and writing _The Elder Scrolls III: Morrowind_ game
files. As of now, the library is in alpha state; all beavior is subject to change, and proper function is
not guaranteed.


## Installation

Currently Visual Studio is required to build the project DLLs locally. A [NuGet](https://www.nuget.org/)
distribution is coming soon.

Once built, the required assemblies can be referenced directly. You will need to reference at least the 
following:
- `TES3.Util.dll`
- `TES3.Core.dll`
- `TES3.Records.dll`

And if you also desire the higher-level functionality:
- `TES3.GameItem.dll`


## Runtime Requirements

This library targets `netstandard2.0`, but the game file text encoding is `Windows-1252`. Though this encoding
is fully supported by the .NET Framework, it is not (by default) in .NET Core. If your application targets
.NET Core, it must target version 3.0 or above, and register the `CodePagesEncodingProvider`.

To do this, you must add a reference to the `System.Text.Encoding.CodePages` NuGet package. This is most
easily done through the `dotnet` tool in a Visual Studio developer command line environment. In the project's
root directory (containing the `.proj` file):
```
> dotnet add package System.Text.Encoding.CodePages
```

To register the provider in your code:
``` csharp
using System.Text;

// ...

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
```

Technically the `EncodingProvider` must be registered prior to any game files containing text being read, 
but our recommendation is to do it in a centralized static constructor.


## Documentation

TBD


## Licence

TES3.NET is licenced under the MIT and BSD-3-Clause free software licences. A copy can be found [here](LICENCE).

