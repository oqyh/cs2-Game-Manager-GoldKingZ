# [CS2] Game-Manager (1.0.3)

### Game Manager ( Block/Hide , Messages , Ping , Radio , Connect , Disconnect , Sounds , And More )

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
  "DisableRadio": false,                            // Disable Radio
  "DisableGrenadeRadio": false,                     // Disable Throwing Grenade Radio
  "DisablePing": false,                             // Disable Player Ping
  "DisableChatWheel": false,                        // Disable Player ChatWheel
  "DisableKillfeed": false,                         // Disable Killfeed
  "DisableRadar": false,                            // Disable Radar
  "DisableMoneyHUD": false,                         // Disable Money Hud
  "DisableWinOrLosePanel": false,                   // Disable Win/Lose/DRAW Panel
  "DisableWinOrLoseSound": false,                   // Disable Win/Lose/DRAW Sound
  "DisableJumpLandSound": false,                    // Disable Jump Land Sound
  "DisableFallDamage": false,                       // Disable Fall Damage
  "DisableLegs": false,                             // Disable Legs
  "IgnoreJoinTeamMessages": false,                  // Ignore Player Join Team Messages
  "IgnoreRewardMoneyMessages": 0,                   // Ignore Player Reward Money Messages ( 1=Covar [Better Option] , 2= Remove Message Only [[Dont Put 2 Wait For CounterStrikeSharp Update OtherWise Will Crash]])
  "IgnoreTeamMateAttackMessages": false,            // Ignore Player Attack TeamMate Messages ([[Dont Make it True Wait For CounterStrikeSharp Update OtherWise Will Crash]])
  "IgnorePlayerSavedYouByPlayerMessages": false,    // Ignore Player Saved You By Player Messages ([[Dont Make it True Wait For CounterStrikeSharp Update OtherWise Will Crash]])

  "RestartServerLastPlayerDisconnect": false,       // Restart Server On Last Player Disconnect
  "RestartMethod": 1,                               // if [RestartServerLastPlayerDisconnect True] Which Method Do You Like   1= Restart    2= Crash  if 1 not working
  "RestartXTimerInMins": 5,                         // if [RestartServerLastPlayerDisconnect True] How Many In Mins To Wait Before Start [RestartMethod]
  "RestartWhenXPlayersInServerORLess": 0,           // if [RestartServerLastPlayerDisconnect True] (Bot Doesn't Count As Player) How Many Players In Server To Start [RestartMethod] 0= Means 0 Players In Server Do Restart
}
```


## .:[ Change Log ]:.
```
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
