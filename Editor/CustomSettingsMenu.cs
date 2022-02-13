#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OT.Gen.Editor
{
    [InitializeOnLoad]
    public class TagLayersGenSettingsHandler
    {
        private const string genPath = "customSettings.genPath";
        private const string genNamespace = "customSettings.genNamespace";
        private const string genTags = "customSettings.genTags";
        private const string genLayers = "customSettings.genLayers";
        private const string genSortingLayers = "customSettings.genSortingLayers";

        public class GenSettings
        {
            public bool GenTags = false;
            public bool GenLayers = false;
            public bool GenSortingLayers = false;
            public string GenPath = string.Empty;
            public string GenNamespace = string.Empty;
        }

        public static GenSettings GetEditorSettings()
        {
            return new GenSettings
            {
                GenPath = EditorPrefs.GetString(genPath, "/Scripts/Constants/Generated/"),
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

    internal class SettingsGUIContent
    {
        private static GUIContent genPath = new GUIContent("Gen path: ", "gen path");
        private static GUIContent genNamespace = new GUIContent("Gen namespace: ", "gen namespace");
        private static GUIContent genTags = new GUIContent("Enable generate Tags: ");
        private static GUIContent genLayers = new GUIContent("Enable generate Layers: ");
        private static GUIContent genSortingLayers = new GUIContent("Enable generate SortingLayers: ");

        public static void DrawSettingsButtons(TagLayersGenSettingsHandler.GenSettings settings)
        {
            EditorGUI.indentLevel += 1;

            EditorGUILayout.LabelField(
                "Generated classes will be placed by path. Notice 'Assets/' path used by default as root.");
            settings.GenPath = EditorGUILayout.TextField(genPath, settings.GenPath, GUILayout.Width(640));
            EditorGUILayout.LabelField("Add namespace for generated classes if need.");
            settings.GenNamespace =
                EditorGUILayout.TextField(genNamespace, settings.GenNamespace, GUILayout.Width(320));
            settings.GenTags = EditorGUILayout.ToggleLeft(genTags, settings.GenTags);
            settings.GenLayers = EditorGUILayout.ToggleLeft(genLayers, settings.GenLayers);
            settings.GenSortingLayers = EditorGUILayout.ToggleLeft(genSortingLayers, settings.GenSortingLayers);

            EditorGUI.indentLevel -= 1;
        }
    }

#if UNITY_2018_3_OR_NEWER
    static class TagLayersGenSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new SettingsProvider("Preferences/Tag Layers Generator", SettingsScope.User)
            {
                label = "Tag Layers Generator",

                guiHandler = (searchContext) =>
                {
                    TagLayersGenSettingsHandler.GenSettings settings = TagLayersGenSettingsHandler.GetEditorSettings();

                    EditorGUI.BeginChangeCheck();
                    SettingsGUIContent.DrawSettingsButtons(settings);

                    if (EditorGUI.EndChangeCheck())
                    {
                        TagLayersGenSettingsHandler.SetEditorSettings(settings);
                    }
                },

                // Keywords for the search bar in the Unity Preferences menu
                keywords = new HashSet<string>(new[] {"Tags", "Layer", "SortingLayer", "Settings"})
            };

            return provider;
        }
    }
#endif
} // namespace

#endif