using HarmonyLib;
using TheOtherRoles.Patches;

namespace TheOtherRoles.Modules;

public class ModTranslation
{
    public static int defaultLanguage = (int)SupportedLangs.English;

    public static string getString(string key, string def = null)
    {
        return GeneratedTranslations.GetString(key, def);
    }
}

internal static class LanguageExtension
{
    internal static string Translate(this string key)
    {
        return ModTranslation.getString(key);
    }
}

[HarmonyPatch(typeof(LanguageSetter), nameof(LanguageSetter.SetLanguage))]
class SetLanguagePatch
{
    static void Postfix()
    {
        ClientOptionsPatch.updateTranslations();
    }
}