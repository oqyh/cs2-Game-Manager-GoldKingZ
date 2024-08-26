using System.Drawing;
using MySqlConnector;
using Newtonsoft.Json;
using Game_Manager_GoldKingZ.Config;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Localization;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Core.Attributes;

namespace Game_Manager_GoldKingZ;

[MinimumApiVersion(260)]
public class GameManagerGoldKingZ : BasePlugin
{
    public override string ModuleName => "Game Manager (Block/Hide Unnecessaries In Game)";
    public override string ModuleVersion => "2.0.4";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "https://github.com/oqyh";
    internal static IStringLocalizer? Stringlocalizer;

    public override void Load(bool hotReload)
    {
        Configs.Load(ModuleDirectory);
        Stringlocalizer = Localizer;
        Configs.Shared.CookiesModule = ModuleDirectory;
        Configs.Shared.StringLocalizer = Localizer;
        RegisterEventHandler<EventRoundStart>(OnRoundStart);
        RegisterEventHandler<EventPlayerSpawn>(OnEventPlayerSpawn);
        RegisterEventHandler<EventPlayerDeath>(OnEventPlayerDeath, HookMode.Pre);
        RegisterEventHandler<EventRoundMvp>(OnEventRoundMvp, HookMode.Pre);
        RegisterEventHandler<EventBombPlanted>(OnEventBombPlanted, HookMode.Pre);
        RegisterEventHandler<EventPlayerConnectFull>(OnEventPlayerConnectFull);
        RegisterEventHandler<EventGrenadeThrown>(OnEventGrenadeThrown);
        RegisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnect, HookMode.Pre);
        RegisterEventHandler<EventPlayerTeam>(OnEventPlayerTeam, HookMode.Pre);
        RegisterEventHandler<EventPlayerChat>(OnEventPlayerChat, HookMode.Post);
        RegisterEventHandler<EventRoundStart>(OnEventRoundStart, HookMode.Post);
        RegisterListener<Listeners.OnMapStart>(OnMapStart);
        RegisterListener<Listeners.OnMapEnd>(OnMapEnd);

        HookUserMessage(124, um =>
        {
            for (int i = 0; i < um.GetRepeatedFieldCount("param"); i++)
            {
                var message = um.ReadString("param", i);
                
                if(Configs.GetConfigData().IgnoreDefaultTeamMateAttackMessages)
                {
                    for (int X = 0; X < Helper.TeamWarningArray.Length; X++)
                    {
                        if (message.Contains(Helper.TeamWarningArray[X]))
                        {
                            return HookResult.Stop;
                        }
                    }
                }
                
                if(Configs.GetConfigData().IgnoreDefaultAwardsMoneyMessages)
                {
                    for (int X = 0; X < Helper.MoneyMessageArray.Length; X++)
                    {
                        if (message.Contains(Helper.MoneyMessageArray[X]))
                        {
                            return HookResult.Stop;
                        }
                    }
                }

                if(Configs.GetConfigData().IgnorePlayerSavedYouByPlayerMessages)
                {
                    for (int X = 0; X < Helper.SavedbyArray.Length; X++)
                    {
                        if (message.Contains(Helper.SavedbyArray[X]))
                        {
                            return HookResult.Stop;
                        }
                    }
                }
            }
            return HookResult.Continue;
        },HookMode.Pre);

        HookUserMessage(452, um =>
        {
            if(Configs.GetConfigData().Mute_GunShotsMode == 1)
            {
                um.SetInt("sound_type", 0);
            }else if(Configs.GetConfigData().Mute_GunShotsMode == 2)
            {
                um.SetUInt("weapon_id", 0);
                um.SetInt("sound_type", 9);
                um.SetUInt("item_def_index", 60);
            }else if(Configs.GetConfigData().Mute_GunShotsMode == 3)
            {
                um.SetUInt("weapon_id", 0);
                um.SetInt("sound_type", 9);
                um.SetUInt("item_def_index", 61);
            }
            return HookResult.Continue;
        }, HookMode.Pre);
        
        AddCommandListener("playerchatwheel", CommandListener_Chatwheel);
        AddCommandListener("player_ping", CommandListener_Ping);
        for (int i = 0; i < Helper.RadioArray.Length; i++)
        {
            AddCommandListener(Helper.RadioArray[i], CommandListener_RadioCommands);
        }

        Helper.ExectueCommands();

        if(Configs.GetConfigData().AutoCleanDropWeaponsMode == 3)
        {
            if(Configs.GetConfigData().Mode3_EveryTimeXSecs != 0)
            {
                Globals.CleanerTimer?.Kill();
                Globals.CleanerTimer = null;
                Globals.CleanerTimer = AddTimer(Configs.GetConfigData().Mode3_EveryTimeXSecs, Helper.CleanerTimer_Callback, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
            }
        }
    }
    private void OnMapStart(string Map)
    {
        if(Configs.GetConfigData().AutoCleanDropWeaponsMode == 3)
        {
            if(Configs.GetConfigData().Mode3_EveryTimeXSecs != 0)
            {
                Globals.CleanerTimer?.Kill();
                Globals.CleanerTimer = null;
                Globals.CleanerTimer = AddTimer(Configs.GetConfigData().Mode3_EveryTimeXSecs, Helper.CleanerTimer_Callback, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
            }
        }
    }

    public HookResult OnEventPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo info)
    {
        if (@event == null)return HookResult.Continue;
        Helper.ExectueCommands();
        Helper.FetchAndRemoveOldJsonEntries();

        var player = @event.Userid;

        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV) return HookResult.Continue;
        var playerid = player.SteamID;

        if(Configs.GetConfigData().DisableLegsMode == 0 || Configs.GetConfigData().DisableLegsMode == 2 || Configs.GetConfigData().DisableLegsMode == 3)
        {
            if(Configs.GetConfigData().DisableLegsMode == 0)
            {
                if (!Globals.Toggle_DisableLegs.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableLegs.Add(playerid, 0);
                }
                if (Globals.Toggle_DisableLegs.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableLegs[playerid] = 0;
                }
            }else if(Configs.GetConfigData().DisableLegsMode == 2)
            {
                if (!Globals.Toggle_DisableLegs.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableLegs.Add(playerid, 1);
                }
                if (Globals.Toggle_DisableLegs.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableLegs[playerid] = 1;
                }
            }else if(Configs.GetConfigData().DisableLegsMode == 3)
            {
                if (!Globals.Toggle_DisableLegs.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableLegs.Add(playerid, 4);
                }
                if (Globals.Toggle_DisableLegs.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableLegs[playerid] = 4;
                }
            }
        }


        if(Configs.GetConfigData().DisableHUDChatMode == 0 || Configs.GetConfigData().DisableHUDChatMode == 2 || Configs.GetConfigData().DisableHUDChatMode == 3)
        {
            if(Configs.GetConfigData().DisableHUDChatMode == 0 )
            {
                if (!Globals.Toggle_DisableChat.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableChat.Add(playerid, 0);
                }
                if (Globals.Toggle_DisableChat.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableChat[playerid] = 0;
                }
            }else if(Configs.GetConfigData().DisableHUDChatMode == 2)
            {
                if (!Globals.Toggle_DisableChat.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableChat.Add(playerid, 1);
                }
                if (Globals.Toggle_DisableChat.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableChat[playerid] = 1;
                }
            }else if(Configs.GetConfigData().DisableHUDChatMode == 3)
            {
                if (!Globals.Toggle_DisableChat.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableChat.Add(playerid, 4);
                }
                if (Globals.Toggle_DisableChat.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableChat[playerid] = 4;
                }
            }
        }
        if(Configs.GetConfigData().DisableHUDWeaponsMode == 0 || Configs.GetConfigData().DisableHUDWeaponsMode == 2 || Configs.GetConfigData().DisableHUDWeaponsMode == 3)
        {
            if(Configs.GetConfigData().DisableHUDWeaponsMode == 0)
            {
                if (!Globals.Toggle_DisableWeapons.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableWeapons.Add(playerid, 0);
                }
                if (Globals.Toggle_DisableWeapons.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableWeapons[playerid] = 0;
                }
            }else if(Configs.GetConfigData().DisableHUDWeaponsMode == 2)
            {
                if (!Globals.Toggle_DisableWeapons.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableWeapons.Add(playerid, 1);
                }
                if (Globals.Toggle_DisableWeapons.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableWeapons[playerid] = 1;
                }
            }else if(Configs.GetConfigData().DisableHUDWeaponsMode == 3)
            {
                if (!Globals.Toggle_DisableWeapons.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableWeapons.Add(playerid, 4);
                }
                if (Globals.Toggle_DisableWeapons.ContainsKey(playerid))
                {
                    Globals.Toggle_DisableWeapons[playerid] = 4;
                }
            }
        }

        Helper.PersonData personData = Helper.RetrievePersonDataById(playerid);
        if (personData.PlayerSteamID != 0)
        {
            if (Configs.GetConfigData().DisableLegsMode != 0 && Globals.Toggle_DisableLegs.ContainsKey(playerid))
            {
                Globals.Toggle_DisableLegs[playerid] = personData.Disable_Legs;
            }

            if (Configs.GetConfigData().DisableHUDChatMode != 0 && Globals.Toggle_DisableChat.ContainsKey(playerid))
            {
                Globals.Toggle_DisableChat[playerid] = personData.Disable_Chat;
            }

            if (Configs.GetConfigData().DisableHUDWeaponsMode != 0 && Globals.Toggle_DisableWeapons.ContainsKey(playerid))
            {
                Globals.Toggle_DisableWeapons[playerid] = personData.Disable_Weapons;
            }
        }

        
        if(Configs.GetConfigData().Enable_UseMySql)
        {
            async Task PerformDatabaseOperationAsync()
            {
                try
                {
                    var connectionSettings = JsonConvert.DeserializeObject<MySqlDataManager.MySqlConnectionSettings>(await File.ReadAllTextAsync(Path.Combine(Path.Combine(ModuleDirectory, "config"), "MySql_Settings.json")));
                    var connectionString = new MySqlConnectionStringBuilder
                    {
                        Server = connectionSettings!.MySqlHost,
                        Port = (uint)connectionSettings.MySqlPort,
                        Database = connectionSettings.MySqlDatabase,
                        UserID = connectionSettings.MySqlUsername,
                        Password = connectionSettings.MySqlPassword
                    }.ConnectionString;

                    using (var connection = new MySqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        var personDataz = await MySqlDataManager.RetrievePersonDataByIdAsync(playerid, connection);
                        if (personDataz.PlayerSteamID != 0)
                        {
                            if (Configs.GetConfigData().DisableLegsMode != 0 && Globals.Toggle_DisableLegs.ContainsKey(playerid))
                            {
                                if(personDataz.Disable_Legs != 0)
                                {
                                    Globals.Toggle_DisableLegs[playerid] = personDataz.Disable_Legs;
                                }
                            }
                            if (Configs.GetConfigData().DisableHUDChatMode != 0 && Globals.Toggle_DisableChat.ContainsKey(playerid))
                            {
                                if(personDataz.Disable_Chat != 0)
                                {
                                    Globals.Toggle_DisableChat[playerid] = personDataz.Disable_Chat;
                                }
                            }
                            if (Configs.GetConfigData().DisableHUDWeaponsMode != 0 && Globals.Toggle_DisableWeapons.ContainsKey(playerid))
                            {
                                if(personDataz.Disable_Weapons != 0)
                                {
                                    Globals.Toggle_DisableWeapons[playerid] = personDataz.Disable_Weapons;
                                }
                            }
                            Helper.SaveToJsonFile(player.SteamID, personDataz.Disable_Weapons, personDataz.Disable_Legs, personDataz.Disable_Weapons, DateTime.Now);
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"======================== ERROR =============================");
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine($"======================== ERROR =============================");
                }
            }

            Task.Run(PerformDatabaseOperationAsync);
        }
        
        return HookResult.Continue;
    }
    public HookResult OnEventGrenadeThrown(EventGrenadeThrown @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        var player = @event.Userid;
        var nade = @event.Weapon;

        if (Configs.GetConfigData().CustomThrowNadeMessagesMode == 0 || player == null || !player.IsValid || Configs.GetConfigData().CustomThrowNadeMessagesMode == 1 && player.IsBot)return HookResult.Continue;

        Server.NextFrame(() => {
            var playerteam = player.TeamNum;
            var allplayers = Helper.GetAllController2();

            allplayers.ForEach(players => {
                var otherteam = players.TeamNum;
                bool sameTeam = playerteam == otherteam;
                bool teammatesAreEnemies = ConVar.Find("mp_teammates_are_enemies")!.GetPrimitiveValue<bool>();

                if (sameTeam && !teammatesAreEnemies) {
                    Helper.SendGrenadeMessage(nade, players, player.PlayerName);
                } else if (sameTeam && player != players ) {
                    return;
                } else if (sameTeam && (Configs.GetConfigData().CustomThrowNadeMessagesMode == 3 || Configs.GetConfigData().CustomThrowNadeMessagesMode == 4)) {
                    Helper.SendGrenadeMessage(nade, players, player.PlayerName);
                }
            });
        });
        return HookResult.Continue;
    }
    

    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        if(@event == null)return HookResult.Continue;
        Helper.ExectueCommands();

        if(Configs.GetConfigData().AutoCleanDropWeaponsMode != 1)return HookResult.Continue;

        Server.NextFrame(() =>
        {
            AddTimer(Configs.GetConfigData().Mode1_TimeXSecsDelayClean, () =>
            {
                if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("1"))
                {
                    Helper.RemoveWeapons();
                }

                if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("2"))
                {
                    Helper.RemoveGrenades();
                }

                if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("3"))
                {
                    Helper.RemoveDefuserKit();
                }

                if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("4"))
                {
                    Helper.RemoveTaser();
                }

                if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("5"))
                {
                    Helper.RemoveHealthShot();
                }

                if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("6"))
                {
                    Helper.RemoveKnifes();
                }
            },TimerFlags.STOP_ON_MAPCHANGE);
        });


        return HookResult.Continue;
    }
    private HookResult OnEventPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        if(@event == null)return HookResult.Continue;
        Helper.ExectueCommands();

        var player = @event.Userid;
        if(player == null || !player.IsValid)return HookResult.Continue;
        var PlayerSteamID = player.SteamID;

        if(Configs.GetConfigData().DisableDeadBodyMode == 1 || Configs.GetConfigData().DisableDeadBodyMode == 2 || Configs.GetConfigData().DisableDeadBodyMode == 3)
        {
            if (player.PlayerPawn == null
            || !player.PlayerPawn.IsValid
            || player.PlayerPawn.Value == null
            || !player.PlayerPawn.Value.IsValid)
            {
                return HookResult.Continue;
            }
            if(Globals.TimerRemoveDeadBody.ContainsKey(player))
            {
                Globals.TimerRemoveDeadBody[player]?.Kill();
                Globals.TimerRemoveDeadBody[player] = null!;
                Globals.TimerRemoveDeadBody.Remove(player);
            }
            if(Globals.PlayerAlpha.ContainsKey(player))
            {
                Globals.PlayerAlpha.Remove(player);
            }
            player.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 255, 255);
            Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
        }
        
        if(Configs.GetConfigData().AutoCleanDropWeaponsMode == 2)
        {
            Server.NextFrame(() =>
            {
                AddTimer(Configs.GetConfigData().Mode2_TimeXSecsDelayClean, () =>
                {
                    if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("1"))
                    {
                        Helper.RemoveWeapons();
                    }

                    if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("2"))
                    {
                        Helper.RemoveGrenades();
                    }

                    if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("3"))
                    {
                        Helper.RemoveDefuserKit();
                    }

                    if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("4"))
                    {
                        Helper.RemoveTaser();
                    }

                    if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("5"))
                    {
                        Helper.RemoveHealthShot();
                    }

                    if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("6"))
                    {
                        Helper.RemoveKnifes();
                    }
                },TimerFlags.STOP_ON_MAPCHANGE);
            });
        }

        if(Configs.GetConfigData().DisableLegsMode == 1 || Configs.GetConfigData().DisableLegsMode == 2 || Configs.GetConfigData().DisableLegsMode == 3)
        {
            Server.NextFrame(() =>
            {
                if(Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 2 || Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 4 || Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 6)
                {
                    if (player == null
                    || !player.IsValid
                    || player.PlayerPawn == null
                    || !player.PlayerPawn.IsValid
                    || player.PlayerPawn.Value == null
                    || !player.PlayerPawn.Value.IsValid)
                    {
                        return;
                    }
                    
                    player.PlayerPawn.Value.Render = Color.FromArgb(254, 254, 254, 254);

                }else if(Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 1 || Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 3 || Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 5)
                {
                    if (player == null
                    || !player.IsValid
                    || player.PlayerPawn == null
                    || !player.PlayerPawn.IsValid
                    || player.PlayerPawn.Value == null
                    || !player.PlayerPawn.Value.IsValid)
                    {
                        return;
                    }
                    
                    player.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 255, 255);
                }

                if(Configs.GetConfigData().DisableLegsMode == 1)
                {
                    if (player == null
                    || !player.IsValid
                    || player.PlayerPawn == null
                    || !player.PlayerPawn.IsValid
                    || player.PlayerPawn.Value == null
                    || !player.PlayerPawn.Value.IsValid)
                    {
                        return;
                    }
                    
                    player.PlayerPawn.Value.Render = Color.FromArgb(254, 254, 254, 254);
                }
            });
        }

        if(Configs.GetConfigData().DisableHUDChatMode == 1 || Configs.GetConfigData().DisableHUDChatMode == 2 || Configs.GetConfigData().DisableHUDChatMode == 3)
        {
            Server.NextFrame(() =>
            {
                if(Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 2 || Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 4 || Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 6)
                {
                    if(Globals.Toggle_OnDisableChat.ContainsKey(PlayerSteamID))return;

                    if (!Globals.Toggle_OnDisableChat.ContainsKey(PlayerSteamID))
                    {
                        Globals.Toggle_OnDisableChat.Add(PlayerSteamID, true);
                    }
                    if (Globals.Toggle_OnDisableChat.ContainsKey(PlayerSteamID))
                    {
                        Globals.Toggle_OnDisableChat[PlayerSteamID] = true;
                    }

                    if (!string.IsNullOrEmpty(Localizer["hidechat.enabled.warning"]))
                    {
                        Helper.AdvancedPrintToChat(player, Localizer["hidechat.enabled.warning"], Configs.GetConfigData().DisableHUDChatModeWarningTimerInSecs);
                    }
                    AddTimer(Configs.GetConfigData().DisableHUDChatModeWarningTimerInSecs, () =>
                    {
                        if (player == null || !player.IsValid)return;
                        var playerPawn = player.Pawn.Value;
                        if(playerPawn == null || !playerPawn.IsValid)return;
                        playerPawn.HideHUD |= Globals.HIDECHAT;
                        Utilities.SetStateChanged(playerPawn, "CBasePlayerPawn", "m_iHideHUD");
                        Globals.Toggle_OnDisableChat.Remove(PlayerSteamID);
                    },TimerFlags.STOP_ON_MAPCHANGE);
                }else if(Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 1 || Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 3 || Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 5)
                {
                    var playerPawn = player.Pawn.Value;
                    if(playerPawn == null || !playerPawn.IsValid)return;
                    playerPawn.HideHUD &= ~Globals.HIDECHAT;
                    Utilities.SetStateChanged(playerPawn, "CBasePlayerPawn", "m_iHideHUD");
                }

                if(Configs.GetConfigData().DisableHUDChatMode == 1)
                {
                    if (!string.IsNullOrEmpty(Localizer["hidechat.enabled.warning"]))
                    {
                        Helper.AdvancedPrintToChat(player, Localizer["hidechat.enabled.warning"], Configs.GetConfigData().DisableHUDChatModeWarningTimerInSecs);
                    }
                    AddTimer(Configs.GetConfigData().DisableHUDChatModeWarningTimerInSecs, () =>
                    {
                        if (player == null || !player.IsValid)return;
                        var playerPawn = player.Pawn.Value;
                        if(playerPawn == null || !playerPawn.IsValid)return;
                        playerPawn.HideHUD |= Globals.HIDECHAT;
                        Utilities.SetStateChanged(playerPawn, "CBasePlayerPawn", "m_iHideHUD");
                    },TimerFlags.STOP_ON_MAPCHANGE);
                }
            });
        }

        if(Configs.GetConfigData().DisableHUDWeaponsMode == 1 || Configs.GetConfigData().DisableHUDWeaponsMode == 2 || Configs.GetConfigData().DisableHUDWeaponsMode == 3)
        {
            Server.NextFrame(() =>
            {
                if(Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 2 || Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 4 || Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 6)
                {
                    if(Globals.Toggle_OnDisableWeapons.ContainsKey(PlayerSteamID))return;

                    if (!Globals.Toggle_OnDisableWeapons.ContainsKey(PlayerSteamID))
                    {
                        Globals.Toggle_OnDisableWeapons.Add(PlayerSteamID, true);
                    }
                    if (Globals.Toggle_OnDisableWeapons.ContainsKey(PlayerSteamID))
                    {
                        Globals.Toggle_OnDisableWeapons[PlayerSteamID] = true;
                    }

                    if (player == null || !player.IsValid)return;
                    var playerPawn = player.Pawn.Value;
                    if(playerPawn == null || !playerPawn.IsValid)return;
                    playerPawn.HideHUD |= Globals.HIDEWEAPONS;
                    Utilities.SetStateChanged(playerPawn, "CBasePlayerPawn", "m_iHideHUD");
                    Globals.Toggle_OnDisableWeapons.Remove(PlayerSteamID);
                }else if(Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 1 || Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 3 || Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 5)
                {
                    var playerPawn = player.Pawn.Value;
                    if(playerPawn == null || !playerPawn.IsValid)return;
                    playerPawn.HideHUD &= ~Globals.HIDEWEAPONS;
                    Utilities.SetStateChanged(playerPawn, "CBasePlayerPawn", "m_iHideHUD");
                }

                if(Configs.GetConfigData().DisableHUDWeaponsMode == 1)
                {
                    if (player == null || !player.IsValid)return;
                    var playerPawn = player.Pawn.Value;
                    if(playerPawn == null || !playerPawn.IsValid)return;
                    playerPawn.HideHUD |= Globals.HIDEWEAPONS;
                    Utilities.SetStateChanged(playerPawn, "CBasePlayerPawn", "m_iHideHUD");
                }
            });
        }
        return HookResult.Continue;
    }
    private HookResult OnEventRoundMvp(EventRoundMvp @event, GameEventInfo info)
    {
        if (!Configs.GetConfigData().DisableMPVSound || @event == null)return HookResult.Continue;
        var Player = @event.Userid;
        if (Player == null || !Player.IsValid)return HookResult.Continue;
        Player.MusicKitID = 0;
        return HookResult.Continue;
    }
    private HookResult OnEventBombPlanted(EventBombPlanted @event, GameEventInfo info)
    {
        if (!Configs.GetConfigData().IgnoreDefaultBombPlantedAnnounce || @event == null)return HookResult.Continue;
        info.DontBroadcast = true;
        return HookResult.Continue;
    }

    private HookResult OnEventPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        if(@event == null)return HookResult.Continue;
        Helper.ExectueCommands();

        var player = @event.Userid;
        if(player == null || !player.IsValid )return HookResult.Continue;

        if (Configs.GetConfigData().IgnoreDefaultDisconnectMessagesMode == 2)
        {
            if (Globals.Remove_Icon.ContainsKey(player.SteamID))
            {
                info.DontBroadcast = true;
                Globals.Remove_Icon.Remove(player.SteamID);
            }
        }

        if(Configs.GetConfigData().DisableDeadBodyMode == 1)
        {
            if (player.PlayerPawn == null
            || !player.PlayerPawn.IsValid
            || player.PlayerPawn.Value == null
            || !player.PlayerPawn.Value.IsValid)
            {
                return HookResult.Continue;
            }
            player.PlayerPawn.Value.Render = Color.FromArgb(0, 255, 255, 255);
            Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
        }else if(Configs.GetConfigData().DisableDeadBodyMode == 2)
        {
            Server.NextFrame(() =>
            {
                AddTimer(1.00f, () =>
                {
                    AddTimer(Configs.GetConfigData().Mode2_TimeXSecsDelayRemoveDeadBody, () =>
                    {
                        if (player == null
                        ||  !player.IsValid
                        ||  player.PlayerPawn == null
                        ||  !player.PlayerPawn.IsValid
                        ||  player.PlayerPawn.Value == null
                        ||  !player.PlayerPawn.Value.IsValid
                        ||  player.PawnIsAlive)
                        {
                            return;
                        }
                        player.PlayerPawn.Value.Render = Color.FromArgb(0, 255, 255, 255);
                        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
                    }, TimerFlags.STOP_ON_MAPCHANGE);
                }, TimerFlags.STOP_ON_MAPCHANGE);
            });
        }else if(Configs.GetConfigData().DisableDeadBodyMode == 3)
        {
            if (player.PlayerPawn == null
            || !player.PlayerPawn.IsValid
            || player.PlayerPawn.Value == null
            || !player.PlayerPawn.Value.IsValid)
            {
                return HookResult.Continue;
            }
            if (!Globals.PlayerAlpha.ContainsKey(player))
            {
                Globals.PlayerAlpha.Add(player, 255);
            }
            if (!Globals.TimerRemoveDeadBody.ContainsKey(player))
            {
                CounterStrikeSharp.API.Modules.Timers.Timer timer = RemoveDeadBody(player);
                Globals.TimerRemoveDeadBody.Add(player, timer);
            }
        }

        if(Configs.GetConfigData().AutoCleanDropWeaponsMode == 2)
        {
            Server.NextFrame(() =>
            {
                AddTimer(Configs.GetConfigData().Mode2_TimeXSecsDelayClean, () =>
                {
                    if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("1"))
                    {
                        Helper.RemoveWeapons();
                    }

                    if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("2"))
                    {
                        Helper.RemoveGrenades();
                    }

                    if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("3"))
                    {
                        Helper.RemoveDefuserKit();
                    }

                    if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("4"))
                    {
                        Helper.RemoveTaser();
                    }

                    if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("5"))
                    {
                        Helper.RemoveHealthShot();
                    }

                    if (Configs.GetConfigData().AutoCleanTheseDroppedWeaponsOnly.Contains("6"))
                    {
                        Helper.RemoveKnifes();
                    }
                },TimerFlags.STOP_ON_MAPCHANGE);
            });
        }

        var Attacker = @event.Attacker;
        if(Attacker == null || !Attacker.IsValid)return HookResult.Continue;

        if(Configs.GetConfigData().DisableKillfeedMode == 1 || Configs.GetConfigData().DisableKillfeedMode == 2)
        {
            info.DontBroadcast = true;
        }
        if(Configs.GetConfigData().DisableKillfeedMode == 2)
        {
            @event.FireEventToClient(Attacker);
        }

        return HookResult.Continue;
    }
    public CounterStrikeSharp.API.Modules.Timers.Timer RemoveDeadBody(CCSPlayerController player)
    {
        CounterStrikeSharp.API.Modules.Timers.Timer timer = null!;
        timer = AddTimer(Configs.GetConfigData().Mode3_TimeXSecsDecayDeadBody, () =>
        {
            if (player == null || !player.IsValid || player.PlayerPawn == null || !player.PlayerPawn.IsValid || player.PlayerPawn.Value == null || !player.PlayerPawn.Value.IsValid)
            {
                return;
            }

            if (Globals.PlayerAlpha.ContainsKey(player))
            {
                Globals.PlayerAlpha[player]--;
                if (Globals.PlayerAlpha[player] < 0)
                {
                    Server.NextFrame(() =>
                    {
                        if(Globals.TimerRemoveDeadBody.ContainsKey(player))
                        {
                            Globals.TimerRemoveDeadBody[player]?.Kill();
                            Globals.TimerRemoveDeadBody[player] = null!;
                            Globals.TimerRemoveDeadBody.Remove(player);
                        }
                        if(Globals.PlayerAlpha.ContainsKey(player))
                        {
                            Globals.PlayerAlpha.Remove(player);
                        }
                    });
                }
                Color newColor = Color.FromArgb(Globals.PlayerAlpha[player], 255, 255, 255);
                player.PlayerPawn.Value.Render = newColor;
                Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
            }
        }, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);

        return timer;
    }

    public HookResult OnEventRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        if(@event == null)return HookResult.Continue;

        return HookResult.Continue;
    }
    public HookResult OnEventPlayerChat(EventPlayerChat @event, GameEventInfo info)
    {
        if(@event == null)return HookResult.Continue;
        Helper.ExectueCommands();

        var eventplayer = @event.Userid;
        var eventmessage = @event.Text;
        var Player = Utilities.GetPlayerFromUserid(eventplayer);

        if (Player == null || !Player.IsValid)return HookResult.Continue;
        
        var PlayerTeam = Player.TeamNum;
        var PlayerSteamID = Player.SteamID;

        if (string.IsNullOrWhiteSpace(eventmessage)) return HookResult.Continue;
        string trimmedMessageStart = eventmessage.TrimStart();
        string message = trimmedMessageStart.TrimEnd();
        
        if(Configs.GetConfigData().DisableLegsMode == 2 || Configs.GetConfigData().DisableLegsMode == 3)
        {
            string[] ToggleDisableLegsCommandsInGame = Configs.GetConfigData().Toggle_DisableLegsCommandsInGame.Split(',');
            if (ToggleDisableLegsCommandsInGame.Any(cmd => cmd.Equals(message, StringComparison.OrdinalIgnoreCase)))
            {
                if (!string.IsNullOrEmpty(Configs.GetConfigData().Toggle_DisableLegsFlags) && !Helper.IsPlayerInGroupPermission(Player, Configs.GetConfigData().Toggle_DisableLegsFlags))
                {
                    if (!string.IsNullOrEmpty(Localizer["hidelegs.not.allowed"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["hidelegs.not.allowed"]);
                    }
                    return HookResult.Continue;
                }

                if (Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 1 || Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 3)
                {
                    Globals.Toggle_DisableLegs[PlayerSteamID] = 2;
                    if (!string.IsNullOrEmpty(Localizer["hidelegs.disabled"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["hidelegs.disabled"]);
                    }
                }else if (Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 4 || Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 6)
                {
                    Globals.Toggle_DisableLegs[PlayerSteamID] = 5;
                    if (!string.IsNullOrEmpty(Localizer["hidelegs.enabled"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["hidelegs.enabled"]);
                    }
                }else if (Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 2)
                {
                    Globals.Toggle_DisableLegs[PlayerSteamID] = 3;
                    if (!string.IsNullOrEmpty(Localizer["hidelegs.enabled"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["hidelegs.enabled"]);
                    }
                }else if (Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 5)
                {
                    Globals.Toggle_DisableLegs[PlayerSteamID] = 6;
                    if (!string.IsNullOrEmpty(Localizer["hidelegs.disabled"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["hidelegs.disabled"]);
                    }
                }
                

                if(Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 2 || Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 6)
                {
                    if (Player == null
                    || !Player.IsValid
                    || Player.PlayerPawn == null
                    || !Player.PlayerPawn.IsValid
                    || Player.PlayerPawn.Value == null
                    || !Player.PlayerPawn.Value.IsValid)
                    {
                        return HookResult.Continue;
                    }
                    
                    Player.PlayerPawn.Value.Render = Color.FromArgb(254, 254, 254, 254);
                    Utilities.SetStateChanged(Player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
                    
                }else if(Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 3 || Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 5)
                {
                    if (Player == null
                    || !Player.IsValid
                    || Player.PlayerPawn == null
                    || !Player.PlayerPawn.IsValid
                    || Player.PlayerPawn.Value == null
                    || !Player.PlayerPawn.Value.IsValid)
                    {
                        return HookResult.Continue;
                    }
                    
                    Player.PlayerPawn.Value.Render = Color.FromArgb(255, 255, 255, 255);
                    Utilities.SetStateChanged(Player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
                }
            }
        }

        if(Configs.GetConfigData().DisableHUDChatMode == 2 || Configs.GetConfigData().DisableHUDChatMode == 3)
        {
            if(Globals.Toggle_OnDisableChat.ContainsKey(Player.SteamID))return HookResult.Continue;

            string[] ToggleDisableChatCommandsInGame = Configs.GetConfigData().Toggle_DisableHUDChatCommandsInGame.Split(',');
            if (ToggleDisableChatCommandsInGame.Any(cmd => cmd.Equals(message, StringComparison.OrdinalIgnoreCase)))
            {
                if (!string.IsNullOrEmpty(Configs.GetConfigData().Toggle_DisableHUDChatFlags) && !Helper.IsPlayerInGroupPermission(Player, Configs.GetConfigData().Toggle_DisableHUDChatFlags))
                {
                    if (!string.IsNullOrEmpty(Localizer["hidechat.not.allowed"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["hidechat.not.allowed"]);
                    }
                    return HookResult.Continue;
                }

                if (Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 1 || Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 3)
                {
                    Globals.Toggle_DisableChat[PlayerSteamID] = 2;
                    if (!string.IsNullOrEmpty(Localizer["hidechat.disabled"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["hidechat.disabled"]);
                    }
                }else if (Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 4 || Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 6)
                {
                    Globals.Toggle_DisableChat[PlayerSteamID] = 5;
                    if (!string.IsNullOrEmpty(Localizer["hidechat.enabled"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["hidechat.enabled"]);
                    }
                }else if (Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 2)
                {
                    Globals.Toggle_DisableChat[PlayerSteamID] = 3;
                    if (!string.IsNullOrEmpty(Localizer["hidechat.enabled"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["hidechat.enabled"]);
                    }
                }else if (Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 5)
                {
                    Globals.Toggle_DisableChat[PlayerSteamID] = 6;
                    if (!string.IsNullOrEmpty(Localizer["hidechat.disabled"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["hidechat.disabled"]);
                    }
                }

                if(Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 2 || Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 6)
                {
                    Server.NextFrame(() =>
                    {
                        if (!Globals.Toggle_OnDisableChat.ContainsKey(Player.SteamID))
                        {
                            Globals.Toggle_OnDisableChat.Add(Player.SteamID, true);
                        }
                        if (Globals.Toggle_OnDisableChat.ContainsKey(Player.SteamID))
                        {
                            Globals.Toggle_OnDisableChat[Player.SteamID] = true;
                        }
                        if (!string.IsNullOrEmpty(Localizer["hidechat.enabled.warning"]))
                        {
                            Helper.AdvancedPrintToChat(Player, Localizer["hidechat.enabled.warning"], Configs.GetConfigData().DisableHUDChatModeWarningTimerInSecs);
                        }
                        AddTimer(Configs.GetConfigData().DisableHUDChatModeWarningTimerInSecs, () =>
                        {
                            if (Player == null || !Player.IsValid)return;
                            var playerPawn = Player.Pawn.Value;
                            if(playerPawn == null || !playerPawn.IsValid)return;
                            playerPawn.HideHUD |= Globals.HIDECHAT;
                            Utilities.SetStateChanged(playerPawn, "CBasePlayerPawn", "m_iHideHUD");
                            Globals.Toggle_OnDisableChat.Remove(Player.SteamID);
                        },TimerFlags.STOP_ON_MAPCHANGE);
                    });
                }else if(Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 3 || Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 5)
                {
                    var playerPawn = Player.Pawn.Value;
                    if(playerPawn == null || !playerPawn.IsValid)return HookResult.Continue;
                    playerPawn.HideHUD &= ~Globals.HIDECHAT;
                    Utilities.SetStateChanged(playerPawn, "CBasePlayerPawn", "m_iHideHUD");
                }
            }
        }
        if(Configs.GetConfigData().DisableHUDWeaponsMode == 2 || Configs.GetConfigData().DisableHUDWeaponsMode == 3)
        {
            if(Globals.Toggle_OnDisableWeapons.ContainsKey(Player.SteamID))return HookResult.Continue;

            string[] ToggleDisableWeaponsCommandsInGame = Configs.GetConfigData().Toggle_DisableHUDWeaponsCommandsInGame.Split(',');
            if (ToggleDisableWeaponsCommandsInGame.Any(cmd => cmd.Equals(message, StringComparison.OrdinalIgnoreCase)))
            {
                if (!string.IsNullOrEmpty(Configs.GetConfigData().Toggle_DisableHUDWeaponsFlags) && !Helper.IsPlayerInGroupPermission(Player, Configs.GetConfigData().Toggle_DisableHUDWeaponsFlags))
                {
                    if (!string.IsNullOrEmpty(Localizer["hideweapons.not.allowed"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["hideweapons.not.allowed"]);
                    }
                    return HookResult.Continue;
                }

                if (Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 1 || Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 3)
                {
                    Globals.Toggle_DisableWeapons[PlayerSteamID] = 2;
                    if (!string.IsNullOrEmpty(Localizer["hideweapons.disabled"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["hideweapons.disabled"]);
                    }
                }else if (Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 4 || Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 6)
                {
                    Globals.Toggle_DisableWeapons[PlayerSteamID] = 5;
                    if (!string.IsNullOrEmpty(Localizer["hideweapons.enabled"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["hideweapons.enabled"]);
                    }
                }else if (Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 2)
                {
                    Globals.Toggle_DisableWeapons[PlayerSteamID] = 3;
                    if (!string.IsNullOrEmpty(Localizer["hideweapons.enabled"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["hideweapons.enabled"]);
                    }
                }else if (Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 5)
                {
                    Globals.Toggle_DisableWeapons[PlayerSteamID] = 6;
                    if (!string.IsNullOrEmpty(Localizer["hideweapons.disabled"]))
                    {
                        Helper.AdvancedPrintToChat(Player, Localizer["hideweapons.disabled"]);
                    }
                }

                if(Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 2 || Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 6)
                {
                    Server.NextFrame(() =>
                    {
                        if (!Globals.Toggle_OnDisableWeapons.ContainsKey(Player.SteamID))
                        {
                            Globals.Toggle_OnDisableWeapons.Add(Player.SteamID, true);
                        }
                        if (Globals.Toggle_OnDisableWeapons.ContainsKey(Player.SteamID))
                        {
                            Globals.Toggle_OnDisableWeapons[Player.SteamID] = true;
                        }
                        if (Player == null || !Player.IsValid)return;
                        var playerPawn = Player.Pawn.Value;
                        if(playerPawn == null || !playerPawn.IsValid)return;
                        playerPawn.HideHUD |= Globals.HIDEWEAPONS;
                        Utilities.SetStateChanged(playerPawn, "CBasePlayerPawn", "m_iHideHUD");
                        Globals.Toggle_OnDisableWeapons.Remove(Player.SteamID);
                    });
                }else if(Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 3 || Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 5)
                {
                    var playerPawn = Player.Pawn.Value;
                    if(playerPawn == null || !playerPawn.IsValid)return HookResult.Continue;
                    playerPawn.HideHUD &= ~Globals.HIDEWEAPONS;
                    Utilities.SetStateChanged(playerPawn, "CBasePlayerPawn", "m_iHideHUD");
                }
            }
        }
        return HookResult.Continue;
    }


    private HookResult CommandListener_RadioCommands(CCSPlayerController? player, CommandInfo info)
    {
        if(!Configs.GetConfigData().DisableRadio)return HookResult.Continue;
        return HookResult.Handled;
    }
    private HookResult CommandListener_Chatwheel(CCSPlayerController? player, CommandInfo info)
    {
        if(!Configs.GetConfigData().DisableChatWheel)return HookResult.Continue;
        return HookResult.Handled;
    }
    private HookResult CommandListener_Ping(CCSPlayerController? player, CommandInfo info)
    {
        if(!Configs.GetConfigData().DisablePing)return HookResult.Continue;
        return HookResult.Handled;
    }
    public HookResult OnEventPlayerTeam(EventPlayerTeam @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        if (Configs.GetConfigData().IgnoreDefaultJoinTeamMessages)
        {
            info.DontBroadcast = true;
        }

        var player = @event.Userid;
        int JoinTeam = @event.Team;

        if (player == null || !player.IsValid || player.IsBot && Configs.GetConfigData().CustomJoinTeamMessagesMode == 1 || player.IsHLTV) return HookResult.Continue;
        if(Configs.GetConfigData().CustomJoinTeamMessagesMode == 1 || Configs.GetConfigData().CustomJoinTeamMessagesMode == 2)
        {
            var Playername = player.PlayerName;

            if(JoinTeam == 1)
            {
                if (!string.IsNullOrEmpty(Localizer["custom.jointeam.spec"]))
                {
                    Helper.AdvancedPrintToServer(Localizer["custom.jointeam.spec"], Playername);
                }
            }else if(JoinTeam == 2)
            {
                if (!string.IsNullOrEmpty(Localizer["custom.jointeam.t"]))
                {
                    Helper.AdvancedPrintToServer(Localizer["custom.jointeam.t"], Playername);
                }
            }else if(JoinTeam == 3)
            {
                if (!string.IsNullOrEmpty(Localizer["custom.jointeam.ct"]))
                {
                    Helper.AdvancedPrintToServer(Localizer["custom.jointeam.ct"], Playername);
                }
            }
        }
        return HookResult.Continue;
    }

    public HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        if (Configs.GetConfigData().IgnoreDefaultDisconnectMessagesMode == 1 || Configs.GetConfigData().IgnoreDefaultDisconnectMessagesMode == 2)
        {
            info.DontBroadcast = true;
        }

        var player = @event.Userid;

        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV) return HookResult.Continue;
        var playerid = player.SteamID;

        if (Configs.GetConfigData().IgnoreDefaultDisconnectMessagesMode == 2 )
        {
            if (!Globals.Remove_Icon.ContainsKey(playerid))
            {
                Globals.Remove_Icon.Add(playerid, true);
            }
            if (Globals.Remove_Icon.ContainsKey(playerid))
            {
                Globals.Remove_Icon[playerid] = true;
            }
        }

        Helper.FetchAndRemoveOldJsonEntries();
        Helper.PersonData personData = Helper.RetrievePersonDataById(playerid);

        bool valueChangedLegs = Globals.Toggle_DisableLegs.ContainsKey(playerid) && (Globals.Toggle_DisableLegs[playerid] == 2 || Globals.Toggle_DisableLegs[playerid] == 3 || Globals.Toggle_DisableLegs[playerid] == 5 || Globals.Toggle_DisableLegs[playerid] == 6);
        bool valueChangedChat = Globals.Toggle_DisableChat.ContainsKey(playerid) && (Globals.Toggle_DisableChat[playerid] == 2 || Globals.Toggle_DisableChat[playerid] == 3 || Globals.Toggle_DisableChat[playerid] == 5 || Globals.Toggle_DisableChat[playerid] == 6);
        bool valueChangedWeapons = Globals.Toggle_DisableWeapons.ContainsKey(playerid) && (Globals.Toggle_DisableWeapons[playerid] == 2 || Globals.Toggle_DisableWeapons[playerid] == 3 || Globals.Toggle_DisableWeapons[playerid] == 5 || Globals.Toggle_DisableWeapons[playerid] == 6);

        if (valueChangedLegs || valueChangedChat || valueChangedWeapons)
        {
            Helper.SaveToJsonFile(playerid, Globals.Toggle_DisableChat[playerid], Globals.Toggle_DisableLegs[playerid], Globals.Toggle_DisableWeapons[playerid], DateTime.Now);
        }

        if (Configs.GetConfigData().Enable_UseMySql)
        {
            Task.Run(async () =>
            {
                try
                {
                    var connectionSettings = JsonConvert.DeserializeObject<MySqlDataManager.MySqlConnectionSettings>(
                        await File.ReadAllTextAsync(Path.Combine(Path.Combine(ModuleDirectory, "config"), "MySql_Settings.json"))
                    );
                    var connectionString = new MySqlConnectionStringBuilder
                    {
                        Server = connectionSettings!.MySqlHost,
                        Port = (uint)connectionSettings.MySqlPort,
                        Database = connectionSettings.MySqlDatabase,
                        UserID = connectionSettings.MySqlUsername,
                        Password = connectionSettings.MySqlPassword
                    }.ConnectionString;

                    using (var connection = new MySqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        await MySqlDataManager.CreatePersonDataTableIfNotExistsAsync(connection);

                        DateTime personDate = DateTime.Now;
                        var personDataz = Helper.RetrievePersonDataById(playerid);

                        if (personDataz.PlayerSteamID != 0)
                        {
                            if (valueChangedLegs || valueChangedChat || valueChangedWeapons)
                            {
                                await MySqlDataManager.SaveToMySqlAsync(
                                    playerid, personDataz.Disable_Chat, personDataz.Disable_Legs, personDataz.Disable_Weapons, personDate, connection, connectionSettings
                                );
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"======================== ERROR =============================");
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine($"======================== ERROR =============================");
                }
            });
        }

        Globals.Toggle_DisableLegs.Remove(playerid);
        Globals.Toggle_DisableChat.Remove(playerid);
        Globals.Toggle_OnDisableChat.Remove(playerid);
        Globals.Toggle_DisableWeapons.Remove(playerid);
        Globals.Toggle_OnDisableWeapons.Remove(playerid);
        Globals.Remove_Icon.Remove(playerid);
        Globals.TimerRemoveDeadBody.Remove(player);
        Globals.PlayerAlpha.Remove(player);
        return HookResult.Continue;
    }

    public void OnMapEnd()
    {
        Helper.ClearVariables();
    }
    public override void Unload(bool hotReload)
    {
        Helper.ClearVariables();
    }
}