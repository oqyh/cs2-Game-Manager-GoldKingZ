using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using System.Text.Json.Serialization;
using System.Drawing;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Cvars;

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
    [JsonPropertyName("DisableTeamMateHeadTag")] public int DisableTeamMateHeadTag { get; set; } = 0;
    [JsonPropertyName("DisableWinOrLosePanel")] public bool DisableWinOrLosePanel { get; set; } = false;
    [JsonPropertyName("DisableWinOrLoseSound")] public bool DisableWinOrLoseSound { get; set; } = false;
    [JsonPropertyName("DisableJumpLandSound")] public bool DisableJumpLandSound { get; set; } = false;
    [JsonPropertyName("DisableFallDamage")] public bool DisableFallDamage { get; set; } = false;
    [JsonPropertyName("DisableLegs")] public bool DisableLegs { get; set; } = false;
    [JsonPropertyName("DisableSvCheats")] public bool DisableSvCheats { get; set; } = false;
    [JsonPropertyName("DisableRewardMoneyMessages")] public bool DisableRewardMoneyMessages { get; set; } = false;

    [JsonPropertyName("IgnoreDefaultDisconnectMessages")] public bool IgnoreDefaultDisconnectMessages { get; set; } = false;
    [JsonPropertyName("IgnoreDefaultJoinTeamMessages")] public bool IgnoreDefaultJoinTeamMessages { get; set; } = false;

    [JsonPropertyName("CustomJoinTeamMessages")] public bool CustomJoinTeamMessages { get; set; } = false;
    [JsonPropertyName("CustomJoinTeamMessagesCT")] public string CustomJoinTeamMessagesCT { get; set; } = "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {grey}is joining the {lime}Counter-Terrorists";
    [JsonPropertyName("CustomJoinTeamMessagesT")] public string CustomJoinTeamMessagesT { get; set; } = "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {grey}is joining the {lime}Terrorists";
    [JsonPropertyName("CustomJoinTeamMessagesSpec")] public string CustomJoinTeamMessagesSpec { get; set; } = "{green}Gold KingZ {grey}| {purple}{PLAYERNAME} {grey}is joining the {lime}Spectators";

    [JsonPropertyName("RestartServerMode")] public int RestartServerMode { get; set; } = 0;
    [JsonPropertyName("RestartXTimerInMins")] public int RestartXTimerInMins { get; set; } = 5;  
    [JsonPropertyName("RestartWhenXPlayersInServerORLess")] public int RestartWhenXPlayersInServerORLess { get; set; } = 0;
    
    [JsonPropertyName("RotationServerMode")] public int RotationServerMode { get; set; } = 0;
    [JsonPropertyName("RotationXTimerInMins")] public int RotationXTimerInMins { get; set; } = 8;  
    [JsonPropertyName("RotationWhenXPlayersInServerORLess")] public int RotationWhenXPlayersInServerORLess { get; set; } = 0;
    //[JsonPropertyName("IgnoreTeamMateAttackMessages")] public bool IgnoreTeamMateAttackMessages { get; set; } = false;
    //[JsonPropertyName("IgnorePlayerSavedYouByPlayerMessages")] public bool IgnorePlayerSavedYouByPlayerMessages { get; set; } = false;

}



public class GameBManger : BasePlugin, IPluginConfig<GameBMangerConfig> 
{
    public override string ModuleName => "Game Manager";
    public override string ModuleVersion => "1.0.4";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "Block/Hide , Messages , Ping , Radio , Team , Connect , Disconnect , Sounds , Restart On Last Player Disconnect , Map Rotation";
    public GameBMangerConfig Config { get; set; } = new GameBMangerConfig();

    //public CHandle<CBaseEntity> RagdollSource => Schema.GetDeclaredClass<CHandle<CBaseEntity>>(Handle, "CRagdollProp", "m_hRagdollSource");
    //public nint Handle { get; private set; }
    //private HashSet<string> uniqueLines = new HashSet<string>();
    private static string maplist = "";
    private string GmapName = "";
    private static string[] _lines = null!;
    private static int _currentIndex = -1;
    private bool onetime = false;
    public void OnConfigParsed(GameBMangerConfig config)
    {
        Config = config;

        if (Config.DisableTeamMateHeadTag < 0 || Config.DisableTeamMateHeadTag > 2)
        {
            config.DisableTeamMateHeadTag = 0;
            Console.WriteLine("|||||||||||||||||||||||||||||||||||| I N V A L I D ||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("DisableTeamMateHeadTag: is invalid, setting to default value (0) Please Choose 0 or 1 or 2.");
            Console.WriteLine("|||||||||||||||||||||||||||||||||||| I N V A L I D ||||||||||||||||||||||||||||||||||||");
        }
        if (Config.RestartServerMode < 0 || Config.RestartServerMode > 2)
        {
            config.RestartServerMode = 0;
            Console.WriteLine("|||||||||||||||||||||||||||||||||||| I N V A L I D ||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("RestartServerMode: is invalid, setting to default value (0) Please Choose 0 or 1 or 2.");
            Console.WriteLine("|||||||||||||||||||||||||||||||||||| I N V A L I D ||||||||||||||||||||||||||||||||||||");
        }
        if (Config.RotationServerMode < 0 || Config.RotationServerMode > 2)
        {
            config.RotationServerMode = 0;
            Console.WriteLine("|||||||||||||||||||||||||||||||||||| I N V A L I D ||||||||||||||||||||||||||||||||||||");
            Console.WriteLine("RotationServerMode: is invalid, setting to default value (0) Please Choose 0 or 1 or 2.");
            Console.WriteLine("|||||||||||||||||||||||||||||||||||| I N V A L I D ||||||||||||||||||||||||||||||||||||");
        }

    }
    private CounterStrikeSharp.API.Modules.Timers.Timer? _restartTimer;
    private CounterStrikeSharp.API.Modules.Timers.Timer? _restartTimer2;
    private CounterStrikeSharp.API.Modules.Timers.Timer? RotationTimer;
    private CounterStrikeSharp.API.Modules.Timers.Timer? RotationTimer2;
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
        maplist = Path.Combine(ModuleDirectory, "RotationServerMapList.txt");

        AddCommandListener("player_ping", CommandListener_Ping);
        AddCommandListener("playerchatwheel", CommandListener_chatwheel);

        RegisterListener<Listeners.OnMapStart>(mapName =>
        {
            Server.NextFrame(() =>
            {
                _restartTimer?.Kill();
                _restartTimer = null;
                _restartTimer2?.Kill();
                _restartTimer2 = null;

                onetime = false;
                RotationTimer?.Kill();
                RotationTimer = null;
                RotationTimer2?.Kill();
                RotationTimer2 = null;
                RotationTimer = AddTimer(0.1f, RotationTimer_Callback, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);

                if(Config.RotationServerMode == 1 || Config.RotationServerMode == 2 || Config.RestartServerMode != 0)
                {
                    ConVar sv_hibernate_when_empty = ConVar.Find("sv_hibernate_when_empty")!;
                    if (sv_hibernate_when_empty.GetPrimitiveValue<bool>() == true)
                    {
                        Console.WriteLine("|||||||||||||||||||||||||||||| E R R O R ||||||||||||||||||||||||||||||");
                        Console.WriteLine("[Error] Please Disable (sv_hibernate_when_empty)");
                        Console.WriteLine("|||||||||||||||||||||||||||||| E R R O R ||||||||||||||||||||||||||||||");
                    }
                }
            });
        });

        RegisterEventHandler<EventRoundEnd>((@event, info) =>
        {
            if (!Config.DisableWinOrLoseSound || @event == null)return HookResult.Continue;

            info.DontBroadcast = true;
            
            return HookResult.Continue;
        }, HookMode.Pre);

        RegisterEventHandler<EventRoundStart>((@event, info) =>
        {
            if (@event == null)return HookResult.Continue;

            if(Config.DisableTeamMateHeadTag == 1)
            {
                Server.ExecuteCommand("sv_teamid_overhead 1; sv_teamid_overhead_always_prohibit 1");
            }

            if(Config.DisableTeamMateHeadTag == 2)
            {
                Server.ExecuteCommand("sv_teamid_overhead 0");
            }

            if(Config.DisableSvCheats)
            {
                
                Server.ExecuteCommand("sv_cheats 0");
            }
            if(Config.DisableRewardMoneyMessages)
            {
                Server.ExecuteCommand("mp_playercashawards 0; cash_team_bonus_shorthanded 0");
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
        }, HookMode.Post);
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
            if (!Config.IgnoreDefaultJoinTeamMessages || @event == null)return HookResult.Continue;

            info.DontBroadcast = true;
            
            return HookResult.Continue;
        }, HookMode.Pre);

        RegisterEventHandler<EventPlayerTeam>((@event, info) =>
        {
            if (!Config.CustomJoinTeamMessages || @event == null)return HookResult.Continue;

            CCSPlayerController player = @event.Userid;

            if (player == null
            || !player.IsValid)
            {
                return HookResult.Continue;
            }

            int Team = @event.Team;
            var Playername = player.PlayerName;

            if(Team == 1)
            {
                if (!string.IsNullOrEmpty(Config.CustomJoinTeamMessagesSpec))
                {
                    var replacer = ReplaceMessages(" " + Config.CustomJoinTeamMessagesSpec, Playername);
                    Server.PrintToChatAll(replacer);
                }
            }else if(Team == 2)
            {
                if (!string.IsNullOrEmpty(Config.CustomJoinTeamMessagesT))
                {
                    var replacer = ReplaceMessages(" " + Config.CustomJoinTeamMessagesT, Playername);
                    Server.PrintToChatAll(replacer);
                }
            }else if(Team == 3)
            {
                
                if (!string.IsNullOrEmpty(Config.CustomJoinTeamMessagesCT))
                {
                    var replacer = ReplaceMessages(" " + Config.CustomJoinTeamMessagesCT, Playername);
                    Server.PrintToChatAll(replacer);
                }
            }
            
            return HookResult.Continue;
        }, HookMode.Post);

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
            var players = Utilities.GetPlayers().Where(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot);
            var playersCount = players.Count();

            if (Config.RestartServerMode != 0)
            {
                if(playersCount <= Config.RestartWhenXPlayersInServerORLess)
                {
                _restartTimer?.Kill();
                _restartTimer = null;
                _restartTimer2?.Kill();
                _restartTimer2 = null;

                _restartTimer = AddTimer(0.1f, RestartTimer_Callback, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
                }else if(playersCount > Config.RestartWhenXPlayersInServerORLess)
                {
                    _restartTimer?.Kill();
                    _restartTimer = null;
                    
                    _restartTimer2?.Kill();
                    _restartTimer2 = null;
                }
            }

            if(Config.RotationServerMode == 1 || Config.RotationServerMode == 2)
            {
                if(playersCount <= Config.RotationWhenXPlayersInServerORLess)
                {
                    onetime = false;
                    RotationTimer?.Kill();
                    RotationTimer = null;
                    RotationTimer2?.Kill();
                    RotationTimer2 = null;
                    RotationTimer = AddTimer(0.1f, RotationTimer_Callback, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
                }else if(playersCount > Config.RotationWhenXPlayersInServerORLess)
                {
                    onetime = false;
                    RotationTimer?.Kill();
                    RotationTimer = null;
                    RotationTimer2?.Kill();
                    RotationTimer2 = null;
                }
            }
        });

        RegisterListener<Listeners.OnClientDisconnectPost>(playerSlot =>
        {
            var players = Utilities.GetPlayers().Where(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot);
            var playersCount = players.Count();

            if (Config.RestartServerMode != 0)
            {
                if(playersCount <= Config.RestartWhenXPlayersInServerORLess)
                {
                _restartTimer?.Kill();
                _restartTimer = null;
                _restartTimer2?.Kill();
                _restartTimer2 = null;

                _restartTimer = AddTimer(0.1f, RestartTimer_Callback, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
                }else if(playersCount > Config.RestartWhenXPlayersInServerORLess)
                {
                    _restartTimer?.Kill();
                    _restartTimer = null;
                    
                    _restartTimer2?.Kill();
                    _restartTimer2 = null;
                }
            }

            if(Config.RotationServerMode == 1 || Config.RotationServerMode == 2)
            {
                if(playersCount <= Config.RotationWhenXPlayersInServerORLess)
                {
                    onetime = false;
                    RotationTimer?.Kill();
                    RotationTimer = null;
                    RotationTimer2?.Kill();
                    RotationTimer2 = null;
                    RotationTimer = AddTimer(0.1f, RotationTimer_Callback, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
                }else if(playersCount > Config.RotationWhenXPlayersInServerORLess)
                {
                    onetime = false;
                    RotationTimer?.Kill();
                    RotationTimer = null;
                    RotationTimer2?.Kill();
                    RotationTimer2 = null;
                }
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
            if(Config.RestartServerMode == 1)
            {
                Server.ExecuteCommand("sv_cheats 1; restart");
            }else if(Config.RestartServerMode == 2)
            {
                Server.ExecuteCommand("sv_cheats 1; crash");
            }

        }else if(playersCount > Config.RestartWhenXPlayersInServerORLess)
        {
            _restartTimer?.Kill();
            _restartTimer = null;
            
            _restartTimer2?.Kill();
            _restartTimer2 = null;
        }
    }
    private void RotationTimer_Callback()
    {
        var players = Utilities.GetPlayers().Where(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot);
        var playersCount = players.Count();

        if(playersCount <= Config.RotationWhenXPlayersInServerORLess && onetime == false)
        {
            RotationTimer2 = AddTimer(Config.RotationXTimerInMins * 60, RotationTimer_Callback2, TimerFlags.STOP_ON_MAPCHANGE);
            onetime = true;
        }else if(playersCount > Config.RotationWhenXPlayersInServerORLess)
        {
            onetime = false;
            RotationTimer?.Kill();
            RotationTimer = null;
            RotationTimer2?.Kill();
            RotationTimer2 = null;
        }
    }
    private void RotationTimer_Callback2()
    {
        var players = Utilities.GetPlayers().Where(x => x.Connected == PlayerConnectedState.PlayerConnected && !x.IsBot);
        var playersCount = players.Count();

        if(playersCount <= Config.RotationWhenXPlayersInServerORLess)
        {
            if(Config.RotationServerMode == 1)
            {
                GmapName = GetNextMap();
            }else if(Config.RotationServerMode == 2)
            {
                GmapName = GetRandomMap();
            }

            if (GmapName.StartsWith("ds:") )
            {
                string dsworkshop = GmapName.TrimStart().Substring("ds:".Length).Trim();
                Server.ExecuteCommand($"ds_workshop_changelevel {dsworkshop}");
            }else if (GmapName.StartsWith("host:"))
            {
                string hostworkshop = GmapName.TrimStart().Substring("host:".Length).Trim();
                Server.ExecuteCommand($"host_workshop_map {hostworkshop}");
            }else if (!(GmapName.StartsWith("ds:") || GmapName.StartsWith("host:")))
            {
                Server.ExecuteCommand($"changelevel {GmapName}");
            }

        }else if(playersCount > Config.RotationWhenXPlayersInServerORLess)
        {
            onetime = false;
            RotationTimer?.Kill();
            RotationTimer = null;
            RotationTimer2?.Kill();
            RotationTimer2 = null;
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
    private static string GetRandomMap()
    {
        if (File.Exists(maplist))
        {
            string[] lines = File.ReadAllLines(maplist);
            if (lines.Length > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(0, lines.Length);
                return lines[randomIndex];
            }
            else
            {
                Console.WriteLine("|||||||||||||||||||||||||||||| E R R O R ||||||||||||||||||||||||||||||");
                Console.WriteLine("[Error] RotationServerMapList.txt is empty");
                Console.WriteLine("|||||||||||||||||||||||||||||| E R R O R ||||||||||||||||||||||||||||||");
            }
        }
        else
        {
            Console.WriteLine("|||||||||||||||||||||||||||||| E R R O R ||||||||||||||||||||||||||||||");
            Console.WriteLine("[Error] RotationServerMapList.txt does not exist");
            Console.WriteLine("|||||||||||||||||||||||||||||| E R R O R ||||||||||||||||||||||||||||||");
        }
        return null!;
    }
    private static string GetNextMap()
    {
        if (!File.Exists(maplist))
        {
            Console.WriteLine("|||||||||||||||||||||||||||||| E R R O R ||||||||||||||||||||||||||||||");
            Console.WriteLine("[Error] RotationServerMapList.txt does not exist");
            Console.WriteLine("|||||||||||||||||||||||||||||| E R R O R ||||||||||||||||||||||||||||||");
            return null!;
        }

        if (_lines == null || _currentIndex == _lines.Length - 1)
        {
            _lines = File.ReadAllLines(maplist);
            _currentIndex = -1;
        }
        _currentIndex++;
        return _lines[_currentIndex];
    }
    private string ReplaceMessages(string Message, string PlayerName)
    {
        var replacedMessage = Message
                                    .Replace("{PLAYERNAME}", PlayerName.ToString());
        replacedMessage = ReplaceColors(replacedMessage);
        return replacedMessage;
    }

    private string ReplaceColors(string input)
    {
        string[] colorPatterns =
        {
            "{default}", "{white}", "{darkred}", "{green}", "{lightyellow}",
            "{lightblue}", "{olive}", "{lime}", "{red}", "{lightpurple}",
            "{purple}", "{grey}", "{yellow}", "{gold}", "{silver}",
            "{blue}", "{darkblue}", "{bluegrey}", "{magenta}", "{lightred}",
            "{orange}"
        };
        string[] colorReplacements =
        {
            "\x01", "\x01", "\x02", "\x04", "\x09", "\x0B", "\x05",
            "\x06", "\x07", "\x03", "\x0E", "\x08", "\x09", "\x10",
            "\x0A", "\x0B", "\x0C", "\x0A", "\x0E", "\x0F", "\x10"
        };

        for (var i = 0; i < colorPatterns.Length; i++)
            input = input.Replace(colorPatterns[i], colorReplacements[i]);

        return input;
    }

    /*
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
                    return HookResult.Handled;
                }
            }
        }

        if(Config.IgnorePlayerSavedYouByPlayerMessages == true)
        {
            for (int i = 0; i < SavedbyArray.Length; i++)
            {
                if (message.Contains(SavedbyArray[i]))
                {
                    return HookResult.Handled;
                }
            }
        }

        if(Config.IgnoreRewardMoneyMessages == 2)
        {
            for (int i = 0; i < MoneyMessageArray.Length; i++)
            {
                if (message.Contains(MoneyMessageArray[i]))
                {
                    return HookResult.Handled;
                }
            }
        }
        
        return HookResult.Continue;
    }
    */
}