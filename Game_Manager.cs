using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Core.Attributes;
using System.Text.Json.Serialization;

namespace Game_Manager;


public class GameBMangerConfig : BasePluginConfig
{
    [JsonPropertyName("DisableRadio")] public bool DisableRadio { get; set; } = false;
    [JsonPropertyName("DisablePing")] public bool DisablePing { get; set; } = false;
    [JsonPropertyName("DisableChatWheel")] public bool DisableChatWheel { get; set; } = false;
    [JsonPropertyName("DisableKillfeed")] public bool DisableKillfeed { get; set; } = false;
    [JsonPropertyName("DisableWinOrLosePanel")] public bool DisableWinOrLosePanel { get; set; } = false;
    [JsonPropertyName("DisableWinOrLoseSound")] public bool DisableWinOrLoseSound { get; set; } = false;
    [JsonPropertyName("IgnoreJoinTeamMessages")] public bool IgnoreJoinTeamMessages { get; set; } = false;
    [JsonPropertyName("IgnoreDefaultDisconnectMessages")] public bool IgnoreDefaultDisconnectMessages { get; set; } = false;
}

public class GameBManger : BasePlugin, IPluginConfig<GameBMangerConfig>
{
    public override string ModuleName => "Game Manager";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "Block/Hide , Messages , Ping , Radio , Connect , Disconnect , Sounds";

    public GameBMangerConfig Config { get; set; }
    
    public void OnConfigParsed(GameBMangerConfig config)
    {
        Config = config;
    }
    private string[] RadioArray = new string[] {
    "coverme",
	"takepoint",
	"holdpos",
	"regroup",
	"followme",
	"takingfire",
	"go",
	"fallback",
	"sticktog",
	"getinpos",
	"stormfront",
	"report",
	"roger",
	"enemyspot",
	"needbackup",
	"sectorclear",
	"inposition",
	"reportingin",
	"getout",
	"negative",
	"enemydown",
	"sorry",
	"cheer",
	"compliment",
	"thanks",
	"go_a",
	"go_b",
	"needrop",
	"deathcry"
    };
    
    public override void Load(bool hotReload)
    {
        
        //AddCommandListener("chatwheel_ping", CommandListener_chatwheelping);
        //AddCommandListener("playerradio", CommandListener_playerradio);
        AddCommandListener("player_ping", CommandListener_Ping);
        AddCommandListener("playerchatwheel", CommandListener_chatwheel);
        //RegisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnect, HookMode.Pre);
        //RegisterEventHandler<EventPlayerTeam>(OnPlayerTeam, HookMode.Pre);
        RegisterEventHandler<EventRoundEnd>((@event, info) =>
        {
            if (!Config.DisableWinOrLoseSound || @event == null)return HookResult.Continue;

            info.DontBroadcast = true;
            
            return HookResult.Continue;
        }, HookMode.Pre);
        RegisterEventHandler<EventCsWinPanelRound>((@event, info) =>
        {
            if (!Config.DisableWinOrLosePanel || @event == null)return HookResult.Continue;

            info.DontBroadcast = true;
            
            return HookResult.Continue;
        }, HookMode.Pre);
        RegisterEventHandler<EventPlayerDisconnect>((@event, info) =>
        {
            if (!Config.IgnoreDefaultDisconnectMessages || @event == null)return HookResult.Continue;

            info.DontBroadcast = true;
            
            return HookResult.Continue;
        }, HookMode.Pre);
        RegisterEventHandler<EventPlayerTeam>((@event, info) =>
        {
            if (!Config.IgnoreJoinTeamMessages || @event == null)return HookResult.Continue;

            info.DontBroadcast = true;
            
            return HookResult.Continue;
        }, HookMode.Pre);
        RegisterEventHandler<EventPlayerDeath>((@event, info) =>
        {
            if (!Config.DisableKillfeed || @event == null)return HookResult.Continue;

            info.DontBroadcast = true;
            
            return HookResult.Continue;
        }, HookMode.Pre);
        for (int i = 0; i < RadioArray.Length; i++)
        {
            AddCommandListener(RadioArray[i], CommandListener_RadioCommands);
        }
    }
    private HookResult CommandListener_Ping(CCSPlayerController? player, CommandInfo info)
    {
        if(!Config.DisablePing || player == null || !player.IsValid)return HookResult.Continue;

        return HookResult.Handled;
    }

    private HookResult CommandListener_chatwheel(CCSPlayerController? player, CommandInfo info)
    {
        if(!Config.DisableChatWheel || player == null || !player.IsValid)return HookResult.Continue;

        return HookResult.Handled;
    }
    private HookResult CommandListener_RadioCommands(CCSPlayerController? player, CommandInfo info)
    {
        if(!Config.DisableRadio || player == null || !player.IsValid)return HookResult.Continue;

        return HookResult.Handled;
    }
}
