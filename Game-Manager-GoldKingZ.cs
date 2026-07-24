using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Localization;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Timers;
using Game_Manager_GoldKingZ.Config;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using System.Numerics;
using System.Text;
using System.Runtime.InteropServices;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using ClientPrefs_GoldKingZ.Shared;
using System.Runtime.CompilerServices;


namespace Game_Manager_GoldKingZ;


public sealed class ClientPrefs
{
    public int Toggle_AimPunch { get; set; } = Configs.Instance.Disable_AimPunch.DisableAimPunch switch
    {
        2 => 1,
        3 => 2,
        _ => 0
    };

    public int Toggle_Custom_MuteSounds1 { get; set; } = Configs.Instance.Custom_MuteSounds_1.Custom_MuteSounds1 switch
    {
        2 => 1,
        3 => 2,
        _ => 0
    };
    public int Toggle_Custom_MuteSounds2 { get; set; } = Configs.Instance.Custom_MuteSounds_2.Custom_MuteSounds2 switch
    {
        2 => 1,
        3 => 2,
        _ => 0
    };
    public int Toggle_Custom_MuteSounds3 { get; set; } = Configs.Instance.Custom_MuteSounds_3.Custom_MuteSounds3 switch
    {
        2 => 1,
        3 => 2,
        _ => 0
    };
}


public class MainPlugin : BasePlugin
{
    public override string ModuleName => "Game Manager (Block/Hide Unnecessaries In Game)";
    public override string ModuleVersion => "2.1.6";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "https://github.com/oqyh";
    public static MainPlugin Instance { get; set; } = new();
    public readonly Game_Listeners Game_Listeners = new();
    public readonly Game_UserMessages Game_UserMessages = new();
    public Globals g_Main = new();
    public IPrefsStore<ClientPrefs>? _prefs;

    public override void Load(bool hotReload)
    {
        Instance = this;
        Configs.Load(ModuleDirectory);
        
        Helper.RemoveRegisterCommandsAndHooks();
        Helper.RemoveOnEntityTakeDamagePre();
        Helper.ClearVariables();
        
        Helper.DownloadMissingFiles();
        Helper.LoadJson();
        Helper.RegisterCommandsAndHooks();
        Helper.ExectueCommands();
        Helper.StartTimer();

        if (hotReload)
        {
            Helper.RemoveRegisterCommandsAndHooks();
            Helper.RemoveOnEntityTakeDamagePre();
            Helper.ClearVariables();

            Helper.DownloadMissingFiles();
            Helper.LoadJson();
            Helper.RegisterCommandsAndHooks();
            Helper.RegisterHookOnEntityTakeDamagePre();
            Helper.ExectueCommands();
            Helper.StartTimer();
            Helper.ReloadPlayersGlobals();
        }
    }

    public override void OnAllPluginsLoaded(bool hotReload)
    {
        var api = ClientPrefsApi.Get();
        if (api == null)
        {
            Helper.DebugMessage("Missing ClientPrefs-GoldKingZ API!", 0);
        }else
        {
            _prefs = api.CreatePrefs<ClientPrefs>(this, new ClientPrefsOptions
            {
                PrefsAPI_CookiesEnable = (PrefsAPI_SaveMode)Configs.Instance.Cookies_Enable,
                PrefsAPI_CookiesAutoRemoveInactivePlayersOlderThanDays = Configs.Instance.Cookies_AutoRemoveInactivePlayersOlderThanDays,

                PrefsAPI_MySqlEnable = (PrefsAPI_SaveMode)Configs.Instance.MySql_Enable,
                PrefsAPI_MySqlAutoRemoveInactivePlayersOlderThanDays = Configs.Instance.MySql_AutoRemoveInactivePlayersOlderThanDays,
                PrefsAPI_MySqlConnectionTimeout = Configs.Instance.MySql_ConnectionTimeout,
                PrefsAPI_MySqlRetryAttempts = Configs.Instance.MySql_RetryAttempts,
                PrefsAPI_MySqlRetryDelay = Configs.Instance.MySql_RetryDelay,
                PrefsAPI_MySqlConfig = new ClientPrefs_GoldKingZ.Shared.MySqlConfig
                {
                    MySql_Servers = Configs.Instance.MySql_Config.MySql_Servers
                        .Select(s => new ClientPrefs_GoldKingZ.Shared.MySqlServer
                        {
                            Server   = s.Server,
                            Port     = s.Port,
                            Database = s.Database,
                            Username = s.Username,
                            Password = s.Password,
                        }).ToList()
                },
                PrefsAPI_DebugEnable = Configs.Instance.EnableDebug == 1?true:false
            });
        }
        
        if (hotReload)
        {
            _prefs?.Refresh();
        }
    }

    public void OnMapStart(string mapname)
    {
        Helper.RemoveOnEntityTakeDamagePre();

        Helper.DownloadMissingFiles();
        Helper.LoadJson();
        Helper.RegisterHookOnEntityTakeDamagePre();
        Helper.ExectueCommands();
        Helper.StartTimer();
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
        Helper.DisableChickenFromSpawn(entity);

        if (Configs.Instance.DisableNewReloadClips == 1)
        {
            Helper.DisableNewReloadClipsGlobal(entity);
        }
        else if (Configs.Instance.DisableNewReloadClips == 2)
        {
            Helper.DisableNewReloadClips(entity);
        }
    }

    public void OnEntityDeleted(CEntityInstance entity)
    {
        Helper.DeleteNewReloadClips(entity);
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

        Helper.CheckPlayerInGlobals(player);
        Helper.CheckPlayerName(player);
        Helper.SetPlayerClan(player);
                
        return HookResult.Continue;
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
    
    public HookResult OnEventPlayerPing(EventPlayerPing @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        var player = @event.Userid;
        if (!player.IsValid(true)) return HookResult.Continue;

        info.DontBroadcast = true;
                
        return HookResult.Continue;
    }
    
    public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        if(!string.IsNullOrEmpty(Configs.Instance.ExecuteOnEveryRoundStart))
        {
            Server.ExecuteCommand(Configs.Instance.ExecuteOnEveryRoundStart);
        }
        
        Helper.ExectueCommands();
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
        if (@event == null || !Configs.Instance.Ignore_BombPlantedHUDMessagesAndSound) return HookResult.Continue;
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

        if (Configs.Instance.HideChatHUD > 0 || Configs.Instance.HideDeadBody > 0 || Configs.Instance.HideWeaponsHUD || Configs.Instance.DisableNewReloadClips > 0)
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
                Helper.ReapplyWeaponAmmo(getcontroller);
            });
        }

        

        return HookResult.Continue;
    }

    public HookResult OnEventWeaponReload(EventWeaponReload @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        var player = @event.Userid;
        if (!player.IsValid(true)) return HookResult.Continue;

        var pawn = player.PlayerPawn?.Value;
        if (pawn == null || !pawn.IsValid) return HookResult.Continue;

        var weapon = pawn.WeaponServices?.ActiveWeapon?.Value;
        if (weapon == null || !weapon.IsValid) return HookResult.Continue;

        string weaponName = Helper.CheckSilencerWeapons(weapon);
        var settings = Helper.GetSettings(player, weaponName);
        if (settings == null) return HookResult.Continue;

        if (settings.UnlimitedClip || settings.UseOldReload || settings.UnlimitedAmmo || settings.Clip.HasValue || settings.Ammo.HasValue)
        {
            Helper.StartReloadLoop(player, weapon, settings);
        }

        return HookResult.Continue;
    }

    public HookResult OnEventWeaponFire(EventWeaponFire @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        var player = @event.Userid;
        if (!player.IsValid(true)) return HookResult.Continue;

        var pawn = player.PlayerPawn?.Value;
        if (pawn == null || !pawn.IsValid) return HookResult.Continue;

        var weapon = pawn.WeaponServices?.ActiveWeapon?.Value;
        if (weapon == null || !weapon.IsValid) return HookResult.Continue;

        string weaponName = Helper.CheckSilencerWeapons(weapon);
        var settings = Helper.GetSettings(player, weaponName);
        if (settings == null) return HookResult.Continue;

        int preFireClip = weapon.Clip1;

        if (settings.UnlimitedClip)
        {
            Server.NextFrame(() =>
            {
                if (!player.IsValid(true) || !weapon.IsValid) return;

                if (settings.Clip.HasValue)
                    weapon.Clip1 = settings.Clip.Value;

                if (settings.UnlimitedAmmo && settings.Ammo.HasValue)
                    weapon.ReserveAmmo[0] = settings.Ammo.Value;

                Utilities.SetStateChanged(weapon, "CBasePlayerWeapon", "m_iClip1");
                Utilities.SetStateChanged(weapon, "CBasePlayerWeapon", "m_pReserveAmmo");
            });

            return HookResult.Continue;
        }

        Server.NextFrame(() =>
        {
            if (!player.IsValid(true) || !weapon.IsValid) return;

            int expectedClip = preFireClip - 1;
            if (settings.Clip.HasValue && weapon.Clip1 < expectedClip && expectedClip >= 0)
            {
                weapon.Clip1 = expectedClip;
                Utilities.SetStateChanged(weapon, "CBasePlayerWeapon", "m_iClip1");
            }

            if (weapon.Clip1 <= 0 && weapon.ReserveAmmo[0] > 0)
            {
                if (settings.UseOldReload || settings.UnlimitedAmmo || settings.Clip.HasValue || settings.Ammo.HasValue)
                {
                    Helper.StartReloadLoop(player, weapon, settings);
                }
            }
        });

        return HookResult.Continue;
    }

    public HookResult OnEventItemEquip(EventItemEquip @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        var player = @event.Userid;
        if (!player.IsValid(true)) return HookResult.Continue;

        int slot = player.Slot;
        g_Main.ActiveReloadId.Remove(slot);

        Server.NextFrame(() =>
        {
            if (!player.IsValid(true)) return;

            var pawn = player.PlayerPawn?.Value;
            if (pawn == null || !pawn.IsValid) return;

            var weapon = pawn.WeaponServices?.ActiveWeapon?.Value;
            if (weapon == null || !weapon.IsValid) return;

            string weaponName = Helper.CheckSilencerWeapons(weapon);
            var settings = Helper.GetSettings(player, weaponName);

            bool changed = false;

            if (settings != null)
            {
                if (settings.Clip.HasValue && weapon.Clip1 > settings.Clip.Value)
                {
                    weapon.Clip1 = settings.Clip.Value;
                    changed = true;
                }

                if (settings.Ammo.HasValue && weapon.ReserveAmmo[0] > settings.Ammo.Value)
                {
                    weapon.ReserveAmmo[0] = settings.Ammo.Value;
                    changed = true;
                }
            }
            else
            {
                try
                {
                    var weaponBase = weapon.As<CCSWeaponBase>();
                    var vdata = weaponBase?.VData;
                    if (vdata != null)
                    {
                        int defaultClip = vdata.MaxClip1;
                        int defaultAmmo = vdata.PrimaryReserveAmmoMax;

                        if (defaultClip > 0 && weapon.Clip1 > defaultClip)
                        {
                            weapon.Clip1 = defaultClip;
                            changed = true;
                        }

                        if (defaultAmmo > 0 && weapon.ReserveAmmo[0] > defaultAmmo)
                        {
                            weapon.ReserveAmmo[0] = defaultAmmo;
                            changed = true;
                        }
                    }
                }
                catch { }
            }

            if (changed)
            {
                Utilities.SetStateChanged(weapon, "CBasePlayerWeapon", "m_iClip1");
                Utilities.SetStateChanged(weapon, "CBasePlayerWeapon", "m_pReserveAmmo");
            }

            if (settings != null && weapon.Clip1 <= 0 && weapon.ReserveAmmo[0] > 0)
            {
                if (settings.UnlimitedClip || settings.UseOldReload || settings.UnlimitedAmmo || settings.Clip.HasValue || settings.Ammo.HasValue)
                {
                    Helper.StartReloadLoop(player, weapon, settings);
                }
            }
        });

        return HookResult.Continue;
    }

    public HookResult OnEntityTakeDamagePre(CBaseEntity entity, CTakeDamageInfo info)
    {
        try
        {
            var g_Main = MainPlugin.Instance.g_Main;
            var ent = entity;
            if (ent == null || !ent.IsValid || ent.DesignerName != "player") return HookResult.Continue;

            var damageinfo = info;
            if (damageinfo == null) return HookResult.Continue;

            if (damageinfo.Attacker.Value == null) return HookResult.Continue;

            var attacker = damageinfo.Attacker.Value.GetPlayerFromCBaseEntity();
            if (!attacker.IsValid(true)) return HookResult.Continue;

            var victim = ent.GetPlayerFromCBaseEntity();
            if (!victim.IsValid(true)) return HookResult.Continue;

            if (Configs.Instance.Custom_MuteSounds_1.Custom_MuteSounds1 > 1 || Configs.Instance.Custom_MuteSounds_2.Custom_MuteSounds2 > 1 || Configs.Instance.Custom_MuteSounds_3.Custom_MuteSounds3 > 1 || Configs.Instance.Sounds_MuteKnife == 2)
            {
                if (g_Main.Player_Data.TryGetValue(victim.Slot, out var victim_handle))
                {
                    victim_handle.Attacker = attacker;
                    victim_handle.Victim = victim;
                }

                if (g_Main.Player_Data.TryGetValue(attacker.Slot, out var attacker_handlee))
                {
                    attacker_handlee.Attacker = attacker;
                    attacker_handlee.Victim = victim;
                }

                MainPlugin.Instance.AddTimer(0.01f, () =>
                {
                    if (victim.IsValid(true) && g_Main.Player_Data.TryGetValue(victim.Slot, out var victim_handle2))
                    {
                        victim_handle2.Attacker = null!;
                        victim_handle2.Victim = null!;
                    }

                    if (attacker.IsValid(true) && g_Main.Player_Data.TryGetValue(attacker.Slot, out var attacker_handle2))
                    {
                        attacker_handle2.Attacker = null!;
                        attacker_handle2.Victim = null!;
                    }
                }, TimerFlags.STOP_ON_MAPCHANGE);
            }

            bool teammatesAreEnemies = ConVar.Find("mp_teammates_are_enemies")!.GetPrimitiveValue<bool>();
            bool ValidHit = victim.IsValid(true) && attacker.IsValid(true) && attacker != victim && (teammatesAreEnemies || attacker.TeamNum != victim.TeamNum);
            
            if(Configs.Instance.Sounds_MuteKnife == 2 && attacker != victim && g_Main.Player_Data.TryGetValue(attacker.Slot, out var attacker_handle))
            {
                if(damageinfo.BitsDamageType == DamageTypes_t.DMG_SLASH)
                {
                    if (ValidHit)
                    {
                        attacker_handle.StabedHisTeamMate = false;

                    }else
                    {
                        attacker_handle.StabedHisTeamMate = true;

                        MainPlugin.Instance.AddTimer(1.00f, () =>
                        {
                            if (attacker.IsValid(true) && g_Main.Player_Data.TryGetValue(attacker.Slot, out var attacker_handle2))
                            {
                                attacker_handle2.StabedHisTeamMate = false;
                            }
                        }, TimerFlags.STOP_ON_MAPCHANGE);
                    }
                }
            }

            if(Configs.Instance.DisableKnifeDamage && attacker != victim)
            {
                if(ValidHit && damageinfo.BitsDamageType == DamageTypes_t.DMG_SLASH)
                {
                    damageinfo.Damage = 0;
                }
            }

            if(Configs.Instance.DisableZeusDamage && attacker != victim)
            {
                if(ValidHit && damageinfo.BitsDamageType == DamageTypes_t.DMG_SHOCK)
                {
                    damageinfo.Damage = 0;
                }
            }
            
            if (Configs.Instance.Disable_AimPunch.DisableAimPunch == 1 || (Configs.Instance.Disable_AimPunch.DisableAimPunch > 1 && MainPlugin.Instance._prefs != null && MainPlugin.Instance._prefs.TryGetValue(victim.Slot, out var handle) && handle.Toggle_AimPunch == 1))
            {
                Server.NextFrame(() =>
                {
                    if (!victim.IsValid(true)) return;

                    if (Configs.Instance.Disable_AimPunch.DisableAimPunch == 1 || (Configs.Instance.Disable_AimPunch.DisableAimPunch > 1 && MainPlugin.Instance._prefs != null && MainPlugin.Instance._prefs.TryGetValue(victim.Slot, out var handle) && handle.Toggle_AimPunch == 1))
                    {
                        var VictimPlayerPawn = victim.PlayerPawn;
                        if (VictimPlayerPawn == null || !VictimPlayerPawn.IsValid) return;

                        var VictimPlayerPawnValue = VictimPlayerPawn.Value;
                        if (VictimPlayerPawnValue == null || !VictimPlayerPawnValue.IsValid) return;

                        var aimPunch = VictimPlayerPawnValue.AimPunchServices;
                        if (aimPunch == null) return;

                        aimPunch.UnpredictableBaseAngle.X = 0;
                        aimPunch.UnpredictableBaseAngle.Y = 0;
                        aimPunch.UnpredictableBaseAngle.Z = 0;
                    }
                });
            }
            return HookResult.Continue;
        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"OnTakeDamage Error : {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
            Helper.DebugMessage($"[StackTrace] OnTakeDamage Error: {ex.StackTrace}", Configs.Instance.EnableDebug.ToDebugConfig(1));
            return HookResult.Continue;
        }
    }
    
    public HookResult OnEventPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;
        Helper.StartTimer();

        var victim = @event.Userid;
        if (!victim.IsValid(true)) return HookResult.Continue;

        if (Configs.Instance.Ignore_DisconnectMessages == 2)
        {
            if (victim.Connected == PlayerConnectedState.Disconnecting)
            {
                info.DontBroadcast = true;
            }
        }

        if (Configs.Instance.HideChatHUD > 0 || Configs.Instance.HideDeadBody > 0 || Configs.Instance.HideWeaponsHUD)
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

    public HookResult OnUserMessage_OnSayText(CounterStrikeSharp.API.Modules.UserMessages.UserMessage um)
    {
        if (Configs.Instance.BlockMapSaying == 0) return HookResult.Continue;

        var entityindex = um.ReadInt("playerindex");
        if (entityindex != -1) return HookResult.Continue;

        var message = um.ReadString("text");

        if (Configs.Instance.BlockMapSaying == 1)
        {
            Helper.DebugMessage("[BlockMapSaying = 1] Blocked Message: " + message, Configs.Instance.EnableDebug.ToDebugConfig(1));
            um.Recipients.Clear();
            return HookResult.Handled;
        }
        else if (Configs.Instance.BlockMapSaying == 2)
        {
            var rule = Helper.GetBlockMapSayingRule();
            if (rule != null)
            {
                var matchedWord = rule.Block_Words.FirstOrDefault(word => !string.IsNullOrWhiteSpace(word) && message.Contains(word, StringComparison.OrdinalIgnoreCase));
                if (matchedWord != null)
                {
                    Helper.DebugMessage("[BlockMapSaying = 2] Blocked Message: " + message + " | Matched Word: " + matchedWord, Configs.Instance.EnableDebug.ToDebugConfig(1));
                    um.Recipients.Clear();
                    return HookResult.Handled;
                }
            }
        }
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

            g_Main.Player_Data.Remove(player.Slot);
        }

        return HookResult.Continue;
    }

    public void OnMapEnd()
    {
        try
        {
            Helper.RemoveOnEntityTakeDamagePre();
            Helper.ClearVariables();
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
            _prefs?.Unload();
            Helper.RemoveRegisterCommandsAndHooks();
            Helper.RemoveOnEntityTakeDamagePre();
            Helper.ClearVariables();
            Helper.RenderBackPlayers();
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
                Helper.RemoveOnEntityTakeDamagePre();
                Helper.ClearVariables();
                Helper.RenderBackPlayers();
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