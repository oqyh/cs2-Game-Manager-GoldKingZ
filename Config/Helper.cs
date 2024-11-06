using System.Text.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Game_Manager_GoldKingZ.Config;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Cvars;

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
    public static readonly string[] WeaponsList =
    {
        "weapon_awp",
        "weapon_g3sg1",
        "weapon_scar20", //Sniper Rifles
        "weapon_ssg08",

        "weapon_ak47",
        "weapon_aug",
        "weapon_famas",
        "weapon_galilar",
        "weapon_m4a1_silencer", //Assault Rifles
        "weapon_m4a1",
        "weapon_sg556",

        "weapon_m249",
        "weapon_negev",  //Machine Guns

        "weapon_mag7",  
        "weapon_nova",
        "weapon_sawedoff", //Shotguns
        "weapon_xm1014",

        "weapon_bizon",
        "weapon_mac10",
        "weapon_mp5sd",
        "weapon_mp7",  //Sub-Machine Guns
        "weapon_mp9",
        "weapon_p90",
        "weapon_ump45",

        "weapon_cz75a",
        "weapon_deagle",
        "weapon_elite",
        "weapon_fiveseven",
        "weapon_glock",
        "weapon_hkp2000",  //Pistols
        "weapon_p250",
        "weapon_revolver",
        "weapon_tec9",
        "weapon_usp_silencer",

        "weapon_smokegrenade",
        "weapon_hegrenade",
        "weapon_flashbang", //Grenades
        "weapon_decoy",
        "weapon_molotov",
        "weapon_incgrenade",

        "item_defuser",
        "item_cutters", //Defuse Kits

        "weapon_taser", //Taser

        "weapon_healthshot", //healthshot

        "weapon_knife",
        "weapon_knife_t" //Knifes
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
    public static void AdvancedServerPrintToChatAll(string message, params object[] args)
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
                Server.PrintToChatAll(" " + messages);
            }
        }else
        {
            Server.PrintToChatAll(message);
        }
    }
    public static void AdvancedPlayerPrintToConsole(CCSPlayerController player, string message, params object[] args)
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
                player.PrintToConsole(" " + messages);
            }
        }else
        {
            player.PrintToConsole(message);
        }
    }
    
    public static bool IsPlayerInGroupPermission(CCSPlayerController player, string groups)
    {
        var excludedGroups = groups.Split(',');
        foreach (var group in excludedGroups)
        {
            switch (group[0])
            {
                case '#':
                    if (AdminManager.PlayerInGroup(player, group))
                        return true;
                    break;

                case '@':
                    if (AdminManager.PlayerHasPermissions(player, group))
                        return true;
                    break;

                default:
                    return false;
            }
        }
        return false;
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
            if(Configs.GetConfigData().CustomThrowNadeMessagesMode == 4 && ConVar.Find("mp_teammates_are_enemies")!.GetPrimitiveValue<bool>())
            {
                Helper.AdvancedServerPrintToChatAll(Configs.Shared.StringLocalizer![messages[nade]], playerName, nadelocation);
                return;
            }
            Helper.AdvancedPlayerPrintToChat(players, Configs.Shared.StringLocalizer![messages[nade]], playerName, nadelocation);
        }
    }
    public static void ClearVariables()
    {
        Globals.Toggle_DisableLegs.Clear();
        Globals.Toggle_DisableChat.Clear();
        Globals.Toggle_OnDisableChat.Clear();
        Globals.Toggle_DisableWeapons.Clear();
        Globals.Toggle_OnDisableWeapons.Clear();
        Globals.CbaseWeapons.Clear();
        Globals.Remove_Icon.Clear();
        Globals.StabedHisTeamMate.Clear();

        foreach (var timer in Globals.TimerRemoveDeadBody.Values)
        {
            timer?.Kill();
        }
        Globals.TimerRemoveDeadBody.Clear();

        Globals.PlayerAlpha.Clear();
    }
    public static string RemoveLeadingSpaces(string content)
    {
        string[] lines = content.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].TrimStart();
        }
        return string.Join("\n", lines);
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
            Server.ExecuteCommand("sv_teamid_overhead 1; sv_teamid_overhead_always_prohibit 1");
        }

        if(Configs.GetConfigData().DisableTeamMateHeadTag == 2)
        {
            Server.ExecuteCommand("sv_teamid_overhead 0");
        }

        if(Configs.GetConfigData().Sounds_MuteJumpLand)
        {
            Server.ExecuteCommand("sv_min_jump_landing_sound 999999");
        }

        if(Configs.GetConfigData().DisableFallDamage)
        {
            Server.ExecuteCommand("sv_falldamage_scale 0");
        }

        if(Configs.GetConfigData().DisableSvCheats)
        {
            Server.ExecuteCommand("sv_cheats 0");
        }

        if(Configs.GetConfigData().DisableC4)
        {
            Server.ExecuteCommand("mp_give_player_c4 0");
        }
    }
    public static void CreateDefaultWeaponsJson2(string jsonFilePath)
    {
        if (!File.Exists(jsonFilePath))
        {
            var configData = new Dictionary<string, object>
            {
                {"MySqlHost", "your_mysql_host"},
                {"MySqlDatabase", "your_mysql_database"},
                {"MySqlUsername", "your_mysql_username"},
                {"MySqlPassword", "your_mysql_password"},
                {"MySqlPort", 3306}
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = System.Text.Json.JsonSerializer.Serialize(configData, options);

            File.WriteAllText(jsonFilePath, json);
        }
    }
    public static void DebugMessage(string message)
    {
        if(!Configs.GetConfigData().EnableDebug)return;
        Console.WriteLine($"================================= [ Debug Game Manager  ] =================================");
        Console.WriteLine(message);
        Console.WriteLine("=========================================================================================");
    }
    
    public class PersonData
    {
        public ulong PlayerSteamID { get; set; }
        public int Disable_Chat { get; set; }
        public int Disable_Legs { get; set; }
        public int Disable_Weapons { get; set; }
        public DateTime DateAndTime { get; set; }
    }
    public static void SaveToJsonFile(ulong PlayerSteamID, int Disable_Chat, int Disable_Legs, int Disable_Weapons , DateTime DateAndTime)
    {
        string Fpath = Path.Combine(Configs.Shared.CookiesModule!, "../../plugins/Game-Manager-GoldKingZ/Cookies/");
        string Fpathc = Path.Combine(Configs.Shared.CookiesModule!, "../../plugins/Game-Manager-GoldKingZ/Cookies/Game_Manager_Cookies.json");
        try
        {
            if (!Directory.Exists(Fpath))
            {
                Directory.CreateDirectory(Fpath);
            }

            if (!File.Exists(Fpathc))
            {
                File.WriteAllText(Fpathc, "[]");
            }

            List<PersonData> allPersonsData;
            string jsonData = File.ReadAllText(Fpathc);
            allPersonsData = JsonConvert.DeserializeObject<List<PersonData>>(jsonData) ?? new List<PersonData>();

            
            
            PersonData existingPerson = allPersonsData.Find(p => p.PlayerSteamID == PlayerSteamID)!;

            if (existingPerson != null)
            {
                existingPerson.Disable_Chat = Disable_Chat;
                existingPerson.Disable_Legs = Disable_Legs;
                existingPerson.Disable_Weapons = Disable_Weapons;
                existingPerson.DateAndTime = DateAndTime;
            }
            else
            {
                PersonData newPerson = new PersonData
                {
                    PlayerSteamID = PlayerSteamID,
                    Disable_Chat = Disable_Chat,
                    Disable_Legs = Disable_Legs,
                    Disable_Weapons = Disable_Weapons,
                    DateAndTime = DateAndTime
                };
                allPersonsData.Add(newPerson);
            }
            

            allPersonsData.RemoveAll(p => (DateTime.Now - p.DateAndTime).TotalDays > Configs.GetConfigData().Toggle_AutoRemovePlayerCookieOlderThanXDays);

            string updatedJsonData = JsonConvert.SerializeObject(allPersonsData, Formatting.Indented);
            try
            {
                File.WriteAllText(Fpathc, updatedJsonData);
            }
            catch (Exception ex)
            {
                DebugMessage(ex.Message);
            }
        }
        catch (Exception ex)
        {
            DebugMessage(ex.Message);
        }
    }

    public static PersonData RetrievePersonDataById(ulong targetId)
    {
        string Fpath = Path.Combine(Configs.Shared.CookiesModule!, "../../plugins/Game-Manager-GoldKingZ/Cookies/");
        string Fpathc = Path.Combine(Configs.Shared.CookiesModule!, "../../plugins/Game-Manager-GoldKingZ/Cookies/Game_Manager_Cookies.json");
        try
        {
            if (Directory.Exists(Fpath) && File.Exists(Fpathc))
            {
                string jsonData = File.ReadAllText(Fpathc);
                List<PersonData> allPersonsData = JsonConvert.DeserializeObject<List<PersonData>>(jsonData) ?? new List<PersonData>();

                PersonData targetPerson = allPersonsData.Find(p => p.PlayerSteamID == targetId)!;

               
                if (targetPerson != null && (DateTime.Now - targetPerson.DateAndTime<= TimeSpan.FromDays(Configs.GetConfigData().Toggle_AutoRemovePlayerCookieOlderThanXDays)))
                {
                    return targetPerson;
                }
                else if (targetPerson != null)
                {
                    allPersonsData.Remove(targetPerson);
                    string updatedJsonData = JsonConvert.SerializeObject(allPersonsData, Formatting.Indented);
                    try
                    {
                        File.WriteAllText(Fpathc, updatedJsonData);
                    }
                    catch (Exception ex)
                    {
                        DebugMessage(ex.Message);
                    }
                }
                
                
            }
        }
        catch (Exception ex)
        {
            DebugMessage(ex.Message);
        }
        return new PersonData();
    }
    public static void FetchAndRemoveOldJsonEntries()
    {
        string Fpath = Path.Combine(Configs.Shared.CookiesModule!, "../../plugins/Game-Manager-GoldKingZ/Cookies/");
        string Fpathc = Path.Combine(Configs.Shared.CookiesModule!, "../../plugins/Game-Manager-GoldKingZ/Cookies/Game_Manager_Cookies.json");
        try
        {
            if (Directory.Exists(Fpath) && File.Exists(Fpathc))
            {
                string jsonData = File.ReadAllText(Fpathc);
                List<PersonData> allPersonsData = JsonConvert.DeserializeObject<List<PersonData>>(jsonData) ?? new List<PersonData>();

                int daysToKeep = Configs.GetConfigData().Toggle_AutoRemovePlayerCookieOlderThanXDays;
                allPersonsData.RemoveAll(p => (DateTime.Now - p.DateAndTime).TotalDays > daysToKeep);

                string updatedJsonData = JsonConvert.SerializeObject(allPersonsData, Formatting.Indented);
                try
                {
                    File.WriteAllText(Fpathc, updatedJsonData);
                }
                catch (Exception ex)
                {
                    DebugMessage(ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            DebugMessage(ex.Message);
        }
    }
}