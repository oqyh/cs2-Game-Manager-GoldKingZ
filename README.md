# [CS2] Game-Manager-GoldKingZ (2.0.0)

### Block/Hide Unnecessaries In Game



![blockchatwheel](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/bbd0bd16-fb65-49f9-b008-cecb190bb4bd)

![decay](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/6960136b-4aef-467e-b1ad-e4ec8c6baf8a)

![teamattack](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/09beefa3-8431-4325-9352-9e2451b0d234)

![blockradio](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/26efd5d8-3c3f-44c1-a0e6-43c6ce2157b8)

![hidechat](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/1b5e2e57-3936-416f-895b-02731780e577)



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

  //Disable Grenade Radio
  "DisableGrenadeRadio": false,

  //Disable Radar
  "DisableRadar": false,

  //Disable Cash Awards And Money HUD
  "DisableCashAwardsAndMoneyHUD": false,

  //Disable Jump Land Sound
  "DisableJumpLandSound": false,

  //Disable Fall Damage
  "DisableFallDamage": false,

  //Disable sv_cheats ( if you enable it will make it always sv_cheats 0)
  "DisableSvCheats": false,

  //Disable C4 ( Will Remove C4 )
  "DisableC4": false,

  //Disable MPV Sound At End
  "DisableMPVSound": false,

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

//-----------------------------------------------------------------------------------------

  //Ignore Default Bomb Planted Announce
  "IgnoreDefaultBombPlantedAnnounce": false,

  //Ignore Default TeamMate Attack Messages
  "IgnoreDefaultTeamMateAttackMessages": false,

  //Ignore Default Join Team Messages
  "IgnoreDefaultJoinTeamMessages": false,

  //Ignore Default Disconnect Messages
  "IgnoreDefaultDisconnectMessages": false,

//-----------------------------------------------------------------------------------------

            //Custom Messages Located In Lang Folder

  //(1) = Custom Join Team Messages + Exclude Bots
  //(2) = Custom Join Team Messages + Include Bots
  "CustomJoinTeamMessagesMode": 0,

  //(1) = Custom Throw Nade Messages + Exclude Bots
  //(2) = Custom Throw Nade Messages  + Include Bots
  //(3) = Custom Throw Nade Messages  + Hide Nade Message From All When (mp_teammates_are_enemies true)
  "CustomThrowNadeMessagesMode": 0,

//-----------------------------------------------------------------------------------------

  //(1) = Clear Only On Every Round Start With Delay Clear Mode1_TimeXSecsDelayClean
  //(2) = Clear Only On Every Spawn Or Death With Delay Clear Mode2_TimeXSecsDelayClean
  //(3) = Clear On AnyTime With Mode3_EveryTimeXSecs
  "AutoCleanDropWeaponsMode": 0,

  //Whats Inside AutoCleanDropWeapons will be Auto Deleted
  //Add Many As You Like
  //1 = Weapons (AK,M4,Pistol, etc...)
  //2 = Grenades (Smoke,Molly, etc...)
  //3 = DefuserKit
  //4 = Taser
  //5 = HealthShot
  //6 = Knifes
  "AutoCleanTheseDroppedWeaponsOnly": "1,2,3",

  //If AutoCleanDropWeaponsMode 1 How Many In Secs
  "Mode1_TimeXSecsDelayClean": 10,

  //If AutoCleanDropWeaponsMode 2 How Many In Secs
  "Mode2_TimeXSecsDelayClean": 10,

  //If AutoCleanDropWeaponsMode 3 How Many In Secs
  "Mode3_EveryTimeXSecs": 10,

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

	"custom.hegrenade": "{green}Gold KingZ {grey}| {purple}{0} {grey}Throwed {red}☄ HE Grenade! ☄",
	"custom.smokegrenade": "{green}Gold KingZ {grey}| {purple}{0} {grey}Throwed {Olive}☁︎ Smoke! ☁︎",
	"custom.molotov": "{green}Gold KingZ {grey}| {purple}{0} {grey}Throwed {orange}♨ Molotov! ♨",
	"custom.flashbang": "{green}Gold KingZ {grey}| {purple}{0} {grey}Throwed {Blue}˗ˏˋ★ Flashbang! ★ˎˊ˗",
	"custom.incgrenade": "{green}Gold KingZ {grey}| {purple}{0} {grey}Throwed {orange} ♨ Incendiary! ♨",
	"custom.decoy": "{green}Gold KingZ {grey}| {purple}{0} {grey}Throwed {grey}✦ Decoy! ✦"
}
```


## .:[ Change Log ]:.
```
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
