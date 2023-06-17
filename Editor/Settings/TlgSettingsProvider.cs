using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace OT.Gen.Editor.Settings
{
    /// <summary>Settings provider for <see cref="TlgSettings" />.</summary>
    internal sealed class TlgSettingsProvider : SettingsProvider
    {
        /// <summary>Path to the Project Settings.</summary>
        internal const string ProjectSettingPath = TagsAndLayersProjectSettings + "/Generator";

        /// <summary>Path to the built-in Tags and Layers Manager.</summary>
        private const string TagsAndLayersProjectSettings = "Project/Tags and Layers";

        /// <summary><see cref="TlgSettings" /> wrapped in a <see cref="SerializedObject" />.</summary>
        private SerializedObject _settings;

        private TlgSettingsProvider(string path, SettingsScope scope) : base(path, scope)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _settings = TlgSettings.GetSerializedSettings();
        }

        public override void OnGUI(string searchContext)
        {
            PropertiesGUI(nameof(TlgSettings.tag));
            DrawGenBtn(TlsType.Tag);
            PropertiesGUI(nameof(TlgSettings.layer));
            DrawGenBtn(TlsType.Layer);
            PropertiesGUI(nameof(TlgSettings.sortingLayer));
            DrawGenBtn(TlsType.SortingLayer);

            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel, GUILayout.Height(24));
            EditorGUILayout.LabelField("Open", EditorStyles.boldLabel,GUILayout.Height(24));
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Tags and Layers",GUILayout.Height(24))) SettingsService.OpenProjectSettings(TagsAndLayersProjectSettings);
            if (GUILayout.Button("Settings Asset",GUILayout.Height(24)))
                Selection.SetActiveObjectWithContext(_settings.targetObject, _settings.context);
            EditorGUILayout.EndHorizontal();

            _settings.ApplyModifiedPropertiesWithoutUndo();
        }

        private void DrawGenBtn(TlsType type)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(12);
            
            GUILayout.FlexibleSpace();
            if (GUILayout.Button($"Generate {type}s",
                    GUILayout.Width(220), GUILayout.Height(24)))
            {
                switch (type)
                {
                    case TlsType.Tag:
                        TlGenerator.GenClass(TlgSettings.GetOrCreateSettings.tag.filePath,
                            TlgSettings.GetOrCreateSettings.tag.nameSpace, TlgSettings.GetOrCreateSettings.tag.typeName,
                            type);
                        break;
                    case TlsType.Layer:
                        TlGenerator.GenClass(TlgSettings.GetOrCreateSettings.layer.filePath,
                            TlgSettings.GetOrCreateSettings.layer.nameSpace,
                            TlgSettings.GetOrCreateSettings.layer.typeName, type);
                        break;
                    case TlsType.SortingLayer:
                        TlGenerator.GenClass(TlgSettings.GetOrCreateSettings.sortingLayer.filePath,
                            TlgSettings.GetOrCreateSettings.sortingLayer.nameSpace,
                            TlgSettings.GetOrCreateSettings.sortingLayer.typeName, type);
                        break;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void PropertiesGUI(string property)
        {
            EditorGUILayout.LabelField(property, EditorStyles.largeLabel);
            EditorGUILayout.Space(12);
            GUI.enabled = false;

            EditorGUILayout.DelayedTextField(
                _settings.FindProperty($"{property}.{nameof(TlgSettings.Settings.typeName)}"), Styles.TypeName);
            EditorGUILayout.DelayedTextField(
                _settings.FindProperty($"{property}.{nameof(TlgSettings.Settings.filePath)}"), Styles.FilePath);
            EditorGUILayout.DelayedTextField(
                _settings.FindProperty($"{property}.{nameof(TlgSettings.Settings.nameSpace)}"), Styles.Namespace);
            EditorGUILayout.Space();
            GUI.enabled = true;
        }

        /// <summary>Creates the <see cref="SettingsProvider" /> for the Project Settings window.</summary>
        /// <returns>The <see cref="SettingsProvider" /> for the Project Settings window.</returns>
        [SettingsProvider]
        private static SettingsProvider CreateTagClassGeneratorSettingsProvider() =>
            new TlgSettingsProvider(ProjectSettingPath, SettingsScope.Project)
                {keywords = GetSearchKeywordsFromGUIContentProperties<Styles>()};

        /// <summary>Styles for the <see cref="SettingsProvider" />.</summary>
        private readonly struct Styles
        {
            public static readonly GUIContent TypeName = new GUIContent("Type Name");
            public static readonly GUIContent FilePath = new GUIContent("File Path");
            public static readonly GUIContent Namespace = new GUIContent("Namespace");
        }
    }
}