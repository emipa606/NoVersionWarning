using HarmonyLib;
using Verse;

namespace NoVersionWarning;

[HarmonyPatch(typeof(ModLister), "RebuildModList")]
public static class ModLister_RebuildModList
{
    [HarmonyPostfix]
    public static void MainMenuOnGUI()
    {
        if (!Main.modIdsToUpdate.Any())
        {
            return;
        }

        var reportMods = !Main.updatedMods.Any();

        foreach (var modId in Main.modIdsToUpdate)
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

            modInfo.meta.SupportedVersions.Add(Main.currentVersion);
            modInfo.meta.description =
                $"<color=yellow>Version-tag {Main.currentVersion.Major}.{Main.currentVersion.Minor} added by No Version Warning-mod</color>\n\n{modInfo.meta.description}";

            Main.updatedMods.Add(modInfo.Name);
        }

        if (reportMods && Main.updatedMods.Any())
        {
            Log.Message(
                $"[NoVersionWarning]: Updated the version tag for the following mods:\n{string.Join("\n", Main.updatedMods)}");
        }
    }
}