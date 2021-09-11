using System.Collections.Generic;
using JetBrains.Annotations;
using Verse;

namespace NoVersionWarning
{
    public class ModUpdateTag : ThingDef
    {
        [UsedImplicitly] public readonly List<string> ModIdsToFix = new List<string>();
    }
}