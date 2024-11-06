using CounterStrikeSharp.API.Core;

namespace Game_Manager_GoldKingZ;

public class Globals
{
    public static List<CBaseEntity> CbaseWeapons = new List<CBaseEntity>();

    public static bool onetimeclean = false;
    public static uint HIDEWEAPONS = 64;
    public static uint HIDECHAT = 128;
    public static uint CROSSHAIRANDNAMETAGS = 256;
    public static Dictionary<ulong, int> Toggle_DisableChat = new Dictionary<ulong, int>();
    public static Dictionary<ulong, bool> Toggle_OnDisableChat = new Dictionary<ulong, bool>();
    public static Dictionary<CCSPlayerController, bool> StabedHisTeamMate = new Dictionary<CCSPlayerController, bool>();
    public static Dictionary<ulong, int> Toggle_DisableWeapons = new Dictionary<ulong, int>();
    public static Dictionary<ulong, bool> Toggle_OnDisableWeapons = new Dictionary<ulong, bool>();
    public static Dictionary<ulong, int> Toggle_DisableLegs = new Dictionary<ulong, int>();
    public static Dictionary<ulong, bool> Remove_Icon = new Dictionary<ulong, bool>();
    public static Dictionary<CCSPlayerController, CounterStrikeSharp.API.Modules.Timers.Timer> TimerRemoveDeadBody = new Dictionary<CCSPlayerController, CounterStrikeSharp.API.Modules.Timers.Timer>();

    public static Dictionary<CCSPlayerController, int> PlayerAlpha = new Dictionary<CCSPlayerController, int>();
}