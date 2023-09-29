using System;
using System.IO;
using System.Text;
using OT.Gen.Editor.Settings;
using UnityEditor;
using UnityEngine;

namespace OT.Gen.Editor
{
    public static class TlGenerator
    {
        private const string Comment =
            "// This class is auto-generated, do not modify (use ProjectSettings:Tags and Layers > Generator)\n\n";

        private const string Tab = "    ";
        private static string _namespace;

        private static string CreateDirectory(string path)
        {
            // Debug.Log($"Create dir: {path}");
            var createPath = Path.Combine(Application.dataPath, path);
            Directory.CreateDirectory(createPath);
            return createPath;
        }
        public static void GenClass(string path, string nameSpace, string typeName, TlsType type)
        {
            if (TlgSettingsValidator.IsValidNamespace(nameSpace) == false)
            {
                Debug.LogError("Invalid namespace.");
                return;
            }
            _namespace = nameSpace;
            string validPath = CreateDirectory(path);
            switch (type)
            {
                case TlsType.Tag:
                    GenTagsSource(validPath + $"/{typeName}.cs", typeName);
                    break;
                case TlsType.Layer:
                    GenLayersSource(validPath + $"/{typeName}.cs", typeName);
                    break;
                case TlsType.SortingLayer:
                    GenSortLayersSource(validPath + $"/{typeName}.cs", typeName);
                    break;
            }
            AssetDatabase.Refresh();
            Debug.Log($"Script generated: {path}");
        }

        /// <summary> Generation Tags.cs file.</summary>
        /// <param name="filePath">file path</param>
        /// <param name="typeName">type name</param>
        private static void GenTagsSource(string filePath, string typeName)
        {
            string tab = Tab;
            string open = "\n{\n";
            StringBuilder sb = new StringBuilder();

            sb.Append(Comment);
            if (string.IsNullOrEmpty(_namespace) == false)
            {
                sb.Append("namespace " + $"{_namespace}{open}");
                tab += tab;
                sb.Append(Tab + $"public sealed class {typeName}{Tab + open}");
            }
            else
                sb.Append($"public sealed class {typeName}{Tab + open}");

            var srcArr = UnityEditorInternal.InternalEditorUtility.tags;
            var tags = new string[srcArr.Length];
            Array.Copy(srcArr, tags, tags.Length);
            Array.Sort(tags, StringComparer.InvariantCultureIgnoreCase);

            for (int i = 0, n = tags.Length; i < n; ++i)
            {
                string tagName = tags[i];

                sb.Append(tab + "public const string " + tagName + " = \"" + tagName + "\";\n");
            }

            if (string.IsNullOrEmpty(_namespace) == false)
                sb.Append(Tab + "}\n");
            sb.Append("}\n");

            using StreamWriter swm = File.CreateText(filePath);
            swm.Write(sb.ToString());
            swm.Close();

            
        }

        /// <summary> Generation Layers.cs file.</summary>
        /// <param name="filePath">file path</param>
        /// <param name="typeName">type name</param>
        private static void GenLayersSource(string filePath, string typeName)
        {
            string tab = Tab;
            string open = "\n{\n";
            StringBuilder sb = new StringBuilder();

            sb.Append(Comment);
            if (string.IsNullOrEmpty(_namespace) == false)
            {
                sb.Append("namespace " + $"{_namespace} {open}");
                tab += tab;
                sb.Append(Tab + $"public sealed class {typeName} {Tab + open}");
            }
            else
                sb.Append($"public sealed class {typeName} {Tab + open}");

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
                sb.Append(Tab + "}\n");
            sb.Append("}\n");
            using StreamWriter swm = File.CreateText(filePath);
            swm.Write(sb.ToString());
        }

        /// <summary> Generation SortingLayers.cs file</summary>
        /// <param name="filePath">file path</param>
        /// <param name="typeName">type name</param>
        private static void GenSortLayersSource(string filePath, string typeName)
        {
            string tab = Tab;
            string open = "\n{\n";
            StringBuilder sb = new StringBuilder();
            sb.Append(Comment);
            if (string.IsNullOrEmpty(_namespace) == false)
            {
                sb.Append("namespace " + $"{_namespace}{open}");
                tab += tab;
                sb.Append(Tab + $"public sealed class {typeName}{Tab + open}");
            }
            else
                sb.Append($"public sealed class {typeName}{Tab + open}");


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
                sb.Append(Tab + "}\n");
            sb.Append("}\n");
            using StreamWriter swm = File.CreateText(filePath);
            swm.Write(sb.ToString());
        }

        private static string GetVariableName(string str)
        {
            return str.Replace(" ", "");
        }
    }
}