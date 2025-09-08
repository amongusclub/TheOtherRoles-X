using System.Collections.Generic;
using TheOtherRoles.Modules;
using TheOtherRoles.Utilities;
using UnityEngine;
using static TheOtherRoles.TheOtherRoles;
using Types = TheOtherRoles.Modules.CustomOption.CustomOptionType;

namespace TheOtherRoles;

public class CustomOptionHolder
{
    public static string[] rates = new[]
        { "0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%" };

    public static string[] ratesModifier = new[]
        { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };

    public static string[] presets = new[]
    {
        "presetSelectionText1", "presetSelectionText2", "presetSelectionText3", "presetSelectionText4", "presetSelectionText5",
        "presetSelectionText6", "presetSelectionText7", "presetSelectionText8"
    };

    public static CustomOption presetSelection;
    public static CustomOption activateRoles;
    public static CustomOption crewmateRolesCountMin;
    public static CustomOption crewmateRolesCountMax;
    public static CustomOption crewmateRolesFill;
    public static CustomOption neutralRolesCountMin;
    public static CustomOption neutralRolesCountMax;
    public static CustomOption impostorRolesCountMin;
    public static CustomOption impostorRolesCountMax;
    public static CustomOption modifiersCountMin;
    public static CustomOption modifiersCountMax;

    public static CustomOption isDraftMode;
    public static CustomOption draftModeCanChat;
    public static CustomOption draftModeAmountOfChoices;
    public static CustomOption draftModeTimeToChoose;
    public static CustomOption draftModeShowRoles;
    public static CustomOption draftModeHideRandomRoles;
    public static CustomOption draftModeHideCrewRoles;
    public static CustomOption draftModeHideImpRoles;
    public static CustomOption draftModeHideNeutralRoles;

    public static CustomOption anyPlayerCanStopStart;
    public static CustomOption enableEventMode;
    public static CustomOption eventReallyNoMini;
    public static CustomOption eventKicksPerRound;
    public static CustomOption eventHeavyAge;
    public static CustomOption deadImpsBlockSabotage;

    public static CustomOption mafiaSpawnRate;
    public static CustomOption janitorCooldown;

    public static CustomOption morphlingSpawnRate;
    public static CustomOption morphlingCooldown;
    public static CustomOption morphlingDuration;

    public static CustomOption camouflagerSpawnRate;
    public static CustomOption camouflagerCooldown;
    public static CustomOption camouflagerDuration;

    public static CustomOption vampireSpawnRate;
    public static CustomOption vampireKillDelay;
    public static CustomOption vampireCooldown;
    public static CustomOption vampireCanKillNearGarlics;

    public static CustomOption eraserSpawnRate;
    public static CustomOption eraserCooldown;
    public static CustomOption eraserCanEraseAnyone;
    public static CustomOption guesserSpawnRate;
    public static CustomOption guesserIsImpGuesserRate;
    public static CustomOption guesserNumberOfShots;
    public static CustomOption guesserHasMultipleShotsPerMeeting;
    public static CustomOption guesserKillsThroughShield;
    public static CustomOption guesserEvilCanKillSpy;
    public static CustomOption guesserSpawnBothRate;
    public static CustomOption guesserCantGuessSnitchIfTaksDone;

    public static CustomOption jesterSpawnRate;
    public static CustomOption jesterCanCallEmergency;
    public static CustomOption jesterHasImpostorVision;

    public static CustomOption arsonistSpawnRate;
    public static CustomOption arsonistCooldown;
    public static CustomOption arsonistDuration;

    public static CustomOption jackalSpawnRate;
    public static CustomOption jackalKillCooldown;
    public static CustomOption jackalCreateSidekickCooldown;
    public static CustomOption jackalCanSabotageLights;
    public static CustomOption jackalCanUseVents;
    public static CustomOption jackalCanCreateSidekick;
    public static CustomOption sidekickPromotesToJackal;
    public static CustomOption sidekickCanKill;
    public static CustomOption sidekickCanUseVents;
    public static CustomOption sidekickCanSabotageLights;
    public static CustomOption jackalPromotedFromSidekickCanCreateSidekick;
    public static CustomOption jackalCanCreateSidekickFromImpostor;
    public static CustomOption jackalAndSidekickHaveImpostorVision;

    public static CustomOption bountyHunterSpawnRate;
    public static CustomOption bountyHunterBountyDuration;
    public static CustomOption bountyHunterReducedCooldown;
    public static CustomOption bountyHunterPunishmentTime;
    public static CustomOption bountyHunterShowArrow;
    public static CustomOption bountyHunterArrowUpdateIntervall;

    public static CustomOption witchSpawnRate;
    public static CustomOption witchCooldown;
    public static CustomOption witchAdditionalCooldown;
    public static CustomOption witchCanSpellAnyone;
    public static CustomOption witchSpellCastingDuration;
    public static CustomOption witchTriggerBothCooldowns;
    public static CustomOption witchVoteSavesTargets;

    public static CustomOption ninjaSpawnRate;
    public static CustomOption ninjaCooldown;
    public static CustomOption ninjaKnowsTargetLocation;
    public static CustomOption ninjaTraceTime;
    public static CustomOption ninjaTraceColorTime;
    public static CustomOption ninjaInvisibleDuration;

    public static CustomOption mayorSpawnRate;
    public static CustomOption mayorCanSeeVoteColors;
    public static CustomOption mayorTasksNeededToSeeVoteColors;
    public static CustomOption mayorMeetingButton;
    public static CustomOption mayorMaxRemoteMeetings;
    public static CustomOption mayorChooseSingleVote;

    public static CustomOption portalmakerSpawnRate;
    public static CustomOption portalmakerCooldown;
    public static CustomOption portalmakerUsePortalCooldown;
    public static CustomOption portalmakerLogOnlyColorType;
    public static CustomOption portalmakerLogHasTime;
    public static CustomOption portalmakerCanPortalFromAnywhere;

    public static CustomOption engineerSpawnRate;
    public static CustomOption engineerNumberOfFixes;
    public static CustomOption engineerHighlightForImpostors;
    public static CustomOption engineerHighlightForTeamJackal;

    public static CustomOption sheriffSpawnRate;
    public static CustomOption sheriffCooldown;
    public static CustomOption sheriffCanKillNeutrals;
    public static CustomOption deputySpawnRate;

    public static CustomOption deputyNumberOfHandcuffs;
    public static CustomOption deputyHandcuffCooldown;
    public static CustomOption deputyGetsPromoted;
    public static CustomOption deputyKeepsHandcuffs;
    public static CustomOption deputyHandcuffDuration;
    public static CustomOption deputyKnowsSheriff;

    public static CustomOption lighterSpawnRate;
    public static CustomOption lighterModeLightsOnVision;
    public static CustomOption lighterModeLightsOffVision;
    public static CustomOption lighterFlashlightWidth;

    public static CustomOption detectiveSpawnRate;
    public static CustomOption detectiveAnonymousFootprints;
    public static CustomOption detectiveFootprintIntervall;
    public static CustomOption detectiveFootprintDuration;
    public static CustomOption detectiveReportNameDuration;
    public static CustomOption detectiveReportColorDuration;

    public static CustomOption timeMasterSpawnRate;
    public static CustomOption timeMasterCooldown;
    public static CustomOption timeMasterRewindTime;
    public static CustomOption timeMasterShieldDuration;

    public static CustomOption medicSpawnRate;
    public static CustomOption medicShowShielded;
    public static CustomOption medicShowAttemptToShielded;
    public static CustomOption medicSetOrShowShieldAfterMeeting;
    public static CustomOption medicShowAttemptToMedic;

    public static CustomOption swapperSpawnRate;
    public static CustomOption swapperCanCallEmergency;
    public static CustomOption swapperCanOnlySwapOthers;
    public static CustomOption swapperSwapsNumber;
    public static CustomOption swapperRechargeTasksNumber;

    public static CustomOption seerSpawnRate;
    public static CustomOption seerMode;
    public static CustomOption seerSoulDuration;
    public static CustomOption seerLimitSoulDuration;

    public static CustomOption hackerSpawnRate;
    public static CustomOption hackerCooldown;
    public static CustomOption hackerHackeringDuration;
    public static CustomOption hackerOnlyColorType;
    public static CustomOption hackerToolsNumber;
    public static CustomOption hackerRechargeTasksNumber;
    public static CustomOption hackerNoMove;

    public static CustomOption trackerSpawnRate;
    public static CustomOption trackerUpdateIntervall;
    public static CustomOption trackerResetTargetAfterMeeting;
    public static CustomOption trackerCanTrackCorpses;
    public static CustomOption trackerCorpsesTrackingCooldown;
    public static CustomOption trackerCorpsesTrackingDuration;
    public static CustomOption trackerTrackingMethod;

    public static CustomOption snitchSpawnRate;
    public static CustomOption snitchLeftTasksForReveal;
    public static CustomOption snitchMode;
    public static CustomOption snitchTargets;

    public static CustomOption spySpawnRate;
    public static CustomOption spyCanDieToSheriff;
    public static CustomOption spyImpostorsCanKillAnyone;
    public static CustomOption spyCanEnterVents;
    public static CustomOption spyHasImpostorVision;

    public static CustomOption tricksterSpawnRate;
    public static CustomOption tricksterPlaceBoxCooldown;
    public static CustomOption tricksterLightsOutCooldown;
    public static CustomOption tricksterLightsOutDuration;

    public static CustomOption cleanerSpawnRate;
    public static CustomOption cleanerCooldown;

    public static CustomOption warlockSpawnRate;
    public static CustomOption warlockCooldown;
    public static CustomOption warlockRootTime;

    public static CustomOption securityGuardSpawnRate;
    public static CustomOption securityGuardCooldown;
    public static CustomOption securityGuardTotalScrews;
    public static CustomOption securityGuardCamPrice;
    public static CustomOption securityGuardVentPrice;
    public static CustomOption securityGuardCamDuration;
    public static CustomOption securityGuardCamMaxCharges;
    public static CustomOption securityGuardCamRechargeTasksNumber;
    public static CustomOption securityGuardNoMove;

    public static CustomOption vultureSpawnRate;
    public static CustomOption vultureCooldown;
    public static CustomOption vultureNumberToWin;
    public static CustomOption vultureCanUseVents;
    public static CustomOption vultureShowArrows;

    public static CustomOption mediumSpawnRate;
    public static CustomOption mediumCooldown;
    public static CustomOption mediumDuration;
    public static CustomOption mediumOneTimeUse;
    public static CustomOption mediumChanceAdditionalInfo;

    public static CustomOption lawyerSpawnRate;
    public static CustomOption lawyerIsProsecutorChance;
    public static CustomOption lawyerTargetCanBeJester;
    public static CustomOption lawyerVision;
    public static CustomOption lawyerKnowsRole;
    public static CustomOption lawyerCanCallEmergency;
    public static CustomOption pursuerCooldown;
    public static CustomOption pursuerBlanksNumber;

    public static CustomOption thiefSpawnRate;
    public static CustomOption thiefCooldown;
    public static CustomOption thiefHasImpVision;
    public static CustomOption thiefCanUseVents;
    public static CustomOption thiefCanKillSheriff;
    public static CustomOption thiefCanStealWithGuess;


    public static CustomOption trapperSpawnRate;
    public static CustomOption trapperCooldown;
    public static CustomOption trapperMaxCharges;
    public static CustomOption trapperRechargeTasksNumber;
    public static CustomOption trapperTrapNeededTriggerToReveal;
    public static CustomOption trapperAnonymousMap;
    public static CustomOption trapperInfoType;
    public static CustomOption trapperTrapDuration;

    public static CustomOption bomberSpawnRate;
    public static CustomOption bomberBombDestructionTime;
    public static CustomOption bomberBombDestructionRange;
    public static CustomOption bomberBombHearRange;
    public static CustomOption bomberDefuseDuration;
    public static CustomOption bomberBombCooldown;
    public static CustomOption bomberBombActiveAfter;

    public static CustomOption yoyoSpawnRate;
    public static CustomOption yoyoBlinkDuration;
    public static CustomOption yoyoMarkCooldown;
    public static CustomOption yoyoMarkStaysOverMeeting;
    public static CustomOption yoyoHasAdminTable;
    public static CustomOption yoyoAdminTableCooldown;
    public static CustomOption yoyoSilhouetteVisibility;


    public static CustomOption modifiersAreHidden;

    public static CustomOption modifierBait;
    public static CustomOption modifierBaitQuantity;
    public static CustomOption modifierBaitReportDelayMin;
    public static CustomOption modifierBaitReportDelayMax;
    public static CustomOption modifierBaitShowKillFlash;

    public static CustomOption modifierLover;
    public static CustomOption modifierLoverImpLoverRate;
    public static CustomOption modifierLoverBothDie;
    public static CustomOption modifierLoverEnableChat;

    public static CustomOption modifierBloody;
    public static CustomOption modifierBloodyQuantity;
    public static CustomOption modifierBloodyDuration;

    public static CustomOption modifierAntiTeleport;
    public static CustomOption modifierAntiTeleportQuantity;

    public static CustomOption modifierTieBreaker;

    public static CustomOption modifierSunglasses;
    public static CustomOption modifierSunglassesQuantity;
    public static CustomOption modifierSunglassesVision;

    public static CustomOption modifierMini;
    public static CustomOption modifierMiniGrowingUpDuration;
    public static CustomOption modifierMiniGrowingUpInMeeting;

    public static CustomOption modifierVip;
    public static CustomOption modifierVipQuantity;
    public static CustomOption modifierVipShowColor;

    public static CustomOption modifierInvert;
    public static CustomOption modifierInvertQuantity;
    public static CustomOption modifierInvertDuration;

    public static CustomOption modifierChameleon;
    public static CustomOption modifierChameleonQuantity;
    public static CustomOption modifierChameleonHoldDuration;
    public static CustomOption modifierChameleonFadeDuration;
    public static CustomOption modifierChameleonMinVisibility;

    public static CustomOption modifierArmored;

    public static CustomOption modifierShifter;
    public static CustomOption modifierShifterShiftsMedicShield;

    public static CustomOption maxNumberOfMeetings;
    public static CustomOption blockSkippingInEmergencyMeetings;
    public static CustomOption noVoteIsSelfVote;
    public static CustomOption hidePlayerNames;
    public static CustomOption allowParallelMedBayScans;
    public static CustomOption shieldFirstKill;
    public static CustomOption finishTasksBeforeHauntingOrZoomingOut;
    public static CustomOption camsNightVision;
    public static CustomOption camsNoNightVisionIfImpVision;

    public static CustomOption dynamicMap;
    public static CustomOption dynamicMapEnableSkeld;
    public static CustomOption dynamicMapEnableMira;
    public static CustomOption dynamicMapEnablePolus;
    public static CustomOption dynamicMapEnableAirShip;
    public static CustomOption dynamicMapEnableFungle;
    public static CustomOption dynamicMapEnableSubmerged;
    public static CustomOption dynamicMapSeparateSettings;

    //Guesser Gamemode
    public static CustomOption guesserGamemodeCrewNumber;
    public static CustomOption guesserGamemodeNeutralNumber;
    public static CustomOption guesserGamemodeImpNumber;
    public static CustomOption guesserForceJackalGuesser;
    public static CustomOption guesserForceThiefGuesser;
    public static CustomOption guesserGamemodeHaveModifier;
    public static CustomOption guesserGamemodeNumberOfShots;
    public static CustomOption guesserGamemodeHasMultipleShotsPerMeeting;
    public static CustomOption guesserGamemodeKillsThroughShield;
    public static CustomOption guesserGamemodeEvilCanKillSpy;
    public static CustomOption guesserGamemodeCantGuessSnitchIfTaksDone;
    public static CustomOption guesserGamemodeCrewGuesserNumberOfTasks;
    public static CustomOption guesserGamemodeSidekickIsAlwaysGuesser;

    // Hide N Seek Gamemode
    public static CustomOption hideNSeekHunterCount;
    public static CustomOption hideNSeekKillCooldown;
    public static CustomOption hideNSeekHunterVision;
    public static CustomOption hideNSeekHuntedVision;
    public static CustomOption hideNSeekTimer;
    public static CustomOption hideNSeekCommonTasks;
    public static CustomOption hideNSeekShortTasks;
    public static CustomOption hideNSeekLongTasks;
    public static CustomOption hideNSeekTaskWin;
    public static CustomOption hideNSeekTaskPunish;
    public static CustomOption hideNSeekCanSabotage;
    public static CustomOption hideNSeekMap;
    public static CustomOption hideNSeekHunterWaiting;

    public static CustomOption hunterLightCooldown;
    public static CustomOption hunterLightDuration;
    public static CustomOption hunterLightVision;
    public static CustomOption hunterLightPunish;
    public static CustomOption hunterAdminCooldown;
    public static CustomOption hunterAdminDuration;
    public static CustomOption hunterAdminPunish;
    public static CustomOption hunterArrowCooldown;
    public static CustomOption hunterArrowDuration;
    public static CustomOption hunterArrowPunish;

    public static CustomOption huntedShieldCooldown;
    public static CustomOption huntedShieldDuration;
    public static CustomOption huntedShieldRewindTime;
    public static CustomOption huntedShieldNumber;

    // Prop Hunt Settings
    public static CustomOption propHuntMap;
    public static CustomOption propHuntTimer;
    public static CustomOption propHuntNumberOfHunters;
    public static CustomOption hunterInitialBlackoutTime;
    public static CustomOption hunterMissCooldown;
    public static CustomOption hunterHitCooldown;
    public static CustomOption hunterMaxMissesBeforeDeath;
    public static CustomOption propBecomesHunterWhenFound;
    public static CustomOption propHunterVision;
    public static CustomOption propVision;
    public static CustomOption propHuntRevealCooldown;
    public static CustomOption propHuntRevealDuration;
    public static CustomOption propHuntRevealPunish;
    public static CustomOption propHuntUnstuckCooldown;
    public static CustomOption propHuntUnstuckDuration;
    public static CustomOption propHuntInvisCooldown;
    public static CustomOption propHuntInvisDuration;
    public static CustomOption propHuntSpeedboostCooldown;
    public static CustomOption propHuntSpeedboostDuration;
    public static CustomOption propHuntSpeedboostSpeed;
    public static CustomOption propHuntSpeedboostEnabled;
    public static CustomOption propHuntInvisEnabled;
    public static CustomOption propHuntAdminCooldown;
    public static CustomOption propHuntFindCooldown;
    public static CustomOption propHuntFindDuration;

    internal static Dictionary<byte, byte[]> blockedRolePairings = new();

    public static string cs(Color c, string s)
    {
        return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b),
            ToByte(c.a), s);
    }

    private static byte ToByte(float f)
    {
        f = Mathf.Clamp01(f);
        return (byte)(f * 255);
    }

    public static bool isMapSelectionOption(CustomOption option)
    {
        return option == propHuntMap && option == hideNSeekMap;
    }

    public static void Load()
    {
        CustomOption.vanillaSettings = TheOtherRolesPlugin.Instance.Config.Bind("Preset0", "VanillaOptions", "");

        // Role Options
        presetSelection = new CustomOption(0, "", Types.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "presetSelectionText"), presets, "", null, true);

        if (EventUtility.canBeEnabled)
            enableEventMode = CustomOption.Create(Types.General, cs(Color.green, "enableEventMode"), true,
                null, true);

        isDraftMode = CustomOption.Create(Types.General, cs(Color.yellow, "enableDraftMode"), false, null, true, heading: "headingRoleDraft");
        draftModeCanChat = CustomOption.Create(Types.General, cs(Color.yellow, "draftModeCanChat"), true, isDraftMode, false);
        draftModeAmountOfChoices = CustomOption.Create(Types.General, cs(Color.yellow, "draftModeAmountOfChoices"), 3f, 2f, 6f, 1f, isDraftMode, false);
        draftModeTimeToChoose = CustomOption.Create(Types.General, cs(Color.yellow, "draftModeTimeToChoose"), 5f, 3f, 20f, 1f, isDraftMode, false);
        draftModeShowRoles = CustomOption.Create(Types.General, cs(Color.yellow, "draftModeShowRoles"), false, isDraftMode, false);
        draftModeHideRandomRoles = CustomOption.Create(Types.General, cs(Color.yellow, "draftModeHideRandomRoles"), false, draftModeShowRoles, false);
        draftModeHideCrewRoles = CustomOption.Create(Types.General, cs(Color.yellow, "draftModeHideCrewRoles"), false, draftModeShowRoles, false);
        draftModeHideImpRoles = CustomOption.Create(Types.General, cs(Color.yellow, "draftModeHideImpRoles"), false, draftModeShowRoles, false);
        draftModeHideNeutralRoles = CustomOption.Create(Types.General, cs(Color.yellow, "draftModeHideNeutralRoles"), false, draftModeShowRoles, false);

        // Using new id's for the options to not break compatibilty with older versions
        crewmateRolesCountMin = CustomOption.Create(Types.General,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "crewmateRolesCountMin"), 15f, 0f, 15f, 1f, null, true,
            heading: "crewmateRolesCountMinHeading");
        crewmateRolesCountMax = CustomOption.Create(Types.General,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "crewmateRolesCountMax"), 15f, 0f, 15f, 1f);
        neutralRolesCountMin = CustomOption.Create(Types.General,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "neutralRolesCountMin"), 15f, 0f, 15f, 1f);
        neutralRolesCountMax = CustomOption.Create(Types.General,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "neutralRolesCountMax"), 15f, 0f, 15f, 1f);
        impostorRolesCountMin = CustomOption.Create(Types.General,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "impostorRolesCountMin"), 15f, 0f, 15f, 1f);
        impostorRolesCountMax = CustomOption.Create(Types.General,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "impostorRolesCountMax"), 15f, 0f, 15f, 1f);
        modifiersCountMin = CustomOption.Create(Types.General,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "modifiersCountMin"), 15f, 0f, 15f, 1f);
        modifiersCountMax = CustomOption.Create(Types.General,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "modifiersCountMax"), 15f, 0f, 15f, 1f);
        crewmateRolesFill = CustomOption.Create(Types.General,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "crewmateRolesFill"), false);

        mafiaSpawnRate = CustomOption.Create(Types.Impostor, Janitor.color, "mafia", rates, null, true);
        janitorCooldown = CustomOption.Create(Types.Impostor, "janitorCooldown", 30f, 10f, 60f, 2.5f, mafiaSpawnRate);

        morphlingSpawnRate = CustomOption.CreateRoleOption(Types.Impostor, RoleId.Morphling, rates, null, true);
        morphlingCooldown = CustomOption.Create(Types.Impostor, "morphlingCooldown", 30f, 10f, 60f, 2.5f, morphlingSpawnRate);
        morphlingDuration = CustomOption.Create(Types.Impostor, "morphlingDuration", 10f, 1f, 20f, 0.5f, morphlingSpawnRate);

        camouflagerSpawnRate = CustomOption.CreateRoleOption(Types.Impostor, RoleId.Camouflager, rates, null, true);
        camouflagerCooldown = CustomOption.Create(Types.Impostor, "camouflagerCooldown", 30f, 10f, 60f, 2.5f, camouflagerSpawnRate);
        camouflagerDuration = CustomOption.Create(Types.Impostor, "camouflagerDuration", 10f, 1f, 20f, 0.5f, camouflagerSpawnRate);

        vampireSpawnRate = CustomOption.CreateRoleOption(Types.Impostor, RoleId.Vampire, rates, null, true);
        vampireKillDelay = CustomOption.Create(Types.Impostor, "vampireKillDelay", 10f, 1f, 20f, 1f, vampireSpawnRate);
        vampireCooldown = CustomOption.Create(Types.Impostor, "vampireCooldown", 30f, 10f, 60f, 2.5f, vampireSpawnRate);
        vampireCanKillNearGarlics = CustomOption.Create(Types.Impostor, "vampireCanKillNearGarlics", true, vampireSpawnRate);

        eraserSpawnRate = CustomOption.CreateRoleOption(Types.Impostor, RoleId.Eraser, rates, null, true);
        eraserCooldown = CustomOption.Create(Types.Impostor, "eraserCooldown", 30f, 10f, 120f, 5f, eraserSpawnRate);
        eraserCanEraseAnyone = CustomOption.Create(Types.Impostor, "eraserCanEraseAnyone", false, eraserSpawnRate);

        tricksterSpawnRate = CustomOption.CreateRoleOption(Types.Impostor, RoleId.Trickster, rates, null, true);
        tricksterPlaceBoxCooldown = CustomOption.Create(Types.Impostor, "tricksterPlaceBoxCooldown", 10f, 2.5f, 30f, 2.5f, tricksterSpawnRate);
        tricksterLightsOutCooldown = CustomOption.Create(Types.Impostor, "tricksterLightsOutCooldown", 30f, 10f, 60f, 5f, tricksterSpawnRate);
        tricksterLightsOutDuration = CustomOption.Create(Types.Impostor, "tricksterLightsOutDuration", 15f, 5f, 60f, 2.5f, tricksterSpawnRate);

        cleanerSpawnRate = CustomOption.CreateRoleOption(Types.Impostor, RoleId.Cleaner, rates, null, true);
        cleanerCooldown = CustomOption.Create(Types.Impostor, "cleanerCooldown", 30f, 10f, 60f, 2.5f, cleanerSpawnRate);

        warlockSpawnRate = CustomOption.CreateRoleOption(Types.Impostor, RoleId.Warlock, rates, null, true); // ԭ������Cleaner.colorӦΪWarlock.color���˴�ֱ���滻RoleId
        warlockCooldown = CustomOption.Create(Types.Impostor, "warlockCooldown", 30f, 10f, 60f, 2.5f, warlockSpawnRate);
        warlockRootTime = CustomOption.Create(Types.Impostor, "warlockRootTime", 5f, 0f, 15f, 1f, warlockSpawnRate);

        bountyHunterSpawnRate = CustomOption.CreateRoleOption(Types.Impostor, RoleId.BountyHunter, rates, null, true);
        bountyHunterBountyDuration = CustomOption.Create(Types.Impostor, "bountyHunterBountyDuration", 60f, 10f, 180f, 10f, bountyHunterSpawnRate);
        bountyHunterReducedCooldown = CustomOption.Create(Types.Impostor, "bountyHunterReducedCooldown", 2.5f, 0f, 30f, 2.5f, bountyHunterSpawnRate);
        bountyHunterPunishmentTime = CustomOption.Create(Types.Impostor, "bountyHunterPunishmentTime", 20f, 0f, 60f, 2.5f, bountyHunterSpawnRate);
        bountyHunterShowArrow = CustomOption.Create(Types.Impostor, "bountyHunterShowArrow", true, bountyHunterSpawnRate);
        bountyHunterArrowUpdateIntervall = CustomOption.Create(Types.Impostor, "bountyHunterArrowUpdateIntervall", 15f, 2.5f, 60f, 2.5f, bountyHunterShowArrow);

        witchSpawnRate = CustomOption.CreateRoleOption(Types.Impostor, RoleId.Witch, rates, null, true);
        witchCooldown = CustomOption.Create(Types.Impostor, "witchCooldown", 30f, 10f, 120f, 5f, witchSpawnRate);
        witchAdditionalCooldown = CustomOption.Create(Types.Impostor, "witchAdditionalCooldown", 10f, 0f, 60f, 5f, witchSpawnRate);
        witchCanSpellAnyone = CustomOption.Create(Types.Impostor, "witchCanSpellAnyone", false, witchSpawnRate);
        witchSpellCastingDuration = CustomOption.Create(Types.Impostor, "witchSpellCastingDuration", 1f, 0f, 10f, 1f, witchSpawnRate);
        witchTriggerBothCooldowns = CustomOption.Create(Types.Impostor, "witchTriggerBothCooldowns", true, witchSpawnRate);
        witchVoteSavesTargets = CustomOption.Create(Types.Impostor, "witchVoteSavesTargets", true, witchSpawnRate);

        ninjaSpawnRate = CustomOption.CreateRoleOption(Types.Impostor, RoleId.Ninja, rates, null, true);
        ninjaCooldown = CustomOption.Create(Types.Impostor, "ninjaCooldown", 30f, 10f, 120f, 5f, ninjaSpawnRate);
        ninjaKnowsTargetLocation = CustomOption.Create(Types.Impostor, "ninjaKnowsTargetLocation", true, ninjaSpawnRate);
        ninjaTraceTime = CustomOption.Create(Types.Impostor, "ninjaTraceTime", 5f, 1f, 20f, 0.5f, ninjaSpawnRate);
        ninjaTraceColorTime = CustomOption.Create(Types.Impostor, "ninjaTraceColorTime", 2f, 0f, 20f, 0.5f, ninjaSpawnRate);
        ninjaInvisibleDuration = CustomOption.Create(Types.Impostor, "ninjaInvisibleDuration", 3f, 0f, 20f, 1f, ninjaSpawnRate);

        bomberSpawnRate = CustomOption.CreateRoleOption(Types.Impostor, RoleId.Bomber, rates, null, true);
        bomberBombDestructionTime = CustomOption.Create(Types.Impostor, "bomberBombDestructionTime", 20f, 2.5f, 120f, 2.5f, bomberSpawnRate);
        bomberBombDestructionRange = CustomOption.Create(Types.Impostor, "bomberBombDestructionRange", 50f, 5f, 150f, 5f, bomberSpawnRate);
        bomberBombHearRange = CustomOption.Create(Types.Impostor, "bomberBombHearRange", 60f, 5f, 150f, 5f, bomberSpawnRate);
        bomberDefuseDuration = CustomOption.Create(Types.Impostor, "bomberDefuseDuration", 3f, 0.5f, 30f, 0.5f, bomberSpawnRate);
        bomberBombCooldown = CustomOption.Create(Types.Impostor, "bomberBombCooldown", 15f, 2.5f, 30f, 2.5f, bomberSpawnRate);
        bomberBombActiveAfter = CustomOption.Create(Types.Impostor, "bomberBombActiveAfter", 3f, 0.5f, 15f, 0.5f, bomberSpawnRate);

        yoyoSpawnRate = CustomOption.CreateRoleOption(Types.Impostor, RoleId.Yoyo, rates, null, true);
        yoyoBlinkDuration = CustomOption.Create(Types.Impostor, "yoyoBlinkDuration", 20f, 2.5f, 120f, 2.5f, yoyoSpawnRate);
        yoyoMarkCooldown = CustomOption.Create(Types.Impostor, "yoyoMarkCooldown", 20f, 2.5f, 120f, 2.5f, yoyoSpawnRate);
        yoyoMarkStaysOverMeeting = CustomOption.Create(Types.Impostor, "yoyoMarkStaysOverMeeting", true, yoyoSpawnRate);
        yoyoHasAdminTable = CustomOption.Create(Types.Impostor, "yoyoHasAdminTable", true, yoyoSpawnRate);
        yoyoAdminTableCooldown = CustomOption.Create(Types.Impostor, "yoyoAdminTableCooldown", 20f, 2.5f, 120f, 2.5f, yoyoHasAdminTable);
        yoyoSilhouetteVisibility = CustomOption.Create(Types.Impostor, "yoyoSilhouetteVisibility", new[] { "0%", "10%", "20%", "30%", "40%", "50%" }, yoyoSpawnRate);

        guesserSpawnRate = CustomOption.Create(Types.Neutral, Guesser.color, "guesser", rates, null, true);
        guesserIsImpGuesserRate = CustomOption.Create(Types.Neutral, "guesserIsImpGuesserRate", rates, guesserSpawnRate);
        guesserNumberOfShots = CustomOption.Create(Types.Neutral, "guesserNumberOfShots", 2f, 1f, 15f, 1f, guesserSpawnRate);
        guesserHasMultipleShotsPerMeeting = CustomOption.Create(Types.Neutral, "guesserHasMultipleShotsPerMeeting", false, guesserSpawnRate);
        guesserKillsThroughShield = CustomOption.Create(Types.Neutral, "guesserKillsThroughShield", true, guesserSpawnRate);
        guesserEvilCanKillSpy = CustomOption.Create(Types.Neutral, "guesserEvilCanKillSpy", true, guesserSpawnRate);
        guesserSpawnBothRate = CustomOption.Create(Types.Neutral, "guesserSpawnBothRate", rates, guesserSpawnRate);
        guesserCantGuessSnitchIfTaksDone = CustomOption.Create(Types.Neutral, "guesserCantGuessSnitchIfTaksDone", true, guesserSpawnRate);

        jesterSpawnRate = CustomOption.CreateRoleOption(Types.Neutral, RoleId.Jester, rates, null, true);
        jesterCanCallEmergency = CustomOption.Create(Types.Neutral, "jesterCanCallEmergency", true, jesterSpawnRate);
        jesterHasImpostorVision = CustomOption.Create(Types.Neutral, "jesterHasImpostorVision", false, jesterSpawnRate);

        arsonistSpawnRate = CustomOption.CreateRoleOption(Types.Neutral, RoleId.Arsonist, rates, null, true);
        arsonistCooldown = CustomOption.Create(Types.Neutral, "arsonistCooldown", 12.5f, 2.5f, 60f, 2.5f, arsonistSpawnRate);
        arsonistDuration = CustomOption.Create(Types.Neutral, "arsonistDuration", 3f, 1f, 10f, 1f, arsonistSpawnRate);

        jackalSpawnRate = CustomOption.CreateRoleOption(Types.Neutral, RoleId.Jackal, rates, null, true);
        jackalKillCooldown = CustomOption.Create(Types.Neutral, "jackalKillCooldown", 30f, 10f, 60f, 2.5f, jackalSpawnRate);
        jackalCanUseVents = CustomOption.Create(Types.Neutral, "jackalCanUseVents", true, jackalSpawnRate);
        jackalCanSabotageLights = CustomOption.Create(Types.Neutral, "jackalCanSabotageLights", true, jackalSpawnRate);
        jackalCanCreateSidekick = CustomOption.Create(Types.Neutral, "jackalCanCreateSidekick", false, jackalSpawnRate, stringId: "jackalCanCreateSidekick");
        jackalCreateSidekickCooldown = CustomOption.Create(Types.Neutral, "jackalCreateSidekickCooldown", 30f, 10f, 60f, 2.5f, jackalCanCreateSidekick);
        sidekickPromotesToJackal = CustomOption.Create(Types.Neutral, "sidekickPromotesToJackal", false, jackalCanCreateSidekick);
        sidekickCanKill = CustomOption.Create(Types.Neutral, "sidekickCanKill", false, jackalCanCreateSidekick);
        sidekickCanUseVents = CustomOption.Create(Types.Neutral, "sidekickCanUseVents", true, jackalCanCreateSidekick);
        sidekickCanSabotageLights = CustomOption.Create(Types.Neutral, "sidekickCanSabotageLights", true, jackalCanCreateSidekick);
        jackalPromotedFromSidekickCanCreateSidekick = CustomOption.Create(Types.Neutral, "jackalPromotedFromSidekickCanCreateSidekick", true, sidekickPromotesToJackal);
        jackalCanCreateSidekickFromImpostor = CustomOption.Create(Types.Neutral, "jackalCanCreateSidekickFromImpostor", true, jackalCanCreateSidekick);
        jackalAndSidekickHaveImpostorVision = CustomOption.Create(Types.Neutral, "jackalAndSidekickHaveImpostorVision", false, jackalSpawnRate);

        vultureSpawnRate = CustomOption.CreateRoleOption(Types.Neutral, RoleId.Vulture, rates, null, true);
        vultureCooldown =
            CustomOption.Create(Types.Neutral, "vultureCooldown", 15f, 10f, 60f, 2.5f, vultureSpawnRate);
        vultureNumberToWin = CustomOption.Create(Types.Neutral, "vultureNumberToWin", 4f, 1f,
            10f, 1f, vultureSpawnRate);
        vultureCanUseVents = CustomOption.Create(Types.Neutral, "vultureCanUseVents", true, vultureSpawnRate);
        vultureShowArrows = CustomOption.Create(Types.Neutral, "vultureShowArrows", true,
            vultureSpawnRate);

        lawyerSpawnRate = CustomOption.CreateRoleOption(Types.Neutral, RoleId.Lawyer, rates, null, true);
        lawyerIsProsecutorChance = CustomOption.Create(Types.Neutral, "lawyerIsProsecutorChance", rates, lawyerSpawnRate, stringId: "lawyerIsProsecutorChance");
        lawyerVision = CustomOption.Create(Types.Neutral, "lawyerVision", 1f, 0.25f, 3f, 0.25f, lawyerSpawnRate);
        lawyerKnowsRole = CustomOption.Create(Types.Neutral, "lawyerKnowsRole", false,
            lawyerSpawnRate);
        lawyerCanCallEmergency = CustomOption.Create(Types.Neutral, "lawyerCanCallEmergency",
            true, lawyerSpawnRate);
        lawyerTargetCanBeJester =
            CustomOption.Create(Types.Neutral, "lawyerTargetCanBeJester", false, lawyerSpawnRate);
        pursuerCooldown = CustomOption.Create(Types.Neutral, "pursuerCooldown", 30f, 5f, 60f, 2.5f,
            lawyerSpawnRate);
        pursuerBlanksNumber = CustomOption.Create(Types.Neutral, "pursuerBlanksNumber", 5f, 1f, 20f, 1f,
            lawyerSpawnRate);

        thiefSpawnRate = CustomOption.CreateRoleOption(Types.Neutral, RoleId.Thief, rates, null, true);
        thiefCooldown = CustomOption.Create(Types.Neutral, "thiefCooldown", 30f, 5f, 120f, 5f, thiefSpawnRate);
        thiefCanKillSheriff = CustomOption.Create(Types.Neutral, "thiefCanKillSheriff", true, thiefSpawnRate);
        thiefHasImpVision = CustomOption.Create(Types.Neutral, "thiefHasImpVision", true, thiefSpawnRate);
        thiefCanUseVents = CustomOption.Create(Types.Neutral, "thiefCanUseVents", true, thiefSpawnRate);
        thiefCanStealWithGuess = CustomOption.Create(Types.Neutral, "thiefCanStealWithGuess", false, thiefSpawnRate);

        mayorSpawnRate = CustomOption.CreateRoleOption(Types.Crewmate, RoleId.Mayor, rates, null, true);
        mayorCanSeeVoteColors =
            CustomOption.Create(Types.Crewmate, "mayorCanSeeVoteColors", false, mayorSpawnRate);
        mayorTasksNeededToSeeVoteColors = CustomOption.Create(Types.Crewmate,
            "mayorTasksNeededToSeeVoteColors", 5f, 0f, 20f, 1f, mayorCanSeeVoteColors);
        mayorMeetingButton = CustomOption.Create(Types.Crewmate, "mayorMeetingButton", true, mayorSpawnRate);
        mayorMaxRemoteMeetings = CustomOption.Create(Types.Crewmate, "mayorMaxRemoteMeetings", 1f, 1f, 5f, 1f,
            mayorMeetingButton);
        mayorChooseSingleVote = CustomOption.Create(Types.Crewmate, "mayorChooseSingleVote",
            new[] { "mayorChooseSingleVote1", "mayorChooseSingleVote2", "mayorChooseSingleVote3" }, mayorSpawnRate);

        engineerSpawnRate = CustomOption.CreateRoleOption(Types.Crewmate, RoleId.Engineer, rates, null, true);
        engineerNumberOfFixes = CustomOption.Create(Types.Crewmate, "engineerNumberOfFixes", 1f, 1f, 3f, 1f,
            engineerSpawnRate);
        engineerHighlightForImpostors = CustomOption.Create(Types.Crewmate, "engineerHighlightForImpostors", true,
            engineerSpawnRate);
        engineerHighlightForTeamJackal = CustomOption.Create(Types.Crewmate,
            "engineerHighlightForTeamJackal", true, engineerSpawnRate);

        sheriffSpawnRate = CustomOption.CreateRoleOption(Types.Crewmate, RoleId.Sheriff, rates, null, true);
        sheriffCooldown =
            CustomOption.Create(Types.Crewmate, "sheriffCooldown", 30f, 10f, 60f, 2.5f, sheriffSpawnRate);
        sheriffCanKillNeutrals =
            CustomOption.Create(Types.Crewmate, "sheriffCanKillNeutrals", false, sheriffSpawnRate);
        deputySpawnRate = CustomOption.Create(Types.Crewmate, "deputySpawnRate", rates, sheriffSpawnRate);
        deputyNumberOfHandcuffs = CustomOption.Create(Types.Crewmate, "deputyNumberOfHandcuffs", 3f, 1f, 10f,
            1f, deputySpawnRate, stringId: "deputySpawnRate");
        deputyHandcuffCooldown =
            CustomOption.Create(Types.Crewmate, "deputyHandcuffCooldown", 30f, 10f, 60f, 2.5f, deputySpawnRate);
        deputyHandcuffDuration =
            CustomOption.Create(Types.Crewmate, "deputyHandcuffDuration", 15f, 5f, 60f, 2.5f, deputySpawnRate);
        deputyKnowsSheriff = CustomOption.Create(Types.Crewmate, "deputyKnowsSheriff", true,
            deputySpawnRate);
        deputyGetsPromoted = CustomOption.Create(Types.Crewmate, "deputyGetsPromoted",
            new[] { "deputyGetsPromoted1", "deputyGetsPromoted2", "deputyGetsPromoted3" }, deputySpawnRate);
        deputyKeepsHandcuffs = CustomOption.Create(Types.Crewmate, "deputyKeepsHandcuffs", true,
            deputyGetsPromoted);

        lighterSpawnRate = CustomOption.CreateRoleOption(Types.Crewmate, RoleId.Lighter, rates, null, true);
        lighterModeLightsOnVision = CustomOption.Create(Types.Crewmate, "lighterModeLightsOnVision", 1.5f, 0.25f, 5f,
            0.25f, lighterSpawnRate);
        lighterModeLightsOffVision = CustomOption.Create(Types.Crewmate, "lighterModeLightsOffVision", 0.5f, 0.25f, 5f,
            0.25f, lighterSpawnRate);
        lighterFlashlightWidth = CustomOption.Create(Types.Crewmate, "lighterFlashlightWidth", 0.3f, 0.1f, 1f, 0.1f,
            lighterSpawnRate);

        detectiveSpawnRate =
            CustomOption.CreateRoleOption(Types.Crewmate, RoleId.Detective, rates, null, true);
        detectiveAnonymousFootprints =
            CustomOption.Create(Types.Crewmate, "detectiveAnonymousFootprints", false, detectiveSpawnRate);
        detectiveFootprintIntervall = CustomOption.Create(Types.Crewmate, "detectiveFootprintIntervall", 0.5f, 0.25f, 10f,
            0.25f, detectiveSpawnRate);
        detectiveFootprintDuration = CustomOption.Create(Types.Crewmate, "detectiveFootprintDuration", 5f, 0.25f, 10f,
            0.25f, detectiveSpawnRate);
        detectiveReportNameDuration = CustomOption.Create(Types.Crewmate,
            "detectiveReportNameDuration", 0, 0, 60, 2.5f, detectiveSpawnRate);
        detectiveReportColorDuration = CustomOption.Create(Types.Crewmate,
            "detectiveReportColorDuration", 20, 0, 120, 2.5f, detectiveSpawnRate);

        timeMasterSpawnRate =
            CustomOption.CreateRoleOption(Types.Crewmate, RoleId.TimeMaster, rates, null, true);
        timeMasterCooldown = CustomOption.Create(Types.Crewmate, "timeMasterCooldown", 30f, 10f, 120f, 2.5f,
            timeMasterSpawnRate);
        timeMasterRewindTime =
            CustomOption.Create(Types.Crewmate, "timeMasterRewindTime", 3f, 1f, 10f, 1f, timeMasterSpawnRate);
        timeMasterShieldDuration = CustomOption.Create(Types.Crewmate, "timeMasterShieldDuration", 3f, 1f, 20f,
            1f, timeMasterSpawnRate);

        medicSpawnRate = CustomOption.CreateRoleOption(Types.Crewmate, RoleId.Medic, rates, null, true);
        medicShowShielded = CustomOption.Create(Types.Crewmate, "medicShowShielded",
            new[] { "medicShowShielded1", "medicShowShielded2", "medicShowShielded3" }, medicSpawnRate);
        medicShowAttemptToShielded = CustomOption.Create(Types.Crewmate, "medicShowAttemptToShielded",
            false, medicSpawnRate);
        medicSetOrShowShieldAfterMeeting = CustomOption.Create(Types.Crewmate, "medicSetOrShowShieldAfterMeeting",
            new[] { "medicSetOrShowShieldAfterMeeting1", "medicSetOrShowShieldAfterMeeting2", "medicSetOrShowShieldAfterMeeting3" }, medicSpawnRate);

        medicShowAttemptToMedic = CustomOption.Create(Types.Crewmate,
            "medicShowAttemptToMedic", false, medicSpawnRate);

        swapperSpawnRate = CustomOption.CreateRoleOption(Types.Crewmate, RoleId.Swapper, rates, null, true);
        swapperCanCallEmergency = CustomOption.Create(Types.Crewmate, "swapperCanCallEmergency", false,
            swapperSpawnRate);
        swapperCanOnlySwapOthers =
            CustomOption.Create(Types.Crewmate, "swapperCanOnlySwapOthers", false, swapperSpawnRate);

        swapperSwapsNumber =
            CustomOption.Create(Types.Crewmate, "swapperSwapsNumber", 1f, 0f, 5f, 1f, swapperSpawnRate);
        swapperRechargeTasksNumber = CustomOption.Create(Types.Crewmate, "swapperRechargeTasksNumber",
            2f, 1f, 10f, 1f, swapperSpawnRate);

        seerSpawnRate = CustomOption.CreateRoleOption(Types.Crewmate, RoleId.Seer, rates, null, true);
        seerMode = CustomOption.Create(Types.Crewmate, "seerMode",
            new[] { "seerMode1", "seerMode2", "seerMode3" }, seerSpawnRate);
        seerLimitSoulDuration =
            CustomOption.Create(Types.Crewmate, "seerLimitSoulDuration", false, seerSpawnRate);
        seerSoulDuration = CustomOption.Create(Types.Crewmate, "seerSoulDuration", 15f, 0f, 120f, 5f,
            seerLimitSoulDuration);

        hackerSpawnRate = CustomOption.CreateRoleOption(Types.Crewmate, RoleId.Hacker, rates, null, true);
        hackerCooldown = CustomOption.Create(Types.Crewmate, "hackerCooldown", 30f, 5f, 60f, 5f, hackerSpawnRate);
        hackerHackeringDuration =
            CustomOption.Create(Types.Crewmate, "hackerHackeringDuration", 10f, 2.5f, 60f, 2.5f, hackerSpawnRate);
        hackerOnlyColorType =
            CustomOption.Create(Types.Crewmate, "hackerOnlyColorType", false, hackerSpawnRate);
        hackerToolsNumber = CustomOption.Create(Types.Crewmate, "hackerToolsNumber", 5f, 1f, 30f, 1f,
            hackerSpawnRate);
        hackerRechargeTasksNumber = CustomOption.Create(Types.Crewmate, "hackerRechargeTasksNumber",
            2f, 1f, 5f, 1f, hackerSpawnRate);
        hackerNoMove = CustomOption.Create(Types.Crewmate, "hackerNoMove", true,
            hackerSpawnRate);

        trackerSpawnRate = CustomOption.CreateRoleOption(Types.Crewmate, RoleId.Tracker, rates, null, true);
        trackerUpdateIntervall = CustomOption.Create(Types.Crewmate, "trackerUpdateIntervall", 5f, 1f, 30f, 1f,
            trackerSpawnRate);
        trackerResetTargetAfterMeeting = CustomOption.Create(Types.Crewmate, "trackerResetTargetAfterMeeting",
            false, trackerSpawnRate);
        trackerCanTrackCorpses =
            CustomOption.Create(Types.Crewmate, "trackerCanTrackCorpses", true, trackerSpawnRate);
        trackerCorpsesTrackingCooldown = CustomOption.Create(Types.Crewmate, "trackerCorpsesTrackingCooldown", 30f, 5f,
            120f, 5f, trackerCanTrackCorpses);
        trackerCorpsesTrackingDuration = CustomOption.Create(Types.Crewmate, "trackerCorpsesTrackingDuration", 5f, 2.5f,
            30f, 2.5f, trackerCanTrackCorpses);
        trackerTrackingMethod = CustomOption.Create(Types.Crewmate, "trackerTrackingMethod",
            new[] { "trackerTrackingMethod1", "trackerTrackingMethod2", "trackerTrackingMethod3" }, trackerSpawnRate);

        snitchSpawnRate = CustomOption.CreateRoleOption(Types.Crewmate, RoleId.Snitch, rates, null, true);
        snitchLeftTasksForReveal = CustomOption.Create(Types.Crewmate,
            "snitchLeftTasksForReveal", 5f, 0f, 25f, 1f, snitchSpawnRate);
        snitchMode = CustomOption.Create(Types.Crewmate, "snitchMode", new[] { "snitchMode1", "snitchMode2", "snitchMode3" },
            snitchSpawnRate);
        snitchTargets = CustomOption.Create(Types.Crewmate, "snitchTargets",
            new[] { "snitchTargets1", "snitchTargets2" }, snitchSpawnRate);

        spySpawnRate = CustomOption.CreateRoleOption(Types.Crewmate, RoleId.Spy, rates, null, true);
        spyCanDieToSheriff = CustomOption.Create(Types.Crewmate, "spyCanDieToSheriff", false, spySpawnRate);
        spyImpostorsCanKillAnyone = CustomOption.Create(Types.Crewmate,
            "spyImpostorsCanKillAnyone", true, spySpawnRate);
        spyCanEnterVents = CustomOption.Create(Types.Crewmate, "spyCanEnterVents", false, spySpawnRate);
        spyHasImpostorVision = CustomOption.Create(Types.Crewmate, "spyHasImpostorVision", false, spySpawnRate);

        portalmakerSpawnRate =
            CustomOption.CreateRoleOption(Types.Crewmate, RoleId.Portalmaker, rates, null, true);
        portalmakerCooldown = CustomOption.Create(Types.Crewmate, "portalmakerCooldown", 30f, 10f, 60f, 2.5f,
            portalmakerSpawnRate);
        portalmakerUsePortalCooldown = CustomOption.Create(Types.Crewmate, "portalmakerUsePortalCooldown", 30f, 10f, 60f,
            2.5f, portalmakerSpawnRate);
        portalmakerLogOnlyColorType = CustomOption.Create(Types.Crewmate, "portalmakerLogOnlyColorType",
            true, portalmakerSpawnRate);
        portalmakerLogHasTime = CustomOption.Create(Types.Crewmate, "portalmakerLogHasTime", true, portalmakerSpawnRate);
        portalmakerCanPortalFromAnywhere = CustomOption.Create(Types.Crewmate,
            "portalmakerCanPortalFromAnywhere", true, portalmakerSpawnRate);

        securityGuardSpawnRate = CustomOption.CreateRoleOption(Types.Crewmate, RoleId.SecurityGuard, rates, null, true);
        securityGuardCooldown = CustomOption.Create(Types.Crewmate, "securityGuardCooldown", 30f, 10f, 60f, 2.5f, securityGuardSpawnRate);
        securityGuardTotalScrews = CustomOption.Create(Types.Crewmate, "securityGuardTotalScrews", 7f, 1f, 15f, 1f, securityGuardSpawnRate);
        securityGuardCamPrice = CustomOption.Create(Types.Crewmate, "securityGuardCamPrice", 2f, 1f, 15f, 1f, securityGuardSpawnRate);
        securityGuardVentPrice = CustomOption.Create(Types.Crewmate, "securityGuardVentPrice", 1f, 1f, 15f, 1f, securityGuardSpawnRate);
        securityGuardCamDuration = CustomOption.Create(Types.Crewmate, "securityGuardCamDuration", 10f, 2.5f, 60f, 2.5f, securityGuardSpawnRate);
        securityGuardCamMaxCharges = CustomOption.Create(Types.Crewmate, "securityGuardCamMaxCharges", 5f, 1f, 30f, 1f, securityGuardSpawnRate);
        securityGuardCamRechargeTasksNumber = CustomOption.Create(Types.Crewmate, "securityGuardCamRechargeTasksNumber", 3f, 1f, 10f, 1f, securityGuardSpawnRate);
        securityGuardNoMove = CustomOption.Create(Types.Crewmate, "securityGuardNoMove", true, securityGuardSpawnRate);

        mediumSpawnRate = CustomOption.CreateRoleOption(Types.Crewmate, RoleId.Medium, rates, null, true);
        mediumCooldown = CustomOption.Create(Types.Crewmate, "mediumCooldown", 30f, 5f, 120f, 5f, mediumSpawnRate);
        mediumDuration = CustomOption.Create(Types.Crewmate, "mediumDuration", 3f, 0f, 15f, 1f, mediumSpawnRate);
        mediumOneTimeUse = CustomOption.Create(Types.Crewmate, "mediumOneTimeUse", false, mediumSpawnRate);
        mediumChanceAdditionalInfo = CustomOption.Create(Types.Crewmate, "mediumChanceAdditionalInfo", rates, mediumSpawnRate);

        trapperSpawnRate = CustomOption.CreateRoleOption(Types.Crewmate, RoleId.Trapper, rates, null, true);
        trapperCooldown = CustomOption.Create(Types.Crewmate, "trapperCooldown", 30f, 5f, 120f, 5f, trapperSpawnRate);
        trapperMaxCharges = CustomOption.Create(Types.Crewmate, "trapperMaxCharges", 5f, 1f, 15f, 1f, trapperSpawnRate);
        trapperRechargeTasksNumber = CustomOption.Create(Types.Crewmate, "trapperRechargeTasksNumber", 2f, 1f, 15f, 1f, trapperSpawnRate);
        trapperTrapNeededTriggerToReveal = CustomOption.Create(Types.Crewmate, "trapperTrapNeededTriggerToReveal", 3f, 2f, 10f, 1f, trapperSpawnRate);
        trapperAnonymousMap = CustomOption.Create(Types.Crewmate, "trapperAnonymousMap", false, trapperSpawnRate);
        trapperInfoType = CustomOption.Create(Types.Crewmate, "trapperInfoType", new[] { "trapperInfoType1", "trapperInfoType2", "trapperInfoType3" }, trapperSpawnRate);
        trapperTrapDuration = CustomOption.Create(Types.Crewmate, "trapperTrapDuration", 5f, 1f, 15f, 1f, trapperSpawnRate);

        // Modifier
        modifiersAreHidden = CustomOption.Create(Types.Modifier,
            cs(Color.yellow, "modifiersAreHidden"), true, null, true,
            heading: cs(Color.yellow, "modifiersAreHiddenHeading"));

        modifierBloody = CustomOption.CreateModifierOption(Types.Modifier, RoleId.Bloody, rates, null, true);
        modifierBloodyQuantity = CustomOption.Create(Types.Modifier, cs(Color.yellow, "modifierBloodyQuantity"),
            ratesModifier, modifierBloody);
        modifierBloodyDuration =
            CustomOption.Create(Types.Modifier, "modifierBloodyDuration", 10f, 3f, 60f, 1f, modifierBloody);

        modifierAntiTeleport =
            CustomOption.CreateModifierOption(Types.Modifier, RoleId.AntiTeleport, rates, null, true);
        modifierAntiTeleportQuantity = CustomOption.Create(Types.Modifier,
            cs(Color.yellow, "modifierAntiTeleportQuantity"), ratesModifier, modifierAntiTeleport);

        modifierTieBreaker =
            CustomOption.CreateModifierOption(Types.Modifier, RoleId.Tiebreaker, rates, null, true);

        modifierBait = CustomOption.CreateModifierOption(Types.Modifier, RoleId.Bait, rates, null, true);
        modifierBaitQuantity = CustomOption.Create(Types.Modifier, cs(Color.yellow, "modifierBaitQuantity"),
            ratesModifier, modifierBait);
        modifierBaitReportDelayMin =
            CustomOption.Create(Types.Modifier, "modifierBaitReportDelayMin", 0f, 0f, 10f, 1f, modifierBait);
        modifierBaitReportDelayMax =
            CustomOption.Create(Types.Modifier, "modifierBaitReportDelayMax", 0f, 0f, 10f, 1f, modifierBait);
        modifierBaitShowKillFlash =
            CustomOption.Create(Types.Modifier, "modifierBaitShowKillFlash", true, modifierBait);

        modifierLover = CustomOption.CreateModifierOption(Types.Modifier, RoleId.Lover, rates, null, true);
        modifierLoverImpLoverRate = CustomOption.Create(Types.Modifier, "modifierLoverImpLoverRate",
            rates, modifierLover);
        modifierLoverBothDie = CustomOption.Create(Types.Modifier, "modifierLoverBothDie", true, modifierLover);
        modifierLoverEnableChat = CustomOption.Create(Types.Modifier, "modifierLoverEnableChat", true, modifierLover);

        modifierSunglasses =
            CustomOption.CreateModifierOption(Types.Modifier, RoleId.Sunglasses, rates, null, true);
        modifierSunglassesQuantity = CustomOption.Create(Types.Modifier, cs(Color.yellow, "modifierSunglassesQuantity"),
            ratesModifier, modifierSunglasses);
        modifierSunglassesVision = CustomOption.Create(Types.Modifier, "modifierSunglassesVision",
            new[] { "-10%", "-20%", "-30%", "-40%", "-50%" }, modifierSunglasses);

        modifierMini = CustomOption.CreateModifierOption(Types.Modifier, RoleId.Mini, rates, null, true);
        modifierMiniGrowingUpDuration = CustomOption.Create(Types.Modifier, "modifierMiniGrowingUpDuration", 400f,
            100f, 1500f, 100f, modifierMini);
        modifierMiniGrowingUpInMeeting =
            CustomOption.Create(Types.Modifier, "modifierMiniGrowingUpInMeeting", true, modifierMini);
        if (EventUtility.canBeEnabled || EventUtility.isEnabled)
        {
            eventKicksPerRound = CustomOption.Create(Types.Modifier,
                cs(Color.green, "eventKicksPerRound"), 4f, 0f, 14f, 1f, modifierMini);
            eventHeavyAge = CustomOption.Create(Types.Modifier, cs(Color.green, "eventHeavyAge"),
                12f, 6f, 18f, 0.5f, modifierMini);
            eventReallyNoMini = CustomOption.Create(Types.Modifier, cs(Color.green, "eventReallyNoMini"), false,
                modifierMini, invertedParent: true);
        }

        modifierVip = CustomOption.CreateModifierOption(Types.Modifier, RoleId.Vip, rates, null, true);
        modifierVipQuantity = CustomOption.Create(Types.Modifier, cs(Color.yellow, "modifierVipQuantity"), ratesModifier,
            modifierVip);
        modifierVipShowColor = CustomOption.Create(Types.Modifier, "modifierVipShowColor", true, modifierVip);

        modifierInvert = CustomOption.CreateModifierOption(Types.Modifier, RoleId.Invert, rates, null, true);
        modifierInvertQuantity = CustomOption.Create(Types.Modifier, cs(Color.yellow, "modifierInvertQuantity"),
            ratesModifier, modifierInvert);
        modifierInvertDuration = CustomOption.Create(Types.Modifier, "modifierInvertDuration", 3f, 1f, 15f,
            1f, modifierInvert);

        modifierChameleon = CustomOption.CreateModifierOption(Types.Modifier, RoleId.Chameleon, rates, null, true);
        modifierChameleonQuantity = CustomOption.Create(Types.Modifier, cs(Color.yellow, "modifierChameleonQuantity"),
            ratesModifier, modifierChameleon);
        modifierChameleonHoldDuration = CustomOption.Create(Types.Modifier, "modifierChameleonHoldDuration", 3f, 1f,
            10f, 0.5f, modifierChameleon);
        modifierChameleonFadeDuration = CustomOption.Create(Types.Modifier, "modifierChameleonFadeDuration", 1f, 0.25f, 10f,
            0.25f, modifierChameleon);
        modifierChameleonMinVisibility = CustomOption.Create(Types.Modifier, "modifierChameleonMinVisibility",
            new[] { "0%", "10%", "20%", "30%", "40%", "50%" }, modifierChameleon);

        modifierArmored = CustomOption.CreateModifierOption(Types.Modifier, RoleId.Armored, rates, null, true);

        modifierShifter = CustomOption.CreateModifierOption(Types.Modifier, RoleId.Shifter, rates, null, true);
        modifierShifterShiftsMedicShield =
            CustomOption.Create(Types.Modifier, "modifierShifterShiftsMedicShield", false, modifierShifter);

        // Guesser Gamemode (2000 - 2999)
        guesserGamemodeCrewNumber = CustomOption.Create(Types.Guesser,
            cs(Guesser.color, "guesserGamemodeCrewNumber"), 15f, 0f, 15f, 1f, null, true, heading: "guesserGamemodeCrewNumberHeading");
        guesserGamemodeNeutralNumber = CustomOption.Create(Types.Guesser,
            cs(Guesser.color, "guesserGamemodeNeutralNumber"), 15f, 0f, 15f, 1f);
        guesserGamemodeImpNumber = CustomOption.Create(Types.Guesser,
            cs(Guesser.color, "guesserGamemodeImpNumber"), 15f, 0f, 15f, 1f);
        guesserForceJackalGuesser = CustomOption.Create(Types.Guesser, "guesserForceJackalGuesser", false, null, true,
            heading: "guesserForceJackalGuesserHeading");
        guesserGamemodeSidekickIsAlwaysGuesser =
            CustomOption.Create(Types.Guesser, "guesserGamemodeSidekickIsAlwaysGuesser", false);
        guesserForceThiefGuesser = CustomOption.Create(Types.Guesser, "guesserForceThiefGuesser", false);
        guesserGamemodeHaveModifier = CustomOption.Create(Types.Guesser, "guesserGamemodeHaveModifier", true,
            null, true, heading: "guesserGamemodeHaveModifierHeading");
        guesserGamemodeNumberOfShots =
            CustomOption.Create(Types.Guesser, "guesserGamemodeNumberOfShots", 3f, 1f, 15f, 1f);
        guesserGamemodeHasMultipleShotsPerMeeting = CustomOption.Create(Types.Guesser,
            "guesserGamemodeHasMultipleShotsPerMeeting", false);
        guesserGamemodeCrewGuesserNumberOfTasks = CustomOption.Create(Types.Guesser,
            "guesserGamemodeCrewGuesserNumberOfTasks", 0f, 0f, 15f, 1f);
        guesserGamemodeKillsThroughShield =
            CustomOption.Create(Types.Guesser, "guesserGamemodeKillsThroughShield", true);
        guesserGamemodeEvilCanKillSpy =
            CustomOption.Create(Types.Guesser, "guesserGamemodeEvilCanKillSpy", true);
        guesserGamemodeCantGuessSnitchIfTaksDone = CustomOption.Create(Types.Guesser,
            "guesserGamemodeCantGuessSnitchIfTaksDone", true);

        // Hide N Seek Gamemode (3000 - 3999)
        hideNSeekMap = CustomOption.Create(Types.HideNSeekMain, cs(Color.yellow, "hideNSeekMap"),
            new[] { "The Skeld", "Mira", "Polus", "Airship", "Fungle", "Submerged", "LI Map" }, null, true, () =>
            {
                var map = hideNSeekMap.selection;
                if (map >= 3) map++;
                GameOptionsManager.Instance.currentNormalGameOptions.MapId = (byte)map;
            });
        hideNSeekHunterCount = CustomOption.Create(Types.HideNSeekMain, cs(Color.yellow, "hideNSeekHunterCount"), 1f,
            1f, 3f, 1f);
        hideNSeekKillCooldown = CustomOption.Create(Types.HideNSeekMain, cs(Color.yellow, "hideNSeekKillCooldown"), 10f,
            2.5f, 60f, 2.5f);
        hideNSeekHunterVision = CustomOption.Create(Types.HideNSeekMain, cs(Color.yellow, "hideNSeekHunterVision"), 0.5f,
            0.25f, 2f, 0.25f);
        hideNSeekHuntedVision = CustomOption.Create(Types.HideNSeekMain, cs(Color.yellow, "hideNSeekHuntedVision"), 2f,
            0.25f, 5f, 0.25f);
        hideNSeekCommonTasks =
            CustomOption.Create(Types.HideNSeekMain, cs(Color.yellow, "hideNSeekCommonTasks"), 1f, 0f, 4f, 1f);
        hideNSeekShortTasks =
            CustomOption.Create(Types.HideNSeekMain, cs(Color.yellow, "hideNSeekShortTasks"), 3f, 1f, 23f, 1f);
        hideNSeekLongTasks =
            CustomOption.Create(Types.HideNSeekMain, cs(Color.yellow, "hideNSeekLongTasks"), 3f, 0f, 15f, 1f);
        hideNSeekTimer =
            CustomOption.Create(Types.HideNSeekMain, cs(Color.yellow, "hideNSeekTimer"), 5f, 1f, 30f, 0.5f);
        hideNSeekTaskWin =
            CustomOption.Create(Types.HideNSeekMain, cs(Color.yellow, "hideNSeekTaskWin"), false);
        hideNSeekTaskPunish = CustomOption.Create(Types.HideNSeekMain,
            cs(Color.yellow, "hideNSeekTaskPunish"), 10f, 0f, 30f, 1f);
        hideNSeekCanSabotage =
            CustomOption.Create(Types.HideNSeekMain, cs(Color.yellow, "hideNSeekCanSabotage"), false);
        hideNSeekHunterWaiting = CustomOption.Create(Types.HideNSeekMain,
            cs(Color.yellow, "hideNSeekHunterWaiting"), 15f, 2.5f, 60f, 2.5f);

        hunterLightCooldown = CustomOption.Create(Types.HideNSeekRoles, cs(Color.red, "hunterLightCooldown"),
            30f, 5f, 60f, 1f, null, true, heading: "hunterLightCooldownHeading");
        hunterLightDuration = CustomOption.Create(Types.HideNSeekRoles, cs(Color.red, "hunterLightDuration"),
            5f, 1f, 60f, 1f);
        hunterLightVision = CustomOption.Create(Types.HideNSeekRoles, cs(Color.red, "hunterLightVision"), 3f,
            1f, 5f, 0.25f);
        hunterLightPunish = CustomOption.Create(Types.HideNSeekRoles, cs(Color.red, "hunterLightPunish"),
            5f, 0f, 30f, 1f);
        hunterAdminCooldown = CustomOption.Create(Types.HideNSeekRoles, cs(Color.red, "hunterAdminCooldown"),
            30f, 5f, 60f, 1f);
        hunterAdminDuration = CustomOption.Create(Types.HideNSeekRoles, cs(Color.red, "hunterAdminDuration"),
            5f, 1f, 60f, 1f);
        hunterAdminPunish = CustomOption.Create(Types.HideNSeekRoles, cs(Color.red, "hunterAdminPunish"),
            5f, 0f, 30f, 1f);
        hunterArrowCooldown = CustomOption.Create(Types.HideNSeekRoles, cs(Color.red, "hunterArrowCooldown"),
            30f, 5f, 60f, 1f);
        hunterArrowDuration = CustomOption.Create(Types.HideNSeekRoles, cs(Color.red, "hunterArrowDuration"),
            5f, 0f, 60f, 1f);
        hunterArrowPunish = CustomOption.Create(Types.HideNSeekRoles, cs(Color.red, "hunterArrowPunish"),
            5f, 0f, 30f, 1f);

        huntedShieldCooldown = CustomOption.Create(Types.HideNSeekRoles, cs(Color.gray, "huntedShieldCooldown"),
            30f, 5f, 60f, 1f, null, true, heading: "huntedShieldCooldownHeading");
        huntedShieldDuration = CustomOption.Create(Types.HideNSeekRoles, cs(Color.gray, "huntedShieldDuration"),
            5f, 1f, 60f, 1f);
        huntedShieldRewindTime = CustomOption.Create(Types.HideNSeekRoles, cs(Color.gray, "huntedShieldRewindTime"),
            3f, 1f, 10f, 1f);
        huntedShieldNumber = CustomOption.Create(Types.HideNSeekRoles, cs(Color.gray, "huntedShieldNumber"), 3f,
            1f, 15f, 1f);

        // Prop Hunt General Options
        propHuntMap = CustomOption.Create(Types.PropHunt, cs(Color.yellow, "hideNSeekMap"),
            new[] { "The Skeld", "Mira", "Polus", "Airship", "Fungle", "Submerged", "LI Map" }, null, true, () =>
            {
                var map = propHuntMap.selection;
                if (map >= 3) map++;
                GameOptionsManager.Instance.currentNormalGameOptions.MapId = (byte)map;
            });
        propHuntTimer = CustomOption.Create(Types.PropHunt, cs(Color.yellow, "propHuntTimer"), 5f, 1f, 30f, 0.5f,
            null, true, heading: "propHuntTimerHeading");
        propHuntUnstuckCooldown = CustomOption.Create(Types.PropHunt, cs(Color.yellow, "propHuntUnstuckCooldown"), 30f,
            2.5f, 60f, 2.5f);
        propHuntUnstuckDuration =
            CustomOption.Create(Types.PropHunt, cs(Color.yellow, "propHuntUnstuckDuration"), 2f, 1f, 60f, 1f);
        propHunterVision = CustomOption.Create(Types.PropHunt, cs(Color.yellow, "propHunterVision"), 0.5f, 0.25f, 2f,
            0.25f);
        propVision = CustomOption.Create(Types.PropHunt, cs(Color.yellow, "propVision"), 2f, 0.25f, 5f, 0.25f);
        // Hunter Options
        propHuntNumberOfHunters = CustomOption.Create(Types.PropHunt, cs(Color.red, "propHuntNumberOfHunters"), 1f, 1f,
            5f, 1f, null, true, heading: "propHuntNumberOfHuntersHeading");
        hunterInitialBlackoutTime = CustomOption.Create(Types.PropHunt,
            cs(Color.red, "hunterInitialBlackoutTime"), 10f, 5f, 20f, 1f);
        hunterMissCooldown = CustomOption.Create(Types.PropHunt, cs(Color.red, "hunterMissCooldown"), 10f,
            2.5f, 60f, 2.5f);
        hunterHitCooldown = CustomOption.Create(Types.PropHunt, cs(Color.red, "hunterHitCooldown"), 10f,
            2.5f, 60f, 2.5f);
        propHuntRevealCooldown = CustomOption.Create(Types.PropHunt, cs(Color.red, "propHuntRevealCooldown"), 30f,
            10f, 90f, 2.5f);
        propHuntRevealDuration =
            CustomOption.Create(Types.PropHunt, cs(Color.red, "propHuntRevealDuration"), 5f, 1f, 60f, 1f);
        propHuntRevealPunish =
            CustomOption.Create(Types.PropHunt, cs(Color.red, "propHuntRevealPunish"), 10f, 0f, 1800f, 5f);
        propHuntAdminCooldown = CustomOption.Create(Types.PropHunt, cs(Color.red, "propHuntAdminCooldown"), 30f,
            2.5f, 1800f, 2.5f);
        propHuntFindCooldown =
            CustomOption.Create(Types.PropHunt, cs(Color.red, "propHuntFindCooldown"), 60f, 2.5f, 1800f, 2.5f);
        propHuntFindDuration =
            CustomOption.Create(Types.PropHunt, cs(Color.red, "propHuntFindDuration"), 5f, 1f, 15f, 1f);
        // Prop Options
        propBecomesHunterWhenFound = CustomOption.Create(Types.PropHunt,
            cs(Palette.CrewmateBlue, "propBecomesHunterWhenFound"), false, null, true, heading: "propBecomesHunterWhenFoundHeading");
        propHuntInvisEnabled = CustomOption.Create(Types.PropHunt,
            cs(Palette.CrewmateBlue, "propHuntInvisEnabled"), true, null, true);
        propHuntInvisCooldown = CustomOption.Create(Types.PropHunt,
            cs(Palette.CrewmateBlue, "propHuntInvisCooldown"), 120f, 10f, 1800f, 2.5f, propHuntInvisEnabled);
        propHuntInvisDuration = CustomOption.Create(Types.PropHunt,
            cs(Palette.CrewmateBlue, "propHuntInvisDuration"), 5f, 1f, 30f, 1f, propHuntInvisEnabled);
        propHuntSpeedboostEnabled = CustomOption.Create(Types.PropHunt,
            cs(Palette.CrewmateBlue, "propHuntSpeedboostEnabled"), true, null, true);
        propHuntSpeedboostCooldown = CustomOption.Create(Types.PropHunt,
            cs(Palette.CrewmateBlue, "propHuntSpeedboostCooldown"), 60f, 2.5f, 1800f, 2.5f, propHuntSpeedboostEnabled);
        propHuntSpeedboostDuration = CustomOption.Create(Types.PropHunt,
            cs(Palette.CrewmateBlue, "propHuntSpeedboostDuration"), 5f, 1f, 15f, 1f, propHuntSpeedboostEnabled);
        propHuntSpeedboostSpeed = CustomOption.Create(Types.PropHunt,
            cs(Palette.CrewmateBlue, "propHuntSpeedboostSpeed"), 2f, 1.25f, 5f, 0.25f, propHuntSpeedboostEnabled);


        // Other options
        maxNumberOfMeetings = CustomOption.Create(Types.General, "maxNumberOfMeetings", 10,
            0, 15, 1, null, true, heading: "maxNumberOfMeetingsHeading");
        anyPlayerCanStopStart = CustomOption.Create(Types.General,
            cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "anyPlayerCanStopStart"), false);
        blockSkippingInEmergencyMeetings =
            CustomOption.Create(Types.General, "blockSkippingInEmergencyMeetings", false);
        noVoteIsSelfVote = CustomOption.Create(Types.General, "noVoteIsSelfVote", false,
            blockSkippingInEmergencyMeetings);
        hidePlayerNames = CustomOption.Create(Types.General, "hidePlayerNames", false);
        allowParallelMedBayScans = CustomOption.Create(Types.General, "allowParallelMedBayScans", false);
        shieldFirstKill = CustomOption.Create(Types.General, "shieldFirstKill", false);
        finishTasksBeforeHauntingOrZoomingOut =
            CustomOption.Create(Types.General, "finishTasksBeforeHauntingOrZoomingOut", true);
        deadImpsBlockSabotage = CustomOption.Create(Types.General, "deadImpsBlockSabotage", false);
        camsNightVision = CustomOption.Create(Types.General, "camsNightVision", false,
            null, true, heading: "camsNightVisionHeading");
        camsNoNightVisionIfImpVision = CustomOption.Create(Types.General,
            "camsNoNightVisionIfImpVision", false, camsNightVision);


        dynamicMap = CustomOption.Create(Types.General, "dynamicMap", false, null, true,
            heading: "dynamicMapHeading");
        dynamicMapEnableSkeld = CustomOption.Create(Types.General, "dynamicMapEnableSkeld", rates, dynamicMap);
        dynamicMapEnableSkeld = CustomOption.Create(Types.General, "Skeld", rates, dynamicMap);
        dynamicMapEnableMira = CustomOption.Create(Types.General, "Mira", rates, dynamicMap);
        dynamicMapEnablePolus = CustomOption.Create(Types.General, "Polus", rates, dynamicMap);
        dynamicMapEnableAirShip = CustomOption.Create(Types.General, "Airship", rates, dynamicMap);
        dynamicMapEnableFungle = CustomOption.Create(Types.General, "Fungle", rates, dynamicMap);
        dynamicMapEnableSubmerged = CustomOption.Create(Types.General, "Submerged", rates, dynamicMap);
        dynamicMapSeparateSettings =
            CustomOption.Create(Types.General, "dynamicMapSeparateSettings", false, dynamicMap);

        blockedRolePairings.Add((byte)RoleId.Vampire, new[] { (byte)RoleId.Warlock });
        blockedRolePairings.Add((byte)RoleId.Warlock, new[] { (byte)RoleId.Vampire });
        blockedRolePairings.Add((byte)RoleId.Spy, new[] { (byte)RoleId.Mini });
        blockedRolePairings.Add((byte)RoleId.Mini, new[] { (byte)RoleId.Spy });
        blockedRolePairings.Add((byte)RoleId.Vulture, new[] { (byte)RoleId.Cleaner });
        blockedRolePairings.Add((byte)RoleId.Cleaner, new[] { (byte)RoleId.Vulture });
    }
}