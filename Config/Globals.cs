using CounterStrikeSharp.API.Core;
using Newtonsoft.Json.Linq;

namespace Game_Manager_GoldKingZ;

public class Globals_Static
{
    public class PersonData
    {
        public ulong PlayerSteamID { get; set; }
        public int Toggle_AimPunch { get; set; }
        public int Toggle_Custom_MuteSounds1 { get; set; }
        public int Toggle_Custom_MuteSounds2 { get; set; }
        public int Toggle_Custom_MuteSounds3 { get; set; }
        public DateTime DateAndTime { get; set; }
    }

    public static readonly Dictionary<string, (string State, string ChatType)> messageMappings = new()
    {
        { "Cstrike_Chat_All", ("ALIVE", "ALL") },
        { "Cstrike_Chat_AllDead", ("DEAD", "ALL") },
        { "Cstrike_Chat_AllSpec", ("ALIVE", "ALL") },
        { "Cstrike_Chat_CT", ("ALIVE", "TEAM") },
        { "Cstrike_Chat_CT_Loc", ("ALIVE", "TEAM") },
        { "Cstrike_Chat_T", ("ALIVE", "TEAM") },
        { "Cstrike_Chat_T_Loc", ("ALIVE", "TEAM") },
        { "Cstrike_Chat_Spec", ("SPEC", "TEAM") },
        { "Cstrike_Chat_CT_Dead", ("DEAD", "TEAM") },
        { "Cstrike_Chat_T_Dead", ("DEAD", "TEAM") },
    };
    public const uint HIDEWEAPONS = 64;
    public const uint HIDECHAT = 128;
    public const uint CROSSHAIRANDNAMETAGS = 256;
}

public class Globals
{
    public string AntiCrash_StartWith = "";
    public string AntiCrash_Contains = "";
    public string AntiCrash_BlockRadio = "";
    public bool OnTakeDamage_Hooked = false;
    public class PlayerDataClass
    {
        public CCSPlayerController Player { get; set; }
        public CCSPlayerController Attacker { get; set; }
        public string PlayerName { get; set; }
        public int PlayerName_Count { get; set; }
        public bool PlayerName_Block { get; set; }
        public bool PlayerName_Block_Message { get; set; }
        public ulong SteamId { get; set; }
        public int Toggle_AimPunch { get; set; }
        public int Toggle_Custom_MuteSounds1 { get; set; }
        public int Toggle_Custom_MuteSounds2 { get; set; }
        public int Toggle_Custom_MuteSounds3 { get; set; }
        public string MessageType { get; set; }
        public int PlayerAlpha { get; set; }
        public bool StabedHisTeamMate { get; set; }
        public CounterStrikeSharp.API.Modules.Timers.Timer Timer_DeadBody { get; set; }
        public DateTime EventPlayerChat { get; set; }
        public DateTime LastNameChangeTime { get; set; }
        public PlayerDataClass(CCSPlayerController player, CCSPlayerController Attackerr, string PlayerNamee, int PlayerName_Countt, bool PlayerName_Blockk, bool PlayerName_Block_Messagee, ulong steamId, int Toggle_AimPunchh, int Toggle_Custom_MuteSounds11, int Toggle_Custom_MuteSounds22, int Toggle_Custom_MuteSounds33, string MessageTypee, int playerAlpha, bool StabedHisTeamMatee, CounterStrikeSharp.API.Modules.Timers.Timer timer_DeadBody, DateTime EventPlayerChatt, DateTime LastNameChangeTimee)
        {
            Player = player;
            Attacker = Attackerr;
            PlayerName = PlayerNamee;
            PlayerName_Count = PlayerName_Countt;
            PlayerName_Block = PlayerName_Blockk;
            PlayerName_Block_Message = PlayerName_Block_Messagee;
            SteamId = steamId;
            Toggle_AimPunch = Toggle_AimPunchh;
            Toggle_Custom_MuteSounds1 = Toggle_Custom_MuteSounds11;
            Toggle_Custom_MuteSounds2 = Toggle_Custom_MuteSounds22;
            Toggle_Custom_MuteSounds3 = Toggle_Custom_MuteSounds33;
            PlayerAlpha = playerAlpha;
            StabedHisTeamMate = StabedHisTeamMatee;
            MessageType = MessageTypee;
            Timer_DeadBody = timer_DeadBody;
            EventPlayerChat = EventPlayerChatt;
            LastNameChangeTime = LastNameChangeTimee;
        }
    }
    public Dictionary<int, PlayerDataClass> Player_Data = new Dictionary<int, PlayerDataClass>();
    public List<CBaseEntity> CbaseWeapons = new List<CBaseEntity>();
    public JObject? JsonData { get; set; }
    public CounterStrikeSharp.API.Modules.Timers.Timer? TimerCleanUp;
    public CounterStrikeSharp.API.Modules.Timers.Timer? TimerChecker;

    public void Clear()
    {
        CbaseWeapons?.Clear();
        TimerCleanUp?.Kill();
        TimerCleanUp = null;
        TimerChecker?.Kill();
        TimerChecker = null;
    }
}