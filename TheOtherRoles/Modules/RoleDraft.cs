using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using Reactor.Utilities.Extensions;
using TheOtherRoles.Patches;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TheOtherRoles.TheOtherRoles;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Modules
{
    [HarmonyPatch]
    class RoleDraft
    {
        public static bool isEnabled => CustomOptionHolder.isDraftMode.getBool() && (TORMapOptions.gameMode == CustomGamemodes.Classic || TORMapOptions.gameMode == CustomGamemodes.Guesser);
        public static bool isRunning = false;

        public static List<byte> pickOrder = new();
        private static bool picked = false;
        private static float timer = 0f;
        private static List<ActionButton> buttons = new List<ActionButton>();
        private static TMPro.TextMeshPro feedText;
        public static List<byte> alreadyPicked = new();
        public static IEnumerator CoSelectRoles(IntroCutscene __instance)
        {
            if (!isEnabled) yield break;

            if (CustomOptionHolder.draftModeCanChat.getBool()) HudManager.Instance.Chat.SetVisible(true);
            isRunning = true;
            SoundEffectsManager.play("draft", volume: 1f, true, true);
            alreadyPicked.Clear();
            bool playedAlert = false;
            feedText = UnityEngine.Object.Instantiate(__instance.TeamTitle, __instance.transform);
            var aspectPosition = feedText.gameObject.AddComponent<AspectPosition>();
            aspectPosition.Alignment = AspectPosition.EdgeAlignments.LeftTop;
            aspectPosition.DistanceFromEdge = new Vector2(1.62f, 1.2f);
            aspectPosition.AdjustPosition();
            feedText.transform.localScale = new Vector3(0.6f, 0.6f, 1);
            feedText.text = $"<size=200%>{ModTranslation.getString("playerPicks")}</size>\n\n";
            feedText.alignment = TMPro.TextAlignmentOptions.TopLeft;
            feedText.autoSizeTextContainer = true;
            feedText.fontSize = 3f;
            feedText.enableAutoSizing = false;
            __instance.TeamTitle.transform.localPosition = __instance.TeamTitle.transform.localPosition + new Vector3(1f, 0f);
            __instance.TeamTitle.text = $"{Helpers.ColorString(Color.red, $"<size=300%>{ModTranslation.getString("welcomeText")}</size>")}";
            __instance.BackgroundBar.enabled = false;
            __instance.TeamTitle.transform.localScale = new Vector3(0.25f, 0.25f, 1f);
            __instance.TeamTitle.autoSizeTextContainer = true;
            __instance.TeamTitle.enableAutoSizing = false;
            __instance.TeamTitle.fontSize = 5;
            __instance.TeamTitle.alignment = TMPro.TextAlignmentOptions.Top;
            __instance.ImpostorText.gameObject.SetActive(false);
            GameObject.Find("BackgroundLayer")?.SetActive(false);
            foreach (var player in UnityEngine.Object.FindObjectsOfType<PoolablePlayer>())
            {
                if (player.name.Contains("Dummy"))
                {
                    player.gameObject.SetActive(false);
                }
            }
            __instance.FrontMost.gameObject.SetActive(false);

            if (AmongUsClient.Instance.AmHost)
            {
                sendPickOrder();
            }

            yield return CoShowRandomOrderAnimation(__instance);

            while (pickOrder.Count == 0)
            {
                yield return null;
            }

            while (pickOrder.Count > 0)
            {
                picked = false;
                timer = 0;
                float maxTimer = CustomOptionHolder.draftModeTimeToChoose.getFloat();
                string playerText = "";
                int currentPlayerNumber = PlayerControl.AllPlayerControls.Count - pickOrder.Count + 1;
                while (timer < maxTimer || !picked)
                {
                    if (pickOrder.Count == 0)
                        break;
                    // wait for pick
                    timer += Time.deltaTime;
                    if (PlayerControl.LocalPlayer.PlayerId == pickOrder[0])
                    {
                        if (!playedAlert)
                        {
                            playedAlert = true;
                            SoundManager.Instance.PlaySound(ShipStatus.Instance.SabotageSound, false, 1f, null);
                        }
                        // Animate beginning of choice, by changing background color
                        float min = 50 / 255f;
                        Color backGroundColor = new Color(min, min, min, 1);
                        if (timer < 1)
                        {
                            float max = 230 / 255f;
                            if (timer < 0.5f)
                            { // White flash                              
                                float p = timer / 0.5f;
                                float value = (float)Math.Pow(p, 2f) * max;
                                backGroundColor = new Color(value, value, value, 1);
                            }
                            else
                            {
                                float p = (1 - timer) / 0.5f;
                                float value = (float)Math.Pow(p, 2f) * max + (1 - (float)Math.Pow(p, 2f)) * min;
                                backGroundColor = new Color(value, value, value, 1);
                            }

                        }
                        HudManager.Instance.FullScreen.color = backGroundColor;
                        GameObject.Find("BackgroundLayer")?.SetActive(false);

                        // enable pick, wait for pick
                        Color youColor = timer - (int)timer > 0.5 ? Color.red : Color.yellow;
                        playerText = string.Format(ModTranslation.getString("playerYouText"), currentPlayerNumber);
                        // Available Roles:
                        List<RoleInfo> availableRoles = new();
                        foreach (RoleInfo roleInfo in RoleInfo.allRoleInfos)
                        {
                            int impostorCount = PlayerControl.AllPlayerControls.ToArray().ToList().Where(x => x.Data.Role.IsImpostor).Count();
                            if (roleInfo.isModifier) continue;
                            // Remove Impostor Roles
                            if (PlayerControl.LocalPlayer.Data.Role.IsImpostor && !roleInfo.isImpostor) continue;
                            if (!PlayerControl.LocalPlayer.Data.Role.IsImpostor && roleInfo.isImpostor) continue;

                            RoleManagerSelectRolesPatch.RoleAssignmentData roleData = RoleManagerSelectRolesPatch.getRoleAssignmentData();
                            roleData.crewSettings.Add((byte)RoleId.Sheriff, CustomOptionHolder.sheriffSpawnRate.getSelection());
                            if (CustomOptionHolder.sheriffSpawnRate.getSelection() > 0)
                                roleData.crewSettings.Add((byte)RoleId.Deputy, CustomOptionHolder.deputySpawnRate.getSelection());
                            if (roleData.neutralSettings.ContainsKey((byte)roleInfo.roleId) && roleData.neutralSettings[(byte)roleInfo.roleId] == 0) continue;
                            else if (roleData.impSettings.ContainsKey((byte)roleInfo.roleId) && roleData.impSettings[(byte)roleInfo.roleId] == 0) continue;
                            else if (roleData.crewSettings.ContainsKey((byte)roleInfo.roleId) && roleData.crewSettings[(byte)roleInfo.roleId] == 0) continue;
                            else if (new List<RoleId>() { RoleId.Janitor, RoleId.Godfather, RoleId.Mafioso }.Contains(roleInfo.roleId) && (CustomOptionHolder.mafiaSpawnRate.getSelection() == 0 || GameOptionsManager.Instance.currentGameOptions.NumImpostors < 3)) continue;
                            else if (roleInfo.roleId == RoleId.Sidekick) continue;
                            if (roleInfo.roleId == RoleId.Deputy && Sheriff.sheriff == null) continue;
                            if (roleInfo.roleId == RoleId.Pursuer) continue;
                            if (roleInfo.roleId == RoleId.Spy && impostorCount < 2) continue;
                            if (roleInfo.roleId == RoleId.Prosecutor && (CustomOptionHolder.lawyerIsProsecutorChance.getSelection() == 0 || CustomOptionHolder.lawyerSpawnRate.getSelection() == 0)) continue;
                            if (roleInfo.roleId == RoleId.Lawyer && (CustomOptionHolder.lawyerIsProsecutorChance.getSelection() == 10 || CustomOptionHolder.lawyerSpawnRate.getSelection() == 0)) continue;
                            if (TORMapOptions.gameMode == CustomGamemodes.Guesser && (roleInfo.roleId == RoleId.EvilGuesser || roleInfo.roleId == RoleId.NiceGuesser)) continue;
                            if (alreadyPicked.Contains((byte)roleInfo.roleId) && roleInfo.roleId != RoleId.Crewmate) continue;
                            if (CustomOptionHolder.crewmateRolesFill.getBool() && roleInfo.roleId == RoleId.Crewmate) continue;

                            int impsPicked = alreadyPicked.Where(x => RoleInfo.roleInfoById[(RoleId)x].isImpostor).Count();

                            // Hanlde forcing of 100% roles for impostors
                            if (PlayerControl.LocalPlayer.Data.Role.IsImpostor)
                            {
                                int impsMax = CustomOptionHolder.impostorRolesCountMax.getSelection();
                                int impsMin = CustomOptionHolder.impostorRolesCountMin.getSelection();
                                if (impsMin > impsMax) impsMin = impsMax;
                                int impsLeft = pickOrder.Where(x => Helpers.playerById(x).Data.Role.IsImpostor).Count();
                                int imps100 = roleData.impSettings.Where(x => x.Value == 10).Count();
                                if (imps100 > impsMax) imps100 = impsMax;
                                int imps100Picked = alreadyPicked.Where(x => roleData.impSettings.GetValueSafe(x) == 10).Count();
                                if (imps100 - imps100Picked >= impsLeft && !(roleData.impSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.roleId).Count() > 0)) continue;
                                if (impsMin - impsPicked >= impsLeft && roleInfo.roleId == RoleId.Impostor) continue;
                                if (impsPicked >= impsMax && roleInfo.roleId != RoleId.Impostor) continue;
                            }

                            // Player is no impostor! Handle forcing of 100% roles for crew and neutral
                            else
                            {
                                // No more neutrals possible!
                                int neutralsPicked = alreadyPicked.Where(x => RoleInfo.roleInfoById[(RoleId)x].isNeutral).Count();
                                int crewPicked = alreadyPicked.Count - impsPicked - neutralsPicked;
                                int neutralsMax = CustomOptionHolder.neutralRolesCountMax.getSelection();
                                int neutralsMin = CustomOptionHolder.neutralRolesCountMin.getSelection();
                                int neutrals100 = roleData.neutralSettings.Where(x => x.Value == 10).Count();
                                if (neutrals100 > neutralsMin) neutralsMin = neutrals100;
                                if (neutralsMin > neutralsMax) neutralsMin = neutralsMax;

                                // If crewmate fill disabled and crew picked the amount of allowed crewmates alreay: no more crewmate except vanilla crewmate allowed!
                                int crewLimit = PlayerControl.AllPlayerControls.Count - impostorCount - (neutralsMin > neutrals100 ? neutralsMin : neutrals100 > neutralsMax ? neutralsMax : neutrals100);
                                int maxCrew = CustomOptionHolder.crewmateRolesFill.getBool() ? CustomOptionHolder.crewmateRolesCountMax.getSelection() : crewLimit;
                                if (maxCrew > crewLimit)
                                    maxCrew = crewLimit;
                                if (crewPicked >= crewLimit && !roleInfo.isNeutral && roleInfo.roleId != RoleId.Crewmate) continue;
                                // Fill roles means no crewmates allowed!
                                if (CustomOptionHolder.crewmateRolesFill.getBool() && roleInfo.roleId == RoleId.Crewmate) continue;

                                bool allowAnyNeutral = false;
                                if (neutralsPicked >= neutralsMax && roleInfo.isNeutral) continue;
                                // More neutrals needed? Then no more crewmates! This takes precedence over crew roles set to 100%!
                                var crewmatesLeft = pickOrder.Count - pickOrder.Where(x => Helpers.playerById(x).Data.Role.IsImpostor).Count();

                                if (crewmatesLeft <= neutralsMin - neutralsPicked && !roleInfo.isNeutral)
                                {
                                    continue;
                                }
                                else if (neutralsMin - neutrals100 > neutralsPicked)
                                    allowAnyNeutral = true;
                                // Handle 100% Roles PER Faction.

                                int neutrals100Picked = alreadyPicked.Where(x => roleData.neutralSettings.GetValueSafe(x) == 10).Count();
                                if (neutrals100 > neutralsMax) neutrals100 = neutralsMax;

                                int crew100 = roleData.crewSettings.Where(x => x.Value == 10).Count();
                                int crew100Picked = alreadyPicked.Where(x => roleData.crewSettings.GetValueSafe(x) == 10).Count();
                                if (neutrals100 > neutralsMax) neutrals100 = neutralsMax;

                                if (crew100 > maxCrew) crew100 = maxCrew;
                                if ((neutrals100 - neutrals100Picked >= crewmatesLeft || roleInfo.isNeutral && neutrals100 - neutrals100Picked >= neutralsMax - neutralsPicked) && !(neutrals100Picked >= neutralsMax) && !(roleData.neutralSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.roleId).Count() > 0)) continue;
                                if (!(allowAnyNeutral && roleInfo.isNeutral) && crew100 - crew100Picked >= crewmatesLeft && !(roleData.crewSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.roleId).Count() > 0)) continue;

                                if (!(allowAnyNeutral && roleInfo.isNeutral) && neutrals100 + crew100 - neutrals100Picked - crew100Picked >= crewmatesLeft && !(roleData.crewSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.roleId).Count() > 0 || roleData.neutralSettings.Where(x => x.Value == 10 && x.Key == (byte)roleInfo.roleId).Count() > 0)) continue;

                            }
                            // Handle role pairings that are blocked, e.g. Vampire Warlock, Cleaner Vulture etc.
                            bool blocked = false;
                            foreach (var blockedRoleId in CustomOptionHolder.blockedRolePairings)
                            {
                                if (alreadyPicked.Contains(blockedRoleId.Key) && blockedRoleId.Value.ToList().Contains((byte)roleInfo.roleId))
                                {
                                    blocked = true;
                                    break;
                                }
                            }
                            if (blocked) continue;


                            availableRoles.Add(roleInfo);
                        }

                        // Fallback for if all roles are somehow removed. (This is only the case if there is a bug, hence print a warning
                        if (availableRoles.Count == 0)
                        {
                            if (PlayerControl.LocalPlayer.Data.Role.IsImpostor)
                                availableRoles.Add(RoleInfo.impostor);
                            else
                                availableRoles.Add(RoleInfo.crewmate);
                            TheOtherRolesPlugin.Logger.LogWarning("Draft Mode: Fallback triggered, because no roles were left. Forced addition of basegame Imp/Crewmate");
                        }

                        List<RoleInfo> originalAvailable = new(availableRoles);

                        // remove some roles, so that you can't always get the same roles:
                        if (availableRoles.Count > CustomOptionHolder.draftModeAmountOfChoices.getFloat())
                        {
                            int countToRemove = availableRoles.Count - (int)CustomOptionHolder.draftModeAmountOfChoices.getFloat();
                            while (countToRemove-- > 0)
                            {
                                var toRemove = availableRoles.OrderBy(_ => Guid.NewGuid()).First();
                                availableRoles.Remove(toRemove);
                            }
                        }

                        if (timer >= maxTimer)
                        {
                            sendPick((byte)originalAvailable.OrderBy(_ => Guid.NewGuid()).First().roleId);
                        }


                        if (GameObject.Find("RoleButton") == null)
                        {
                            SoundEffectsManager.play("timemasterShield");
                            int i = 0;
                            int buttonsPerRow = 4;
                            int lastRow = availableRoles.Count / buttonsPerRow;
                            int buttonsInLastRow = availableRoles.Count % buttonsPerRow;

                            foreach (RoleInfo roleInfo in availableRoles)
                            {
                                float row = i / buttonsPerRow;
                                float col = i % buttonsPerRow;
                                if (buttonsInLastRow != 0 && row == lastRow)
                                {
                                    col += (buttonsPerRow - buttonsInLastRow) / 2f;
                                }
                                // planned rows: maximum of 4, hence the following calculation for rows as well:
                                row += (4 - lastRow - 1) / 2f;
                                float extraYOffset = buttons.Count >= 4 ? 2f : 0f;

                                ActionButton actionButton = UnityEngine.Object.Instantiate(HudManager.Instance.KillButton, __instance.TeamTitle.transform);
                                actionButton.gameObject.SetActive(true);
                                actionButton.gameObject.name = "RoleButton";
                                actionButton.transform.localPosition = new Vector3(-8.4f + col * 5.5f, (-10 - row * 3f - extraYOffset) + 1f);
                                actionButton.transform.localScale = new Vector3(2f, 2f);
                                actionButton.SetCoolDown(0, 0);
                                GameObject textHolder = new GameObject("textHolder");
                                var text = textHolder.AddComponent<TMPro.TextMeshPro>();
                                text.text = $"<b>{roleInfo.name}</b>";
                                text.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center;
                                text.fontSize = 4;
                                textHolder.layer = actionButton.gameObject.layer;
                                text.outlineWidth = 0.1f;
                                text.outlineColor = Color.black;
                                text.color = roleInfo.color;
                                textHolder.transform.SetParent(actionButton.transform, false);
                                textHolder.transform.localPosition = new Vector3(0, -3.075f, -1);
                                GameObject buttonSprite = new GameObject("buttonSprite");
                                var sprite = buttonSprite.AddComponent<SpriteRenderer>();
                                sprite.sprite =
                                    roleInfo.isImpostor ?
                                    Helpers.loadSpriteFromResources("TheOtherRoles.Resources.DraftRoleCardImpostor.png", 250f) :
                                    roleInfo.isNeutral ?
                                    Helpers.loadSpriteFromResources("TheOtherRoles.Resources.DraftRoleCardNeutral.png", 250f) :
                                    Helpers.loadSpriteFromResources("TheOtherRoles.Resources.DraftRoleCardCrew.png", 250f);
                                buttonSprite.layer = actionButton.gameObject.layer;
                                buttonSprite.transform.SetParent(actionButton.transform, false);
                                buttonSprite.transform.localPosition = new Vector3(0, 0.025f, -1);

                                PassiveButton button = actionButton.GetComponent<PassiveButton>();
                                button.OnClick = new Button.ButtonClickedEvent();
                                button.OnClick.AddListener((Action)(() => {
                                    sendPick((byte)roleInfo.roleId);
                                }));
                                HudManager.Instance.StartCoroutine(Effects.Lerp(0.5f, new Action<float>((p) => {
                                    actionButton.OverrideText("");
                                })));
                                buttons.Add(actionButton);
                                i++;
                            }
                        }

                        if (GameObject.Find("RandomButton") == null && availableRoles.Count > 1)
                        {
                            int lastButtonIndex = availableRoles.Count - 1;
                            int buttonsPerRow = 4;

                            int lastRow = lastButtonIndex / buttonsPerRow;
                            int lastColumn = lastButtonIndex % buttonsPerRow;

                            int randomRow;
                            int randomColumn;
                            if (lastColumn == buttonsPerRow - 1)
                            {
                                randomRow = lastRow + 1;
                                randomColumn = 0;
                            }
                            else
                            {
                                randomRow = lastRow;
                                randomColumn = lastColumn + 1;
                            }

                            int buttonsInLastRow = availableRoles.Count % buttonsPerRow;
                            if (buttonsInLastRow != 0 && randomRow == lastRow)
                            {
                                randomColumn += (int)((buttonsPerRow - buttonsInLastRow) / 2f);
                            }

                            float rowOffset = (4 - randomRow - 1) / 2f;
                            float extraYOffset = buttons.Count >= 4 ? 2f : 0f;

                            ActionButton randomButton = Object.Instantiate(HudManager.Instance.KillButton, __instance.TeamTitle.transform);
                            randomButton.gameObject.SetActive(true);
                            randomButton.gameObject.name = "RandomButton";
                            randomButton.transform.localPosition = new Vector3(
                                (-8f + randomColumn * 5.5f) + 2f,
                                (-10 - randomRow * 3f - rowOffset * 3f) + 1f - extraYOffset,
                                0
                            );
                            randomButton.transform.localScale = new Vector3(2f, 2f);
                            randomButton.SetCoolDown(0, 0);
                            randomButton.buttonLabelText.gameObject.SetActive(false);

                            var randomTextHolder = new GameObject("randomTextHolder");
                            var randomText = randomTextHolder.AddComponent<TextMeshPro>();
                            randomText.text = $"{ModTranslation.getString("randomButtonText")}";
                            randomText.horizontalAlignment = HorizontalAlignmentOptions.Center;
                            randomText.fontSize = 4;
                            randomTextHolder.layer = randomButton.gameObject.layer;
                            randomText.outlineWidth = 0.1f;
                            randomText.outlineColor = Color.black;
                            randomText.color = Color.green;
                            randomTextHolder.transform.SetParent(randomButton.transform, false);
                            randomTextHolder.transform.localPosition = new Vector3(0, -3.075f, -1);
                            GameObject buttonSprite = new GameObject("buttonSprite");
                            var sprite = buttonSprite.AddComponent<SpriteRenderer>();
                            sprite.sprite = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.DraftRoleCardRandom.png", 250f);
                            buttonSprite.layer = randomButton.gameObject.layer;
                            buttonSprite.transform.SetParent(randomButton.transform, false);
                            buttonSprite.transform.localPosition = new Vector3(0, 0.025f, -1);

                            var randomPassiveButton = randomButton.GetComponent<PassiveButton>();
                            randomPassiveButton.OnClick.AddListener((Action)(() =>
                            {
                                var randomRole = availableRoles.OrderBy(_ => Guid.NewGuid()).Random();
                                sendPick((byte)randomRole.roleId, true);
                            }));

                            HudManager.Instance.StartCoroutine(Effects.Lerp(0.5f,
                                new Action<float>(p =>
                                {
                                    randomButton.graphic.material.SetFloat("_Desat", p % 0.5f * 2f);
                                })));

                            buttons.Add(randomButton);
                        }
                    }
                    else
                    {
                        playerText = string.Format(ModTranslation.getString("playerText"), currentPlayerNumber);
                        HudManager.Instance.FullScreen.color = Color.black;
                    }
                    __instance.TeamTitle.text = $"{Helpers.ColorString(Color.red, $"<size=300%>{ModTranslation.getString("welcomeText")}</size>")}\n\n<color=#FFFFFF> <size=200%> {ModTranslation.getString("currentlyPicking")}</size>\n\n\n<size=250%>{playerText}</size>";
                    int waitMore = pickOrder.IndexOf(PlayerControl.LocalPlayer.PlayerId);
                    string waitMoreText = "";
                    if (waitMore > 0)
                    {
                        waitMoreText = string.Format($" {ModTranslation.getString("roundUntilText")}", waitMore);
                    }
                    __instance.TeamTitle.text += $"\n\n{waitMoreText}\n{ModTranslation.getString("selectionInText")} {(int)(maxTimer + 1 - timer)}\n {(SoundManager.MusicVolume > -80 ? "♫ Music: Ultimate Superhero 3 - Kenët & Rez ♫" : "")}</color>";
                    yield return null;
                }
            }
            HudManager.Instance.FullScreen.color = Color.black;
            __instance.FrontMost.gameObject.SetActive(true);
            GameObject.Find("BackgroundLayer")?.SetActive(true);
            if (AmongUsClient.Instance.AmHost)
            {
                RoleManagerSelectRolesPatch.assignRoleTargets(null); // Assign targets for Lawyer & Prosecutor
                if (RoleManagerSelectRolesPatch.isGuesserGamemode) RoleManagerSelectRolesPatch.assignGuesserGamemode();
                RoleManagerSelectRolesPatch.assignModifiers(); // Assign modifier
            }

            float myTimer = 0f;
            while (myTimer < 3f)
            {
                myTimer += Time.deltaTime;
                Color c = new Color(0, 0, 0, myTimer / 3.0f);
                __instance.FrontMost.color = c;
                yield return null;
            }

            SoundEffectsManager.stop("draft");
            isRunning = false;
            if (CustomOptionHolder.draftModeCanChat.getBool() && !(PlayerControl.LocalPlayer.isLover() && CustomOptionHolder.modifierLoverEnableChat.getBool()))
                HudManager.Instance.Chat.SetVisible(false);
            yield break;
        }

        private static IEnumerator CoShowRandomOrderAnimation(IntroCutscene __instance)
        {
            var titleText = UnityEngine.Object.Instantiate(__instance.TeamTitle, __instance.transform);
            titleText.text = $"<color=red>{ModTranslation.getString("randomizingOrder")}</color>";
            titleText.transform.localPosition = new Vector3(0, 2f, -10f);
            titleText.transform.localScale = new Vector3(1.3f, 1.3f, 1f);
            titleText.alignment = TMPro.TextAlignmentOptions.Center;

            var numberText = UnityEngine.Object.Instantiate(__instance.TeamTitle, __instance.transform);
            numberText.text = "0";
            numberText.transform.localPosition = new Vector3(0, 0.5f, -10f);
            numberText.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            numberText.alignment = TMPro.TextAlignmentOptions.Center;
            numberText.color = Color.white;

            while (pickOrder.Count == 0)
            {
                yield return null;
            }

            int playerIndex = pickOrder.IndexOf(PlayerControl.LocalPlayer.PlayerId);
            int playerNumber = playerIndex + 1;

            float animationTime = 3.5f;
            float timer = 0f;
            System.Random random = new System.Random();
            float lastChangeTime = 0f;
            float changeInterval = 0.15f;

            while (timer < animationTime)
            {
                timer += Time.deltaTime;

                if (timer < animationTime - 1.0f)
                {
                    if (timer - lastChangeTime > changeInterval)
                    {
                        lastChangeTime = timer;
                        int randomNumber = random.Next(1, PlayerControl.AllPlayerControls.Count + 1);
                        numberText.text = randomNumber.ToString();
                    }
                }
                else
                {
                    float progress = (timer - (animationTime - 1.0f)) / 1.0f;
                    if (progress > 0.7f)
                    {
                        numberText.text = playerNumber.ToString();
                    }
                    else if (progress > 0.3f)
                    {
                        if (timer - lastChangeTime > changeInterval * 2f)
                        {
                            lastChangeTime = timer;
                            int randomNumber = random.Next(
                                Math.Max(1, playerNumber - 2),
                                Math.Min(PlayerControl.AllPlayerControls.Count + 1, playerNumber + 3));
                            numberText.text = randomNumber.ToString();
                        }
                    }
                }

                yield return null;
            }

            numberText.text = playerNumber.ToString();

            titleText.text = $"<color=red>{ModTranslation.getString("pickOrderText")}</color>";

            yield return new WaitForSeconds(2f);

            titleText.gameObject.Destroy();
            numberText.gameObject.Destroy();
        }

        public static void receivePick(byte playerId, byte roleId, bool isRandom)
        {
            if (!isEnabled) return;
            RPCProcedure.setRole(roleId, playerId);
            alreadyPicked.Add(roleId);
            try
            {
                pickOrder.Remove(playerId);
                timer = 0;
                picked = true;
                RoleInfo roleInfo = RoleInfo.allRoleInfos.First(x => (byte)x.roleId == roleId);
                string roleString = Helpers.ColorString(roleInfo.color, roleInfo.name);
                int roleLength = roleInfo.name.Length;  // Not used for now, but stores the amount of charactes of the roleString.

                if (!CustomOptionHolder.draftModeShowRoles.getBool() && !(playerId == PlayerControl.LocalPlayer.PlayerId))
                {
                    roleString = ModTranslation.getString("unknownRoleText");
                    roleLength = roleString.Length;
                }
                else if (CustomOptionHolder.draftModeHideRandomRoles.getBool() && isRandom && !(playerId == PlayerControl.LocalPlayer.PlayerId))
                {
                    roleString = Helpers.ColorString(Color.green, ModTranslation.getString("randomButtonText"));
                    roleLength = ModTranslation.getString("randomButtonText").Length;
                }
                else if (CustomOptionHolder.draftModeHideImpRoles.getBool() && roleInfo.isImpostor && !(playerId == PlayerControl.LocalPlayer.PlayerId))
                {
                    roleString = Helpers.ColorString(Palette.ImpostorRed, ModTranslation.getString("impostorRoleText"));
                    roleLength = ModTranslation.getString("impostorRoleText").Length;
                }
                else if (CustomOptionHolder.draftModeHideNeutralRoles.getBool() && roleInfo.isNeutral && !(playerId == PlayerControl.LocalPlayer.PlayerId))
                {
                    roleString = Helpers.ColorString(Palette.Blue, ModTranslation.getString("neutralRoleText"));
                    roleLength = ModTranslation.getString("neutralRoleText").Length;
                }
                else if (CustomOptionHolder.draftModeHideCrewRoles.getBool() && !roleInfo.isImpostor && !roleInfo.isNeutral && !(playerId == PlayerControl.LocalPlayer.PlayerId))
                {
                    roleString = Helpers.ColorString(Color.white, ModTranslation.getString("crewmateRoleText"));
                    roleLength = ModTranslation.getString("crewmateRoleText").Length;
                }

                string line = $"{(playerId == PlayerControl.LocalPlayer.PlayerId ? ModTranslation.getString("youText") : alreadyPicked.Count)}:";
                line = line + string.Concat(Enumerable.Repeat(" ", 6 - line.Length)) + roleString;
                feedText.text += line + "\n";
                SoundEffectsManager.play("select");
            }
            catch (Exception e) { TheOtherRolesPlugin.Logger.LogError(e); }
        }

        public static void sendPick(byte RoleId, bool isRandom = false)
        {
            SoundEffectsManager.stop("timeMasterShield");
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DraftModePick, SendOption.Reliable, -1);
            writer.Write(PlayerControl.LocalPlayer.PlayerId);
            writer.Write(RoleId);
            writer.Write(isRandom);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            receivePick(PlayerControl.LocalPlayer.PlayerId, RoleId, isRandom);

            // destroy all the buttons:
            foreach (var button in buttons)
            {
                button?.gameObject?.Destroy();
            }
            buttons.Clear();
        }


        public static void sendPickOrder()
        {
            pickOrder = PlayerControl.AllPlayerControls.ToArray().Select(x => x.PlayerId).OrderBy(_ => Guid.NewGuid()).ToList().ToList();
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DraftModePickOrder, SendOption.Reliable, -1);
            writer.Write((byte)pickOrder.Count);
            foreach (var item in pickOrder)
            {
                writer.Write(item);
            }
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }


        public static void receivePickOrder(int amount, MessageReader reader)
        {
            pickOrder.Clear();
            for (int i = 0; i < amount; i++)
            {
                pickOrder.Add(reader.ReadByte());
            }
        }
    }
}