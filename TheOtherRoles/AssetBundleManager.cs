﻿using System.Collections.Generic;
using System.Reflection;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace TheOtherRoles
{
    internal class AssetBundleManager
    {
        private static Dictionary<string, Sprite> buttonSprites = new();
        private static Dictionary<string, Texture2D> buttonTextures = new();

        public static void Load()
        {
            buttonSprites = new Dictionary<string, Sprite>();
            buttonTextures = new Dictionary<string, Texture2D>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resourceNames = assembly.GetManifestResourceNames();

#if PC
            var resourceBundle = assembly.GetManifestResourceStream("TheOtherRoles.Resources.torxresources_Win");
#else
            var resourceBundle = assembly.GetManifestResourceStream("TheOtherRoles.Resources.torxresources_Android");
#endif
            var assetBundle = AssetBundle.LoadFromMemory(resourceBundle.ReadFully());
            foreach (var f in assetBundle.GetAllAssetNames())
            {
                var texture = assetBundle.LoadAsset<Texture2D>(f).DontUnload();
                buttonTextures.Add(f, texture);
                buttonSprites.Add(f, Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f).DontUnload());
            }
            assetBundle.Unload(false);
        }

        public static Sprite get(string path, float pixelsPerUnit = 100f)
        {
            if (!path.Contains("assets")) path = "assets/resources/" + path.ToLower();

            if (pixelsPerUnit == 100f && buttonSprites.TryGetValue(path, out var sprite))
                return sprite;

            if (buttonTextures.TryGetValue(path, out var texture))
            {
                var newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
                newSprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;

                if (pixelsPerUnit == 100f)
                    buttonSprites[path] = newSprite;

                return newSprite;
            }

            return null;
        }
    }
}
