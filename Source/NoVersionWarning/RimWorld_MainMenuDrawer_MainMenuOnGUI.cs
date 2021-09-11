using System;
using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace NoVersionWarning
{
    [HarmonyPatch(typeof(ModLister), "RebuildModList")]
    public static class ModLister_RebuildModList
    {
        [HarmonyPostfix]
        public static void MainMenuOnGUI()
        {
            if (!NoVersionWarning.modIdsToUpdate.Any())
            {
                return;
            }

            var reportMods = !NoVersionWarning.updatedMods.Any();

            foreach (var modId in NoVersionWarning.modIdsToUpdate)
            {
                var modInfo = ModLister.GetModWithIdentifier(modId);
                if (modInfo == null)
                {
                    continue;
                }

                if (modInfo.VersionCompatible)
                {
                    continue;
                }

                var metaTraverse = Traverse.Create(modInfo).Field("meta");
                if (metaTraverse == null)
                {
                    continue;
                }

                var versionsTraverse = metaTraverse.Property("SupportedVersions");
                if (versionsTraverse == null)
                {
                    continue;
                }

                var descriptionTraverse = metaTraverse.Field("description");
                if (descriptionTraverse == null)
                {
                    continue;
                }

                var currentVersions = (List<Version>)versionsTraverse.GetValue();

                if (currentVersions == null)
                {
                    continue;
                }

                currentVersions.Add(NoVersionWarning.currentVersion);

                versionsTraverse.SetValue(currentVersions);

                var currentDescription = descriptionTraverse.GetValue();
                if (currentDescription != null)
                {
                    descriptionTraverse.SetValue(
                        $"<color=yellow>Version-tag {NoVersionWarning.currentVersion.Major}.{NoVersionWarning.currentVersion.Minor} added by No Version Warning-mod</color>\n\n{currentDescription}");
                }

                NoVersionWarning.updatedMods.Add(modInfo.Name);
            }

            if (reportMods && NoVersionWarning.updatedMods.Any())
            {
                Log.Message(
                    $"[NoVersionWarning]: Updated the version tag for the following mods:\n {string.Join("\n", NoVersionWarning.updatedMods)}");
            }
        }
    }
}