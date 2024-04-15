using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using HarmonyLib;
using RimWorld;
using Verse;

namespace NoVersionWarning;

public static class Main
{
    public static Version currentVersion;
    public static List<string> modIdsToUpdate;
    public static HashSet<string> updatedMods;
}

public class NoVersionWarningMod : Mod
{
    public NoVersionWarningMod(ModContentPack content) : base(content)
    {
        var currentVersionString = VersionControl.CurrentVersionStringWithoutBuild;
        if (currentVersionString == null)
        {
            Log.Message("[NoVersionWarning]: Could not figure out current game-version, ignoring.");
            return;
        }

        Main.currentVersion = new Version(currentVersionString);
        Main.updatedMods = [];
        Main.modIdsToUpdate = fixedIdsIn();

        if (!Main.modIdsToUpdate.Any())
        {
            Log.Message("[NoVersionWarning]: No mod-ids found, ignoring.");
            return;
        }

        var harmony = new Harmony("Mlie.NoVersionWarning");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    private List<string> fixedIdsIn()
    {
        var xmlFolder = Path.Combine(Content.RootDir, Main.currentVersion.ToString());
        if (!Directory.Exists(xmlFolder))
        {
            return [];
        }

        var xmlFile = Path.Combine(xmlFolder, "ModIdsToFix.xml");
        if (!File.Exists(xmlFile))
        {
            return [];
        }

        var xmlDocument = new XmlDocument();
        xmlDocument.Load(xmlFile);

        var returnValue = new List<string>();
        var modIds = xmlDocument.GetElementsByTagName("li");
        foreach (XmlNode modId in modIds)
        {
            returnValue.Add(modId.InnerText);
        }

        return returnValue;
    }
}