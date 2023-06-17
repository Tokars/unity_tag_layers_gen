using System;
using System.Linq;
using OT.Extensions;
using UnityEditor;
using UnityEngine;
using static System.String;

namespace OT.Gen.Editor.Settings
{
    [CreateAssetMenu(menuName = "Scriptable/Tlgs/Settings File", fileName = "TLGSettings")]
    public sealed class TlgSettings : ScriptableObject
    {
        /// <summary>Where to create a new <see cref="TlgSettings" /> asset.</summary>
        private const string DefaultSettingsAssetPath = "Assets/Editor/";

        /// <summary>Settings for Tags.</summary>
        [SerializeField] internal Settings tag;

        /// <summary>Settings for Layers.</summary>
        [SerializeField] internal Settings layer;

        /// <summary>Settings for Sorting Layers.</summary>
        [SerializeField] internal Settings sortingLayer;

        /// <summary>Returns <see cref="InvalidOperationException" /> or creates a new one and saves the asset.</summary>
        /// <value>The <see cref="TlgSettings" /> to use.</value>
        /// <exception cref="TlgSettings">More than one <see cref="TlgSettings" /> are in the project.</exception>
        internal static TlgSettings GetOrCreateSettings
        {
            get
            {
                string[] guids = AssetDatabase.FindAssets($"t:{nameof(TlgSettings)}",
                    TlgSettingsDefaults.SearchInFolders);

                TlgSettings settings;

                switch (guids.Length)
                {
                    case 0:
                        CreateSettings(out settings);
                        break;
                    case 1:
                        LoadSettings(guids.Single(), out settings);
                        break;
                    default:
                        throw new InvalidOperationException(
                            $"There MUST be only one {nameof(TlgSettings)} asset in '{Application.productName}'.\n " +
                            $"Found: {Join(", ", guids.Select(AssetDatabase.GUIDToAssetPath))}.");
                }

                return settings;
            }
        }

        /// <summary>Reset to default values.</summary>
        private void Reset()
        {
            tag = TlgSettingsDefaults.Tag;
            layer = TlgSettingsDefaults.Layer;
            sortingLayer = TlgSettingsDefaults.SortingLayer;
        }

        /// <summary>Loads <see cref="GUID" /> via <see cref="GUID" />.</summary>
        /// <param name="guid">The <see cref="GUID" /> of the asset.</param>
        /// <param name="settings">The loaded <see cref="TlgSettings" />.</param>
        private static void LoadSettings(string guid, out TlgSettings settings)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            settings = AssetDatabase.LoadAssetAtPath<TlgSettings>(path);
        }

        /// <summary>Creates new <see cref="TlgSettings" /> and stores in the project.</summary>
        /// <param name="settings">The loaded <see cref="TlgSettings" />.</param>
        private static void CreateSettings(out TlgSettings settings)
        {
            settings = AssetDatabaseUtilities.CreateScriptable<TlgSettings>(DefaultSettingsAssetPath);
        }

        /// <summary>Returns <see cref="SerializedObject" /> wrapped in a <see cref="SerializedObject" />.</summary>
        /// <returns><see cref="TlgSettings" /> wrapped in a <see cref="TlgSettings" />.</returns>
        internal static SerializedObject GetSerializedSettings() => new SerializedObject(GetOrCreateSettings);

        /// <summary>Type generation settings.</summary>
        [Serializable]
        public sealed class Settings
        {
            /// <summary>The name of the type to generate.</summary>
            [SerializeField] internal string typeName;

            /// <summary>The path relative to the project's asset folder.</summary>
            [SerializeField] internal string filePath = "Assets/Scripts/Constants/Generated";

            /// <summary>Optional namespace to put the type in. Can be '<see langword="null" />' or empty..</summary>
            [SerializeField] internal string nameSpace = "Constants.Generated";
        }
    }
}