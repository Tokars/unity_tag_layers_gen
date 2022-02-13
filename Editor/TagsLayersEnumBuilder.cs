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
    public class TagsLayersEnumBuilder : EditorWindow
    {
        [MenuItem("Edit/Rebuild Tags And Layers Enums")]
        static void RebuildTagsAndLayersEnums()
        {
            var enumsPath = Application.dataPath + "/Scripts/Generated/Constant/";

            // todo: Generate file optional.
            RebuildTagsFile(enumsPath + "Tags.cs");
            RebuildLayersFile(enumsPath + "Layers.cs");
            RebuildSortingLayers(enumsPath + "SortingLayers.cs");

            AssetDatabase.ImportAsset(enumsPath + "Tags.cs", ImportAssetOptions.ForceUpdate);
            AssetDatabase.ImportAsset(enumsPath + "Layers.cs", ImportAssetOptions.ForceUpdate);
            AssetDatabase.ImportAsset(enumsPath + "SortingLayers.cs", ImportAssetOptions.ForceUpdate);
        }

        static void RebuildTagsFile(string filePath)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("//This class is auto-generated, do not modify (TagsLayersEnumBuilder.cs)\n");
            sb.Append("public sealed class Tags \n{\n");

            var srcArr = UnityEditorInternal.InternalEditorUtility.tags;
            var tags = new String[srcArr.Length];
            Array.Copy(srcArr, tags, tags.Length);
            Array.Sort(tags, StringComparer.InvariantCultureIgnoreCase);

            for (int i = 0, n = tags.Length; i < n; ++i)
            {
                string tagName = tags[i];

                sb.Append("\tpublic const string " + tagName + " = \"" + tagName + "\";\n");
            }

            sb.Append("}\n");

#if !UNITY_WEBPLAYER
            File.WriteAllText(filePath, sb.ToString());
#endif
        }

        static void RebuildLayersFile(string filePath, string fileNamespace = "")
        {
            string tab = "\t";
            StringBuilder sb = new StringBuilder();

            sb.Append("//This class is auto-generated, do not modify (use Tools/TagsLayersEnumBuilder)\n");
            if (string.IsNullOrEmpty(fileNamespace) == false)
            {
                sb.Append("namespace " + $"{fileNamespace}" + " \n{\n");
                tab += tab;
            }

            sb.Append(tab + "public sealed class Layers \n{\n");

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
                string layerMask = layerNumber == 0 ? "1" : ("1 << " + layerNumber);

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

            sb.Append(tab + "}\n");
            sb.Append("}\n");

#if !UNITY_WEBPLAYER
            File.WriteAllText(filePath, sb.ToString());
#endif
        }

        private static void RebuildSortingLayers(string filePath)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("//This class is auto-generated, do not modify (use Tools/TagsLayersEnumBuilder)\n");
            sb.Append("public sealed class SortingLayers \n{\n");

            var sortingLayers = SortingLayer.layers;

            for (int i = 0, n = sortingLayers.Length; i < n; ++i)
            {
                string layerName = sortingLayers[i].name;

                sb.Append("\tpublic const string " + GetVariableName(layerName) + " = \"" + layerName + "\";\n");
            }

            sb.Append("\n");

            for (int i = 0, n = sortingLayers.Length; i < n; ++i)
            {
                string layerName = sortingLayers[i].name;
                int id = sortingLayers[i].id;

                sb.Append("\tpublic const int " + GetVariableName(layerName) + "Id" + " = " + id + ";\n");
            }

            sb.Append("}\n");

#if !UNITY_WEBPLAYER
            File.WriteAllText(filePath, sb.ToString());
#endif
        }

        private static string GetVariableName(string str)
        {
            return str.Replace(" ", "");
        }
    }
}