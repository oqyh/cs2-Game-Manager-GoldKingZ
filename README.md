## .:[ Join Our Discord For Support ]:.

<a href="https://discord.com/invite/U7AuQhu"><img src="https://discord.com/api/guilds/651838917687115806/widget.png?style=banner2"></a>

# [CS2] Game-Manager-GoldKingZ (2.1.1)

Block/Hide Unnecessaries In Game

![decay](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/6960136b-4aef-467e-b1ad-e4ec8c6baf8a)
![teamattack](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/09beefa3-8431-4325-9352-9e2451b0d234)
![blockradio](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/26efd5d8-3c3f-44c1-a0e6-43c6ce2157b8)
![hidechat](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/1b5e2e57-3936-416f-895b-02731780e577)
![dm](https://github.com/user-attachments/assets/8e7e1631-bd94-4f8c-be22-20e3175eddec)
![reward](https://github.com/user-attachments/assets/6964f35e-daa9-4132-9d47-52dfd1947abf)


---

## 📦 Dependencies

[![Metamod:Source](https://img.shields.io/badge/Metamod:Source-2d2d2d?logo=sourceengine)](https://www.sourcemm.net)

[![CounterStrikeSharp](https://img.shields.io/badge/CounterStrikeSharp-83358F)](https://github.com/roflmuffin/CounterStrikeSharp)


[![MySQL](https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=white)](https://dev.mysql.com/doc/connector-net/en/) [Included in zip]

[![JSON](https://img.shields.io/badge/JSON-000000?logo=json)](https://www.newtonsoft.com/json) [Included in zip]

[![GeoLite2-City.mmdb](https://img.shields.io/badge/GeoLite2--City.mmdb-181717?logo=github&logoColor=white)](https://github.com/P3TERX/GeoLite.mmdb) [Included in zip]

[![MaxMind.Db](https://img.shields.io/badge/MaxMind.Db-2A4365?logo=database&logoColor=white)](https://www.nuget.org/packages/MaxMind.Db) [Included in zip]

[![MaxMind.GeoIP2](https://img.shields.io/badge/MaxMind.GeoIP2-2A4365?logo=database&logoColor=white)](https://www.nuget.org/packages/MaxMind.GeoIP2) [Included in zip]

---

## 📥 Installation

### Plugin Installation
1. Download the latest `Game-Manager-GoldKingZ.x.x.x.zip` release
2. Extract contents to your `csgo` directory
3. Configure settings in `Game-Manager-GoldKingZ/config/config.json`
4. Restart your server

---

## ⚙️ Configuration

> [!IMPORTANT]
> **Main Configuration**  
> `../Game-Manager-GoldKingZ/config/config.json`  
> **Chat Configuration**  
> `../Game-Manager-GoldKingZ/config/chat_processor.json`


## 🛠️ `config/config.json`

<details open>
<summary><b>Main Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `AutoSetPlayerLanguage` | Auto set player language based on country | `true`/`false` | - |
| `Reload_GameManager_CommandsInGame` | Commands to reload plugin | `Console_Commands: css_reloadgamemanager,css_reloadgm | Chat_Commands:` | - |
| `Reload_GameManager_Flags` | Restricted flags for reload command | `SteamIDs: 76561198206086993,STEAM_0:1:507335558 | Flags: @css/root,@css/admin | Groups: #css/root,#css/admin` | - |
| `Reload_GameManager_Hide` | Hide chat after reload command | `0`-No<br>`1`-Only after success<br>`2`-Always hide | - |

</details>

<details>
<summary><b>Block Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `BlockRadio` | Block Players Radio | `true`/`false` | - |
| `BlockBotRadio` | Block Bot Radio | `true`/`false` | - |
| `BlockGrenadesRadio` | Block Radio When Throwing Grenades | `true`/`false` | - |
| `BlockChatWheel` | Block Chat Wheel | `true`/`false` | - |
| `BlockPing` | Block Players Ping | `true`/`false` | - |
| `BlockNameChanger` | Block animated name changers | `0`-No<br>`1`-Send to spec with warning<br>`2`-Send to spec + execute command after delay | - |
| `BlockNameChanger_Block` | Block duration (seconds) | e.g. `10` | `BlockNameChanger=1 or 2` |
| `BlockNameChanger_SendServerConsoleCommand` | Command after block timer | Placeholders: `{PLAYER_NAME}`, `{PLAYER_ID}`, etc. | `BlockNameChanger=2` |
| `Block_Commands_StartWith` | Block commands starting with | Array of strings | - |
| `Block_Commands_StartWith_IgnoreCase` | Ignore case for start-with | `true`/`false` | - |
| `Block_Commands_Contains` | Block commands containing | Array of strings | - |
| `Block_Commands_Contains_IgnoreCase` | Ignore case for contains | `true`/`false` | - |
| `Block_Commands_Ignore_Flags` | Ignore flags for command blocking | `SteamIDs: | Flags: | Groups:` | - |

</details>

<details>
<summary><b>Hide Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `HideRadar` | Hide Players Radar | `true`/`false` | - |
| `HideKillfeed` | Hide Killfeed | `0`-No<br>`1`-Hide completely<br>`2`-Show only my kills | - |
| `HideBloodAndHsSpark` | Hide Blood/Headshot Effects | `true`/`false` | - |
| `HideTeamMateHeadTag` | Hide Teammate Head Tags | `0`-No<br>`1`-Disable completely<br>`2`-Disable behind walls<br>`3`-Disable by distance | - |
| `HideTeamMateHeadTag_Distance` | Head Tag Visibility Distance | `50`-Very close<br>`150`-Close<br>`250`-Far | `HideTeamMateHeadTag=3` |
| `HideDeadBody` | Hide Dead Bodies | `0`-No<br>`1`-Immediately<br>`2`-After delay<br>`3`-Decay body | - |
| `HideDeadBody_Delay` | Body Hide Delay (seconds) | e.g. `10` | `HideDeadBody=2` |
| `HideLegs` | Hide Player Legs | `true`/`false` | - |
| `HideChatHUD` | Hide Chat HUD | `0`-No<br>`1`-Yes<br>`2`-Yes with delay | - |
| `HideChatHUD_Delay` | Chat Hide Delay (seconds) | e.g. `10` | `HideChatHUD=2` |
| `HideWeaponsHUD` | Hide Weapons Icons | `true`/`false` | - |

</details>

<details>
<summary><b>Disable Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `DisableFallDamage` | Disable Players Fall Damage | `true`/`false` | - |
| `DisableSvCheats_1` | Force-disable sv_cheats | `true`/`false` | - |
| `DisableC4` | Disable C4 In Game | `true`/`false` | - |
| `DisableCameraSpectator` | Disable spectator camera transitions | `true`/`false` | - |
| `DisableAimPunch` | Disable screen shake when damaged | `0`-No<br>`1`-Yes<br>`2`-Togglable (enabled by default)<br>`3`-Togglable (disabled by default) | - |
| `DisableAimPunch_CommandsInGame` | Toggle commands for aim punch | `Console_Commands: css_aim,css_aimpunch | Chat_Commands:` | `DisableAimPunch=2 or 3` |
| `DisableAimPunch_Flags` | Restricted flags for aim punch toggle | `SteamIDs: | Flags: | Groups:` | `DisableAimPunch=2 or 3` |
| `DisableAimPunch_Hide` | Hide chat after aim punch toggle | `0`-No<br>`1`-Only after success<br>`2`-Always hide | `DisableAimPunch=2 or 3` |

</details>

<details>
<summary><b>Sounds Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `Sounds_MuteMVPMusic` | Mute MVP music | `0`-No<br>`1`-MVP music only<br>`2`-MVP + round end music | - |
| `Sounds_MutePlayersFootSteps` | Mute footsteps | `true`/`false` | - |
| `Sounds_MuteJumpLand` | Mute jump land sounds | `true`/`false` | - |
| `Sounds_MuteKnife` | Mute knife stab sounds | `0`-No<br>`1`-Completely<br>`2`-Only on teammates | - |
| `Sounds_MuteKnife_SoundeventHash` | Soundevent hashes for knife mute | Array of numbers | `Sounds_MuteKnife=1 or 2` |
| `Sounds_MuteGunShots` | Mute gunshot sounds | `0`-No<br>`1`-Completely<br>`2`-Replace with M4 silencer<br>`3`-Replace with USP silencer<br>`4`-Custom replacement | - |
| `Sounds_MuteGunShots_weapon_id` | Custom gun sound: weapon ID | Number (e.g. `0`) | `Sounds_MuteGunShots=4` |
| `Sounds_MuteGunShots_sound_type` | Custom gun sound: type | Number (e.g. `9`) | `Sounds_MuteGunShots=4` |
| `Sounds_MuteGunShots_item_def_index` | Custom gun sound: item index | Number (e.g. `61`) | `Sounds_MuteGunShots=4` |
| `Custom_MuteSounds1` | Custom mute sounds 1 | `0`-No<br>`1`-Yes<br>`2`-Togglable (enabled)<br>`3`-Togglable (disabled) | - |
| `Custom_MuteSounds1_SoundeventHash_Global_Side` | Global soundevent hashes | Array of numbers | `Custom_MuteSounds1=1` |
| `Custom_MuteSounds1_SoundeventHash_Victim_Side` | Victim-side soundevent hashes | Array of numbers | `Custom_MuteSounds1=2 or 3` |
| `Custom_MuteSounds1_SoundeventHash_Attacker_Side` | Attacker-side soundevent hashes | Array of numbers | `Custom_MuteSounds1=2 or 3` |
| `Custom_MuteSounds1_CommandsInGame` | Toggle commands | `Console_Commands: | Chat_Commands:` | `Custom_MuteSounds1=2 or 3` |
| `Custom_MuteSounds1_Flags` | Restricted flags | `SteamIDs: | Flags: | Groups:` | `Custom_MuteSounds1=2 or 3` |
| `Custom_MuteSounds1_Hide` | Hide chat after toggle | `0`-No<br>`1`-Only after success<br>`2`-Always hide | `Custom_MuteSounds1=2 or 3` |
| `Custom_MuteSounds2` | Custom mute sounds 2 | `0`-No<br>`1`-Yes<br>`2`-Togglable (enabled)<br>`3`-Togglable (disabled) | - |
| `Custom_MuteSounds3` | Custom mute sounds 3 | `0`-No<br>`1`-Yes<br>`2`-Togglable (enabled)<br>`3`-Togglable (disabled) | - |

</details>

<details>
<summary><b>Default Messages Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `Ignore_BombPlantedHUDMessages` | Ignore bomb planted HUD messages/sound | `true`/`false` | - |
| `Ignore_TeamMateAttackMessages` | Ignore teammate attack messages | `true`/`false` | - |
| `Ignore_AwardsMoneyMessages` | Ignore money award messages | `true`/`false` | - |
| `Ignore_PlayerSavedYouByPlayerMessages` | Ignore "saved you" messages | `true`/`false` | - |
| `Ignore_ChickenKilledMessages` | Ignore chicken death messages | `true`/`false` | - |
| `Ignore_JoinTeamMessages` | Ignore team join messages | `true`/`false` | - |
| `Ignore_PlantingBombMessages` | Ignore "[PLANTING!]" messages | `true`/`false` | - |
| `Ignore_DefusingBombMessages` | Ignore "[DEFUSING!]" messages | `true`/`false` | - |
| `Ignore_DisconnectMessages` | Ignore disconnect messages | `0`-No<br>`1`-Completely<br>`2`-Also remove killfeed icon | - |
| `Ignore_Custom_TextMsg` | Ignore custom TextMsg messages | Array of strings | - |
| `Ignore_Custom_HintText` | Ignore custom HintText messages | Array of strings | - |
| `Ignore_Custom_RadioText` | Ignore custom RadioText messages | Array of strings | - |

</details>

<details>
<summary><b>Custom Messages Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `Custom_ChatMessages` | Enable custom chat messages (via `chat_processor.json`) | `true`/`false` | - |
| `Custom_JoinTeamMessages` | Customize team join messages | `true`-Exclude bots<br>`false`-Include bots | `Custom_ChatMessages=true` |
| `Custom_ThrowNadeMessages` | Customize grenade throw messages | `1`-Exclude bots<br>`2`-Include bots<br>`3`-Hide when (mp_teammates_are_enemies true)<br>`4`-Show when (exclude bots)<br>`5`-Show when (include bots) | `Custom_ChatMessages=true` |
| `Custom_ChatMessages_Mode` | Chat message visibility | `1`-Show to all<br>`2`-Alive can't see dead messages<br>`3`-Alive see only team dead messages | `Custom_ChatMessages=true` |
| `Custom_ChatMessages_ExcludeStartWith` | Exclude chat messages starting with prefixes | Array of strings | `Custom_ChatMessages=true` |
| `Custom_ChatMessages_ExcludeStartWith_IgnoreCase` | Ignore case for start-with | `true`/`false` | `Custom_ChatMessages=true` |
| `Custom_ChatMessages_ExcludeContains` | Exclude chat messages containing text | Array of strings | `Custom_ChatMessages=true` |
| `Custom_ChatMessages_ExcludeContains_IgnoreCase` | Ignore case for contains | `true`/`false` | `Custom_ChatMessages=true` |

</details>

<details>
<summary><b>Auto Clean Drop Weapons Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `AutoClean_Enable` | Enable auto clean dropped weapons | `true`/`false` | - |
| `AutoClean_Timer` | Check interval (seconds) | `1`-`999` | `AutoClean_Enable=true` |
| `AutoClean_MaxWeaponsOnGround` | Start cleaning when X weapons on ground | `1`-`999` | `AutoClean_Enable=true` |
| `AutoClean_TheseDroppedWeaponsOnly` | Weapons to auto clean | `A`-Snipers<br>`B`-Rifles<br>`C`-LMGs<br>`D`-Shotguns<br>`E`-SMGs<br>`F`-Pistols<br>`G`-Grenades<br>`H`-Defuse kits<br>`I`-Taser<br>`J`-Healthshot<br>`K`-Knives<br>`ANY`-All weapons<br>Or specific weapon names | `AutoClean_Enable=true` |

**Weapon Categories Key:**
- `A`: AWP, G3SG1, SCAR-20, SSG 08
- `B`: AK-47, AUG, FAMAS, Galil, M4 variants
- `C`: M249, Negev
- `D`: Mag-7, Nova, Sawed-off, XM1014
- `E`: Bizon, MAC-10, MP5, MP7, MP9, P90, UMP-45
- `F`: All pistols
- `G`: All grenades
- `H`: Defuse kits
- `I`: Zeus
- `J`: Healthshot
- `K`: Knives

</details>

<details>
<summary><b>Advanced Filters Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `Filter_Whitelist_Ips` | Whitelist IP addresses | Array of IPs | - |
| `Filter_Whitelist_URLs` | Whitelist URLs | Array of URLs | - |
| `Filter_Players_Names` | Filter player names | `0`-No<br>`1`-Check IPs<br>`2`-Check URLs<br>`3`-Check both | - |
| `Filter_Players_Chat` | Filter player chat | `0`-No<br>`1`-Check IPs<br>`2`-Check URLs<br>`3`-Check both | - |

</details>

<details>
<summary><b>Locally Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `Cookies_Enable` | Save player data locally | `0`-No<br>`1`-On disconnect<br>`2`-On map change | - |
| `Cookies_AutoRemovePlayerOlderThanXDays` | Auto delete inactive players (days) | `0`-Don't delete<br>`1`+ days | `Cookies_Enable=1 or 2` |

</details>

<details>
<summary><b>MySql Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `MySql_Enable` | Save player data to MySQL | `0`-No<br>`1`-On disconnect<br>`2`-On map change | - |
| `MySql_ConnectionTimeout` | Connection timeout (seconds) | e.g. `30` | `MySql_Enable=1 or 2` |
| `MySql_RetryAttempts` | Retry attempts on failure | e.g. `3` | `MySql_Enable=1 or 2` |
| `MySql_RetryDelay` | Delay between retries (seconds) | e.g. `2` | `MySql_Enable=1 or 2` |
| `MySql_Servers` | MySQL server configurations | Array of server objects | `MySql_Enable=1 or 2` |
| `MySql_AutoRemovePlayerOlderThanXDays` | Auto delete inactive players (days) | `0`-Don't delete<br>`1`+ days | `MySql_Enable=1 or 2` |

</details>

<details>
<summary><b>Utilities Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `AutoUpdateGeoLocation` | Auto update GeoLocation data | `true`/`false` | - |
| `EnableDebug` | Enable Debug Mode | `0`-No<br>`1`-Debug everything<br>`2`-Custom_MuteSounds only<br>`3`-Sounds_MuteGunShots only<br>`4`-Ignore_Custom messages only | - |

</details>

## 🛠️ `config/chat_processor.json`

<details open>
<summary><b>Chat Processor Config</b> (Click to expand 🔽)</summary>

## Configuration Placeholders
| Placeholder | Description |
|-------------|-------------|
| `{ClanTag_ScoreBoard}` | Clan Tag At ScoreBoard |
| `{ClanTag_Chat}` | Clan Tag At Chat |
| `{CT_ALIVE_ALL}` | CT Alive To All Message |
| `{CT_ALIVE_TEAM}` | CT Alive To TeamSide Message |
| `{CT_DEAD_ALL}` | CT Dead To All Message |
| `{CT_DEAD_TEAM}` | CT Dead To TeamSide Message |
| `{T_ALIVE_ALL}` | T Alive To All Message |
| `{T_ALIVE_TEAM}` | T Alive To TeamSide Message |
| `{T_DEAD_ALL}` | T Dead To All Message |
| `{T_DEAD_TEAM}` | T Dead To TeamSide Message |
| `{SPEC_ALL}` | Spectator To All Message |
| `{SPEC_TEAM}` | Spectator To TeamSide Message |
| `{BotTakeOver}` | Bot Take Over Message |
| `{JoinTeam_SPEC}` | Join Spectators Message |
| `{JoinTeam_CT}` | Join Counter-Terrorists Message |
| `{JoinTeam_T}` | Join Terrorists Message |
| `{Nade_Hegrenade}` | HE Grenade Throw Message |
| `{Nade_Smokegrenade}` | Smoke Grenade Throw Message |
| `{Nade_Molotov}` | Molotov Throw Message |
| `{Nade_Incgrenade}` | Incendiary Grenade Throw Message |
| `{Nade_Flashbang}` | Flashbang Throw Message |
| `{Nade_Decoy}` | Decoy Grenade Throw Message |

## Message Placeholders
| Placeholder | Description |
|-------------|-------------|
| `{PLAYER_NAME}` | Player Name |
| `{BOT_NAME}` | BOT Controlled Name |
| `{ClanTag_ScoreBoard}` | Clan Tag At ScoreBoard |
| `{ClanTag_Chat}` | Clan Tag At Chat |
| `{PLAYER_LOCATION}` | Player Location |
| `{PLAYER_MSG}` | Player Message |

## Available Colors
| Color | Code | Color | Code |
|-------|------|-------|------|
| Default | `{Default}` | White | `{White}` |
| Darkred | `{Darkred}` | Green | `{Green}` |
| LightYellow | `{LightYellow}` | LightBlue | `{LightBlue}` |
| Olive | `{Olive}` | Lime | `{Lime}` |
| Red | `{Red}` | LightPurple | `{LightPurple}` |
| Purple | `{Purple}` | Grey | `{Grey}` |
| Yellow | `{Yellow}` | Gold | `{Gold}` |
| Silver | `{Silver}` | Blue | `{Blue}` |
| DarkBlue | `{DarkBlue}` | BlueGrey | `{BlueGrey}` |
| Magenta | `{Magenta}` | LightRed | `{LightRed}` |
| Orange | `{Orange}` | Team Color | `{team_color}` |

**Special Color Notes:**
- `{team_color}` = Dynamically changes color based on team:
  - Spectator = LightPurple
  - Terrorist = Orange  
  - Counter-Terrorist = LightBlue

</details>

---


## 📜 Changelog

<details>
<summary><b>📋 View Version History</b> (Click to expand 🔽)</summary>

### [2.1.1]
- Remove Hooks On Plugin Load Avoid Duplicate
- Clean Up Code 
- Fix Toggles Configs On Reload Plugin 


### [2.1.0]
- Rework On Plugin
- Optimize On Hook UnHook
- Moved Custom_JoinTeamMessages,Custom_ThrowNadeMessages,Custom_ChatMessages_ExcludeStartWith To Custom_ChatMessages (chat_processor.json)
- Fix/Compatibility With cs2fix
- Fix Exploit On Names/Chat In Custom_ChatMessages
- Fix HideDeadBody
- Fix Ignore_DisconnectMessages 2
- Fix AutoClean_Enable Lag
- Fix On chat_processor.json Flags
- Removed AutoClean_DropWeapons
- Added Reload_GameManager_CommandsInGame
- Added Reload_GameManager_Flags
- Added Reload_GameManager_Hide
- Added Block_Commands_StartWith
- Added Block_Commands_StartWith_IgnoreCase
- Added Block_Commands_Contains_IgnoreCase
- Added Block_Commands_Ignore_Flags
- Added DisableAimPunch
- Added DisableAimPunch_CommandsInGame
- Added DisableAimPunch_Flags
- Added DisableAimPunch_Hide
- Added Custom_MuteSounds1 And MuteSounds2 And MuteSounds3
- Added Custom_MuteSounds
- Added Custom_MuteSounds_SoundeventHash_Global_Side
- Added Custom_MuteSounds_SoundeventHash_Victim_Side
- Added Custom_MuteSounds_SoundeventHash_Attacker_Side
- Added Custom_MuteSounds_CommandsInGame
- Added Custom_MuteSounds_Flags
- Added Custom_MuteSounds_Hide
- Added Multiple MySql
- Added Locally 
- Added Added AutoSetPlayerLanguage
- Added Added BlockNameChanger
- Added Added BlockNameChanger_SendServerConsoleCommand
- Added Added Block_Commands_StartWith
- Added Added Block_Commands_StartWith_IgnoreCase
- Added Added Block_Commands_Contains
- Added Added Block_Commands_Contains_IgnoreCase
- Added Added Block_Commands_Ignore_Flags
- Added Added DisableCameraSpectator
- Added Added Sounds_MuteMVPMusic 2 = MVP Music And Round End Music
- Added Added Sounds_MuteKnife_SoundeventHash
- Added Added Ignore_Custom_TextMsg
- Added Added Ignore_Custom_HintText
- Added Added Ignore_Custom_RadioText
- Added Added Custom_ChatMessages_Mode
- Added Added Custom_ChatMessages_ExcludeStartWith
- Added Added Custom_ChatMessages_ExcludeStartWith_IgnoreCase
- Added Added Custom_ChatMessages_ExcludeContains
- Added Added Custom_ChatMessages_ExcludeContains_IgnoreCase
- Added AutoClean_Timer
- Added Filter_Whitelist_Ips
- Added Filter_Whitelist_URLs
- Added Filter_Players_Names
- Added Filter_Players_Chat
- Added AutoUpdateGeoLocation
- Added EnableDebug 1 to 4
- Added In chat_processor.json
 - ClanTag_ScoreBoard
 - ClanTag_Chat
 - BotTakeOver
 - {PLAYER_NAME}
 - {BOT_NAME}
 - {ClanTag_ScoreBoard}
 - {ClanTag_Chat}
 - {PLAYER_LOCATION}
 - {PLAYER_MSG}
 - {team_color}
 
### [2.0.9]
#### **Bug Fixes**
- Fixed various bugs
- Fixed HideDeadBody issues
- Fixed DisableTeamMateHeadTag_Distance
- Fixed EnableDebug

#### **Improvements**  
- Reworked plugin for better stability  
- Added config descriptions in `config.json`  

#### **New Features**
- Added DisableTeamMateHeadTag 3 Distance
- Added DisableTeamMateHeadTag_Distance
- Added chat_processor.json
- Added Custom_ChatMessages  
- Added Custom_ChatMessages_ExcludeStartWith

#### **Removals**
- Removed Mode3_TimeXSecsDecayDeadBody 
- Removed all Toggle options:
  - Toggle_AutoRemovePlayerCookieOlderThanXDays  
  - Toggle_AutoRemovePlayerMySqlOlderThanXDays
- Removed MySQL integration

### [2.0.8]
#### **Bug Fixes**
- Fixed bot join error ("System.ArgumentException: Player with slot X not found")
- Fixed nade location placeholder `{1}` in language files

### [2.0.7]
#### **New Features**
- Added `Sounds_MutePlayersFootSteps`
- Added `Sounds_MuteDropWeapons` (K) for knife drop sounds
- Added `Ignore_PlantingBombMessages`
- Added `Ignore_DefusingBombMessages`
- Added `AutoCleanDropWeaponsOnXWeaponsInGround`

#### **Improvements**
- General code cleanup
- Removed deprecated timing modes
- Reworked weapon cleanup systems

### [2.0.6]
#### **Audio Controls**
- Added custom gunshot sound replacement (Mode 4)
- Added knife sound muting options
- Added various sound mute toggles

#### **Bug Fixes**
- Fixed dead body and glove visibility issues

### [2.0.5]
#### **New Features**
- Added `IgnoreChickenKilledMessages`
- Added `EnableDebug` option

#### **Fixes**
- Fixed glove clearing in body modes

### [2.0.4]
#### **Audio**
- Added gunshot mute modes (1-3)

#### **Messages**
- Added money award ignore options

### [2.0.3]
#### **HUD Improvements**
- Fixed leg, chat and weapon HUD modes
- Added disconnect message controls

### [2.0.2]
#### **Grenade System**
- Fixed null grenade messages
- Added Mode 4 grenade messages

### [2.0.1]
#### **Bug Fixes**
- Fixed dead body mode null exception

### [2.0.0] Major Update
#### **Core Changes**
- Upgraded to .NET 8
- Split features to separate plugins

#### **New Systems**
- Added MySQL support
- Enhanced body/hud management
- Added weapon auto-cleanup

### [1.0.8]
#### **Server Management**
- Fixed restart/rotation modes
- Added weapon cleanup timer

### [1.0.7]
- Added default restart map
- Fixed dead body interactions

### [1.0.6]
- Fixed dead body implementation

### [1.0.5]
#### **Radio/Chat**
- Added cooldown systems
- Added threshold controls

### [1.0.4]
#### **Team Management**
- Added head tag controls
- Added server rotation

### [1.0.3]
- Added server restart system

### [1.0.2]
- Added leg disable option
- Fixed message systems

### [1.0.1]
#### **Initial Features**
- Added grenade radio disable
- Added radar/money controls
- Added message ignore options

### [1.0.0]
- Initial plugin release

</details>

---
