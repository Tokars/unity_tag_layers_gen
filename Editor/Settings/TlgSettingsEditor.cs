using UnityEditor;
using UnityEngine;

namespace OT.Gen.Editor.Settings
{
    /// <summary>Custom inspector for <see cref="TlgSettings" />.</summary>
    [CustomEditor(typeof(TlgSettings))]
    internal sealed class TlgSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.LabelField("Open", EditorStyles.boldLabel);
            if (GUILayout.Button("Project Settings")) SettingsService.OpenProjectSettings(TlgSettingsProvider.ProjectSettingPath);
            if (GUILayout.Button("Tags and Layers")) SettingsService.OpenProjectSettings("Project/Tags and Layers");
        }
    }
}