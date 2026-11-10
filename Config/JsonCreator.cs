namespace Game_Manager_GoldKingZ;

public class JsonCreator
{

  private static readonly Dictionary<string, string> DefaultResources = new()
  {
    {
      Path.Combine(MainPlugin.Instance.ModuleDirectory, "config", "chat_processor.json"),
@"	//==========================
	//        Configuration
	//==========================
	//{ClanTag_ScoreBoard} = Clan Tag At ScoreBoard
	//{ClanTag_Chat} = Clan Tag At Chat
	//{CT_ALIVE_ALL} = CT Alive To All Message
	//{CT_ALIVE_TEAM} = CT Alive To TeamSide Message
	//{CT_DEAD_ALL} = CT Dead To All Message
	//{CT_DEAD_TEAM} = CT Dead To TeamSide Message
	//{T_ALIVE_ALL} = T Alive To All Message
	//{T_ALIVE_TEAM} = T Alive To TeamSide Message
	//{T_DEAD_ALL} = T Dead To All Message
	//{T_DEAD_TEAM} = T Dead To TeamSide Message
	//{SPEC_ALL} = Spectator To All Message
	//{SPEC_TEAM} = Spectator To TeamSide Message
	//{BotTakeOver} = Bot Take Over Message
	//{JoinTeam_SPEC} = Join Spectators Message
	//{JoinTeam_CT} = Join Counter-Terrorists Message
	//{JoinTeam_T} = Join Terrorists Message
	//{Nade_Hegrenade} = HE Grenade Throw Message
	//{Nade_Smokegrenade} = Smoke Grenade Throw Message
	//{Nade_Molotov} = Molotov Throw Message
	//{Nade_Incgrenade} = Incendiary Grenade Throw Message
	//{Nade_Flashbang} = Flashbang Throw Message
	//{Nade_Decoy} = Decoy Grenade Throw Message
	//==========================
	//        Placeholders
	//==========================
	//{PLAYER_NAME} = Player Name
	//{BOT_NAME} = BOT Controlled Name
	//{ClanTag_ScoreBoard} = Clan Tag At ScoreBoard
	//{ClanTag_Chat} = Clan Tag At Chat
	//{PLAYER_LOCATION} = Player Location
	//{PLAYER_MSG} = Player Message
	//==========================
	//        Colors
	//==========================
	//{Default} {White} {Darkred} {Green} {LightYellow} {LightBlue} 
	//{Olive} {Lime} {Red} {LightPurple} {Purple} {Grey} {Yellow} 
	//{Gold} {Silver} {Blue} {DarkBlue} {BlueGrey} {Magenta} {LightRed} {Orange}
	//{team_color} = Dynamic Change Color Depend Team (Spec= LightPurple/T= Orange/CT= LightBlue)
	//==========================

{
  ""ANY"": {
    ""ClanTag_ScoreBoard"": ""[GoldKingZ]"",
    ""ClanTag_Chat"": ""{grey}GKz | {LightYellow}"",
    ""CT_ALIVE_ALL"": ""{LightYellow}[ALL] {darkblue}{ClanTag_Chat} {team_color}{PLAYER_NAME}{white}: {grey}{PLAYER_MSG}"",
    ""CT_ALIVE_TEAM"": ""{LightYellow}[CT] {darkblue}{ClanTag_Chat} {team_color}{PLAYER_NAME} {green}@{PLAYER_LOCATION}{white}: {grey}{PLAYER_MSG}"",
    ""CT_DEAD_ALL"": ""{LightYellow}[ALL] {darkblue}{ClanTag_Chat} {team_color}{PLAYER_NAME} {grey}[☠︎]{white}: {grey}{PLAYER_MSG}"",
    ""CT_DEAD_TEAM"": ""{LightYellow}[CT] {darkblue}{ClanTag_Chat} {team_color}{PLAYER_NAME} {grey}[☠︎]{white}: {grey}{PLAYER_MSG}"",
    ""T_ALIVE_ALL"": ""{LightYellow}[ALL] {orange}{ClanTag_Chat} {team_color}{PLAYER_NAME}{white}: {grey}{PLAYER_MSG}"",
    ""T_ALIVE_TEAM"": ""{LightYellow}[T] {orange}{ClanTag_Chat} {team_color}{PLAYER_NAME} {green}@{PLAYER_LOCATION}{white}: {grey}{PLAYER_MSG}"",
    ""T_DEAD_ALL"": ""{LightYellow}[ALL] {orange}{ClanTag_Chat} {team_color}{PLAYER_NAME} {grey}[☠︎]{white}: {grey}{PLAYER_MSG}"",
    ""T_DEAD_TEAM"": ""{LightYellow}[T] {orange}{ClanTag_Chat} {team_color}{PLAYER_NAME} {grey}[☠︎]{white}: {grey}{PLAYER_MSG}"",
    ""SPEC_ALL"": ""{LightYellow}[ALL] {Olive}{ClanTag_Chat} {team_color}{PLAYER_NAME}{white}: {grey}{PLAYER_MSG}"",
    ""SPEC_TEAM"": ""{LightYellow}[SPEC] {Olive}{ClanTag_Chat} {team_color}{PLAYER_NAME}{white}: {grey}{PLAYER_MSG}"",
    ""BotTakeOver"": ""{darkblue}{ClanTag_Chat} {team_color}{PLAYER_NAME} {grey}Is Controlling {LightBlue}[BOT] {BOT_NAME}"",
    ""JoinTeam_SPEC"": ""{purple}{ClanTag_Chat} {team_color}{PLAYER_NAME} {grey}is joining the {Silver}Spectators"",
    ""JoinTeam_CT"": ""{purple}{ClanTag_Chat} {team_color}{PLAYER_NAME} {grey}is joining the {blue}Counter-Terrorists"",
    ""JoinTeam_T"": ""{purple}{ClanTag_Chat} {team_color}{PLAYER_NAME} {grey}is joining the {darkred}Terrorists"",
    ""Nade_Hegrenade"": ""{purple}{ClanTag_Chat} {team_color}{PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {red}☄ HE Grenade! ☄"",
    ""Nade_Smokegrenade"": ""{purple}{ClanTag_Chat} {team_color}{PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {Olive}☁︎ Smoke! ☁︎"",
    ""Nade_Molotov"": ""{purple}{ClanTag_Chat} {team_color}{PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {orange}♨ Molotov! ♨"",
    ""Nade_Incgrenade"": ""{purple}{ClanTag_Chat} {team_color}{PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {orange} ♨ Incendiary! ♨"",
    ""Nade_Flashbang"": ""{purple}{ClanTag_Chat} {team_color}{PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {Blue}˗ˏˋ★ Flashbang! ★ˎˊ˗"",
    ""Nade_Decoy"": ""{purple}{ClanTag_Chat} {team_color}{PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {grey}✦ Decoy! ✦""
  },
  ""SteamIDs:  | Flags: @css/admins,@css/admin,admins | Groups: #css/admins,#css/admin,admins"": {
    ""ClanTag_ScoreBoard"": ""[ADMIN]"",
    ""ClanTag_Chat"": ""{Magenta}[ADMIN] {LightYellow}"",
    ""CT_ALIVE_ALL"": ""{LightYellow}[ALL] {darkblue}{ClanTag_Chat} {PLAYER_NAME}{white}: {grey}{PLAYER_MSG}"",
    ""CT_ALIVE_TEAM"": ""{LightYellow}[CT] {darkblue}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{white}: {grey}{PLAYER_MSG}"",
    ""CT_DEAD_ALL"": ""{LightYellow}[ALL] {darkblue}{ClanTag_Chat} {PLAYER_NAME} {grey}[☠︎]{white}: {grey}{PLAYER_MSG}"",
    ""CT_DEAD_TEAM"": ""{LightYellow}[CT] {darkblue}{ClanTag_Chat} {PLAYER_NAME} {grey}[☠︎]{white}: {grey}{PLAYER_MSG}"",
    ""T_ALIVE_ALL"": ""{LightYellow}[ALL] {orange}{ClanTag_Chat} {PLAYER_NAME}{white}: {grey}{PLAYER_MSG}"",
    ""T_ALIVE_TEAM"": ""{LightYellow}[T] {orange}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{white}: {grey}{PLAYER_MSG}"",
    ""T_DEAD_ALL"": ""{LightYellow}[ALL] {orange}{ClanTag_Chat} {PLAYER_NAME} {grey}[☠︎]{white}: {grey}{PLAYER_MSG}"",
    ""T_DEAD_TEAM"": ""{LightYellow}[T] {orange}{ClanTag_Chat} {PLAYER_NAME} {grey}[☠︎]{white}: {grey}{PLAYER_MSG}"",
    ""SPEC_ALL"": ""{LightYellow}[ALL] {Olive}{ClanTag_Chat} {PLAYER_NAME}{white}: {grey}{PLAYER_MSG}"",
    ""SPEC_TEAM"": ""{LightYellow}[SPEC] {Olive}{ClanTag_Chat} {PLAYER_NAME}{white}: {grey}{PLAYER_MSG}"",
    ""BotTakeOver"": ""{darkblue}{ClanTag_Chat} {PLAYER_NAME} {grey}Is Controlling {LightBlue}[BOT] {BOT_NAME}"",
    ""JoinTeam_SPEC"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {grey}is joining the {Silver}Spectators"",
    ""JoinTeam_CT"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {grey}is joining the {blue}Counter-Terrorists"",
    ""JoinTeam_T"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {grey}is joining the {darkred}Terrorists"",
    ""Nade_Hegrenade"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {red}☄ HE Grenade! ☄"",
    ""Nade_Smokegrenade"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {Olive}☁︎ Smoke! ☁︎"",
    ""Nade_Molotov"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {orange}♨ Molotov! ♨"",
    ""Nade_Incgrenade"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {orange} ♨ Incendiary! ♨"",
    ""Nade_Flashbang"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {Blue}˗ˏˋ★ Flashbang! ★ˎˊ˗"",
    ""Nade_Decoy"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {grey}✦ Decoy! ✦""
  },
  ""SteamIDs: 76561198206086993,STEAM_0:1:507335558 | Flags:  | Groups: "": {
    ""ClanTag_ScoreBoard"": ""[OWNER]"",
    ""ClanTag_Chat"": ""{red}[OWNER] {LightYellow}"",
    ""CT_ALIVE_ALL"": ""{LightYellow}[ALL] {darkblue}{ClanTag_Chat} {PLAYER_NAME}{white}: {grey}{PLAYER_MSG}"",
    ""CT_ALIVE_TEAM"": ""{LightYellow}[CT] {darkblue}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{white}: {grey}{PLAYER_MSG}"",
    ""CT_DEAD_ALL"": ""{LightYellow}[ALL] {darkblue}{ClanTag_Chat} {PLAYER_NAME} {grey}[☠︎]{white}: {grey}{PLAYER_MSG}"",
    ""CT_DEAD_TEAM"": ""{LightYellow}[CT] {darkblue}{ClanTag_Chat} {PLAYER_NAME} {grey}[☠︎]{white}: {grey}{PLAYER_MSG}"",
    ""T_ALIVE_ALL"": ""{LightYellow}[ALL] {orange}{ClanTag_Chat} {PLAYER_NAME}{white}: {grey}{PLAYER_MSG}"",
    ""T_ALIVE_TEAM"": ""{LightYellow}[T] {orange}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{white}: {grey}{PLAYER_MSG}"",
    ""T_DEAD_ALL"": ""{LightYellow}[ALL] {orange}{ClanTag_Chat} {PLAYER_NAME} {grey}[☠︎]{white}: {grey}{PLAYER_MSG}"",
    ""T_DEAD_TEAM"": ""{LightYellow}[T] {orange}{ClanTag_Chat} {PLAYER_NAME} {grey}[☠︎]{white}: {grey}{PLAYER_MSG}"",
    ""SPEC_ALL"": ""{LightYellow}[ALL] {Olive}{ClanTag_Chat} {PLAYER_NAME}{white}: {grey}{PLAYER_MSG}"",
    ""SPEC_TEAM"": ""{LightYellow}[SPEC] {Olive}{ClanTag_Chat} {PLAYER_NAME}{white}: {grey}{PLAYER_MSG}"",
    ""BotTakeOver"": ""{darkblue}{ClanTag_Chat} {PLAYER_NAME} {grey}Is Controlling {LightBlue}[BOT] {BOT_NAME}"",
    ""JoinTeam_SPEC"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {grey}is joining the {Silver}Spectators"",
    ""JoinTeam_CT"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {grey}is joining the {blue}Counter-Terrorists"",
    ""JoinTeam_T"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {grey}is joining the {darkred}Terrorists"",
    ""Nade_Hegrenade"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {red}☄ HE Grenade! ☄"",
    ""Nade_Smokegrenade"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {Olive}☁︎ Smoke! ☁︎"",
    ""Nade_Molotov"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {orange}♨ Molotov! ♨"",
    ""Nade_Incgrenade"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {orange} ♨ Incendiary! ♨"",
    ""Nade_Flashbang"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {Blue}˗ˏˋ★ Flashbang! ★ˎˊ˗"",
    ""Nade_Decoy"": ""{purple}{ClanTag_Chat} {PLAYER_NAME} {green}@{PLAYER_LOCATION}{grey}: {grey}Threw {grey}✦ Decoy! ✦""
  }
}"

    },
    {
      Path.Combine(MainPlugin.Instance.ModuleDirectory, "config", "weapons_config.json"),
@"	//==========================
	//        Configuration
	//==========================
	// ""Clip"": 50,           // Magazine Size || Not Used = Default Weapon Magazine
	// ""Ammo"": 200,          // Reserve Ammo || Not Used = New Reload Clips Behavior
	// ""UnlimitedClip"": true, // Magazine Never Runs Out, So The Player Never Reloads? || true = Yes || false OR Not Used = No
	// ""UnlimitedAmmo"": true, // Reserve Ammo Never Decreases, But The Player Still Reloads Normally? || true = Yes || false OR Not Used = No
	// ""UseOldReload"": true,   // Use Old Reload Behavior (Bullets Left In Clip Are Wasted)? || true = Yes || false OR Not Used = No
	//==========================
	//        Weapon Names
	//==========================
	//Snipers       = weapon_awp, weapon_g3sg1, weapon_scar20, weapon_ssg08
	//Rifles        = weapon_ak47, weapon_aug, weapon_famas, weapon_galilar, weapon_m4a1_silencer, weapon_m4a1, weapon_sg556
	//Machine Guns  = weapon_m249, weapon_negev
	//Shotguns      = weapon_mag7, weapon_nova, weapon_sawedoff, weapon_xm1014
	//SMGs          = weapon_bizon, weapon_mac10, weapon_mp5sd, weapon_mp7, weapon_mp9, weapon_p90, weapon_ump45
	//Pistols       = weapon_cz75a, weapon_deagle, weapon_elite, weapon_fiveseven, weapon_glock, weapon_hkp2000, weapon_p250, weapon_revolver, weapon_tec9, weapon_usp_silencer
	//==========================
	//        Note
	//==========================
	//- The New Reload Clips Behavior Will Only Apply To Weapons Not Listed Here
	//==========================

{
  ""ANY"": {
    ""weapon_ak47"": {
      ""Clip"": 30,
      ""Ammo"": 90,
      ""UseOldReload"": true
    },
    ""weapon_m4a1"": {
      ""Clip"": 30,
      ""Ammo"": 90,
      ""UseOldReload"": true
    },
    ""weapon_bizon"": {
      ""Clip"": 64,
      ""Ammo"": 120,
      ""UseOldReload"": true
    },
    ""weapon_usp_silencer"": {
      ""Ammo"": 4
    }
  },
  ""SteamIDs:  | Flags: @css/admins,@css/admin,admins | Groups: #css/admins,#css/admin,admins"": {
    ""weapon_bizon"": {
      ""Clip"": 64,
      ""Ammo"": 120,
      ""UseOldReload"": true,
      ""UnlimitedAmmo"": true
    },
    ""weapon_awp"": {
      ""Clip"": 10
    },
    ""weapon_deagle"": {
      ""Clip"": 10,
      ""UnlimitedAmmo"": true
    },
    ""weapon_usp_silencer"": {
      ""Clip"": 21,
      ""Ammo"": 4
    }
  },
  ""SteamIDs: 76561198206086993,STEAM_0:1:507335558 | Flags:  | Groups: "": {
    ""weapon_ak47"": {
      ""Clip"": 999,
      ""Ammo"": 999,
      ""UnlimitedClip"": true
    },
    ""weapon_deagle"": {
      ""Clip"": 999,
      ""UnlimitedClip"": true,
      ""UnlimitedAmmo"": true
    }
  }
}"
    }
  };

    public static void StartCreate()
    {
        foreach (var (filePath, defaultJson) in DefaultResources)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, defaultJson);
                }
            }
            catch
            {
                // silently skip
            }
        }
    }
}