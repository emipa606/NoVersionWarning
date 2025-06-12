using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace NoVersionWarning;

[HarmonyPatch(typeof(ModLister), nameof(ModLister.RebuildModList))]
public static class ModLister_RebuildModList
{
    private static readonly FieldInfo metaFieldInfo = AccessTools.Field(typeof(ModMetaData), "meta");
    private static readonly FieldInfo descriptionFieldInfo = AccessTools.Field("ModMetaDataInternal:description");

    private static readonly PropertyInfo supportedVersionsPropertyInfo =
        AccessTools.Property("ModMetaDataInternal:SupportedVersions");

    [HarmonyPostfix]
    public static void MainMenuOnGUI()
    {
        if (!Main.ModIdsToUpdate.Any())
        {
            return;
        }

        var reportMods = !Main.UpdatedMods.Any();

        foreach (var modId in Main.ModIdsToUpdate)
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

            var metaObject = metaFieldInfo.GetValue(modInfo);
            if (metaObject == null)
            {
                Log.Warning($"[NoVersionWarning]: Mod {modInfo.Name} has no meta data, skipping.");
                continue;
            }

            var currentDescription = (string)descriptionFieldInfo.GetValue(metaObject);
            if (currentDescription != null)
            {
                descriptionFieldInfo.SetValue(metaObject,
                    $"<color=yellow>{"NVW.TagAdded".Translate($"{Main.CurrentVersion.Major}.{Main.CurrentVersion.Minor}")}</color>\n\n{currentDescription}");
            }

            var supportedVersions = (List<Version>)supportedVersionsPropertyInfo.GetValue(metaObject);
            if (supportedVersions == null)
            {
                Log.Warning($"[NoVersionWarning]: Mod {modInfo.Name} has no supported versions, skipping.");
                continue;
            }

            if (supportedVersions.Contains(Main.CurrentVersion))
            {
                continue;
            }

            supportedVersions.Add(Main.CurrentVersion);
            supportedVersionsPropertyInfo.SetValue(metaObject, supportedVersions);
            //modInfo.meta.SupportedVersions.Add(Main.CurrentVersion);
            //modInfo.meta.description =
            //    $"<color=yellow>{"NVW.TagAdded".Translate($"{Main.CurrentVersion.Major}.{Main.CurrentVersion.Minor}")}</color>\n\n{modInfo.meta.description}";

            Main.UpdatedMods.Add(modInfo.Name);
        }

        if (reportMods && Main.UpdatedMods.Any())
        {
            Log.Message(
                $"[NoVersionWarning]: Updated the version tag for the following mods:\n{string.Join("\n", Main.UpdatedMods)}");
        }
    }
}