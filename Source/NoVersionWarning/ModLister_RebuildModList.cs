using HarmonyLib;
using Verse;

namespace NoVersionWarning;

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


            modInfo.meta.SupportedVersions.Add(NoVersionWarning.currentVersion);
            modInfo.meta.description =
                $"<color=yellow>Version-tag {NoVersionWarning.currentVersion.Major}.{NoVersionWarning.currentVersion.Minor} added by No Version Warning-mod</color>\n\n{modInfo.meta.description}";

            NoVersionWarning.updatedMods.Add(modInfo.Name);
        }

        if (reportMods && NoVersionWarning.updatedMods.Any())
        {
            Log.Message(
                $"[NoVersionWarning]: Updated the version tag for the following mods:\n{string.Join("\n", NoVersionWarning.updatedMods)}");
        }
    }
}