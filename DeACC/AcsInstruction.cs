using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csnxs.DeACC
{
    public enum Opcode
    {
        Nop,
        Terminate,
        Suspend,

        PushNumber,

        LSpec1,
        LSpec2,
        LSpec3,
        LSpec4,
        LSpec5,
        
        LSpec1Direct,
        LSpec2Direct,
        LSpec3Direct,
        LSpec4Direct,
        LSpec5Direct,

        Add,
        Subtract,
        Multiply,
        Divide,
        Modulus,

        Eq,
        Ne,
        Lt,
        Gt,
        Le,
        Ge,

        AssignScriptVar,
        AssignMapVar,
        AssignWorldVar,

        PushScriptVar,
        PushMapVar,
        PushWorldVar,

        AddScriptVar,
        AddMapVar,
        AddWorldVar,

        SubScriptVar,
        SubMapVar,
        SubWorldVar,

        MulScriptVar,
        MulMapVar,
        MulWorldVar,

        DivScriptVar,
        DivMapVar,
        DivWorldVar,

        ModScriptVar,
        ModMapVar,
        ModWorldVar,

        IncScriptVar,
        IncMapVar,
        IncWorldVar,

        DecScriptVar,
        DecMapVar,
        DecWorldVar,

        Goto,
        IfGoto,

        Drop,

        Delay,
        DelayDirect,

        Random,
        RandomDirect,

        ThingCount,
        ThingCountDirect,

        TagWait,
        TagWaitDirect,

        PolyWait,
        PolyWaitDirect,

        ChangeFloor,
        ChangeFloorDirect,

        ChangeCeiling,
        ChangeCeilingDirect,

        Restart,

        AndLogical,
        OrLogical,

        AndBitwise,
        OrBitwise,
        EorBitwise,

        NegateLogical,

        LShift,
        RShift,

        UnaryMinus,

        IfNotGoto,

        LineSide,

        ScriptWait,
        ScriptWaitDirect,

        ClearLineSpecial,

        CaseGoto,

        BeginPrint,
        EndPrint,
        PrintString,
        PrintNumber,
        PrintCharacter,

        PlayerCount,

        GameType,
        GameSkill,

        Timer,

        SectorSound,

        AmbientSound,
        SoundSequence,

        SetLineTexture,
        SetLineBlocking,
        SetLineSpecial,

        ThingSound,

        EndPrintBold,

        // ZDoom
        ActivatorSound,
        LocalAmbientSound,

        SetLineMonsterBlocking,

        // Skulltag
        PlayerBlueSkull,
        PlayerRedSkull,
        PlayerYellowSkull,
        PlayerMasterSkull,

        PlayerRedCard,
        PlayerYellowCard,
        PlayerMasterCard,

        PlayerBlackSkull,
        PlayerSilverSkull,
        PlayerGoldSkull,

        PlayerBlackCard,
        PlayerSilverCard,

        PlayerOnTeam,

        PlayerTeam,
        PlayerHealth,
        PlayerArmorPoints,
        PlayerFrags,

        PlayerExpert,

        BlueTeamCount,
        RedTeamCount,

        BlueTeamScore,
        RedTeamScore,

        IsOneFlagCTF,

        GetInvasionWave,
        GetInvasionState,

        PrintName,

        MusicChange,

        ConsoleCommandDirect,
        ConsoleCommand,

        SinglePlayer,

        // ZDoom
        FixedMul,
        FixedDiv,

        SetGravity,
        SetGravityDirect,

        SetAirControl,
        SetAirControlDirect,

        ClearInventory,
        GiveInventory,
        GiveInventoryDirect,
        TakeInventory,
        TakeInventoryDirect,
        CheckInventory,
        CheckInventoryDirect,

        Spawn,
        SpawnDirect,

        SpawnSpot,
        SpawnSpotDirect,

        SetMusic,
        SetMusicDirect,

        LocalSetMusic,
        LocalSetMusicDirect,

        PrintFixed,
        PrintLocalized,

        MoreHudMessage,
        OptHudMessage,
        EndHudMessage,
        EndHudMessageBold,

        SetStyle,
        SetStyleDirect,

        SetFont,
        SetFontDirect,

        PushByte,

        LSpec1DirectB,
        LSpec2DirectB,
        LSpec3DirectB,
        LSpec4DirectB,
        LSpec5DirectB,

        DelayDirectB,
        
        RandomDirectB,

        PushBytes,
        Push2Bytes,
        Push3Bytes,
        Push4Bytes,
        Push5Bytes,

        SetThingSpecial,

        AssignGlobalVar,
        PushGlobalVar,
        AddGlobalVar,
        SubGlobalVar,
        MulGlobalVar,
        DivGlobalVar,
        ModGlobalVar,
        IncGlobalVar,
        DecGlobalVar,

        FadeTo,
        FadeRange,
        CancelFade,

        PlayMovie,

        SetFloorTrigger,
        SetCeilingTrigger,

        GetActorX,
        GetActorY,
        GetActorZ,

        StartTranslation,

        TranslationRange1,
        TranslationRange2,

        EndTranslation,

        Call,
        CallDiscard,

        ReturnVoid,
        ReturnVal,

        PushMapArray,
        AssignMapArray,
        AddMapArray,
        SubMapArray,
        DivMapArray,
        ModMapArray,
        DecMapArray,

        dup,
        swap,

        WriteToIni,
        GetFromIni,

        Sin,
        Cos,

        VectorAngle,

        CheckWeapon,
        SetWeapon,

        TagString,

        PushWorldArray,
        AssignWorldArray,
        AddWorldArray,
        SubWorldArray,
        MulWorldArray,
        DivWorldArray,
        ModWorldArray,
        IncWorldArray,
        DecWorldArray,

        AddGlobalArray,
        SubGlobalArray,
        MulGlobalArray,
        DivGlobalArray,
        ModGlobalArray,
        IncGlobalArray,
        DecGlobalArray,

        SetMarineWeapon,
        
        SetActorProperty,
        GetActorProperty,

        PlayerNumber,
        ActivatorTid,

        SetMarineSprite,

        GetScreenWidth,
        GetScreenHeight,

        ThingProjectile2,

        StrLen,
        
        SetHudSize,

        GetCvar,

        CaseGotoSorted,

        SetResultValue,

        GetLineRowOffset,

        GetActorFloorZ,
        GetActorAngle,

        GetSectorFloorZ,
        GetSectorCeilingZ,

        LSpec5Result,

        GetSigilPieces,

        GetLevelInfo,

        ChangeSky,

        PlayerInGame,
        PlayerIsBot,

        SetCameraToTexture,

        EndLog,

        GetAmmoCapacity,
        SetAmmoCapacity,

        PrintMapCharArray,
        PrintWorldCharArray,
        PrintGlobalCharArray,

        SetActorAngle,

        GrabInput,
        SetMousePointer,
        MoveMousePoiner,

        SpawnProjectile,

        GetSectorLightLevel,

        GetActorCeilingZ,

        SetActorPosition,

        ClearActorInventory,
        GiveActorInventory,
        TakeActorInventory,
        CheckActorInventory,

        ThingCountName,

        SpawnSpotFacing,

        PlayerClass,

        AndScriptVar,
        AndMapVar,
        AndWorldVar,
        AndGlobalVar,

        AndMapArray,
        AndWorldArray,
        AndGlobalArray,

        EorScriptVar,
        EorMapVar,
        EorWorldVar,
        EorGlobalVar,

        EorMapArray,
        EorWorldArray,
        EorGlobalArary,

        OrScriptVar,
        OrMapVar,
        OrWorldVar,
        OrGlobalVar,

        OrMapArray,
        OrWorldArray,
        OrGlobalArray,
    }

    class AcsInstruction
    {
    }
}
