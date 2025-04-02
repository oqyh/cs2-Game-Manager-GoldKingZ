## .:[ Join Our Discord For Support ]:.

<a href="https://discord.com/invite/U7AuQhu"><img src="https://discord.com/api/guilds/651838917687115806/widget.png?style=banner2"></a>

# [CS2] Game-Manager-GoldKingZ (2.0.9)

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

[![JSON](https://img.shields.io/badge/JSON-000000?logo=json)](https://www.newtonsoft.com/json) [Included in zip]

---

## 📥 Installation

### Plugin Installation
1. Download the latest `Game-Manager-GoldKingZ.x.x.x.zip` release
2. Extract contents to your `csgo` directory
3. Configure settings in `Game-Manager-GoldKingZ/config/config.json`
4. Restart your server

---

## ⚙️ Configuration

> [!NOTE]
> Located In ..\Game-Manager-GoldKingZ\config\config.json                                           
>

<details open>
<summary><b>Block/Hide Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `DisableRadio` | Disable Players Radio | `true`/`false` | - |
| `DisableBotRadio` | Disable Bot Radio | `true`/`false` | - |
| `DisableChatWheel` | Disable Chat Wheel | `true`/`false` | - |
| `DisablePing` | Disable Players Ping | `true`/`false` | - |
| `DisableGrenadeRadio` | Disable Radio When Throwing Grenades | `true`/`false` | - |
| `DisableRadar` | Disable Players Radar | `true`/`false` | - |
| `DisableFallDamage` | Disable Players Fall Damage | `true`/`false` | - |
| `DisableSvCheats_1` | Force-disable sv_cheats | `true`/`false` | - |
| `DisableC4` | Disable C4 In Game | `true`/`false` | - |
| `DisableBloodAndHsSpark` | Disable Blood/Headshot Effects | `true`/`false` | - |
| `DisableKillfeed` | Disable Killfeed | `0`-No<br>`1`-Disable completely<br>`2`-Show only my kills | - |
| `DisableTeamMateHeadTag` | Disable Teammate Head Tags | `0`-No<br>`1`-Disable completely<br>`2`-Disable behind walls<br>`3`-Disable by distance | - |
| `DisableTeamMateHeadTag_Distance` | Head Tag Visibility Distance | `50`-Very close<br>`150`-Close<br>`250`-Far | `DisableTeamMateHeadTag=3` |
| `HideDeadBody` | Hide Dead Bodies | `0`-No<br>`1`-Immediately<br>`2`-After delay<br>`3`-Decay body | - |
| `HideDeadBody_Delay` | Body Hide Delay (seconds) | e.g. `10` | `HideDeadBody=2` |
| `HideLegs` | Hide Player Legs | `true`/`false` | - |
| `HideChatHUD` | Hide Chat HUD | `0`-No<br>`1`-Yes<br>`2`-Yes with delay | - |
| `HideChatHUD_Delay` | Chat Hide Delay (seconds) | e.g. `10` | `HideChatHUD=2` |
| `HideWeaponsHUD` | Hide Weapons Icons | `true`/`false` | - |

</details>

<details>
<summary><b>Sounds Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `Sounds_MuteHeadShot` | Mute headshot sounds | `true`/`false` | - |
| `Sounds_MuteBodyShot` | Mute bodyshot sounds | `true`/`false` | - |
| `Sounds_MutePlayerDeathVoice` | Mute death voice sounds | `true`/`false` | - |
| `Sounds_MuteAfterDeathCrackling` | Mute death crackling sounds | `true`/`false` | - |
| `Sounds_MuteSwitchModeSemiToAuto` | Mute fire mode switch sounds | `true`/`false` | - |
| `Sounds_MuteMVPMusic` | Mute MVP music | `true`/`false` | - |
| `Sounds_MutePlayersFootSteps` | Mute footsteps | `true`/`false` | - |
| `Sounds_MuteJumpLand` | Mute jump land sounds | `true`/`false` | - |
| `Sounds_MuteKnifeStab` | Mute knife stab sounds | `0`-No<br>`1`-Completely<br>`2`-Only on teammates | - |
| `Sounds_MuteGunShots` | Mute gunshot sounds | `0`-No<br>`1`-Completely<br>`2`-Replace with M4 silencer<br>`3`-Replace with USP silencer<br>`4`-Custom replacement | - |
| `Sounds_MuteGunShots_weapon_id` | Custom gun sound: weapon ID | Number (e.g. `0`) | `Sounds_MuteGunShots=4` |
| `Sounds_MuteGunShots_sound_type` | Custom gun sound: type | Number (e.g. `9`) | `Sounds_MuteGunShots=4` |
| `Sounds_MuteGunShots_item_def_index` | Custom gun sound: item index | Number (e.g. `61`) | `Sounds_MuteGunShots=4` |
| `Sounds_MuteDropWeapons` | Mute weapon drop sounds | `A`-C4<br>`B`-Pistols<br>`C`-Shotguns<br>`D`-SMGs<br>`E`-Rifles<br>`F`-Snipers<br>`G`-Flash/Decoy<br>`H`-Smoke/Incendiary<br>`I`-HE Grenade<br>`J`-Molotov<br>`K`-Knife<br>Combine letters (e.g. `"ABCD"`) | - |

</details>

<details>
<summary><b>Default Messages Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `Ignore_BombPlantedHUDMessages` | Ignore bomb planted HUD messages/sound | `true`/`false` (Requires restart) | - |
| `Ignore_TeamMateAttackMessages` | Ignore teammate attack messages | `true`/`false` (Requires restart) | - |
| `Ignore_AwardsMoneyMessages` | Ignore money award messages | `true`/`false` (Requires restart) | - |
| `Ignore_PlayerSavedYouByPlayerMessages` | Ignore "saved you" messages | `true`/`false` (Requires restart) | - |
| `Ignore_ChickenKilledMessages` | Ignore chicken death messages | `true`/`false` (Requires restart) | - |
| `Ignore_JoinTeamMessages` | Ignore team join messages | `true`/`false` (Requires restart) | - |
| `Ignore_PlantingBombMessages` | Ignore "[PLANTING!]" messages | `true`/`false` (Requires restart) | - |
| `Ignore_DefusingBombMessages` | Ignore "[DEFUSING!]" messages | `true`/`false` (Requires restart) | - |
| `Ignore_DisconnectMessages` | Ignore disconnect messages | `0`-No<br>`1`-Completely<br>`2`-Also remove killfeed icon | - |

</details>


<details>
<summary><b>Auto Clean Drop Weapons Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|
| `AutoClean_Enable` | Enable auto clean dropped weapons | `true`/`false` | - |
| `AutoClean_WhenXWeaponsInGround` | Start cleaning when X weapons are on ground | Number (e.g. `5`) | `AutoClean_Enable=true` |
| `AutoClean_DropWeapons` | Cleanup method | `1`-Remove all at once<br>`2`-Remove oldest first<br>`3`-Remove newest first | `AutoClean_Enable=true` |
| `AutoClean_TheseDroppedWeaponsOnly` | Weapons to auto clean | `A`-Snipers<br>`B`-Rifles<br>`C`-LMGs<br>`D`-Shotguns<br>`E`-SMGs<br>`F`-Pistols<br>`G`-Grenades<br>`H`-Defuse kits<br>`I`-Taser<br>`J`-Healthshot<br>`K`-Knives<br>`ANY`-All weapons<br>Or specific weapon names (e.g. `"A,B,weapon_taser"`) | `AutoClean_Enable=true` |

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
<summary><b>Utilities Config</b> (Click to expand 🔽)</summary>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|  
| `EnableDebug` | Enable Debug Mode | `true`/`false` | - |  

</details>

---


## 📜 Changelog

<details>
<summary><b>📋 View Version History</b> (Click to expand 🔽)</summary>

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
