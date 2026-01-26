using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using Game_Manager_GoldKingZ.Config;
using CounterStrikeSharp.API.Modules.Commands;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.Extensions.Localization;
using CounterStrikeSharp.API.Modules.Cvars;
using MySqlConnector;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Modules.Timers;
using System.Diagnostics;
using System.IO;
using CounterStrikeSharp.API.ValveConstants.Protobuf;
using System.Xml.Linq;
using CounterStrikeSharp.API.Core.Translations;
using System.Globalization;


namespace Game_Manager_GoldKingZ;

public class MainPlugin : BasePlugin
{
    public override string ModuleName => "Game Manager (Block/Hide Unnecessaries In Game)";
    public override string ModuleVersion => "2.1.4";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "https://github.com/oqyh";
    public static MainPlugin Instance { get; set; } = new();
    public Globals g_Main = new();
    public readonly Game_Listeners Game_Listeners = new();
    public readonly Game_UserMessages Game_UserMessages = new();
    public override void Load(bool hotReload)
    {
        Instance = this;
        Configs.Load(ModuleDirectory);

        _ = Task.Run(Helper.DownloadMissingFilesAsync);
        Helper.RemoveRegisterCommandsAndHooks();
        Helper.LoadJson();
        Helper.RegisterCommandsAndHooks(true);
        Helper.ExectueCommands();
        Helper.StartTimer();

        if (hotReload)
        {
            _ = Task.Run(Helper.DownloadMissingFilesAsync);
            Helper.RemoveRegisterCommandsAndHooks();
            Helper.LoadJson();
            Helper.RegisterCommandsAndHooks();
            Helper.ExectueCommands();
            Helper.StartTimer();
            Helper.ReloadPlayersGlobals();

            if (Configs.Instance.MySql_Enable > 0)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (Configs.Instance.MySql_Enable > 0)
                        {
                            await MySqlDataManager.CreateTableIfNotExistsAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.DebugMessage($"hotReload Error: {ex.Message}", 0);
                    }
                });
            }
        }
    }

    public void OnEntityCreated(CEntityInstance entity)
    {
        if (entity == null || !entity.IsValid || entity.DesignerName != "player_spray_decal") return;

        Server.NextFrame(() =>
        {
            if (entity == null || !entity.IsValid) return;

            entity.AcceptInput("kill");
        });
        
    }

    public void OnEntitySpawned(CEntityInstance entity)
    {
        if (entity == null || !entity.IsValid || entity.DesignerName != "chicken") return;

        entity.AcceptInput("kill");
    }

    public void OnClientAuthorized(int playerSlot, SteamID steamId)
    {
        var player = Utilities.GetPlayerFromSlot(playerSlot);
        if (!player.IsValid(true)) return;
        Helper.CheckPlayerName(player);
    }

    public HookResult OnEventPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;
        var player = @event.Userid;
        if (!player.IsValid(true)) return HookResult.Continue;

        _ = HandlePlayerConnectionsAsync(player);
        
        return HookResult.Continue;
    }
    public async Task HandlePlayerConnectionsAsync(CCSPlayerController Getplayer)
    {
        if (!Getplayer.IsValid(true)) return;

        Helper.CheckPlayerName(Getplayer);
        Helper.SetPlayerClan(Getplayer);

        try
        {
            var player = Getplayer;
            if (!player.IsValid(true)) return;

            if (Configs.Instance.AutoSetPlayerLanguage)
            {
                var playerip = player.IpAddress?.Split(':')[0] ?? "";
                var countryCode = Helper.GetGeoIsoCodeInfoAsync(playerip);

                Server.NextFrame(() =>
                {
                    if (player.IsValid())
                    {
                        Helper.SetPlayerLanguage(player, countryCode);
                    }
                });
            }

            await Helper.LoadPlayerData(player);
        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"HandlePlayerConnectionsAsync error: {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
        }
    }

    public void OnTick()
    {
        foreach(var getplayer in g_Main.Player_Data.Values)
        {
            if (getplayer == null) continue;

            var player = getplayer.Player;
            if (!player.IsValid() || !getplayer.PlayerName_Block) continue;

            var timeSinceLastChange = (DateTime.Now - getplayer.LastNameChangeTime).TotalSeconds;
            var totalBlock = Configs.Instance.BlockNameChanger_Block;
            var timeLeft = totalBlock - timeSinceLastChange;

            if (timeLeft < 0)
            {
                timeLeft = 0;
            }

            StringBuilder builder = new StringBuilder();
            var getLocalizer = Configs.Instance.BlockNameChanger == 1 ? "PrintToCenterToPlayer.NameChanging.Mode1.Blocked" : "PrintToCenterToPlayer.NameChanging.Mode2.Blocked";
            builder.AppendFormat(Localizer[getLocalizer, (int)Math.Ceiling(timeLeft)]);
            var centerhtml = builder.ToString();
            player.PrintToCenterHtml(centerhtml);
        }
    }
    
    public void OnMapStart(string mapname)
    {
        Helper.RemoveRegisterCommandsAndHooks();
        Helper.LoadJson();
        Helper.RegisterCommandsAndHooks();
        Helper.ExectueCommands();
        Helper.StartTimer();

        if (Configs.Instance.MySql_Enable > 0)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    if (Configs.Instance.MySql_Enable > 0)
                    {
                        await MySqlDataManager.CreateTableIfNotExistsAsync();
                    }
                }
                catch (Exception ex)
                {
                    Helper.DebugMessage($"OnMapStart Error: {ex.Message}", 0);
                }
            });
        }
    }
    
    public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        if(!string.IsNullOrEmpty(Configs.Instance.ExecuteOnEveryRoundStart))
        {
            Server.ExecuteCommand(Configs.Instance.ExecuteOnEveryRoundStart);
        }
        
        Helper.ExectueCommands();
        g_Main.CbaseWeapons?.Clear();
        Helper.StartTimer();
        Helper.ReloadCheckPlayerName();

        return HookResult.Continue;
    }
    public HookResult OnEventRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        if(!string.IsNullOrEmpty(Configs.Instance.ExecuteOnEveryRoundEnd))
        {
            Server.ExecuteCommand(Configs.Instance.ExecuteOnEveryRoundEnd);
        }

        return HookResult.Continue;
    }

    public HookResult OnEventPlayerTeam(EventPlayerTeam @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        if (Configs.Instance.Ignore_JoinTeamMessages)
        {
            info.DontBroadcast = true;
        }
        
        if (!Configs.Instance.Custom_ChatMessages) return HookResult.Continue;

        var player = @event.Userid;
        int JoinTeam = @event.Team;
        if (!player.IsValid(true)) return HookResult.Continue;
        Helper.CheckPlayerInGlobals(player);

        var GetValues = Helper.GetValuesInJson(player, "");
        if(string.IsNullOrEmpty(GetValues.JoinTeam_CT) && string.IsNullOrEmpty(GetValues.JoinTeam_T) && string.IsNullOrEmpty(GetValues.JoinTeam_SPEC)) return HookResult.Continue;

        if (Configs.Instance.Custom_JoinTeamMessages && player.IsBot) return HookResult.Continue;

        if (JoinTeam == (byte)CsTeam.Spectator)
        {
            var spec_message = GetValues.JoinTeam_SPEC?.ReplaceChatMessages(clan_chat: GetValues.ClanTag_Chat ?? "", clan_scoreboard: GetValues.ClanTag_ScoreBoard ?? "", PlayerName: player.PlayerName.RemoveColorNames(), location: player.PlayerPawn.Value?.LastPlaceName ?? "", team_color: player.TeamNum.ToTeamColor());
            Helper.AdvancedServerPrintToChatAll(spec_message!);
        }
        else if (JoinTeam == (byte)CsTeam.Terrorist)
        {
            var t_message = GetValues.JoinTeam_T?.ReplaceChatMessages(clan_chat: GetValues.ClanTag_Chat ?? "", clan_scoreboard: GetValues.ClanTag_ScoreBoard ?? "", PlayerName: player.PlayerName.RemoveColorNames(), location: player.PlayerPawn.Value?.LastPlaceName ?? "", team_color: player.TeamNum.ToTeamColor());
            Helper.AdvancedServerPrintToChatAll(t_message!);
        }
        else if (JoinTeam == (byte)CsTeam.CounterTerrorist)
        {
            var ct_message = GetValues.JoinTeam_CT?.ReplaceChatMessages(clan_chat: GetValues.ClanTag_Chat ?? "", clan_scoreboard: GetValues.ClanTag_ScoreBoard ?? "", PlayerName: player.PlayerName.RemoveColorNames(), location: player.PlayerPawn.Value?.LastPlaceName ?? "", team_color: player.TeamNum.ToTeamColor());
            Helper.AdvancedServerPrintToChatAll(ct_message!);
        }
        return HookResult.Continue;
    }

    public HookResult OnEventBombPlanted(EventBombPlanted @event, GameEventInfo info)
    {
        if (@event == null || !Configs.Instance.Ignore_BombPlantedHUDMessages) return HookResult.Continue;
        info.DontBroadcast = true;
        return HookResult.Continue;
    }
    
    public HookResult OnEventBotTakeover(EventBotTakeover @event, GameEventInfo info)
    {
        if (@event == null || !Configs.Instance.Custom_ChatMessages) return HookResult.Continue;

        var player = @event.Userid;
        if (!player.IsValid(true)) return HookResult.Continue;
        var GetValues = Helper.GetValuesInJson(player, "");
        if (string.IsNullOrEmpty(GetValues.BotTakeOver)) return HookResult.Continue;
        
        var BotTakeover = Utilities.GetPlayers().FirstOrDefault(p => p.IsValid(true) && p.OriginalControllerOfCurrentPawn.Value == player);
        if (!BotTakeover.IsValid(true)) return HookResult.Continue;
        var BotTakeOver_message = GetValues.BotTakeOver?.ReplaceChatMessages(clan_chat: GetValues.ClanTag_Chat ?? "", clan_scoreboard: GetValues.ClanTag_ScoreBoard ?? "", PlayerName: player.PlayerName.RemoveColorNames(), location: player.PlayerPawn.Value?.LastPlaceName ?? "", BOT_Controlled: BotTakeover.PlayerName, team_color: player.TeamNum.ToTeamColor());
        
        Helper.AdvancedServerPrintToChatAll(BotTakeOver_message!);
        return HookResult.Continue;
    }

    public HookResult OnEventGrenadeThrown(EventGrenadeThrown @event, GameEventInfo info)
    {
        if (@event == null || !Configs.Instance.Custom_ChatMessages) return HookResult.Continue;
        
        var getplayer = @event.Userid;
        var getnade = @event.Weapon;

        if (!getplayer.IsValid(true)) return HookResult.Continue;

        var GetValues = Helper.GetValuesInJson(getplayer, "");
        if (string.IsNullOrEmpty(GetValues.Nade_Decoy) && string.IsNullOrEmpty(GetValues.Nade_Flashbang) 
        && string.IsNullOrEmpty(GetValues.Nade_Hegrenade) && string.IsNullOrEmpty(GetValues.Nade_Incgrenade)
        && string.IsNullOrEmpty(GetValues.Nade_Molotov) && string.IsNullOrEmpty(GetValues.Nade_Smokegrenade)) return HookResult.Continue;

        if (getplayer.IsBot && (Configs.Instance.Custom_ThrowNadeMessages == 1 || Configs.Instance.Custom_ThrowNadeMessages == 3 || Configs.Instance.Custom_ThrowNadeMessages == 4)) return HookResult.Continue;

        Server.NextFrame(() =>
        {
            var player = getplayer;
            if (!player.IsValid(true)) return;
            
            var GetValues = Helper.GetValuesInJson(player, "");
            var Nade_Name = getnade.ToCustomGrenadeName(player, GetValues);
            if (string.IsNullOrEmpty(Nade_Name)) return;

            bool mp_teammates_are_enemies = ConVar.Find("mp_teammates_are_enemies")?.GetPrimitiveValue<bool>() ?? false;

            foreach (var players in Helper.GetPlayersController(true, false, false))
            {
                if (!players.IsValid(true)) continue;
                if (Configs.Instance.Custom_ThrowNadeMessages == 1 && player.TeamNum == players.TeamNum)
                {
                    Helper.AdvancedPlayerPrintToChat(players, null!, Nade_Name);
                }
                else if (Configs.Instance.Custom_ThrowNadeMessages == 2 && player.TeamNum == players.TeamNum)
                {
                    Helper.AdvancedPlayerPrintToChat(players, null!, Nade_Name);
                }
                else if (Configs.Instance.Custom_ThrowNadeMessages == 3 && mp_teammates_are_enemies && player == players)
                {
                    Helper.AdvancedPlayerPrintToChat(players, null!, Nade_Name);
                }
                else if (mp_teammates_are_enemies && (Configs.Instance.Custom_ThrowNadeMessages == 4 || Configs.Instance.Custom_ThrowNadeMessages == 5))
                {
                    Helper.AdvancedPlayerPrintToChat(players, null!, Nade_Name);
                }
            }
        });
        return HookResult.Continue;
    }
    
    public HookResult OnEventRoundMvp(EventRoundMvp @event, GameEventInfo info)
    {
        if (@event == null || Configs.Instance.Sounds_MuteMVPMusic < 1)return HookResult.Continue;

        var player = @event.Userid;
        if(!player.IsValid(true))return HookResult.Continue;

        if(Configs.Instance.Sounds_MuteMVPMusic == 1)
        {
            player.MusicKitID = 0;
            Utilities.SetStateChanged(player, "CCSPlayerController", "m_iMusicKitID");
        }else if(Configs.Instance.Sounds_MuteMVPMusic == 2)
        {
            Helper.EmitSound_World("StopSoundEvents.StopAllMusic");
        }

        return HookResult.Continue;
    }

    

    public HookResult OnJoinTeam(CCSPlayerController? player, CommandInfo command)
    {
        if (!player.IsValid() || !g_Main.Player_Data.TryGetValue(player.Slot, out var handle)) return HookResult.Continue;

        if (handle.PlayerName_Block)
        {
            var timeSinceLastChange = (DateTime.Now - handle.LastNameChangeTime).TotalSeconds;
            var totalBlock = Configs.Instance.BlockNameChanger_Block;
            var timeLeft = totalBlock - timeSinceLastChange;

            if (timeLeft < 0)
            {
                timeLeft = 0;
            }

            if (Configs.Instance.BlockNameChanger == 1)
            {
                Helper.AdvancedPlayerPrintToChat(player, null!, Localizer["PrintToChatToPlayer.NameChanging.Mode1.Blocked"], (int)Math.Ceiling(timeLeft));
            }
            else
            {
                Helper.AdvancedPlayerPrintToChat(player, null!, Localizer["PrintToChatToPlayer.NameChanging.Mode2.Blocked"], (int)Math.Ceiling(timeLeft));
            }

            return HookResult.Handled;        
        }
        else
        {
            handle.PlayerName_Count = 0;
            handle.PlayerName_Block = false;
            handle.PlayerName_Block_Message = false;
        }

        return HookResult.Continue;
    }
    
    public HookResult OnEventPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        if(@event == null)return HookResult.Continue;
        Helper.ExectueCommands();
        Helper.StartTimer();

        if (Configs.Instance.HideChatHUD_Delay > 0 || Configs.Instance.HideDeadBody > 0 || Configs.Instance.HideWeaponsHUD)
        {
            var player = @event.Userid;
            if (!player.IsValid(true)) return HookResult.Continue;

            Server.NextFrame(() =>
            {
                if (!player.IsValid(true)) return;

                Helper.HideChatHUD(player);
                Helper.HideWeaponsHUD(player);

                var getcontroller = player.CheckPlayerController();
                if (!getcontroller.IsValid(true)) return;

                Helper.RemoveHideDeadBody(getcontroller);
            });
        }

        return HookResult.Continue;
    }


    public HookResult OnEventPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;
        Helper.StartTimer();

        var victim = @event.Userid;
        if (!victim.IsValid(true)) return HookResult.Continue;

        if (Configs.Instance.Ignore_DisconnectMessages == 2)
        {
            if (victim.Connected == PlayerConnectedState.PlayerDisconnecting)
            {
                info.DontBroadcast = true;
            }
        }

        if (Configs.Instance.HideChatHUD_Delay > 0 || Configs.Instance.HideDeadBody > 0 || Configs.Instance.HideWeaponsHUD)
        {
            Server.NextFrame(() =>
            {
                if (!victim.IsValid(true)) return;

                Helper.HideChatHUD(victim);
                Helper.HideWeaponsHUD(victim);

                var getcontroller = victim.CheckPlayerController();
                if (!getcontroller.IsValid(true)) return;

                Helper.HideDeadBody(getcontroller);
            });
        }

        if (Configs.Instance.HideKillfeed > 0)
        {
            var attacker = @event.Attacker;
            if (!attacker.IsValid(true)) return HookResult.Continue;

            info.DontBroadcast = true;
            if (Configs.Instance.HideKillfeed == 2)
            {
                @event.FireEventToClient(attacker);
            }
        }
        return HookResult.Continue;
    }
    
    public HookResult OnPlayerSay(CCSPlayerController? player, CommandInfo info)
    {
        if (!player.IsValid()) return HookResult.Continue;

        var eventmessage = info.ArgString;
        eventmessage = eventmessage.TrimStart('"');
        eventmessage = eventmessage.TrimEnd('"');
        if (string.IsNullOrWhiteSpace(eventmessage)) return HookResult.Continue;

        string message = eventmessage.Trim();

        Game_UserMessages.HookPlayerChat_UserMessages(null, player, message, false);

        return HookResult.Continue;
    }
    public HookResult OnPlayerSay_Team(CCSPlayerController? player, CommandInfo info)
    {
        if (!player.IsValid()) return HookResult.Continue;

        var eventmessage = info.ArgString;
        eventmessage = eventmessage.TrimStart('"');
        eventmessage = eventmessage.TrimEnd('"');
        if (string.IsNullOrWhiteSpace(eventmessage)) return HookResult.Continue;

        string message = eventmessage.Trim();

        Game_UserMessages.HookPlayerChat_UserMessages(null, player, message, true);

        return HookResult.Continue;
    }
    public HookResult OnUserMessage_OnSayText2(CounterStrikeSharp.API.Modules.UserMessages.UserMessage um)
    {
        var entityindex = um.ReadInt("entityindex");
        var player = Utilities.GetPlayerFromIndex(entityindex);
        if (!player.IsValid()) return HookResult.Continue;

        var message_type = um.ReadString("messagename");
        var eventmessage_Bytes = um.ReadBytes("param2");
        var eventmessage = Encoding.UTF8.GetString(eventmessage_Bytes);
        if (string.IsNullOrWhiteSpace(eventmessage)) return HookResult.Continue;

        string message = eventmessage.Trim();
        bool TeamChat = false;
        if (message_type.Equals("Cstrike_Chat_CT") || message_type.Equals("Cstrike_Chat_CT_Loc") || message_type.Equals("Cstrike_Chat_T") || message_type.Equals("Cstrike_Chat_T_Loc")
        || message_type.Equals("Cstrike_Chat_Spec") || message_type.Equals("Cstrike_Chat_CT_Dead") || message_type.Equals("Cstrike_Chat_T_Dead"))
        {
            TeamChat = true;
        }

        if (g_Main.Player_Data.TryGetValue(player.Slot, out var handle))
        {
            handle.MessageType = message_type;
        }

        Game_UserMessages.HookPlayerChat_UserMessages(um, player, message, TeamChat);

        return HookResult.Continue;
    }
    
    public HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        if (Configs.Instance.Ignore_DisconnectMessages > 0)
        {
            info.DontBroadcast = true;
        }

        var player = @event.Userid;
        if (!player.IsValid()) return HookResult.Continue;
            
        if (g_Main.Player_Data.TryGetValue(player.Slot, out var handle))
        {
            handle.PlayerName_Count = 0;
            handle.PlayerName_Block = false;
            handle.PlayerName_Block_Message = false;
        }

        if (Configs.Instance.MySql_Enable == 1 || Configs.Instance.Cookies_Enable == 1)
        {
            _ = HandlePlayerDisconnectAsync(player);
        }

        return HookResult.Continue;
    }

    public async Task HandlePlayerDisconnectAsync(CCSPlayerController player)
    {
        try
        {
            if (!player.IsValid()) return;
            await Helper.SavePlayerDataOnDisconnect(player);
        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"HandlePlayerDisconnectAsync error: {ex.Message}", 0);
        }
    }

    public void OnMapEnd()
    {
        try
        {
            if (Configs.Instance.MySql_Enable > 0 || Configs.Instance.Cookies_Enable > 0)
            {
                Helper.SavePlayersValues();
            }

            Helper.ClearVariables();

            if (g_Main.OnTakeDamage_Hooked)
            {
                RemoveListener<Listeners.OnEntityTakeDamagePre>(Game_Listeners.OnEntityTakeDamagePre);
                g_Main.OnTakeDamage_Hooked = false;
            }
        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"OnMapEnd Error: {ex.Message}", 0);
        }
    }

    public override void Unload(bool hotReload)
    {
        try
        {
            Helper.RemoveRegisterCommandsAndHooks();
            Helper.ClearVariables();

        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"Unload Error: {ex.Message}", 0);
        }

        if (hotReload)
        {
            try
            {
                Helper.RemoveRegisterCommandsAndHooks();
                Helper.ClearVariables();
            }
            catch (Exception ex)
            {
                Helper.DebugMessage($"Unload hotReload Error: {ex.Message}", 0);
            }
        }
    }

    /* [ConsoleCommand("css_test", "testttt")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    public void test(CCSPlayerController? player, CommandInfo commandInfo)
    {
        if (!player.IsValid()) return;
    } */
}