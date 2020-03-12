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

## Overview

_TES3_ game files are collections of records and sub-records. Every record and sub-record is identified by a
four character mnemonic and some additional header data.

TES3.NET represents and manages these through three principal classes:

- `SubRecord`

   Represents an individual sub-record.
   
- `Record`

   Represents an individual record. Manages a collection of `SubRecord`s.
   
- `ModFile`

   Represents a _TES3_ game file; either an ESM or ESP file. Manages a collection of `Record`s.

On their own, `Record`s and `SubRecord`s aren't necessarily straightforward to understand. TES3.NET clarifies
them through a higher-level API built on top of them. It has two principal classes:

- `TES3GameItem`

   A clear, human-readable representation of a `Record`. It has its own fields and properties; communication
   with `Record`s is transactional.
   
- `TES3Registry`

   Manages collections of `TES3GameItem`s and their corresponding `Record`s. Reads in `ModFile`s, and 
   automatically translates their `Record`s to `TES3GameItem`s, and manages the relationship between each.


## An Example

The following example program searches the input game files for NPCs that offer services, but attack on sight
(and aren't dead or vampires), and generates a new ESP correcting their `Fight` value to something more
reasonable.

It also generates an "inventory" HTML file detailing affected NPCs.


``` csharp
using System.Collections.Generic;
using System.IO;
using System.Linq;

using TES3.GameItem;
using TES3.GameItem.Item;
using TES3.Records;
using TES3.Util;

// If targeting .NET Core (required 3.0+):
//
// using System.Text;

namespace test
{
    class Program
    {

        // If targeting .NET Core (required 3.0+):
        //
        // static Program()
        // {
        //     Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        // }

        static void Main(string[] args)
        {
            // Read in ModFiles.
            var registry = new TES3Registry();
            foreach (var modFile in ModFile.ReadFiles(args))
            {
                registry.Load(modFile);
            }


            // Setup.
            var outFileName = "better_qorwynn.esp";
            var invFileName = $"{outFileName}-inventory.html";
            byte friendlyFightValue = 30;


            // Primary query.
            var q = from item in registry.GetItemsByType(typeof(NPC))
                    let npc = (NPC) item.Item
                    where npc.OffersServices                              // NPC offers services,
                        && npc.Fight >= 80                                // will attack on sight,
                        && npc.Health > 0                                 // is not dead,
                        && !npc.HeadModel.ToLower().StartsWith("b_v")     // and is not a vampire...

                        // (By default, Agents and Scouts offer services; comment below line to pacify them, as well.)
                        && npc.Class != "Agent" && npc.Class != "Scout"   // and doesn't offer services by default.
                    select item;
            // => Lonely ole' Qorwynn.
            // (assuming last query condition is not commented, and input is only Morrowind.esm, Tribunal.esm, and Bloodmoon.esm)
            

            // Write inventory file.
            using (var writer = new HTMLTagWriter(new StreamWriter(File.Open(invFileName, FileMode.Create))))
            {
                writer.WriteDoctype();
                writer.StartTag("html")
                    .StartTag("body")
                        .StartTag("table").Attribute("id", "report")
                            .StartTag("thead")
                                .StartTag("tr")
                                    .StartTag("th").Content("Id").CloseTag()
                                    .StartTag("th").Content("Name").CloseTag()
                                    .StartTag("th").Content("Old Fight").CloseTag()
                                    .StartTag("th").Content("New Fight").CloseTag()
                                    .StartTag("th").Content("Head").CloseTag()
                                    .StartTag("th").Content("File").CloseTag()
                                .CloseTag()
                            .CloseTag()
                            .StartTag("tbody");
                                foreach (var item in q)
                                {
                                    var npc = (NPC) item.Item;
                                    
                                    writer.StartTag("tr")
                                        .StartTag("td").Content(npc.Name).CloseTag()
                                        .StartTag("td").Content(npc.DisplayName).CloseTag()
                                        .StartTag("td").Content(npc.Fight).CloseTag()
                                        .StartTag("td").Content(friendlyFightValue).CloseTag()
                                        .StartTag("td").Content(npc.HeadModel).CloseTag()
                                        .StartTag("td").Content(item.ModFileName).CloseTag()
                                    .CloseTag();
                                }
                            writer.CloseTag()
                        .CloseTag()
                    .CloseTag()
                .CloseTag();
            }


            // Create new file.
            registry.CreateModFile(outFileName, "<Your Name>", "A world with a better Qorwynn.");

            // Register parents.
            var parents = new HashSet<string>();
            foreach (var item in q)
            {
                parents.Add(item.ModFileName);                
            }
            registry.AddParents(outFileName, parents);


            // Aggregate source NPCs.
            var sources = new List<NPC>();
            foreach (var item in q)
            {
                sources.Add((NPC) item.Item);
            }
            
            // Add targets, update Fight.
            var targets = registry.AddItems(outFileName, sources);
            foreach (NPC npc in targets)
            {
                npc.Fight = friendlyFightValue;
            }


            // Write output.
            registry.WriteModFile(".", outFileName);

        }
        
    }
}
```

## Documentation

TBD


## Licence

TES3.NET is licenced under the MIT and BSD-3-Clause free software licences. A copy can be found [here](LICENCE).

