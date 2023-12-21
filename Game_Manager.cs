using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using System.Text.Json.Serialization;
using System.Drawing;
using CounterStrikeSharp.API.Modules.Timers;

namespace Game_Manager;

public class GameBMangerConfig : BasePluginConfig
{
    [JsonPropertyName("DisableRadio")] public bool DisableRadio { get; set; } = false;
    [JsonPropertyName("DisableGrenadeRadio")] public bool DisableGrenadeRadio { get; set; } = false;
    [JsonPropertyName("DisablePing")] public bool DisablePing { get; set; } = false;
    [JsonPropertyName("DisableChatWheel")] public bool DisableChatWheel { get; set; } = false;
    [JsonPropertyName("DisableKillfeed")] public bool DisableKillfeed { get; set; } = false;
    [JsonPropertyName("DisableRadar")] public bool DisableRadar { get; set; } = false;
    [JsonPropertyName("DisableMoneyHUD")] public bool DisableMoneyHUD { get; set; } = false;
    [JsonPropertyName("DisableWinOrLosePanel")] public bool DisableWinOrLosePanel { get; set; } = false;
    [JsonPropertyName("DisableWinOrLoseSound")] public bool DisableWinOrLoseSound { get; set; } = false;
    [JsonPropertyName("DisableJumpLandSound")] public bool DisableJumpLandSound { get; set; } = false;
    [JsonPropertyName("DisableFallDamage")] public bool DisableFallDamage { get; set; } = false;
    [JsonPropertyName("DisableLegs")] public bool DisableLegs { get; set; } = false;
    [JsonPropertyName("IgnoreJoinTeamMessages")] public bool IgnoreJoinTeamMessages { get; set; } = false;
    [JsonPropertyName("IgnoreRewardMoneyMessages")] public int IgnoreRewardMoneyMessages { get; set; } = 0;
    [JsonPropertyName("IgnoreTeamMateAttackMessages")] public bool IgnoreTeamMateAttackMessages { get; set; } = false;
    [JsonPropertyName("IgnorePlayerSavedYouByPlayerMessages")] public bool IgnorePlayerSavedYouByPlayerMessages { get; set; } = false;
    [JsonPropertyName("IgnoreDefaultDisconnectMessages")] public bool IgnoreDefaultDisconnectMessages { get; set; } = false;
    [JsonPropertyName("RestartServerLastPlayerDisconnect")] public bool RestartServerLastPlayerDisconnect { get; set; } = false;
    [JsonPropertyName("RestartMethod")] public int RestartMethod { get; set; } = 1;
    [JsonPropertyName("RestartXTimerInMins")] public int RestartXTimerInMins { get; set; } = 5;  
    [JsonPropertyName("RestartWhenXPlayersInServerORLess")] public int RestartWhenXPlayersInServerORLess { get; set; } = 0;

}



public class GameBManger : BasePlugin, IPluginConfig<GameBMangerConfig> 
{
    public override string ModuleName => "Game Manager";
    public override string ModuleVersion => "1.0.3";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "Block/Hide , Messages , Ping , Radio , Team , Connect , Disconnect , Sounds , Restart On Last Player Disconnect";
    public GameBMangerConfig Config { get; set; } = new GameBMangerConfig();
    public void OnConfigParsed(GameBMangerConfig config)
    {
        Config = config;

        if (Config.IgnoreRewardMoneyMessages < 0 || Config.IgnoreRewardMoneyMessages > 2)
        {
            config.IgnoreRewardMoneyMessages = 0;
            Console.WriteLine("IgnoreRewardMoneyMessages: is invalid, setting to default value (0) Please Choose 0 or 1 or 2.");
        }
        if (Config.RestartXTimerInMins < 0 )
        {
            config.RestartXTimerInMins = 5;
            Console.WriteLine("RestartXTimerInMins: is invalid, setting to default value (5) Please Choose 0 OR Above.");
        }
        if (Config.RestartWhenXPlayersInServerORLess < 0 )
        {
            config.RestartWhenXPlayersInServerORLess = 0;
            Console.WriteLine("RestartWhenXPlayersInServerORLess: is invalid, setting to default value (0) Please Choose 0 OR Above.");
        }
        if (Config.RestartMethod < 0 || Config.RestartMethod > 2)
        {
            config.RestartMethod = 1;
            Console.WriteLine("RestartMethod: is invalid, setting to default value (1) Please Choose 0 or 1 or 2.");
        }
    }

    /*
    public void HookConVarChange(ConVar convar, bool oldValue, bool newValue)
    {
        var cheatsCvar = ConVar.Find("sv_ignoregrenaderadio");

        if(oldValue == false || newValue == false)
        {
            cheatsCvar.GetPrimitiveValue<bool>() = true;
        }
    }
    */
    private CounterStrikeSharp.API.Modules.Timers.Timer? _restartTimer;
    private CounterStrikeSharp.API.Modules.Timers.Timer? _restartTimer2;
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
    private string[] MoneyMessageArray = new string[] {
    "#Player_Cash_Award_Kill_Teammate",
	"#Player_Cash_Award_Killed_VIP",
	"#Player_Cash_Award_Killed_Enemy_Generic",
	"#Player_Cash_Award_Killed_Enemy",
	"#Player_Cash_Award_Bomb_Planted",
	"#Player_Cash_Award_Bomb_Defused",
	"#Player_Cash_Award_Rescued_Hostage",
	"#Player_Cash_Award_Interact_Hostage",
	"#Player_Cash_Award_Respawn",
	"#Player_Cash_Award_Get_Killed",
	"#Player_Cash_Award_Damage_Hostage",
	"#Player_Cash_Award_Kill_Hostage",
	"#Player_Point_Award_Killed_Enemy",
	"#Player_Point_Award_Killed_Enemy_Plural",
	"#Player_Point_Award_Killed_Enemy_NoWeapon",
	"#Player_Point_Award_Killed_Enemy_NoWeapon_Plural",
	"#Player_Point_Award_Assist_Enemy",
	"#Player_Point_Award_Assist_Enemy_Plural",
	"#Player_Point_Award_Picked_Up_Dogtag",
	"#Player_Point_Award_Picked_Up_Dogtag_Plural",
	"#Player_Team_Award_Killed_Enemy",
	"#Player_Team_Award_Killed_Enemy_Plural",
	"#Player_Team_Award_Bonus_Weapon",
	"#Player_Team_Award_Bonus_Weapon_Plural",
	"#Player_Team_Award_Picked_Up_Dogtag",
	"#Player_Team_Award_Picked_Up_Dogtag_Plural",
	"#Player_Team_Award_Picked_Up_Dogtag_Friendly",
	"#Player_Cash_Award_ExplainSuicide_YouGotCash",
	"#Player_Cash_Award_ExplainSuicide_TeammateGotCash",
	"#Player_Cash_Award_ExplainSuicide_EnemyGotCash",
	"#Player_Cash_Award_ExplainSuicide_Spectators",
	"#Team_Cash_Award_T_Win_Bomb",
	"#Team_Cash_Award_Elim_Hostage",
	"#Team_Cash_Award_Elim_Bomb",
	"#Team_Cash_Award_Win_Time",
	"#Team_Cash_Award_Win_Defuse_Bomb",
	"#Team_Cash_Award_Win_Hostages_Rescue",
	"#Team_Cash_Award_Win_Hostage_Rescue",
	"#Team_Cash_Award_Loser_Bonus",
	"#Team_Cash_Award_Bonus_Shorthanded",
    "#Notice_Bonus_Enemy_Team",
    "#Notice_Bonus_Shorthanded_Eligibility",
    "#Notice_Bonus_Shorthanded_Eligibility_Single",
	"#Team_Cash_Award_Loser_Bonus_Neg",
	"#Team_Cash_Award_Loser_Zero",
	"#Team_Cash_Award_Rescued_Hostage",
	"#Team_Cash_Award_Hostage_Interaction",
	"#Team_Cash_Award_Hostage_Alive",
	"#Team_Cash_Award_Planted_Bomb_But_Defused",
	"#Team_Cash_Award_Survive_GuardianMode_Wave",
	"#Team_Cash_Award_CT_VIP_Escaped",
	"#Team_Cash_Award_T_VIP_Killed",
	"#Team_Cash_Award_no_income",
	"#Team_Cash_Award_no_income_suicide",
	"#Team_Cash_Award_Generic",
	"#Team_Cash_Award_Custom"
    };
    private string[] SavedbyArray = new string[] {
	"#Chat_SavePlayer_Savior",
    "#Chat_SavePlayer_Spectator",
    "#Chat_SavePlayer_Saved"
    };
    private string[] TeamWarningArray = new string[] {
    "#Cstrike_TitlesTXT_Game_teammate_attack",
	"#Cstrike_TitlesTXT_Game_teammate_kills",
	"#Cstrike_TitlesTXT_Hint_careful_around_teammates",
	"#Cstrike_TitlesTXT_Hint_try_not_to_injure_teammates",
	"#Cstrike_TitlesTXT_Killed_Teammate",
	"#SFUI_Notice_Game_teammate_kills",
	"#SFUI_Notice_Hint_careful_around_teammates",
	"#SFUI_Notice_Killed_Teammate"
    };
    public override void Load(bool hotReload)
    {
        if(Config.IgnoreTeamMateAttackMessages == true || Config.IgnorePlayerSavedYouByPlayerMessages == true || Config.IgnoreRewardMoneyMessages == 2)
        {
            VirtualFunctions.ClientPrintFunc.Hook(OnPrintToChat, HookMode.Pre);
            VirtualFunctions.ClientPrintAllFunc.Hook(OnPrintToChatAll, HookMode.Pre);
        }

        AddCommandListener("player_ping", CommandListener_Ping);
        AddCommandListener("playerchatwheel", CommandListener_chatwheel);

        RegisterEventHandler<EventRoundPrestart>((@event, info) =>
        {
            if(Config.IgnoreRewardMoneyMessages == 1)
            {
                Server.ExecuteCommand("mp_playercashawards 0");
                Server.ExecuteCommand("cash_team_bonus_shorthanded 0");
            }
            if(Config.DisableRadar)
            {
                Server.ExecuteCommand("sv_disable_radar 1");
            }

            if(Config.DisableGrenadeRadio)
            {
                Server.ExecuteCommand("sv_ignoregrenaderadio 1");
            }

            if(Config.DisableFallDamage)
            {
                Server.ExecuteCommand("sv_falldamage_scale 0");
            }

            if(Config.DisableMoneyHUD)
            {
                Server.ExecuteCommand("mp_teamcashawards 0");
            }

            if(Config.DisableJumpLandSound)
            {
                Server.ExecuteCommand("sv_min_jump_landing_sound 999999");
            }

            return HookResult.Continue;
        }, HookMode.Pre);
        RegisterListener<Listeners.OnMapStart>(mapName =>
        {
            _restartTimer?.Kill();
            _restartTimer = null;
            _restartTimer2?.Kill();
            _restartTimer2 = null;
        });
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
        RegisterEventHandler<EventPlayerSpawn>((@event, info) =>
        {
            if(!Config.DisableLegs || @event == null)
            {
                return HookResult.Continue;
            }

            CCSPlayerController player = @event.Userid;

            if (player == null
            || !player.IsValid
            || player.PlayerPawn == null
            || !player.PlayerPawn.IsValid
            || player.PlayerPawn.Value == null
            || !player.PlayerPawn.Value.IsValid)
            {
                return HookResult.Continue;
            }

            player.PlayerPawn.Value.Render = Color.FromArgb(254, 254, 254, 254);

            return HookResult.Continue;
            
        });
        RegisterListener<Listeners.OnClientConnected>(playerSlot =>
        {
            if (!Config.RestartServerLastPlayerDisconnect || Config.RestartMethod == 0) return;
            var players = Utilities.GetPlayers().Where(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot);
            var playersCount = players.Count();

            if(playersCount <= Config.RestartWhenXPlayersInServerORLess)
            {
            _restartTimer?.Kill();
            _restartTimer = null;
            _restartTimer2?.Kill();
            _restartTimer2 = null;

            _restartTimer = AddTimer(0.1f, RestartTimer_Callback, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
            }else if(playersCount >= Config.RestartWhenXPlayersInServerORLess)
            {
                _restartTimer?.Kill();
                _restartTimer = null;
                
                _restartTimer2?.Kill();
                _restartTimer2 = null;
            }

        });
        RegisterListener<Listeners.OnClientDisconnectPost>(playerSlot =>
        {
            if (!Config.RestartServerLastPlayerDisconnect || Config.RestartMethod == 0) return;

            var players = Utilities.GetPlayers().Where(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot);
            var playersCount = players.Count();

            if(playersCount <= Config.RestartWhenXPlayersInServerORLess)
            {
            _restartTimer?.Kill();
            _restartTimer = null;
            _restartTimer2?.Kill();
            _restartTimer2 = null;

            _restartTimer = AddTimer(0.1f, RestartTimer_Callback, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
            }else if(playersCount >= Config.RestartWhenXPlayersInServerORLess)
            {
                _restartTimer?.Kill();
                _restartTimer = null;
                
                _restartTimer2?.Kill();
                _restartTimer2 = null;
            }
        });
        for (int i = 0; i < RadioArray.Length; i++)
        {
            AddCommandListener(RadioArray[i], CommandListener_RadioCommands);
        }
    }
    private void RestartTimer_Callback()
    {
        var players = Utilities.GetPlayers().Where(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot);
        var playersCount = players.Count();

        //Server.PrintToConsole($"playersCount{playersCount} Config.RestartWhenXPlayersInServerORLess{Config.RestartWhenXPlayersInServerORLess}");
        if(playersCount <= Config.RestartWhenXPlayersInServerORLess)
        {
            _restartTimer2 = AddTimer(Config.RestartXTimerInMins * 60, RestartTimer_Callback2, TimerFlags.STOP_ON_MAPCHANGE);
        }
    }
    private void RestartTimer_Callback2()
    {
        var players = Utilities.GetPlayers().Where(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot);
        var playersCount = players.Count();

        if(playersCount <= Config.RestartWhenXPlayersInServerORLess)
        {
            if(Config.RestartMethod == 1)
            {
                Server.ExecuteCommand("sv_cheats 1; restart");
            }else if(Config.RestartMethod == 2)
            {
                Server.ExecuteCommand("sv_cheats 1; crash");
            }

        }else if(playersCount >= Config.RestartWhenXPlayersInServerORLess)
        {
            _restartTimer?.Kill();
            _restartTimer = null;
            
            _restartTimer2?.Kill();
            _restartTimer2 = null;
        }
    }
    private HookResult CommandListener_Ping(CCSPlayerController? player, CommandInfo info)
    {
        if(!Config.DisablePing || player == null || !player.IsValid)return HookResult.Continue;

        return HookResult.Handled;
    }

    private HookResult OnPrintToChat(DynamicHook hook)
    {
        return InternalHandler(hook.GetParam<string>(2));
    }
    private HookResult OnPrintToChatAll(DynamicHook hook)
    {
        return InternalHandler(hook.GetParam<string>(1));
    }
    private HookResult InternalHandler(string message)
    {
        if(Config.IgnoreTeamMateAttackMessages == true)
        {
            for (int i = 0; i < TeamWarningArray.Length; i++)
            {
                if (message.Contains(TeamWarningArray[i]))
                {
                    return HookResult.Stop;
                }
            }
        }

        if(Config.IgnorePlayerSavedYouByPlayerMessages == true)
        {
            for (int i = 0; i < SavedbyArray.Length; i++)
            {
                if (message.Contains(SavedbyArray[i]))
                {
                    return HookResult.Stop;
                }
            }
        }

        if(Config.IgnoreRewardMoneyMessages == 2)
        {
            for (int i = 0; i < MoneyMessageArray.Length; i++)
            {
                if (message.Contains(MoneyMessageArray[i]))
                {
                    return HookResult.Stop;
                }
            }
        }
        
        return HookResult.Continue;
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
    public override void Unload(bool hotReload)
    {
        if(Config.IgnoreTeamMateAttackMessages == true || Config.IgnorePlayerSavedYouByPlayerMessages == true || Config.IgnoreRewardMoneyMessages == 2)
        {
            VirtualFunctions.ClientPrintFunc.Unhook(OnPrintToChat, HookMode.Pre);
            VirtualFunctions.ClientPrintAllFunc.Unhook(OnPrintToChatAll, HookMode.Pre);
        }
    }
}