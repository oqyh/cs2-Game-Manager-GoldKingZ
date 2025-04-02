using CounterStrikeSharp.API.Core;
using Newtonsoft.Json.Linq;

namespace Game_Manager_GoldKingZ;

public class Globals_Static
{
    public static Dictionary<string, (string State, string ChatType)> messageMappings = new()
    {
        { "Cstrike_Chat_All", ("ALIVE", "ALL") },
        { "Cstrike_Chat_AllDead", ("DEAD", "ALL") },
        { "Cstrike_Chat_AllSpec", ("ALIVE", "ALL") },
        { "Cstrike_Chat_CT_Loc", ("ALIVE", "TEAM") },
        { "Cstrike_Chat_T_Loc", ("ALIVE", "TEAM") },
        { "Cstrike_Chat_Spec", ("SPEC", "TEAM") },
        { "Cstrike_Chat_CT_Dead", ("DEAD", "TEAM") },
        { "Cstrike_Chat_T_Dead", ("DEAD", "TEAM") },
    };
    public static uint HIDEWEAPONS = 64;
    public static uint HIDECHAT = 128;
    public static uint CROSSHAIRANDNAMETAGS = 256;
}

public class Globals
{
    public class PlayerDataClass
    {
        public CCSPlayerController Player { get; set; }
        public string Messagename { get; set; }
        public int PlayerAlpha { get; set; }
        public bool Remove_Icon { get; set; }
        public bool StabedHisTeamMate { get; set; }
        public CounterStrikeSharp.API.Modules.Timers.Timer Timer_DeadBody { get; set; }
        public PlayerDataClass(CCSPlayerController player, string messagename, int playerAlpha, bool remove_Icon, bool stabedHisTeamMate, CounterStrikeSharp.API.Modules.Timers.Timer timer_DeadBody)
        {
            Player = player;
            PlayerAlpha = playerAlpha;
            Messagename = messagename;
            Remove_Icon = remove_Icon;
            StabedHisTeamMate = stabedHisTeamMate;
            Timer_DeadBody = timer_DeadBody;
        }
    }
    public Dictionary<CCSPlayerController, PlayerDataClass> Player_Data = new Dictionary<CCSPlayerController, PlayerDataClass>();
    public List<CBaseEntity> CbaseWeapons = new List<CBaseEntity>();
    public JObject? JsonData { get; set; }

}