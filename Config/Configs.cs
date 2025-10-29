using System.Reflection;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Encodings.Web;

namespace Game_Manager_GoldKingZ.Config
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]public class ForceStringAttribute : Attribute{public string FallbackValue { get; }public ForceStringAttribute(string fallbackValue){FallbackValue = fallbackValue;}}
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)] public class StringAttribute : Attribute{public string[] Keys { get; }public StringAttribute(params string[] keys) => Keys = keys;}
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)] public class CommentAttribute : Attribute{public string Text;public CommentAttribute(string t) => Text = t;}
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]public class BreakLineAttribute : Attribute{public string Text;public BreakLineAttribute(string t) => Text = t;}
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]public class InfoAttribute : Attribute{public string Key { get; }public InfoAttribute(string key) => Key = key;}
    [AttributeUsage(AttributeTargets.Property)] public class RangeAttribute : Attribute { public double Min, Max, Default; public string? Message; public RangeAttribute(double min, double max, double def, string? msg = null) { Min = min; Max = max; Default = def; Message = msg; } }

    public class Reload_GameManager
    {
        [Comment("Commands To Reload Plugin")]
        [Comment("Note: Console_Commands Can Be Execute Via Both Console And Chat By (!)")]
        [Comment("Making Both Console_Commands And Chat_Commands Empty = Disable")]
        [String("Console_Commands", "Chat_Commands")]
        public string Reload_GameManager_CommandsInGame { get; set; } = "Console_Commands: css_reloadgamemanager,css_reloadgm | Chat_Commands: ";

        [Comment("If [Reload_GameManager_CommandsInGame] Pass, Is There Any Specified Restricted Flags, Groups, SteamIDs")]
        [Comment("Example:")]
        [Comment("\"SteamIDs: 76561198206086993,STEAM_0:1:507335558 | Flags: @css/root,@css/admin | Groups: #css/root,#css/admin\"")]
        [Comment("\"SteamIDs:  | Flags:  | Groups: \" = To Allow Everyone")]
        [String("SteamIDs", "Flags", "Groups")]
        public string Reload_GameManager_Flags { get; set; } = "SteamIDs: 76561198206086993,STEAM_0:1:507335558 | Flags: @css/root,@css/admin | Groups: #css/root,#css/admin";

        [Comment("If [Reload_GameManager_Flags] Pass, Hide Chat After Execute Reload_GameManager_CommandsInGame?:")]
        [Comment("0 = No")]
        [Comment("1 = Yes, But Only After Toggle Successfully")]
        [Comment("2 = Yes, Hide All The Time")]
        [Range(0, 2, 0,
        "Reload_GameManager_Hide: is invalid, setting to default value (0) Please Choose From 0 To 2.\n" +
        "0 = No\n" +
        "1 = Yes, But Only After Toggle Successfully\n" +
        "2 = Yes, Hide All The Time")]
        public int Reload_GameManager_Hide { get; set; } = 0;
    }

    public class Block_Commands
    {
        [Comment("Block Ingame Commands If It Start With")]
        public List<string> Block_Commands_StartWith { get; set; } = new List<string>
        {
            "meta",
            "meta list",
            "meta version"
        };

        [Comment("If You Using [Block_Commands_StartWith] Do We Ignore Upper/LowerCase?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool Block_Commands_StartWith_IgnoreCase { get; set; } = true;

        [Comment("Block Ingame Commands If It Contains")]
        public List<string> Block_Commands_Contains { get; set; } = new List<string>
        {
            "meta"
        };

        [Comment("If You Using [Block_Commands_Contains] Do We Ignore Upper/LowerCase?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool Block_Commands_Contains_IgnoreCase { get; set; } = true;

        [Comment("If [Block_Commands_StartWith or Block_Commands_Contains] Pass, Is There Any Specified Ignore Flags, Groups, SteamIDs")]
        [Comment("Example:")]
        [Comment("\"SteamIDs: 76561198206086993,STEAM_0:1:507335558 | Flags: @css/root,@css/admin | Groups: #css/root,#css/admin\"")]
        [Comment("\"SteamIDs:  | Flags:  | Groups: \" = To Ignore Everyone")]
        [String("SteamIDs", "Flags", "Groups")]
        public string Block_Commands_Ignore_Flags { get; set; } = "SteamIDs: 76561198206086993,STEAM_0:1:507335558 | Flags: @css/root,@css/admin | Groups: #css/root,#css/admin";
    }

    public class Disable_AimPunch
    {
        [Comment("Disable Players Screen Shake When They Get Damage?")]
        [Comment("0 = No")]
        [Comment("1 = Yes")]
        [Comment("2 = Yes, But Make It Togglable And Enabled By Default To New Players")]
        [Comment("3 = Yes, But Make It Togglable And Disabled By Default To New Players")]
        [Range(0, 3, 0,
        "DisableAimPunch: is invalid, setting to default value (0) Please Choose From 0 To 3.\n" +
        "0 = No\n" +
        "1 = Yes\n" +
        "2 = Yes, But Make It Togglable And Enabled By Default To New Players\n" +
        "3 = Yes, But Make It Togglable And Disabled By Default To New Players")]
        public int DisableAimPunch { get; set; } = 0;

        [Comment("If [DisableAimPunch = 2 or 3], Commands To Toggle")]
        [Comment("Note: Console_Commands Can Be Execute Via Both Console And Chat By (!)")]
        [Comment("Making Both Console_Commands And Chat_Commands Empty = Disable")]
        [String("Console_Commands", "Chat_Commands")]
        public string DisableAimPunch_CommandsInGame { get; set; } = "Console_Commands: css_aim,css_aimpunch | Chat_Commands: ";

        [Comment("If [DisableAimPunch_CommandsInGame] Pass, Is There Any Specified Restricted Flags, Groups, SteamIDs")]
        [Comment("Example:")]
        [Comment("\"SteamIDs: 76561198206086993,STEAM_0:1:507335558 | Flags: @css/root,@css/admin | Groups: #css/root,#css/admin\"")]
        [Comment("\"SteamIDs:  | Flags:  | Groups: \" = To Allow Everyone")]
        [String("SteamIDs", "Flags", "Groups")]
        public string DisableAimPunch_Flags { get; set; } = "SteamIDs: | Flags: | Groups:";

        [Comment("If [DisableAimPunch_Flags] Pass, Hide Chat After Execute DisableAimPunch_CommandsInGame?:")]
        [Comment("0 = No")]
        [Comment("1 = Yes, But Only After Toggle Successfully")]
        [Comment("2 = Yes, Hide All The Time")]
        [Range(0, 2, 0,
        "DisableAimPunch_Hide: is invalid, setting to default value (0) Please Choose From 0 To 2.\n" +
        "0 = No\n" +
        "1 = Yes, But Only After Toggle Successfully\n" +
        "2 = Yes, Hide All The Time")]
        public int DisableAimPunch_Hide { get; set; } = 0;
    }

    public class Custom_MuteSounds_1
    {
        [Comment("Mute Custom Mute 1 Sounds?")]
        [Comment("0 = No")]
        [Comment("1 = Yes")]
        [Comment("2 = Yes, But Make It Togglable And Enabled By Default To New Players")]
        [Comment("3 = Yes, But Make It Togglable And Disabled By Default To New Players")]
        [Range(0, 3, 0,
        "Custom_MuteSounds1: is invalid, setting to default value (0) Please Choose From 0 To 3.\n" +
        "0 = No\n" +
        "1 = Yes\n" +
        "2 = Yes, But Make It Togglable And Enabled By Default To New Players\n" +
        "3 = Yes, But Make It Togglable And Disabled By Default To New Players")]
        public int Custom_MuteSounds1 { get; set; } = 0;

        [Comment("If [Custom_MuteSounds1 = 1], Soundevent Hash Global Side")]
        [Comment("To Get Soundevent Hash Please Check https://github.com/oqyh/cs2-Game-Manager-GoldKingZ/blob/main/Resources/SoundEvents_Hash.txt Or Make [EnableDebug = 2] To See Soundevents")]
        [Comment("You Can Use Victim/Attacker/Global Side")]
        public List<uint> Custom_MuteSounds1_SoundeventHash_Global_Side { get; set; } = new List<uint>
        {
            
        };

        [Comment("If [Custom_MuteSounds1 = 2 or 3], Soundevent Hash Victim Side")]
        [Comment("To Get Soundevent Hash Please Check https://github.com/oqyh/cs2-Game-Manager-GoldKingZ/blob/main/Resources/SoundEvents_Hash.txt Or Make [EnableDebug = 2] To See Soundevents")]
        [Comment("You Can Use Victim Side Only OtherWise Plugin Will Break")]
        public List<uint> Custom_MuteSounds1_SoundeventHash_Victim_Side { get; set; } = new List<uint>
        {
            3573863551,
            2020934318
        };

        [Comment("If [Custom_MuteSounds1 = 2 or 3], Soundevent Hash Attacker Side")]
        [Comment("To Get Soundevent Hash Please Check https://github.com/oqyh/cs2-Game-Manager-GoldKingZ/blob/main/Resources/SoundEvents_Hash.txt Or Make [EnableDebug = 2] To See Soundevents")]
        [Comment("You Can Use Attacker Side Only OtherWise Plugin Will Break")]
        public List<uint> Custom_MuteSounds1_SoundeventHash_Attacker_Side { get; set; } = new List<uint>
        {
            2831007164,
            3535174312
        };

        [Comment("If [Custom_MuteSounds1 = 2 or 3], Commands To Toggle")]
        [Comment("Note: Console_Commands Can Be Execute Via Both Console And Chat By (!)")]
        [Comment("Making Both Console_Commands And Chat_Commands Empty = Disable")]
        [String("Console_Commands", "Chat_Commands")]
        public string Custom_MuteSounds1_CommandsInGame { get; set; } = "Console_Commands: | Chat_Commands: ";

        [Comment("If [Custom_MuteSounds1_CommandsInGame] Pass, Is There Any Specified Restricted Flags, Groups, SteamIDs")]
        [Comment("Example:")]
        [Comment("\"SteamIDs: 76561198206086993,STEAM_0:1:507335558 | Flags: @css/root,@css/admin | Groups: #css/root,#css/admin\"")]
        [Comment("\"SteamIDs:  | Flags:  | Groups: \" = To Allow Everyone")]
        [String("SteamIDs", "Flags", "Groups")]
        public string Custom_MuteSounds1_Flags { get; set; } = "SteamIDs: | Flags: | Groups:";

        [Comment("If [Custom_MuteSounds1_Flags] Pass, Hide Chat After Execute Custom_MuteSounds1_CommandsInGame?:")]
        [Comment("0 = No")]
        [Comment("1 = Yes, But Only After Toggle Successfully")]
        [Comment("2 = Yes, Hide All The Time")]
        [Range(0, 2, 0,
        "Custom_MuteSounds1_Hide: is invalid, setting to default value (0) Please Choose From 0 To 2.\n" +
        "0 = No\n" +
        "1 = Yes, But Only After Toggle Successfully\n" +
        "2 = Yes, Hide All The Time")]
        public int Custom_MuteSounds1_Hide { get; set; } = 0;
    }

    public class Custom_MuteSounds_2
    {
        [Comment("Mute Custom Mute 2 Sounds?")]
        [Comment("0 = No")]
        [Comment("1 = Yes")]
        [Comment("2 = Yes, But Make It Togglable And Enabled By Default To New Players")]
        [Comment("3 = Yes, But Make It Togglable And Disabled By Default To New Players")]
        [Range(0, 3, 0,
        "Custom_MuteSounds2: is invalid, setting to default value (0) Please Choose From 0 To 3.\n" +
        "0 = No\n" +
        "1 = Yes\n" +
        "2 = Yes, But Make It Togglable And Enabled By Default To New Players\n" +
        "3 = Yes, But Make It Togglable And Disabled By Default To New Players")]
        public int Custom_MuteSounds2 { get; set; } = 0;

        [Comment("If [Custom_MuteSounds2 = 1], Soundevent Hash Global Side")]
        [Comment("To Get Soundevent Hash Please Check https://github.com/oqyh/cs2-Game-Manager-GoldKingZ/blob/main/Resources/SoundEvents_Hash.txt Or Make [EnableDebug = 2] To See Soundevents")]
        [Comment("You Can Use Victim/Attacker/Global Side")]
        public List<uint> Custom_MuteSounds2_SoundeventHash_Global_Side { get; set; } = new List<uint>
        {
            
        };

        [Comment("If [Custom_MuteSounds2 = 2 or 3], Soundevent Hash Victim Side")]
        [Comment("To Get Soundevent Hash Please Check https://github.com/oqyh/cs2-Game-Manager-GoldKingZ/blob/main/Resources/SoundEvents_Hash.txt Or Make [EnableDebug = 2] To See Soundevents")]
        [Comment("You Can Use Victim Side Only OtherWise Plugin Will Break")]
        public List<uint> Custom_MuteSounds2_SoundeventHash_Victim_Side { get; set; } = new List<uint>
        {
            3124768561,
            2323025056
        };

        [Comment("If [Custom_MuteSounds2 = 2 or 3], Soundevent Hash Attacker Side")]
        [Comment("To Get Soundevent Hash Please Check https://github.com/oqyh/cs2-Game-Manager-GoldKingZ/blob/main/Resources/SoundEvents_Hash.txt Or Make [EnableDebug = 2] To See Soundevents")]
        [Comment("You Can Use Attacker Side Only OtherWise Plugin Will Break")]
        public List<uint> Custom_MuteSounds2_SoundeventHash_Attacker_Side { get; set; } = new List<uint>
        {
            708038349,
            1771184788
        };

        [Comment("If [Custom_MuteSounds2 = 2 or 3], Commands To Toggle")]
        [Comment("Note: Console_Commands Can Be Execute Via Both Console And Chat By (!)")]
        [Comment("Making Both Console_Commands And Chat_Commands Empty = Disable")]
        [String("Console_Commands", "Chat_Commands")]
        public string Custom_MuteSounds2_CommandsInGame { get; set; } = "Console_Commands: | Chat_Commands: ";

        [Comment("If [Custom_MuteSounds2_CommandsInGame] Pass, Is There Any Specified Restricted Flags, Groups, SteamIDs")]
        [Comment("Example:")]
        [Comment("\"SteamIDs: 76561198206086993,STEAM_0:1:507335558 | Flags: @css/root,@css/admin | Groups: #css/root,#css/admin\"")]
        [Comment("\"SteamIDs:  | Flags:  | Groups: \" = To Allow Everyone")]
        [String("SteamIDs", "Flags", "Groups")]
        public string Custom_MuteSounds2_Flags { get; set; } = "SteamIDs: | Flags: | Groups:";

        [Comment("If [Custom_MuteSounds2_Flags] Pass, Hide Chat After Execute Custom_MuteSounds2_CommandsInGame?:")]
        [Comment("0 = No")]
        [Comment("1 = Yes, But Only After Toggle Successfully")]
        [Comment("2 = Yes, Hide All The Time")]
        [Range(0, 2, 0,
        "Custom_MuteSounds2_Hide: is invalid, setting to default value (0) Please Choose From 0 To 2.\n" +
        "0 = No\n" +
        "1 = Yes, But Only After Toggle Successfully\n" +
        "2 = Yes, Hide All The Time")]
        public int Custom_MuteSounds2_Hide { get; set; } = 0;
    }

    public class Custom_MuteSounds_3
    {
        [Comment("Mute Custom Mute 3 Sounds?")]
        [Comment("0 = No")]
        [Comment("1 = Yes")]
        [Comment("2 = Yes, But Make It Togglable And Enabled By Default To New Players")]
        [Comment("3 = Yes, But Make It Togglable And Disabled By Default To New Players")]
        [Range(0, 3, 0,
        "Custom_MuteSounds3: is invalid, setting to default value (0) Please Choose From 0 To 3.\n" +
        "0 = No\n" +
        "1 = Yes\n" +
        "2 = Yes, But Make It Togglable And Enabled By Default To New Players\n" +
        "3 = Yes, But Make It Togglable And Disabled By Default To New Players")]
        public int Custom_MuteSounds3 { get; set; } = 0;

        [Comment("If [Custom_MuteSounds3 = 1], Soundevent Hash Global Side")]
        [Comment("To Get Soundevent Hash Please Check https://github.com/oqyh/cs2-Game-Manager-GoldKingZ/blob/main/Resources/SoundEvents_Hash.txt Or Make [EnableDebug = 2] To See Soundevents")]
        [Comment("You Can Use Victim/Attacker/Global Side")]
        public List<uint> Custom_MuteSounds3_SoundeventHash_Global_Side { get; set; } = new List<uint>
        {
            46413566,
            1815352525,
            2323025056,
            1771184788,
            3396420465,
            1823342283,
            3988751453,
            2192712263
        };

        [Comment("If [Custom_MuteSounds3 = 2 or 3], Soundevent Hash Victim Side")]
        [Comment("To Get Soundevent Hash Please Check https://github.com/oqyh/cs2-Game-Manager-GoldKingZ/blob/main/Resources/SoundEvents_Hash.txt Or Make [EnableDebug = 2] To See Soundevents")]
        [Comment("You Can Use Victim Side Only OtherWise Plugin Will Break")]
        public List<uint> Custom_MuteSounds3_SoundeventHash_Victim_Side { get; set; } = new List<uint>
        {
            
        };

        [Comment("If [Custom_MuteSounds3 = 2 or 3], Soundevent Hash Attacker Side")]
        [Comment("To Get Soundevent Hash Please Check https://github.com/oqyh/cs2-Game-Manager-GoldKingZ/blob/main/Resources/SoundEvents_Hash.txt Or Make [EnableDebug = 2] To See Soundevents")]
        [Comment("You Can Use Attacker Side Only OtherWise Plugin Will Break")]
        public List<uint> Custom_MuteSounds3_SoundeventHash_Attacker_Side { get; set; } = new List<uint>
        {
            
        };

        [Comment("If [Custom_MuteSounds3 = 2 or 3], Commands To Toggle")]
        [Comment("Note: Console_Commands Can Be Execute Via Both Console And Chat By (!)")]
        [Comment("Making Both Console_Commands And Chat_Commands Empty = Disable")]
        [String("Console_Commands", "Chat_Commands")]
        public string Custom_MuteSounds3_CommandsInGame { get; set; } = "Console_Commands: | Chat_Commands: ";

        [Comment("If [Custom_MuteSounds3_CommandsInGame] Pass, Is There Any Specified Restricted Flags, Groups, SteamIDs")]
        [Comment("Example:")]
        [Comment("\"SteamIDs: 76561198206086993,STEAM_0:1:507335558 | Flags: @css/root,@css/admin | Groups: #css/root,#css/admin\"")]
        [Comment("\"SteamIDs:  | Flags:  | Groups: \" = To Allow Everyone")]
        [String("SteamIDs", "Flags", "Groups")]
        public string Custom_MuteSounds3_Flags { get; set; } = "SteamIDs: | Flags: | Groups:";

        [Comment("If [Custom_MuteSounds3_Flags] Pass, Hide Chat After Execute Custom_MuteSounds3_CommandsInGame?:")]
        [Comment("0 = No")]
        [Comment("1 = Yes, But Only After Toggle Successfully")]
        [Comment("2 = Yes, Hide All The Time")]
        [Range(0, 2, 0,
        "Custom_MuteSounds3_Hide: is invalid, setting to default value (0) Please Choose From 0 To 2.\n" +
        "0 = No\n" +
        "1 = Yes, But Only After Toggle Successfully\n" +
        "2 = Yes, Hide All The Time")]
        public int Custom_MuteSounds3_Hide { get; set; } = 0;
    }

    public class MySqlServer
    {
        [Comment("MySQL Server address (hostname or IP)")]
        public string Server { get; set; } = "localhost";

        [Comment("MySQL Server port")]
        public int Port { get; set; } = 3306;

        [Comment("MySQL Database name")]
        public string Database { get; set; } = "MySql_Database";

        [Comment("MySQL Username")]
        public string Username { get; set; } = "MySql_Username";

        [Comment("MySQL Password")]
        public string Password { get; set; } = "MySql_Password";
    }

    public class MySqlConfig
    {
        [Comment("MySQL Servers You Can Add As Many As You like")]
        public List<MySqlServer> MySql_Servers { get; set; } = new List<MySqlServer>
        {
            new MySqlServer
            {
                Server = "localhost",
                Port = 3306,
                Database = "Database",
                Username = "Username",
                Password = "Password"
            },
            new MySqlServer
            {
                Server = "localhost2",
                Port = 3306,
                Database = "Database2",
                Username = "Username2",
                Password = "Password2"
            }
        };
    }

    public class Config
    {
        [BreakLine("----------------------------[ ↓ Plugin Info ↓ ]----------------------------{nextline}")]
        [Info("Version")]
        [Info("Github")]
        public object __InfoSection { get; set; } = null!;

        [BreakLine("----------------------------[ ↓ Main Config ↓ ]----------------------------{nextline}")]

        [Comment("Auto Set Player Language Depend Player Country?")]
        [Comment("true = Yes (Use Lang Depend Player Country Json If Found In Lang Folder, If Not Found Use Default Server core.json ServerLanguage json)")]
        [Comment("false = No (Use Default Server core.json ServerLanguage json)")]
        public bool AutoSetPlayerLanguage { get; set; } = false;

        [Comment("Reload Game Manager Plugin")]
        public Reload_GameManager Reload_GameManager { get; set; } = new();

        [BreakLine("----------------------------[ ↓ Block Config ↓ ]----------------------------{nextline}")]

        [Comment("Block Players Radio?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool BlockRadio { get; set; } = false;

        [Comment("Block Bot Radio?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool BlockBotRadio { get; set; } = false;
        
        [Comment("Block Players Radio When Throw Grenades?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool BlockGrenadesRadio { get; set; } = false;

        [Comment("Block Chat Wheel?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool BlockChatWheel { get; set; } = false;

        [Comment("Block Players Ping?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool BlockPing { get; set; } = false;

        [Comment("Block Players Animated Name Changer?")]
        [Comment("0 = No")]
        [Comment("1 = Yes, Send Him To Spec With Message Warning To Stop And Block From Joining Team Until Stop Changing Name")]
        [Comment("2 = Yes, Send Him To Spec With Message Warning To Stop And Block From Joining Team After [BlockNameChanger_Block] Secs Finish Then Send Server Console [BlockNameChanger_SendServerConsoleCommand]")]
        [Range(0, 2, 0,
        "BlockNameChanger: is invalid, setting to default value (0) Please Choose From 0 To 2.\n" +
        "0 = No\n" +
        "1 = Yes, Send Him To Spec With Message Warning To Stop And Block From Joining Team Until Stop Changing Name\n" +
        "2 = Yes, Send Him To Spec With Message Warning To Stop And Block From Joining Team After [BlockNameChanger_Block] Secs Finish Then Send Server Console [BlockNameChanger_SendServerConsoleCommand]")]
        public int BlockNameChanger { get; set; } = 0;

        [Comment("If [BlockNameChanger = 1 or 2], How Many (In Secs) Block Him From Joining")]
        public int BlockNameChanger_Block { get; set; } = 10;

        [Comment("If [BlockNameChanger = 2], Send Server Cosnole Command After Timer [BlockNameChanger_Block] Finish")]
        [Comment("==========================")]
        [Comment("        Placeholders")]
        [Comment("==========================")]
        [Comment("{PLAYER_NAME} = Player Name")]
        [Comment("{PLAYER_ID} = Player ID (Slot)")]
        [Comment("{PLAYER_IP} = Player IP")]
        [Comment("{PLAYER_IP_WITHOUT_PORT} = Player IP Without Port")]
        [Comment("{PLAYER_STEAMID} = Player SteamID (STEAM_0:1:122910632)")]
        [Comment("{PLAYER_STEAMID3} = Player SteamID ([U:1:245821265])")]
        [Comment("{PLAYER_STEAMID32} = Player SteamID (245821265)")]
        [Comment("{PLAYER_STEAMID64} = Player SteamID (76561198206086993)")]
        public string BlockNameChanger_SendServerConsoleCommand { get; set; } = "kick {PLAYER_NAME}";

        [Comment("Block Ingame Commands")]
        public Block_Commands Block_Commands { get; set; } = new();

        [BreakLine("----------------------------[ ↓ Hide Config ↓ ]----------------------------{nextline}")]

        [Comment("Hide Players Radar?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool HideRadar { get; set; } = false;

        [Comment("Hide Killfeed?")]
        [Comment("0 = No")]
        [Comment("1 = Yes, Hide Killfeed Completely")]
        [Comment("2 = Yes, Hide Players Killfeed And Show Who I Killed Only")]
        [Range(0, 2, 0,
        "HideKillfeed: is invalid, setting to default value (0) Please Choose From 0 To 2.\n" +
        "0 = No\n" +
        "1 = Yes, Hide Killfeed Completely\n" +
        "2 = Yes, Hide Players Killfeed And Show Who I Killed Only")]
        public int HideKillfeed { get; set; } = 0;

        [Comment("Hide Blood And HeadShot Spark Decals/Effects?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool HideBloodAndHsSpark { get; set; } = false;

        [Comment("Hide TeamMate Head Tag Names?")]
        [Comment("0 = No")]
        [Comment("1 = Yes, Disable It Completely But Show Names By Aiming")]
        [Comment("2 = Yes, Only Disable It When Players Behind Walls")]
        [Comment("3 = Yes, Disable It By Distance [HideTeamMateHeadTag_Distance] But Show Gradually When Im Closer To Player")]
        [Range(0, 3, 0,
        "HideTeamMateHeadTag: is invalid, setting to default value (0) Please Choose From 0 To 3.\n" +
        "0 = No\n" +
        "1 = Yes, Disable It Completely But Show Names By Aiming\n" +
        "2 = Yes, Only Disable It When Players Behind Walls\n" +
        "3 = Yes, Disable It By Distance [HideTeamMateHeadTag_Distance] But Show Gradually When Im Closer To Player")]
        public int HideTeamMateHeadTag { get; set; } = 0;

        [Comment("If [HideTeamMateHeadTag = 3], Show TeamMate Head Tag Names Gradually When Im :")]
        [Comment("50 = Very Close")]
        [Comment("150 = Close (Recommended)")]
        [Comment("250 = Far")]
        [Range(0, 999, 150,
        "HideTeamMateHeadTag_Distance: is invalid, setting to default value (150) Please Choose From 0 To 999.\n" +
        "50 = Very Close\n" +
        "150 = Close (Recommended)\n" +
        "250 = Far")]
        public int HideTeamMateHeadTag_Distance { get; set; } = 150;

        [Comment("Hide Dead Body?")]
        [Comment("0 = No")]
        [Comment("1 = Yes, After Death Immediately")]
        [Comment("2 = Yes, After Death With Delay [HideDeadBody_Delay]")]
        [Comment("3 = Yes, After Death Decay Body (Not Recommended For Performance)")]
        [Range(0, 3, 0,
        "HideDeadBody: is invalid, setting to default value (0) Please Choose From 0 To 3.\n" +
        "0 = No\n" +
        "1 = Yes, After Death Immediately\n" +
        "2 = Yes, After Death With Delay [HideDeadBody_Delay]\n" +
        "3 = Yes, After Death Decay Body (Not Recommended For Performance)")]
        public int HideDeadBody { get; set; } = 0;

        [Comment("If [HideDeadBody = 2], How Much Delay (In Secs)")]
        [Range(0, 999, 10,
        "HideDeadBody_Delay: is invalid, setting to default value (10) Please Choose From 0 To 999.")]
        public int HideDeadBody_Delay { get; set; } = 10;

        [Comment("Hide Legs?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool HideLegs { get; set; } = false;

        [Comment("Hide Chat HUD?")]
        [Comment("0 = No")]
        [Comment("1 = Yes")]
        [Comment("2 = Yes, But Delay [HideChatHUD_Delay] With Message Warning")]
        [Range(0, 2, 0,
        "HideChatHUD: is invalid, setting to default value (0) Please Choose From 0 To 2.\n" +
        "0 = No\n" +
        "1 = Yes\n" +
        "2 = Yes, But Delay [HideChatHUD_Delay] With Message Warning")]
        public int HideChatHUD { get; set; } = 0;

        [Comment("If [HideChatHUD = 2], How Much Delay (In Secs)")]
        [Range(0, 999, 10,
        "HideChatHUD_Delay: is invalid, setting to default value (10) Please Choose From 0 To 999.")]
        public int HideChatHUD_Delay { get; set; } = 10;

        [Comment("Hide Weapons HUD (Right Side Weapons Icons)?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool HideWeaponsHUD { get; set; } = false;

        [BreakLine("----------------------------[ ↓ Disable Config ↓ ]----------------------------{nextline}")]

        [Comment("Disable Players Fall Damage?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool DisableFallDamage { get; set; } = false;

        [Comment("Disable sv_cheats?")]
        [Comment("true = Yes (Will Force sv_cheats To Be Disabled)")]
        [Comment("false = No")]
        public bool DisableSvCheats_1 { get; set; } = false;

        [Comment("Disable C4 In The Game?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool DisableC4 { get; set; } = false;

        [Comment("Disable New Spectator Camera Transitions To All Players [https://www.youtube.com/watch?v=K2Ox69FHxlw]?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool DisableCameraSpectator { get; set; } = false;

        [Comment("Disable Players Screen Shake")]
        public Disable_AimPunch Disable_AimPunch { get; set; } = new();

        [BreakLine("----------------------------[ ↓ Sounds Config ↓ ]----------------------------{nextline}")]

        [Comment("Mute MVP Music?")]
        [Comment("0 = No")]
        [Comment("1 = Yes MVP Music Only")]
        [Comment("2 = Yes MVP Music And Round End Music")]
        [Range(0, 2, 0,
        "Sounds_MuteMVPMusic: is invalid, setting to default value (0) Please Choose From 0 To 2.\n" +
        "0 = No\n" +
        "1 = Yes MVP Music Only\n" +
        "2 = Yes MVP Music And Round End Music")]
        public int Sounds_MuteMVPMusic { get; set; } = 0;

        [Comment("Mute Players FootSteps Sounds?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool Sounds_MutePlayersFootSteps { get; set; } = false;

        [Comment("Mute Players Jump Land Sounds?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool Sounds_MuteJumpLand { get; set; } = false;

        [Comment("Mute Players Knife Stab Sounds?")]
        [Comment("0 = No")]
        [Comment("1 = Yes Completely")]
        [Comment("2 = Yes, Only On TeamMates")]
        [Range(0, 2, 0,
        "Sounds_MuteKnife: is invalid, setting to default value (0) Please Choose From 0 To 2.\n" +
        "0 = No\n" +
        "1 = Yes Completely\n" +
        "2 = Yes, Only On TeamMates")]
        public int Sounds_MuteKnife { get; set; } = 0;

        [Comment("If [Sounds_MuteKnife = 1 or 2], What We Mute In Soundevent Hash")]
        [Comment("To Get Soundevent Hash Please Check https://github.com/oqyh/cs2-Game-Manager-GoldKingZ/blob/main/Resources/SoundEvents_Hash.txt Or Make [EnableDebug = 2] To See Soundevents")]
        [Comment("You Can Use Victim/Attacker/Global Side")]
        public List<uint> Sounds_MuteKnife_SoundeventHash { get; set; } = new List<uint>
        {
            427534867,
            3475734633,
            1769891506
        };

        [Comment("Mute Players Gun Shots Sounds?")]
        [Comment("0 = No")]
        [Comment("1 = Yes Completely")]
        [Comment("2 = Yes, But Replace It With M4 Silencer")]
        [Comment("3 = Yes, But Replace It With Usp Silencer")]
        [Comment("4 = Yes, But Replace It With Custom Sounds [Sounds_MuteGunShots_weapon_id][Sounds_MuteGunShots_sound_type][Sounds_MuteGunShots_item_def_index]")]
        [Range(0, 4, 0,
        "Sounds_MuteGunShots: is invalid, setting to default value (0) Please Choose From 0 To 4.\n" +
        "0 = No\n" +
        "1 = Yes Completely\n" +
        "2 = Yes, But Replace It With M4 Silencer\n" +
        "3 = Yes, But Replace It With Usp Silencer\n" +
        "4 = Yes, But Replace It With Custom Sounds [Sounds_MuteGunShots_weapon_id][Sounds_MuteGunShots_sound_type][Sounds_MuteGunShots_item_def_index]")]
        public int Sounds_MuteGunShots { get; set; } = 0;

        [Comment("If [Sounds_MuteGunShots = 4], Whats weapon_id")]
        [Comment("Note: To Get Any weapon_id Make [EnableDebug = 3]")]
        public int Sounds_MuteGunShots_weapon_id { get; set; } = 0;

        [Comment("If [Sounds_MuteGunShots = 4], Whats sound_type")]
        [Comment("Note: To Get Any sound_type Make [EnableDebug = 3]")]
        public int Sounds_MuteGunShots_sound_type { get; set; } = 9;

        [Comment("If [Sounds_MuteGunShots = 4], Whats item_def_index")]
        [Comment("Note: To Get Any item_def_index Make [EnableDebug = 3]")]
        public int Sounds_MuteGunShots_item_def_index { get; set; } = 61;

        [Comment("Custom Mute Players (Headshot For Example)")]
        public Custom_MuteSounds_1 Custom_MuteSounds_1 { get; set; } = new();

        [Comment("Custom Mute Players (BodyShot For Example)")]
        public Custom_MuteSounds_2 Custom_MuteSounds_2 { get; set; } = new();

        [Comment("Custom Mute Players (Extra Put What Ever You Want)")]
        public Custom_MuteSounds_3 Custom_MuteSounds_3 { get; set; } = new();

        [BreakLine("----------------------------[ ↓ Default Messages Config ↓ ]----------------------------{nextline}")]

        [Comment("Ignore Bomb Planted HUD Messages And Sound?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool Ignore_BombPlantedHUDMessages { get; set; } = false;

        [Comment("Ignore TeamMate Attack Messages?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool Ignore_TeamMateAttackMessages { get; set; } = false;

        [Comment("Ignore Awards Money Messages?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool Ignore_AwardsMoneyMessages { get; set; } = false;

        [Comment("Ignore Saved You By Player Messages?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool Ignore_PlayerSavedYouByPlayerMessages { get; set; } = false;

        [Comment("Ignore You Chicken Has Been Killed Messages?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool Ignore_ChickenKilledMessages { get; set; } = false;

        [Comment("Ignore Join Team Messages?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool Ignore_JoinTeamMessages { get; set; } = false;

        [Comment("Ignore [PLANTING!] When Player Start Planting Bomb Messages?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool Ignore_PlantingBombMessages { get; set; } = false;

        [Comment("Ignore [DEFUSING!] When Player Start Defusing Bomb Messages?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool Ignore_DefusingBombMessages { get; set; } = false;

        [Comment("Ignore Disconnect Messages?")]
        [Comment("0 = No")]
        [Comment("1 = Yes Completely")]
        [Comment("2 = Yes, Also Remove Disconnect Icon In Killfeed")]
        [Range(0, 2, 0,
        "Ignore_DisconnectMessages: is invalid, setting to default value (0) Please Choose From 0 To 2.\n" +
        "0 = No\n" +
        "1 = Yes Completely\n" +
        "2 = Yes, Also Remove Disconnect Icon In Killfeed")]
        public int Ignore_DisconnectMessages { get; set; } = 0;

        [Comment("Ignore Custom TextMsg")]
        [Comment("Note: To Get TextMsg Messages Make [EnableDebug = 4]")]
        [Comment("Example:")]
        [Comment("\"#Team_Cash_Award_Loser_Bonus\"")]
        [Comment("\"#Player_Cash_Award_Killed_Enemy\"")]
        public List<string> Ignore_Custom_TextMsg { get; set; } = new List<string>
        {

        };
        
        [Comment("Ignore Custom HintText")]
        [Comment("Note: To Get HintText Messages Make [EnableDebug = 4]")]
        [Comment("Example:")]
        [Comment("\"#Cstrike_TitlesTXT_Switch_To_BurstFire\"")]
        [Comment("\"#Cstrike_TitlesTXT_Switch_To_SemiAuto\"")]
        public List<string> Ignore_Custom_HintText { get; set; } = new List<string>
        {

        };
        
        [Comment("Ignore Custom RadioText")]
        [Comment("Note: To Get RadioText Messages Make [EnableDebug = 4]")]
        [Comment("Example:")]
        [Comment("\"#Cstrike_TitlesTXT_Roger_that\"")]
        [Comment("\"#Cstrike_TitlesTXT_Negative\"")]
        public List<string> Ignore_Custom_RadioText { get; set; } = new List<string>
        {
            
        };

        [BreakLine("----------------------------[ ↓ Custom Messages Config ↓ ]----------------------------{nextline}")]

        [Comment("Custom Chat Messages (config/chat_processor.json)?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool Custom_ChatMessages { get; set; } = false;

        [Comment("If [Custom_ChatMessages = true], Do You Want Join Team Messages Exclude Bots?")]
        [Comment("true = Yes (Exclude Bots)")]
        [Comment("false = No (Include Bots)")]
        public bool Custom_JoinTeamMessages { get; set; } = false;

        [Comment("If [Custom_ChatMessages = true], How Would You Like Custom Throw Grenades Messages To Be Shown:")]
        [Comment("1 = Exclude Bots")]
        [Comment("2 = Include Bots")]
        [Comment("3 = Hide Nade Message From All When (mp_teammates_are_enemies true)")]
        [Comment("4 = Show Nade Message To All When (mp_teammates_are_enemies true) But Exclude Bots")]
        [Comment("5 = Show Nade Message To All When (mp_teammates_are_enemies true) But Include Bots")]
        [Range(1, 5, 1,
        "Custom_ThrowNadeMessages: is invalid, setting to default value (1) Please Choose From 1 To 5.\n" +
        "1 = Exclude Bots\n" +
        "2 = Include Bots\n" +
        "3 = Hide Nade Message From All When (mp_teammates_are_enemies true)\n" +
        "4 = Show Nade Message To All When (mp_teammates_are_enemies true) But Exclude Bots\n" +
        "5 = Show Nade Message To All When (mp_teammates_are_enemies true) But Include Bots")]
        public int Custom_ThrowNadeMessages { get; set; } = 1;

        [Comment("If [Custom_ChatMessages = true], How Would You Like The Message To Be Shown:")]
        [Comment("1 = Message Show To All")]
        [Comment("2 = Alive Players Cannot See Any Messages From Dead Players")]
        [Comment("3 = Alive Players Can See Only Their Team’s Dead Players Messages, Not The Enemy’s Messages")]
        [Range(1, 3, 1,
        "Custom_ChatMessages_Mode: is invalid, setting to default value (1) Please Choose From 1 To 3.\n" +
        "1 = Message Show To All\n" +
        "2 = Alive Players Cannot See Any Messages From Dead Players\n" +
        "3 = Alive Players Can See Only Their Team’s Dead Players Messages, Not The Enemy’s Messages")]
        public int Custom_ChatMessages_Mode { get; set; } = 1;

        [Comment("If [Custom_ChatMessages = true], Exclude Custom Chat Messages If It Start With")]
        public List<string> Custom_ChatMessages_ExcludeStartWith { get; set; } = new List<string>
        {
            "!",
            ".",
            "/"
        };

        [Comment("If You Using [Custom_ChatMessages_ExcludeStartWith] Do We Ignore Upper/LowerCase?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool Custom_ChatMessages_ExcludeStartWith_IgnoreCase { get; set; } = true;

        [Comment("If [Custom_ChatMessages = true], Exclude Custom Chat Messages If It Contains")]
        public List<string> Custom_ChatMessages_ExcludeContains { get; set; } = new List<string>
        {
            "GoldKingZ",
            "oqyh"
        };
        
        [Comment("If You Using [Custom_ChatMessages_ExcludeContains] Do We Ignore Upper/LowerCase?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool Custom_ChatMessages_ExcludeContains_IgnoreCase { get; set; } = true;

        [BreakLine("----------------------------[ ↓ Auto Clean Drop Weapons Config ↓ ]----------------------------{nextline}")]

        [Comment("Enable Auto Clean Drop Weapons?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool AutoClean_Enable { get; set; } = false;

        [Comment("If [AutoClean_Enable = true] Defines How Often (In Secs) The Plugin Checks For Ground Weapons")]
        [Comment("Choose From 1 To 999")]
        [Range(1, 999, 5,
        "AutoClean_MaxWeaponsOnGround: is invalid, setting to default value (5) Please Choose From 1 To 999.")]
        public int AutoClean_Timer { get; set; } = 5;

        [Comment("If [AutoClean_Timer] Pass Whats Max Weapons On Ground")]
        [Comment("Choose From 1 To 999")]
        [Range(1, 999, 5,
        "AutoClean_MaxWeaponsOnGround: is invalid, setting to default value (5) Please Choose From 1 To 999.")]
        public int AutoClean_MaxWeaponsOnGround { get; set; } = 5;

        [Comment("If [AutoClean_MaxWeaponsOnGround] Pass Choose Which Weapons Do We Target")]
        [Comment("A = (weapon_awp, weapon_g3sg1, weapon_scar20, weapon_ssg08)")]
        [Comment("B = (weapon_ak47, weapon_aug, weapon_famas, weapon_galilar, weapon_m4a1_silencer, weapon_m4a1, weapon_sg556)")]
        [Comment("C = (weapon_m249, weapon_negev)")]
        [Comment("D = (weapon_mag7, weapon_nova, weapon_sawedoff, weapon_xm1014)")]
        [Comment("E = (weapon_bizon, weapon_mac10, weapon_mp5sd, weapon_mp7, weapon_mp9, weapon_p90, weapon_ump45)")]
        [Comment("F = (weapon_cz75a, weapon_deagle, weapon_elite, weapon_fiveseven, weapon_glock, weapon_hkp2000, weapon_p250, weapon_revolver, weapon_tec9, weapon_usp_silencer)")]
        [Comment("G = (weapon_smokegrenade, weapon_hegrenade, weapon_flashbang, weapon_decoy, weapon_molotov, weapon_incgrenade)")]
        [Comment("H = (item_defuser, item_cutters)")]
        [Comment("I = (weapon_taser)")]
        [Comment("J = (weapon_healthshot)")]
        [Comment("K = (weapon_knife, weapon_knife_t)")]
        [Comment("ANY OR Empty = Means All Weapons")]
        [Comment("Or You Can Add Weapon Name Manually")]
        public List<string> AutoClean_TheseDroppedWeaponsOnly { get; set; } = new List<string>
        {
            "A",
            "B",
            "C",
            "D",
            "E",
            "G",
            "H",
            "weapon_glock",
            "weapon_usp_silencer",
            "weapon_hkp2000"
        };

        [BreakLine("----------------------------[ ↓ Advanced Filters Config ↓ ]----------------------------{nextline}")]

        [Comment("Filter Ip Address")]
        [Comment("Whitelist These IPs (No Need For Ports)")]
        [Comment("Any Ip Not In The List Will Be Blocked")]
        [Comment("Example:")]
        [Comment("\"12.34.56.789\"")]
        [Comment("\"14.21.61.121\"")]
        [Comment("Empty = Disable This Feature")]
        public List<string> Filter_Whitelist_Ips { get; set; } = new List<string>
        {
            "1.1.1.1",
            "12.34.56.789",
            "14.21.61.121"
        };

        [Comment("Filter URLs")]
        [Comment("Whitelist These URLs")]
        [Comment("Any URLs Not In The List Will Be Blocked")]
        [Comment("Example:")]
        [Comment("\"https://www.youtube.com/\" To Allow Any Youtube Link")]
        [Comment("\"https://www.youtube.com/test\" Only Specific Youtube Video URL Is Allowed")]
        [Comment("Empty = Disable This Feature")]
        public List<string> Filter_Whitelist_URLs { get; set; } = new List<string>
        {
            "goldkingz.net",
            "www.google.com",
            "https://www.youtube.com"
        };

        [Comment("Filter Players Name?")]
        [Comment("0 = No")]
        [Comment("1 = Yes, Check If Player Name Has [Filter_Whitelist_Ips] Remove It")]
        [Comment("2 = Yes, Check If Player Name Has [Filter_Whitelist_URLs] Remove It")]
        [Comment("3 = Yes, Check If Player Name Has [Filter_Whitelist_Ips] Or [Filter_Whitelist_URLs] Remove It")]
        [Range(0, 3, 0,
        "Cookies_Enable: is invalid, setting to default value (0) Please Choose From 0 To 3.\n" +
        "0 = No\n" +
        "1 = Yes, Check If Player Name Has [Filter_Whitelist_Ips] Remove It\n" +
        "2 = Yes, Check If Player Name Has [Filter_Whitelist_URLs] Remove It\n" +
        "3 = Yes, Check If Player Name Has [Filter_Whitelist_Ips] Or [Filter_Whitelist_URLs] Remove It")]
        public int Filter_Players_Names { get; set; } = 0;

        [Comment("Filter Players Chat?")]
        [Comment("0 = No")]
        [Comment("1 = Yes, Check If Player Chat Has [Filter_Whitelist_Ips] Block It")]
        [Comment("2 = Yes, Check If Player Chat Has [Filter_Whitelist_URLs] Block It")]
        [Comment("3 = Yes, Check If Player Chat Has [Filter_Whitelist_Ips] Or [Filter_Whitelist_URLs] Block It")]
        [Range(0, 3, 0,
        "Cookies_Enable: is invalid, setting to default value (0) Please Choose From 0 To 3.\n" +
        "0 = No\n" +
        "1 = Yes, Check If Player Chat Has [Filter_Whitelist_Ips] Block It\n" +
        "2 = Yes, Check If Player Chat Has [Filter_Whitelist_URLs] Block It\n" +
        "3 = Yes, Check If Player Chat Has [Filter_Whitelist_Ips] Or [Filter_Whitelist_URLs] Block It")]
        public int Filter_Players_Chat { get; set; } = 0;

        [BreakLine("----------------------------[ ↓ Locally Config ↓ ]----------------------------{nextline}")]

        [Comment("Save Players Data By Cookies Locally (In ../Game-Manager-GoldKingZ/cookies/)?")]
        [Comment("0 = No")]
        [Comment("1 = Yes, But Save Data On Players Disconnect (Warning Performance)")]
        [Comment("2 = Yes, But Save Data On Map Change (Recommended)")]
        [Range(0, 2, 2,
        "Cookies_Enable: is invalid, setting to default value (2) Please Choose From 0 To 2.\n" +
        "0 = No\n" +
        "1 = Yes, But Save Data On Players Disconnect (Warning Performance)\n" +
        "2 = Yes, But Save Data On Map Change (Recommended)")]
        public int Cookies_Enable { get; set; } = 2;

        [Comment("If [Cookies_Enable = 1 or 2], Auto Delete Inactive Players More Than X (Days) Old")]
        [Comment("0 = Dont Auto Delete")]
        public int Cookies_AutoRemovePlayerOlderThanXDays { get; set; } = 7;

        [BreakLine("----------------------------[ ↓ MySql Config ↓ ]----------------------------{nextline}")]
        
        [Comment("Save Players Data Into MySql?")]
        [Comment("0 = No")]
        [Comment("1 = Yes, But Save Data On Players Disconnect (Warning Performance)")]
        [Comment("2 = Yes, But Save Data On Map Change (Recommended)")]
        [Range(0, 2, 0,
        "MySql_Enable: is invalid, setting to default value (0) Please Choose From 0 To 2.\n" +
        "0 = No\n" +
        "1 = Yes, But Save Data On Players Disconnect (Warning Performance)\n" +
        "2 = Yes, But Save Data On Map Change (Recommended)")]
        public int MySql_Enable { get; set; } = 0;

        [Comment("Connection Timeout In Seconds")]
        [Range(5, 60, 30, "Connection timeout must be between 5 and 60 seconds")]
        public int MySql_ConnectionTimeout { get; set; } = 30;

        [Comment("Retry Attempts When Connection Fails")]
        [Range(1, 5, 3, "Retry attempts must be between 1 and 5")]
        public int MySql_RetryAttempts { get; set; } = 3;

        [Comment("Delay Between Retries In Seconds")]
        [Range(1, 10, 2, "Retry delay must be between 1 and 10 seconds")]
        public int MySql_RetryDelay { get; set; } = 2;

        [Comment("MySql Config")]
        public MySqlConfig MySql_Config { get; set; } = new MySqlConfig();

        [Comment("Auto Delete Inactive Players More Than X (Days) Old")]
        [Comment("0 = Dont Auto Delete")]
        public int MySql_AutoRemovePlayerOlderThanXDays { get; set; } = 7;

        [BreakLine("----------------------------[ ↓ Utilities  ↓ ]----------------------------{nextline}")]

        [Comment("Auto Update GeoLocation (In ../Game-Manager-GoldKingZ/GeoLocation/)?")]
        [Comment("true = Yes")]
        [Comment("false = No")]
        public bool AutoUpdateGeoLocation { get; set; } = true;
        
        [Comment("Enable Debug Plugin In Server Console?")]
        [Comment("0 = No")]
        [Comment("1 = Yes, Debug Everything")]
        [Comment("2 = Yes, Debug For Custom_MuteSounds Only")]
        [Comment("3 = Yes, Debug For Sounds_MuteGunShots Only")]
        [Comment("4 = Yes, Debug For Ignore_Custom (TextMsg/HintText/RadioText) Only")]
        [Range(0, 4, 0,
        "EnableDebug: is invalid, setting to default value (0) Please Choose From 0 To 4.\n" +
        "0 = No\n" +
        "1 = Yes, Debug Everything\n" +
        "2 = Yes, Debug For Custom_MuteSounds Only\n" +
        "3 = Yes, Debug For Sounds_MuteGunShots Only\n" +
        "4 = Yes, Debug For Ignore_Custom (TextMsg/HintText/RadioText) Only")]
        public int EnableDebug { get; set; } = 0;
    }

    public static class Configs
    {
        public static string Version = $"Version : {MainPlugin.Instance.ModuleVersion ?? "Unknown"}";
        public static string Github = "https://github.com/oqyh/cs2-Game-Manager-GoldKingZ";
        public static Config Instance { get; private set; } = new Config();
        static string? filePath;
        static bool IsSimple(Type t) => t.IsPrimitive || t.IsEnum || t == typeof(string) || t == typeof(decimal) || t == typeof(DateTime) || t == typeof(uint);
        static bool IsList(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>);
        
        private static readonly JsonSerializerOptions SimpleValueJsonOptions = new JsonSerializerOptions{WriteIndented = false,Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping};

        public static void Load(string moduleDirectory)
        {
            string configDirectory = Path.Combine(moduleDirectory ?? ".", "config");
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }

            filePath = Path.Combine(configDirectory, "config.json");

            if (!File.Exists(filePath)) { Save(); return; }

            try
            {
                var json = File.ReadAllText(filePath);

                json = RemoveTrailingCommas(json);
                
                var lines = json.Split('\n').Where(l => !l.TrimStart().StartsWith("//")).ToArray();
                json = string.Join("\n", lines);

                JsonNode? root = null;
                try
                {
                    root = JsonNode.Parse(json);
                }
                catch
                {
                    Instance = new Config();
                    EnsureNestedDefaults(Instance);
                    ValidateStringRecursive(Instance);
                    ValidateRangesRecursive(Instance);
                    ValidateForceStringRecursive(Instance);
                    Save();
                    return;
                }

                if (root is JsonObject rootObj)
                {
                    CleanJsonObjectStrict(rootObj, Instance.GetType());
                }

                string normalizedJson = root?.ToJsonString(new JsonSerializerOptions { WriteIndented = false }) ?? "{}";
                Instance = JsonSerializer.Deserialize<Config>(normalizedJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new Config();
            }
            catch
            {
                Instance = new Config();
            }

            EnsureNestedDefaults(Instance);
            ValidateStringRecursive(Instance);
            ValidateRangesRecursive(Instance);
            ValidateForceStringRecursive(Instance);
            Save();
        }

        public static void Save()
        {
            try
            {
                var path = filePath ?? Path.Combine(".", "config", "config.json");
                string? directory = Path.GetDirectoryName(path);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                var props = typeof(Config).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.GetIndexParameters().Length == 0);

                var rendered = new List<string>();
                foreach (var p in props) 
                {
                    try
                    {
                        rendered.Add(RenderProperty(p, Instance, 2));
                    }
                    catch
                    {
                    }
                }

                string JoinJsonProperties(List<string> propsList)
                {
                    var filtered = propsList.Where(r => !string.IsNullOrWhiteSpace(r)).ToList();
                    var result = new List<string>();

                    bool BlockContainsProperty(string block)
                    {
                        return block
                            .Split('\n')
                            .Any(line =>
                            {
                                var t = line.TrimStart();
                                return t.StartsWith("\"") && t.Contains("\":");
                            });
                    }

                    for (int i = 0; i < filtered.Count; i++)
                    {
                        var current = filtered[i];
                        bool isCurrentPropertyBlock = BlockContainsProperty(current);

                        int nextPropIndex = -1;
                        for (int j = i + 1; j < filtered.Count; j++)
                        {
                            if (BlockContainsProperty(filtered[j]))
                            {
                                nextPropIndex = j;
                                break;
                            }
                        }

                        bool hasNextProp = nextPropIndex != -1;
                        if (isCurrentPropertyBlock && hasNextProp)
                        {
                            if (!current.TrimEnd().EndsWith(","))
                            {
                                current += ",";
                            }
                        }

                        result.Add(current);
                    }

                    return string.Join("\n\n", result);
                }

                var body = JoinJsonProperties(rendered);
                var final = "{\n" + body + "\n}\n";
                File.WriteAllText(path, final);
            }
            catch
            {
            }
        }

        static void EnsureNestedDefaults(object? obj)
        {
            if (obj == null) return;
            
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite && p.GetIndexParameters().Length == 0);

            foreach (var p in props)
            {
                if (p.GetCustomAttributes<InfoAttribute>().Any()) continue;

                if (IsList(p.PropertyType)) continue;

                if (p.PropertyType == typeof(string) || p.PropertyType.IsValueType) continue;
                
                try
                {
                    var val = p.GetValue(obj);
                    if (val == null)
                    {
                        try 
                        { 
                            var inst = Activator.CreateInstance(p.PropertyType); 
                            if (inst != null) p.SetValue(obj, inst); 
                        }
                        catch { }
                    }
                    EnsureNestedDefaults(p.GetValue(obj));
                }
                catch (TargetParameterCountException)
                {
                    continue;
                }
                catch
                {
                }
            }
        }

        public static void ValidateStringRecursive(object? obj)
        {
            if (obj == null) return;
            
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite && p.GetIndexParameters().Length == 0);

            foreach (var prop in props)
            {
                if (prop.GetCustomAttributes<InfoAttribute>().Any() && obj is Config) continue;

                if (prop.PropertyType == typeof(string))
                {
                    if (prop.GetCustomAttribute<StringAttribute>() is StringAttribute attr)
                    {
                        try
                        {
                            var current = prop.GetValue(obj) as string;
                            prop.SetValue(obj, string.Join(" | ", attr.Keys.Select(key => 
                                $"{key}: {GetStringValue(current, key)}")));
                        }
                        catch
                        {
                        }
                    }
                }
                else if (!IsSimple(prop.PropertyType) && !IsList(prop.PropertyType))
                {
                    try
                    {
                        ValidateStringRecursive(prop.GetValue(obj));
                    }
                    catch (TargetParameterCountException)
                    {
                        continue;
                    }
                }
            }
        }

        public static string GetStringValue(string? input, string key)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            
            return input.Split('|')
                .Select(segment => segment.Trim())
                .FirstOrDefault(segment => segment.StartsWith(key + ":", StringComparison.OrdinalIgnoreCase))?
                .Substring(key.Length + 1)
                .Trim() ?? "";
        }

        static void ValidateRangesRecursive(object? obj)
        {
            if (obj == null) return;
            
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite && p.GetIndexParameters().Length == 0);

            foreach (var p in props)
            {
                if (p.GetCustomAttributes<InfoAttribute>().Any()) continue;

                try
                {
                    var range = p.GetCustomAttribute<RangeAttribute>();
                    var val = p.GetValue(obj);
                    if (range != null && val != null)
                    {
                        if (double.TryParse(Convert.ToString(val), out double d))
                        {
                            if (d < range.Min || d > range.Max)
                            {
                                if (!string.IsNullOrEmpty(range.Message))
                                {
                                    var messageLines = range.Message.Replace("\\n", "\n").Split('\n');
                                    foreach (var line in messageLines)
                                    {
                                        if (!string.IsNullOrWhiteSpace(line))
                                        {
                                            Helper.DebugMessage(line.Trim(), 0);
                                        }
                                    }
                                }
                                p.SetValue(obj, Convert.ChangeType(range.Default, p.PropertyType));
                            }
                        }
                    }
                    if (!IsSimple(p.PropertyType) && !IsList(p.PropertyType)) 
                    {
                        ValidateRangesRecursive(p.GetValue(obj));
                    }
                }
                catch (TargetParameterCountException)
                {
                    continue;
                }
                catch
                {
                }
            }
        }

        static void ValidateForceStringRecursive(object? obj)
        {
            if (obj == null) return;
            
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite && p.GetIndexParameters().Length == 0);

            foreach (var prop in props)
            {
                if (prop.GetCustomAttributes<InfoAttribute>().Any()) continue;

                try
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        if (prop.GetCustomAttribute<ForceStringAttribute>() is ForceStringAttribute forceAttr)
                        {
                            var current = prop.GetValue(obj) as string;
                            if (string.IsNullOrWhiteSpace(current))
                            {
                                prop.SetValue(obj, forceAttr.FallbackValue);
                            }
                        }
                    }
                    else if (!IsSimple(prop.PropertyType) && !IsList(prop.PropertyType))
                    {
                        ValidateForceStringRecursive(prop.GetValue(obj));
                    }
                }
                catch (TargetParameterCountException)
                {
                    continue;
                }
                catch
                {
                }
            }
        }

        static IEnumerable<string> RenderCommentLines(string? text, string pad)
        {
            if (string.IsNullOrWhiteSpace(text)) yield break;
            var lines = text.Replace("\r", "").Split('\n');
            foreach (var raw in lines)
            {
                var t = raw.TrimEnd();
                if (string.IsNullOrWhiteSpace(t))
                {
                    yield return pad + "//";
                }
                else if (t == "{nextline}")
                {
                    yield return "";
                }
                else
                {
                    yield return pad + "// " + t;
                }
            }
        }
        private static string RemoveTrailingCommas(string json)
        {
            json = System.Text.RegularExpressions.Regex.Replace(json, @",(\s*[]])", "$1");
            json = System.Text.RegularExpressions.Regex.Replace(json, @",(\s*[}])", "$1");
            return json;
        }

        static string RenderProperty(PropertyInfo p, object? instance, int indent)
        {
            var pad = new string(' ', indent);
            var parts = new List<string>();

            try
            {
                var br = p.GetCustomAttribute<BreakLineAttribute>();
                if (br != null)
                {
                    var txt = br.Text ?? "";

                    bool emptyLineBefore = txt.StartsWith("{nextline}");
                    bool emptyLineAfter = txt.EndsWith("{nextline}");

                    if (emptyLineBefore) txt = txt.Substring("{nextline}".Length);
                    if (emptyLineAfter) txt = txt.Substring(0, txt.Length - "{nextline}".Length);

                    txt = txt.Trim();

                    if (emptyLineBefore)
                        parts.Add(string.Empty);

                    foreach (var line in RenderCommentLines(txt, pad))
                        parts.Add(line);

                    if (emptyLineAfter)
                        parts.Add(string.Empty);
                }

                var infos = p.GetCustomAttributes<InfoAttribute>();
                foreach (var info in infos)
                {
                    string text = info.Key switch
                    {
                        "Version" => Version,
                        "Github" => Github,
                        _ => info.Key
                    };
                    foreach (var line in RenderCommentLines(text, pad))
                        parts.Add(line);
                }

                var comments = p.GetCustomAttributes<CommentAttribute>();
                foreach (var comment in comments)
                {
                    foreach (var line in RenderCommentLines(comment.Text, pad))
                        parts.Add(line);
                }

                if (p.GetCustomAttributes<InfoAttribute>().Any() && (p.PropertyType == typeof(object) || p.PropertyType == typeof(void)))
                    return string.Join("\n", parts);

                var val = p.GetValue(instance);
                if (IsSimple(p.PropertyType))
                {
                    var jsonVal = JsonSerializer.Serialize(val, SimpleValueJsonOptions);
                    parts.Add(pad + $"\"{p.Name}\": {jsonVal}");
                }
                else if (IsList(p.PropertyType))
                {
                    parts.Add(pad + $"\"{p.Name}\":");
                    parts.Add(pad + "[");

                    if (val is System.Collections.IList list && list.Count > 0)
                    {
                        var listItems = new List<string>();
                        var elementType = p.PropertyType.GetGenericArguments()[0];
                        
                        if (IsSimple(elementType))
                        {
                            foreach (var item in list)
                            {
                                if (item != null)
                                {
                                    var jsonVal = JsonSerializer.Serialize(item, SimpleValueJsonOptions);
                                    listItems.Add(pad + "  " + jsonVal);
                                }
                            }
                            parts.Add(string.Join(",\n", listItems));
                        }
                        else
                        {
                            foreach (var item in list)
                            {
                                if (item != null)
                                {
                                    var itemProps = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                        .Where(ip => ip.CanRead && ip.GetIndexParameters().Length == 0);
                                    var itemLines = new List<string>();
                                    foreach (var ip in itemProps) 
                                    {
                                        try
                                        {
                                            itemLines.Add(RenderProperty(ip, item, indent + 4));
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    var filtered = itemLines.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                                    var itemJson = string.Join(",\n", filtered);
                                    listItems.Add(pad + "  {\n" + itemJson + "\n" + pad + "  }");
                                }
                            }
                            parts.Add(string.Join(",\n", listItems));
                        }
                    }

                    parts.Add(pad + "]");
                }
                else
                {
                    if (val == null)
                    {
                        parts.Add(pad + $"\"{p.Name}\": null");
                    }
                    else
                    {
                        parts.Add(pad + $"\"{p.Name}\":");
                        parts.Add(pad + "{");

                        var innerProps = p.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(ip => ip.CanRead && ip.GetIndexParameters().Length == 0);
                        
                        var innerLines = new List<string>();
                        foreach (var ip in innerProps) 
                        {
                            try
                            {
                                innerLines.Add(RenderProperty(ip, val, indent + 2));
                            }
                            catch
                            {
                            }
                        }
                        
                        var filtered = innerLines.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                        var innerJoined = string.Join(",\n\n", filtered);
                        if (!string.IsNullOrEmpty(innerJoined)) parts.Add(innerJoined);

                        parts.Add(pad + "}");
                    }
                }
            }
            catch
            {
                return "";
            }

            return string.Join("\n", parts);
        }

        static void CleanJsonObjectStrict(JsonObject jsonObj, Type targetType)
        {
            if (jsonObj == null || targetType == null) return;

            var props = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .Where(p => !p.GetCustomAttributes<InfoAttribute>().Any() && p.GetIndexParameters().Length == 0)
                                .ToList();
            var map = props.ToDictionary(p => p.Name.ToLowerInvariant(), p => p);

            foreach (var key in jsonObj.Select(kv => kv.Key).ToList())
            {
                var keyLower = key.ToLowerInvariant();
                if (!map.TryGetValue(keyLower, out var prop))
                {
                    jsonObj.Remove(key);
                    continue;
                }

                var expectedType = prop.PropertyType;
                var underlying = Nullable.GetUnderlyingType(expectedType) ?? expectedType;
                var node = jsonObj[key];
                if (node == null) continue;

                if (IsSimple(underlying) || underlying.IsEnum)
                {
                    if (node is JsonValue jv)
                    {
                        if (!JsonValueIsExactType(jv, underlying))
                        {
                            jsonObj.Remove(key);
                        }
                    }
                    else
                    {
                        jsonObj.Remove(key);
                    }
                }
                else if (IsList(underlying))
                {
                    if (!(node is JsonArray))
                    {
                        jsonObj.Remove(key);
                    }
                    else
                    {
                        var array = node.AsArray();
                        var itemType = underlying.GetGenericArguments()[0];
                        
                        for (int i = array.Count - 1; i >= 0; i--)
                        {
                            var item = array[i];
                            if (IsSimple(itemType))
                            {
                                if (!(item is JsonValue jvItem) || !JsonValueIsExactType(jvItem, itemType))
                                {
                                    if (itemType == typeof(string) && item is JsonValue jv)
                                    {
                                        try
                                        {
                                            if (jv.TryGetValue<int>(out int intVal))
                                            {
                                                array[i] = JsonValue.Create(intVal.ToString());
                                                continue;
                                            }
                                            if (jv.TryGetValue<double>(out double doubleVal))
                                            {
                                                array[i] = JsonValue.Create(doubleVal.ToString());
                                                continue;
                                            }
                                            if (jv.TryGetValue<long>(out long longVal))
                                            {
                                                array[i] = JsonValue.Create(longVal.ToString());
                                                continue;
                                            }
                                        }
                                        catch
                                        {
                                            array.RemoveAt(i);
                                        }
                                    }
                                    else
                                    {
                                        array.RemoveAt(i);
                                    }
                                }
                            }
                            else if (item is JsonObject itemObj)
                            {
                                CleanJsonObjectStrict(itemObj, itemType);
                            }
                            else if (!(item is JsonObject))
                            {
                                array.RemoveAt(i);
                            }
                        }
                    }
                }
                else
                {
                    if (node is JsonObject childObj)
                    {
                        CleanJsonObjectStrict(childObj, underlying);
                    }
                    else
                    {
                        jsonObj.Remove(key);
                    }
                }
            }
        }

        static bool JsonValueIsExactType(JsonValue jv, Type clrType)
        {
            try
            {
                var t = clrType;
                if (t == typeof(bool))
                {
                    try { jv.GetValue<bool>(); return true; } catch { return false; }
                }

                if (t == typeof(int) || t == typeof(long) || t == typeof(short) || t == typeof(byte) ||
                    t == typeof(uint) || t == typeof(ulong) || t == typeof(ushort) || t == typeof(sbyte))
                {
                    try
                    {
                        jv.GetValue<long>();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

                if (t == typeof(double) || t == typeof(float) || t == typeof(decimal))
                {
                    try { jv.GetValue<double>(); return true; } catch { return false; }
                }

                if (t == typeof(string))
                {
                    try { jv.GetValue<string>(); return true; } catch { return false; }
                }

                if (t == typeof(DateTime))
                {
                    try
                    {
                        var s = jv.GetValue<string>();
                        return DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out _);
                    }
                    catch { return false; }
                }

                if (t.IsEnum)
                {
                    try
                    {
                        var s = jv.GetValue<string>();
                        if (!string.IsNullOrEmpty(s))
                        {
                            var names = Enum.GetNames(t);
                            if (names.Any(n => string.Equals(n, s, StringComparison.OrdinalIgnoreCase)))
                                return true;
                        }
                    }
                    catch { }

                    try
                    {
                        var v = jv.GetValue<long>();
                        return true;
                    }
                    catch { }

                    return false;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}