using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csnxs.DeACC
{
    public enum OpcodeEnum
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

        LsScriptVar,
        LsMapVar,
        LsWorldVar,
        LsGlobalVar,

        LsMapArray,
        LsWorldArray,
        LsGlobalArray,
       
        RsScriptVar,
        RsMapVar,
        RsWorldVar,
        RsGlobalVar,

        RsMapArray,
        RsWorldArray,
        RsGlobalArray,
        
        GetPlayerInfo,

        ChangeLevel,

        SectorDamage,

        ReplaceTextures,

        NegateBinary,

        GetActorPitch,
        SetActorPitch,

        PrintBind,

        SetActorState,

        ThingDamage2,

        UseInventory,
        UseActorInventory,

        CheckActorCeilingTexture,

        CheckActorFloorTexture,

        GetActorLightLevel,

        SetMugShotState,

        ThingCountSector,
        ThingCountNameSector,

        CheckPlayerCamera,

        MorphActor,
        UnMorphActor,

        ClassifyActor,

        PrintBinary,
        PrintHex,

        CallFunc,

        SaveString,

        PrintMapChRange,
        PrintWorldChRange,
        PrintGlobalChRange,

        StrCpyToMapChRange,
        StrCpyToWorldChRange,
        StrCpyToGlobalChRange,

        PushFunction,
        CallStack,

        ScriptWaitNamed,

        TranslationRange3
    }

    public struct AcsOpcode
    {
        public string Name { get; set; }
        public int NumberOfArguments;
        public bool FirstArgumentIsByte;
        public bool AllArgsAreByte;
    }

    class AcsInstruction
    {
        public static readonly AcsOpcode[] Opcodes = {
            new AcsOpcode {Name = "Nop",                      NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Terminate",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Suspend",                  NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PushNumber",               NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LSpec1",                   NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LSpec2",                   NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LSpec3",                   NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LSpec4",                   NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LSpec5",                   NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LSpec1Direct",             NumberOfArguments = 2, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LSpec2Direct",             NumberOfArguments = 3, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LSpec3Direct",             NumberOfArguments = 4, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LSpec4Direct",             NumberOfArguments = 5, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LSpec5Direct",             NumberOfArguments = 6, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Add",                      NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Subtract",                 NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Multiply",                 NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Divide",                   NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Modulus",                  NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Eq",                       NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Ne",                       NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Lt",                       NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Gt",                       NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Le",                       NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Ge",                       NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "AssignScriptVar",          NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "AssignMapVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "AssignWorldVar",           NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "PushScriptVar",            NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "PushMapVar",               NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "PushWorldVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "AddScriptVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "AddMapVar",                NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "AddWorldVar",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "SubScriptVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "SubMapVar",                NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "SubWorldVar",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "MulScriptVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "MulMapVar",                NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "MulWorldVar",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "DivScriptVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "DivMapVar",                NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "DivWorldVar",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "ModScriptVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "ModMapVar",                NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "ModWorldVar",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "IncScriptVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "IncMapVar",                NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "IncWorldVar",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "DecScriptVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "DecMapVar",                NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "DecWorldVar",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "Goto",                     NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "IfGoto",                   NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Drop",                     NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Delay",                    NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "DelayDirect",              NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Random",                   NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "RandomDirect",             NumberOfArguments = 2, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ThingCount",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ThingCountDirect",         NumberOfArguments = 2, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "TagWait",                  NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "TagWaitDirect",            NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PolyWait",                 NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PolyWaitDirect",           NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ChangeFloor",              NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ChangeFloorDirect",        NumberOfArguments = 2, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ChangeCeiling",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ChangeCeilingDirect",      NumberOfArguments = 2, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Restart",                  NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "AndLogical",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "OrLogical",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "AndBitwise",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "OrBitwise",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "EorBitwise",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "NegateLogical",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LShift",                   NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "RShift",                   NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "UnaryMinus",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "IfNotGoto",                NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LineSide",                 NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ScriptWait",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ScriptWaitDirect",         NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ClearLineSpecial",         NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "CaseGoto",                 NumberOfArguments = 2, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "BeginPrint",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "EndPrint",                 NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PrintString",              NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PrintNumber",              NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PrintCharacter",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerCount",              NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GameType",                 NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GameSkill",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Timer",                    NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SectorSound",              NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "AmbientSound",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SoundSequence",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetLineTexture",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetLineBlocking",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetLineSpecial",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ThingSound",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "EndPrintBold",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ActivatorSound",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LocalAmbientSound",        NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetLineMonsterBlocking",   NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerBlueSkull",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerRedSkull",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerYellowSkull",        NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerMasterSkull",        NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerRedCard",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerYellowCard",         NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerMasterCard",         NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerBlackSkull",         NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerSilverSkull",        NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerGoldSkull",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerBlackCard",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerSilverCard",         NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerOnTeam",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerTeam",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerHealth",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerArmorPoints",        NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerFrags",              NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerExpert",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "BlueTeamCount",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "RedTeamCount",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "BlueTeamScore",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "RedTeamScore",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "IsOneFlagCTF",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetInvasionWave",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetInvasionState",         NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PrintName",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "MusicChange",              NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ConsoleCommandDirect",     NumberOfArguments = 3, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ConsoleCommand",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SinglePlayer",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "FixedMul",                 NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "FixedDiv",                 NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetGravity",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetGravityDirect",         NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetAirControl",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetAirControlDirect",      NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ClearInventory",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GiveInventory",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GiveInventoryDirect",      NumberOfArguments = 2, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "TakeInventory",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "TakeInventoryDirect",      NumberOfArguments = 2, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "CheckInventory",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "CheckInventoryDirect",     NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Spawn",                    NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SpawnDirect",              NumberOfArguments = 6, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SpawnSpot",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SpawnSpotDirect",          NumberOfArguments = 4, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetMusic",                 NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetMusicDirect",           NumberOfArguments = 3, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LocalSetMusic",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LocalSetMusicDirect",      NumberOfArguments = 3, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PrintFixed",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PrintLocalized",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "MoreHudMessage",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "OptHudMessage",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "EndHudMessage",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "EndHudMessageBold",        NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetStyle",                 NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetStyleDirect",           NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetFont",                  NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetFontDirect",            NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PushByte",                 NumberOfArguments = 1, FirstArgumentIsByte = true },
            new AcsOpcode {Name = "LSpec1DirectB",            NumberOfArguments = 1, FirstArgumentIsByte = true,  AllArgsAreByte = true},
            new AcsOpcode {Name = "LSpec2DirectB",            NumberOfArguments = 2, FirstArgumentIsByte = true,  AllArgsAreByte = true},
            new AcsOpcode {Name = "LSpec3DirectB",            NumberOfArguments = 3, FirstArgumentIsByte = true,  AllArgsAreByte = true},
            new AcsOpcode {Name = "LSpec4DirectB",            NumberOfArguments = 4, FirstArgumentIsByte = true,  AllArgsAreByte = true},
            new AcsOpcode {Name = "LSpec5DirectB",            NumberOfArguments = 5, FirstArgumentIsByte = true,  AllArgsAreByte = true},
            new AcsOpcode {Name = "DelayDirectB",             NumberOfArguments = 1, FirstArgumentIsByte = true,  AllArgsAreByte = true},
            new AcsOpcode {Name = "RandomDirectB",            NumberOfArguments = 2, FirstArgumentIsByte = true,  AllArgsAreByte = true},
            new AcsOpcode {Name = "PushBytes",                NumberOfArguments = 1, FirstArgumentIsByte = true,  AllArgsAreByte = true},
            new AcsOpcode {Name = "Push2Bytes",               NumberOfArguments = 2, FirstArgumentIsByte = true,  AllArgsAreByte = true},
            new AcsOpcode {Name = "Push3Bytes",               NumberOfArguments = 3, FirstArgumentIsByte = true,  AllArgsAreByte = true},
            new AcsOpcode {Name = "Push4Bytes",               NumberOfArguments = 4, FirstArgumentIsByte = true,  AllArgsAreByte = true},
            new AcsOpcode {Name = "Push5Bytes",               NumberOfArguments = 5, FirstArgumentIsByte = true,  AllArgsAreByte = true},
            new AcsOpcode {Name = "SetThingSpecial",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "AssignGlobalVar",          NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "PushGlobalVar",            NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "AddGlobalVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "SubGlobalVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "MulGlobalVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "DivGlobalVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "ModGlobalVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "IncGlobalVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "DecGlobalVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "FadeTo",                   NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "FadeRange",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "CancelFade",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayMovie",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetFloorTrigger",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetCeilingTrigger",        NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetActorX",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetActorY",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetActorZ",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "StartTranslation",         NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "TranslationRange1",        NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "TranslationRange2",        NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "EndTranslation",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Call",                     NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "CallDiscard",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "ReturnVoid",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ReturnVal",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PushMapArray",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "AssignMapArray",           NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "AddMapArray",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "SubMapArray",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "DivMapArray",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "ModMapArray",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "DecMapArray",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "dup",                      NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "swap",                     NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "WriteToIni",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetFromIni",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Sin",                      NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "Cos",                      NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "VectorAngle",              NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "CheckWeapon",              NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetWeapon",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "TagString",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PushWorldArray",           NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "AssignWorldArray",         NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "AddWorldArray",            NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "SubWorldArray",            NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "MulWorldArray",            NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "DivWorldArray",            NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "ModWorldArray",            NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "IncWorldArray",            NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "DecWorldArray",            NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "AddGlobalArray",           NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "SubGlobalArray",           NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "MulGlobalArray",           NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "DivGlobalArray",           NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "ModGlobalArray",           NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "IncGlobalArray",           NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "DecGlobalArray",           NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "SetMarineWeapon",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetActorProperty",         NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetActorProperty",         NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerNumber",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ActivatorTid",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetMarineSprite",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetScreenWidth",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetScreenHeight",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ThingProjectile2",         NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "StrLen",                   NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetHudSize",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetCvar",                  NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "CaseGotoSorted",           NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetResultValue",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetLineRowOffset",         NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetActorFloorZ",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetActorAngle",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetSectorFloorZ",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetSectorCeilingZ",        NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "LSpec5Result",             NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetSigilPieces",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetLevelInfo",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ChangeSky",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerInGame",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerIsBot",              NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetCameraToTexture",       NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "EndLog",                   NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetAmmoCapacity",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetAmmoCapacity",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PrintMapCharArray",        NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PrintWorldCharArray",      NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PrintGlobalCharArray",     NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetActorAngle",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GrabInput",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetMousePointer",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "MoveMousePoiner",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SpawnProjectile",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetSectorLightLevel",      NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetActorCeilingZ",         NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetActorPosition",         NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ClearActorInventory",      NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GiveActorInventory",       NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "TakeActorInventory",       NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "CheckActorInventory",      NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ThingCountName",           NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SpawnSpotFacing",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PlayerClass",              NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "AndScriptVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "AndMapVar",                NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "AndWorldVar",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "AndGlobalVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "AndMapArray",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "AndWorldArray",            NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "AndGlobalArray",           NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "EorScriptVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "EorMapVar",                NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "EorWorldVar",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "EorGlobalVar",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "EorMapArray",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "EorWorldArray",            NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "EorGlobalArary",           NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "OrScriptVar",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "OrMapVar",                 NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "OrWorldVar",               NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "OrGlobalVar",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "OrMapArray",               NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "OrWorldArray",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "OrGlobalArray",            NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "LsScriptVar",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "LsMapVar",                 NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "LsWorldVar",               NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "LsGlobalVar",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "LsMapArray",               NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "LsWorldArray",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "LsGlobalArray",            NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "RsScriptVar",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "RsMapVar",                 NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "RsWorldVar",               NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "RsGlobalVar",              NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "RsMapArray",               NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "RsWorldArray",             NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "RsGlobalArray",            NumberOfArguments = 1, FirstArgumentIsByte = true},
            new AcsOpcode {Name = "GetPlayerInfo",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ChangeLevel",              NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SectorDamage",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ReplaceTextures",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "NegateBinary",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetActorPitch",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetActorPitch",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PrintBind",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetActorState",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ThingDamage2",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "UseInventory",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "UseActorInventory",        NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "CheckActorCeilingTexture", NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "CheckActorFloorTexture",   NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "GetActorLightLevel",       NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SetMugShotState",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ThingCountSector",         NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ThingCountNameSector",     NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "CheckPlayerCamera",        NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "MorphActor",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "UnMorphActor",             NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ClassifyActor",            NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PrintBinary",              NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PrintHex",                 NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "CallFunc",                 NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "SaveString",               NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PrintMapChRange",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PrintWorldChRange",        NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PrintGlobalChRange",       NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "StrCpyToMapChRange",       NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "StrCpyToWorldChRange",     NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "StrCpyToGlobalChRange",    NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "PushFunction",             NumberOfArguments = 1, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "CallStack",                NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "ScriptWaitNamed",          NumberOfArguments = 0, FirstArgumentIsByte = false},
            new AcsOpcode {Name = "TranslationRange3",        NumberOfArguments = 0, FirstArgumentIsByte = false},
        };

        public AcsOpcode Opcode { get; private set; }
        public int[] Arguments { get; private set; }

        private AcsInstruction(AcsOpcode opcode, int[] args)
        {
            this.Opcode = opcode;
            this.Arguments = args;
        }

        public static AcsInstruction[] ReadCode(AcsFile file, ref BinaryReader reader)
        {
            List<AcsInstruction> instructions = new List<AcsInstruction>();

            AcsOpcode opcode;

            while (!OpcodesAreEqual((opcode = ReadOpcode(ref reader, (file.Format == AcsFormat.Acs95))), Opcodes[(int)OpcodeEnum.Terminate]))
            {
                List<int> args = new List<int>(opcode.NumberOfArguments);
                int i = 0;

                if (opcode.FirstArgumentIsByte)
                {
                    args.Add(reader.ReadByte());
                    i++;
                }

                for (; i < opcode.NumberOfArguments; i++)
                {
                    if (opcode.AllArgsAreByte)
                    {
                        args.Add(reader.ReadByte());
                    }
                    else
                    {
                        args.Add(reader.ReadInt32());
                    }
                }

                AcsInstruction instruction = new AcsInstruction(opcode, args.ToArray());
                instructions.Add(instruction);
            }

            return instructions.ToArray();
        }

        public static AcsOpcode ReadOpcode(ref BinaryReader reader, bool longFormat)
        {
            if (longFormat)
            {
                return Opcodes[reader.ReadInt32()];
            }
            else
            {
                int n;
                byte one = reader.ReadByte();
                if (one >= 240)
                {
                    n = 240 + ((one - 240) << 8) + reader.ReadByte();
                }
                else
                {
                    n = one;
                }

                return Opcodes[n];
            }
        }

        private static bool OpcodesAreEqual(AcsOpcode one, AcsOpcode two)
        {
            return (one.Name == two.Name);
        }
    }
}
