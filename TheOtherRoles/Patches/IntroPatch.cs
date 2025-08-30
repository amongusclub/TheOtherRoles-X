using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using HarmonyLib;
using Hazel;
using PowerTools;
using TheOtherRoles.CustomGameModes;
using TheOtherRoles.Modules;
using TheOtherRoles.Utilities;
using UnityEngine;
using static TheOtherRoles.TheOtherRoles;

namespace TheOtherRoles.Patches {
#if PC
        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
#endif
#if ANDROID
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.CoBegin))]
#endif
    class IntroCutsceneOnDestroyPatch
    {
        public static PoolablePlayer playerPrefab;
        public static Vector3 bottomLeft;
        public static void Prefix(IntroCutscene __instance) {
            // Generate and initialize player icons
            int playerCounter = 0;
            int hideNSeekCounter = 0;
            if (PlayerControl.LocalPlayer != null && FastDestroyableSingleton<HudManager>.Instance != null) {
                float aspect = Camera.main.aspect;
                float safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(Camera.main);
                float xpos = 1.75f - safeOrthographicSize * aspect * 1.70f;
                float ypos = 0.15f - safeOrthographicSize * 1.7f;
                bottomLeft = new Vector3(xpos / 2, ypos/2, -61f);

                foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                    NetworkedPlayerInfo data = p.Data;
                    PoolablePlayer player = UnityEngine.Object.Instantiate<PoolablePlayer>(__instance.PlayerPrefab, FastDestroyableSingleton<HudManager>.Instance.transform);
                    playerPrefab = __instance.PlayerPrefab;
                    p.SetPlayerMaterialColors(player.cosmetics.currentBodySprite.BodySprite);
                    player.SetSkin(data.DefaultOutfit.SkinId, data.DefaultOutfit.ColorId);
                    player.cosmetics.SetHat(data.DefaultOutfit.HatId, data.DefaultOutfit.ColorId);
                   // PlayerControl.SetPetImage(data.DefaultOutfit.PetId, data.DefaultOutfit.ColorId, player.PetSlot);
                    player.cosmetics.nameText.text = data.PlayerName;
                    player.SetFlipX(true);
                    TORMapOptions.playerIcons[p.PlayerId] = player;
                    player.gameObject.SetActive(false);

                    if (PlayerControl.LocalPlayer == Arsonist.arsonist && p != Arsonist.arsonist) {
                        player.transform.localPosition = bottomLeft + new Vector3(-0.25f, -0.25f, 0) + Vector3.right * playerCounter++ * 0.35f;
                        player.transform.localScale = Vector3.one * 0.2f;
                        player.setSemiTransparent(true);
                        player.gameObject.SetActive(true);
                    } else if (HideNSeek.isHideNSeekGM) {
                        if (HideNSeek.isHunted() && p.Data.Role.IsImpostor) {
                            player.transform.localPosition = bottomLeft + new Vector3(-0.25f, 0.4f, 0) + Vector3.right * playerCounter++ * 0.6f;
                            player.transform.localScale = Vector3.one * 0.3f;
                            player.cosmetics.nameText.text += $"{Helpers.cs(Color.red, " (Hunter)")}";
                            player.gameObject.SetActive(true);
                        } else if (!p.Data.Role.IsImpostor) {
                            player.transform.localPosition = bottomLeft + new Vector3(-0.35f, -0.25f, 0) + Vector3.right * hideNSeekCounter++ * 0.35f;
                            player.transform.localScale = Vector3.one * 0.2f;
                            player.setSemiTransparent(true);
                            player.gameObject.SetActive(true);
                        }

                    } else if (PropHunt.isPropHuntGM) {
                        player.transform.localPosition = bottomLeft + new Vector3(-1.25f, -0.1f, 0) + Vector3.right * hideNSeekCounter++ * 0.4f;
                        player.transform.localScale = Vector3.one * 0.24f;
                        player.setSemiTransparent(false);
                        player.cosmetics.nameText.transform.localPosition += Vector3.up * 0.2f * (hideNSeekCounter % 2 == 0 ? 1 : -1);
                        player.SetFlipX(false);
                        player.gameObject.SetActive(true);
                    } else {   //  This can be done for all players not just for the bounty hunter as it was before. Allows the thief to have the correct position and scaling
                        player.transform.localPosition = bottomLeft;
                        player.transform.localScale = Vector3.one * 0.4f;
                        player.gameObject.SetActive(false);
                    }
                }
            }

            // Force Bounty Hunter to load a new Bounty when the Intro is over
            if (BountyHunter.bounty != null && PlayerControl.LocalPlayer == BountyHunter.bountyHunter) {
                BountyHunter.bountyUpdateTimer = 0f;
                if (FastDestroyableSingleton<HudManager>.Instance != null) {
                    BountyHunter.cooldownText = UnityEngine.Object.Instantiate<TMPro.TextMeshPro>(FastDestroyableSingleton<HudManager>.Instance.KillButton.cooldownTimerText, FastDestroyableSingleton<HudManager>.Instance.transform);
                    BountyHunter.cooldownText.alignment = TMPro.TextAlignmentOptions.Center;
                    BountyHunter.cooldownText.transform.localPosition = bottomLeft + new Vector3(0f, -0.35f, -62f);
                    BountyHunter.cooldownText.transform.localScale = Vector3.one * 0.4f;
                    BountyHunter.cooldownText.gameObject.SetActive(true);
                }
            }           

            // First kill
            if (AmongUsClient.Instance.AmHost && TORMapOptions.shieldFirstKill && TORMapOptions.firstKillName != "" && !HideNSeek.isHideNSeekGM && !PropHunt.isPropHuntGM) {
                PlayerControl target = PlayerControl.AllPlayerControls.ToArray().ToList().FirstOrDefault(x => x.Data.PlayerName.Equals(TORMapOptions.firstKillName));
                if (target != null) {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetFirstKill, Hazel.SendOption.Reliable, -1);
                    writer.Write(target.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.setFirstKill(target.PlayerId);
                }
            }
            TORMapOptions.firstKillName = "";

            EventUtility.gameStartsUpdate();

            if (HideNSeek.isHideNSeekGM) {
                foreach (PlayerControl player in HideNSeek.getHunters()) {
                    player.moveable = false;
                    player.NetTransform.Halt();
                    HideNSeek.timer = HideNSeek.hunterWaitingTime;
                    FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(HideNSeek.hunterWaitingTime, new Action<float>((p) => {
                        if (p == 1f) {
                            player.moveable = true;
                            HideNSeek.timer = CustomOptionHolder.hideNSeekTimer.getFloat() * 60;
                            HideNSeek.isWaitingTimer = false;
                        }
                    })));
                    player.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
                }

                if (HideNSeek.polusVent == null && GameOptionsManager.Instance.currentNormalGameOptions.MapId == 2) {
                    var list = GameObject.FindObjectsOfType<Vent>().ToList();
                    var adminVent = list.FirstOrDefault(x => x.gameObject.name == "AdminVent");
                    var bathroomVent = list.FirstOrDefault(x => x.gameObject.name == "BathroomVent");
                    HideNSeek.polusVent = UnityEngine.Object.Instantiate<Vent>(adminVent);
                    HideNSeek.polusVent.gameObject.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
                    HideNSeek.polusVent.transform.position = new Vector3(36.55068f, -21.5168f, -0.0215168f);
                    HideNSeek.polusVent.Left = adminVent;
                    HideNSeek.polusVent.Right = bathroomVent;
                    HideNSeek.polusVent.Center = null;
                    HideNSeek.polusVent.Id = MapUtilities.CachedShipStatus.AllVents.Select(x => x.Id).Max() + 1; // Make sure we have a unique id
                    var allVentsList = MapUtilities.CachedShipStatus.AllVents.ToList();
                    allVentsList.Add(HideNSeek.polusVent);
                    MapUtilities.CachedShipStatus.AllVents = allVentsList.ToArray();
                    HideNSeek.polusVent.gameObject.SetActive(true);
                    HideNSeek.polusVent.name = "newVent_" + HideNSeek.polusVent.Id;

                    adminVent.Center = HideNSeek.polusVent;
                    bathroomVent.Center = HideNSeek.polusVent;
                }

                ShipStatusPatch.originalNumCrewVisionOption = GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod;
                ShipStatusPatch.originalNumImpVisionOption = GameOptionsManager.Instance.currentNormalGameOptions.ImpostorLightMod;
                ShipStatusPatch.originalNumKillCooldownOption = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown;

                GameOptionsManager.Instance.currentNormalGameOptions.ImpostorLightMod = CustomOptionHolder.hideNSeekHunterVision.getFloat();
                GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod = CustomOptionHolder.hideNSeekHuntedVision.getFloat();
                GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown = CustomOptionHolder.hideNSeekKillCooldown.getFloat();
            }
        }
    }

    [HarmonyPatch]
    class IntroPatch {
        public static IEnumerator CoBegin(IntroCutscene __instance)
        {
            SoundManager.Instance.PlaySound(__instance.IntroStinger, false, 1f, null);
            if (GameManager.Instance.IsNormal())
            {
                __instance.LogPlayerRoleData();
                __instance.HideAndSeekPanels.SetActive(false);
                __instance.CrewmateRules.SetActive(false);
                __instance.ImpostorRules.SetActive(false);
                __instance.ImpostorName.gameObject.SetActive(false);
                __instance.ImpostorTitle.gameObject.SetActive(false);
                var list = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                list =
                    IntroCutscene.SelectTeamToShow(
                        (Func<NetworkedPlayerInfo, bool>)(pcd =>
                            !PlayerControl.LocalPlayer.Data.Role.IsImpostor ||
                            pcd.Role.TeamType == PlayerControl.LocalPlayer.Data.Role.TeamType
                        )
                    );
                if (PlayerControl.LocalPlayer.Data.Role.IsImpostor)
                {
                    __instance.ImpostorText.gameObject.SetActive(false);
                }
                else
                {
                    int adjustedNumImpostors = GameManager.Instance.LogicOptions.GetAdjustedNumImpostors(GameData.Instance.PlayerCount);
                    if (adjustedNumImpostors == 1)
                    {
                        __instance.ImpostorText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.NumImpostorsS, new UnityEngine.Object());
                    }
                    else
                    {
                        __instance.ImpostorText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.NumImpostorsP, (Il2CppReferenceArray<Il2CppSystem.Object>)(new object[] { adjustedNumImpostors }));
                    }
                    __instance.ImpostorText.text = __instance.ImpostorText.text.Replace("[FF1919FF]", "<color=#FF1919FF>");
                    __instance.ImpostorText.text = __instance.ImpostorText.text.Replace("[]", "</color>");
                }
                yield return __instance.ShowTeam(list, 3f);
                yield return RoleDraft.CoSelectRoles(__instance).WrapToIl2Cpp();
                yield return __instance.ShowRole();
            }
            else
            {
                __instance.LogPlayerRoleData();
                __instance.HideAndSeekPanels.SetActive(true);
                if (PlayerControl.LocalPlayer.Data.Role.IsImpostor)
                {
                    __instance.CrewmateRules.SetActive(false);
                    __instance.ImpostorRules.SetActive(true);
                }
                else
                {
                    __instance.CrewmateRules.SetActive(true);
                    __instance.ImpostorRules.SetActive(false);
                }
                Il2CppSystem.Collections.Generic.List<PlayerControl> list2 = IntroCutscene.SelectTeamToShow(
                    (Func<NetworkedPlayerInfo, bool>)(pcd => PlayerControl.LocalPlayer.Data.Role.IsImpostor != pcd.Role.IsImpostor)
                );
                PlayerControl impostor = PlayerControl.AllPlayerControls.Find(
                    (Il2CppSystem.Predicate<PlayerControl>)(pc => pc.Data.Role.IsImpostor)
                );
                GameManager.Instance.SetSpecialCosmetics(impostor);
                __instance.ImpostorName.gameObject.SetActive(true);
                __instance.ImpostorTitle.gameObject.SetActive(true);
                __instance.BackgroundBar.enabled = false;
                __instance.TeamTitle.gameObject.SetActive(false);
                if (impostor != null)
                {
                    __instance.ImpostorName.text = impostor.Data.PlayerName;
                }
                else
                {
                    __instance.ImpostorName.text = "???";
                }
                yield return new WaitForSecondsRealtime(0.1f);
                PoolablePlayer playerSlot = null;
                if (impostor != null)
                {
                    playerSlot = __instance.CreatePlayer(1, 1, impostor.Data, false);
                    playerSlot.SetBodyType(PlayerBodyTypes.Normal);
                    playerSlot.SetFlipX(false);
                    playerSlot.transform.localPosition = __instance.impostorPos;
                    playerSlot.transform.localScale = Vector3.one * __instance.impostorScale;
                }
                yield return ShipStatus.Instance.CosmeticsCache.PopulateFromPlayers();
                yield return new WaitForSecondsRealtime(6f);
                if (playerSlot != null)
                {
                    playerSlot.gameObject.SetActive(false);
                }
                __instance.HideAndSeekPanels.SetActive(false);
                __instance.CrewmateRules.SetActive(false);
                __instance.ImpostorRules.SetActive(false);
                LogicOptionsHnS logicOptionsHnS = GameManager.Instance.LogicOptions as LogicOptionsHnS;
                LogicHnSMusic logicHnSMusic = GameManager.Instance.GetLogicComponent<LogicHnSMusic>() as LogicHnSMusic;
                if (logicHnSMusic != null)
                {
                    logicHnSMusic.StartMusicWithIntro();
                }
                if (PlayerControl.LocalPlayer.Data.Role.IsImpostor)
                {
                    float crewmateLeadTime = (float)logicOptionsHnS.GetCrewmateLeadTime();
                    __instance.HideAndSeekTimerText.gameObject.SetActive(true);
                    PoolablePlayer poolablePlayer;
                    AnimationClip animationClip;
                    if (AprilFoolsMode.ShouldHorseAround())
                    {
                        poolablePlayer = __instance.HorseWrangleVisualSuit;
                        poolablePlayer.gameObject.SetActive(true);
                        poolablePlayer.SetBodyType(PlayerBodyTypes.Seeker);
                        animationClip = __instance.HnSSeekerSpawnHorseAnim;
                        __instance.HorseWrangleVisualPlayer.SetBodyType(PlayerBodyTypes.Normal);
                        __instance.HorseWrangleVisualPlayer.UpdateFromPlayerData(PlayerControl.LocalPlayer.Data, PlayerControl.LocalPlayer.CurrentOutfitType, PlayerMaterial.MaskType.None, false, null, false);
                    }
                    else if (AprilFoolsMode.ShouldLongAround())
                    {
                        poolablePlayer = __instance.HideAndSeekPlayerVisual;
                        poolablePlayer.gameObject.SetActive(true);
                        poolablePlayer.SetBodyType(PlayerBodyTypes.LongSeeker);
                        animationClip = __instance.HnSSeekerSpawnLongAnim;
                    }
                    else
                    {
                        poolablePlayer = __instance.HideAndSeekPlayerVisual;
                        poolablePlayer.gameObject.SetActive(true);
                        poolablePlayer.SetBodyType(PlayerBodyTypes.Seeker);
                        animationClip = __instance.HnSSeekerSpawnAnim;
                    }
                    poolablePlayer.SetBodyCosmeticsVisible(false);
                    poolablePlayer.UpdateFromPlayerData(PlayerControl.LocalPlayer.Data, PlayerControl.LocalPlayer.CurrentOutfitType, PlayerMaterial.MaskType.None, false, null, false);
                    SpriteAnim component = poolablePlayer.GetComponent<SpriteAnim>();
                    poolablePlayer.gameObject.SetActive(true);
                    poolablePlayer.ToggleName(false);
                    component.Play(animationClip, 1f);
                    while (crewmateLeadTime > 0f)
                    {
                        __instance.HideAndSeekTimerText.text = Mathf.RoundToInt(crewmateLeadTime).ToString();
                        crewmateLeadTime -= Time.deltaTime;
                        yield return null;
                    }
                }
                else
                {
                    ShipStatus.Instance.HideCountdown = (float)logicOptionsHnS.GetCrewmateLeadTime();
                    if (AprilFoolsMode.ShouldHorseAround())
                    {
                        if (impostor != null)
                        {
                            impostor.AnimateCustom(__instance.HnSSeekerSpawnHorseInGameAnim);
                        }
                    }
                    else if (AprilFoolsMode.ShouldLongAround())
                    {
                        if (impostor != null)
                        {
                            impostor.AnimateCustom(__instance.HnSSeekerSpawnLongInGameAnim);
                        }
                    }
                    else if (impostor != null)
                    {
                        impostor.AnimateCustom(__instance.HnSSeekerSpawnAnim);
                        impostor.cosmetics.SetBodyCosmeticsVisible(false);
                    }
                }
                impostor = null;
                playerSlot = null;
            }
            ShipStatus.Instance.StartSFX();
            UnityEngine.Object.Destroy(__instance.gameObject);
            yield break;
        }

        public static void setupIntroTeamIcons(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
            // Intro solo teams
            if (Helpers.isNeutral(PlayerControl.LocalPlayer)) {
                var soloTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                soloTeam.Add(PlayerControl.LocalPlayer);
                yourTeam = soloTeam;
            }

            // Add the Spy to the Impostor team (for the Impostors)
            if (Spy.spy != null && PlayerControl.LocalPlayer.Data.Role.IsImpostor) {
                List<PlayerControl> players = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
                var fakeImpostorTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>(); // The local player always has to be the first one in the list (to be displayed in the center)
                fakeImpostorTeam.Add(PlayerControl.LocalPlayer);
                foreach (PlayerControl p in players) {
                    if (PlayerControl.LocalPlayer != p && (p == Spy.spy || p.Data.Role.IsImpostor))
                        fakeImpostorTeam.Add(p);
                }
                yourTeam = fakeImpostorTeam;
            }

            // Role draft: If spy is enabled, don't show the team
            if (CustomOptionHolder.spySpawnRate.getSelection() > 0 && PlayerControl.AllPlayerControls.ToArray().ToList().Where(x => x.Data.Role.IsImpostor).Count() > 1) {
                var fakeImpostorTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>(); // The local player always has to be the first one in the list (to be displayed in the center)
                fakeImpostorTeam.Add(PlayerControl.LocalPlayer);
                yourTeam = fakeImpostorTeam;
            }
        }

        public static void setupIntroTeam(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
            List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(PlayerControl.LocalPlayer);
            RoleInfo roleInfo = infos.Where(info => !info.isModifier).FirstOrDefault();
            var neutralColor = new Color32(76, 84, 78, 255);
            if (roleInfo == null || roleInfo == RoleInfo.crewmate) {
                if (RoleDraft.isEnabled && CustomOptionHolder.neutralRolesCountMax.getSelection() > 0) {
                    __instance.TeamTitle.text = "<size=60%>Crewmate" + Helpers.cs(Color.white, " / ") + Helpers.cs(neutralColor, "Neutral") + "</size>";
                }
                return;
            }
            if (roleInfo.isNeutral) {                
                __instance.BackgroundBar.material.color = neutralColor;
                __instance.TeamTitle.text = "Neutral";
                __instance.TeamTitle.color = neutralColor;
            }
        }

        public static IEnumerator<WaitForSeconds> EndShowRole(IntroCutscene __instance) {
            yield return new WaitForSeconds(5f);
            __instance.YouAreText.gameObject.SetActive(false);
            __instance.RoleText.gameObject.SetActive(false);
            __instance.RoleBlurbText.gameObject.SetActive(false);
            __instance.ourCrewmate.gameObject.SetActive(false);
           
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.CreatePlayer))]
        class CreatePlayerPatch {
            public static void Postfix(IntroCutscene __instance, bool impostorPositioning, ref PoolablePlayer __result) {
                if (impostorPositioning) __result.SetNameColor(Palette.ImpostorRed);
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.CoBegin))]
        class IntroCutsceneCoBeginPatch
        {
            public static bool Prefix(IntroCutscene __instance, ref Il2CppSystem.Collections.IEnumerator __result)
            {
                __result = CoBegin(__instance).WrapToIl2Cpp();

                return false;
            }
        }

#if PC
        [HarmonyPatch(typeof(IntroCutscene._ShowRole_d__41), nameof(IntroCutscene._ShowRole_d__41.MoveNext))]
#endif
#if ANDROID
        [HarmonyPatch(typeof(IntroCutscene._ShowRole_d__40), nameof(IntroCutscene._ShowRole_d__40.MoveNext))]
#endif
        class SetUpRoleTextPatch {
            static int seed = 0;
            static public void SetRoleTexts(IntroCutscene __instance) {
                // Don't override the intro of the vanilla roles
                List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(PlayerControl.LocalPlayer);
                RoleInfo roleInfo = infos.Where(info => !info.isModifier).FirstOrDefault();
                RoleInfo modifierInfo = infos.Where(info => info.isModifier).FirstOrDefault();

                if (EventUtility.isEnabled) {
                    var roleInfos = RoleInfo.allRoleInfos.Where(x => !x.isModifier).ToList();
                    if (roleInfo.isNeutral) roleInfos.RemoveAll(x => !x.isNeutral);
                    if (roleInfo.color == Palette.ImpostorRed) roleInfos.RemoveAll(x => x.color != Palette.ImpostorRed);
                    if (!roleInfo.isNeutral && roleInfo.color != Palette.ImpostorRed) roleInfos.RemoveAll(x => x.color == Palette.ImpostorRed || x.isNeutral);
                    var rnd = new System.Random(seed);
                    roleInfo = roleInfos[rnd.Next(roleInfos.Count)];
                }

                __instance.RoleBlurbText.text = "";
                if (roleInfo != null) {
                    __instance.RoleText.text = roleInfo.name;
                    __instance.RoleText.color = roleInfo.color;
                    __instance.RoleBlurbText.text = roleInfo.introDescription;
                    __instance.RoleBlurbText.color = roleInfo.color;
                }
                if (modifierInfo != null) {
                    if (modifierInfo.roleId != RoleId.Lover)
                        __instance.RoleBlurbText.text += Helpers.cs(modifierInfo.color, $"\n{modifierInfo.introDescription}");
                    else {
                        PlayerControl otherLover = PlayerControl.LocalPlayer == Lovers.lover1 ? Lovers.lover2 : Lovers.lover1;
                        __instance.RoleBlurbText.text += Helpers.cs(Lovers.color, $"\n♥ You are in love with {otherLover?.Data?.PlayerName ?? ""} ♥");
                    }
                }
                if (Deputy.knowsSheriff && Deputy.deputy != null && Sheriff.sheriff != null) {
                    if (infos.Any(info => info.roleId == RoleId.Sheriff))
                        __instance.RoleBlurbText.text += Helpers.cs(Sheriff.color, $"\nYour Deputy is {Deputy.deputy?.Data?.PlayerName ?? ""}");
                    else if (infos.Any(info => info.roleId == RoleId.Deputy))
                        __instance.RoleBlurbText.text += Helpers.cs(Sheriff.color, $"\nYour Sheriff is {Sheriff.sheriff?.Data?.PlayerName ?? ""}");
                }
            }
#if PC
            static public void SetRoleTexts(IntroCutscene._ShowRole_d__41 __instance) {
#endif
#if ANDROID
            static public void Prefix(IntroCutscene._ShowRole_d__40 __instance) {
#endif
                seed = rnd.Next(5000);
                FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) => {
                    SetRoleTexts(__instance.__4__this);
                })));
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
        class BeginCrewmatePatch {
            public static void Prefix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) {
                setupIntroTeamIcons(__instance, ref teamToDisplay);
            }

            public static void Postfix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) {
                setupIntroTeam(__instance, ref teamToDisplay);
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
        class BeginImpostorPatch {
            public static void Prefix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
                setupIntroTeamIcons(__instance, ref yourTeam);
            }

            public static void Postfix(IntroCutscene __instance, ref  Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
                setupIntroTeam(__instance, ref yourTeam);
            }
        }
    }

    /* Horses are broken since 2024.3.5 - keeping this code in case they return.
     * [HarmonyPatch(typeof(AprilFoolsMode), nameof(AprilFoolsMode.ShouldHorseAround))]
    public static class ShouldAlwaysHorseAround {
        public static bool Prefix(ref bool __result) {
            __result = EventUtility.isEnabled && !EventUtility.disableEventMode;
            return false;
        }
    }*/

    [HarmonyPatch(typeof(AprilFoolsMode), nameof(AprilFoolsMode.ShouldShowAprilFoolsToggle))]
    public static class ShouldShowAprilFoolsToggle {
        public static void Postfix(ref bool __result) {
            __result = __result || EventUtility.isEventDate || EventUtility.canBeEnabled;  // Extend it to a 7 day window instead of just 1st day of the Month
        }
    }
}

