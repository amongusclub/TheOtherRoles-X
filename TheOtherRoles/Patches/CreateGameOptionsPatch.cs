using HarmonyLib;
using Reactor.Utilities.Extensions;
using System;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Patches;

internal class CreateGameOptionsPatch
{
    public static PassiveButton modeButtonGS;
    public static PassiveButton modeButtonHK;
    public static PassiveButton modeButtonPH;

    public static bool One = true;

    [HarmonyPatch(typeof(CreateGameOptions), nameof(CreateGameOptions.Show))]
    static class CreateGameOptionsOpenShowPatch
    {
        static void Postfix(CreateGameOptions __instance)
        {
            if ((modeButtonGS != null && modeButtonGS.IsSelected()) ||
                (modeButtonHK != null && modeButtonHK.IsSelected()) ||
                (modeButtonPH != null && modeButtonPH.IsSelected()))
            {
                __instance.modeButtons[0].SelectButton(false);
                __instance.modeButtons[1].SelectButton(false);
            }
        }
    }

    [HarmonyPatch(typeof(CreateGameOptions), nameof(CreateGameOptions.Start))]
    public static class CreateGameOptionsStartPatch
    {
        private static void Postfix(CreateGameOptions __instance)
        {
            __instance.levelButtons[0].transform.parent.gameObject.SetActive(false);
            GameObject.Find("ModeOptions").transform.SetLocalY(-2.52f);
            GameObject.Find("ServerOption").transform.SetLocalY(-0.86f);
            __instance.serverDropdown.transform.SetLocalY(-0.6f);

            __instance.modeButtons[0].OnClick.AddListener((Action)(() =>
            {
                modeButtonGS.SelectButton(false);
                modeButtonHK.SelectButton(false);
                modeButtonPH.SelectButton(false);
            }
            ));

            __instance.modeButtons[1].OnClick.AddListener((Action)(() =>
            {
                modeButtonGS.SelectButton(false);
                modeButtonHK.SelectButton(false);
                modeButtonPH.SelectButton(false);
            }
            ));

            modeButtonGS = Object.Instantiate(__instance.modeButtons[0], __instance.modeButtons[0].transform);
            modeButtonGS.name = "TORGUESSER";
            changeButtonText(modeButtonGS, "TOR Guesser");
            modeButtonGS.transform.SetLocalX(5.86f);
            modeButtonGS.OnClick.RemoveAllListeners();
            __instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>(p => modeButtonGS.SelectButton(false))));
            modeButtonGS.OnMouseOver.AddListener((Action)(() => __instance.tooltip.SetText("An extension to the Classic-Gamemode and gives you a multitude of new options for Guessers.")));
            modeButtonGS.OnClick.AddListener((Action)(() =>
            {
                TORMapOptions.gameMode = CustomGamemodes.Guesser;
                modeButtonGS.SelectButton(true);
                __instance.modeButtons[0].SelectButton(false);
                __instance.modeButtons[1].SelectButton(false);
                modeButtonHK.SelectButton(false);
                modeButtonPH.SelectButton(false);
            }
            ));

            modeButtonHK = Object.Instantiate(modeButtonGS, __instance.modeButtons[0].transform);
            modeButtonHK.name = "TORHIDENSEEK";
            changeButtonText(modeButtonHK, "TOR Hide N Seek");
            modeButtonHK.transform.SetLocalX(0);
            modeButtonHK.transform.SetLocalY(-0.9f);
            modeButtonHK.OnClick.RemoveAllListeners();
            __instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>(p => modeButtonHK.SelectButton(false))));
            modeButtonHK.OnMouseOver.AddListener((Action)(() => __instance.tooltip.SetText("A standalone Gamemode where Hunter have to catch their prey (\"Hunted\" players).")));
            modeButtonHK.OnClick.AddListener((Action)(() =>
            {
                TORMapOptions.gameMode = CustomGamemodes.HideNSeek;
                modeButtonHK.SelectButton(true);
                __instance.modeButtons[0].SelectButton(false);
                __instance.modeButtons[1].SelectButton(false);
                modeButtonGS.SelectButton(false);
                modeButtonPH.SelectButton(false);
            }
            ));

            modeButtonPH = Object.Instantiate(modeButtonHK, __instance.modeButtons[0].transform);
            modeButtonPH.name = "TORPROPHUNT";
            changeButtonText(modeButtonPH, "TOR Prop Hunt");
            modeButtonPH.transform.SetLocalX(2.91f);
            modeButtonPH.OnClick.RemoveAllListeners();
            __instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>(p => modeButtonPH.SelectButton(false))));
            modeButtonPH.OnMouseOver.AddListener((Action)(() => __instance.tooltip.SetText("A standalone Gamemode where Hunters have to find the disguised players (\"Props\").")));
            modeButtonPH.OnClick.AddListener((Action)(() =>
            {
                TORMapOptions.gameMode = CustomGamemodes.PropHunt;
                modeButtonPH.SelectButton(true);
                __instance.modeButtons[0].SelectButton(false);
                __instance.modeButtons[1].SelectButton(false);
                modeButtonGS.SelectButton(false);
                modeButtonHK.SelectButton(false);
            }
            ));
        }
        internal static void changeButtonText(PassiveButton passiveButton, string buttonText)
        {
            passiveButton.transform.FindChild("SelectedInactive/ClassicText").gameObject.GetComponentInChildren<TextTranslatorTMP>().Destroy();
            passiveButton.transform.FindChild("Inactive/ClassicText").gameObject.GetComponentInChildren<TextTranslatorTMP>().Destroy();
            passiveButton.transform.FindChild("Highlight/ClassicText").gameObject.GetComponentInChildren<TextTranslatorTMP>().Destroy();
            passiveButton.transform.FindChild("SelectedHighlight/ClassicText").gameObject.GetComponentInChildren<TextTranslatorTMP>().Destroy();

            passiveButton.transform.FindChild("SelectedInactive/ClassicText").gameObject.GetComponentInChildren<TMP_Text>().SetText(buttonText);
            passiveButton.transform.FindChild("Inactive/ClassicText").gameObject.GetComponentInChildren<TMP_Text>().SetText(buttonText);
            passiveButton.transform.FindChild("Highlight/ClassicText").gameObject.GetComponentInChildren<TMP_Text>().SetText(buttonText);
            passiveButton.transform.FindChild("SelectedHighlight/ClassicText").gameObject.GetComponentInChildren<TMP_Text>().SetText(buttonText);
        }
    }
    [HarmonyPatch(typeof(CreateGameOptions), nameof(CreateGameOptions.OpenConfirmPopup))]
    static class CreateGameOptionsOpenConfirmPopupPatch
    {
        static string getModeText(CustomGamemodes customGamemodes)
        {
            switch (customGamemodes)
            {
                case CustomGamemodes.Classic:
                    return DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameTypeClassic);
                case CustomGamemodes.Guesser:
                    return "TOR Guesser";
                case CustomGamemodes.HideNSeek:
                    return "TOR Hide N Seek";
                case CustomGamemodes.PropHunt:
                    return "TOR Prop Hunt";
                default:
                    return DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameTypeHideAndSeek);
            }
        }

        static void Postfix(CreateGameOptions __instance)
        {
            __instance.containerConfirm.GetChild(10).gameObject.SetActive(false);
            __instance.containerConfirm.GetChild(8).localPosition = new(4f, -0.47f, -0.1f);
            __instance.containerConfirm.GetChild(5).GetChild(2).GetComponent<TextMeshPro>().SetText(getModeText(TORMapOptions.gameMode));
        }
    }
}