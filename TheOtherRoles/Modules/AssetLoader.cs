using System.IO;
using System.Reflection;
using UnityEngine;

namespace TheOtherRoles.Modules
{
    public static class AssetLoader
    {
        private static readonly Assembly dll = Assembly.GetExecutingAssembly();
        private static bool flag = false;
        public static GameObject customServerMenu;
        public static void LoadAssets()
        {
            if (flag) return;
            flag = true;

#if PC
            var resourceAssetBundleStream = dll.GetManifestResourceStream("TheOtherRoles.Resources.Prefab.customServer_Win");
#else
            var resourceAssetBundleStream = dll.GetManifestResourceStream("TheOtherRoles.Resources.Prefab.customServer_Android");
#endif
            var assetBundleBundle = AssetBundle.LoadFromMemory(resourceAssetBundleStream.ReadFully());
            customServerMenu = assetBundleBundle.LoadAsset<GameObject>("CustomServer.prefab").DontUnload();
        }

        public static byte[] ReadFully(this Stream input)
        {
            using var ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }

        public static T LoadAsset<T>(this AssetBundle assetBundle, string name) where T : UnityEngine.Object
        {
            return assetBundle.LoadAsset(name, Il2CppType.Of<T>())?.Cast<T>();
        }

        public static T DontUnload<T>(this T obj) where T : Object
        {
            obj.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            return obj;
        }
    }


}
