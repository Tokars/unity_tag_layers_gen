using System.CodeDom.Compiler;
using System.Linq;
using UnityEngine;

namespace OT.Gen.Editor.Settings
{
    internal static class TlgSettingsValidator
    {
        /// <summary>Log errors about invalidate identifiers with this string.</summary>
        internal const string InvalidIdentifier = "'{0}' is not a valid identifier. See <a href=\"https://bit.ly/IdentifierNames\">https://bit.ly/IdentifierNames</a> for details.";

        
        internal static bool IsValidNamespace(string @namespace)
        {
            if (!string.IsNullOrWhiteSpace(@namespace) && @namespace.Split('.').All(CodeGenerator.IsValidLanguageIndependentIdentifier)) return true;
            Debug.LogErrorFormat(InvalidIdentifier, @namespace);
            return false;
        }
        
        /// <summary>Validates all the settings. <see cref="IsValidNamespace" /></summary>
        /// <param name="settings">The <see cref="TlgSettings.Settings" /> to validate.</param>
        /// <returns>True if all settings are valid.</returns>
        internal static bool ValidateAll(TlgSettings.Settings settings) =>
            IsValidNamespace(settings.nameSpace);
    }
}