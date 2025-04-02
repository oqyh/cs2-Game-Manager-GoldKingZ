using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Game_Manager_GoldKingZ.Config
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RangeAttribute : Attribute
    {
        public int Min { get; }
        public int Max { get; }
        public int Default { get; }
        public string Message { get; }

        public RangeAttribute(int min, int max, int defaultValue, string message)
        {
            Min = min;
            Max = max;
            Default = defaultValue;
            Message = message;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CommentAttribute : Attribute
    {
        public string Comment { get; }

        public CommentAttribute(string comment)
        {
            Comment = comment;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class BreakLineAttribute : Attribute
    {
        public string BreakLine { get; }

        public BreakLineAttribute(string breakLine)
        {
            BreakLine = breakLine;
        }
    }
    public static class Configs
    {
        private static readonly string ConfigDirectoryName = "config";
        private static readonly string ConfigFileName = "config.json";
        private static string? _configFilePath;
        private static ConfigData? _configData;

        private static readonly JsonSerializerOptions SerializationOptions = new()
        {
            Converters =
            {
                new JsonStringEnumConverter()
            },
            WriteIndented = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
        };

        public static bool IsLoaded()
        {
            return _configData is not null;
        }

        public static ConfigData GetConfigData()
        {
            if (_configData is null)
            {
                throw new Exception("Config not yet loaded.");
            }
            
            return _configData;
        }

        public static ConfigData Load(string modulePath)
        {
            var configFileDirectory = Path.Combine(modulePath, ConfigDirectoryName);
            if(!Directory.Exists(configFileDirectory))
            {
                Directory.CreateDirectory(configFileDirectory);
            }

            _configFilePath = Path.Combine(configFileDirectory, ConfigFileName);
            if (File.Exists(_configFilePath))
            {
                _configData = JsonSerializer.Deserialize<ConfigData>(File.ReadAllText(_configFilePath), SerializationOptions);
                _configData!.Validate();
            }
            else
            {
                _configData = new ConfigData();
                _configData.Validate();
            }

            if (_configData is null)
            {
                throw new Exception("Failed to load configs.");
            }

            SaveConfigData(_configData);
            
            return _configData;
        }

        private static void SaveConfigData(ConfigData configData)
        {
            if (_configFilePath is null)
                throw new Exception("Config not yet loaded.");

            string json = JsonSerializer.Serialize(configData, SerializationOptions);
            json = Regex.Unescape(json);

            var lines = json.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var newLines = new List<string>();

            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"^\s*""(\w+)""\s*:.*");
                bool isPropertyLine = false;
                PropertyInfo? propInfo = null;

                if (match.Success)
                {
                    string propName = match.Groups[1].Value;
                    propInfo = typeof(ConfigData).GetProperty(propName);

                    var breakLineAttr = propInfo?.GetCustomAttribute<BreakLineAttribute>();
                    if (breakLineAttr != null)
                    {
                        string breakLine = breakLineAttr.BreakLine;

                        if (breakLine.Contains("{space}"))
                        {
                            breakLine = breakLine.Replace("{space}", "").Trim();

                            if (breakLineAttr.BreakLine.StartsWith("{space}"))
                            {
                                newLines.Add("");
                            }

                            newLines.Add("// " + breakLine);
                            newLines.Add("");
                        }
                        else
                        {
                            newLines.Add("// " + breakLine);
                        }
                    }

                    var commentAttr = propInfo?.GetCustomAttribute<CommentAttribute>();
                    if (commentAttr != null)
                    {
                        var commentLines = commentAttr.Comment.Split('\n');
                        foreach (var commentLine in commentLines)
                        {
                            newLines.Add("// " + commentLine.Trim());
                        }
                    }

                    isPropertyLine = true;
                }

                newLines.Add(line);

                if (isPropertyLine && propInfo?.GetCustomAttribute<CommentAttribute>() != null)
                {
                    newLines.Add("");
                }
            }

            var adjustedLines = new List<string>();
            foreach (var line in newLines)
            {
                adjustedLines.Add(line);
                if (Regex.IsMatch(line, @"^\s*\],?\s*$"))
                {
                    adjustedLines.Add("");
                }
            }

            File.WriteAllText(_configFilePath, string.Join(Environment.NewLine, adjustedLines), Encoding.UTF8);
        }

        public class ConfigData
        {
            private string? _Version;
            private string? _Link;
            [BreakLine("----------------------------[ ↓ Plugin Info ↓ ]----------------------------{space}")]
            public string Version
            {
                get => _Version!;
                set
                {
                    _Version = value;
                    if (_Version != GameManagerGoldKingZ.Instance.ModuleVersion)
                    {
                        Version = GameManagerGoldKingZ.Instance.ModuleVersion;
                    }
                }
            }

            public string Link
            {
                get => _Link!;
                set
                {
                    _Link = value;
                    if (_Link != "https://github.com/oqyh/cs2-Game-Manager-GoldKingZ")
                    {
                        Link = "https://github.com/oqyh/cs2-Game-Manager-GoldKingZ";
                    }
                }
            }

            [BreakLine("{space}----------------------------[ ↓ Block/Hide Config ↓ ]----------------------------{space}")]
            [Comment("Disable Players Radio?\ntrue = Yes\nfalse = No")]
            public bool DisableRadio { get; set; }

            [Comment("Disable Bot Radio?\ntrue = Yes\nfalse = No")]
            public bool DisableBotRadio { get; set; }

            [Comment("Disable Chat Wheel?\ntrue = Yes\nfalse = No")]
            public bool DisableChatWheel { get; set; }

            [Comment("Disable Players Ping?\ntrue = Yes\nfalse = No")]
            public bool DisablePing { get; set; }

            [Comment("Disable Players Radio When Throw Grenades?\ntrue = Yes\nfalse = No")]
            public bool DisableGrenadeRadio { get; set; }

            [Comment("Disable Players Radar?\ntrue = Yes\nfalse = No")]
            public bool DisableRadar { get; set; }

            [Comment("Disable Players Fall Damage?\ntrue = Yes\nfalse = No")]
            public bool DisableFallDamage  { get; set; }

            [Comment("Disable sv_cheats?\ntrue = Yes (Will Force sv_cheats To Be Disabled)\nfalse = No (Ignore sv_cheats)")]
            public bool DisableSvCheats_1  { get; set; }

            [Comment("Disable C4 In The Game?\ntrue = Yes\nfalse = No")]
            public bool DisableC4  { get; set; }

            [Comment("Disable Blood And HeadShot Spark Decals/Effects?\ntrue = Yes\nfalse = No")]
            public bool DisableBloodAndHsSpark  { get; set; }

            [Comment("Disable Killfeed?:\n0 = No\n1 = Yes, Disable Killfeed Completely\n2 = Yes, Disable Players Killfeed And Show Who I Killed Only")]
            [Range(0, 2, 0, "[Game Manager] DisableKillfeed: is invalid, setting to default value (0) Please Choose From 0 To 2.\n[Game Manager] 1 = Yes, Disable Killfeed Completely\n[Game Manager] 2 = Yes, Disable Players Killfeed And Show Who I Killed Only\n[Game Manager] 0 = No")]
            public int DisableKillfeed { get; set; }

            [Comment("Disable TeamMate Head Tag Names?:\n0 = No\n1 = Yes, Disable It Completely But Show Names By Aiming\n2 = Yes, Only Disable It When Players Behind Walls\n3 = Yes, Disable It By Distance [DisableTeamMateHeadTag_Distance] But Show Gradually When Im Closer To Player")]
            [Range(0, 3, 0, "[Game Manager] DisableTeamMateHeadTag: is invalid, setting to default value (0) Please Choose From 0 To 3.\n[Game Manager] 0 = No\n[Game Manager] 1 = Yes, Disable It Completely But Show Names By Aiming\n[Game Manager] 2 = Yes, Only Disable It When Players Behind Walls\n[Game Manager] 3 = Yes, Disable It By Distance [DisableTeamMateHeadTag_Distance] But Show Gradually When Im Closer To Player")]
            public int DisableTeamMateHeadTag { get; set; }

            [Comment("Required [DisableTeamMateHeadTag = 3]\nShow TeamMate Head Tag Names Gradually When Im :\n50 = Very Close\n150 = Close (Recommended)\n250 = Far")]
            public int DisableTeamMateHeadTag_Distance { get; set; }

            [Comment("Hide Dead Body?:\n0 = No\n1 = Yes, After Death Immediately\n2 = Yes, After Death With Delay [HideDeadBody_Delay]\n3 = Yes, After Death Decay Body (Not Recommended For Performance)")]
            [Range(0, 3, 0, "[Game Manager] HideDeadBody: is invalid, setting to default value (0) Please Choose From 0 To 3.\n[Game Manager] 0 = No\n[Game Manager] 1 = Yes, After Death Immediately\n[Game Manager] 2 = Yes, After Death With Delay [HideDeadBody_Delay]\n[Game Manager] 3 = Yes, After Death Decay Body (Not Recommended For Performance)")]
            public int HideDeadBody { get; set; }

            [Comment("Required [HideDeadBody = 2]\nHow Much Delay (In Secs)")]
            public float HideDeadBody_Delay { get; set; }

            [Comment("Hide Legs?\ntrue = Yes\nfalse = No")]
            public bool HideLegs { get; set; }

            [Comment("Hide Chat HUD?:\n0 = No\n1 = Yes\n2 = Yes, But Delay [HideChatHUD_Delay] With Message Warning")]
            [Range(0, 2, 0, "[Game Manager] HideChatHUD: is invalid, setting to default value (0) Please Choose From 0 To 2.\n[Game Manager] 0 = No\n[Game Manager] 1 = Yes\n[Game Manager] 2 = Yes, But Delay [HideChatHUD_Delay] With Message Warning")]
            public int HideChatHUD { get; set; }

            [Comment("Required [HideChatHUD = 2]\nHow Much Delay (In Secs)")]
            public float HideChatHUD_Delay { get; set; }

            [Comment("Hide Weapons HUD (Right Side Weapons Icons)?\ntrue = Yes\nfalse = No")]
            public bool HideWeaponsHUD { get; set; }
            [BreakLine("{space}----------------------------[ ↓ Sounds Config ↓ ]----------------------------{space}")]

            [Comment("Mute HeadShot Hit Sounds?\ntrue = Yes\nfalse = No")]
            public bool Sounds_MuteHeadShot { get; set; }

            [Comment("Mute BodyShots Hit Sounds?\ntrue = Yes\nfalse = No")]
            public bool Sounds_MuteBodyShot { get; set; }

            [Comment("Mute After Player Death Voice Sounds?\ntrue = Yes\nfalse = No")]
            public bool Sounds_MutePlayerDeathVoice { get; set; }

            [Comment("Mute After Player Death Crackling Sounds?\ntrue = Yes\nfalse = No")]
            public bool Sounds_MuteAfterDeathCrackling { get; set; }

            [Comment("Mute When Switch Semi To Auto Or Opposite Sounds?\ntrue = Yes\nfalse = No")]
            public bool Sounds_MuteSwitchModeSemiToAuto { get; set; }

            [Comment("Mute MVP Music?\ntrue = Yes\nfalse = No")]
            public bool Sounds_MuteMVPMusic { get; set; }

            [Comment("Mute Players FootSteps Sounds?\ntrue = Yes\nfalse = No")]
            public bool Sounds_MutePlayersFootSteps { get; set; }

            [Comment("Mute Players Jump Land Sounds?\ntrue = Yes\nfalse = No")]
            public bool Sounds_MuteJumpLand { get; set; }

            [Comment("Mute Players Knife Stab Sounds?:\n0 = No\n1 = Yes Completely\n2 = Yes, Only On TeamMates (Restart Server Needed)")]
            [Range(0, 2, 0, "[Game Manager] Sounds_MuteKnifeStab: is invalid, setting to default value (0) Please Choose From 0 To 2.\n[Game Manager] 0 = No\n[Game Manager] 1 = Yes Completely\n[Game Manager] 2 = Yes, Only On TeamMates (Restart Server Needed)")]
            public int Sounds_MuteKnifeStab { get; set; }

            [Comment("Mute Players Gun Shots Sounds?:\n0 = No\n1 = Yes Completely\n2 = Yes, But Replace It With M4 Silencer\n3 = Yes, But Replace It With Usp Silencer\n4 = Yes, But Replace It With Custom Sounds [Sounds_MuteGunShots_weapon_id][Sounds_MuteGunShots_sound_type][Sounds_MuteGunShots_item_def_index]")]
            [Range(0, 4, 0, "[Game Manager] Sounds_MuteKnifeStab: is invalid, setting to default value (0) Please Choose From 0 To 4.\n[Game Manager] 0 = No\n[Game Manager] 1 = Yes Completely\n[Game Manager] 2 = Yes, But Replace It With M4 Silencer\n[Game Manager] 3 = Yes, But Replace It With Usp Silencer\n[Game Manager] 4 = Yes, But Replace It With Custom Sounds [Sounds_MuteGunShots_weapon_id][Sounds_MuteGunShots_sound_type][Sounds_MuteGunShots_item_def_index]")]
            public int Sounds_MuteGunShots { get; set; }

            [Comment("Required [Sounds_MuteGunShots = 4]\nWhats weapon_id")]
            public uint Sounds_MuteGunShots_weapon_id { get; set; }

            [Comment("Required [Sounds_MuteGunShots = 4]\nWhats sound_type")]
            public int Sounds_MuteGunShots_sound_type  { get; set; }

            [Comment("Required [Sounds_MuteGunShots = 4]\nWhats item_def_index")]
            public uint Sounds_MuteGunShots_item_def_index { get; set; }

            [Comment("Which Weapons Do You Want To Mute On Drop Weaponds:\nA = C4\nB = Pistol And Taser\nC = Shotguns\nD = SMGs\nE = AssaultRifles\nF = Snipers\nG = Flash And Decoy\nH = Smoke And Incendiary Grenade\nI = HE Grenade\nJ = Molotov\nK = Knife\nExample How to Use \"Sounds_MuteDropWeapons\": \"ABCDEF\"\n\"\" = None")]
            public string Sounds_MuteDropWeapons { get; set; }

            [BreakLine("{space}----------------------------[ ↓ Default Messages Config ↓ ]----------------------------{space}")]
            
            [Comment("Ignore Bomb Planted HUD Messages And Sound?\ntrue = Yes (Restart Server Needed)\nfalse = No")]
            public bool Ignore_BombPlantedHUDMessages  { get; set; }

            [Comment("Ignore TeamMate Attack Messages?\ntrue = Yes (Restart Server Needed)\nfalse = No")]
            public bool Ignore_TeamMateAttackMessages  { get; set; }

            [Comment("Ignore Awards Money Messages?\ntrue = Yes (Restart Server Needed)\nfalse = No")]
            public bool Ignore_AwardsMoneyMessages  { get; set; }

            [Comment("Ignore Saved You By Player Messages?\ntrue = Yes (Restart Server Needed)\nfalse = No")]
            public bool Ignore_PlayerSavedYouByPlayerMessages  { get; set; }

            [Comment("Ignore You Chicken Has Been Killed Messages?\ntrue = Yes (Restart Server Needed)\nfalse = No")]
            public bool Ignore_ChickenKilledMessages  { get; set; }

            [Comment("Ignore Join Team Messages?\ntrue = Yes (Restart Server Needed)\nfalse = No")]
            public bool Ignore_JoinTeamMessages  { get; set; }

            [Comment("Ignore [PLANTING!] When Player Start Planting Bomb Messages?\ntrue = Yes (Restart Server Needed)\nfalse = No")]
            public bool Ignore_PlantingBombMessages  { get; set; }

            [Comment("Ignore [DEFUSING!] When Player Start Defusing Bomb Messages?\ntrue = Yes (Restart Server Needed)\nfalse = No")]
            public bool Ignore_DefusingBombMessages  { get; set; }

            [Comment("Ignore Disconnect Messages?:\n0 = No\n1 = Yes Completely\n2 = Yes, Also Remove Disconnect Icon In Killfeed")]
            [Range(0, 2, 0, "[Game Manager] Ignore_DisconnectMessages: is invalid, setting to default value (0) Please Choose From 0 To 2.\n[Game Manager] 0 = No\n[Game Manager] 1 = Yes Completely\n[Game Manager] 2 = Yes, Also Remove Disconnect Icon In Killfeed")]
            public int Ignore_DisconnectMessages { get; set; }
            
            [BreakLine("{space}----------------------------[ ↓ Custom Messages Config ↓ ]----------------------------{space}")]

            [Comment("Custom Join Team Messages?:\n0 = No\n1 = Yes, But Exclude Bots\n2 = Yes, But Include Bots")]
            [Range(0, 2, 0, "[Game Manager] Custom_JoinTeamMessages: is invalid, setting to default value (0) Please Choose From 0 To 2.\n[Game Manager] 0 = No\n[Game Manager] 1 = Yes, But Exclude Bots\n[Game Manager] 2 = Yes, But Include Bots")]
            public int Custom_JoinTeamMessages { get; set; }

            [Comment("Custom Throw Grenades Messages?:\n0 = No\n1 = Yes, But Exclude Bots\n2 = Yes, But Include Bots\n3 = Yes, But Hide Nade Message From All When (mp_teammates_are_enemies true)\n4 = Yes, But Show Nade Message To All When (mp_teammates_are_enemies true)")]
            [Range(0, 4, 0, "[Game Manager] Custom_ThrowNadeMessages: is invalid, setting to default value (0) Please Choose From 0 To 4.\n[Game Manager] 0 = No\n[Game Manager] 1 = Yes, But Exclude Bots\n[Game Manager] 2 = Yes, But Include Bots\n[Game Manager] 3 = Yes, But Hide Nade Message From All When (mp_teammates_are_enemies true)\n[Game Manager] 4 = Yes, But Show Nade Message To All When (mp_teammates_are_enemies true)")]
            public int Custom_ThrowNadeMessages { get; set; }

            [Comment("Custom Chat Messages (config/chat_processor.json)?\ntrue = Yes\nfalse = No")]
            public bool Custom_ChatMessages  { get; set; }

            [Comment("Required [Custom_ChatMessages = true]\nExclude Custom Chat Messages If It Start With")]
            public List<string> Custom_ChatMessages_ExcludeStartWith { get; set; }

            [BreakLine("{space}----------------------------[ ↓ Auto Clean Drop Weapons Config ↓ ]----------------------------{space}")]
            
            [Comment("Enable Auto Clean Drop Weapons?\ntrue = Yes\nfalse = No")]
            public bool AutoClean_Enable  { get; set; }
            
            [Comment("Required [AutoClean_Enable = true]\nStart Clean Weapons When There Is X WeaponsInGround")]
            public int AutoClean_WhenXWeaponsInGround { get; set; }

            [Comment("Required [AutoClean_Enable = true]\nif [AutoClean_WhenXWeaponsInGround] Pass Which Method Do We Use To Gound Weapons?:\n1 = Remove All Weapons At The Same Time\n2 = Remove Weapons One By One From Oldest\n3 = Remove Newest Weapons")]
            [Range(1, 3, 2, "[Game Manager] AutoClean_DropWeapons: is invalid, setting to default value (2) Please Choose From 1 To 3.\n[Game Manager] 1 = Remove All Weapons At The Same Time\n[Game Manager] 2 = Remove Weapons One By One From Oldest\n[Game Manager] 3 = Remove Newest Weapons")]
            public int AutoClean_DropWeapons { get; set; }

            [Comment("Required [AutoClean_Enable = true]\nWhats Inside AutoClean_TheseDroppedWeaponsOnly will be Auto Deleted\nA = (weapon_awp, weapon_g3sg1, weapon_scar20, weapon_ssg08)\nB = (weapon_ak47, weapon_aug, weapon_famas, weapon_galilar, weapon_m4a1_silencer, weapon_m4a1, weapon_sg556)\nC = (weapon_m249, weapon_negev)\nD = (weapon_mag7, weapon_nova, weapon_sawedoff, weapon_xm1014)\nE = (weapon_bizon, weapon_mac10, weapon_mp5sd, weapon_mp7, weapon_mp9, weapon_p90, weapon_ump45)\nF = (weapon_cz75a, weapon_deagle, weapon_elite, weapon_fiveseven, weapon_glock, weapon_hkp2000, weapon_p250, weapon_revolver, weapon_tec9, weapon_usp_silencer)\nG = (weapon_smokegrenade, weapon_hegrenade, weapon_flashbang, weapon_decoy, weapon_molotov, weapon_incgrenade)\nH = (item_defuser, item_cutters)\nI = (weapon_taser)\nJ = (weapon_healthshot)\nK = (weapon_knife, weapon_knife_t)\nANY = Means All Weapons\nOr You Can Add Weapon Name Manually Like This (Example \"AutoCleanTheseDroppedWeaponsOnly\": \"A,B,weapon_taser,weapon_healthshot,weapon_knife\")")]
            public string AutoClean_TheseDroppedWeaponsOnly { get; set; }

            [BreakLine("{space}----------------------------[ ↓ Utilities  ↓ ]----------------------------{space}")]

            [Comment("Enable Debug Plugin In Server Console (Helps You To Debug Issues You Facing)?\ntrue = Yes\nfalse = No")]
            public bool EnableDebug { get; set; }
            
            public ConfigData()
            {
                Version = GameManagerGoldKingZ.Instance.ModuleVersion;
                Link = "https://github.com/oqyh/cs2-Game-Manager-GoldKingZ";

                DisableRadio = false;
                DisableBotRadio = false;
                DisableChatWheel = false;
                DisablePing = false;
                DisableGrenadeRadio = false;
                DisableRadar = false;
                DisableFallDamage = false;
                DisableSvCheats_1 = false;
                DisableC4 = false;
                DisableBloodAndHsSpark = false;
                DisableKillfeed = 0;
                DisableTeamMateHeadTag = 0;
                DisableTeamMateHeadTag_Distance = 150;
                HideDeadBody = 0;
                HideDeadBody_Delay = 10.0f;
                HideLegs = false;
                HideChatHUD = 0;
                HideChatHUD_Delay = 10.0f;
                HideWeaponsHUD = false;

                Sounds_MuteHeadShot = false;
                Sounds_MuteBodyShot = false;
                Sounds_MutePlayerDeathVoice = false;
                Sounds_MuteAfterDeathCrackling = false;
                Sounds_MuteSwitchModeSemiToAuto = false;
                Sounds_MuteMVPMusic = false;
                Sounds_MutePlayersFootSteps = false;
                Sounds_MuteJumpLand = false;
                Sounds_MuteKnifeStab = 0;
                Sounds_MuteGunShots = 0;
                Sounds_MuteGunShots_weapon_id = 0;
                Sounds_MuteGunShots_sound_type = 9;
                Sounds_MuteGunShots_item_def_index = 61;
                Sounds_MuteDropWeapons = "";

                Ignore_BombPlantedHUDMessages = false;
                Ignore_TeamMateAttackMessages = false;
                Ignore_AwardsMoneyMessages = false;
                Ignore_PlayerSavedYouByPlayerMessages = false;
                Ignore_ChickenKilledMessages = false;
                Ignore_JoinTeamMessages = false;
                Ignore_PlantingBombMessages = false;
                Ignore_DefusingBombMessages = false;
                Ignore_DisconnectMessages = 0;

                Custom_JoinTeamMessages = 0;
                Custom_ThrowNadeMessages = 0;
                Custom_ChatMessages = false;
                Custom_ChatMessages_ExcludeStartWith = new List<string>
                {
                    "!",".","/","rtv"
                };

                AutoClean_Enable = false;
                AutoClean_WhenXWeaponsInGround = 5;
                AutoClean_DropWeapons = 2;
                AutoClean_TheseDroppedWeaponsOnly = "A,B,C,D,E,F,weapon_hegrenade";

                EnableDebug = false;
            }
            public void Validate()
            {
                foreach (var prop in GetType().GetProperties())
                {
                    var rangeAttr = prop.GetCustomAttribute<RangeAttribute>();
                    if (rangeAttr != null && prop.PropertyType == typeof(int))
                    {
                        int value = (int)prop.GetValue(this)!;
                        if (value < rangeAttr.Min || value > rangeAttr.Max)
                        {
                            prop.SetValue(this, rangeAttr.Default);
                            Helper.DebugMessage(rangeAttr.Message,false);
                        }
                    }
                }
            }
        }
    }
}