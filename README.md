# [CS2] Game-Manager (1.0.7)

### Game Manager ( Block/Hide , Messages , Ping , Radio , Connect , Disconnect , Sounds , Restart On Last Player Disconnect , Map Rotation , And More )

![3](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/76d08c47-d838-4867-8410-06b7c8249add)
![2](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/1d2c9311-3092-4c49-8198-b37d3cb65890)
![1](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/65c8b2d0-045a-46d2-b75a-a2c235fc6a26)
![4](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/138b8ff5-df2e-4c3a-a85a-f8996aeda63b)
![111](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/52c68d54-9981-4c7e-898d-1f423caa621e)

## .:[ Dependencies ]:.
[Metamod:Source (2.x)](https://www.sourcemm.net/downloads.php/?branch=master)

[CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/releases)

## .:[ Configuration ]:.
```json
{
  "DisableBotRadio": false,
  
  "DisableRadio": 0, // (1) = Disable Radio Completely || (2) = Make it Cooldown [DisableRadioThreshold] + [DisableRadioTime]
  "DisableRadioThreshold": 6, // if DisableRadio (2) How Much Threshold Radio
  "DisableRadioTime": 5, // if DisableRadio (2) Time In Sec Give Cooldown
  
  "DisableChatWheel": 0, // (1) = Disable ChatWheel Completely || (2) = Make it Cooldown [DisableChatWheelThreshold] + [DisableChatWheelTime]
  "DisableChatWheelThreshold": 6, // if DisableChatWheel (2) How Much Threshold Radio
  "DisableChatWheelTime": 5, // if DisableChatWheel (2) Time In Sec Give Cooldown
  
  "DisableGrenadeRadio": false,
  "DisablePing": false,
  "DisableKillfeed": 0, // (1) = Disable Killfeed Completely || (2) = Disable Killfeed And Show Who I Killed Only
  "DisableRadar": false,
  "DisableMoneyHUD": false,
  "DisableTeamMateHeadTag": 0, // (1) = Remove Head Tag Only Behind Wall || (2) = Remove Head Tag Completely
  "DisableWinOrLosePanel": false,
  "DisableWinOrLoseSound": false,
  "DisableJumpLandSound": false,
  "DisableMPVSound": false,
  "DisableFallDamage": false,
  "DisableLegs": false,
  "DisableSvCheats": false, // Force sv_cheats 0
  "DisableRewardMoneyMessages": false,
  "DisableDeadBody": false,
  "DisableBomb": false,

//-----------------------------------------------------------------------------------------

  "IgnoreDefaultDisconnectMessages": false,
  "IgnoreDefaultJoinTeamMessages": false,
  "IgnoreTeamMateAttackMessages": false,
  
//-----------------------------------------------------------------------------------------

  "CustomJoinTeamMessages": 0, // (1) Enable Custom Join Team Messages || (2) = Enable Custom Join Team Messages Without Bots
  
//-----------------------------------------------------------------------------------------

  //Restart The Server If (RestartWhenXPlayersInServerORLess) After (RestartXTimerInMins)
  //RestartServerMode (1) = Restart Method
  //RestartServerMode (2) = Crash Method Sometimes Restart Will Not Work Use This Method Instead
  "RestartServerMode": 0,
  "RestartXTimerInMins": 5,
  "RestartWhenXPlayersInServerORLess": 0,
  //if RestartServerMode Is (1) Which Map After Restarting Would You Like To Be
  //Using ds: Means What map list in ds_workshop_listmaps
  //Using host: To Get Any Workshop Map example https://steamcommunity.com/sharedfiles/filedetails/?id=3112654794 Means host:3112654794
  //Using Without any ds: or host: means what inside /../csgo/maps/  de_dust2 de_mirage
  "RestartServerDefaultMap": "de_dust2",
  
//-----------------------------------------------------------------------------------------

  //Rotate Maps Server If (RotationWhenXPlayersInServerORLess) After (RotationXTimerInMins)
  //RotationServerMode (1) = Get Maps From Top To Bottom In RotationServerMapList.txt
  //RotationServerMode (2) = Get Random Maps In RotationServerMapList.txt
  "RotationServerMode": 0,
  "RotationXTimerInMins": 8,
  "RotationWhenXPlayersInServerORLess": 0,
  
//-----------------------------------------------------------------------------------------
  "ConfigVersion": 1
}
```

![colors](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/4035e186-58f5-43ed-a50a-be189a21daaa)

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
	//==========================
	//    CustomJoinTeamMessages
	//{0} = Player Join Name
	//==========================
	//==========================
	//    WarningMessages
	//{0} = CooldownTimer
	//==========================
	"CustomJoinTeamMessages_SPEC": "{green}Gold KingZ {grey}| {purple}{0} {grey}is joining the {lime}Spectators",
	"CustomJoinTeamMessages_T": "{green}Gold KingZ {grey}| {purple}{0} {grey}is joining the {lime}Terrorists",
	"CustomJoinTeamMessages_CT": "{green}Gold KingZ {grey}| {purple}{0} {grey}is joining the {lime}Counter-Terrorists",
	"Radio_WarningCooldown": "{green}Gold KingZ {grey}| {darkred}Wait {0} Secs Cooldown For Spaming Radio",
	"ChatWheel_WarningCooldown": "{green}Gold KingZ {grey}| {darkred}Wait {0} Secs Cooldown For Spaming ChatWheel"
}
```

## .:[ Change Log ]:.
```
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
