using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using Game_Manager_GoldKingZ.Config;

namespace Game_Manager_GoldKingZ;

public class GameManagerGoldKingZ : BasePlugin
{
    public override string ModuleName => "Game Manager (Block/Hide Unnecessaries In Game)";
    public override string ModuleVersion => "2.0.9";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "https://github.com/oqyh";
    public static GameManagerGoldKingZ Instance { get; set; } = new();
    public Globals g_Main = new();
    private readonly PlayerChat _PlayerChat = new();

    public override void Load(bool hotReload)
    {
        Instance = this;
        Configs.Load(ModuleDirectory);

        _ = Task.Run(async () =>
        {
            try
            {
                await Helper.DownloadMissingFiles();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DownloadMissingFiles failed: {ex}");
            }
        });

        Helper.LoadJson();

        RegisterEventHandler<EventRoundStart>(OnRoundStart);
        RegisterEventHandler<EventPlayerSpawn>(OnEventPlayerSpawn);
        RegisterEventHandler<EventPlayerDeath>(OnEventPlayerDeath, HookMode.Pre);
        RegisterEventHandler<EventRoundMvp>(OnEventRoundMvp, HookMode.Pre);
        RegisterEventHandler<EventBombPlanted>(OnEventBombPlanted, HookMode.Pre);
        RegisterEventHandler<EventPlayerConnectFull>(OnEventPlayerConnectFull);
        RegisterEventHandler<EventGrenadeThrown>(OnEventGrenadeThrown);
        RegisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnect, HookMode.Pre);
        RegisterEventHandler<EventPlayerTeam>(OnEventPlayerTeam, HookMode.Pre);
        RegisterListener<Listeners.OnMapStart>(OnMapStart);
        RegisterListener<Listeners.OnMapEnd>(OnMapEnd);

        if(Configs.GetConfigData().Custom_ChatMessages)
        {
            AddCommandListener("say", OnPlayerChat, HookMode.Post);
            AddCommandListener("say_team", OnPlayerChatTeam, HookMode.Post);
        }

        if(Configs.GetConfigData().Custom_ChatMessages)
        {
            HookUserMessage(118, um =>
            {
                var entityindex = um.ReadInt("entityindex");
                var player = Utilities.GetPlayerFromIndex(entityindex);
                if (!player.IsValid() || player.IsBot) return HookResult.Continue;
                Helper.AddPlayerToGlobal(player);
                
                var messagename = um.ReadString("messagename");
                var playername = um.ReadString("param1");
                var playermessage = um.ReadString("param2");

                if(messagename.Equals("Cstrike_Chat_All") || messagename.Equals("Cstrike_Chat_CT_Loc") || messagename.Equals("Cstrike_Chat_AllDead")
                || messagename.Equals("Cstrike_Chat_CT_Dead") || messagename.Equals("Cstrike_Chat_T_Loc") || messagename.Equals("Cstrike_Chat_T_Dead")
                || messagename.Equals("Cstrike_Chat_AllSpec") || messagename.Equals("Cstrike_Chat_Spec"))
                {
                    if (Configs.GetConfigData().Custom_ChatMessages_ExcludeStartWith.Any(prefix => 
                    playermessage.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                    {
                        return HookResult.Continue;
                    }

                    if (g_Main.Player_Data.ContainsKey(player))
                    {
                        g_Main.Player_Data[player].Messagename = messagename;
                    }
                    um.Recipients.Clear();
                }
                return HookResult.Continue;
            }, HookMode.Pre);
        }
        
        if(Configs.GetConfigData().Sounds_MuteKnifeStab == 2)
        {
            VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Hook(OnTakeDamage, HookMode.Pre);
        }
        
        if(Configs.GetConfigData().Ignore_PlantingBombMessages || Configs.GetConfigData().Ignore_DefusingBombMessages)
        {
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
        }
        
        if(Configs.GetConfigData().Sounds_MuteKnifeStab > 0 || Configs.GetConfigData().Sounds_MuteHeadShot
        || Configs.GetConfigData().Sounds_MuteBodyShot || Configs.GetConfigData().Sounds_MutePlayerDeathVoice
        || Configs.GetConfigData().Sounds_MuteAfterDeathCrackling || Configs.GetConfigData().Sounds_MuteSwitchModeSemiToAuto
        || !string.IsNullOrEmpty(Configs.GetConfigData().Sounds_MuteDropWeapons) || Configs.GetConfigData().AutoClean_Enable)
        {
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

                bool MuteKnife = Configs.GetConfigData().Sounds_MuteKnifeStab == 1 ?
                soundevent == Knife_Rightstab_BothSides || soundevent == Knife_leftstab_BothSides || soundevent == Knife_SwingAir_BothSides || soundevent == Knife_StabWall_BothSides :
                Configs.GetConfigData().Sounds_MuteKnifeStab == 2 ? g_Main.Player_Data.Values.Any( player => player.StabedHisTeamMate == true) &&
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

                if(Configs.GetConfigData().AutoClean_Enable)
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
                        g_Main.CbaseWeapons.RemoveAll(entity => 
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
                                g_Main.CbaseWeapons.Contains(entity))
                                {
                                    continue;
                                }
                                g_Main.CbaseWeapons.Add(entity);
                            }
                        }
                        if (Configs.GetConfigData().AutoClean_DropWeapons == 1 && g_Main.CbaseWeapons.Count == Configs.GetConfigData().AutoClean_WhenXWeaponsInGround)
                        {
                            foreach (var weapon in g_Main.CbaseWeapons.ToList())
                            {
                                if (weapon != null && weapon.IsValid && weapon.Entity != null &&
                                    weapon.OwnerEntity != null && !weapon.OwnerEntity.IsValid)
                                {
                                    weapon.AcceptInput("Kill");
                                    g_Main.CbaseWeapons.Remove(weapon);
                                }
                            }
                        }else if (Configs.GetConfigData().AutoClean_DropWeapons == 2 && g_Main.CbaseWeapons.Count == Configs.GetConfigData().AutoClean_WhenXWeaponsInGround)
                        {
                            var oldestWeapon = g_Main.CbaseWeapons[0];
                            if (oldestWeapon != null && oldestWeapon.IsValid && oldestWeapon.Entity != null && 
                                oldestWeapon.OwnerEntity != null && !oldestWeapon.OwnerEntity.IsValid)
                            {
                                oldestWeapon.AcceptInput("Kill");
                                g_Main.CbaseWeapons.RemoveAt(0);
                            }
                        }else if (Configs.GetConfigData().AutoClean_DropWeapons == 3 && g_Main.CbaseWeapons.Count == Configs.GetConfigData().AutoClean_WhenXWeaponsInGround)
                        {
                            var newestWeapon = g_Main.CbaseWeapons[g_Main.CbaseWeapons.Count - 1];
                            if (newestWeapon != null && newestWeapon.IsValid && newestWeapon.Entity != null &&
                                newestWeapon.OwnerEntity != null && !newestWeapon.OwnerEntity.IsValid)
                            {
                                newestWeapon.AcceptInput("Kill");
                                g_Main.CbaseWeapons.RemoveAt(g_Main.CbaseWeapons.Count - 1);
                            }
                        }
                    }
                }

                if(MuteKnife || MuteHeadShot || MuteBodyShot || MuteDeath || MuteCrackling || MuteSwitchToSemi || MuteDropWeapons)
                {
                    return HookResult.Stop; 
                }

                return HookResult.Continue; 
                
            }, HookMode.Pre);
        }

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

        if(Configs.GetConfigData().Ignore_TeamMateAttackMessages)
        {
            HookUserMessage(323, um =>
            {

                var message = um.ReadString("message");
                for (int X = 0; X < Helper.TeamWarningArray.Length; X++)
                {
                    if (message.Contains(Helper.TeamWarningArray[X]))
                    {
                        return HookResult.Stop;
                    }
                }
                
                return HookResult.Continue;
            },HookMode.Pre);
        }
        
        if(Configs.GetConfigData().Ignore_TeamMateAttackMessages || Configs.GetConfigData().Ignore_AwardsMoneyMessages 
        || Configs.GetConfigData().Ignore_PlayerSavedYouByPlayerMessages || Configs.GetConfigData().Ignore_ChickenKilledMessages)
        {
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
        }
        
        if(Configs.GetConfigData().Sounds_MuteGunShots > 0)
        {
            HookUserMessage(452, um =>
            {
                if(Configs.GetConfigData().Sounds_MuteGunShots == 1)
                {
                    um.SetInt("sound_type", 0);
                }else if(Configs.GetConfigData().Sounds_MuteGunShots == 2)
                {
                    um.SetUInt("weapon_id", 0);
                    um.SetInt("sound_type", 9);
                    um.SetUInt("item_def_index", 60);
                }else if(Configs.GetConfigData().Sounds_MuteGunShots == 3)
                {
                    um.SetUInt("weapon_id", 0);
                    um.SetInt("sound_type", 9);
                    um.SetUInt("item_def_index", 61);
                }else if(Configs.GetConfigData().Sounds_MuteGunShots == 4)
                {
                    um.SetUInt("weapon_id", Configs.GetConfigData().Sounds_MuteGunShots_weapon_id);
                    um.SetInt("sound_type", Configs.GetConfigData().Sounds_MuteGunShots_sound_type);
                    um.SetUInt("item_def_index", Configs.GetConfigData().Sounds_MuteGunShots_item_def_index);
                }
                return HookResult.Continue;
            }, HookMode.Pre);
        }
        
        if(Configs.GetConfigData().DisableChatWheel)
        {
            AddCommandListener("playerchatwheel", CommandListener_Chatwheel);
        }
        
        if(Configs.GetConfigData().DisablePing)
        {
            AddCommandListener("player_ping", CommandListener_Ping);
        }
        
        if(Configs.GetConfigData().DisableRadio)
        {
            for (int i = 0; i < Helper.RadioArray.Length; i++)
            {
                AddCommandListener(Helper.RadioArray[i], CommandListener_RadioCommands);
            }
        }

        Helper.ExectueCommands();

        if(hotReload)
        {
            Helper.LoadJson();
            Helper.ExectueCommands();
        }
    }

    private HookResult OnPlayerChat(CCSPlayerController? player, CommandInfo info)
	{
        if (player == null || !player.IsValid)return HookResult.Continue;

        _PlayerChat.OnPlayerChat(player, info, false);

        return HookResult.Continue;
    }
    private HookResult OnPlayerChatTeam(CCSPlayerController? player, CommandInfo info)
	{
        if (player == null || !player.IsValid)return HookResult.Continue;

        _PlayerChat.OnPlayerChat(player, info, true);

        return HookResult.Continue;
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

        var victim = pawn.OriginalController.Get();
        if (victim == null || !victim.IsValid) { return HookResult.Continue; }

        
        if (attacker.PlayerPawn == null || !attacker.PlayerPawn.IsValid
        || attacker.PlayerPawn.Value == null || !attacker.PlayerPawn.Value.IsValid
        || attacker.PlayerPawn.Value.WeaponServices == null
        || attacker.PlayerPawn.Value.WeaponServices.ActiveWeapon == null || !attacker.PlayerPawn.Value.WeaponServices.ActiveWeapon.IsValid
        || attacker.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value == null || !attacker.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value!.IsValid
        || string.IsNullOrEmpty(attacker.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value!.DesignerName))
        {
            return HookResult.Continue; 
        };

        bool Check_teammates_are_enemies = ConVar.Find("mp_teammates_are_enemies")!.GetPrimitiveValue<bool>() == false && attacker.TeamNum != victim.TeamNum || ConVar.Find("mp_teammates_are_enemies")!.GetPrimitiveValue<bool>() == true;

        if(attacker.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value!.DesignerName.Contains("weapon_knife") || attacker.PlayerPawn.Value.WeaponServices.ActiveWeapon.Value!.DesignerName.Contains("weapon_bayonet"))
        {
            if(Check_teammates_are_enemies)
            {
                if (g_Main.Player_Data.ContainsKey(attacker))
                {
                    g_Main.Player_Data[attacker].StabedHisTeamMate = false;
                }
            }else
            {
                if (g_Main.Player_Data.ContainsKey(attacker))
                {
                    g_Main.Player_Data[attacker].StabedHisTeamMate = true;
                }

                AddTimer(0.01f, () =>
                {
                    if (g_Main.Player_Data.ContainsKey(attacker))
                    {
                        g_Main.Player_Data[attacker].StabedHisTeamMate = false;
                    }
                },TimerFlags.STOP_ON_MAPCHANGE);
            }
            
        }
        return HookResult.Continue;
    }
    
    private void OnMapStart(string Map)
    {
        Helper.ExectueCommands();
    }

    public HookResult OnEventPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo info)
    {
        if (@event == null)return HookResult.Continue;

        var player = @event.Userid;
        if (!player.IsValid(false)) return HookResult.Continue;
        
        Helper.AddPlayerToGlobal(player);

        return HookResult.Continue;
    }
    public HookResult OnEventGrenadeThrown(EventGrenadeThrown @event, GameEventInfo info)
    {
        if (@event == null) return HookResult.Continue;

        var player = @event.Userid;
        var nade = @event.Weapon;

        if (Configs.GetConfigData().Custom_ThrowNadeMessages == 0 || player == null || !player.IsValid || Configs.GetConfigData().Custom_ThrowNadeMessages == 1 && player.IsBot)return HookResult.Continue;
        
        Server.NextFrame(() =>
        {
            var playerteam = player.TeamNum;
            var allplayers = Helper.GetPlayersController(true,false);

            allplayers.ForEach(players => {
                if(players == null || !players.IsValid ||
                players.PlayerPawn == null || !players.PlayerPawn.IsValid ||
                players.PlayerPawn.Value == null || !players.PlayerPawn.Value.IsValid)return;
                var otherteam = players.TeamNum;
                bool sameTeam = playerteam == otherteam;
                bool teammatesAreEnemies = ConVar.Find("mp_teammates_are_enemies")!.GetPrimitiveValue<bool>();
                
                string Nadelocation = player?.PlayerPawn?.Value?.LastPlaceName ?? "";

                if (sameTeam && !teammatesAreEnemies) {
                    Helper.SendGrenadeMessage(nade, players, player!.PlayerName, Nadelocation.ToString());
                } else if (sameTeam && player != players ) {
                    return;
                } else if (sameTeam && (Configs.GetConfigData().Custom_ThrowNadeMessages == 3 || Configs.GetConfigData().Custom_ThrowNadeMessages == 4)) {
                    Helper.SendGrenadeMessage(nade, players, player!.PlayerName, Nadelocation!.ToString());
                }
            });
        });
        return HookResult.Continue;
    }
    

    private HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        if(@event == null)return HookResult.Continue;
        Helper.ExectueCommands();
        g_Main.CbaseWeapons.Clear();

        return HookResult.Continue;
    }
    private HookResult OnEventPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        if(@event == null)return HookResult.Continue;
        Helper.ExectueCommands();

        var player = @event.Userid;
        if(!player.IsValid(false))return HookResult.Continue;
        
        Helper.AddPlayerToGlobal(player);

        
        Server.NextFrame(() =>
        {
            if(!player.IsValid(false))return;

            if(Configs.GetConfigData().HideDeadBody == 1 
            || Configs.GetConfigData().HideDeadBody == 2 
            || Configs.GetConfigData().HideDeadBody == 3)
            {
                
                if (player.IsAlive() && !player.ControllingBot && g_Main.Player_Data.ContainsKey(player))
                {
                    g_Main.Player_Data[player].Timer_DeadBody?.Kill();
                    g_Main.Player_Data[player].Timer_DeadBody = null!;
                    g_Main.Player_Data[player].PlayerAlpha = 255;
                    player.PlayerRender(255);
                }
            }

            if(Configs.GetConfigData().HideLegs)
            {
                Helper.HideLegs(player);
            }

            if(Configs.GetConfigData().HideWeaponsHUD)
            {
                Helper.HideWeaponsHUD(player);
            }
            
            Helper.HideChatHUD(player);
        });

        return HookResult.Continue;
    }
    private HookResult OnEventRoundMvp(EventRoundMvp @event, GameEventInfo info)
    {
        if (@event == null)return HookResult.Continue;

        var player = @event.Userid;
        if(!player.IsValid())return HookResult.Continue;

        if(Configs.GetConfigData().Sounds_MuteMVPMusic)
        {
            player.MusicKitID = 0;
            Utilities.SetStateChanged(player, "CCSPlayerController", "m_iMusicKitID");
        }

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

        var victim = @event.Userid;
        if(!victim.IsValid(false))return HookResult.Continue;

        if (Configs.GetConfigData().Ignore_DisconnectMessages == 2)
        {
            if(g_Main.Player_Data.ContainsKey(victim) && g_Main.Player_Data[victim].Remove_Icon)
            {
                info.DontBroadcast = true;
                g_Main.Player_Data.Remove(victim);
            }
        }
        
        Server.NextFrame(() =>
        {
            Helper.HideDeadBody(victim);
        });

        var attacker = @event.Attacker;
        if(!attacker.IsValid(false))return HookResult.Continue;
        
        if(Configs.GetConfigData().DisableKillfeed == 1 || Configs.GetConfigData().DisableKillfeed == 2)
        {
            info.DontBroadcast = true;
            if(Configs.GetConfigData().DisableKillfeed == 2)
            {
                @event.FireEventToClient(attacker);
            }
        }
        

        return HookResult.Continue;
    }
    
    private HookResult CommandListener_RadioCommands(CCSPlayerController? player, CommandInfo info)
    {
        return HookResult.Handled;
    }
    private HookResult CommandListener_Chatwheel(CCSPlayerController? player, CommandInfo info)
    {
        return HookResult.Handled;
    }
    private HookResult CommandListener_Ping(CCSPlayerController? player, CommandInfo info)
    {
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

        if (player == null || !player.IsValid || player.IsBot && Configs.GetConfigData().Custom_JoinTeamMessages == 1 || player.IsHLTV) return HookResult.Continue;
        
        if(Configs.GetConfigData().Custom_JoinTeamMessages == 1 || Configs.GetConfigData().Custom_JoinTeamMessages == 2)
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

        if (Configs.GetConfigData().Ignore_DisconnectMessages == 1 || Configs.GetConfigData().Ignore_DisconnectMessages == 2)
        {
            info.DontBroadcast = true;
        }

        var player = @event.Userid;

        if(!player.IsValid(false))return HookResult.Continue;

        if (Configs.GetConfigData().Ignore_DisconnectMessages == 2)
        {
            if(g_Main.Player_Data.ContainsKey(player))
            {
                g_Main.Player_Data[player].Remove_Icon = true;
            }
        }else
        {
            if(g_Main.Player_Data.ContainsKey(player))g_Main.Player_Data.Remove(player);
        }


        return HookResult.Continue;
    }

    public void OnMapEnd()
    {
        Helper.ClearVariables();
    }
    public override void Unload(bool hotReload)
    {
        if(Configs.GetConfigData().Sounds_MuteKnifeStab == 2)
        {
            VirtualFunctions.CBaseEntity_TakeDamageOldFunc.Unhook(OnTakeDamage, HookMode.Pre);
        }
        Helper.ClearVariables();
    }
}