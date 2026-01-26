using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using Game_Manager_GoldKingZ.Config;
using CounterStrikeSharp.API.Modules.Memory;
using System.Drawing;
using System.Security.Cryptography;
using System.Globalization;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;


namespace Game_Manager_GoldKingZ;

public class Helper
{
    public static StringComparison GetComparison(bool ignoreCase) => ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
    public static string[] RadioArray = new string[] {
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
    public static string[] MoneyMessageArray = new string[] {
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
    public static string[] SavedbyArray = new string[] {
    "#Chat_SavePlayer_Savior",
    "#Chat_SavePlayer_Spectator",
    "#Chat_SavePlayer_Saved"
    };
    public static string[] TeamWarningArray = new string[] {
    "#Cstrike_TitlesTXT_Game_teammate_attack",
    "#Cstrike_TitlesTXT_Game_teammate_kills",
    "#Cstrike_TitlesTXT_Hint_careful_around_teammates",
    "#Cstrike_TitlesTXT_Hint_try_not_to_injure_teammates",
    "#Cstrike_TitlesTXT_Killed_Teammate",
    "#SFUI_Notice_Game_teammate_kills",
    "#SFUI_Notice_Hint_careful_around_teammates",
    "#SFUI_Notice_Killed_Teammate"
    };

    public static readonly Dictionary<string, string[]> WeaponCategories = new Dictionary<string, string[]>
    {
        {"A", new[] { "weapon_awp", "weapon_g3sg1", "weapon_scar20", "weapon_ssg08" }},
        {"B", new[] { "weapon_ak47", "weapon_aug", "weapon_famas", "weapon_galilar", "weapon_m4a1_silencer", "weapon_m4a1", "weapon_sg556" }},
        {"C", new[] { "weapon_m249", "weapon_negev" }},
        {"D", new[] { "weapon_mag7", "weapon_nova", "weapon_sawedoff", "weapon_xm1014" }},
        {"E", new[] { "weapon_bizon", "weapon_mac10", "weapon_mp5sd", "weapon_mp7", "weapon_mp9", "weapon_p90", "weapon_ump45" }},
        {"F", new[] { "weapon_cz75a", "weapon_deagle", "weapon_elite", "weapon_fiveseven", "weapon_glock", "weapon_hkp2000", "weapon_p250", "weapon_revolver", "weapon_tec9", "weapon_usp_silencer" }},
        {"G", new[] { "weapon_smokegrenade", "weapon_hegrenade", "weapon_flashbang", "weapon_decoy", "weapon_molotov", "weapon_incgrenade" }},
        {"H", new[] { "item_defuser", "item_cutters" }},
        {"I", new[] { "weapon_taser" }},
        {"J", new[] { "weapon_healthshot" }},
        {"K", new[] { "weapon_knife", "weapon_knife_t" }},
        {"ANY", new[] {
            "weapon_awp", "weapon_g3sg1", "weapon_scar20", "weapon_ssg08",
            "weapon_ak47", "weapon_aug", "weapon_famas", "weapon_galilar", "weapon_m4a1_silencer", "weapon_m4a1", "weapon_sg556",
            "weapon_m249", "weapon_negev",
            "weapon_mag7", "weapon_nova", "weapon_sawedoff", "weapon_xm1014",
            "weapon_bizon", "weapon_mac10", "weapon_mp5sd", "weapon_mp7", "weapon_mp9", "weapon_p90", "weapon_ump45",
            "weapon_cz75a", "weapon_deagle", "weapon_elite", "weapon_fiveseven", "weapon_glock", "weapon_hkp2000", "weapon_p250", "weapon_revolver", "weapon_tec9", "weapon_usp_silencer",
            "weapon_smokegrenade", "weapon_hegrenade", "weapon_flashbang", "weapon_decoy", "weapon_molotov", "weapon_incgrenade",
            "item_defuser", "item_cutters",
            "weapon_taser",
            "weapon_healthshot",
            "weapon_knife", "weapon_knife_t"
        }}
    };

    public static void RegisterCssCommands(string[]? commands, string description, CommandInfo.CommandCallback callback)
    {
        if (commands == null || commands.Length == 0) return;

        foreach (var cmd in commands)
        {
            if (string.IsNullOrEmpty(cmd)) continue;
            MainPlugin.Instance.AddCommand(cmd, description, callback);
        }
    }


    public static void RemoveCssCommands(string[]? commands, CommandInfo.CommandCallback callback)
    {
        if (commands == null || commands.Length == 0) return;

        foreach (var cmd in commands)
        {
            if (string.IsNullOrEmpty(cmd)) continue;
            MainPlugin.Instance.RemoveCommand(cmd, callback);
        }
    }

    public static void RegisterCssListener(string[]? commands, CommandInfo.CommandListenerCallback callback)
    {
        if (commands == null || commands.Length == 0) return;

        foreach (var cmd in commands)
        {
            if (string.IsNullOrEmpty(cmd)) continue;
            MainPlugin.Instance.AddCommandListener(cmd, callback, HookMode.Pre);
        }
    }

    public static void RemoveCssListener(string[]? commands, CommandInfo.CommandListenerCallback callback)
    {
        if (commands == null || commands.Length == 0) return;

        foreach (var cmd in commands)
        {
            if (string.IsNullOrEmpty(cmd)) continue;
            MainPlugin.Instance.RemoveCommandListener(cmd, callback, HookMode.Pre);
        }
    }

    public static void AdvancedPlayerPrintToChat(CCSPlayerController player, CounterStrikeSharp.API.Modules.Commands.CommandInfo commandInfo, string message, params object[] args)
    {
        if (string.IsNullOrEmpty(message)) return;

        for (int i = 0; i < args.Length; i++)
        {
            message = message.Replace($"{{{i}}}", args[i]?.ToString() ?? "");
        }

        if (Regex.IsMatch(message, "{nextline}", RegexOptions.IgnoreCase))
        {
            string[] parts = Regex.Split(message, "{nextline}", RegexOptions.IgnoreCase);
            foreach (string part in parts)
            {
                string trimmedPart = part.Trim();
                trimmedPart = trimmedPart.ReplaceColorTags();
                if (!string.IsNullOrEmpty(trimmedPart))
                {
                    if (commandInfo != null && commandInfo.CallingContext == CounterStrikeSharp.API.Modules.Commands.CommandCallingContext.Console)
                    {
                        player.PrintToConsole(" " + trimmedPart);
                    }
                    else
                    {
                        player.PrintToChat(" " + trimmedPart);
                    }
                }
            }
        }
        else
        {
            message = message.ReplaceColorTags();
            if (commandInfo != null && commandInfo.CallingContext == CounterStrikeSharp.API.Modules.Commands.CommandCallingContext.Console)
            {
                player.PrintToConsole(message);
            }
            else
            {
                player.PrintToChat(message);
            }
        }
    }

    public static void AdvancedServerPrintToChatAll(string message, params object[] args)
    {
        if (string.IsNullOrEmpty(message)) return;

        for (int i = 0; i < args.Length; i++)
        {
            message = message.Replace($"{{{i}}}", args[i].ToString());
        }
        if (Regex.IsMatch(message, "{nextline}", RegexOptions.IgnoreCase))
        {
            string[] parts = Regex.Split(message, "{nextline}", RegexOptions.IgnoreCase);
            foreach (string part in parts)
            {
                string trimmedPart = part.Trim();
                trimmedPart = trimmedPart.ReplaceColorTags();
                if (!string.IsNullOrEmpty(trimmedPart))
                {
                    Server.PrintToChatAll(" " + trimmedPart);
                }
            }
        }
        else
        {
            message = message.ReplaceColorTags();
            Server.PrintToChatAll(message);
        }
    }
    public static void AdvancedPlayerPrintToConsole(CCSPlayerController player, string message, params object[] args)
    {
        if (string.IsNullOrEmpty(message)) return;

        for (int i = 0; i < args.Length; i++)
        {
            message = message.Replace($"{{{i}}}", args[i].ToString());
        }
        if (Regex.IsMatch(message, "{nextline}", RegexOptions.IgnoreCase))
        {
            string[] parts = Regex.Split(message, "{nextline}", RegexOptions.IgnoreCase);
            foreach (string part in parts)
            {
                string trimmedPart = part.Trim();
                trimmedPart = trimmedPart.ReplaceColorTags();
                if (!string.IsNullOrEmpty(trimmedPart))
                {
                    player.PrintToConsole(" " + trimmedPart);
                }
            }
        }
        else
        {
            message = message.ReplaceColorTags();
            player.PrintToConsole(message);
        }
    }

    public static void ClearVariables(bool force = false)
    {
        var g_Main = MainPlugin.Instance.g_Main;

        g_Main.Clear(force);
    }

    public static CCSGameRules? GetGameRules()
    {
        try
        {
            var gameRulesEntities = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules");
            return gameRulesEntities.First().GameRules;
        }
        catch
        {
            return null;
        }
    }
    public static bool IsWarmup()
    {
        return GetGameRules()?.WarmupPeriod ?? false;
    }

    public static void RegisterCommandsAndHooks(bool Late_Hook = false)
    {
        Server.ExecuteCommand("sv_hibernate_when_empty false");

        MainPlugin.Instance.RegisterListener<Listeners.OnClientAuthorized>(MainPlugin.Instance.OnClientAuthorized);
        MainPlugin.Instance.RegisterListener<Listeners.OnMapStart>(MainPlugin.Instance.OnMapStart);
        MainPlugin.Instance.RegisterListener<Listeners.OnMapEnd>(MainPlugin.Instance.OnMapEnd);

        MainPlugin.Instance.RegisterEventHandler<EventRoundStart>(MainPlugin.Instance.OnRoundStart);
        MainPlugin.Instance.RegisterEventHandler<EventRoundEnd>(MainPlugin.Instance.OnEventRoundEnd);
        MainPlugin.Instance.RegisterEventHandler<EventPlayerSpawn>(MainPlugin.Instance.OnEventPlayerSpawn);
        MainPlugin.Instance.RegisterEventHandler<EventPlayerDeath>(MainPlugin.Instance.OnEventPlayerDeath, HookMode.Pre);
        MainPlugin.Instance.RegisterEventHandler<EventRoundMvp>(MainPlugin.Instance.OnEventRoundMvp, HookMode.Pre);
        MainPlugin.Instance.RegisterEventHandler<EventPlayerConnectFull>(MainPlugin.Instance.OnEventPlayerConnectFull);
        MainPlugin.Instance.RegisterEventHandler<EventPlayerDisconnect>(MainPlugin.Instance.OnPlayerDisconnect, HookMode.Pre);
        MainPlugin.Instance.RegisterEventHandler<EventBombPlanted>(MainPlugin.Instance.OnEventBombPlanted, HookMode.Pre);
        MainPlugin.Instance.RegisterEventHandler<EventPlayerTeam>(MainPlugin.Instance.OnEventPlayerTeam, HookMode.Pre);
        MainPlugin.Instance.RegisterEventHandler<EventGrenadeThrown>(MainPlugin.Instance.OnEventGrenadeThrown);
        MainPlugin.Instance.RegisterEventHandler<EventBotTakeover>(MainPlugin.Instance.OnEventBotTakeover);

        MainPlugin.Instance.AddCommandListener("say", MainPlugin.Instance.OnPlayerSay, HookMode.Post);
        MainPlugin.Instance.AddCommandListener("say_team", MainPlugin.Instance.OnPlayerSay_Team, HookMode.Post);
        MainPlugin.Instance.HookUserMessage(118, MainPlugin.Instance.OnUserMessage_OnSayText2, HookMode.Pre);

        RegisterCssCommands(Configs.Instance.Reload_GameManager.Reload_GameManager_CommandsInGame.ConvertCommands(), "Commands To Reload Game Manager Plugin", MainPlugin.Instance.Game_UserMessages.CommandsAction_ReloadPlugin);

        if (Configs.Instance.Block_Commands.Block_Commands_StartWith != null && Configs.Instance.Block_Commands.Block_Commands_StartWith.Any())
        {
            var commands = Configs.Instance.Block_Commands.Block_Commands_StartWith.ToList();
            MainPlugin.Instance.g_Main.AntiCrash_StartWith = "AntiCrash_StartWith_" + DateTime.Now;
            commands.Add(MainPlugin.Instance.g_Main.AntiCrash_StartWith);
            
            RegisterCssListener(commands.ToArray(), MainPlugin.Instance.Game_Listeners.BlockCommands_Listener);
        }

        if (Configs.Instance.Block_Commands.Block_Commands_Contains != null && Configs.Instance.Block_Commands.Block_Commands_Contains.Any())
        {
            var commands = Configs.Instance.Block_Commands.Block_Commands_Contains.ToList();
            MainPlugin.Instance.g_Main.AntiCrash_Contains = "AntiCrash_Contains_" + DateTime.Now;
            commands.Add(MainPlugin.Instance.g_Main.AntiCrash_Contains);
            
            RegisterCssListener(commands.ToArray(), MainPlugin.Instance.Game_Listeners.BlockCommands_Listener);
        }
        
        if(Configs.Instance.DisableChickenFromSpawn)
        {
            MainPlugin.Instance.RegisterListener<Listeners.OnEntitySpawned>(MainPlugin.Instance.OnEntitySpawned);
        }

        if (Configs.Instance.BlockNameChanger > 0)
        {
            MainPlugin.Instance.RegisterListener<Listeners.OnTick>(MainPlugin.Instance.OnTick);
            MainPlugin.Instance.AddCommandListener("jointeam", MainPlugin.Instance.OnJoinTeam, HookMode.Pre);
        }

        if (Configs.Instance.BlockSpray)
        {
            MainPlugin.Instance.RegisterListener<Listeners.OnEntityCreated>(MainPlugin.Instance.OnEntityCreated);
        }

        if (Configs.Instance.Disable_AimPunch.DisableAimPunch > 1)
        {
            RegisterCssCommands(Configs.Instance.Disable_AimPunch.DisableAimPunch_CommandsInGame.ConvertCommands(), "Commands To Toggle AimPunch Shake", MainPlugin.Instance.Game_UserMessages.CommandsAction_Toggle_AimPunch);
        }

        if (Configs.Instance.Custom_MuteSounds_1.Custom_MuteSounds1 > 1)
        {
            RegisterCssCommands(Configs.Instance.Custom_MuteSounds_1.Custom_MuteSounds1_CommandsInGame.ConvertCommands(), "Commands To Toggle Mute Mute Sounds 1", MainPlugin.Instance.Game_UserMessages.CommandsAction_Toggle_MuteSounds_1);
        }

        if (Configs.Instance.Custom_MuteSounds_2.Custom_MuteSounds2 > 1)
        {
            RegisterCssCommands(Configs.Instance.Custom_MuteSounds_2.Custom_MuteSounds2_CommandsInGame.ConvertCommands(), "Commands To Toggle Mute Mute Sounds 2", MainPlugin.Instance.Game_UserMessages.CommandsAction_Toggle_MuteSounds_2);
        }

        if (Configs.Instance.Custom_MuteSounds_3.Custom_MuteSounds3 > 1)
        {
            RegisterCssCommands(Configs.Instance.Custom_MuteSounds_3.Custom_MuteSounds3_CommandsInGame.ConvertCommands(), "Commands To Toggle Mute Mute Sounds 3", MainPlugin.Instance.Game_UserMessages.CommandsAction_Toggle_MuteSounds_3);
        }

        if (Configs.Instance.BlockRadio)
        {
            var commands = RadioArray.ToList();
            MainPlugin.Instance.g_Main.AntiCrash_BlockRadio = "AntiCrash_BlockRadio_" + DateTime.Now;
            commands.Add(MainPlugin.Instance.g_Main.AntiCrash_BlockRadio);
            
            RegisterCssListener(commands.ToArray(), MainPlugin.Instance.Game_Listeners.BlockRadio_Listener);
        }

        if (Configs.Instance.BlockChatWheel)
        {
            MainPlugin.Instance.AddCommandListener("playerchatwheel", MainPlugin.Instance.Game_Listeners.BlockChatwheel_Listener, HookMode.Pre);
        }

        if (Configs.Instance.BlockPing)
        {
            MainPlugin.Instance.AddCommandListener("player_ping", MainPlugin.Instance.Game_Listeners.BlockPing_Listener, HookMode.Pre);
        }

        if (Configs.Instance.HideBloodAndHsSpark)
        {
            MainPlugin.Instance.HookUserMessage(400, MainPlugin.Instance.Game_UserMessages.HideBloodAndHsSpark_UserMessages, HookMode.Pre);
            MainPlugin.Instance.HookUserMessage(411, MainPlugin.Instance.Game_UserMessages.HideBloodAndHsSpark_UserMessages, HookMode.Pre);
        }

        if (Configs.Instance.Custom_MuteSounds_1.Custom_MuteSounds1 > 0 || Configs.Instance.Custom_MuteSounds_2.Custom_MuteSounds2 > 0 || Configs.Instance.Custom_MuteSounds_3.Custom_MuteSounds3 > 0
        || Configs.Instance.Sounds_MuteKnife > 0 || Configs.Instance.EnableDebug.ToDebugConfig(2) is 2 or 1)
        {
            MainPlugin.Instance.HookUserMessage(208, MainPlugin.Instance.Game_UserMessages.MuteSounds_UserMessages, HookMode.Pre);
            MainPlugin.Instance.HookUserMessage(369, MainPlugin.Instance.Game_UserMessages.MuteSounds_WeaponSound, HookMode.Pre);
        }

        if (Configs.Instance.Sounds_MuteGunShots > 0 || Configs.Instance.EnableDebug.ToDebugConfig(3) is 3 or 1)
        {
            MainPlugin.Instance.HookUserMessage(452, MainPlugin.Instance.Game_UserMessages.MuteGunShots_UserMessages, HookMode.Pre);
        }

        if (Configs.Instance.Ignore_TeamMateAttackMessages || Configs.Instance.Ignore_AwardsMoneyMessages || Configs.Instance.Ignore_PlayerSavedYouByPlayerMessages || Configs.Instance.Ignore_ChickenKilledMessages
        || Configs.Instance.EnableDebug.ToDebugConfig(4) is 4 or 1)
        {
            MainPlugin.Instance.HookUserMessage(124, MainPlugin.Instance.Game_UserMessages.Ignore_TextMsg_UserMessages, HookMode.Pre);
        }

        if (Configs.Instance.Ignore_TeamMateAttackMessages || Configs.Instance.EnableDebug.ToDebugConfig(4) is 4 or 1)
        {
            MainPlugin.Instance.HookUserMessage(323, MainPlugin.Instance.Game_UserMessages.Ignore_HintText_UserMessages, HookMode.Pre);
        }

        if (Configs.Instance.Ignore_PlantingBombMessages || Configs.Instance.Ignore_DefusingBombMessages || Configs.Instance.EnableDebug.ToDebugConfig(4) is 4 or 1)
        {
            MainPlugin.Instance.HookUserMessage(322, MainPlugin.Instance.Game_UserMessages.Ignore_RadioText_UserMessages, HookMode.Pre);
        }

        if (Late_Hook) return;

        if (!MainPlugin.Instance.g_Main.OnTakeDamage_Hooked)
        {
            if (Configs.Instance.Disable_AimPunch.DisableAimPunch > 0 || Configs.Instance.Custom_MuteSounds_1.Custom_MuteSounds1 > 1 || Configs.Instance.Custom_MuteSounds_2.Custom_MuteSounds2 > 1 || Configs.Instance.Custom_MuteSounds_3.Custom_MuteSounds3 > 1
            || Configs.Instance.Sounds_MuteKnife == 2 || Configs.Instance.DisableKnifeDamage || Configs.Instance.DisableZeusDamage || Configs.Instance.EnableDebug.ToDebugConfig(2) == 2)
            {
                MainPlugin.Instance.RegisterListener<Listeners.OnEntityTakeDamagePre>(MainPlugin.Instance.Game_Listeners.OnEntityTakeDamagePre);
                MainPlugin.Instance.g_Main.OnTakeDamage_Hooked = true;
            }
        }
    }

    public static void RemoveRegisterCommandsAndHooks()
    {
        MainPlugin.Instance.RemoveListener<Listeners.OnClientAuthorized>(MainPlugin.Instance.OnClientAuthorized);
        MainPlugin.Instance.RemoveListener<Listeners.OnMapStart>(MainPlugin.Instance.OnMapStart);
        MainPlugin.Instance.RemoveListener<Listeners.OnTick>(MainPlugin.Instance.OnTick);
        MainPlugin.Instance.RemoveListener<Listeners.OnMapEnd>(MainPlugin.Instance.OnMapEnd);
        MainPlugin.Instance.RemoveListener<Listeners.OnEntitySpawned>(MainPlugin.Instance.OnEntitySpawned);
        MainPlugin.Instance.RemoveListener<Listeners.OnEntityCreated>(MainPlugin.Instance.OnEntityCreated);
        RemoveCssCommands(Configs.Instance.Reload_GameManager.Reload_GameManager_CommandsInGame.ConvertCommands(), MainPlugin.Instance.Game_UserMessages.CommandsAction_ReloadPlugin);
        RemoveCssCommands(Configs.Instance.Disable_AimPunch.DisableAimPunch_CommandsInGame.ConvertCommands(), MainPlugin.Instance.Game_UserMessages.CommandsAction_Toggle_AimPunch);
        RemoveCssCommands(Configs.Instance.Custom_MuteSounds_1.Custom_MuteSounds1_CommandsInGame.ConvertCommands(), MainPlugin.Instance.Game_UserMessages.CommandsAction_Toggle_MuteSounds_1);
        RemoveCssCommands(Configs.Instance.Custom_MuteSounds_2.Custom_MuteSounds2_CommandsInGame.ConvertCommands(), MainPlugin.Instance.Game_UserMessages.CommandsAction_Toggle_MuteSounds_2);
        RemoveCssCommands(Configs.Instance.Custom_MuteSounds_3.Custom_MuteSounds3_CommandsInGame.ConvertCommands(), MainPlugin.Instance.Game_UserMessages.CommandsAction_Toggle_MuteSounds_3);

        var commands_RadioArray = RadioArray.ToList();
        commands_RadioArray.Add(MainPlugin.Instance.g_Main.AntiCrash_BlockRadio);
        RemoveCssListener(commands_RadioArray.ToArray(), MainPlugin.Instance.Game_Listeners.BlockRadio_Listener);
       
        var commands_Block_Commands_StartWith = Configs.Instance.Block_Commands.Block_Commands_StartWith.ToList();
        commands_Block_Commands_StartWith.Add(MainPlugin.Instance.g_Main.AntiCrash_StartWith);
        RemoveCssListener(commands_Block_Commands_StartWith.ToArray(), MainPlugin.Instance.Game_Listeners.BlockCommands_Listener);
        
        var commands_Block_Commands_Contains = Configs.Instance.Block_Commands.Block_Commands_Contains.ToList();
        commands_Block_Commands_Contains.Add(MainPlugin.Instance.g_Main.AntiCrash_Contains);
        RemoveCssListener(commands_Block_Commands_Contains.ToArray(), MainPlugin.Instance.Game_Listeners.BlockCommands_Listener);
        
        MainPlugin.Instance.DeregisterEventHandler<EventRoundStart>(MainPlugin.Instance.OnRoundStart);
	    MainPlugin.Instance.DeregisterEventHandler<EventRoundEnd>(MainPlugin.Instance.OnEventRoundEnd);
        MainPlugin.Instance.DeregisterEventHandler<EventPlayerSpawn>(MainPlugin.Instance.OnEventPlayerSpawn);
        MainPlugin.Instance.DeregisterEventHandler<EventPlayerDeath>(MainPlugin.Instance.OnEventPlayerDeath, HookMode.Pre);
        MainPlugin.Instance.DeregisterEventHandler<EventRoundMvp>(MainPlugin.Instance.OnEventRoundMvp, HookMode.Pre);
        MainPlugin.Instance.DeregisterEventHandler<EventPlayerConnectFull>(MainPlugin.Instance.OnEventPlayerConnectFull);
        MainPlugin.Instance.DeregisterEventHandler<EventPlayerDisconnect>(MainPlugin.Instance.OnPlayerDisconnect, HookMode.Pre);
        MainPlugin.Instance.DeregisterEventHandler<EventBombPlanted>(MainPlugin.Instance.OnEventBombPlanted, HookMode.Pre);
        MainPlugin.Instance.DeregisterEventHandler<EventPlayerTeam>(MainPlugin.Instance.OnEventPlayerTeam, HookMode.Pre);
        MainPlugin.Instance.DeregisterEventHandler<EventGrenadeThrown>(MainPlugin.Instance.OnEventGrenadeThrown);
        MainPlugin.Instance.DeregisterEventHandler<EventBotTakeover>(MainPlugin.Instance.OnEventBotTakeover);

        MainPlugin.Instance.RemoveCommandListener("say", MainPlugin.Instance.OnPlayerSay, HookMode.Post);
        MainPlugin.Instance.RemoveCommandListener("say_team", MainPlugin.Instance.OnPlayerSay_Team, HookMode.Post);
        MainPlugin.Instance.RemoveCommandListener("jointeam", MainPlugin.Instance.OnJoinTeam, HookMode.Pre);
        MainPlugin.Instance.RemoveCommandListener("playerchatwheel", MainPlugin.Instance.Game_Listeners.BlockChatwheel_Listener, HookMode.Pre);
        MainPlugin.Instance.RemoveCommandListener("player_ping", MainPlugin.Instance.Game_Listeners.BlockPing_Listener, HookMode.Pre);
        MainPlugin.Instance.UnhookUserMessage(118, MainPlugin.Instance.OnUserMessage_OnSayText2, HookMode.Pre);
        MainPlugin.Instance.UnhookUserMessage(400, MainPlugin.Instance.Game_UserMessages.HideBloodAndHsSpark_UserMessages, HookMode.Pre);
        MainPlugin.Instance.UnhookUserMessage(411, MainPlugin.Instance.Game_UserMessages.HideBloodAndHsSpark_UserMessages, HookMode.Pre);
        MainPlugin.Instance.UnhookUserMessage(208, MainPlugin.Instance.Game_UserMessages.MuteSounds_UserMessages, HookMode.Pre);
        MainPlugin.Instance.UnhookUserMessage(369, MainPlugin.Instance.Game_UserMessages.MuteSounds_WeaponSound, HookMode.Pre);
        MainPlugin.Instance.UnhookUserMessage(452, MainPlugin.Instance.Game_UserMessages.MuteGunShots_UserMessages, HookMode.Pre);
        MainPlugin.Instance.UnhookUserMessage(124, MainPlugin.Instance.Game_UserMessages.Ignore_TextMsg_UserMessages, HookMode.Pre);
        MainPlugin.Instance.UnhookUserMessage(322, MainPlugin.Instance.Game_UserMessages.Ignore_RadioText_UserMessages, HookMode.Pre);
        MainPlugin.Instance.UnhookUserMessage(323, MainPlugin.Instance.Game_UserMessages.Ignore_HintText_UserMessages, HookMode.Pre);

        if (MainPlugin.Instance.g_Main.OnTakeDamage_Hooked)
        {
            MainPlugin.Instance.RemoveListener<Listeners.OnEntityTakeDamagePre>(MainPlugin.Instance.Game_Listeners.OnEntityTakeDamagePre);
            MainPlugin.Instance.g_Main.OnTakeDamage_Hooked = false;
        }
    }

    public static List<CCSPlayerController> GetPlayersController(bool IncludeBots = false, bool IncludeHLTV = false, bool IncludeNone = true, bool IncludeSPEC = true, bool IncludeCT = true, bool IncludeT = true)
    {
        return Utilities
            .FindAllEntitiesByDesignerName<CCSPlayerController>("cs_player_controller")
            .Where(p =>
                p != null &&
                p.IsValid &&
                p.Connected == PlayerConnectedState.PlayerConnected &&
                (IncludeBots || !p.IsBot) &&
                (IncludeHLTV || !p.IsHLTV) &&
                ((IncludeCT && p.TeamNum == (byte)CsTeam.CounterTerrorist) ||
                (IncludeT && p.TeamNum == (byte)CsTeam.Terrorist) ||
                (IncludeNone && p.TeamNum == (byte)CsTeam.None) ||
                (IncludeSPEC && p.TeamNum == (byte)CsTeam.Spectator)))
            .ToList();
    }
    public static int GetPlayersCount(bool IncludeBots = false, bool IncludeHLTV = false, bool IncludeSPEC = true, bool IncludeCT = true, bool IncludeT = true)
    {
        return Utilities.GetPlayers().Count(p =>
            p != null &&
            p.IsValid &&
            p.Connected == PlayerConnectedState.PlayerConnected &&
            (IncludeBots || !p.IsBot) &&
            (IncludeHLTV || !p.IsHLTV) &&
            ((IncludeCT && p.TeamNum == (byte)CsTeam.CounterTerrorist) ||
            (IncludeT && p.TeamNum == (byte)CsTeam.Terrorist) ||
            (IncludeSPEC && p.TeamNum == (byte)CsTeam.Spectator))
        );
    }

    public static bool IsPlayerInGroupPermission(CCSPlayerController player, string groups)
    {
        if (string.IsNullOrEmpty(groups) || player == null || !player.IsValid)
            return false;

        return groups.Split('|')
            .Select(segment => segment.Trim())
            .Any(trimmedSegment => Permission_CheckPermissionSegment(player, trimmedSegment));
    }

    private static bool Permission_CheckPermissionSegment(CCSPlayerController player, string segment)
    {
        if (string.IsNullOrEmpty(segment)) return false;

        int colonIndex = segment.IndexOf(':');
        if (colonIndex == -1 || colonIndex == 0) return false;

        string prefix = segment.Substring(0, colonIndex).Trim().ToLower();
        string values = segment.Substring(colonIndex + 1).Trim();

        return prefix switch
        {
            "steamid" or "steamids" or "steam" or "steams" => Permission_CheckSteamIds(player, values),
            "flag" or "flags" => Permission_CheckFlags(player, values),
            "group" or "groups" => Permission_CheckGroups(player, values),
            _ => false
        };
    }

    private static bool Permission_CheckSteamIds(CCSPlayerController player, string steamIds)
    {
        if (string.IsNullOrEmpty(steamIds)) return false;

        steamIds = steamIds.Replace("[", "").Replace("]", "");

        var (steam2, steam3, steam32, steam64) = player.SteamID.GetPlayerSteamID();
        var steam3NoBrackets = steam3.Trim('[', ']');

        return steamIds
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(id => id.Trim())
            .Any(trimmedId =>
                string.Equals(trimmedId, steam2, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(trimmedId, steam3NoBrackets, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(trimmedId, steam32, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(trimmedId, steam64, StringComparison.OrdinalIgnoreCase)
            );
    }

    private static bool Permission_CheckFlags(CCSPlayerController player, string flags)
    {
        if (player == null || !player.IsValid ||
            player.Connected != PlayerConnectedState.PlayerConnected ||
            player.IsBot || player.IsHLTV)
            return false;

        if (string.IsNullOrEmpty(flags))
            return false;

        var playerData = AdminManager.GetPlayerAdminData(player);
        if (playerData == null)
            return false;

        var requiredFlags = flags
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(f => f.Trim())
            .ToList();

        if (playerData._flags != null &&
            requiredFlags.Any(reqFlag =>
                playerData._flags.Contains(reqFlag, StringComparer.OrdinalIgnoreCase)))
            return true;

        var allFlags = playerData.GetAllFlags();
        return allFlags != null &&
            requiredFlags.Any(reqFlag =>
                allFlags.Contains(reqFlag, StringComparer.OrdinalIgnoreCase));
    }

    private static bool Permission_CheckGroups(CCSPlayerController player, string groups)
    {
        if (player == null || !player.IsValid ||
            player.Connected != PlayerConnectedState.PlayerConnected ||
            player.IsBot || player.IsHLTV)
            return false;

        if (string.IsNullOrEmpty(groups))
            return false;

        var playerData = AdminManager.GetPlayerAdminData(player);
        if (playerData == null || playerData.Groups == null || !playerData.Groups.Any())
            return false;

        return groups
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(g => g.Trim())
            .Any(reqGroup => playerData.Groups.Contains(reqGroup, StringComparer.OrdinalIgnoreCase));
    }

    public static void MuteCommands(CounterStrikeSharp.API.Modules.UserMessages.UserMessage? um, int Config, bool Fully = false)
    {
        if (um == null) return;
        if (!Fully && Config == 2 || Fully && Config > 0)
        {
            um.Recipients.Clear();
        }
    }

    public static void DebugMessage(string message, int config = -1)
    {
        if (config < 0) return;

        if (config is 0 or 5)
        {
            if (config == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }
        else if (config == 1)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
        }
        else if (config == 2)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
        }
        else if (config == 3)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
        }
        else if (config == 4)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
        }

        string output = $"[Game Manager] {message}";
        Console.WriteLine(output);
        Console.ResetColor();
    }

    public static void ExectueCommands()
    {
        if (Configs.Instance.BlockBotRadio)
        {
            Server.ExecuteCommand("bot_chatter off");
        }

        if (Configs.Instance.BlockBots)
        {
            Server.ExecuteCommand("bot_kick");
        }

        if (Configs.Instance.BlockGrenadesRadio)
        {
            Server.ExecuteCommand("sv_ignoregrenaderadio 1");
        }

        if (Configs.Instance.HideRadar)
        {
            Server.ExecuteCommand("sv_disable_radar 1");
        }

        if (Configs.Instance.DisableFallDamage)
        {
            Server.ExecuteCommand("sv_falldamage_scale 0");
        }

        if (Configs.Instance.DisableSvCheats_1)
        {
            Server.ExecuteCommand("sv_cheats 0");
        }

        if (Configs.Instance.DisableC4)
        {
            Server.ExecuteCommand("mp_give_player_c4 0");
        }

        if (Configs.Instance.DisableCameraSpectator)
        {
            Server.ExecuteCommand("sv_disable_observer_interpolation true");
        }

        if (Configs.Instance.Sounds_MutePlayersFootSteps)
        {
            Server.ExecuteCommand("sv_footsteps 0");
        }

        if (Configs.Instance.Sounds_MuteJumpLand)
        {
            Server.ExecuteCommand("sv_min_jump_landing_sound 999999");
        }

        if (Configs.Instance.HideTeamMateHeadTag == 1)
        {
            Server.ExecuteCommand("sv_teamid_overhead 0");
        }
        else if (Configs.Instance.HideTeamMateHeadTag == 2)
        {
            Server.ExecuteCommand("sv_teamid_overhead 1; sv_teamid_overhead_always_prohibit 1; sv_teamid_overhead_maxdist 0");
        }
        else if (Configs.Instance.HideTeamMateHeadTag == 3 && Configs.Instance.HideTeamMateHeadTag_Distance > 0)
        {
            Server.ExecuteCommand($"sv_teamid_overhead 1; sv_teamid_overhead_always_prohibit 1; sv_teamid_overhead_maxdist {Configs.Instance.HideTeamMateHeadTag_Distance}");
        }
    }


    public static (string? Nade_Decoy, string? Nade_Flashbang, string? Nade_Incgrenade, string? Nade_Molotov,
    string? Nade_Smokegrenade, string? Nade_Hegrenade, string? JoinTeam_SPEC, string? JoinTeam_CT,
    string? JoinTeam_T, string? BotTakeOver, string? ClanTag_ScoreBoard, string? ClanTag_Chat,
    string? formatString) GetValuesInJson(CCSPlayerController player, string messageType)
    {
        var jsonData = MainPlugin.Instance.g_Main.JsonData;
        if (!Configs.Instance.Custom_ChatMessages || jsonData == null || !player.IsValid(true)) return (null, null, null, null, null, null, null, null, null, null, null, null, null);

        var allProperties = jsonData.Properties();
        var permissionKeys = allProperties
            .Where(p => !p.Name.Equals("ANY", StringComparison.OrdinalIgnoreCase))
            .Select(p => p.Name)
            .ToList();

        foreach (var permissionKey in permissionKeys)
        {
            if (!string.IsNullOrEmpty(permissionKey) && permissionKey.HasValidPermissionData() && IsPlayerInGroupPermission(player, permissionKey))
            {
                var formatGroup = jsonData[permissionKey] as JObject;
                var format = !string.IsNullOrEmpty(messageType) ? formatGroup?[messageType]?.Value<string>() : null;

                var ClanTag_ScoreBoard = formatGroup?["ClanTag_ScoreBoard"]?.Value<string>();
                var ClanTag_Chat = formatGroup?["ClanTag_Chat"]?.Value<string>();
                var BotTakeOver = formatGroup?["BotTakeOver"]?.Value<string>();
                var JoinTeam_SPEC = formatGroup?["JoinTeam_SPEC"]?.Value<string>();
                var JoinTeam_CT = formatGroup?["JoinTeam_CT"]?.Value<string>();
                var JoinTeam_T = formatGroup?["JoinTeam_T"]?.Value<string>();
                var Nade_Hegrenade = formatGroup?["Nade_Hegrenade"]?.Value<string>();
                var Nade_Smokegrenade = formatGroup?["Nade_Smokegrenade"]?.Value<string>();
                var Nade_Molotov = formatGroup?["Nade_Molotov"]?.Value<string>();
                var Nade_Incgrenade = formatGroup?["Nade_Incgrenade"]?.Value<string>();
                var Nade_Flashbang = formatGroup?["Nade_Flashbang"]?.Value<string>();
                var Nade_Decoy = formatGroup?["Nade_Decoy"]?.Value<string>();

                if (!string.IsNullOrEmpty(format) || string.IsNullOrEmpty(messageType))
                {
                    return (Nade_Decoy, Nade_Flashbang, Nade_Incgrenade, Nade_Molotov, Nade_Smokegrenade, Nade_Hegrenade, JoinTeam_SPEC, JoinTeam_CT, JoinTeam_T, BotTakeOver, ClanTag_ScoreBoard, ClanTag_Chat, format);
                }
            }
        }

        var anyGroup = allProperties.FirstOrDefault(p => p.Name.Equals("ANY", StringComparison.OrdinalIgnoreCase))?.Value as JObject;
        var anyFormat = !string.IsNullOrEmpty(messageType) ? anyGroup?[messageType]?.Value<string>() : null;
        var ClanTag_ScoreBoardd = anyGroup?["ClanTag_ScoreBoard"]?.Value<string>();
        var ClanTag_Chatt = anyGroup?["ClanTag_Chat"]?.Value<string>();
        var BotTakeOverr = anyGroup?["BotTakeOver"]?.Value<string>();
        var JoinTeam_SPECC = anyGroup?["JoinTeam_SPEC"]?.Value<string>();
        var JoinTeam_CTT = anyGroup?["JoinTeam_CT"]?.Value<string>();
        var JoinTeam_TT = anyGroup?["JoinTeam_T"]?.Value<string>();
        var Nade_Hegrenadee = anyGroup?["Nade_Hegrenade"]?.Value<string>();
        var Nade_Smokegrenadee = anyGroup?["Nade_Smokegrenade"]?.Value<string>();
        var Nade_Molotovv = anyGroup?["Nade_Molotov"]?.Value<string>();
        var Nade_Incgrenadee = anyGroup?["Nade_Incgrenade"]?.Value<string>();
        var Nade_Flashbangg = anyGroup?["Nade_Flashbang"]?.Value<string>();
        var Nade_Decoyy = anyGroup?["Nade_Decoy"]?.Value<string>();

        return (Nade_Decoyy, Nade_Flashbangg, Nade_Incgrenadee, Nade_Molotovv, Nade_Smokegrenadee, Nade_Hegrenadee, JoinTeam_SPECC, JoinTeam_CTT, JoinTeam_TT, BotTakeOverr, ClanTag_ScoreBoardd, ClanTag_Chatt, anyFormat);
    }

    public static string DetermineMessageKey(string messagename, CCSPlayerController player)
    {
        CsTeam team = (CsTeam)player.TeamNum;
        string teamStr = team switch
        {
            CsTeam.CounterTerrorist => "CT",
            CsTeam.Terrorist => "T",
            _ => "SPEC"
        };

        if (!Globals_Static.messageMappings.TryGetValue(messagename, out var mapping))
        {
            mapping = ("ALIVE", "ALL");
        }

        string stateStr = mapping.State;
        string chatType = mapping.ChatType;

        if (teamStr == "SPEC")
        {
            return $"{teamStr}_{chatType}";
        }
        else
        {
            return $"{teamStr}_{stateStr}_{chatType}";
        }
    }

    public static void ReloadPlayersGlobals()
    {
        foreach (var players in GetPlayersController(false, false, false))
        {
            if (!players.IsValid()) continue;
            _ = MainPlugin.Instance.HandlePlayerConnectionsAsync(players);
        }
    }

    public static void SetPlayerClan(CCSPlayerController? player)
    {
        if (!Configs.Instance.Custom_ChatMessages || !player.IsValid()) return;

        var clanTag = GetValuesInJson(player, "");

        Server.NextFrame(() =>
        {
            var player_clantag = clanTag;
            var player_change = player;

            if (!player_change.IsValid(true)) return;

            player_change.Player_ClanTag(clanTag.ClanTag_ScoreBoard ?? "");

        });
    }

    public static void ReloadCheckPlayerName()
    {
        if (Configs.Instance.Filter_Players_Names < 1) return;
        foreach (var players in GetPlayersController(false, false, false))
        {
            if (!players.IsValid()) continue;
            CheckPlayerName(players);
        }
    }

    public static void CheckPlayerName(CCSPlayerController? getplayer)
    {
        if (Configs.Instance.Filter_Players_Names < 1 || !getplayer.IsValid()) return;

        Server.NextFrame(() =>
        {
            var player = getplayer;
            if (!player.IsValid()) return;

            string originalName = player.PlayerName;
            if (string.IsNullOrWhiteSpace(originalName)) return;

            string cleanedName = originalName;
            string nameNoSpaces = originalName.Replace(" ", "");
            bool shouldRename = false;

            if (Configs.Instance.Filter_Players_Names == 1 || Configs.Instance.Filter_Players_Names == 3)
            {
                var ipMatches = Regex.Matches(
                    nameNoSpaces,
                    @"(?<!\d)(?:(?:25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)\.){3}(?:25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(?!\d)"
                );

                var blockedIps = ipMatches.Cast<Match>()
                    .Select(m => m.Value)
                    .Where(ip => !Configs.Instance.Filter_Whitelist_Ips
                        .Any(w => ip.Equals(w, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                if (blockedIps.Count > 0)
                {
                    cleanedName = blockedIps.Aggregate(
                        cleanedName,
                        (current, badIp) => Regex.Replace(current, Regex.Escape(badIp), "", RegexOptions.IgnoreCase)
                    );
                    shouldRename = true;
                }
            }

            if (Configs.Instance.Filter_Players_Names == 2 || Configs.Instance.Filter_Players_Names == 3)
            {
                var urlMatches = Regex.Matches(
                    nameNoSpaces,
                    @"(?:https?:\/\/)?(?:www\.)?(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(?:\/[^\s]*)?",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled
                );

                var blockedUrls = urlMatches.Cast<Match>()
                    .Select(m => m.Value.Trim())
                    .Where(u => !string.IsNullOrEmpty(u))
                    .Where(u => !IsUrlWhitelisted(u, Configs.Instance.Filter_Whitelist_URLs))
                    .ToList();

                if (blockedUrls.Count > 0)
                {
                    cleanedName = blockedUrls.Aggregate(
                        cleanedName,
                        (current, badUrl) => Regex.Replace(current, Regex.Escape(badUrl), "", RegexOptions.IgnoreCase)
                    );
                    shouldRename = true;
                }
            }

            if (shouldRename)
            {
                cleanedName = Regex.Replace(cleanedName, @"\s+", " ").Trim();
                player.Player_Name(cleanedName);
            }
        });
    }

    public static string GetGeoIsoCodeInfoAsync(string ipAddress)
    {
        if (!Configs.Instance.AutoSetPlayerLanguage || ipAddress == "127.0.0.1" || ipAddress.Contains("192.168."))
            return "";

        try
        {
            using var reader = new DatabaseReader(Path.GetFullPath(Path.Combine(MainPlugin.Instance.ModuleDirectory, "..", "..", "shared/GoldKingZ/GeoLocation/GeoLite2-City.mmdb")));

            var response = reader.City(ipAddress);

            return response.Country.IsoCode ?? "";
        }
        catch (Exception ex)
        {
            DebugMessage($"GetGeoIsoCodeInfoAsync Error {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
        }
        return "";
    }
    public static void SetPlayerLanguage(CCSPlayerController? player, string isoCode)
    {
        if (!Configs.Instance.AutoSetPlayerLanguage || !player.IsValid()) return;
        if (string.IsNullOrWhiteSpace(isoCode)) return;

        try
        {
            var cultureInfo = CultureInfo
            .GetCultures(CultureTypes.SpecificCultures)
            .FirstOrDefault(c =>
            {
                try
                {
                    var region = new RegionInfo(c.LCID);
                    return region.TwoLetterISORegionName.Equals(isoCode, StringComparison.OrdinalIgnoreCase);
                }
                catch
                {
                    return false;
                }
            });

            if (cultureInfo == null)
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }

            var steamId = (SteamID)player.SteamID;
            PlayerLanguageManager.Instance.SetLanguage(steamId, cultureInfo);
        }
        catch (Exception ex)
        {
            DebugMessage($"SetPlayerLanguage Error: {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
        }
    }

    public static void CheckPlayerInGlobals(CCSPlayerController player)
    {
        if (!player.IsValid(true)) return;

        var g_Main = MainPlugin.Instance.g_Main;
        if (!g_Main.Player_Data.ContainsKey(player.Slot))
        {
            var initialData = new Globals.PlayerDataClass(
                player,
                null!,
                null!,
                player.PlayerName,
                0,
                false,
                false,
                player.SteamID,
                Configs.Instance.Disable_AimPunch.DisableAimPunch == 2 ? 1 : Configs.Instance.Disable_AimPunch.DisableAimPunch == 3 ? 2 : 0,
                Configs.Instance.Custom_MuteSounds_1.Custom_MuteSounds1 == 2 ? 1 : Configs.Instance.Custom_MuteSounds_1.Custom_MuteSounds1 == 3 ? 2 : 0,
                Configs.Instance.Custom_MuteSounds_2.Custom_MuteSounds2 == 2 ? 1 : Configs.Instance.Custom_MuteSounds_2.Custom_MuteSounds2 == 3 ? 2 : 0,
                Configs.Instance.Custom_MuteSounds_3.Custom_MuteSounds3 == 2 ? 1 : Configs.Instance.Custom_MuteSounds_3.Custom_MuteSounds3 == 3 ? 2 : 0,
                "",
                255,
                false,
                null!,
                DateTime.MinValue,
                DateTime.MinValue
            );
            g_Main.Player_Data.TryAdd(player.Slot, initialData);
        }

        if (g_Main.Player_Data.TryGetValue(player.Slot, out var handle))
        {
            handle.Player = player;
        }
    }

    public static async Task LoadPlayerData(CCSPlayerController player)
    {
        try
        {
            var g_Main = MainPlugin.Instance.g_Main;
            if (!player.IsValid() || g_Main.Player_Data.ContainsKey(player.Slot)) return;

            var steamId = player.SteamID;

            await Server.NextFrameAsync(() =>
            {
                if (!player.IsValid()) return;

                CheckPlayerInGlobals(player);
            });

            if (Configs.Instance.Cookies_Enable > 0)
            {
                try
                {
                    await Server.NextFrameAsync(async () =>
                    {
                        if (!player.IsValid()) return;

                        var cookieData = await Cookies.RetrievePersonDataById(steamId);
                        if (cookieData.PlayerSteamID != 0)
                        {
                            UpdatePlayerData(player, cookieData);
                        }
                    });
                }
                catch (Exception ex)
                {
                    DebugMessage($"LoadPlayerData Update Cookies Error: {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
                }
            }


            if (Configs.Instance.MySql_Enable > 0)
            {
                try
                {
                    var mysqlData = await MySqlDataManager.RetrievePersonDataByIdAsync(steamId);

                    await Server.NextFrameAsync(() =>
                    {
                        if (!player.IsValid()) return;

                        if (mysqlData.PlayerSteamID != 0)
                        {
                            UpdatePlayerData(player, mysqlData);
                        }
                    });
                }
                catch (Exception ex)
                {
                    DebugMessage($"LoadPlayerData Update MySql Error: {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
                }
            }
        }
        catch (Exception ex)
        {
            DebugMessage($"LoadPlayerData Error: {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
        }
    }



    private static void UpdatePlayerData(CCSPlayerController player, Globals_Static.PersonData data)
    {
        if (!player.IsValid()) return;

        var g_Main = MainPlugin.Instance.g_Main;
        if (!g_Main.Player_Data.TryGetValue(player.Slot, out var handle)) return;


        if (data.Toggle_AimPunch < 0 || data.Toggle_Custom_MuteSounds1 < 0 || data.Toggle_Custom_MuteSounds2 < 0 || data.Toggle_Custom_MuteSounds3 < 0)
        {
            if (data.Toggle_AimPunch < 0)
            {
                handle.Toggle_AimPunch = data.Toggle_AimPunch;
            }
            if (data.Toggle_Custom_MuteSounds1 < 0)
            {
                handle.Toggle_Custom_MuteSounds1 = data.Toggle_Custom_MuteSounds1;
            }
            if (data.Toggle_Custom_MuteSounds2 < 0)
            {
                handle.Toggle_Custom_MuteSounds2 = data.Toggle_Custom_MuteSounds2;
            }
            if (data.Toggle_Custom_MuteSounds3 < 0)
            {
                handle.Toggle_Custom_MuteSounds3 = data.Toggle_Custom_MuteSounds3;
            }
        }
    }

    public static async Task SavePlayerDataOnDisconnect(CCSPlayerController player)
    {
        try
        {
            if (!player.IsValid()) return;

            var g_Main = MainPlugin.Instance.g_Main;
            var steamId = player.SteamID;

            if (g_Main.Player_Data.TryGetValue(player.Slot, out var alldata))
            {
                if (alldata == null) return;

                if (alldata.Toggle_AimPunch < 0 || alldata.Toggle_Custom_MuteSounds1 < 0 || alldata.Toggle_Custom_MuteSounds2 < 0 || alldata.Toggle_Custom_MuteSounds3 < 0)
                {
                    var player_SteamID = alldata.SteamId;

                    var player_Toggle_AimPunch = alldata.Toggle_AimPunch;
                    var player_Toggle_Custom_MuteSounds1 = alldata.Toggle_Custom_MuteSounds1;
                    var player_Toggle_Custom_MuteSounds2 = alldata.Toggle_Custom_MuteSounds2;
                    var player_Toggle_Custom_MuteSounds3 = alldata.Toggle_Custom_MuteSounds3;

                    if (Configs.Instance.Cookies_Enable == 1)
                    {
                        try
                        {
                            await Server.NextFrameAsync(async () =>
                            {
                                await Cookies.SaveToJsonFile(
                                player_SteamID,
                                player_Toggle_AimPunch,
                                player_Toggle_Custom_MuteSounds1,
                                player_Toggle_Custom_MuteSounds2,
                                player_Toggle_Custom_MuteSounds3,
                                DateTime.Now
                                );

                            });
                        }
                        catch (Exception ex)
                        {
                            DebugMessage($"SavePlayerDataOnDisconnect Save Cookies Error: {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
                        }
                    }

                    if (Configs.Instance.MySql_Enable == 1)
                    {
                        try
                        {
                            await Server.NextFrameAsync(async () =>
                            {
                                await MySqlDataManager.SaveToMySqlAsync(new Globals_Static.PersonData
                                {
                                    PlayerSteamID = player_SteamID,
                                    Toggle_AimPunch = player_Toggle_AimPunch,
                                    Toggle_Custom_MuteSounds1 = player_Toggle_Custom_MuteSounds1,
                                    Toggle_Custom_MuteSounds2 = player_Toggle_Custom_MuteSounds2,
                                    Toggle_Custom_MuteSounds3 = player_Toggle_Custom_MuteSounds3,
                                    DateAndTime = DateTime.Now
                                });

                            });
                        }
                        catch (Exception ex)
                        {
                            DebugMessage($"SavePlayerDataOnDisconnect Save MySql Error: {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
                        }
                    }
                }

                g_Main.Player_Data.Remove(player.Slot);
            }
        }
        catch (Exception ex)
        {
            DebugMessage($"SavePlayerDataOnDisconnect Error: {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
        }
    }

    public static void SavePlayersValues()
    {
        var g_Main = MainPlugin.Instance.g_Main;
        foreach (var alldata in g_Main.Player_Data.Values)
        {
            if (alldata == null)
            {
                g_Main.Player_Data.Clear();
                return;
            }

            if (alldata.Toggle_AimPunch < 0 || alldata.Toggle_Custom_MuteSounds1 < 0 || alldata.Toggle_Custom_MuteSounds2 < 0 || alldata.Toggle_Custom_MuteSounds3 < 0)
            {
                var player_SteamID = alldata.SteamId;

                var player_Toggle_AimPunch = alldata.Toggle_AimPunch;
                var player_Toggle_Custom_MuteSounds1 = alldata.Toggle_Custom_MuteSounds1;
                var player_Toggle_Custom_MuteSounds2 = alldata.Toggle_Custom_MuteSounds2;
                var player_Toggle_Custom_MuteSounds3 = alldata.Toggle_Custom_MuteSounds3;

                if (Configs.Instance.Cookies_Enable == 2)
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await Cookies.SaveToJsonFile(
                            player_SteamID,
                            player_Toggle_AimPunch,
                            player_Toggle_Custom_MuteSounds1,
                            player_Toggle_Custom_MuteSounds2,
                            player_Toggle_Custom_MuteSounds3,
                            DateTime.Now
                            );
                        }
                        catch (Exception ex)
                        {
                            DebugMessage($"SavePlayersValues Save Cookies Error: {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
                        }
                    });
                }


                if (Configs.Instance.MySql_Enable == 2)
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await MySqlDataManager.SaveToMySqlAsync(new Globals_Static.PersonData
                            {
                                PlayerSteamID = player_SteamID,
                                Toggle_AimPunch = player_Toggle_AimPunch,
                                Toggle_Custom_MuteSounds1 = player_Toggle_Custom_MuteSounds1,
                                Toggle_Custom_MuteSounds2 = player_Toggle_Custom_MuteSounds2,
                                Toggle_Custom_MuteSounds3 = player_Toggle_Custom_MuteSounds3,
                                DateAndTime = DateTime.Now
                            });
                        }
                        catch (Exception ex)
                        {
                            DebugMessage($"SavePlayersValues Save MySql Error: {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
                        }
                    });
                }
            }
        }

        g_Main.Player_Data.Clear();

        if (Configs.Instance.Cookies_Enable > 0)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await Cookies.RemoveOldEntries();
                }
                catch (Exception ex)
                {
                    DebugMessage($"SavePlayersValues Remove Cookies Error: {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
                }
            });
        }

        if (Configs.Instance.MySql_Enable > 0)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await MySqlDataManager.DeleteOldPlayersAsync();
                }
                catch (Exception ex)
                {
                    DebugMessage($"SavePlayersValues Remove MySql Error: {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
                }
            });
        }
    }


    public static void RemoveHideDeadBody(CCSPlayerController player)
    {
        if (Configs.Instance.HideDeadBody < 1) return;

        var g_Main = MainPlugin.Instance.g_Main;

        if (!player.IsValid(true)) return;

        CheckPlayerInGlobals(player);

        if (g_Main.Player_Data.TryGetValue(player.Slot, out var handle))
        {
            handle.Timer_DeadBody?.Kill();
            handle.Timer_DeadBody = null!;
            handle.PlayerAlpha = 255;
        }

        if (Configs.Instance.HideDeadBody > 0)
        {
            player.PlayerRender(255);
        }
        if (Configs.Instance.HideLegs)
        {
            player.PlayerRender(254);
        }
    }

    public static void HideDeadBody(CCSPlayerController player)
    {
        if (Configs.Instance.HideDeadBody < 1) return;

        var g_Main = MainPlugin.Instance.g_Main;

        if (!player.IsValid(true) || player.IsAlive() && !player.ControllingBot) return;

        CheckPlayerInGlobals(player);

        var PlayerPawn = player.PlayerPawn;
        if (PlayerPawn == null || !PlayerPawn.IsValid) return;

        var PlayerPawnValue = PlayerPawn.Value;
        if (PlayerPawnValue == null || !PlayerPawnValue.IsValid) return;

        var orginalmodel = PlayerPawnValue.CBodyComponent?.SceneNode?.GetSkeletonInstance()?.ModelState.ModelName ?? string.Empty;
        if (!string.IsNullOrEmpty(orginalmodel))
        {
            PlayerPawnValue.SetModel("characters/models/tm_jumpsuit/tm_jumpsuit_varianta.vmdl");
            PlayerPawnValue.SetModel(orginalmodel);
        }

        if (Configs.Instance.HideDeadBody == 1)
        {
            player.PlayerRender(0);
        }
        else if (Configs.Instance.HideDeadBody == 2)
        {
            if (g_Main.Player_Data.TryGetValue(player.Slot, out var handle))
            {
                handle.Timer_DeadBody = MainPlugin.Instance.AddTimer(Configs.Instance.HideDeadBody_Delay, () =>
                {
                    if (!player.IsValid(true) || player.IsAlive() && !player.ControllingBot) return;
                    player.PlayerRender(0);
                }, TimerFlags.STOP_ON_MAPCHANGE);
            }
        }
        else if (Configs.Instance.HideDeadBody == 3)
        {
            if (g_Main.Player_Data.TryGetValue(player.Slot, out var handle))
            {
                handle.PlayerAlpha = 255;
                handle.Timer_DeadBody = MainPlugin.Instance.AddTimer(0.01f, () =>
                {
                    StartDecay(player);
                }, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
            }
        }
    }

    public static void StartDecay(CCSPlayerController player)
    {
        var g_Main = MainPlugin.Instance.g_Main;

        if (!player.IsValid(true) || player.IsAlive() && !player.ControllingBot) return;

        if (g_Main.Player_Data.TryGetValue(player.Slot, out var handle))
        {
            if (handle.PlayerAlpha <= 0)
            {
                handle.Timer_DeadBody?.Kill();
                handle.Timer_DeadBody = null!;
                handle.PlayerAlpha = 255;
                return;
            }
            var DecayPlayer = handle.PlayerAlpha--;
            player.PlayerRender(DecayPlayer);
        }
    }

    public static void HideChatHUD(CCSPlayerController player)
    {
        if (Configs.Instance.HideChatHUD_Delay < 1 || !player.IsValid(true)) return;

        if (Configs.Instance.HideChatHUD == 1)
        {
            player.PlayerHideHUD(Globals_Static.HIDECHAT, true);
        }
        else if (Configs.Instance.HideChatHUD == 2 && Configs.Instance.HideChatHUD_Delay > 0)
        {
            AdvancedPlayerPrintToChat(player, null!, MainPlugin.Instance.Localizer["PrintToChatToPlayer.HideChat.Warning"], Configs.Instance.HideChatHUD_Delay);

            MainPlugin.Instance.AddTimer(Configs.Instance.HideChatHUD_Delay, () =>
            {
                if (!player.IsValid(true)) return;
                player.PlayerHideHUD(Globals_Static.HIDECHAT, true);
            }, TimerFlags.STOP_ON_MAPCHANGE);
        }
    }

    public static void HideWeaponsHUD(CCSPlayerController player)
    {
        if (!Configs.Instance.HideWeaponsHUD || !player.IsValid(true)) return;

        player.PlayerHideHUD(Globals_Static.HIDEWEAPONS, true);
    }

    public static void EmitSound_World(string soundEventName)
    {
        if (string.IsNullOrEmpty(soundEventName)) return;

        var worldEntity = Utilities.GetEntityFromIndex<CBaseEntity>(0);
        if (worldEntity == null || !worldEntity.IsValid || !worldEntity.DesignerName.Contains("world")) return;

        worldEntity.EmitSound(soundEventName);
    }

    public static void StartTimer()
    {
        if (Configs.Instance.BlockNameChanger > 0)
        {
            if (MainPlugin.Instance.g_Main.TimerChecker == null)
            {
                MainPlugin.Instance.g_Main.TimerChecker = MainPlugin.Instance.AddTimer(2.0f, () => Timer_Checker_Callback(), TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
            }
        }

        if (Configs.Instance.AutoClean_Enable)
        {
            if (MainPlugin.Instance.g_Main.TimerCleanUp == null)
            {
                MainPlugin.Instance.g_Main.TimerCleanUp = MainPlugin.Instance.AddTimer(Configs.Instance.AutoClean_Timer, () => Timer_AutoClean_Callback(), TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
            }
        }
    }
    
    public static void Timer_Checker_Callback()
    {
        foreach (var getplayers in MainPlugin.Instance.g_Main.Player_Data.Values.ToList())
        {
            if (getplayers == null) continue;

            var player = getplayers.Player;
            if (!player.IsValid() || player.IsBot) continue;

            var currentPlayerName = player.PlayerName;
            var storedPlayerName = getplayers.PlayerName;

            if (!getplayers.PlayerName_Block && currentPlayerName != storedPlayerName)
            {
                getplayers.PlayerName_Count++;
                getplayers.LastNameChangeTime = DateTime.Now;
                getplayers.PlayerName = currentPlayerName;

                if (getplayers.PlayerName_Count >= Configs.Instance.BlockNameChanger_Changes)
                {
                    PunishPlayer(player, getplayers);
                }
            }
            else if (getplayers.PlayerName_Block)
            {
                if (currentPlayerName != storedPlayerName)
                {
                    getplayers.PlayerName = currentPlayerName;

                    if (Configs.Instance.BlockNameChanger == 1)
                    {
                        getplayers.LastNameChangeTime = DateTime.Now;
                    }
                }

                var timeSinceLastChange = (DateTime.Now - getplayers.LastNameChangeTime).TotalSeconds;

                if (Configs.Instance.BlockNameChanger == 1 && timeSinceLastChange >= Configs.Instance.BlockNameChanger_Block)
                {
                    getplayers.PlayerName_Count = 0;
                    getplayers.PlayerName_Block = false;
                    getplayers.PlayerName_Block_Message = false;
                    getplayers.PlayerName = currentPlayerName;
                    AdvancedPlayerPrintToChat(player, null!, MainPlugin.Instance.Localizer["PrintToChatToPlayer.NameChanging.Mode1.UnBlocked"]);
                }
                else if (Configs.Instance.BlockNameChanger == 2 && timeSinceLastChange >= Configs.Instance.BlockNameChanger_Block)
                {
                    ExecuteBlockCommand(player);
                    getplayers.PlayerName_Count = 0;
                    getplayers.PlayerName_Block = false;
                    getplayers.PlayerName_Block_Message = false;
                    getplayers.PlayerName = currentPlayerName;
                }
            }
        }
    }

    private static void PunishPlayer(CCSPlayerController player, Globals.PlayerDataClass playerData)
    {
        if (!player.IsValid() || playerData.PlayerName_Block) return;

        playerData.PlayerName_Block = true;
        playerData.LastNameChangeTime = DateTime.Now;

        if (!playerData.PlayerName_Block_Message)
        {
            AdvancedPlayerPrintToChat(player, null!, MainPlugin.Instance.Localizer["PrintToChatToPlayer.NameChanging.Warning"], Configs.Instance.BlockNameChanger_Block);
            playerData.PlayerName_Block_Message = true;
        }

        player.ChangeTeam(CsTeam.Spectator);
    }

    private static void ExecuteBlockCommand(CCSPlayerController player)
    {
        if (!player.IsValid()) return;

        var player_name = player.PlayerName;
        var player_id = player.Slot.ToString();
        var player_ip = player.IpAddress ?? "";
        var player_ip_without_port = player.IpAddress?.Split(':')[0] ?? "";
        var player_steamid = player.AuthorizedSteamID?.SteamId2.ToString() ?? "";
        var player_steamid3 = player.AuthorizedSteamID?.SteamId3.ToString() ?? "";
        var player_steamid32 = player.AuthorizedSteamID?.SteamId32.ToString() ?? "";
        var player_steamid64 = player.AuthorizedSteamID?.SteamId64.ToString() ?? "";

        var server_command = Configs.Instance.BlockNameChanger_SendServerConsoleCommand.ReplaceChatMessages(
        PlayerName: player_name, Player_ID: player_id, PLAYER_IP: player_ip, PLAYER_IP_WITHOUT_PORT: player_ip_without_port,
        PLAYER_STEAMID: player_steamid, PLAYER_STEAMID3: player_steamid3, PLAYER_STEAMID32: player_steamid32, PLAYER_STEAMID64: player_steamid64);

        Server.ExecuteCommand(server_command);
    }

    public static void Timer_AutoClean_Callback()
    {
        var g_Main = MainPlugin.Instance.g_Main;

        var selectedWeapons = Configs.Instance.AutoClean_TheseDroppedWeaponsOnly
            .Select(weapon => weapon.Trim().ToLower())
            .ToList();

        var allWeaponsToClean = new HashSet<string>();

        if (selectedWeapons.Contains("ANY", StringComparer.OrdinalIgnoreCase) || selectedWeapons.Count == 0)
        {
            foreach (var category in WeaponCategories.Values)
            {
                allWeaponsToClean.UnionWith(category);
            }
        }
        else
        {
            foreach (var weaponKey in selectedWeapons)
            {
                if (WeaponCategories.ContainsKey(weaponKey.ToUpper()))
                {
                    allWeaponsToClean.UnionWith(WeaponCategories[weaponKey.ToUpper()]);
                }
                else
                {
                    allWeaponsToClean.Add(weaponKey.ToLower());
                }
            }
        }

        for (int i = g_Main.CbaseWeapons.Count - 1; i >= 0; i--)
        {
            var weapon = g_Main.CbaseWeapons[i];
            if (weapon == null || !weapon.IsValid)
            {
                g_Main.CbaseWeapons.RemoveAt(i);
                continue;
            }

            if (weapon.OwnerEntity != null && weapon.OwnerEntity.IsValid)
            {
                g_Main.CbaseWeapons.RemoveAt(i);
            }
        }

        foreach (var weaponClass in allWeaponsToClean)
        {
            foreach (var entity in Utilities.FindAllEntitiesByDesignerName<CBaseEntity>(weaponClass))
            {
                if (entity == null || !entity.IsValid || g_Main.CbaseWeapons.Contains(entity)) continue;

                if (entity.OwnerEntity == null || !entity.OwnerEntity.IsValid)
                {
                    g_Main.CbaseWeapons.Add(entity);
                }

            }
        }

        if (g_Main.CbaseWeapons.Count > Configs.Instance.AutoClean_MaxWeaponsOnGround)
        {
            int weaponsToRemove = g_Main.CbaseWeapons.Count - Configs.Instance.AutoClean_MaxWeaponsOnGround;

            for (int i = 0; i < weaponsToRemove && g_Main.CbaseWeapons.Count > 0; i++)
            {
                int weaponToRemoveIndex = g_Main.CbaseWeapons.Count == 1 ? 0 : Random.Shared.Next(0, g_Main.CbaseWeapons.Count - 1);

                var weaponToRemove = g_Main.CbaseWeapons[weaponToRemoveIndex];
                if (weaponToRemove == null || !weaponToRemove.IsValid) continue;

                if (weaponToRemove.OwnerEntity == null || !weaponToRemove.OwnerEntity.IsValid)
                {
                    weaponToRemove.AcceptInput("Kill");
                }
                g_Main.CbaseWeapons.RemoveAt(weaponToRemoveIndex);
            }
        }
    }

    public static async Task DownloadMissingFilesAsync()
    {
        try
        {
            string Fpath = Path.Combine(MainPlugin.Instance.ModuleDirectory, "GeoLocation");
            if (Directory.Exists(Fpath))
            {
                try
                {
                    Directory.Delete(Fpath, true);
                }
                catch
                {
                    
                }
            }
            
            await DownloadMissingFiles();
        }
        catch (Exception ex)
        {
            DebugMessage($"DownloadMissingFiles failed: {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
        }
    }

    public static async Task DownloadMissingFiles()
    {
        try
        {
            string settingsFileName = "config/chat_processor.json";
            string settingsGithubUrl = "https://raw.githubusercontent.com/oqyh/cs2-Game-Manager-GoldKingZ/main/Resources/chat_processor.json";
            await DownloadFromGitHub(settingsFileName, settingsGithubUrl);

            string geoFileName = Path.GetFullPath(Path.Combine(MainPlugin.Instance.ModuleDirectory, "..", "..", "shared/GoldKingZ/GeoLocation/GeoLite2-City.mmdb"));
            string geoUpdateUrl = "https://raw.githubusercontent.com/oqyh/cs2-Connect-Disconnect-Sound-GoldKingZ/main/Resources/update.txt";
            await DownloadFromGitHub(geoFileName, geoUpdateUrl, Configs.Instance.AutoUpdateGeoLocation);
        }
        catch (Exception ex)
        {
            DebugMessage($"DownloadMissingFiles Error: {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
        }
    }

    public static async Task DownloadFromGitHub(string filePath, string githubUrl, bool AutoUpdate = false)
    {
        try
        {
            string fullPath = Path.Combine(MainPlugin.Instance.ModuleDirectory, filePath);

            string? dir = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", $"CS2-Plugin-Game-Manager");
            client.Timeout = TimeSpan.FromSeconds(50);

            string actualDownloadUrl = githubUrl;

            if (githubUrl.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                actualDownloadUrl = await client.GetStringAsync(githubUrl);
                actualDownloadUrl = actualDownloadUrl.Trim();
            }

            byte[] remoteBytes = await client.GetByteArrayAsync(actualDownloadUrl);

            bool needDownload = !File.Exists(fullPath);

            if (!needDownload && AutoUpdate)
            {
                using var sha256 = SHA256.Create();
                string Hash(byte[] b) => BitConverter.ToString(sha256.ComputeHash(b)).Replace("-", "").ToLowerInvariant();

                byte[] localBytes = await File.ReadAllBytesAsync(fullPath);
                needDownload = Hash(localBytes) != Hash(remoteBytes);
            }

            if (needDownload)
            {
                await File.WriteAllBytesAsync(fullPath, remoteBytes);
            }
        }
        catch (Exception ex)
        {
            DebugMessage($"DownloadFromGitHub Error: {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
        }
    }

    public class MenuEntry
    {
        [String("SteamIDs", "Flags", "Groups")]
        public string Flags { get; set; } = "SteamIDs: 76561198206086993,STEAM_0:1:507335558 | Flags: @css/root,@root,admin | Groups: #css/root,#root,admin";
    }
    public static void LoadJson(bool playerload = false, CCSPlayerController player = null!, CommandInfo info = null!)
    {
        var g_Main = MainPlugin.Instance.g_Main;
        if (playerload && !player.IsValid()) return;

        string path = Path.Combine(MainPlugin.Instance.ModuleDirectory, "config/chat_processor.json");

        void Notify(string message, bool successfully = false)
        {
            if (playerload)
            {
                string color = successfully ? "\x06" : "\x02";
                AdvancedPlayerPrintToChat(player, info, $" \x04[Game Manager]: {color}{message}");
            }
            DebugMessage(message, Configs.Instance.EnableDebug.ToDebugConfig(1));
        }

        try
        {
            if (!File.Exists(path))
            {
                Notify($"{path} file does not exist.");
                g_Main.JsonData = null;
                return;
            }

            string[] allLines = File.ReadAllLines(path);
            string jsonContent = string.Join("\n", allLines.Where(l => !l.TrimStart().StartsWith("//")));

            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                Notify($"{path} content is empty.");
                g_Main.JsonData = null;
                return;
            }

            g_Main.JsonData = JObject.Parse(jsonContent);

            foreach (var property in g_Main.JsonData.Properties().ToList())
            {
                if (property.Name.Equals("ANY", StringComparison.OrdinalIgnoreCase)) continue;

                var entry = new MenuEntry { Flags = property.Name };
                Configs.ValidateStringRecursive(entry);

                if (entry.Flags != property.Name)
                {
                    var value = g_Main.JsonData[property.Name];
                    g_Main.JsonData.Remove(property.Name);
                    g_Main.JsonData[entry.Flags] = value;
                }
            }

            var formattedJson = g_Main.JsonData.ToString(Formatting.Indented);
            var commentLines = allLines.Where(l => l.TrimStart().StartsWith("//")).ToList();

            if (commentLines.Count > 0) commentLines.Add("");
            commentLines.Add(formattedJson);

            File.WriteAllText(path, string.Join(Environment.NewLine, commentLines));

            Notify($"{path} Loaded Successfully", true);
        }
        catch (JsonReaderException ex)
        {
            Notify($"JSON Syntax Error in chat_processor.json: {ex.Message}");
            g_Main.JsonData = null;
        }
        catch (Exception ex)
        {
            Notify($"General Error loading chat_processor.json: {ex.Message}");
            g_Main.JsonData = null;
        }
    }

    public static bool IsUrlWhitelisted(string detectedUrl, List<string> whitelist)
    {
        var normalizedDetected = detectedUrl.Replace(" ", "").ToLower();

        foreach (var whitelistEntry in whitelist)
        {
            var normalizedWhitelist = whitelistEntry.Trim().Replace(" ", "").ToLower();

            if (normalizedWhitelist.Contains("/") &&
                normalizedWhitelist.IndexOf('/') < normalizedWhitelist.Length - 1)
            {
                if (normalizedDetected == normalizedWhitelist)
                    return true;
            }
            else
            {
                var whitelistDomain = GetDomain(normalizedWhitelist);
                var detectedDomain = GetDomain(normalizedDetected);

                if (detectedDomain == whitelistDomain)
                    return true;
            }
        }
        return false;
    }

    public static string GetDomain(string url)
    {
        url = url.Replace("https://", "").Replace("http://", "").Replace("www.", "");

        var slashIndex = url.IndexOf('/');
        if (slashIndex >= 0)
            url = url.Substring(0, slashIndex);

        return url.TrimEnd('/');
    }

    
}