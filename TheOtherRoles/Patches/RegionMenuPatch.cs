// Adapted from https://github.com/MoltenMods/Unify
/*
MIT License

Copyright (c) 2021 Daemon

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using HarmonyLib;
using TheOtherRoles.Modules;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheOtherRoles.Patches {
    public class CustomServerMenu : MonoBehaviour
    {
        private GameObject customServerMenu;

        public void Start()
        {
            var customServerMenuPrefab = AssetLoader.customServerMenu;
            customServerMenu = Instantiate(customServerMenuPrefab, this.transform);
            customServerMenu.transform.localPosition = new Vector3(4.5f, -2.5f, -500f);
            customServerMenu.transform.localScale = Vector3.one;

            var IpField = CreateTextBoxTMP(customServerMenu.transform.GetChild(0).GetChild(0).GetChild(0).gameObject);
            IpField.text = TheOtherRolesPlugin.Ip.Value;
            var PortField = CreateTextBoxTMP(customServerMenu.transform.GetChild(0).GetChild(1).GetChild(0).gameObject);
            PortField.text = TheOtherRolesPlugin.Port.Value.ToString();

            var IpText = customServerMenu.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshPro>();
            IpText.text = ModTranslation.getString("titleIpText");
            var PortText = customServerMenu.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<TextMeshPro>();
            PortText.text = ModTranslation.getString("titlePortText");

            IpField.AllowEmail = true;
            IpField.allowAllCharacters = true;
            IpField.AllowSymbols = true;
            IpField.AllowPaste = true;
            PortField.allowAllCharacters = true;
            PortField.AllowSymbols = true;
            PortField.AllowPaste = true;

            IpField.OnEnter.AddListener((Action)(() => UpdateServer(IpField, PortField)));
            IpField.OnChange.AddListener((Action)(() => UpdateServer(IpField, PortField)));
            PortField.OnEnter.AddListener((Action)(() => UpdateServer(IpField, PortField)));
            PortField.OnChange.AddListener((Action)(() => UpdateServer(IpField, PortField)));
        }

        public static void UpdateServer(TextBoxTMP IpField, TextBoxTMP PortField)
        {
            TheOtherRolesPlugin.Ip.Value = IpField.text;
            ushort port = 0;
            if (ushort.TryParse(PortField.text, out port))
            {
                TheOtherRolesPlugin.Port.Value = port;
                PortField.outputText.color = Color.white;
            }
            else
            {
                PortField.outputText.color = Color.red;
            }
            TheOtherRolesPlugin.UpdateRegions();
        }

        private TextBoxTMP CreateTextBoxTMP(GameObject main)
        {
            var box = main.AddComponent<TextBoxTMP>();
            box.outputText = main.transform.Find("BoxLabel").GetComponent<TextMeshPro>();
            box.outputText.text = "";
            box.text = "";
            box.characterLimit = 30;
            box.sendButtonGlyph = null;
            box.Background = main.GetComponent<SpriteRenderer>();
            box.Pipe = main.transform.Find("Pipe").GetComponent<MeshRenderer>();
            box.colliders = new Il2CppReferenceArray<Collider2D>(new Collider2D[] { main.GetComponent<BoxCollider2D>() });
            box.OnEnter = new Button.ButtonClickedEvent();
            box.OnChange = new Button.ButtonClickedEvent();
            box.OnFocusLost = new Button.ButtonClickedEvent();
            box.tempTxt = new Il2CppSystem.Text.StringBuilder();
            main.MakePassiveButton(box.GiveFocus);
            return box;
        }

        private void OnDestroy()
        {
            if (customServerMenu != null) Destroy(customServerMenu);
        }
    }

    [HarmonyPatch(typeof(CreateGameOptions), nameof(CreateGameOptions.OpenServerDropdown))]
    public static class CreateGameOptionsOpenServerDropdownPatch
    {
        public static void Postfix(CreateGameOptions __instance)
        {
            __instance.serverDropdown.gameObject.AddComponent<CustomServerMenu>();
        }
    }
}
