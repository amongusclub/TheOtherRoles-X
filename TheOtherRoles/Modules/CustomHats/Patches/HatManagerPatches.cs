using System.Collections.Generic;
using System.Linq;
using Cpp2IL.Core.Extensions;
using HarmonyLib;

namespace TheOtherRoles.Modules.CustomHats.Patches;

[HarmonyPatch(typeof(HatManager))]
internal static class HatManagerPatches
{
    private static bool isRunning;
    private static bool isLoaded;
    private static List<HatData> allHats;
        
    [HarmonyPatch(nameof(HatManager.GetHatById))]
    [HarmonyPrefix]
    private static void GetHatByIdPrefix(HatManager __instance)
    {
        if (isRunning || isLoaded) return;
        isRunning = true;
        // Maybe we can use lock keyword to ensure simultaneous list manipulations ?
        allHats = __instance.allHats.ToList();
        var cache = CustomHatManager.UnregisteredHats.Clone();
        foreach (var hat in cache)
        {
            try
            {
                allHats.Add(CustomHatManager.CreateHatBehaviour(hat));
                CustomHatManager.UnregisteredHats.Remove(hat);
            }
            catch
            {
                // This means the file has not been downloaded yet, do nothing...
            }
        }
        if (CustomHatManager.UnregisteredHats.Count == 0)
            isLoaded = true;
        cache.Clear();

        __instance.allHats = allHats.ToArray();
    }

    [HarmonyPatch(nameof(HatManager.GetHatById))]
    [HarmonyPostfix]
    private static void GetHatByIdPostfix()
    {
        isRunning = false;
    }

    [HarmonyPatch(nameof(HatManager.Initialize))]
    [HarmonyPostfix]
    private static void UnlockCosmetics(HatManager __instance)
    {
        foreach (BundleData bundleData in __instance.allBundles)
        {
            bundleData.Free = true;
        }
        foreach (BundleData bundleData2 in __instance.allFeaturedBundles)
        {
            bundleData2.Free = true;
        }
        foreach (CosmicubeData cosmicubeData in __instance.allFeaturedCubes)
        {
            cosmicubeData.Free = true;
        }
        foreach (CosmeticData cosmeticData in __instance.allFeaturedItems)
        {
            cosmeticData.Free = true;
        }
        foreach (HatData hatData in __instance.allHats)
        {
            hatData.Free = true;
        }
        foreach (NamePlateData namePlateData in __instance.allNamePlates)
        {
            namePlateData.Free = true;
        }
        foreach (PetData petData in __instance.allPets)
        {
            petData.Free = true;
        }
        foreach (SkinData skinData in __instance.allSkins)
        {
            skinData.Free = true;
        }
        foreach (StarBundle starBundle in __instance.allStarBundles)
        {
            starBundle.price = 0f;
        }
        foreach (VisorData visorData in __instance.allVisors)
        {
            visorData.Free = true;
        }
    }
}