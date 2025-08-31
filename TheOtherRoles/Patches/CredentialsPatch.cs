using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AmongUs.GameOptions;
using HarmonyLib;
using TheOtherRoles.CustomGameModes;
using TheOtherRoles.Modules;
using TheOtherRoles.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace TheOtherRoles.Patches
{
    [HarmonyPatch]
    public static class CredentialsPatch
    {
        public static string fullCredentialsVersion =
$@"<size=130%><color=#ff351f>TheOtherRoles</color>-<color=#02C3FE>X</color></size> v{TheOtherRolesPlugin.Version.ToString() + (TheOtherRolesPlugin.betaDays > 0 ? "-BETA" : "")}";

        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        internal static class PingTrackerPatch
        {

            static void Postfix(PingTracker __instance)
            {
                var position = __instance.GetComponent<AspectPosition>();
                if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started)
                {
                    string gameModeText = $"";
                    if (HideNSeek.isHideNSeekGM) gameModeText = ModTranslation.getString("pingTrackerGameModeTextHNS");
                    else if (HandleGuesser.isGuesserGm) gameModeText = ModTranslation.getString("pingTrackerGameModeTextGS");
                    else if (PropHunt.isPropHuntGM) gameModeText = ModTranslation.getString("pingTrackerGameModeTextPH");
                    if (gameModeText != "") gameModeText = Helpers.ColorString(Color.yellow, gameModeText) + (MeetingHud.Instance ? " " : "\n");
                    __instance.text.text = $"<size=130%><color=#ff351f>TheOtherRoles</color>-<color=#02C3FE>X</color></size> v{TheOtherRolesPlugin.Version.ToString() + (TheOtherRolesPlugin.betaDays > 0 ? "-BETA" : "")}\n{gameModeText}" + __instance.text.text;
                    __instance.text.alignment = TextAlignmentOptions.Top;
                    position.Alignment = AspectPosition.EdgeAlignments.Top;
                    position.DistanceFromEdge = new Vector3(1.5f, 0.11f, 0);
                }
                else
                {
                    string gameModeText = $"";
                    if (TORMapOptions.gameMode == CustomGamemodes.HideNSeek) gameModeText = ModTranslation.getString("pingTrackerGameModeTextHNS");
                    else if (TORMapOptions.gameMode == CustomGamemodes.Guesser) gameModeText = ModTranslation.getString("pingTrackerGameModeTextGS");
                    else if (TORMapOptions.gameMode == CustomGamemodes.PropHunt) gameModeText = ModTranslation.getString("pingTrackerGameModeTextPH");
                    if (gameModeText != "") gameModeText = Helpers.ColorString(Color.yellow, gameModeText);

                    __instance.text.text = $"{fullCredentialsVersion}\n{string.Format($"<size=60%>{ModTranslation.getString("fullCredentials")}</size>", "<color=#FCCE03FF>Fangkuai</color>", "<color=#FCCE03FF>TheOtherRoles</color>")}\n {__instance.text.text}";
                    position.Alignment = AspectPosition.EdgeAlignments.LeftTop;
                    __instance.text.alignment = TextAlignmentOptions.TopLeft;
                    position.DistanceFromEdge = new Vector3(0.5f, 0.11f);

                    try
                    {
                        var GameModeText = GameObject.Find("GameModeText")?.GetComponent<TextMeshPro>();
                        GameModeText.text = gameModeText == "" ? (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek ? "Van. HideNSeek" : "Classic") : gameModeText;
                        var ModeLabel = GameObject.Find("ModeLabel")?.GetComponentInChildren<TextMeshPro>();
                        ModeLabel.text = ModTranslation.getString("pingTrackerGameModeText");
                    }
                    catch { }
                }
            }
        }

        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
        public static class LogoPatch
        {
            public static SpriteRenderer renderer;
            public static Sprite bannerSprite;
            public static Sprite horseBannerSprite;
            public static Sprite banner2Sprite;
            private static PingTracker instance;

            public static GameObject motdObject;
            public static TextMeshPro motdText;

            static void Postfix(PingTracker __instance)
            {
                var torLogo = new GameObject("bannerLogo_TOR");
                torLogo.transform.SetParent(GameObject.Find("RightPanel").transform, false);
                torLogo.transform.localPosition = new Vector3(-0.4f, 1f, 5f);

                renderer = torLogo.AddComponent<SpriteRenderer>();
                loadSprites();
                instance = __instance;
                renderer.sprite = EventUtility.isEnabled ? banner2Sprite : bannerSprite;
                var credentialObject = new GameObject("credentialsTOR");
                var credentials = credentialObject.AddComponent<TextMeshPro>();
                credentials.SetText($"v{TheOtherRolesPlugin.Version.ToString() + (TheOtherRolesPlugin.betaDays > 0 ? "-BETA" : "")}\n<size=30f%>\n</size>{string.Format(ModTranslation.getString("fullCredentials"), "<color=#FCCE03FF>Fangkuai</color>", "<color=#FCCE03FF>TheOtherRoles</color>")}\n<size=30%>\n</size>{string.Format($"<size=60%> <color=#FCCE03FF>{ModTranslation.getString("contributorsCredentials")}</color></size>", "<color=#ff351f>All-Of-Us-Mods</color>")}");
                credentials.alignment = TMPro.TextAlignmentOptions.Center;
                credentials.fontSize *= 0.05f;

                credentials.transform.SetParent(torLogo.transform);
                credentials.transform.localPosition = Vector3.down * 1.25f;
                motdObject = new GameObject("torMOTD");
                motdText = motdObject.AddComponent<TextMeshPro>();
                motdText.alignment = TMPro.TextAlignmentOptions.Center;
                motdText.fontSize *= 0.04f;

                motdText.transform.SetParent(torLogo.transform);
                motdText.enableWordWrapping = true;
                var rect = motdText.gameObject.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(5.2f, 0.25f);

                motdText.transform.localPosition = Vector3.down * 2.25f;
                motdText.color = new Color(1, 53f / 255, 31f / 255);
                Material mat = motdText.fontSharedMaterial;
                mat.shaderKeywords = new string[] { "OUTLINE_ON" };
                motdText.SetOutlineColor(Color.white);
                motdText.SetOutlineThickness(0.025f);
            }

            public static void loadSprites()
            {
                if (bannerSprite == null) bannerSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.Banner.png", 300f);
                if (banner2Sprite == null) banner2Sprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.Banner2.png", 300f);
                if (horseBannerSprite == null) horseBannerSprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.bannerTheHorseRoles.png", 300f);
            }

            public static void updateSprite()
            {
                loadSprites();
                if (renderer != null)
                {
                    float fadeDuration = 1f;
                    instance.StartCoroutine(Effects.Lerp(fadeDuration, new Action<float>((p) => {
                        renderer.color = new Color(1, 1, 1, 1 - p);
                        if (p == 1)
                        {
                            renderer.sprite = TORMapOptions.enableHorseMode ? horseBannerSprite : bannerSprite;
                            instance.StartCoroutine(Effects.Lerp(fadeDuration, new Action<float>((p) => {
                                renderer.color = new Color(1, 1, 1, p);
                            })));
                        }
                    })));
                }
            }
        }

        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.LateUpdate))]
        public static class MOTD
        {
            public static List<string> motds = new List<string>();
            private static float timer = 0f;
            private static float maxTimer = 5f;
            private static int currentIndex = 0;

            public static void Postfix()
            {
                if (motds.Count == 0)
                {
                    timer = maxTimer;
                    return;
                }
                if (motds.Count > currentIndex && LogoPatch.motdText != null)
                    LogoPatch.motdText.SetText(motds[currentIndex]);
                else return;

                // fade in and out:
                float alpha = Mathf.Clamp01(Mathf.Min(new float[] { timer, maxTimer - timer }));
                if (motds.Count == 1) alpha = 1;
                LogoPatch.motdText.color = LogoPatch.motdText.color.SetAlpha(alpha);
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = maxTimer;
                    currentIndex = (currentIndex + 1) % motds.Count;
                }
            }

#if PC
            public static async Task loadMOTDs() {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://dlhk.fangkuai.fun/TheOtherRoles-X/motd.txt");
                response.EnsureSuccessStatusCode();
                string motds = await response.Content.ReadAsStringAsync();
                foreach(string line in motds.Split("\n", StringSplitOptions.RemoveEmptyEntries)) {
                        MOTD.motds.Add(line);
                }
            }
#else
            public static void loadMOTDs()
            {
                string url = "https://dlhk.fangkuai.fun/TheOtherRoles-X/motd.txt";
                var request = UnityWebRequest.Get(url);
                request.SendWebRequest();

                // Wait for the request to complete
                while (!request.isDone) { }

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    TheOtherRolesPlugin.Logger.LogError($"Couldn't fetch mod news from Server: {request.error}");
                    return;
                }

                string motdsText = request.downloadHandler.text;
                foreach (string line in motdsText.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                {
                    MOTD.motds.Add(line.Trim());
                }
            }
#endif
        }
    }
}
