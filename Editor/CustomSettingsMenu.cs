#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OT.Gen.Editor
{
    public class GenSettings
    {
        public bool GenTags = false;
        public bool GenLayers = false;
        public bool GenSortingLayers = false;
        public string GenPath = string.Empty;
        public string GenNamespace = string.Empty;

        public override string ToString()
        {
            return
                $"{nameof(GenTags)}: {GenTags}, {nameof(GenLayers)}: {GenLayers}, {nameof(GenSortingLayers)}: {GenSortingLayers}, {nameof(GenPath)}: {GenPath}, {nameof(GenNamespace)}: {GenNamespace}";
        }
    }


    [InitializeOnLoad]
    public class TagLayersGenSettingsHandler
    {
        private const string genPath = "customSettings.genPath";
        private const string genNamespace = "customSettings.genNamespace";
        private const string genTags = "customSettings.genTags";
        private const string genLayers = "customSettings.genLayers";
        private const string genSortingLayers = "customSettings.genSortingLayers";


        public static GenSettings GetEditorSettings()
        {
            return new GenSettings
            {
                GenPath = EditorPrefs.GetString(genPath, "Scripts/Constants/Generated/"),
                GenNamespace = EditorPrefs.GetString(genNamespace, ""),
                GenTags = EditorPrefs.GetBool(genTags, false),
                GenLayers = EditorPrefs.GetBool(genLayers, false),
                GenSortingLayers = EditorPrefs.GetBool(genSortingLayers, false)
            };
        }

        public static void SetEditorSettings(GenSettings settings)
        {
            EditorPrefs.SetString(genPath, settings.GenPath);
            EditorPrefs.SetString(genNamespace, settings.GenNamespace);
            EditorPrefs.SetBool(genTags, settings.GenTags);
            EditorPrefs.SetBool(genLayers, settings.GenLayers);
            EditorPrefs.SetBool(genSortingLayers, settings.GenSortingLayers);
        }
    }

    static class TagLayersGenSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new SettingsProvider("Preferences/Tags Layers Generator", SettingsScope.User)
            {
                label = "Tags Layers Generator",

                guiHandler = (searchContext) =>
                {
                    GenSettings settings = TagLayersGenSettingsHandler.GetEditorSettings();

                    EditorGUI.BeginChangeCheck();
                    SettingsGUIContent.DrawSettingsButtons(settings);

                    if (EditorGUI.EndChangeCheck())
                    {
                        TagLayersGenSettingsHandler.SetEditorSettings(settings);
                        Debug.Log(settings.ToString());
                    }
                },

                // Keywords for the search bar in the Unity Preferences menu
                keywords = new HashSet<string>(new[] {"Tags", "Layers", "SortingLayers", "Settings"})
            };

            return provider;
        }
    }
}

#endif