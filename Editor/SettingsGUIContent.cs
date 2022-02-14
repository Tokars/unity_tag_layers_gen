using System.IO;
using UnityEditor;
using UnityEngine;

namespace OT.Gen.Editor
{
    internal static class SettingsGUIContent
    {
        private static GUIContent genPath = new GUIContent("Gen path: ", "gen path");
        private static GUIContent genNamespace = new GUIContent("Gen namespace", "gen namespace");
        private static GUIContent genTags = new GUIContent("Generate Tags");
        private static GUIContent genLayers = new GUIContent("Generate Layers");
        private static GUIContent genSortingLayers = new GUIContent("Generate SortingLayers");

        public static void DrawSettingsButtons(GenSettings settings)
        {
            EditorGUI.indentLevel += 1;

            EditorGUILayout.LabelField(
                "Generated classes will be placed by path relatively Assets/ folder.");
            settings.GenPath = EditorGUILayout.TextField(genPath, settings.GenPath, GUILayout.Width(640));
            EditorGUILayout.LabelField("Add namespace for generated classes if need.");
            settings.GenNamespace =
                EditorGUILayout.TextField(genNamespace, settings.GenNamespace, GUILayout.Width(640));
            settings.GenTags = EditorGUILayout.ToggleLeft(genTags, settings.GenTags);
            settings.GenLayers = EditorGUILayout.ToggleLeft(genLayers, settings.GenLayers);
            settings.GenSortingLayers = EditorGUILayout.ToggleLeft(genSortingLayers, settings.GenSortingLayers);

            GUILayout.Space(24);
            if (GUILayout.Button(nameof(Generate), GUILayout.MaxWidth(120)))
                Generate();

            EditorGUI.indentLevel -= 1;


            void Generate()
            {
                var path = Application.dataPath;
                if (settings.GenPath.Substring(0, 1) != "/")
                    path += "/" + settings.GenPath;
                else path += settings.GenPath;

                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);

                TagsLayersClassesGenerator.Run(settings, path);
            }
        }
    }
}