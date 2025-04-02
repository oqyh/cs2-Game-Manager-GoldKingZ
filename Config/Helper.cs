using System.Text.RegularExpressions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Core.Translations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Game_Manager_GoldKingZ.Config;

namespace Game_Manager_GoldKingZ;

public class Helper
{
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
    public static string[] MoneyMessageArray = new string[] {
    "Player_Cash_Award_Kill_Teammate",
    "Player_Cash_Award_Killed_VIP",
    "Player_Cash_Award_Killed_Enemy_Generic",
    "Player_Cash_Award_Killed_Enemy",
    "Player_Cash_Award_Bomb_Planted",
    "Player_Cash_Award_Bomb_Defused",
    "Player_Cash_Award_Rescued_Hostage",
    "Player_Cash_Award_Interact_Hostage",
    "Player_Cash_Award_Respawn",
    "Player_Cash_Award_Get_Killed",
    "Player_Cash_Award_Damage_Hostage",
    "Player_Cash_Award_Kill_Hostage",
    "Player_Point_Award_Killed_Enemy",
    "Player_Point_Award_Killed_Enemy_Plural",
    "Player_Point_Award_Killed_Enemy_NoWeapon",
    "Player_Point_Award_Killed_Enemy_NoWeapon_Plural",
    "Player_Point_Award_Assist_Enemy",
    "Player_Point_Award_Assist_Enemy_Plural",
    "Player_Point_Award_Picked_Up_Dogtag",
    "Player_Point_Award_Picked_Up_Dogtag_Plural",
    "Player_Team_Award_Killed_Enemy",
    "Player_Team_Award_Killed_Enemy_Plural",
    "Player_Team_Award_Bonus_Weapon",
    "Player_Team_Award_Bonus_Weapon_Plural",
    "Player_Team_Award_Picked_Up_Dogtag",
    "Player_Team_Award_Picked_Up_Dogtag_Plural",
    "Player_Team_Award_Picked_Up_Dogtag_Friendly",
    "Player_Cash_Award_ExplainSuicide_YouGotCash",
    "Player_Cash_Award_ExplainSuicide_TeammateGotCash",
    "Player_Cash_Award_ExplainSuicide_EnemyGotCash",
    "Player_Cash_Award_ExplainSuicide_Spectators",
    "Team_Cash_Award_T_Win_Bomb",
    "Team_Cash_Award_Elim_Hostage",
    "Team_Cash_Award_Elim_Bomb",
    "Team_Cash_Award_Win_Time",
    "Team_Cash_Award_Win_Defuse_Bomb",
    "Team_Cash_Award_Win_Hostages_Rescue",
    "Team_Cash_Award_Win_Hostage_Rescue",
    "Team_Cash_Award_Loser_Bonus",
    "Team_Cash_Award_Bonus_Shorthanded",
    "Notice_Bonus_Enemy_Team",
    "Notice_Bonus_Shorthanded_Eligibility",
    "Notice_Bonus_Shorthanded_Eligibility_Single",
    "Team_Cash_Award_Loser_Bonus_Neg",
    "Team_Cash_Award_Loser_Zero",
    "Team_Cash_Award_Rescued_Hostage",
    "Team_Cash_Award_Hostage_Interaction",
    "Team_Cash_Award_Hostage_Alive",
    "Team_Cash_Award_Planted_Bomb_But_Defused",
    "Team_Cash_Award_Survive_GuardianMode_Wave",
    "Team_Cash_Award_CT_VIP_Escaped",
    "Team_Cash_Award_T_VIP_Killed",
    "Team_Cash_Award_no_income",
    "Team_Cash_Award_no_income_suicide",
    "Team_Cash_Award_Generic",
    "Team_Cash_Award_Custom"
    };
    public static string[] SavedbyArray = new string[] {
    "Chat_SavePlayer_Savior",
    "Chat_SavePlayer_Spectator",
    "Chat_SavePlayer_Saved"
    };
    public static string[] TeamWarningArray = new string[] {
    "Cstrike_TitlesTXT_Game_teammate_attack",
    "Cstrike_TitlesTXT_Game_teammate_kills",
    "Cstrike_TitlesTXT_Hint_careful_around_teammates",
    "Cstrike_TitlesTXT_Hint_try_not_to_injure_teammates",
    "Cstrike_TitlesTXT_Killed_Teammate",
    "SFUI_Notice_Game_teammate_kills",
    "SFUI_Notice_Hint_careful_around_teammates",
    "SFUI_Notice_Killed_Teammate"
    };
    public static void AdvancedPlayerPrintToChat(CCSPlayerController player, string message, params object[] args)
    {
        if (string.IsNullOrEmpty(message))return;

        for (int i = 0; i < args.Length; i++)
        {
            message = message.Replace($"{{{i}}}", args[i].ToString());
        }
        if (Regex.IsMatch(message, "{nextline}", RegexOptions.IgnoreCase))
        {
            string[] parts = Regex.Split(message, "{nextline}", RegexOptions.IgnoreCase);
            foreach (string part in parts)
            {
                string messages = part.Trim();
                player.PrintToChat(" " + messages);
            }
        }else
        {
            player.PrintToChat(message);
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
    
    public static List<CCSPlayerController> GetPlayersController(bool IncludeBots = false, bool IncludeSPEC = true, bool IncludeCT = true, bool IncludeT = true) 
    {
        var playerList = Utilities
            .FindAllEntitiesByDesignerName<CCSPlayerController>("cs_player_controller")
            .Where(p => p != null && p.IsValid && 
                        (IncludeBots || (!p.IsBot && !p.IsHLTV)) && 
                        p.Connected == PlayerConnectedState.PlayerConnected && 
                        ((IncludeCT && p.TeamNum == (byte)CsTeam.CounterTerrorist) || 
                        (IncludeT && p.TeamNum == (byte)CsTeam.Terrorist) || 
                        (IncludeSPEC && p.TeamNum == (byte)CsTeam.Spectator)))
            .ToList();

        return playerList;
    }
    public static int GetPlayersCount(bool IncludeBots = false, bool IncludeSPEC = true, bool IncludeCT = true, bool IncludeT = true)
    {
        return Utilities.GetPlayers().Count(p => 
            p != null && 
            p.IsValid && 
            p.Connected == PlayerConnectedState.PlayerConnected && 
            (IncludeBots || (!p.IsBot && !p.IsHLTV)) && 
            ((IncludeCT && p.TeamNum == (byte)CsTeam.CounterTerrorist) || 
            (IncludeT && p.TeamNum == (byte)CsTeam.Terrorist) || 
            (IncludeSPEC && p.TeamNum == (byte)CsTeam.Spectator))
        );
    }

    public static bool IsPlayerInGroupPermission(CCSPlayerController player, string groups)
    {
        if (string.IsNullOrEmpty(groups))
        {
            return false;
        }
        var Groups = groups.Split(',');
        foreach (var group in Groups)
        {
            if (string.IsNullOrEmpty(group))
            {
                continue;
            }
            string groupId = group[0] == '!' ? group.Substring(1) : group;
            if (group[0] == '#' && AdminManager.PlayerInGroup(player, group))
            {
                return true;
            }
            else if (group[0] == '@' && AdminManager.PlayerHasPermissions(player, group))
            {
                return true;
            }
            else if (group[0] == '!' && player.AuthorizedSteamID != null && (groupId == player.AuthorizedSteamID.SteamId2.ToString() || groupId == player.AuthorizedSteamID.SteamId3.ToString() ||
            groupId == player.AuthorizedSteamID.SteamId32.ToString() || groupId == player.AuthorizedSteamID.SteamId64.ToString()))
            {
                return true;
            }
            else if (AdminManager.PlayerInGroup(player, group))
            {
                return true;
            }
        }
        return false;
    }

    public static void LoadJson()
    {
        if(!Configs.GetConfigData().Custom_ChatMessages)return;
        
        try
        {
            var g_Main = GameManagerGoldKingZ.Instance.g_Main;
            string jsonFilePath = Path.Combine(GameManagerGoldKingZ.Instance.ModuleDirectory, $"config/chat_processor.json");
            if (!File.Exists(jsonFilePath))
            {
                DebugMessage($"{jsonFilePath} file does not exist.");
                return;
            }

            string jsonContent = File.ReadAllText(jsonFilePath);
            if (string.IsNullOrEmpty(jsonContent))
            {
                DebugMessage($"{jsonFilePath} content is empty.");
                return;
            }

            JObject jsonObject = JObject.Parse(jsonContent);
            if (jsonObject == null) return;

            g_Main.JsonData = jsonObject;

            DebugMessage($"{jsonFilePath} Loaded Successfully");
        }
        catch (JsonReaderException ex)
        {
            DebugMessage($"Error reading JSON from file: {ex.Message}");
        }
    }

    
    public static void SendGrenadeMessage(string nade, CCSPlayerController players, string playerName, string nadelocation)
    {
        var messages = new Dictionary<string, string> {
            {"hegrenade", "custom.hegrenade"},
            {"smokegrenade", "custom.smokegrenade"},
            {"flashbang", "custom.flashbang"},
            {"molotov", "custom.molotov"},
            {"incgrenade", "custom.incgrenade"},
            {"decoy", "custom.decoy"}
        };

        if (messages.ContainsKey(nade)) {
            if(Configs.GetConfigData().Custom_ThrowNadeMessages == 4 && ConVar.Find("mp_teammates_are_enemies")!.GetPrimitiveValue<bool>())
            {
                AdvancedServerPrintToChatAll(GameManagerGoldKingZ.Instance.Localizer[messages[nade]], playerName, nadelocation);
                return;
            }
            AdvancedPlayerPrintToChat(players, GameManagerGoldKingZ.Instance.Localizer[messages[nade]], playerName, nadelocation);
        }
    }
    public static void ClearVariables()
    {
        var g_Main = GameManagerGoldKingZ.Instance.g_Main;

        foreach(var Player_Data in g_Main.Player_Data.Values)
        {
            if(Player_Data == null)continue;

            if(Player_Data.Timer_DeadBody != null)
            {
                Player_Data.Timer_DeadBody.Kill();
                Player_Data.Timer_DeadBody = null!;
            }
        }

        g_Main.Player_Data.Clear();
        g_Main.CbaseWeapons.Clear();
        g_Main.JsonData = null;

        LoadJson();
    }
    
    public static void AddPlayerToGlobal(CCSPlayerController player)
    {
        if(!player.IsValid(false))return;

        if (!GameManagerGoldKingZ.Instance.g_Main.Player_Data.ContainsKey(player))
        {
            GameManagerGoldKingZ.Instance.g_Main.Player_Data.Add(player, new Globals.PlayerDataClass(player, "", 255, false, false, null!));
        }
    }
    
    private static CCSGameRules? GetGameRules()
    {
        try
        {
            var gameRulesEntities = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules");
            return gameRulesEntities.First().GameRules;
        }
        catch (Exception ex)
        {
            DebugMessage(ex.Message);
            return null;
        }
    }
    public static bool IsWarmup()
    {
        return GetGameRules()?.WarmupPeriod ?? false;
    }
    public static void ExectueCommands()
    {
        if(Configs.GetConfigData().Sounds_MutePlayersFootSteps)
        {
            Server.ExecuteCommand("sv_footsteps 0");
        }

        if(Configs.GetConfigData().DisableRadar)
        {
            Server.ExecuteCommand("sv_disable_radar 1");
        }

        if(Configs.GetConfigData().DisableBotRadio)
        {
            Server.ExecuteCommand("bot_chatter off");
        }

        if(Configs.GetConfigData().DisableGrenadeRadio)
        {
            Server.ExecuteCommand("sv_ignoregrenaderadio 1");
        }

        if(Configs.GetConfigData().DisableTeamMateHeadTag == 1)
        {
            Server.ExecuteCommand("sv_teamid_overhead 0");
        }else if(Configs.GetConfigData().DisableTeamMateHeadTag == 2)
        {
            Server.ExecuteCommand("sv_teamid_overhead 1; sv_teamid_overhead_always_prohibit 1; sv_teamid_overhead_maxdist 0");
        }else if(Configs.GetConfigData().DisableTeamMateHeadTag == 3 && Configs.GetConfigData().DisableTeamMateHeadTag_Distance > 0)
        {
            Server.ExecuteCommand($"sv_teamid_overhead 1; sv_teamid_overhead_always_prohibit 1; sv_teamid_overhead_maxdist {Configs.GetConfigData().DisableTeamMateHeadTag_Distance}");
        }

        if(Configs.GetConfigData().Sounds_MuteJumpLand)
        {
            Server.ExecuteCommand("sv_min_jump_landing_sound 999999");
        }

        if(Configs.GetConfigData().DisableFallDamage)
        {
            Server.ExecuteCommand("sv_falldamage_scale 0");
        }

        if(Configs.GetConfigData().DisableSvCheats_1)
        {
            Server.ExecuteCommand("sv_cheats 0");
        }

        if(Configs.GetConfigData().DisableC4)
        {
            Server.ExecuteCommand("mp_give_player_c4 0");
        }
    }
    public static void DebugMessage(string message, bool prefix = true)
    {
        if (!Configs.GetConfigData().EnableDebug) return;

        Console.ForegroundColor = ConsoleColor.Magenta;
        string output = prefix ? $"[Game Manager]: {message}" : message;
        Console.WriteLine(output);
        
        Console.ResetColor();
    }


    public static void HideDeadBody(CCSPlayerController player)
    {
        if(!player.IsValid(false)
        || player.PlayerPawn == null
        || !player.PlayerPawn.IsValid
        || player.PlayerPawn.Value == null
        || !player.PlayerPawn.Value.IsValid
        || player.IsAlive() && !player.ControllingBot)return;

        AddPlayerToGlobal(player);

        var orginalmodel = player.PlayerPawn.Value.CBodyComponent?.SceneNode?.GetSkeletonInstance()?.ModelState.ModelName ?? string.Empty;
        if (!string.IsNullOrEmpty(orginalmodel ))
        {
            player.PlayerPawn.Value.SetModel("characters/models/tm_jumpsuit/tm_jumpsuit_varianta.vmdl");
            player.PlayerPawn.Value.SetModel(orginalmodel);
        }
        
        if(Configs.GetConfigData().HideDeadBody == 1)
        {
            player.PlayerRender(0);
        }else if(Configs.GetConfigData().HideDeadBody == 2)
        {
            GameManagerGoldKingZ.Instance.AddTimer(Configs.GetConfigData().HideDeadBody_Delay, () =>
            {
                if(!player.IsValid(false) || player.IsAlive() && !player.ControllingBot)return;
                player.PlayerRender(0);
            }, TimerFlags.STOP_ON_MAPCHANGE);
        }else if(Configs.GetConfigData().HideDeadBody == 3)
        {
            
            if (GameManagerGoldKingZ.Instance.g_Main.Player_Data.ContainsKey(player))
            {
                GameManagerGoldKingZ.Instance.g_Main.Player_Data[player].PlayerAlpha = 255;
                GameManagerGoldKingZ.Instance.g_Main.Player_Data[player].Timer_DeadBody = GameManagerGoldKingZ.Instance.AddTimer(0.01f, () =>{
                StartDecay(player);
                }, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
            }
        }
    }

    public static void StartDecay(CCSPlayerController player)
    {
        if(!player.IsValid(false) || !GameManagerGoldKingZ.Instance.g_Main.Player_Data.ContainsKey(player))return;

        if(player.IsAlive() && !player.ControllingBot || GameManagerGoldKingZ.Instance.g_Main.Player_Data[player].PlayerAlpha <= 0)
        {
            GameManagerGoldKingZ.Instance.g_Main.Player_Data[player].Timer_DeadBody?.Kill();
            GameManagerGoldKingZ.Instance.g_Main.Player_Data[player].Timer_DeadBody = null!;
            GameManagerGoldKingZ.Instance.g_Main.Player_Data[player].PlayerAlpha = 255;
            return;
        }

        var devayplayer = GameManagerGoldKingZ.Instance.g_Main.Player_Data[player].PlayerAlpha--;
        player.PlayerRender(devayplayer);
    }

    public static void HideLegs(CCSPlayerController player)
    {
        if(!player.IsValid(false))return;
        player.PlayerRender(254);
    }
    public static void HideWeaponsHUD(CCSPlayerController player)
    {
        if(!player.IsValid(false))return;
        player.PlayerHideHUD(Globals_Static.HIDEWEAPONS, true);
    }

    public static void HideChatHUD(CCSPlayerController player)
    {
        if(!player.IsValid(false))return;
        if(Configs.GetConfigData().HideChatHUD == 1)
        {
            player.PlayerHideHUD(Globals_Static.HIDECHAT, true);
        }else if(Configs.GetConfigData().HideChatHUD == 2 && Configs.GetConfigData().HideChatHUD_Delay > 0)
        {
            AdvancedPlayerPrintToChat(player, GameManagerGoldKingZ.Instance.Localizer["hidechat.enabled.warning"], Configs.GetConfigData().HideChatHUD_Delay);
            
            GameManagerGoldKingZ.Instance.AddTimer(Configs.GetConfigData().HideChatHUD_Delay, () =>
            {
                if(!player.IsValid(false))return;
                player.PlayerHideHUD(Globals_Static.HIDECHAT, true);
            }, TimerFlags.STOP_ON_MAPCHANGE);
        }
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
    public static async Task DownloadMissingFiles()
    {
        try
        {
            string baseFolderPath = GameManagerGoldKingZ.Instance.ModuleDirectory;

            string settingsFileName = "config/chat_processor.json";
            string settingsGithubUrl = "https://raw.githubusercontent.com/oqyh/cs2-Game-Manager-GoldKingZ/main/Resources/chat_processor.json";
            string settingsFilePath = Path.Combine(baseFolderPath, settingsFileName);
            string settingsDirectoryPath = Path.GetDirectoryName(settingsFilePath)!;
            await DownloadFileIfNotExists(settingsFilePath, settingsGithubUrl, settingsDirectoryPath);
        }
        catch (Exception ex)
        {
            DebugMessage($"Error in DownloadMissingFiles: {ex.Message}");
        }
    }

    public static async Task DownloadFileIfNotExists(string filePath, string githubUrl, string directoryPath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                await DownloadFileFromGithub(githubUrl, filePath);
            }
        }
        catch (Exception ex)
        {
            DebugMessage($"Error in DownloadFileIfNotExists: {ex.Message}");
        }
    }

    public static async Task DownloadFileFromGithub(string url, string destinationPath)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                byte[] fileBytes = await client.GetByteArrayAsync(url);
                await File.WriteAllBytesAsync(destinationPath, fileBytes);
            }
            catch (Exception ex)
            {
                Helper.DebugMessage($"Error downloading file: {ex.Message}");
            }
        }
    }
}