using HarmonyLib;
using Hazel;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TheOtherRoles.Modules;
using TheOtherRoles.Utilities;
using TMPro;
using UnityEngine;

namespace TheOtherRoles.Patches
{
    public class GameStartManagerPatch
    {
        public static Dictionary<int, PlayerVersion> playerVersions = new Dictionary<int, PlayerVersion>();
        public static float timer = 600f;
        private static float kickingTimer = 0f;
        private static bool versionSent = false;
        private static string lobbyCodeText = "";

        // [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))] NOT Run in Android(Android user join in PC Room)
        [HarmonyPatch(typeof(PlayerPhysics._CoSpawnPlayer_d__42), nameof(PlayerPhysics._CoSpawnPlayer_d__42.MoveNext))]
        public class AmongUsClientCreatePlayerPatch
        {
            public static void Postfix(PlayerPhysics._CoSpawnPlayer_d__42 __instance)
            {
                if (PlayerControl.LocalPlayer != null)
                {
                    Helpers.shareGameVersion();
                }
                GameStartManagerUpdatePatch.sendGamemode = true;
            }
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
        public class GameStartManagerStartPatch
        {
            public static void Postfix(GameStartManager __instance)
            {
                // Trigger version refresh
                versionSent = false;
                // Reset lobby countdown timer
                timer = 600f;
                // Reset kicking timer
                kickingTimer = 0f;
                // Copy lobby code
                string code = InnerNet.GameCode.IntToGameName(AmongUsClient.Instance.GameId);
                GUIUtility.systemCopyBuffer = code;
                lobbyCodeText = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RoomCode, new Il2CppReferenceArray<Il2CppSystem.Object>(0)) + "\r\n" + code;
            }
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
        public class GameStartManagerUpdatePatch
        {
            public static float startingTimer = 0;
            private static bool update = false;
            private static string currentText = "";
            private static GameObject copiedStartButton;
            public static bool sendGamemode = true;

            public static void Prefix(GameStartManager __instance)
            {
                if (!GameData.Instance) return; // No instance
                __instance.MinPlayers = 1;
                update = GameData.Instance.PlayerCount != __instance.LastPlayerCount;
            }

            public static void Postfix(GameStartManager __instance)
            {
                // Send version as soon as PlayerControl.LocalPlayer exists
                if (PlayerControl.LocalPlayer != null && !versionSent)
                {
                    versionSent = true;
                    Helpers.shareGameVersion();
                }

                // Check version handshake infos
                var versionMismatch = false;
                var message = "";
                foreach (var client in AmongUsClient.Instance.allClients.ToArray())
                {
                    if (client.Character == null) continue;
                    if (!playerVersions.ContainsKey(client.Id))
                    {
                        versionMismatch = true;
                        message +=
                            string.Format("gameStartTextDiferentVersion".Translate(), client.Character.Data.PlayerName);
                    }
                    else
                    {
                        var PV = playerVersions[client.Id];
                        var diff = TheOtherRolesPlugin.Version.CompareTo(PV.version);
                        if (diff > 0)
                        {
                            message +=
                                string.Format("gameStartTextOldVersion".Translate(), client.Character.Data.PlayerName, playerVersions[client.Id].version.ToString());
                            versionMismatch = true;
                        }
                        else if (diff < 0)
                        {
                            message +=
                                string.Format("gameStartTextNewVersion".Translate(), client.Character.Data.PlayerName, playerVersions[client.Id].version.ToString());
                            versionMismatch = true;
                        }
                        /*else if (!PV.GuidMatches())
                        {
                            // version presumably matches, check if Guid matches
                            message +=
                                string.Format("gameStartTextModVersion".Translate(), client.Character.Data.PlayerName, playerVersions[client.Id].version.ToString(), PV.guid.ToString());
                            versionMismatch = true;
                        }*/
                    }
                }


                // Display message to the host
                if (AmongUsClient.Instance.AmHost)
                {
                    if (versionMismatch)
                    {
                        __instance.GameStartText.text = message;
                        __instance.GameStartText.transform.localPosition =
                            __instance.StartButton.transform.localPosition + Vector3.up * 5;
                        __instance.GameStartText.transform.localScale = new Vector3(2f, 2f, 1f);
                        __instance.GameStartTextParent.SetActive(true);
                    }
                    else
                    {
                        __instance.GameStartText.transform.localPosition = Vector3.zero;
                        __instance.GameStartText.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
                        if (!__instance.GameStartText.text.Contains(FastDestroyableSingleton<TranslationController>.Instance
                                .GetString(StringNames.GameStarting).Replace("{0}", "")))
                        {
                            __instance.GameStartText.text = string.Empty;
                            __instance.GameStartTextParent.SetActive(false);
                        }
                    }

                    if (__instance.startState != GameStartManager.StartingStates.Countdown)
                        copiedStartButton?.Destroy();
                    // Make starting info available to clients:
                    if (startingTimer <= 0 && __instance.startState == GameStartManager.StartingStates.Countdown)
                    {
                        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                            (byte)CustomRPC.SetGameStarting, SendOption.Reliable);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.setGameStarting();
                        // Activate Stop-Button
                        copiedStartButton = GameObject.Instantiate(__instance.StartButton.gameObject,
                            __instance.StartButton.gameObject.transform.parent);
                        copiedStartButton.transform.localPosition = __instance.StartButton.transform.localPosition;
                        copiedStartButton.SetActive(true);
                        var startButtonText = copiedStartButton.GetComponentInChildren<TextMeshPro>();
                        startButtonText.text = "";
                        startButtonText.fontSize *= 0.8f;
                        startButtonText.fontSizeMax = startButtonText.fontSize;
                        startButtonText.gameObject.transform.localPosition = Vector3.zero;
                        var startButtonPassiveButton = copiedStartButton.GetComponent<PassiveButton>();

                        void StopStartFunc()
                        {
                            __instance.ResetStartState();
                            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                                (byte)CustomRPC.StopStart, SendOption.Reliable);
                            writer.Write(PlayerControl.LocalPlayer.PlayerId);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            copiedStartButton.Destroy();
                            startingTimer = 0;
                            SoundManager.Instance.StopSound(GameStartManager.Instance.gameStartSound);
                        }

                        startButtonPassiveButton.OnClick.AddListener((Action)(() => StopStartFunc()));
                        __instance.StartCoroutine(Effects.Lerp(.1f,
                            new Action<float>(p => { startButtonText.text = ""; })));
                    }
                }

                // Client update with handshake infos
                else
                {
                    if (!playerVersions.ContainsKey(AmongUsClient.Instance.HostId) ||
                        TheOtherRolesPlugin.Version.CompareTo(playerVersions[AmongUsClient.Instance.HostId].version) != 0)
                    {
                        kickingTimer += Time.deltaTime;
                        if (kickingTimer > 10)
                        {
                            kickingTimer = 0;
                            AmongUsClient.Instance.ExitGame(DisconnectReasons.ExitGame);
                            SceneChanger.ChangeScene("MainMenu");
                        }

                        __instance.GameStartText.text =
                            string.Format("gameStartHasDifferentVersion".Translate(), Math.Round(10 - kickingTimer));
                        __instance.GameStartText.transform.localPosition =
                            __instance.StartButton.transform.localPosition + Vector3.up * 5;
                        __instance.GameStartText.transform.localScale = new Vector3(2f, 2f, 1f);
                        __instance.GameStartTextParent.SetActive(true);
                    }
                    else if (versionMismatch)
                    {
                        __instance.GameStartText.text =
                            "gameStartDifferentVersionPlayer".Translate() + message;
                        __instance.GameStartText.transform.localPosition =
                            __instance.StartButton.transform.localPosition + Vector3.up * 5;
                        __instance.GameStartText.transform.localScale = new Vector3(2f, 2f, 1f);
                        __instance.GameStartTextParent.SetActive(true);
                    }
                    else
                    {
                        __instance.GameStartText.transform.localPosition = Vector3.zero;
                        __instance.GameStartText.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
                        if (!__instance.GameStartText.text.Contains(FastDestroyableSingleton<TranslationController>.Instance
                                .GetString(StringNames.GameStarting).Replace("{0}", "")))
                        {
                            __instance.GameStartText.text = string.Empty;
                            __instance.GameStartTextParent.SetActive(false);
                        }
                    }

                    if (!__instance.GameStartText.text.Contains(FastDestroyableSingleton<TranslationController>.Instance
                            .GetString(StringNames.GameStarting).Replace("{0}", "")) ||
                        !CustomOptionHolder.anyPlayerCanStopStart.getBool())
                        copiedStartButton?.Destroy();
                    if (CustomOptionHolder.anyPlayerCanStopStart.getBool() && copiedStartButton == null &&
                        __instance.GameStartText.text.Contains(FastDestroyableSingleton<TranslationController>.Instance
                            .GetString(StringNames.GameStarting).Replace("{0}", "")))
                    {
                        // Activate Stop-Button
                        copiedStartButton = GameObject.Instantiate(__instance.StartButton.gameObject,
                            __instance.StartButton.gameObject.transform.parent);
                        copiedStartButton.transform.localPosition = __instance.StartButton.transform.localPosition;
                        copiedStartButton.SetActive(true);
                        var startButtonText = copiedStartButton.GetComponentInChildren<TextMeshPro>();
                        startButtonText.text = "";
                        startButtonText.fontSize *= 0.8f;
                        startButtonText.fontSizeMax = startButtonText.fontSize;
                        startButtonText.gameObject.transform.localPosition = Vector3.zero;
                        var startButtonPassiveButton = copiedStartButton.GetComponent<PassiveButton>();

                        void StopStartFunc()
                        {
                            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                                (byte)CustomRPC.StopStart, SendOption.Reliable);
                            writer.Write(PlayerControl.LocalPlayer.PlayerId);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            copiedStartButton.Destroy();
                            __instance.GameStartText.text = string.Empty;
                            startingTimer = 0;
                            SoundManager.Instance.StopSound(GameStartManager.Instance.gameStartSound);
                        }

                        startButtonPassiveButton.OnClick.AddListener((Action)(() => StopStartFunc()));
                        __instance.StartCoroutine(Effects.Lerp(.1f,
                            new Action<float>(p => { startButtonText.text = ""; })));
                    }
                }

                // Start Timer
                if (startingTimer > 0) startingTimer -= Time.deltaTime;
                // Lobby timer
                if (!GameData.Instance || !__instance.PlayerCounter) return; // No instance

                if (update) currentText = __instance.PlayerCounter.text;

                timer = Mathf.Max(0f, timer -= Time.deltaTime);
                var minutes = (int)timer / 60;
                var seconds = (int)timer % 60;
                var suffix = $" ({minutes:00}:{seconds:00})";

                if (!AmongUsClient.Instance) return;

                if (AmongUsClient.Instance.AmHost && sendGamemode && PlayerControl.LocalPlayer != null)
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.ShareGamemode, SendOption.Reliable);
                    writer.Write((byte)TORMapOptions.gameMode);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    try
                    { RPCProcedure.shareGamemode((byte)TORMapOptions.gameMode); }
                    catch { }
                    sendGamemode = false;
                }
            }
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.BeginGame))]
        public class GameStartManagerBeginGame
        {
            public static bool Prefix(GameStartManager __instance)
            {
                // Block game start if not everyone has the same mod version
                bool continueStart = true;

                if (AmongUsClient.Instance.AmHost)
                {
                    foreach (InnerNet.ClientData client in AmongUsClient.Instance.allClients.GetFastEnumerator())
                    {
                        if (client.Character == null) continue;
                        var dummyComponent = client.Character.GetComponent<DummyBehaviour>();
                        if (dummyComponent != null && dummyComponent.enabled)
                            continue;

                        if (!playerVersions.ContainsKey(client.Id))
                        {
                            continueStart = false;
                            break;
                        }

                        PlayerVersion PV = playerVersions[client.Id];
                        int diff = TheOtherRolesPlugin.Version.CompareTo(PV.version);
                        if (diff != 0/* || !PV.GuidMatches()*/)
                        {
                            continueStart = false;
                            break;
                        }
                    }
                    if (continueStart && (TORMapOptions.gameMode == CustomGamemodes.HideNSeek || TORMapOptions.gameMode == CustomGamemodes.PropHunt) && GameOptionsManager.Instance.CurrentGameOptions.MapId != 6)
                    {
                        byte mapId = 0;
                        if (TORMapOptions.gameMode == CustomGamemodes.HideNSeek) mapId = (byte)CustomOptionHolder.hideNSeekMap.getSelection();
                        else if (TORMapOptions.gameMode == CustomGamemodes.PropHunt) mapId = (byte)CustomOptionHolder.propHuntMap.getSelection();
                        if (mapId >= 3) mapId++;
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DynamicMapOption, Hazel.SendOption.Reliable, -1);
                        writer.Write(mapId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.dynamicMapOption(mapId);
                    }
                    else if (CustomOptionHolder.dynamicMap.getBool() && continueStart)
                    {
                        // 0 = Skeld
                        // 1 = Mira HQ
                        // 2 = Polus
                        // 3 = Dleks - deactivated
                        // 4 = Airship
                        // 5 = Submerged
                        byte chosenMapId = 0;
                        List<float> probabilities =
                        [
                            CustomOptionHolder.dynamicMapEnableSkeld.getSelection() / 10f,
                            CustomOptionHolder.dynamicMapEnableMira.getSelection() / 10f,
                            CustomOptionHolder.dynamicMapEnablePolus.getSelection() / 10f,
                            CustomOptionHolder.dynamicMapEnableAirShip.getSelection() / 10f,
                            CustomOptionHolder.dynamicMapEnableFungle.getSelection() / 10f,
                            CustomOptionHolder.dynamicMapEnableSubmerged.getSelection() / 10f,
                        ];

                        // if any map is at 100%, remove all maps that are not!
                        if (probabilities.Contains(1.0f))
                        {
                            for (int i = 0; i < probabilities.Count; i++)
                            {
                                if (probabilities[i] != 1.0) probabilities[i] = 0;
                            }
                        }

                        float sum = probabilities.Sum();
                        if (sum == 0) return continueStart;  // All maps set to 0, why are you doing this???
                        for (int i = 0; i < probabilities.Count; i++)
                        {  // Normalize to [0,1]
                            probabilities[i] /= sum;
                        }
                        float selection = (float)TheOtherRoles.rnd.NextDouble();
                        float cumsum = 0;
                        for (byte i = 0; i < probabilities.Count; i++)
                        {
                            cumsum += probabilities[i];
                            if (cumsum > selection)
                            {
                                chosenMapId = i;
                                break;
                            }
                        }

                        // Translate chosen map to presets page and use that maps random map preset page
                        if (CustomOptionHolder.dynamicMapSeparateSettings.getBool())
                        {
                            CustomOptionHolder.presetSelection.updateSelection(chosenMapId + 2);
                        }
                        if (chosenMapId >= 3) chosenMapId++;  // Skip dlekS

                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DynamicMapOption, Hazel.SendOption.Reliable, -1);
                        writer.Write(chosenMapId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.dynamicMapOption(chosenMapId);
                    }
                }
                return continueStart;
            }
        }

        public class PlayerVersion
        {
            public readonly Version version;
            public readonly Guid guid;

            public PlayerVersion(Version version, Guid guid)
            {
                this.version = version;
                this.guid = guid;
            }

            /*public bool GuidMatches()
            {
                return Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId.Equals(this.guid);
            }*/
        }
    }
}