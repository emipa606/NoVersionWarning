using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using HarmonyLib;
using RimWorld;
using Verse;

namespace NoVersionWarning;

public static class Main
{
    public static Version CurrentVersion;
    public static List<string> ModIdsToUpdate;
    public static HashSet<string> UpdatedMods;
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

        Main.CurrentVersion = new Version(currentVersionString);
        Main.UpdatedMods = [];
        Main.ModIdsToUpdate = fixedIdsIn();

        if (!Main.ModIdsToUpdate.Any())
        {
            Log.Message("[NoVersionWarning]: No mod-ids found, ignoring.");
            return;
        }

        var harmony = new Harmony("Mlie.NoVersionWarning");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    private List<string> fixedIdsIn()
    {
        var xmlFolder = Path.Combine(Content.RootDir, Main.CurrentVersion.ToString());
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

        var modIds = xmlDocument.GetElementsByTagName("li");

        return (from XmlNode modId in modIds select modId.InnerText).ToList();
    }
}