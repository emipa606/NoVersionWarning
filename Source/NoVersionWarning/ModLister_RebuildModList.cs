using HarmonyLib;
using Verse;

namespace NoVersionWarning;

[HarmonyPatch(typeof(ModLister), nameof(ModLister.RebuildModList))]
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
                $"<color=yellow>{"NVW.TagAdded".Translate($"{Main.currentVersion.Major}.{Main.currentVersion.Minor}")}</color>\n\n{modInfo.meta.description}";

            Main.updatedMods.Add(modInfo.Name);
        }

        if (reportMods && Main.updatedMods.Any())
        {
            Log.Message(
                $"[NoVersionWarning]: Updated the version tag for the following mods:\n{string.Join("\n", Main.updatedMods)}");
        }
    }
}