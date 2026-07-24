using CounterStrikeSharp.API.Core;
using Newtonsoft.Json.Linq;

namespace Game_Manager_GoldKingZ;

public class Globals_Static
{
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
    public const int EFFECT_BLOOD_SPURT_AND_IMPACT_FLESH = 4;
    public const int EFFECT_HEADSHOT_SPARK = 7;
}

public class Globals
{
    private long _reloadIdCounter = 0;
    public long GetNextReloadId() => ++_reloadIdCounter;
    public string AntiCrash_StartWith = "";
    public string AntiCrash_Contains = "";
    public string AntiCrash_BlockRadio = "";
    public bool OnTakeDamage_Hooked = false;
    public bool Downloading_FromGithub = false;
    public class PlayerDataClass
    {
        public CCSPlayerController Player { get; set; }
        public CCSPlayerController Attacker { get; set; }
        public CCSPlayerController Victim { get; set; }
        public string PlayerName { get; set; }
        public int PlayerName_Count { get; set; }
        public bool PlayerName_Block { get; set; }
        public bool PlayerName_Block_Message { get; set; }
        public ulong SteamId { get; set; }
        public string MessageType { get; set; }
        public int PlayerAlpha { get; set; }
        public int AntiFlood_Times { get; set; }
        public bool StabedHisTeamMate { get; set; }
        public CounterStrikeSharp.API.Modules.Timers.Timer Timer_DeadBody { get; set; }
        public DateTime EventPlayerChat { get; set; }
        public DateTime EventPlayerChat_Filter { get; set; }
        public DateTime LastNameChangeTime { get; set; }
        public DateTime AntiFlood_WindowStart { get; set; }
        public DateTime AntiFlood_CountTime { get; set; }
        public DateTime AntiFlood_BlockedUntil { get; set; }
        
        public PlayerDataClass(CCSPlayerController player, CCSPlayerController Attackerr, CCSPlayerController Victimm, string PlayerNamee, int PlayerName_Countt, bool PlayerName_Blockk, bool PlayerName_Block_Messagee, ulong steamId, string MessageTypee, int playerAlpha, int AntiFlood_Timess, bool StabedHisTeamMatee, CounterStrikeSharp.API.Modules.Timers.Timer timer_DeadBody, DateTime EventPlayerChatt, DateTime EventPlayerChat_Filterr, DateTime LastNameChangeTimee, DateTime AntiFlood_WindowStartt, DateTime AntiFlood_CountTimee, DateTime AntiFlood_BlockedUntill)
        {
            Player = player;
            Attacker = Attackerr;
            Victim = Victimm;
            PlayerName = PlayerNamee;
            PlayerName_Count = PlayerName_Countt;
            PlayerName_Block = PlayerName_Blockk;
            PlayerName_Block_Message = PlayerName_Block_Messagee;
            SteamId = steamId;
            PlayerAlpha = playerAlpha;
            AntiFlood_Times = AntiFlood_Timess;
            StabedHisTeamMate = StabedHisTeamMatee;
            MessageType = MessageTypee;
            Timer_DeadBody = timer_DeadBody;
            EventPlayerChat = EventPlayerChatt;
            EventPlayerChat_Filter = EventPlayerChat_Filterr;
            LastNameChangeTime = LastNameChangeTimee;
            AntiFlood_WindowStart = AntiFlood_WindowStartt;
            AntiFlood_CountTime = AntiFlood_CountTimee;
            AntiFlood_BlockedUntil = AntiFlood_BlockedUntill;
        }
    }
    public Dictionary<int, PlayerDataClass> Player_Data = new Dictionary<int, PlayerDataClass>();
    public Dictionary<string, string> HookConVars = new();
    public Dictionary<int, long> ActiveReloadId = new();
    public List<CBaseEntity> CbaseWeapons = new List<CBaseEntity>();
    public JObject? JsonData { get; set; }
    public JObject? JsonData_Weapon { get; set; }
    public CounterStrikeSharp.API.Modules.Timers.Timer? TimerCleanUp;
    public CounterStrikeSharp.API.Modules.Timers.Timer? TimerChecker;


    public void Clear()
    {
        _reloadIdCounter = 0;
        Player_Data?.Clear();
        ActiveReloadId?.Clear();
        CbaseWeapons?.Clear();

        TimerCleanUp?.Kill();
        TimerCleanUp = null;
        TimerChecker?.Kill();
        TimerChecker = null;
    }
}