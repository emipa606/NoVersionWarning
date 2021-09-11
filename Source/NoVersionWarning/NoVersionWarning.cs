using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace NoVersionWarning
{
    [StaticConstructorOnStartup]
    public class NoVersionWarning
    {
        public static readonly Version currentVersion;
        public static readonly List<string> modIdsToUpdate;
        public static readonly HashSet<string> updatedMods;

        static NoVersionWarning()
        {
            var foundModIdDefs = DefDatabase<ModUpdateTag>.AllDefsListForReading;
            updatedMods = new HashSet<string>();
            if (!foundModIdDefs.Any())
            {
                Log.Message("[NoVersionWarning]: No defs with mod-ids found, ignoring.");
                return;
            }

            modIdsToUpdate = new List<string>();
            foreach (var modUpdateTag in foundModIdDefs)
            {
                if (modUpdateTag.ModIdsToFix == null || !modUpdateTag.ModIdsToFix.Any())
                {
                    continue;
                }

                foreach (var modId in modUpdateTag.ModIdsToFix)
                {
                    modIdsToUpdate.Add(modId);
                }
            }

            if (!modIdsToUpdate.Any())
            {
                Log.Message("[NoVersionWarning]: No mod-ids found, ignoring.");
                return;
            }

            var currentVersionString = VersionControl.CurrentVersionStringWithoutBuild;
            if (currentVersionString == null)
            {
                Log.Message("[NoVersionWarning]: Could not figure out current game-version, ignoring.");
                return;
            }

            currentVersion = new Version(currentVersionString);

            var harmony = new Harmony("Mlie.NoVersionWarning");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}