﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csnxs.DeACC
{
    /// <summary>
    /// ACS built-in functions.
    /// Literally just the ACSF_ enum from p_acs.cpp in ZDoom/Zan source with the ACSF_ prefix removed.
    /// </summary>
    internal enum AcsBuiltIn
    {
        GetLineUDMFInt = 1,
        GetLineUDMFFixed,
        GetThingUDMFInt,
        GetThingUDMFFixed,
        GetSectorUDMFInt,
        GetSectorUDMFFixed,
        GetSideUDMFInt,
        GetSideUDMFFixed,
        GetActorVelX,
        GetActorVelY,
        GetActorVelZ,
        SetActivator,
        SetActivatorToTarget,
        GetActorViewHeight,
        GetChar,
        GetAirSupply,
        SetAirSupply,
        SetSkyScrollSpeed,
        GetArmorType,
        SpawnSpotForced,
        SpawnSpotFacingForced,
        CheckActorProperty,
        SetActorVelocity,
        SetUserVariable,
        GetUserVariable,
        Radius_Quake2,
        CheckActorClass,
        SetUserArray,
        GetUserArray,
        SoundSequenceOnActor,
        SoundSequenceOnSector,
        SoundSequenceOnPolyobj,
        GetPolyobjX,
        GetPolyobjY,
        CheckSight,
        SpawnForced,
        AnnouncerSound,    // Skulltag
        SetPointer,
        ACS_NamedExecute,
        ACS_NamedSuspend,
        ACS_NamedTerminate,
        ACS_NamedLockedExecute,
        ACS_NamedLockedExecuteDoor,
        ACS_NamedExecuteWithResult,
        ACS_NamedExecuteAlways,
        UniqueTID,
        IsTIDUsed,
        Sqrt,
        FixedSqrt,
        VectorLength,
        SetHUDClipRect,
        SetHUDWrapWidth,
        SetCVar,
        GetUserCVar,
        SetUserCVar,
        GetCVarString,
        SetCVarString,
        GetUserCVarString,
        SetUserCVarString,
        LineAttack,
        PlaySound,
        StopSound,
        strcmp,
        stricmp,
        StrLeft,
        StrRight,
        StrMid,
        GetActorClass,
        GetWeapon,
        SoundVolume,
        PlayActorSound,
        SpawnDecal,
        CheckFont,
        DropItem,
        CheckFlag,
        SetLineActivation,
        GetLineActivation,
        GetActorPowerupTics,
        ChangeActorAngle,
        ChangeActorPitch,      // 80
        GetArmorInfo,
        DropInventory,
        PickActor,
        IsPointerEqual,
        CanRaiseActor,

        // [BB] Out of order ZDoom backport.
        Warp = 92,

        /* Zandronum's - these must be skipped when we reach 99!
        -100:ResetMap(0),
        -101 : PlayerIsSpectator(1),
        -102 : ConsolePlayerNumber(0),
        -103 : GetTeamProperty(2),
        -104 : GetPlayerLivesLeft(1),
        -105 : SetPlayerLivesLeft(2),
        -106 : KickFromGame(2),
        */

        // [BB] Out of order ZDoom backport.
        GetActorFloorTexture = 204,

        // [BB] Skulltag functions
        ResetMap = 100,
        PlayerIsSpectator,
        ConsolePlayerNumber,
        GetTeamProperty, // [Dusk]
        GetPlayerLivesLeft,
        SetPlayerLivesLeft,
        ForceToSpectate,
        GetGamemodeState,
        SetDBEntry,
        GetDBEntry,
        SetDBEntryString,
        GetDBEntryString,
        IncrementDBEntry,
        PlayerIsLoggedIn,
        GetPlayerAccountName,
        SortDBEntries,
        CountDBResults,
        FreeDBResults,
        GetDBResultKeyString,
        GetDBResultValueString,
        GetDBResultValue,
        GetDBEntryRank,
        RequestScriptPuke,
        BeginDBTransaction,
        EndDBTransaction,
        GetDBEntries,
        NamedRequestScriptPuke,
        SystemTime,
        GetTimeProperty,
        Strftime,
        SetDeadSpectator,
        SetActivatorToPlayer,
        SetCurrentGamemode,
        GetCurrentGamemode,
        SetGamemodeLimit,
        SetPlayerClass,
        SetPlayerChasecam,
        GetPlayerChasecam,
        SetPlayerScore,
        GetPlayerScore,
        InDemoMode,
        SetActionScript,
        SetPredictableValue,
        GetPredictableValue,
        ExecuteClientScript,
        NamedExecuteClientScript,
        SendNetworkString,
        NamedSendNetworkString,
        GetChatMessage,
        GetMapRotationSize,
        GetMapRotationInfo,
        GetCurrentMapPosition,
        GetEventResult,
        GetActorSectorLocation,
        ChangeTeamScore,
        SetGameplaySetting,

        // ZDaemon
        GetTeamScore = 19620,  // (int team)
        SetTeamScore,			// (int team, int value)
    }
}
