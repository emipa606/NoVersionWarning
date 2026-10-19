# GitHub Copilot Instructions for No Version Warning Mod

## Mod Overview and Purpose

**Mod Name:** No Version Warning  
**Author:** Mlie  
**PackageId:** Mlie.NoVersionWarning  

The **No Version Warning** mod addresses a common issue mod creators face with the compatibility tags of RimWorld mods. Often, mods that work perfectly on newer game versions are marked as incompatible simply because they lack an updated version tag. This leads to confusion and unnecessary update requests. The mod's purpose is to automatically modify the mod-list loader within RimWorld, adding the required version tag to mods verified to work with the current game version. This ensures that users can confidently use these mods without warnings of incompatibility.

## Key Features and Systems

- **Automated Version Tagging:** The mod identifies mods that are functioning with the latest RimWorld version and dynamically adds the necessary version tags.
- **Seamless Integration:** No Version Warning mod can be placed anywhere in your mod-list, as it merely modifies the loader's behavior.
- **User Contribution:** Players are encouraged to report mods that function correctly to add them for version tag updates.
- **Direct Influence:** External mod managers like RimPy may not recognize changes unless they support the mod's features.

## Coding Patterns and Conventions

The mod structure consists of organized C# files and an XML file tailored for dynamic mod-list modifications. Below are the conventions utilized:

- **Namespace Organization:** Code is encapsulated within namespaces to maintain clarity.
- **Type Definitions:** Clear class definitions are in place for types like `ModLister_RebuildModList`.
- **Method Naming Conventions:** Methods like `MainMenuOnGUI` follow PascalCase conventions for ease of readability.

## XML Integration

While most logic occurs in C#, an XML file named `ModIdsToFix.xml` is also utilized. This file is structured to list mod IDs that the mod alters. Developers editing this file should ensure:

- Proper tag closure: Ensure all XML elements are closed properly.
- Consistent formatting: Maintain consistent indentation for better readability.
- Accurate IDs: Verify that all mod IDs are correct to prevent unintended modifications.

## Harmony Patching

The mod uses the Harmony library to apply runtime patches, a crucial component for safe and dynamic code alterations within RimWorld:

- **Dependency:** brrainz.harmony
- **Main Patching Class:** `ModLister_RebuildModList`
- **Method Patching:** The mod modifies the mod-list behavior within `MainMenuOnGUI` to suppress version warnings for verified mods.

Developers should follow the Harmony patching documentation to apply new patches effectively, ensuring compatibility with future RimWorld updates.

## Suggestions for Copilot

To leverage GitHub Copilot effectively, consider:

1. **Prompting Contextual Code Suggestions:** Provide context around mod-list modification or patching points for more accurate code completion.
2. **Refactoring Assistance:** Use Copilot to refactor C# methods for improved efficiency or adherence to modular programming principles.
3. **XML Structure Completion:** Let Copilot assist in completing XML entries for `ModIdsToFix.xml`, ensuring adherence to existing patterns.
4. **Logging Enhancements:** Prompt Copilot to suggest logging frameworks or inline log statements to aid in debugging.
5. **Code Documentation:** Encourage Copilot to generate or augment inline documentation, enhancing code readability for contributors.

In summary, this structured guide aids developers in extending and maintaining the No Version Warning mod, leveraging both XML configurations and Harmony patching within the RimWorld modding ecosystem.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

