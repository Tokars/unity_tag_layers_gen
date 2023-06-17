
namespace OT.Gen.Editor.Settings
{
    /// <summary>Holds the defaults values for the <see cref="TlgSettings" />.</summary>
    internal static class TlgSettingsDefaults
    {
        /// <summary>Default value for <see cref="TlgSettings.tag" /> <see cref="TlgSettings.Settings.typeName" />.</summary>
        private const string DefaultTagTypeName = "Tags";

        /// <summary>Default value for <see cref="TlgSettings.layer" /> <see cref="TlgSettings.Settings.typeName" />.</summary>
        private const string DefaultLayerTypeName = "Layers";

        /// <summary>Default value for <see cref="TlgSettings.sortingLayer" /> <see cref="TlgSettings.Settings.typeName" />.</summary>
        private const string DefaultSortingLayerTypeName = "SortLayers";
        private const string DefaultClassPath = "/Scripts/Constants/Generated";
        private const string DefaultNamespace = "Constants.Generated";

        /// <summary>Where to start the asset search for settings.</summary>
        internal static readonly string[] SearchInFolders = {"Assets"};

        /// <summary>Default settings for <see cref="TlgSettings.tag" />.</summary>
        internal static readonly TlgSettings.Settings Tag = new TlgSettings.Settings
        {
            typeName = DefaultTagTypeName,
            filePath = DefaultClassPath,
            nameSpace = DefaultNamespace
        };

        /// <summary>Default settings for <see cref="TlgSettings.layer" />.</summary>
        internal static readonly TlgSettings.Settings Layer = new TlgSettings.Settings
        {
            typeName = DefaultLayerTypeName,
            filePath = DefaultClassPath,
            nameSpace = DefaultNamespace
        };

        /// <summary>Default settings for <see cref="TlgSettings.layer" />.</summary>
        internal static readonly TlgSettings.Settings SortingLayer = new TlgSettings.Settings
        {
            typeName = DefaultSortingLayerTypeName,
            filePath = DefaultClassPath,
            nameSpace = DefaultNamespace
        };
    }
}