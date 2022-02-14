using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Based-on a gist: https://gist.github.com/Namek/ecafa24a6ae3d730baf1
/// </summary>
namespace OT.Gen.Editor
{
    public static class TagsLayersClassesGenerator
    {
        private static string _namespace;

        private static string _comment =
            "// This class is auto-generated, do not modify (use Preferences:TagsLayersGenerator)\n\n";

        static string _tab = "    ";

        public static void Run(GenSettings settings, string savePath)
        {
            _namespace = settings.GenNamespace;
            if (settings.GenTags)
            {
                GenerateTagsFile(savePath + "Tags.cs");
                AssetDatabase.ImportAsset(savePath + "Tags.cs", ImportAssetOptions.ForceUpdate);
            }

            if (settings.GenLayers)
            {
                GenerateLayersFile(savePath + "Layers.cs");
                AssetDatabase.ImportAsset(savePath + "Layers.cs", ImportAssetOptions.ForceUpdate);
            }

            if (settings.GenSortingLayers)
            {
                GenerateSortingLayersFile(savePath + "SortingLayers.cs");
                AssetDatabase.ImportAsset(savePath + "SortingLayers.cs", ImportAssetOptions.ForceUpdate);
            }
        }

        /// <summary> Generation Tags.cs file.</summary>
        /// <param name="filePath">file path</param>
        private static void GenerateTagsFile(string filePath)
        {
            string tab = _tab;
            StringBuilder sb = new StringBuilder();

            sb.Append(_comment);
            if (string.IsNullOrEmpty(_namespace) == false)
            {
                sb.Append("namespace " + $"{_namespace}" + "\n{\n");
                tab += tab;
                sb.Append(_tab + "public sealed class Tags\n" + _tab + "{\n");
            }
            else
                sb.Append("public sealed class Tags\n{\n");

            var srcArr = UnityEditorInternal.InternalEditorUtility.tags;
            var tags = new String[srcArr.Length];
            Array.Copy(srcArr, tags, tags.Length);
            Array.Sort(tags, StringComparer.InvariantCultureIgnoreCase);

            for (int i = 0, n = tags.Length; i < n; ++i)
            {
                string tagName = tags[i];

                sb.Append(tab + "public const string " + tagName + " = \"" + tagName + "\";\n");
            }

            if (string.IsNullOrEmpty(_namespace) == false)
                sb.Append(_tab + "}\n");
            sb.Append("}\n");
            File.WriteAllText(filePath, sb.ToString());
        }

        /// <summary> Generation Layers.cs file.</summary>
        /// <param name="filePath">file path</param>
        private static void GenerateLayersFile(string filePath)
        {
            string tab = _tab;
            StringBuilder sb = new StringBuilder();

            sb.Append(_comment);
            if (string.IsNullOrEmpty(_namespace) == false)
            {
                sb.Append("namespace " + $"{_namespace}" + " \n{\n");
                tab += tab;
                sb.Append(_tab + "public sealed class Layers\n" + _tab + "{\n");
            }
            else
                sb.Append("public sealed class Layers\n{\n");

            var layers = UnityEditorInternal.InternalEditorUtility.layers;

            for (int i = 0, n = layers.Length; i < n; ++i)
            {
                string layerName = layers[i];

                sb.Append(tab + "public const string " + GetVariableName(layerName) + " = \"" + layerName + "\";\n");
            }

            sb.Append("\n");

            for (int i = 0, n = layers.Length; i < n; ++i)
            {
                string layerName = layers[i];
                int layerNumber = LayerMask.NameToLayer(layerName);
                string layerMask = layerNumber == 0 ? "1" : "1 << " + layerNumber;

                sb.Append(tab + "public const int " + GetVariableName(layerName) + "Mask" + " = " + layerMask + ";\n");
            }

            sb.Append("\n");

            for (int i = 0, n = layers.Length; i < n; ++i)
            {
                string layerName = layers[i];
                int layerNumber = LayerMask.NameToLayer(layerName);

                sb.Append(tab + "public const int " + GetVariableName(layerName) + "Number" + " = " + layerNumber +
                          ";\n");
            }

            if (string.IsNullOrEmpty(_namespace) == false)
                sb.Append(_tab + "}\n");
            sb.Append("}\n");
            File.WriteAllText(filePath, sb.ToString());
        }

        /// <summary> Generation SortingLayers.cs file</summary>
        /// <param name="filePath">file path</param>
        private static void GenerateSortingLayersFile(string filePath)
        {
            string tab = _tab;
            StringBuilder sb = new StringBuilder();
            sb.Append(_comment);
            if (string.IsNullOrEmpty(_namespace) == false)
            {
                sb.Append("namespace " + $"{_namespace}" + " \n{\n");
                tab += tab;
                sb.Append(_tab + "public sealed class SortingLayers\n" + _tab + "{\n");
            }
            else
                sb.Append("public sealed class SortingLayers\n{\n");


            var sortingLayers = SortingLayer.layers;

            for (int i = 0, n = sortingLayers.Length; i < n; ++i)
            {
                string layerName = sortingLayers[i].name;

                sb.Append(tab + "public const string " + GetVariableName(layerName) + " = \"" + layerName + "\";\n");
            }

            sb.Append("\n");

            for (int i = 0, n = sortingLayers.Length; i < n; ++i)
            {
                string layerName = sortingLayers[i].name;
                int id = sortingLayers[i].id;

                sb.Append(tab + "public const int " + GetVariableName(layerName) + "Id" + " = " + id + ";\n");
            }

            if (string.IsNullOrEmpty(_namespace) == false)
                sb.Append(_tab + "}\n");
            sb.Append("}\n");
            File.WriteAllText(filePath, sb.ToString());
        }

        private static string GetVariableName(string str)
        {
            return str.Replace(" ", "");
        }
    }
}