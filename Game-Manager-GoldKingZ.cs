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
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Core.Attributes.Registration;

namespace Game_Manager_GoldKingZ;

[MinimumApiVersion(276)]
public class GameManagerGoldKingZ : BasePlugin
{
    public override string ModuleName => "Game Manager (Block/Hide Unnecessaries In Game)";
    public override string ModuleVersion => "2.0.7";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "https://github.com/oqyh";
    internal static IStringLocalizer? Stringlocalizer;
    public int countingglobal = 0;
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
        RegisterListener<Listeners.OnMapStart>(OnMapStart);
        RegisterListener<Listeners.OnMapEnd>(OnMapEnd);

        if(Configs.GetConfigData().Sounds_MuteKnifesMode == 2)
        {
            VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Hook(OnTakeDamage, HookMode.Pre);
        }
        
        HookUserMessage(322, um =>
        {
           for (int i = 0; i < um.GetRepeatedFieldCount("params"); i++)
            {
                var message = um.ReadString("params", i);
                if (Configs.GetConfigData().Ignore_PlantingBombMessages && message.Contains("Cstrike_TitlesTXT_Planting_Bomb") ||
                    Configs.GetConfigData().Ignore_DefusingBombMessages && message.Contains("Cstrike_TitlesTXT_Defusing_Bomb"))
                {
                    return HookResult.Stop;
                }
            }
            return HookResult.Continue;
        }, HookMode.Pre);

        HookUserMessage(208, um =>
        {
            var soundevent = um.ReadUInt("soundevent_hash");
            
            
            uint PlayerPOVScreen_Got_Damage_ClientSide = 3124768561;
            uint Player_Got_Damage_ServerSide = 524041390;
            uint Player_Got_Damage_ClientSide = 708038349;
            uint Player_Got_Damage_FriendlyDamage_ServerSide = 427534867;

            uint HeadShotHit_ClientSide = 2831007164;
            uint HeadShotKill_ClientSide = 3535174312;
            uint HeadShotHit_ServerSide = 3663896169;
            uint HeadShotKill_ServerSide = 2310318859;

            uint DeathScream_ServerSide = 46413566;
            uint DeathScream_ClientSide = 1815352525;

            uint AfterDeathCracklingSound_ClientSide = 2323025056;
            uint AfterDeathCracklingSound_ServerSide = 3396420465;

            uint Knife_Rightstab_BothSides = 3475734633;
            uint Knife_leftstab_BothSides = 1769891506;
            uint Knife_SwingAir_BothSides = 3634660983;
            uint Knife_StabWall_BothSides = 2486534908;
            uint SwitchToSemi_BothSides = 576815311;
            
            uint DropWeapon_C4_BothSides = 1346129716;
            uint DropWeapon_Knife_BothSides = 3208928088;
            uint DropWeapon_PistolAndTaser_BothSides = 1842263658;
            uint DropWeapon_Shotguns_BothSides = 4003696900;
            uint DropWeapon_SMGs_BothSides = 3003881917;
            uint DropWeapon_AssaultRifles_BothSides = 449069384;
            uint DropWeapon_Snipers_BothSides = 2125410722;
            uint DropWeapon_FlashAndDecoy_BothSides = 95362054;
            uint DropWeapon_SmokeAndInNade_BothSides = 910752207;
            uint DropWeapon_HENade_BothSides = 1252397774;
            uint DropWeapon_Molly_BothSides = 1601161479;

            bool MuteKnife = Configs.GetConfigData().Sounds_MuteKnifesMode == 1 ?
            soundevent == Knife_Rightstab_BothSides || soundevent == Knife_leftstab_BothSides || soundevent == Knife_SwingAir_BothSides || soundevent == Knife_StabWall_BothSides :
            Configs.GetConfigData().Sounds_MuteKnifesMode == 2 ? Globals.StabedHisTeamMate.Any( player => player.Value == true) &&
            (soundevent == PlayerPOVScreen_Got_Damage_ClientSide || soundevent == Player_Got_Damage_ServerSide ||
            soundevent == Player_Got_Damage_ClientSide || soundevent == Player_Got_Damage_FriendlyDamage_ServerSide || 
            soundevent == Knife_Rightstab_BothSides || soundevent == Knife_leftstab_BothSides) : false;

            bool MuteHeadShot = Configs.GetConfigData().Sounds_MuteHeadShot == true ? 
            soundevent == HeadShotHit_ClientSide || soundevent == HeadShotKill_ClientSide ||
            soundevent == HeadShotHit_ServerSide || soundevent == HeadShotKill_ServerSide : false;

            bool MuteBodyShot = Configs.GetConfigData().Sounds_MuteBodyShot == true ? 
            soundevent == PlayerPOVScreen_Got_Damage_ClientSide || soundevent == Player_Got_Damage_ServerSide ||
            soundevent == Player_Got_Damage_ClientSide || soundevent == Player_Got_Damage_FriendlyDamage_ServerSide : false;

            bool MuteDeath = Configs.GetConfigData().Sounds_MutePlayerDeathVoice == true ? 
            soundevent == DeathScream_ServerSide || soundevent == DeathScream_ClientSide : false;

            bool MuteCrackling = Configs.GetConfigData().Sounds_MuteAfterDeathCrackling == true ? 
            soundevent == AfterDeathCracklingSound_ClientSide || soundevent == AfterDeathCracklingSound_ServerSide : false;

            bool MuteSwitchToSemi = Configs.GetConfigData().Sounds_MuteSwitchModeSemiToAuto == true ? 
            soundevent == SwitchToSemi_BothSides : false;

            bool MuteDropWeapons = 
            (Configs.GetConfigData().Sounds_MuteDropWeapons.Contains("A", StringComparison.OrdinalIgnoreCase) && soundevent == DropWeapon_C4_BothSides) ||
            (Configs.GetConfigData().Sounds_MuteDropWeapons.Contains("B", StringComparison.OrdinalIgnoreCase) && soundevent == DropWeapon_PistolAndTaser_BothSides) ||
            (Configs.GetConfigData().Sounds_MuteDropWeapons.Contains("C", StringComparison.OrdinalIgnoreCase) && soundevent == DropWeapon_Shotguns_BothSides) ||
            (Configs.GetConfigData().Sounds_MuteDropWeapons.Contains("D", StringComparison.OrdinalIgnoreCase) && soundevent == DropWeapon_SMGs_BothSides) ||
            (Configs.GetConfigData().Sounds_MuteDropWeapons.Contains("E", StringComparison.OrdinalIgnoreCase) && soundevent == DropWeapon_AssaultRifles_BothSides) ||
            (Configs.GetConfigData().Sounds_MuteDropWeapons.Contains("F", StringComparison.OrdinalIgnoreCase) && soundevent == DropWeapon_Snipers_BothSides) ||
            (Configs.GetConfigData().Sounds_MuteDropWeapons.Contains("G", StringComparison.OrdinalIgnoreCase) && soundevent == DropWeapon_FlashAndDecoy_BothSides) ||
            (Configs.GetConfigData().Sounds_MuteDropWeapons.Contains("H", StringComparison.OrdinalIgnoreCase) && soundevent == DropWeapon_SmokeAndInNade_BothSides) ||
            (Configs.GetConfigData().Sounds_MuteDropWeapons.Contains("I", StringComparison.OrdinalIgnoreCase) && soundevent == DropWeapon_HENade_BothSides) ||
            (Configs.GetConfigData().Sounds_MuteDropWeapons.Contains("J", StringComparison.OrdinalIgnoreCase) && soundevent == DropWeapon_Molly_BothSides) ||
            (Configs.GetConfigData().Sounds_MuteDropWeapons.Contains("K", StringComparison.OrdinalIgnoreCase) && soundevent == DropWeapon_Knife_BothSides);

            if(MuteKnife || MuteHeadShot || MuteBodyShot || MuteDeath || MuteCrackling || MuteSwitchToSemi || MuteDropWeapons)
            {
                return HookResult.Stop; 
            }

            if(Configs.GetConfigData().AutoClean_DropWeaponsMode == 1 ||
            Configs.GetConfigData().AutoClean_DropWeaponsMode == 2 ||
            Configs.GetConfigData().AutoClean_DropWeaponsMode == 3)
            {
                var selectedWeapons = Configs.GetConfigData().AutoClean_TheseDroppedWeaponsOnly
                .Split(',')
                .Select(weapon => weapon.Trim().ToLower())
                .ToList();

                var selectedCategories = Helper.WeaponCategories.Keys
                .Where(key => selectedWeapons.Contains(key.ToLower()))
                .ToList();

                var specificWeapons = selectedWeapons
                .Where(weapon => !Helper.WeaponCategories.ContainsKey(weapon.ToUpper()))
                .ToList();

                var allWeaponsToClean = selectedCategories
                .SelectMany(category => Helper.WeaponCategories[category])
                .Concat(specificWeapons)
                .Distinct()
                .ToList();
                
                if (selectedCategories.Any() || specificWeapons.Any())
                {
                    Globals.CbaseWeapons.RemoveAll(entity => 
                    entity == null || 
                    !entity.IsValid || 
                    entity.Entity == null || 
                    (entity.OwnerEntity != null && entity.OwnerEntity.IsValid));

                    foreach (var weapon in allWeaponsToClean)
                    {
                        foreach (var entity in Utilities.FindAllEntitiesByDesignerName<CBaseEntity>(weapon))
                        {
                            if (entity == null || !entity.IsValid || entity.Entity == null || 
                            (entity.OwnerEntity != null && entity.OwnerEntity.IsValid) ||
                            Globals.CbaseWeapons.Contains(entity))
                            {
                                continue;
                            }
                            Globals.CbaseWeapons.Add(entity);
                        }
                    }
                    if (Configs.GetConfigData().AutoClean_DropWeaponsMode == 1 && Globals.CbaseWeapons.Count == Configs.GetConfigData().AutoClean_WhenXWeaponsInGround)
                    {
                        foreach (var weapon in Globals.CbaseWeapons.ToList())
                        {
                            if (weapon != null && weapon.IsValid && weapon.Entity != null &&
                                weapon.OwnerEntity != null && !weapon.OwnerEntity.IsValid)
                            {
                                weapon.AcceptInput("Kill");
                                Globals.CbaseWeapons.Remove(weapon);
                            }
                        }
                    }else if (Configs.GetConfigData().AutoClean_DropWeaponsMode == 2 && Globals.CbaseWeapons.Count == Configs.GetConfigData().AutoClean_WhenXWeaponsInGround)
                    {
                        var oldestWeapon = Globals.CbaseWeapons[0];
                        if (oldestWeapon != null && oldestWeapon.IsValid && oldestWeapon.Entity != null && 
                            oldestWeapon.OwnerEntity != null && !oldestWeapon.OwnerEntity.IsValid)
                        {
                            oldestWeapon.AcceptInput("Kill");
                            Globals.CbaseWeapons.RemoveAt(0);
                        }
                    }else if (Configs.GetConfigData().AutoClean_DropWeaponsMode == 3 && Globals.CbaseWeapons.Count == Configs.GetConfigData().AutoClean_WhenXWeaponsInGround)
                    {
                        var newestWeapon = Globals.CbaseWeapons[Globals.CbaseWeapons.Count - 1];
                        if (newestWeapon != null && newestWeapon.IsValid && newestWeapon.Entity != null &&
                            newestWeapon.OwnerEntity != null && !newestWeapon.OwnerEntity.IsValid)
                        {
                            newestWeapon.AcceptInput("Kill");
                            Globals.CbaseWeapons.RemoveAt(Globals.CbaseWeapons.Count - 1);
                        }
                    }
                }
            }

            return HookResult.Continue; 
            
        }, HookMode.Pre);
        
        
        if(Configs.GetConfigData().DisableBloodAndHsSpark)
        {
            HookUserMessage(400, um =>
            {
                um.Recipients.Clear();
                return HookResult.Continue;
            },HookMode.Pre);

            HookUserMessage(411, um =>
            {
                um.Recipients.Clear();
                return HookResult.Continue;
            },HookMode.Pre);
        }
        
        HookUserMessage(124, um =>
        {
            for (int i = 0; i < um.GetRepeatedFieldCount("param"); i++)
            {
                var message = um.ReadString("param", i);
                
                if(Configs.GetConfigData().Ignore_TeamMateAttackMessages)
                {
                    for (int X = 0; X < Helper.TeamWarningArray.Length; X++)
                    {
                        if (message.Contains(Helper.TeamWarningArray[X]))
                        {
                            return HookResult.Stop;
                        }
                    }
                }
                
                if(Configs.GetConfigData().Ignore_AwardsMoneyMessages)
                {
                    for (int X = 0; X < Helper.MoneyMessageArray.Length; X++)
                    {
                        if (message.Contains(Helper.MoneyMessageArray[X]))
                        {
                            return HookResult.Stop;
                        }
                    }
                }

                if(Configs.GetConfigData().Ignore_PlayerSavedYouByPlayerMessages)
                {
                    for (int X = 0; X < Helper.SavedbyArray.Length; X++)
                    {
                        if (message.Contains(Helper.SavedbyArray[X]))
                        {
                            return HookResult.Stop;
                        }
                    }
                }
                
                if (Configs.GetConfigData().Ignore_ChickenKilledMessages && message.Contains("Pet_Killed"))
                {
                    return HookResult.Stop;
                }
            }
            return HookResult.Continue;
        },HookMode.Pre);

        HookUserMessage(452, um =>
        {
            if(Configs.GetConfigData().Sounds_MuteGunShotsMode == 1)
            {
                um.SetInt("sound_type", 0);
            }else if(Configs.GetConfigData().Sounds_MuteGunShotsMode == 2)
            {
                um.SetUInt("weapon_id", 0);
                um.SetInt("sound_type", 9);
                um.SetUInt("item_def_index", 60);
            }else if(Configs.GetConfigData().Sounds_MuteGunShotsMode == 3)
            {
                um.SetUInt("weapon_id", 0);
                um.SetInt("sound_type", 9);
                um.SetUInt("item_def_index", 61);
            }else if(Configs.GetConfigData().Sounds_MuteGunShotsMode == 4)
            {
                um.SetUInt("weapon_id", Configs.GetConfigData().Mode4_Sounds_GunShots_weapon_id);
                um.SetInt("sound_type", Configs.GetConfigData().Mode4_Sounds_GunShots_sound_type);
                um.SetUInt("item_def_index", Configs.GetConfigData().Mode4_Sounds_GunShots_item_def_index);
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
    }
    private HookResult OnTakeDamage(DynamicHook hook)
    {
        var ent = hook.GetParam<CEntityInstance>(0);
        if (ent == null || !ent.IsValid || ent.DesignerName != "player") { return HookResult.Continue; }

        var damageinfo = hook.GetParam<CTakeDamageInfo>(1);
        if (damageinfo == null) { return HookResult.Continue; }

        var GetAttacker = damageinfo.Attacker.Value!.As<CBasePlayerPawn>().Controller.Value;
        if (GetAttacker == null || !GetAttacker.IsValid) { return HookResult.Continue; }

        var pawn = ent.As<CCSPlayerPawn>();
        if (pawn == null || !pawn.IsValid) { return HookResult.Continue; }

        var attacker = Utilities.GetPlayerFromIndex((int)GetAttacker.Index);
        if (attacker == null || !attacker.IsValid) { return HookResult.Continue; }

        var Victim = pawn.OriginalController.Get();
        if (Victim == null || !Victim.IsValid) { return HookResult.Continue; }

        if (Victim.TeamNum == attacker.TeamNum)
        {
            if(attacker.PlayerPawn.Value != null && attacker.PlayerPawn.Value.IsValid && attacker.PlayerPawn.Value.WeaponServices != null && attacker.PlayerPawn.Value.WeaponServices.MyWeapons != null)
            {
                var myWeapons = attacker.PlayerPawn.Value.WeaponServices.MyWeapons;
                if (myWeapons != null)
                {
                    foreach (var gun in myWeapons)
                    {
                        if(gun == null || !gun.IsValid)continue;
                        if(gun.Value == null || !gun.Value.IsValid)continue;
                        if(gun.Value.DesignerName == null || string.IsNullOrEmpty(gun.Value.DesignerName))continue;

                        var WeaponName = gun.Value.DesignerName;
                        if(WeaponName.Contains("weapon_knife") || WeaponName.Contains("weapon_bayonet"))
                        {
                            if (!Globals.StabedHisTeamMate.ContainsKey(attacker))
                            {
                                Globals.StabedHisTeamMate.Add(attacker,true);
                            }
                            if (Globals.StabedHisTeamMate.ContainsKey(attacker))
                            {
                                Globals.StabedHisTeamMate[attacker] = true;
                            }

                            AddTimer(0.01f, () =>
                            {
                                if(attacker != null && attacker.IsValid)
                                {
                                    if (!Globals.StabedHisTeamMate.ContainsKey(attacker))
                                    {
                                        Globals.StabedHisTeamMate.Add(attacker,false);
                                    }
                                    if (Globals.StabedHisTeamMate.ContainsKey(attacker))
                                    {
                                        Globals.StabedHisTeamMate[attacker] = false;
                                    }
                                }
                                
                            },TimerFlags.STOP_ON_MAPCHANGE);
                        }
                        
                    }
                }
            }
            
        }else if (Victim.TeamNum != attacker.TeamNum)
        {
            if (!Globals.StabedHisTeamMate.ContainsKey(attacker))
            {
                Globals.StabedHisTeamMate.Add(attacker,false);
            }
            if (Globals.StabedHisTeamMate.ContainsKey(attacker))
            {
                Globals.StabedHisTeamMate[attacker] = false;
            }
        }
        return HookResult.Continue;
    }
    
    private void OnMapStart(string Map)
    {
        
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
                    Helper.DebugMessage(ex.Message);
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
            var allplayers = Helper.GetPlayersController(true,false);

            allplayers.ForEach(players => {
                if(players == null || !players.IsValid ||
                players.PlayerPawn == null || !players.PlayerPawn.IsValid ||
                players.PlayerPawn.Value == null || !players.PlayerPawn.Value.IsValid)return;
                var otherteam = players.TeamNum;
                bool sameTeam = playerteam == otherteam;
                bool teammatesAreEnemies = ConVar.Find("mp_teammates_are_enemies")!.GetPrimitiveValue<bool>();
                var Nadelocation = players.PlayerPawn.Value.LastPlaceName;

                if (sameTeam && !teammatesAreEnemies) {
                    Helper.SendGrenadeMessage(nade, players, player.PlayerName, Nadelocation.ToString());
                } else if (sameTeam && player != players ) {
                    return;
                } else if (sameTeam && (Configs.GetConfigData().CustomThrowNadeMessagesMode == 3 || Configs.GetConfigData().CustomThrowNadeMessagesMode == 4)) {
                    Helper.SendGrenadeMessage(nade, players, player.PlayerName, Nadelocation.ToString());
                }
            });
        });
        return HookResult.Continue;
    }
    

    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        if(@event == null)return HookResult.Continue;
        Globals.CbaseWeapons.Clear();
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

                    
                    Helper.AdvancedPlayerPrintToChat(player, Localizer["hidechat.enabled.warning"], Configs.GetConfigData().DisableHUDChatModeWarningTimerInSecs);
                    
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
                    
                    Helper.AdvancedPlayerPrintToChat(player, Localizer["hidechat.enabled.warning"], Configs.GetConfigData().DisableHUDChatModeWarningTimerInSecs);
                    
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
        if (!Configs.GetConfigData().Sounds_MuteMVP || @event == null)return HookResult.Continue;
        var Player = @event.Userid;
        if (Player == null || !Player.IsValid)return HookResult.Continue;
        Player.MusicKitID = 0;
        return HookResult.Continue;
    }
    private HookResult OnEventBombPlanted(EventBombPlanted @event, GameEventInfo info)
    {
        if (!Configs.GetConfigData().Ignore_BombPlantedHUDMessages || @event == null)return HookResult.Continue;
        info.DontBroadcast = true;
        return HookResult.Continue;
    }

    private HookResult OnEventPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        if(@event == null)return HookResult.Continue;
        Helper.ExectueCommands();

        var player = @event.Userid;
        if(player == null || !player.IsValid )return HookResult.Continue;
        if (player.PlayerPawn == null
            || !player.PlayerPawn.IsValid
            || player.PlayerPawn.Value == null
            || !player.PlayerPawn.Value.IsValid)
            {
                return HookResult.Continue;
            }

        if (Configs.GetConfigData().Ignore_DisconnectMessagesMode == 2)
        {
            if (Globals.Remove_Icon.ContainsKey(player.SteamID))
            {
                info.DontBroadcast = true;
                Globals.Remove_Icon.Remove(player.SteamID);
            }
        }
        
        if(Configs.GetConfigData().DisableDeadBodyMode == 1 || Configs.GetConfigData().DisableDeadBodyMode == 2 || Configs.GetConfigData().DisableDeadBodyMode == 3)
        {
            Server.NextFrame(() =>
            {
                if (player == null
                ||  !player.IsValid
                ||  player.PlayerPawn == null
                ||  !player.PlayerPawn.IsValid
                ||  player.PlayerPawn.Value == null
                ||  !player.PlayerPawn.Value.IsValid)
                {
                    return;
                }
                var orginalmodel = player.PlayerPawn.Value.CBodyComponent?.SceneNode?.GetSkeletonInstance()?.ModelState.ModelName ?? string.Empty;
                if (!string.IsNullOrEmpty(orginalmodel ))
                {
                    player.PlayerPawn.Value.SetModel("characters/models/tm_jumpsuit/tm_jumpsuit_varianta.vmdl");
                    player.PlayerPawn.Value.SetModel(orginalmodel);
                }
            });
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
                Color newColor = Color.FromArgb(Globals.PlayerAlpha[player], 255, 255, 255);
                player.PlayerPawn.Value.Render = newColor;
                Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");

                if (Globals.PlayerAlpha[player] <= 0)
                {
                    if(Globals.TimerRemoveDeadBody.ContainsKey(player))
                    {
                        Globals.TimerRemoveDeadBody[player]?.Kill();
                        Globals.TimerRemoveDeadBody[player] = null!;
                        Globals.TimerRemoveDeadBody.Remove(player);
                    }
                    Globals.PlayerAlpha.Remove(player);
                }
            }
        }, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);

        return timer;
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
                    
                    Helper.AdvancedPlayerPrintToChat(Player, Localizer["hidelegs.not.allowed"]);
                    
                    return HookResult.Continue;
                }

                if (Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 1 || Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 3)
                {
                    Globals.Toggle_DisableLegs[PlayerSteamID] = 2;
                    
                    Helper.AdvancedPlayerPrintToChat(Player, Localizer["hidelegs.disabled"]);
                    
                }else if (Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 4 || Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 6)
                {
                    Globals.Toggle_DisableLegs[PlayerSteamID] = 5;
                    
                    Helper.AdvancedPlayerPrintToChat(Player, Localizer["hidelegs.enabled"]);
                    
                }else if (Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 2)
                {
                    Globals.Toggle_DisableLegs[PlayerSteamID] = 3;
                    
                    Helper.AdvancedPlayerPrintToChat(Player, Localizer["hidelegs.enabled"]);
                    
                }else if (Globals.Toggle_DisableLegs.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableLegs[PlayerSteamID] == 5)
                {
                    Globals.Toggle_DisableLegs[PlayerSteamID] = 6;
                    
                    Helper.AdvancedPlayerPrintToChat(Player, Localizer["hidelegs.disabled"]);
                    
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
                    Helper.AdvancedPlayerPrintToChat(Player, Localizer["hidechat.not.allowed"]);
                    return HookResult.Continue;
                }

                if (Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 1 || Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 3)
                {
                    Globals.Toggle_DisableChat[PlayerSteamID] = 2;
                    
                    Helper.AdvancedPlayerPrintToChat(Player, Localizer["hidechat.disabled"]);
                    
                }else if (Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 4 || Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 6)
                {
                    Globals.Toggle_DisableChat[PlayerSteamID] = 5;
                    
                    Helper.AdvancedPlayerPrintToChat(Player, Localizer["hidechat.enabled"]);
                    
                }else if (Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 2)
                {
                    Globals.Toggle_DisableChat[PlayerSteamID] = 3;
                    Helper.AdvancedPlayerPrintToChat(Player, Localizer["hidechat.enabled"]);
                    
                }else if (Globals.Toggle_DisableChat.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableChat[PlayerSteamID] == 5)
                {
                    Globals.Toggle_DisableChat[PlayerSteamID] = 6;
                    Helper.AdvancedPlayerPrintToChat(Player, Localizer["hidechat.disabled"]);
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
                        
                        Helper.AdvancedPlayerPrintToChat(Player, Localizer["hidechat.enabled.warning"], Configs.GetConfigData().DisableHUDChatModeWarningTimerInSecs);
                        
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
                    
                    Helper.AdvancedPlayerPrintToChat(Player, Localizer["hideweapons.not.allowed"]);
                    
                    return HookResult.Continue;
                }

                if (Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 1 || Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 3)
                {
                    Globals.Toggle_DisableWeapons[PlayerSteamID] = 2;
                    
                    Helper.AdvancedPlayerPrintToChat(Player, Localizer["hideweapons.disabled"]);
                    
                }else if (Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 4 || Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 6)
                {
                    Globals.Toggle_DisableWeapons[PlayerSteamID] = 5;
                    
                    Helper.AdvancedPlayerPrintToChat(Player, Localizer["hideweapons.enabled"]);
                    
                }else if (Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 2)
                {
                    Globals.Toggle_DisableWeapons[PlayerSteamID] = 3;
                    
                    Helper.AdvancedPlayerPrintToChat(Player, Localizer["hideweapons.enabled"]);
                    
                }else if (Globals.Toggle_DisableWeapons.ContainsKey(PlayerSteamID) && Globals.Toggle_DisableWeapons[PlayerSteamID] == 5)
                {
                    Globals.Toggle_DisableWeapons[PlayerSteamID] = 6;
                    
                    Helper.AdvancedPlayerPrintToChat(Player, Localizer["hideweapons.disabled"]);
                    
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

        if (Configs.GetConfigData().Ignore_JoinTeamMessages)
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
                
                Helper.AdvancedServerPrintToChatAll(Localizer["custom.jointeam.spec"], Playername);
                
            }else if(JoinTeam == 2)
            {
                
                Helper.AdvancedServerPrintToChatAll(Localizer["custom.jointeam.t"], Playername);
                
            }else if(JoinTeam == 3)
            {
                
                Helper.AdvancedServerPrintToChatAll(Localizer["custom.jointeam.ct"], Playername);
                
            }
        }
        return HookResult.Continue;
    }

    public HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        if (Configs.GetConfigData().Ignore_DisconnectMessagesMode == 1 || Configs.GetConfigData().Ignore_DisconnectMessagesMode == 2)
        {
            info.DontBroadcast = true;
        }

        var player = @event.Userid;

        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV) return HookResult.Continue;
        var playerid = player.SteamID;

        if (Configs.GetConfigData().Ignore_DisconnectMessagesMode == 2 )
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
                    Helper.DebugMessage(ex.Message);
                }
            });
        }

        if (Globals.Toggle_DisableLegs.ContainsKey(playerid)) Globals.Toggle_DisableLegs.Remove(playerid);
        if (Globals.Toggle_DisableChat.ContainsKey(playerid)) Globals.Toggle_DisableChat.Remove(playerid);
        if (Globals.Toggle_OnDisableChat.ContainsKey(playerid)) Globals.Toggle_OnDisableChat.Remove(playerid);
        if (Globals.Toggle_DisableWeapons.ContainsKey(playerid)) Globals.Toggle_DisableWeapons.Remove(playerid);
        if (Globals.Toggle_OnDisableWeapons.ContainsKey(playerid)) Globals.Toggle_OnDisableWeapons.Remove(playerid);
        if (Globals.TimerRemoveDeadBody.ContainsKey(player)) Globals.TimerRemoveDeadBody.Remove(player);
        if (Globals.PlayerAlpha.ContainsKey(player)) Globals.PlayerAlpha.Remove(player);
        if (Globals.StabedHisTeamMate.ContainsKey(player)) Globals.StabedHisTeamMate.Remove(player);


        return HookResult.Continue;
    }

    public void OnMapEnd()
    {
        Helper.ClearVariables();
    }
    public override void Unload(bool hotReload)
    {
        if(Configs.GetConfigData().Sounds_MuteKnifesMode == 2)
        {
            VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Unhook(OnTakeDamage, HookMode.Pre);
        }
        Helper.ClearVariables();
    }
}