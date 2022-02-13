using UnityEditor;
using UnityEngine;

namespace OT.Gen.Editor
{
    public class TestCustomSettings : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("Test/Preferences")]
        static void Test()
        {
            var settings = TagLayersGenSettingsHandler.GetEditorSettings();
            Debug.Log("GenTags: " + settings.GenTags.ToString());
            Debug.Log("GenLayers: " + settings.GenLayers.ToString());
            Debug.Log("GenSortingLayers: " + settings.GenSortingLayers);
            Debug.Log("GenPath: " + settings.GenPath);
            Debug.Log("GenNamespace: " + settings.GenNamespace);
        }
#endif
    }
}