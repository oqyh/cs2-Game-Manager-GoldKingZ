using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Localization;

namespace Game_Manager_GoldKingZ.Config
{
    public static class Configs
    {
        public static class Shared {
            public static string? CookiesModule { get; set; }
            public static IStringLocalizer? StringLocalizer { get; set; }
        }
        
        private static readonly string ConfigDirectoryName = "config";
        private static readonly string ConfigFileName = "config.json";
        private static readonly string jsonFilePath2 = "MySql_Settings.json";
        private static string? _jsonFilePath2;
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
            _jsonFilePath2 = Path.Combine(configFileDirectory, jsonFilePath2);
            Helper.CreateDefaultWeaponsJson2(_jsonFilePath2);

            _configFilePath = Path.Combine(configFileDirectory, ConfigFileName);
            if (File.Exists(_configFilePath))
            {
                _configData = JsonSerializer.Deserialize<ConfigData>(File.ReadAllText(_configFilePath), SerializationOptions);
            }
            else
            {
                _configData = new ConfigData();
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
            {
                throw new Exception("Config not yet loaded.");
            }
            string json = JsonSerializer.Serialize(configData, SerializationOptions);


            json = System.Text.RegularExpressions.Regex.Unescape(json);
            File.WriteAllText(_configFilePath, json, System.Text.Encoding.UTF8);
        }

        public class ConfigData
        {
            public bool Enable_UseMySql { get; set; }
            public bool DisableRadio { get; set; }
            public bool DisableBotRadio { get; set; }
            public bool DisableChatWheel { get; set; }
            public bool DisablePing { get; set; }
            public bool DisableGrenadeRadio { get; set; }
            public bool DisableRadar { get; set; }
            public bool DisableFallDamage  { get; set; }
            public bool DisableSvCheats  { get; set; }
            public bool DisableC4  { get; set; }
            public bool DisableBloodAndHsSpark  { get; set; }
            public int DisableKillfeedMode { get; set; }
            public int DisableTeamMateHeadTag { get; set; }
            public int DisableDeadBodyMode { get; set; }
            public float Mode2_TimeXSecsDelayRemoveDeadBody { get; set; }
            public float Mode3_TimeXSecsDecayDeadBody { get; set; }
            public int DisableLegsMode { get; set; }
            public string Toggle_DisableLegsFlags { get; set; }
            public string Toggle_DisableLegsCommandsInGame { get; set; }
            public int DisableHUDChatMode { get; set; }
            public float DisableHUDChatModeWarningTimerInSecs { get; set; }
            public string Toggle_DisableHUDChatFlags { get; set; }
            public string Toggle_DisableHUDChatCommandsInGame { get; set; }
            public int DisableHUDWeaponsMode { get; set; }
            public string Toggle_DisableHUDWeaponsFlags { get; set; }
            public string Toggle_DisableHUDWeaponsCommandsInGame { get; set; }
            public int Toggle_AutoRemovePlayerCookieOlderThanXDays { get; set; }
            public int Toggle_AutoRemovePlayerMySqlOlderThanXDays { get; set; }
            public string empty { get; set; }
            public int Sounds_MuteGunShotsMode { get; set; }
            public uint Mode4_Sounds_GunShots_weapon_id { get; set; }
            public int Mode4_Sounds_GunShots_sound_type { get; set; }
            public uint Mode4_Sounds_GunShots_item_def_index { get; set; }
            public bool Sounds_MuteMVP  { get; set; }
            public bool Sounds_MuteJumpLand { get; set; }
            public int Sounds_MuteKnifesMode { get; set; }
            public bool Sounds_MutePlayersFootSteps { get; set; }
            public bool Sounds_MuteHeadShot { get; set; }
            public bool Sounds_MuteBodyShot { get; set; }
            public bool Sounds_MutePlayerDeathVoice { get; set; }
            public bool Sounds_MuteAfterDeathCrackling { get; set; }
            public bool Sounds_MuteSwitchModeSemiToAuto { get; set; }
            public string Sounds_MuteDropWeapons { get; set; }
            public string empty1 { get; set; }
            public bool Ignore_BombPlantedHUDMessages  { get; set; }
            public bool Ignore_TeamMateAttackMessages  { get; set; }
            public bool Ignore_AwardsMoneyMessages  { get; set; }
            public bool Ignore_PlayerSavedYouByPlayerMessages  { get; set; }
            public bool Ignore_ChickenKilledMessages  { get; set; }
            public bool Ignore_JoinTeamMessages  { get; set; }
            public bool Ignore_PlantingBombMessages  { get; set; }
            public bool Ignore_DefusingBombMessages  { get; set; }
            public int Ignore_DisconnectMessagesMode { get; set; }
            
            public string empty2 { get; set; }
            public int CustomJoinTeamMessagesMode { get; set; }
            public int CustomThrowNadeMessagesMode { get; set; }
            public string empty3 { get; set; }
            public int AutoClean_DropWeaponsMode { get; set; }
            public string AutoClean_TheseDroppedWeaponsOnly { get; set; }
            public int AutoClean_WhenXWeaponsInGround { get; set; }
            public string empty4 { get; set; }
            public bool EnableDebug { get; set; }
            public string empty5 { get; set; }
            public string Information_For_You_Dont_Delete_it { get; set; }
            
            public ConfigData()
            {
                Enable_UseMySql = false;
                DisableRadio = false;
                DisableBotRadio = false;
                DisableChatWheel = false;
                DisablePing = false;
                DisableGrenadeRadio = false;
                DisableRadar = false;
                DisableFallDamage = false;
                DisableSvCheats = false;
                DisableC4 = false;
                DisableBloodAndHsSpark = false;
                DisableKillfeedMode = 0;
                DisableTeamMateHeadTag = 0;
                DisableDeadBodyMode = 0;
                Mode2_TimeXSecsDelayRemoveDeadBody = 10.0f;
                Mode3_TimeXSecsDecayDeadBody = 0.01f;
                DisableLegsMode = 0;
                Toggle_DisableLegsFlags = "@css/root,@css/admin,@css/vip,#css/admin,#css/vip";
                Toggle_DisableLegsCommandsInGame = "!hidelegs,!hideleg,!hl";
                DisableHUDChatMode = 0;
                DisableHUDChatModeWarningTimerInSecs = 15;
                Toggle_DisableHUDChatFlags = "@css/root,@css/admin,@css/vip,#css/admin,#css/vip";
                Toggle_DisableHUDChatCommandsInGame = "!hidechat,!hc";
                DisableHUDWeaponsMode = 0;
                Toggle_DisableHUDWeaponsFlags = "@css/root,@css/admin,@css/vip,#css/admin,#css/vip";
                Toggle_DisableHUDWeaponsCommandsInGame = "!hideweapons,!hideweapon,!hw";
                Toggle_AutoRemovePlayerCookieOlderThanXDays = 7;
                Toggle_AutoRemovePlayerMySqlOlderThanXDays = 7;
                empty = "----------------------------[ ↓ Default CS2 Sounds ↓ ]---------------------------------";
                Sounds_MuteGunShotsMode = 0;
                Mode4_Sounds_GunShots_weapon_id = 0;
                Mode4_Sounds_GunShots_sound_type = 9;
                Mode4_Sounds_GunShots_item_def_index = 61;
                Sounds_MuteMVP = false;
                Sounds_MuteJumpLand = false;
                Sounds_MuteKnifesMode = 0;
                Sounds_MutePlayersFootSteps = false;
                Sounds_MuteHeadShot = false;
                Sounds_MuteBodyShot = false;
                Sounds_MutePlayerDeathVoice = false;
                Sounds_MuteAfterDeathCrackling = false;
                Sounds_MuteSwitchModeSemiToAuto = false;
                Sounds_MuteDropWeapons = "";
                empty1 = "----------------------------[ ↓ Default CS2 Messages ↓ ]-------------------------------";
                Ignore_BombPlantedHUDMessages = false;
                Ignore_TeamMateAttackMessages = false;
                Ignore_AwardsMoneyMessages = false;
                Ignore_PlayerSavedYouByPlayerMessages = false;
                Ignore_ChickenKilledMessages = false;
                Ignore_JoinTeamMessages = false;
                Ignore_PlantingBombMessages = false;
                Ignore_DefusingBombMessages = false;
                Ignore_DisconnectMessagesMode = 0;
                empty2 = "----------------------------[ ↓ Custom Messages ↓ ]------------------------------------";
                CustomJoinTeamMessagesMode = 0;
                CustomThrowNadeMessagesMode = 0;
                empty3 = "----------------------------[ ↓ Auto Clean Drop Weapons ↓ ]----------------------------";
                AutoClean_DropWeaponsMode = 0;
                AutoClean_TheseDroppedWeaponsOnly = "A,B,C,D,weapon_hegrenade";
                AutoClean_WhenXWeaponsInGround = 5;
                empty4 = "----------------------------[ ↓ Debug ↓ ]----------------------------------------------";
                EnableDebug = false;
                empty5 = "----------------------------[ ↓ Info For All Configs Above ↓ ]----------------------------";
                Information_For_You_Dont_Delete_it = " Vist  [https://github.com/oqyh/cs2-Game-Manager-GoldKingZ/tree/main?tab=readme-ov-file#-configuration-] To Understand All Above";
            }
        }
    }
}