using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Text;
using Game_Manager_GoldKingZ.Config;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Core.Translations;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using CounterStrikeSharp.API.Modules.UserMessages;

namespace Game_Manager_GoldKingZ;

public class Game_UserMessages
{
    public HookResult HideBloodDecals_UserMessages(UserMessage um)
    {
        um.Recipients.Clear();
        return HookResult.Continue;
    }

    public HookResult HideBloodDecalsAndHideHeadShotSpark_UserMessages(UserMessage um)
    {
        var match = Regex.Match(um.DebugString, @"effectname:\s*(\d+)");
        if (!match.Success)return HookResult.Continue;

        if (!int.TryParse(match.Groups[1].Value, out var effectName))return HookResult.Continue;

        bool shouldBlock = (effectName == Globals_Static.EFFECT_BLOOD_SPURT_AND_IMPACT_FLESH && Configs.Instance.HideBloodDecals)
        || (effectName == Globals_Static.EFFECT_HEADSHOT_SPARK && Configs.Instance.HideHeadShotSpark);

        if (shouldBlock)
        {
            um.Recipients.Clear();
            return HookResult.Stop;
        }

        return HookResult.Continue;
    }

    

    public HookResult MuteGunShots_UserMessages(UserMessage um)
    {
        var weapon_id = um.ReadUInt("weapon_id");
        var sound_type = um.ReadInt("sound_type");
        var item_def_index = um.ReadInt("item_def_index");
        Helper.DebugMessage("[Sounds_MuteGunShots] weapon_id : " + weapon_id, Configs.Instance.EnableDebug.ToDebugConfig(3));
        Helper.DebugMessage("[Sounds_MuteGunShots] sound_type : " + sound_type, Configs.Instance.EnableDebug.ToDebugConfig(3));
        Helper.DebugMessage("[Sounds_MuteGunShots] item_def_index : " + item_def_index, Configs.Instance.EnableDebug.ToDebugConfig(3));

        if (Configs.Instance.Sounds_MuteGunShots == 1)
        {
            um.SetInt("sound_type", 0);
        }
        else if (Configs.Instance.Sounds_MuteGunShots == 2)
        {
            um.SetUInt("weapon_id", 0);
            um.SetInt("sound_type", 9);
            um.SetUInt("item_def_index", 60);
        }
        else if (Configs.Instance.Sounds_MuteGunShots == 3)
        {
            um.SetUInt("weapon_id", 0);
            um.SetInt("sound_type", 9);
            um.SetUInt("item_def_index", 61);
        }
        else if (Configs.Instance.Sounds_MuteGunShots == 4)
        {
            um.SetUInt("weapon_id", (uint)Configs.Instance.Sounds_MuteGunShots_weapon_id);
            um.SetInt("sound_type", Configs.Instance.Sounds_MuteGunShots_sound_type);
            um.SetUInt("item_def_index", (uint)Configs.Instance.Sounds_MuteGunShots_item_def_index);
        }
        return HookResult.Continue;
    }
    

    public HookResult Ignore_TextMsg_UserMessages(UserMessage um)
    {
        for (int i = 0; i < um.GetRepeatedFieldCount("param"); i++)
        {
            var message = um.ReadString("param", i);
            
            if (Configs.Instance.Ignore_TeamMateAttackMessages && Helper.TeamWarningArray.Contains(message)
            || Configs.Instance.Ignore_AwardsMoneyMessages && Helper.MoneyMessageArray.Contains(message)
            || Configs.Instance.Ignore_PlayerSavedYouByPlayerMessages && Helper.SavedbyArray.Contains(message)
            || Configs.Instance.Ignore_ChickenKilledMessages && message.Contains("#Pet_Killed")
            || Configs.Instance.Ignore_Custom_TextMsg.Contains(message))
            {
                Helper.DebugMessage("[Ignore_Custom] [TextMsg] - [MUTED] " + message, Configs.Instance.EnableDebug.ToDebugConfig(4));
                um.Recipients.Clear();
            } else
            {
                Helper.DebugMessage("[Ignore_Custom] [TextMsg] " + message, Configs.Instance.EnableDebug.ToDebugConfig(4));
            }
            
        }
                
        return HookResult.Continue;
    }

    public HookResult Ignore_RadioText_UserMessages(UserMessage um)
    {
        for (int i = 0; i < um.GetRepeatedFieldCount("params"); i++)
        {
            var message = um.ReadString("params", i);
            if (Configs.Instance.Ignore_PlantingBombMessages && message.Contains("#Cstrike_TitlesTXT_Planting_Bomb")
            || Configs.Instance.Ignore_DefusingBombMessages && message.Contains("#Cstrike_TitlesTXT_Defusing_Bomb")
            || Configs.Instance.Ignore_Custom_RadioText.Contains(message))
            {
                Helper.DebugMessage("[Ignore_Custom] [RadioText] - [MUTED] " + message, Configs.Instance.EnableDebug.ToDebugConfig(4));
                um.Recipients.Clear();
            } else
            {
                Helper.DebugMessage("[Ignore_Custom] [RadioText] " + message, Configs.Instance.EnableDebug.ToDebugConfig(4));
            }
            
        }
        return HookResult.Continue;
    }

    public HookResult Ignore_HintText_UserMessages(UserMessage um)
    {
        var message = um.ReadString("message");
        if (Configs.Instance.Ignore_TeamMateAttackMessages && Helper.TeamWarningArray.Contains(message)
        || Configs.Instance.Ignore_Custom_HintText.Contains(message))
        {
            Helper.DebugMessage("[Ignore_Custom] [HintText] - [MUTED] " + message, Configs.Instance.EnableDebug.ToDebugConfig(4));
            um.Recipients.Clear();
        }else
        {
            Helper.DebugMessage("[Ignore_Custom] [HintText] " + message, Configs.Instance.EnableDebug.ToDebugConfig(4));
        }

        return HookResult.Continue;
    }

    public HookResult MuteSounds_UserMessages(UserMessage um)
    {
        var g_Main = MainPlugin.Instance.g_Main;
        var soundevent = um.ReadUInt("soundevent_hash").ToString();
        var entityIndex = um.ReadInt("source_entity_index");

        var entity = Utilities.GetEntityFromIndex<CBaseEntity>(entityIndex);
        if (entity == null) return HookResult.Continue;

        var PlayerMadeSounds = entity.GetPlayerFromCBaseEntity();
        
        CCSPlayerController attacker = null!;
        CCSPlayerController victim = null!;
        if (PlayerMadeSounds.IsValid(true) && g_Main.Player_Data.TryGetValue(PlayerMadeSounds.Slot, out var handle))
        {
            if(handle.Attacker.IsValid(true))
            {
                attacker = handle.Attacker;
            }
            if(handle.Victim.IsValid(true))
            {
                victim = handle.Victim;
            }
        }

        bool Custom_Mute1 = false;
        bool Custom_Mute1_Victim = false;
        bool Custom_Mute1_Attacker = false;
        
        bool Custom_Mute2 = false;
        bool Custom_Mute2_Victim = false;
        bool Custom_Mute2_Attacker = false;
        
        bool Custom_Mute3 = false;
        bool Custom_Mute3_Victim = false;
        bool Custom_Mute3_Attacker = false;
        
        var config1 = Configs.Instance.Custom_MuteSounds_1;
        if (config1.Custom_MuteSounds1 > 0)
        {
            switch (config1.Custom_MuteSounds1)
            {
                case 1:
                    if (config1.Custom_MuteSounds1_SoundeventHashAndString_Global_Side.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        Custom_Mute1 = true;
                    }
                    break;
                        
                case 2 or 3:
                    if (config1.Custom_MuteSounds1_SoundeventHashAndString_Attacker_Side.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (attacker.IsValid(true) && 
                            MainPlugin.Instance._prefs != null && MainPlugin.Instance._prefs.TryGetValue(attacker.Slot, out var attackerData))
                        {
                            var toggleValue = attackerData.Toggle_Custom_MuteSounds1;
                            if (toggleValue == 1)
                            {
                                Custom_Mute1_Attacker = true;
                            }
                        }
                    }
                    
                    if (config1.Custom_MuteSounds1_SoundeventHashAndString_Victim_Side.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (victim.IsValid(true) && 
                            MainPlugin.Instance._prefs != null && MainPlugin.Instance._prefs.TryGetValue(victim.Slot, out var victimData))
                        {
                            var toggleValue = victimData.Toggle_Custom_MuteSounds1;
                            if (toggleValue == 1)
                            {
                                Custom_Mute1_Victim = true;
                            }
                        }
                    }
                    break;
            }
        }
        
        var config2 = Configs.Instance.Custom_MuteSounds_2;
        if (config2.Custom_MuteSounds2 > 0)
        {
            switch (config2.Custom_MuteSounds2)
            {
                case 1:
                    if (config2.Custom_MuteSounds2_SoundeventHashAndString_Global_Side.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        Custom_Mute2 = true;
                    }
                    break;
                        
                case 2 or 3:
                    if (config2.Custom_MuteSounds2_SoundeventHashAndString_Attacker_Side.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (attacker.IsValid(true) && 
                            MainPlugin.Instance._prefs != null && MainPlugin.Instance._prefs.TryGetValue(attacker.Slot, out var attackerData))
                        {
                            var toggleValue = attackerData.Toggle_Custom_MuteSounds2;
                            if (toggleValue == 1)
                            {
                                Custom_Mute2_Attacker = true;
                            }
                        }
                    }
                    
                    if (config2.Custom_MuteSounds2_SoundeventHashAndString_Victim_Side.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (victim.IsValid(true) && 
                            MainPlugin.Instance._prefs != null && MainPlugin.Instance._prefs.TryGetValue(victim.Slot, out var victimData))
                        {
                            var toggleValue = victimData.Toggle_Custom_MuteSounds2;
                            if (toggleValue == 1)
                            {
                                Custom_Mute2_Victim = true;
                            }
                        }
                    }
                    break;
            }
        }
        
        var config3 = Configs.Instance.Custom_MuteSounds_3;
        if (config3.Custom_MuteSounds3 > 0)
        {
            switch (config3.Custom_MuteSounds3)
            {
                case 1:
                    if (config3.Custom_MuteSounds3_SoundeventHashAndString_Global_Side.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        Custom_Mute3 = true;
                    }
                    break;
                        
                case 2 or 3:
                    if (config3.Custom_MuteSounds3_SoundeventHashAndString_Attacker_Side.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (attacker.IsValid(true) && 
                            MainPlugin.Instance._prefs != null && MainPlugin.Instance._prefs.TryGetValue(attacker.Slot, out var attackerData))
                        {
                            var toggleValue = attackerData.Toggle_Custom_MuteSounds3;
                            if (toggleValue == 1)
                            {
                                Custom_Mute3_Attacker = true;
                            }
                        }
                    }
                    
                    if (config3.Custom_MuteSounds3_SoundeventHashAndString_Victim_Side.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (victim.IsValid(true) && 
                            MainPlugin.Instance._prefs != null && MainPlugin.Instance._prefs.TryGetValue(victim.Slot, out var victimData))
                        {
                            var toggleValue = victimData.Toggle_Custom_MuteSounds3;
                            if (toggleValue == 1)
                            {
                                Custom_Mute3_Victim = true;
                            }
                        }
                    }
                    break;
            }
        }
        
        bool MuteKnife = false;
        if (Configs.Instance.Sounds_MuteKnife > 0 && 
            Configs.Instance.Sounds_MuteKnife_SoundeventHashAndString.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
        {
            switch (Configs.Instance.Sounds_MuteKnife)
            {
                case 1:
                    MuteKnife = true;
                    break;
                case 2:
                    MuteKnife = g_Main.Player_Data.Values.Any(player => player.StabedHisTeamMate == true);
                    break;
            }
        }

        if (MuteKnife
            || Custom_Mute1 || Custom_Mute1_Victim || Custom_Mute1_Attacker
            || Custom_Mute2 || Custom_Mute2_Victim || Custom_Mute2_Attacker
            || Custom_Mute3 || Custom_Mute3_Victim || Custom_Mute3_Attacker)
        {
            if(attacker.IsValid(true))
            {
                Helper.DebugMessage($"[Custom_MuteSounds][UserMessages] [MUTED] - Attacker: {attacker.PlayerName} : {soundevent}", Configs.Instance.EnableDebug.ToDebugConfig(2));
            }else if(victim.IsValid(true))
            {
                Helper.DebugMessage($"[Custom_MuteSounds][UserMessages] [MUTED] - Victim: {victim.PlayerName} : {soundevent}", Configs.Instance.EnableDebug.ToDebugConfig(2));
            }else if(PlayerMadeSounds.IsValid(true))
            {
                Helper.DebugMessage($"[Custom_MuteSounds][UserMessages] [MUTED] - Player: {PlayerMadeSounds.PlayerName} : {soundevent}", Configs.Instance.EnableDebug.ToDebugConfig(2));
            }else
            {
                Helper.DebugMessage($"[Custom_MuteSounds][UserMessages] [MUTED] - GLOBAL: {soundevent}", Configs.Instance.EnableDebug.ToDebugConfig(2));
            }
            return HookResult.Stop;
        }
        else
        {
            if(attacker.IsValid(true))
            {
                Helper.DebugMessage($"[Custom_MuteSounds][UserMessages] Attacker: {attacker.PlayerName} : {soundevent}", Configs.Instance.EnableDebug.ToDebugConfig(2));
            }else if(victim.IsValid(true))
            {
                Helper.DebugMessage($"[Custom_MuteSounds][UserMessages] Victim: {victim.PlayerName} : {soundevent}", Configs.Instance.EnableDebug.ToDebugConfig(2));
            }else if(PlayerMadeSounds.IsValid(true))
            {
                Helper.DebugMessage($"[Custom_MuteSounds][UserMessages] Player: {PlayerMadeSounds.PlayerName} : {soundevent}", Configs.Instance.EnableDebug.ToDebugConfig(2));
            }else
            {
                Helper.DebugMessage($"[Custom_MuteSounds][UserMessages] GLOBAL: {soundevent}", Configs.Instance.EnableDebug.ToDebugConfig(2));
            }
        }
        
        return HookResult.Continue;
    }

    public HookResult MuteSounds_WeaponSound(UserMessage um)
    {
        var g_Main = MainPlugin.Instance.g_Main;
        var soundevent = um.ReadString("sound");
        var entityIndex = um.ReadInt("entidx");

        var entity = Utilities.GetEntityFromIndex<CBaseEntity>(entityIndex);
        if (entity == null) return HookResult.Continue;

        var PlayerMadeSounds = entity.GetPlayerFromCBaseEntity();
        
        bool Custom_Mute1 = false;
        bool Custom_Mute1_Attacker = false;
        
        bool Custom_Mute2 = false;
        bool Custom_Mute2_Attacker = false;
        
        bool Custom_Mute3 = false;
        bool Custom_Mute3_Attacker = false;
        
        var config1 = Configs.Instance.Custom_MuteSounds_1;
        if (config1.Custom_MuteSounds1 > 0)
        {
            switch (config1.Custom_MuteSounds1)
            {
                case 1:
                    if (config1.Custom_MuteSounds1_SoundeventHashAndString_Global_Side.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        Custom_Mute1 = true;
                    }
                    break;
                        
                case 2 or 3:
                    if (config1.Custom_MuteSounds1_SoundeventHashAndString_Attacker_Side.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (PlayerMadeSounds.IsValid(true) && MainPlugin.Instance._prefs != null && MainPlugin.Instance._prefs.TryGetValue(PlayerMadeSounds.Slot, out var attackerData))
                        {
                            var toggleValue = attackerData.Toggle_Custom_MuteSounds1;
                            if (toggleValue == 1)
                            {
                                Custom_Mute1_Attacker = true;
                            }
                        }
                    }
                    break;
            }
        }
        
        var config2 = Configs.Instance.Custom_MuteSounds_2;
        if (config2.Custom_MuteSounds2 > 0)
        {
            switch (config2.Custom_MuteSounds2)
            {
                case 1:
                    if (config2.Custom_MuteSounds2_SoundeventHashAndString_Global_Side.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        Custom_Mute2 = true;
                    }
                    break;
                        
                case 2 or 3:
                    if (config2.Custom_MuteSounds2_SoundeventHashAndString_Attacker_Side.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (PlayerMadeSounds.IsValid(true) && MainPlugin.Instance._prefs != null && MainPlugin.Instance._prefs.TryGetValue(PlayerMadeSounds.Slot, out var attackerData))
                        {
                            var toggleValue = attackerData.Toggle_Custom_MuteSounds2;
                            if (toggleValue == 1)
                            {
                                Custom_Mute2_Attacker = true;
                            }
                        }
                    }
                    break;
            }
        }
        
        var config3 = Configs.Instance.Custom_MuteSounds_3;
        if (config3.Custom_MuteSounds3 > 0)
        {
            switch (config3.Custom_MuteSounds3)
            {
                case 1:
                    if (config3.Custom_MuteSounds3_SoundeventHashAndString_Global_Side.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        Custom_Mute3 = true;
                    }
                    break;
                        
                case 2 or 3:
                    if (config3.Custom_MuteSounds3_SoundeventHashAndString_Attacker_Side.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (PlayerMadeSounds.IsValid(true) && MainPlugin.Instance._prefs != null && MainPlugin.Instance._prefs.TryGetValue(PlayerMadeSounds.Slot, out var attackerData))
                        {
                            var toggleValue = attackerData.Toggle_Custom_MuteSounds3;
                            if (toggleValue == 1)
                            {
                                Custom_Mute3_Attacker = true;
                            }
                        }
                    }
                    break;
            }
        }
        
        bool MuteKnife = false;
        if (Configs.Instance.Sounds_MuteKnife > 0 && 
            Configs.Instance.Sounds_MuteKnife_SoundeventHashAndString.Any(x => soundevent.Contains(x, StringComparison.OrdinalIgnoreCase)))
        {
            switch (Configs.Instance.Sounds_MuteKnife)
            {
                case 1:
                    MuteKnife = true;
                    break;
                case 2:
                    MuteKnife = g_Main.Player_Data.Values.Any(player => player.StabedHisTeamMate == true);
                    break;
            }
        }
        
        if (MuteKnife
            || Custom_Mute1 || Custom_Mute1_Attacker
            || Custom_Mute2 || Custom_Mute2_Attacker
            || Custom_Mute3 || Custom_Mute3_Attacker)
        {
            if(PlayerMadeSounds.IsValid(true))
            {
                Helper.DebugMessage($"[Custom_MuteSounds][WeaponSound] [MUTED] - Attacker: {PlayerMadeSounds.PlayerName} : {soundevent}", Configs.Instance.EnableDebug.ToDebugConfig(2));
            }else
            {
                Helper.DebugMessage($"[Custom_MuteSounds][WeaponSound] [MUTED] - GLOBAL: {soundevent}", Configs.Instance.EnableDebug.ToDebugConfig(2));
            }
            return HookResult.Stop;
        }
        else
        {
            if(PlayerMadeSounds.IsValid(true))
            {
                Helper.DebugMessage($"[Custom_MuteSounds][WeaponSound] - Attacker: {PlayerMadeSounds.PlayerName} : {soundevent}", Configs.Instance.EnableDebug.ToDebugConfig(2));
            }else
            {
                Helper.DebugMessage($"[Custom_MuteSounds][WeaponSound] - GLOBAL: {soundevent}", Configs.Instance.EnableDebug.ToDebugConfig(2));
            }
        }
        
        return HookResult.Continue;
    }
    
    public HookResult HookPlayerChat_UserMessages(UserMessage? um, CCSPlayerController? player, string message, bool TeamChat)
    {
        if (!player.IsValid()) return HookResult.Continue;

        if (Configs.Instance.AntiFlood_Messages > 0)
        {
            if (Handle_AntiFlood(player!, um!))
            {
                return HookResult.Handled;
            }
        }

        if (Configs.Instance.Filter_Players_Chat > 0)
        {
            if (Handle_FilterPlayersChat(player!, message, null!, um!))
            {
                return HookResult.Handled;
            }
        }

        if (Configs.Instance.Reload_GameManager.Reload_GameManager_CommandsInGame.ConvertCommands(true)?.Any(c => message.Equals(c.Trim(), StringComparison.OrdinalIgnoreCase)) == true)
        {
            Handle_ReloadPlugin(player!, null!, um!);
        }

        if (Configs.Instance.Disable_AimPunch.DisableAimPunch > 1)
        {
            if (Configs.Instance.Disable_AimPunch.DisableAimPunch_CommandsInGame.ConvertCommands(true)?.Any(c => message.Equals(c.Trim(), StringComparison.OrdinalIgnoreCase)) == true)
            {
                Handle_AimPunch(player!, null!, um!);
            }
        }

        if (Configs.Instance.Custom_MuteSounds_1.Custom_MuteSounds1 > 1)
        {
            if (Configs.Instance.Custom_MuteSounds_1.Custom_MuteSounds1_CommandsInGame.ConvertCommands(true)?.Any(c => message.Equals(c.Trim(), StringComparison.OrdinalIgnoreCase)) == true)
            {
                Handle_Custom_MuteSounds1(player!, null!, um!);
            }
        }

        if (Configs.Instance.Custom_MuteSounds_2.Custom_MuteSounds2 > 1)
        {
            if (Configs.Instance.Custom_MuteSounds_2.Custom_MuteSounds2_CommandsInGame.ConvertCommands(true)?.Any(c => message.Equals(c.Trim(), StringComparison.OrdinalIgnoreCase)) == true)
            {
                Handle_Custom_MuteSounds2(player!, null!, um!);
            }
        }

        if (Configs.Instance.Custom_MuteSounds_3.Custom_MuteSounds3 > 1)
        {
            if (Configs.Instance.Custom_MuteSounds_3.Custom_MuteSounds3_CommandsInGame.ConvertCommands(true)?.Any(c => message.Equals(c.Trim(), StringComparison.OrdinalIgnoreCase)) == true)
            {
                Handle_Custom_MuteSounds3(player!, null!, um!);
            }
        }

        if (Configs.Instance.Custom_ChatMessages)
        {
            Handle_CustomChatMessages(player!, message, null!, um!);
        }

        return HookResult.Continue;
    }


    #region Commands Hook
    
    public void CommandsAction_ReloadPlugin(CCSPlayerController? player, CommandInfo info)
    {
        if (!player.IsValid()) return;

        Handle_ReloadPlugin(player, info, null!);
    }

    public void CommandsAction_AimPunch(CCSPlayerController? player, CommandInfo info)
    {
        if (!player.IsValid()) return;

        Handle_AimPunch(player, info, null!);
    }

    public void CommandsAction_Custom_MuteSounds1(CCSPlayerController? player, CommandInfo info)
    {
        if (!player.IsValid()) return;

        Handle_Custom_MuteSounds1(player, info, null!);
    }

    public void CommandsAction_Custom_MuteSounds2(CCSPlayerController? player, CommandInfo info)
    {
        if (!player.IsValid()) return;

        Handle_Custom_MuteSounds2(player, info, null!);
    }

    public void CommandsAction_Custom_MuteSounds3(CCSPlayerController? player, CommandInfo info)
    {
        if (!player.IsValid()) return;

        Handle_Custom_MuteSounds3(player, info, null!);
    }

    #endregion Commands Hook




    #region Handles

    public static bool Handle_AntiFlood(CCSPlayerController player, UserMessage um = null!)
    {
        if (Configs.Instance.AntiFlood_Messages <= 0) return false;
        if (!MainPlugin.Instance.g_Main.Player_Data.TryGetValue(player.Slot, out var playerData)) return false;
        if (Configs.Instance.AntiFlood_Ignore_Flags.HasValidPermissionData() && Helper.IsPlayerInGroupPermission(player, Configs.Instance.AntiFlood_Ignore_Flags)) return false;

        bool onetime = (DateTime.Now - playerData.AntiFlood_CountTime).TotalSeconds > 0.4;
        if (onetime) playerData.AntiFlood_CountTime = DateTime.Now;

        if (playerData.AntiFlood_BlockedUntil > DateTime.Now)
        {
            var remaining = (playerData.AntiFlood_BlockedUntil - DateTime.Now).TotalSeconds;
            if (onetime) Helper.AdvancedPlayerPrintToChat(player, null!, MainPlugin.Instance.Localizer["PrintToChatToPlayer.AntiFlood.Blocked"], (int)Math.Ceiling(remaining));
            um?.Recipients.Clear();
            return true;
        }

        if ((DateTime.Now - playerData.AntiFlood_WindowStart).TotalSeconds > Configs.Instance.AntiFlood_Seconds)
        {
            playerData.AntiFlood_Times = 0;
            playerData.AntiFlood_WindowStart = DateTime.Now;
        }

        if (onetime) playerData.AntiFlood_Times++;

        if (playerData.AntiFlood_Times > Configs.Instance.AntiFlood_Messages)
        {
            playerData.AntiFlood_BlockedUntil = DateTime.Now.AddSeconds(Configs.Instance.AntiFlood_PunishCooldown);
            playerData.AntiFlood_Times = 0;

            if (onetime) Helper.AdvancedPlayerPrintToChat(player, null!, MainPlugin.Instance.Localizer["PrintToChatToPlayer.AntiFlood.Blocked"], Configs.Instance.AntiFlood_PunishCooldown);
            um?.Recipients.Clear();
            return true;
        }

        return false;
    }

    public static bool Handle_FilterPlayersChat(CCSPlayerController player, string message, CommandInfo commandInfo = null!, UserMessage um = null!)
    {
        if (Configs.Instance.Filter_Players_Chat <= 0) return false;
        if (!MainPlugin.Instance.g_Main.Player_Data.TryGetValue(player.Slot, out var playerData)) return false;

        bool onetime = (DateTime.Now - playerData.EventPlayerChat_Filter).TotalSeconds > 0.4;
        if (onetime) playerData.EventPlayerChat_Filter = DateTime.Now;

        string msgNoSpaces = message.Replace(" ", "");

        if (Configs.Instance.Filter_Players_Chat == 1 || Configs.Instance.Filter_Players_Chat == 3)
        {
            var ipMatches = Regex.Matches(
                msgNoSpaces,
                @"(?<!\d)(?:\d{1,4}\.){2,3}\d{1,4}(?!\d)"
            );

            bool hasBlockedIp = ipMatches.Cast<Match>()
                .Select(m => m.Value)
                .Any(ip =>
                    !Configs.Instance.Filter_Whitelist_Ips.Any(w =>
                        ip.Equals(w, StringComparison.OrdinalIgnoreCase)));

            if (hasBlockedIp)
            {
                if (onetime) Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.Filter.IPs"]);
                um?.Recipients.Clear();
                return true;
            }
        }

        if (Configs.Instance.Filter_Players_Chat == 2 || Configs.Instance.Filter_Players_Chat == 3)
        {
            var urlMatches = Regex.Matches(
                msgNoSpaces,
                @"(?:https?:\/\/)?(?:www\.)?(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(?:\/[^\s]*)?",
                RegexOptions.IgnoreCase | RegexOptions.Compiled
            );

            bool hasBlockedUrl = urlMatches.Cast<Match>()
                .Select(m => m.Value.Trim())
                .Where(u => !string.IsNullOrEmpty(u))
                .Any(url => !Helper.IsUrlWhitelisted(url, Configs.Instance.Filter_Whitelist_URLs));

            if (hasBlockedUrl)
            {
                if (onetime) Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.Filter.URLs"]);
                um?.Recipients.Clear();
                return true;
            }
        }

        return false;
    }

    public static void Handle_CustomChatMessages(CCSPlayerController player, string message, CommandInfo commandInfo = null!, UserMessage um = null!)
    {
        if (!Configs.Instance.Custom_ChatMessages) return;
        if (!MainPlugin.Instance.g_Main.Player_Data.TryGetValue(player.Slot, out var playerData)) return;

        if (Configs.Instance.Custom_ChatMessages_ExcludeStartWith.Any(exclude => message.StartsWith(exclude, Helper.GetComparison(Configs.Instance.Custom_ChatMessages_ExcludeStartWith_IgnoreCase)))
        || Configs.Instance.Custom_ChatMessages_ExcludeContains.Any(exclude => message.Contains(exclude, Helper.GetComparison(Configs.Instance.Custom_ChatMessages_ExcludeContains_IgnoreCase)))) return;

        bool onetime = (DateTime.Now - playerData.EventPlayerChat).TotalSeconds > 0.4;
        if (onetime) playerData.EventPlayerChat = DateTime.Now;

        if (onetime)
        {
            string messageKey = Helper.DetermineMessageKey(playerData.MessageType, player);
            string chatType = messageKey.Split('_').ElementAtOrDefault(2) ?? "ALL";
            bool MessageIsTeamSided = chatType == "TEAM";
            var GetValues = Helper.GetValuesInJson(player, messageKey);
            if (!string.IsNullOrEmpty(GetValues.formatString))
            {
                foreach (var players in Helper.GetPlayersController(false, false, false))
                {
                    if (!players.IsValid()) continue;

                    bool canSeeMessage = true;

                    switch (Configs.Instance.Custom_ChatMessages_Mode)
                    {
                        case 1:
                            canSeeMessage = true;
                            break;

                        case 2:
                            if (players.IsAlive() && !player.IsAlive())
                                canSeeMessage = false;
                            break;

                        case 3:
                            if (players.IsAlive() && !player.IsAlive())
                            {
                                if (players.TeamNum != player.TeamNum)
                                    canSeeMessage = false;
                            }
                            break;
                    }

                    if (MessageIsTeamSided && players.TeamNum != player.TeamNum) continue;

                    if (!canSeeMessage) continue;

                    var message_formate = GetValues.formatString?.ReplaceChatMessages(clan_chat: GetValues.ClanTag_Chat ?? "", clan_scoreboard: GetValues.ClanTag_ScoreBoard ?? "", PlayerName: player.PlayerName.RemoveColorNames(), location: player.PlayerPawn.Value?.LastPlaceName ?? "", message: message.RemoveColorNames(), team_color: player.TeamNum.ToTeamColor());
                    Helper.AdvancedPlayerPrintToChat(players, null!, message_formate!);
                }
            }
        }
        um?.Recipients.Clear();
    }

    public static void Handle_ReloadPlugin(CCSPlayerController player, CommandInfo commandInfo = null!, UserMessage um = null!)
    {
        if (!MainPlugin.Instance.g_Main.Player_Data.TryGetValue(player.Slot, out var playerData)) return;

        bool onetime = (DateTime.Now - playerData.EventPlayerChat).TotalSeconds > 0.4;
        if (onetime) playerData.EventPlayerChat = DateTime.Now;


        var cfg = Configs.Instance.Reload_GameManager;

        if (cfg.Reload_GameManager_Flags.HasValidPermissionData() && !Helper.IsPlayerInGroupPermission(player, cfg.Reload_GameManager_Flags))
        {
            if (onetime)
            {
                Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.ReloadPlugin.Not.Allowed"]);
            }
        }
        else
        {
            if (onetime)
            {
                Server.NextFrame(() =>
                {
                    Helper.RemoveRegisterCommandsAndHooks();
                    Helper.ClearVariables();
                    
                    Configs.Load(MainPlugin.Instance.ModuleDirectory);
                    Helper.LoadJson(true, player);

                    Helper.DownloadMissingFiles();
                    Helper.RegisterCommandsAndHooks();
                    Helper.ExectueCommands();
                    Helper.ReloadPlayersGlobals();
                    Helper.StartTimer();
                });

                Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.ReloadPlugin.Successfully"]);
            }

            Helper.MuteCommands(um, cfg.Reload_GameManager_Hide);
        }

        Helper.MuteCommands(um, cfg.Reload_GameManager_Hide, true);
    }


    public static void Handle_AimPunch(CCSPlayerController player, CommandInfo commandInfo = null!, UserMessage um = null!)
    {
        if (MainPlugin.Instance._prefs == null || !MainPlugin.Instance._prefs.TryGetValue(player.Slot, out var prefs) || !MainPlugin.Instance.g_Main.Player_Data.TryGetValue(player.Slot, out var playerData)) return;

        bool onetime = (DateTime.Now - playerData.EventPlayerChat).TotalSeconds > 0.4;
        if (onetime) playerData.EventPlayerChat = DateTime.Now;


        var cfg = Configs.Instance.Disable_AimPunch;

        if (cfg.DisableAimPunch_Flags.HasValidPermissionData() && !Helper.IsPlayerInGroupPermission(player, cfg.DisableAimPunch_Flags))
        {
            if (onetime)
            {
                Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.Toggle.AntiAimPunch.Not.Allowed"]);
            }
        }else
        {
            if (onetime)
            {
                prefs.Toggle_AimPunch = prefs.Toggle_AimPunch.ToggleOnOff();
                if(prefs.Toggle_AimPunch == 1)
                {
                    Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.Toggle.AntiAimPunch.Enabled"]);
                }else if(prefs.Toggle_AimPunch == 2)
                {
                    Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.Toggle.AntiAimPunch.Disabled"]);
                }
            }

            Helper.MuteCommands(um, cfg.DisableAimPunch_Hide);
        }

        Helper.MuteCommands(um, cfg.DisableAimPunch_Hide, true);
    }


    public static void Handle_Custom_MuteSounds1(CCSPlayerController player, CommandInfo commandInfo = null!, UserMessage um = null!)
    {
        if (MainPlugin.Instance._prefs == null || !MainPlugin.Instance._prefs.TryGetValue(player.Slot, out var prefs) || !MainPlugin.Instance.g_Main.Player_Data.TryGetValue(player.Slot, out var playerData)) return;

        bool onetime = (DateTime.Now - playerData.EventPlayerChat).TotalSeconds > 0.4;
        if (onetime) playerData.EventPlayerChat = DateTime.Now;


        var cfg = Configs.Instance.Custom_MuteSounds_1;

        if (cfg.Custom_MuteSounds1_Flags.HasValidPermissionData() && !Helper.IsPlayerInGroupPermission(player, cfg.Custom_MuteSounds1_Flags))
        {
            if (onetime)
            {
                Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.Toggle.MuteSounds_1.Not.Allowed"]);
            }
        }else
        {
            if (onetime)
            {
                prefs.Toggle_Custom_MuteSounds1 = prefs.Toggle_Custom_MuteSounds1.ToggleOnOff();
                if(prefs.Toggle_Custom_MuteSounds1 == 1)
                {
                    Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.Toggle.MuteSounds_1.Enabled"]);
                }else if(prefs.Toggle_Custom_MuteSounds1 == 2)
                {
                    Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.Toggle.MuteSounds_1.Disabled"]);
                }
            }

            Helper.MuteCommands(um, cfg.Custom_MuteSounds1_Hide);
        }

        Helper.MuteCommands(um, cfg.Custom_MuteSounds1_Hide, true);
    }

    public static void Handle_Custom_MuteSounds2(CCSPlayerController player, CommandInfo commandInfo = null!, UserMessage um = null!)
    {
        if (MainPlugin.Instance._prefs == null || !MainPlugin.Instance._prefs.TryGetValue(player.Slot, out var prefs) || !MainPlugin.Instance.g_Main.Player_Data.TryGetValue(player.Slot, out var playerData)) return;

        bool onetime = (DateTime.Now - playerData.EventPlayerChat).TotalSeconds > 0.4;
        if (onetime) playerData.EventPlayerChat = DateTime.Now;


        var cfg = Configs.Instance.Custom_MuteSounds_2;

        if (cfg.Custom_MuteSounds2_Flags.HasValidPermissionData() && !Helper.IsPlayerInGroupPermission(player, cfg.Custom_MuteSounds2_Flags))
        {
            if (onetime)
            {
                Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.Toggle.MuteSounds_2.Not.Allowed"]);
            }
        }else
        {
            if (onetime)
            {
                prefs.Toggle_Custom_MuteSounds2 = prefs.Toggle_Custom_MuteSounds2.ToggleOnOff();
                if(prefs.Toggle_Custom_MuteSounds2 == 1)
                {
                    Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.Toggle.MuteSounds_2.Enabled"]);
                }else if(prefs.Toggle_Custom_MuteSounds2 == 2)
                {
                    Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.Toggle.MuteSounds_2.Disabled"]);
                }
            }

            Helper.MuteCommands(um, cfg.Custom_MuteSounds2_Hide);
        }

        Helper.MuteCommands(um, cfg.Custom_MuteSounds2_Hide, true);
    }

    public static void Handle_Custom_MuteSounds3(CCSPlayerController player, CommandInfo commandInfo = null!, UserMessage um = null!)
    {
        if (MainPlugin.Instance._prefs == null || !MainPlugin.Instance._prefs.TryGetValue(player.Slot, out var prefs) || !MainPlugin.Instance.g_Main.Player_Data.TryGetValue(player.Slot, out var playerData)) return;

        bool onetime = (DateTime.Now - playerData.EventPlayerChat).TotalSeconds > 0.4;
        if (onetime) playerData.EventPlayerChat = DateTime.Now;


        var cfg = Configs.Instance.Custom_MuteSounds_3;

        if (cfg.Custom_MuteSounds3_Flags.HasValidPermissionData() && !Helper.IsPlayerInGroupPermission(player, cfg.Custom_MuteSounds3_Flags))
        {
            if (onetime)
            {
                Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.Toggle.MuteSounds_3.Not.Allowed"]);
            }
        }else
        {
            if (onetime)
            {
                prefs.Toggle_Custom_MuteSounds3 = prefs.Toggle_Custom_MuteSounds3.ToggleOnOff();
                if(prefs.Toggle_Custom_MuteSounds3 == 1)
                {
                    Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.Toggle.MuteSounds_3.Enabled"]);
                }else if(prefs.Toggle_Custom_MuteSounds3 == 2)
                {
                    Helper.AdvancedPlayerPrintToChat(player, commandInfo, MainPlugin.Instance.Localizer["PrintToChatToPlayer.Toggle.MuteSounds_3.Disabled"]);
                }
            }

            Helper.MuteCommands(um, cfg.Custom_MuteSounds3_Hide);
        }

        Helper.MuteCommands(um, cfg.Custom_MuteSounds3_Hide, true);
    }

    #endregion Handles


}