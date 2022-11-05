using System.Collections.Generic;
using Verse;

namespace NoVersionWarning;

public class ModUpdateTag : ThingDef
{
    public readonly List<string> ModIdsToFix = new List<string>();
}