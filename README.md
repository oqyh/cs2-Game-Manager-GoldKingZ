## .:[ Join Our Discord For Support ]:.
<a href="https://discord.com/invite/U7AuQhu"><img src="https://discord.com/api/guilds/651838917687115806/widget.png?style=banner2"></a>

***
# [CS2] Game-Manager-GoldKingZ (2.0.7)

### Block/Hide Unnecessaries In Game


![blockchatwheel](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/bbd0bd16-fb65-49f9-b008-cecb190bb4bd)

![decay](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/6960136b-4aef-467e-b1ad-e4ec8c6baf8a)

![teamattack](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/09beefa3-8431-4325-9352-9e2451b0d234)

![blockradio](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/26efd5d8-3c3f-44c1-a0e6-43c6ce2157b8)

![hidechat](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/1b5e2e57-3936-416f-895b-02731780e577)

![dm](https://github.com/user-attachments/assets/8e7e1631-bd94-4f8c-be22-20e3175eddec)
![reward](https://github.com/user-attachments/assets/6964f35e-daa9-4132-9d47-52dfd1947abf)


## .:[ Dependencies ]:.
[Metamod:Source (2.x)](https://www.sourcemm.net/downloads.php/?branch=master)

[CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/releases)

[Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)

[MySqlConnector](https://www.nuget.org/packages/MySqlConnector)


## .:[ Configuration ]:.

> [!CAUTION]
> Config Located In ..\addons\counterstrikesharp\plugins\Game-Manager-GoldKingZ\config\config.json                                           
>

```json
{
  //Enable MySql? Located In Game-Manager-GoldKingZ/config/MySql_Settings.json
  "Enable_UseMySql": false,

  //Disable Radio
  "DisableRadio": false,

  //Disable Bot Radio
  "DisableBotRadio": false,

  //Disable ChatWheel
  "DisableChatWheel": false,

  //Disable Ping
  "DisablePing": false,

  //Disable Grenade Radio + Message
  "DisableGrenadeRadio": false,

  //Disable Radar
  "DisableRadar": false,

  //Disable Fall Damage
  "DisableFallDamage": false,

  //Disable sv_cheats ( if you enable it will make it always sv_cheats 0)
  "DisableSvCheats": false,

  //Disable C4 ( Will Remove C4 )
  "DisableC4": false,

  //Disable Blood And HeadShot Spark Decals/Effects
  "DisableBloodAndHsSpark": false,

  //(1) = Disable Killfeed Completely
  //(2) = Disable Killfeed And Show Who I Killed Only
  "DisableKillfeedMode": 0,

  //(1) = Remove Head Tag Only Behind Wall
  //(2) = Remove Head Tag Completely
  "DisableTeamMateHeadTag": 0,

  //(1) = Remove Dead Body After Death Immediately 
  //(2) = Remove Dead Body After Death With Delay Mode2_TimeXSecsDelayRemoveDeadBody
  //(3) = Remove Dead Body After Death Decay Method Mode3_TimeXSecsDecayDeadBody
  "DisableDeadBodyMode": 0,

  //If DisableDeadBodyMode 2 How Much Time In Secs
  "Mode2_TimeXSecsDelayRemoveDeadBody": 10,

  //If DisableDeadBodyMode 3 How Much Time In Secs
  "Mode3_TimeXSecsDecayDeadBody": 0.01,

  //(1) = Disable Legs On FOV + Not Toggleable
  //(2) = Show Legs By Default On FOV + Toggleable By Toggle_DisableLegsFlags With Commands Toggle_DisableLegsCommandsInGame
  //(3) = Hide Legs By Default On FOV + Toggleable By Toggle_DisableLegsFlags With Commands Toggle_DisableLegsCommandsInGame
  "DisableLegsMode": 0,

  //If DisableLegsMode 2 or 3 Who Can Toggle It (Making Empty "" Means Everyone Has Access)
  "Toggle_DisableLegsFlags": "@css/root,@css/admin,@css/vip,#css/admin,#css/vip",

  //If DisableLegsMode 2 or 3 Which Command You Like It To Be
  "Toggle_DisableLegsCommandsInGame": "!hidelegs,!hideleg,!hl",

  //(1) = Disable Chat HUD + Not Toggleable
  //(2) = Show Chat HUD By Default + Toggleable By Toggle_DisableHUDChatFlags With Commands Toggle_DisableHUDChatCommandsInGame
  //(3) = Hide Chat HUD By Default + Toggleable By Toggle_DisableHUDChatFlags With Commands Toggle_DisableHUDChatCommandsInGame
  "DisableHUDChatMode": 0,

  //Delay Before Hide Chat HUD Give Warning 
  "DisableHUDChatModeWarningTimerInSecs": 15,

  //If DisableHUDChatMode 2 or 3 Who Can Toggle It (Making Empty "" Means Everyone Has Access)
  "Toggle_DisableHUDChatFlags": "@css/root,@css/admin,@css/vip,#css/admin,#css/vip",

  //If DisableHUDChatMode 2 or 3 Which Command You Like It To Be
  "Toggle_DisableHUDChatCommandsInGame": "!hidechat,!hc",

  //(1) = Disable Weapons HUD + Not Toggleable
  //(2) = Show Weapons HUD By Default + Toggleable By Toggle_DisableHUDWeaponsFlags With Commands Toggle_DisableHUDWeaponsCommandsInGame
  //(3) = Hide Weapons HUD By Default + Toggleable By Toggle_DisableHUDWeaponsFlags With Commands Toggle_DisableHUDWeaponsCommandsInGame
  "DisableHUDWeaponsMode": 0,

  //If DisableHUDWeaponsMode 2 or 3 Who Can Toggle It (Making Empty "" Means Everyone Has Access)
  "Toggle_DisableHUDWeaponsFlags": "@css/root,@css/admin,@css/vip,#css/admin,#css/vip",

  //If DisableHUDWeaponsMode 2 or 3 Which Command You Like It To Be
  "Toggle_DisableHUDWeaponsCommandsInGame": "!hideweapons,!hideweapon,!hw",

  //Auto Delete Inactive Players Cookies Older Than X Days plugins/Game-Manager-GoldKingZ/Game_Manager_Cookies.json
  "Toggle_AutoRemovePlayerCookieOlderThanXDays": 7,

  //Auto Delete Inactive Players Cookies Older Than X Days In MySql
  "Toggle_AutoRemovePlayerMySqlOlderThanXDays": 7,

//----------------------------[ ↓ Default CS2 Sounds ↓ ]---------------------------------

  //Make All Gun Sound:
  //(1) = Completely Mute
  //(2) = m4 Silencer
  //(3) = Usp Silencer
  //(4) = Custom Mode4_Sounds_GunShots_weapon_id , Mode4_Sounds_GunShots_sound_type , Mode4_Sounds_GunShots_item_def_index
  "Sounds_MuteGunShotsMode": 0,

  //If Sounds_MuteGunShotsMode = 4  Whats (weapon_id , sound_type , item_def_index)
  "Mode4_Sounds_GunShots_weapon_id": 0,
  "Mode4_Sounds_GunShots_sound_type": 9,
  "Mode4_Sounds_GunShots_item_def_index": 61,

  //Mute MVP Sounds
  "Sounds_MuteMVP": false,

  //Mute Jump Land Sounds
  "Sounds_MuteJumpLand": false,

  //(1) = Mute Knife Stab
  //(2) = Mute Knife Stab Only If Stab TeamMates
  "Sounds_MuteKnifesMode": 0,

  //Mute Players FootSteps Sounds
  "Sounds_MutePlayersFootSteps": false,

  //Mute HeadShot Hit Sounds
  "Sounds_MuteHeadShot": false,

  //Mute BodyShots Hit Sounds
  "Sounds_MuteBodyShot": false,

  //Mute After Player Death Voice Sounds
  "Sounds_MutePlayerDeathVoice": false,

  //Mute After Player Death Crackling Sounds
  "Sounds_MuteAfterDeathCrackling": false,

  //Mute When Switch Semi To Auto Or Opposite
  "Sounds_MuteSwitchModeSemiToAuto": false,

  //Which Weapons Do You Want To Mute On Drop Weaponds:
  //A = C4
  //B = Pistol And Taser
  //C = Shotguns
  //D = SMGs
  //E = AssaultRifles
  //F = Snipers
  //G = Flash And Decoy
  //H = Smoke And Incendiary Grenade
  //I = HE Grenade
  //J = Molotov
  //K = Knife
  //Example How to Use "Sounds_MuteDropWeapons": "ABCDEF"
  "Sounds_MuteDropWeapons": "",

//----------------------------[ ↓ Default CS2 Messages ↓ ]-------------------------------

  //Ignore Bomb Planted HUD Messages
  "Ignore_BombPlantedHUDMessages": false,

  //Ignore TeamMate Attack Messages
  "Ignore_TeamMateAttackMessages": false,

  //Ignore Awards Money Messages
  "Ignore_AwardsMoneyMessages": false,

  //Ignore Saved You By Player Messages
  "Ignore_PlayerSavedYouByPlayerMessages": false,

  //Ignore You Chicken Has Been Killed Messages
  "Ignore_ChickenKilledMessages": false,

  //Ignore Join Team Messages
  "Ignore_JoinTeamMessages": false,

  //Ignore Planting Bomb Messages
  "Ignore_PlantingBombMessages": false,

  //Ignore Defusing Bomb Messages
  "Ignore_DefusingBombMessages": false,

  //(1) = Ignore Disconnect Messages
  //(2) = Ignore Disconnect Messages + Remove Disconnect Icon In Killfeed
  "Ignore_DisconnectMessagesMode": 0,

//----------------------------[ ↓ Custom Messages ↓ ]------------------------------------

  //(1) = Custom Join Team Messages + Exclude Bots
  //(2) = Custom Join Team Messages + Include Bots
  "CustomJoinTeamMessagesMode": 0,

  //(1) = Custom Throw Nade Messages + Exclude Bots
  //(2) = Custom Throw Nade Messages  + Include Bots
  //(3) = Custom Throw Nade Messages  + Hide Nade Message From All When (mp_teammates_are_enemies true)
  //(4) = Custom Throw Nade Messages  + Show Nade Message To All When (mp_teammates_are_enemies true)
  "CustomThrowNadeMessagesMode": 0,

//----------------------------[ ↓ Auto Clean Drop Weapons ↓ ]----------------------------

  //(1) = Clear AutoClean_TheseDroppedWeaponsOnly ((All Weapons In The Map)) When AutoClean_WhenXWeaponsInGround
  //(2) = Clear AutoClean_TheseDroppedWeaponsOnly ((Oldest Weapon Only In The Map)) When AutoClean_WhenXWeaponsInGround
  //(3) = Clear AutoClean_TheseDroppedWeaponsOnly ((Newest Weapon Only In The Map)) When AutoClean_WhenXWeaponsInGround
  "AutoClean_DropWeaponsMode": 0,

  //Whats Inside AutoClean_TheseDroppedWeaponsOnly will be Auto Deleted
  //Add Many As You Like
  //A = (weapon_awp, weapon_g3sg1, weapon_scar20, weapon_ssg08)
  //B = (weapon_ak47, weapon_aug, weapon_famas, weapon_galilar, weapon_m4a1_silencer, weapon_m4a1, weapon_sg556)
  //C = (weapon_m249, weapon_negev)
  //D = (weapon_mag7, weapon_nova, weapon_sawedoff, weapon_xm1014)
  //E = (weapon_bizon, weapon_mac10, weapon_mp5sd, weapon_mp7, weapon_mp9, weapon_p90, weapon_ump45)
  //F = (weapon_cz75a, weapon_deagle, weapon_elite, weapon_fiveseven, weapon_glock, weapon_hkp2000, weapon_p250, weapon_revolver, weapon_tec9, weapon_usp_silencer)
  //G = (weapon_smokegrenade, weapon_hegrenade, weapon_flashbang, weapon_decoy, weapon_molotov, weapon_incgrenade)
  //H = (item_defuser, item_cutters)
  //I = (weapon_taser)
  //J = (weapon_healthshot)
  //K = (weapon_knife, weapon_knife_t)
  //ANY = Means All Weapons
  //Or You Can Add Weapon Name Manually Like This (Example "AutoCleanTheseDroppedWeaponsOnly": "A,B,weapon_taser,weapon_healthshot,weapon_knife")
  "AutoClean_TheseDroppedWeaponsOnly": "A,B,C,D,weapon_hegrenade",

  //Do AutoClean_DropWeaponsMode Action When X Weapons In Ground
  "AutoClean_WhenXWeaponsInGround": "1,2,3",

//----------------------------[ ↓ Debug ↓ ]----------------------------------------------
  
  //Enable Debug Will Print Server Console If You Face Any Issue
  "EnableDebug": false,
}
```

![329846165-ba02c700-8e0b-4ebe-bc28-103b796c0b2e](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/3df7caa9-34a7-47da-94aa-8d682f59e85d)


## .:[ Language ]:.
```json
{
	//==========================
	//        Colors
	//==========================
	//{Yellow} {Gold} {Silver} {Blue} {DarkBlue} {BlueGrey} {Magenta} {LightRed}
	//{LightBlue} {Olive} {Lime} {Red} {Purple} {Grey}
	//{Default} {White} {Darkred} {Green} {LightYellow}
	//==========================
	//        Other
	//==========================
	//{nextline} = Print On Next Line
	//==========================
	

	"hidechat.not.allowed": "{green}Gold KingZ {grey}| {darkred}Toggle Hide Chat HUD Is For {lime}VIPS {darkred}Only",
	"hidechat.enabled.warning": "{darkred}-------------------------------------------------------{nextline}{green}Gold KingZ {grey}| {darkred}Chat HUD Will Be Hidden After {lime}{0} Secs {nextline}{green}Gold KingZ {grey}| {darkred}Please Open The {lime}Chat Box {darkred}On Your Keyboard To See Chat Temporarily{nextline}{darkred}-------------------------------------------------------",
	"hidechat.enabled": "{green}Gold KingZ {grey}| Chat HUD Now {lime}Visible {grey}Type {yellow}!hidechat {grey}/ {yellow}!hc {grey}To Toggle On/Off",
	"hidechat.disabled": "{green}Gold KingZ {grey}| Chat HUD Now {darkred}Hidden {grey}Type {yellow}!hidechat {grey}/ {yellow}!hc {grey}To Toggle On/Off",
	
	"hidelegs.not.allowed": "{green}Gold KingZ {grey}| {darkred}Toggle Hide Legs Is For {lime}VIPS {darkred}Only",
	"hidelegs.enabled": "{green}Gold KingZ {grey}| Legs Now {lime}Visible {grey}Type {yellow}!hidelegs {grey}/ {yellow}!hideleg {grey}/ {yellow}!hl {grey}To Toggle On/Off",
	"hidelegs.disabled": "{green}Gold KingZ {grey}| Legs Now {darkred}Hidden {grey}Type {yellow}!hidelegs {grey}/ {yellow}!hideleg {grey}/ {yellow}!hl {grey}To Toggle On/Off",

	"hideweapons.not.allowed": "{green}Gold KingZ {grey}| {darkred}Toggle Hide Weapons HUD Is For {lime}VIPS {darkred}Only",
	"hideweapons.enabled": "{green}Gold KingZ {grey}| Weapons HUD Now {lime}Visible {grey}Type {yellow}!hideweapons {grey}/ {yellow}!hw {grey}To Toggle On/Off",
	"hideweapons.disabled": "{green}Gold KingZ {grey}| Weapons HUD Now {darkred}Hidden {grey}Type {yellow}!hideweapons {grey}/ {yellow}!hw {grey}To Toggle On/Off",

	"custom.jointeam.spec": "{green}Gold KingZ {grey}| {purple}{0} {grey}is joining the {lime}Spectators",
	"custom.jointeam.t": "{green}Gold KingZ {grey}| {purple}{0} {grey}is joining the {lime}Terrorists",
	"custom.jointeam.ct": "{green}Gold KingZ {grey}| {purple}{0} {grey}is joining the {lime}Counter-Terrorists",

	//==========================
	//{0} = Player Name
	//{1} = Location
	//==========================

	"custom.hegrenade": "{green}Gold KingZ {grey}| {purple}{0} {green}@ {1}{grey}: {grey}Threw {red}☄ HE Grenade! ☄",
	"custom.smokegrenade": "{green}Gold KingZ {grey}| {purple}{0} {green}@ {1}{grey}: {grey}Threw {Olive}☁︎ Smoke! ☁︎",
	"custom.molotov": "{green}Gold KingZ {grey}| {purple}{0} {green}@ {1}{grey}: {grey}Threw {orange}♨ Molotov! ♨",
	"custom.flashbang": "{green}Gold KingZ {grey}| {purple}{0} {green}@ {1}{grey}: {grey}Threw {Blue}˗ˏˋ★ Flashbang! ★ˎˊ˗",
	"custom.incgrenade": "{green}Gold KingZ {grey}| {purple}{0} {green}@ {1}{grey}: {grey}Threw {orange} ♨ Incendiary! ♨",
	"custom.decoy": "{green}Gold KingZ {grey}| {purple}{0} {green}@ {1}{grey}: {grey}Threw {grey}✦ Decoy! ✦"
}
```


## .:[ Change Log ]:.
```
(2.0.7)
-Some CleanUp
-Added Sounds_MutePlayersFootSteps
-Added Sounds_MuteDropWeapons (K) Drop Knife Sound
-Added Ignore_PlantingBombMessages
-Added Ignore_DefusingBombMessages
-Added AutoCleanDropWeaponsOnXWeaponsInGround
-Remove Mode1_TimeXSecsDelayClean
-Remove Mode2_TimeXSecsDelayClean
-Remove Mode3_EveryTimeXSecs
-Rework On AutoCleanTheseDroppedWeaponsOnly
-Rework On AutoCleanDropWeaponsMode 1
-Rework On AutoCleanDropWeaponsMode 2
-Rework On AutoCleanDropWeaponsMode 3

(2.0.6)
-Fix Some Bugs
-Fix DisableDeadBodyMode 3 System.ArgumentException: Value of '-1' is not valid for 'alpha'
-Fix DisableDeadBodyMode 1 2 3 Custom Gloves Not Disappear
-Added CustomThrowNadeMessagesMode Location Nade In lang {1}
-Added Sounds_MuteGunShotsMode 4
-Added Mode4_Sounds_GunShots_weapon_id
-Added Mode4_Sounds_GunShots_sound_type
-Added Mode4_Sounds_GunShots_item_def_index
-Added Sounds_MuteKnifesMode 1 Mute Knife Sounds (Stab RightClick + Stab LeftClick + Stab Swing Air)
-Added Sounds_MuteKnifesMode 2 Mute Knife Sounds Only On TeamMates
-Added Sounds_MuteHeadShot
-Added Sounds_MuteBodyShot
-Added Sounds_MutePlayerDeathVoice
-Added Sounds_MuteAfterDeathCrackling
-Added Sounds_MuteSwitchModeSemiToAuto
-Added Sounds_MuteDropWeapons

(2.0.5)
-Fix DisableDeadBodyMode (1) (2) (3) Gloves Not Cleared
-Added DisableBloodAndHsSpark
-Added IgnoreChickenKilledMessages
-Added EnableDebug

(2.0.4)
-Fix IgnoreDefaultTeamMateAttackMessages
-Added Mute_GunShotsMode //(1) = Completely Mute //(2) = m4 Silencer //(3) = Usp Silencer
-Added IgnoreDefaultAwardsMoneyMessages ignore rather than Disabling
-Added IgnorePlayerSavedYouByPlayerMessages
-Removed DisableCashAwardsAndMoneyHUD

(2.0.3)
-Fix DisableLegsMode
-Fix DisableHUDChatMode
-Fix DisableHUDWeaponsMode
-Added IgnoreDefaultDisconnectMessagesMode (Mode 1 / Mode 2)

(2.0.2)
-Fix Null CustomThrowNadeMessagesMode (Mode 0)
-Added CustomThrowNadeMessagesMode (Mode 4)

(2.0.1)
-Fix Null DisableDeadBodyMode (Mode 2)

(2.0.0)
-Upgrade Net.7 To Net.8
-Fix DisableMPVSound
-Removed DisableRadio Modes
-Removed DisableChatWheel Modes
-Removed DisableWinOrLoseSound (Valve Issue)
-Removed DisableWinOrLosePanel (Valve Issue)
-Removed DisableMoneyHUD (Send It To DisableCashAwardsAndMoneyHUD)
-Removed DisableRewardMoneyMessages (Send It To DisableCashAwardsAndMoneyHUD)
-Removed RestartServerMode To Saparate Plugin (https://github.com/oqyh/cs2-Auto-Restart-Server-GoldKingZ)
-Removed RotationServerMode To Saparate Plugin  (Soon)
-Rework Method DisableDeadBody
-Added Enable_UseMySql
-Added DisableCashAwardsAndMoneyHUD
-Added DisableDeadBodyMode
-Added Mode2_TimeXSecsDelayRemoveDeadBody
-Added Mode3_TimeXSecsDecayDeadBody
-Added DisableLegsMode
-Added Toggle_DisableLegsFlags
-Added Toggle_DisableLegsCommandsInGame
-Added DisableHUDChatMode
-Added DisableHUDChatModeWarningTimerInSecs
-Added Toggle_DisableHUDChatFlags
-Added Toggle_DisableHUDChatCommandsInGame
-Added DisableHUDWeaponsMode
-Added Toggle_DisableHUDWeaponsFlags
-Added Toggle_DisableHUDWeaponsCommandsInGame
-Added Toggle_AutoRemovePlayerCookieOlderThanXDays
-Added Toggle_AutoRemovePlayerMySqlOlderThanXDays
-Added IgnoreDefaultBombPlantedAnnounce
-Added CustomThrowNadeMessagesMode
-Added AutoCleanDropWeaponsMode
-Added Mode1_TimeXSecsDelayClean
-Added Mode2_TimeXSecsDelayClean
-Added Mode3_EveryTimeXSecs

(1.0.8)
-Fix RestartServerMode
-Fix RotationServerMode
-Added AutoCleanDropWeaponsTimer
-Added AutoCleanDropWeapons


(1.0.7)
-Added "RestartServerDefaultMap"
-Fix DisableDeadBody With AFK Plugin Any Plugin
-Fix RestartServerMode (1) Better Method

(1.0.6)
-Fix DisableDeadBody


(1.0.5)
-Added "DisableRadio": 0,// (1) = Disable Radio Completely || (2) = Make it Cooldown [DisableRadioThreshold] + [DisableRadioTime]
-Added "DisableRadioThreshold": 6, // if DisableRadio (2) How Much Threshold Radio
-Added "DisableRadioTime": 5, // if DisableRadio (2) Time In Sec Give Cooldown + added to lang "Radio_WarningCooldown"
  
-Added "DisableChatWheel": 0, // (1) = Disable ChatWheel Completely || (2) = Make it Cooldown [DisableChatWheelThreshold] + [DisableChatWheelTime]
-Added "DisableChatWheelThreshold": 6, // if DisableChatWheel (2) How Much Threshold Radio]
-Added "DisableChatWheelTime": 5, // if DisableChatWheel (2) Time In Sec Give Cooldown + added to lang "ChatWheel_WarningCooldown"

-Added "DisableKillfeed" // (1) = Disable Killfeed Completely || (2) = Disable Killfeed And Show Who I Killed Only
-Added "CustomJoinTeamMessages" To lang "CustomJoinTeamMessages_SPEC" "CustomJoinTeamMessages_T" "CustomJoinTeamMessages_CT"

-Added "DisableBotRadio"
-Added "DisableMPVSound"
-Added "DisableBomb"
-Added "DisableDeadBody"
-Added "IgnoreTeamMateAttackMessages"

-Fix "DisableSvCheats"

(1.0.4)
-Added "DisableTeamMateHeadTag"
-Added "DisableSvCheats"
-Added "CustomJoinTeamMessages"
-Added "CustomJoinTeamMessagesCT"
-Added "CustomJoinTeamMessagesT"
-Added "CustomJoinTeamMessagesSpec"
-Added "RotationServerMode"
-Added "RotationXTimerInMins"
-Added "RotationWhenXPlayersInServerORLess"

-Fix / Remove Some Bugs

(1.0.3)
-Added [RestartServerLastPlayerDisconnect]
-Added [RestartMethod]
-Added [RestartXTimerInMins]
-Added [RestartWhenXPlayersInServerORLess]

(1.0.2)
-Added [DisableLegs]
-Fix [IgnoreRewardMoneyMessages]
-Fix [IgnoreTeamMateAttackMessages]
-Fix [IgnorePlayerSavedYouByPlayerMessages]

(1.0.1)
-Added [DisableGrenadeRadio]
-Added [DisableRadar]
-Added [DisableMoneyHUD]
-Added [DisableJumpLandSound]
-Added [DisableFallDamage]
-Added [IgnoreRewardMoneyMessages]
-Added [IgnoreTeamMateAttackMessages]
-Added [IgnorePlayerSavedYouByPlayerMessages]

(1.0.0)
-Initial Release
```

## .:[ Donation ]:.

If this project help you reduce time to develop, you can give me a cup of coffee :)

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://paypal.me/oQYh)
