using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Timers;
using System.Globalization;
using Newtonsoft.Json.Converters;
using System.Drawing;
using CounterStrikeSharp.API.Modules.UserMessages;
using Game_Manager_GoldKingZ.Config;
using System.Security.Cryptography;
using CounterStrikeSharp.API.Modules.Cvars;

namespace Game_Manager_GoldKingZ;
public static class Extension
{

    public static bool IsValid([NotNullWhen(true)] this CCSPlayerController? player, bool IncludeBots = false, bool IncludeHLTV = false)
    {
        if (player == null || !player.IsValid)
            return false;

        if (!IncludeBots && player.IsBot)
            return false;

        if (!IncludeHLTV && player.IsHLTV)
            return false;

        return true;
    }

    public static bool IsAlive(this CCSPlayerController? player)
    {
        if (player == null || !player.IsValid ||
        player.Pawn == null || !player.Pawn.IsValid ||
        player.Pawn.Value == null || !player.Pawn.Value.IsValid ||
        player.PlayerPawn == null || !player.PlayerPawn.IsValid ||
        player.PlayerPawn.Value == null || !player.PlayerPawn.Value.IsValid) return false;

        if (player.PlayerPawn.Value.LifeState == (byte)LifeState_t.LIFE_ALIVE || player.Pawn.Value.LifeState == (byte)LifeState_t.LIFE_ALIVE)
        {
            return true;
        }

        return false;
    }

    public static int ToggleOnOff(this int value)
    {
        return value switch
        {
            1 => -2,
            2 => -1,
            -1 => -2,
            -2 => -1,
            _ => value
        };
    }

    public static int ToDebugConfig(this int enableDebug, int mode)
    {
        if (enableDebug == 1) return 1;
        return enableDebug == mode ? mode : -1;
    }

    public static bool HasValidPermissionData(this string? groups)
    {
        if (string.IsNullOrWhiteSpace(groups)) return false;

        var segments = groups.Split('|', StringSplitOptions.RemoveEmptyEntries);
        foreach (var seg in segments)
        {
            var trimmed = seg.Trim();
            if (string.IsNullOrEmpty(trimmed))
                continue;

            int colonIndex = trimmed.IndexOf(':');
            if (colonIndex == -1 || colonIndex == 0)
                continue;

            string values = trimmed.Substring(colonIndex + 1).Trim();
            if (!string.IsNullOrEmpty(values))
                return true;
        }

        return false;
    }
    
    private const ulong Steam64Offset = 76561197960265728UL;
    public static (string steam2, string steam3, string steam32, string steam64) GetPlayerSteamID(this ulong steamId64)
    {
        uint id32 = (uint)(steamId64 - Steam64Offset);
        var steam32 = id32.ToString();
        uint y = id32 & 1;
        uint z = id32 >> 1;
        var steam2 = $"STEAM_0:{y}:{z}";
        var steam3 = $"[U:1:{steam32}]";
        var steam64 = steamId64.ToString();
        return (steam2, steam3, steam32, steam64);
    }

    public static string[]? ConvertCommands(this string input, bool EventPlayerChat = false)
    {
        var parts = input.Split('|', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Split(':', 2))
            .ToDictionary(
                p => p[0].Trim(),
                p => p.Length > 1
                    ? p[1].Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(c => c.Trim())
                        .Where(c => !string.IsNullOrEmpty(c))
                    : Enumerable.Empty<string>()
            );

        if (!parts.Values.Any(v => v.Any())) return null;

        if (!EventPlayerChat)
        {
            return parts.FirstOrDefault().Value?.Select(c =>
            {
                if (c.StartsWith("!"))
                {
                    var cmd = c.TrimStart('!');
                    return cmd.StartsWith("css_") ? cmd : "css_" + cmd;
                }
                return c;
            }).Distinct().ToArray();
        }

        var first = parts.FirstOrDefault().Value?
            .Select(c =>
            {
                var cmd = c.TrimStart('!');
                if (cmd.StartsWith("css_"))
                    cmd = cmd.Substring(4);
                return "!" + cmd;
            }) ?? Enumerable.Empty<string>();

        var rest = parts.Skip(1).SelectMany(p => p.Value);
        var result = first.Concat(rest).Distinct().ToArray();

        return result.Length == 0 ? null : result;
    }
    

    public static void PlayerRender(this CCSPlayerController player, int alpha, int red = 255, int green = 255, int blue = 255)
    {
        if (player == null || !player.IsValid) return;

        var PlayerPawn = player.PlayerPawn;
        if (PlayerPawn == null || !PlayerPawn.IsValid) return;

        var PlayerPawnValue = PlayerPawn.Value;
        if (PlayerPawnValue == null || !PlayerPawnValue.IsValid) return;

        PlayerPawnValue.Render = Color.FromArgb(alpha, red, green, blue);
        Utilities.SetStateChanged(PlayerPawnValue, "CBaseModelEntity", "m_clrRender");
    }


    public static CCSPlayerController? CheckPlayerController(this CCSPlayerController player)
    {
        if (player == null || !player.IsValid) return null;

        var Pawn = player.Pawn;
        if (Pawn == null || !Pawn.IsValid) return null;

        var PawnValue = Pawn.Value;
        if (PawnValue == null || !PawnValue.IsValid) return null;

        var getplayer = Utilities.GetPlayers().FirstOrDefault(p => p.IsValid(true) && p.PlayerPawn?.Value?.Index == PawnValue.Index);
        if (getplayer == null || !getplayer.IsValid) return null;

        return getplayer;
    }

    public static CCSPlayerController? GetPlayerFromCBaseEntity(this CBaseEntity entity, bool realPlayer = true)
    {
        if (entity == null || !entity.IsValid || entity.DesignerName != "player")return null;

        var entityIndex = entity.Index;

        return Utilities.GetPlayers().FirstOrDefault(p =>p.IsValid(true) &&(realPlayer? p.Pawn.Value?.Index == entityIndex : p.PlayerPawn.Value?.Index == entityIndex));
    }

    public static void PlayerHideHUD(this CCSPlayerController player, uint HideHUD, bool Hide)
    {
        if (player == null || !player.IsValid) return;

        var PlayerPawn = player.PlayerPawn;
        if (PlayerPawn == null || !PlayerPawn.IsValid) return;

        var PlayerPawnValue = PlayerPawn.Value;
        if (PlayerPawnValue == null || !PlayerPawnValue.IsValid) return;

        if (Hide)
        {
            PlayerPawnValue.HideHUD |= HideHUD;
        }
        else
        {
            PlayerPawnValue.HideHUD &= ~HideHUD;
        }

        Utilities.SetStateChanged(PlayerPawnValue, "CBasePlayerPawn", "m_iHideHUD");
    }

    public static void Player_ClanTag(this CCSPlayerController player, string ClanTag)
    {
        if (player == null || !player.IsValid) return;

        player.Clan = "";
        Utilities.SetStateChanged(player, "CCSPlayerController", "m_szClan");

        player.Clan = ClanTag;
        Utilities.SetStateChanged(player, "CCSPlayerController", "m_szClan");
        var newEvent2 = new EventNextlevelChanged(false) { };
        newEvent2.FireEventToClient(player);
    }
    public static void Player_Name(this CCSPlayerController player, string PlayerName)
    {
        if (player == null || !player.IsValid) return;
                
        player.PlayerName = PlayerName;
        Utilities.SetStateChanged(player, "CBasePlayerController", "m_iszPlayerName");
        var newEvent = new EventNextlevelChanged(false){};
        newEvent.FireEventToClient(player);
    }

    public static string ToCustomGrenadeName(this string grenadeName, CCSPlayerController? player,
    (string? Nade_Decoy, string? Nade_Flashbang, string? Nade_Incgrenade, string? Nade_Molotov,
    string? Nade_Smokegrenade, string? Nade_Hegrenade, string? JoinTeam_SPEC, string? JoinTeam_CT,
    string? JoinTeam_T, string? BotTakeOver, string? ClanTag_ScoreBoard, string? ClanTag_Chat,
    string? formatString) getValues)
    {
        if (string.IsNullOrEmpty(grenadeName))
            return grenadeName ?? string.Empty;

        if (player == null || !player.IsValid)
        {
            return GetDefaultGrenadeName(grenadeName);
        }


        string? customName = grenadeName.ToLower() switch
        {
            "hegrenade" => getValues.Nade_Hegrenade,
            "smokegrenade" => getValues.Nade_Smokegrenade,
            "flashbang" => getValues.Nade_Flashbang,
            "molotov" => getValues.Nade_Molotov,
            "incgrenade" => getValues.Nade_Incgrenade,
            "decoy" => getValues.Nade_Decoy,
            _ => null
        };

        return !string.IsNullOrEmpty(customName) 
            ? customName.ReplaceChatMessages(clan_scoreboard: getValues.ClanTag_ScoreBoard ?? "", clan_chat: getValues.ClanTag_Chat ?? "", PlayerName: player.PlayerName.RemoveColorNames(), location: player.PlayerPawn.Value?.LastPlaceName ?? "", team_color: player.TeamNum.ToTeamColor()) 
            : GetDefaultGrenadeName(grenadeName);
    }

    private static string GetDefaultGrenadeName(string grenadeName)
    {
        return grenadeName.ToLower() switch
        {
            "hegrenade" => "Nade_Hegrenade",
            "smokegrenade" => "Nade_Smokegrenade",
            "flashbang" => "Nade_Flashbang",
            "molotov" => "Nade_Molotov",
            "incgrenade" => "Nade_Incgrenade",
            "decoy" => "Nade_Decoy",
            _ => grenadeName
        };
    }
    public static string ToTeamColor(this byte team)
    {
        return team switch
        {
            1 => "{LightPurple}",
            2 => "{Orange}",
            3 => "{LightBlue}",
            0 => "{White}",
            _ => "{White}"
        };
    }
    private static readonly HashSet<string> _colorNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Default", "White", "DarkRed", "Green", "LightYellow", "LightBlue",
        "Olive", "Lime", "Red", "LightPurple", "Purple", "Grey", "Yellow",
        "Gold", "Silver", "Blue", "DarkBlue", "BlueGrey", "Magenta",
        "LightRed", "Orange"
    };

    public static string RemoveColorNames(this string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        string pattern = @"\{([^}]+)\}";
        return System.Text.RegularExpressions.Regex.Replace(input, pattern, match =>
        {
            string colorName = match.Groups[1].Value;
            return _colorNames.Contains(colorName) ? "" : match.Value;
        });
    }
    
    public static string ReplaceChatMessages(this string Message_format, string PlayerName = "",
    string Player_ID = "", string PLAYER_IP = "", string PLAYER_IP_WITHOUT_PORT = "", string PLAYER_STEAMID = "", string PLAYER_STEAMID3 = "",
    string PLAYER_STEAMID32 = "", string PLAYER_STEAMID64 = "",
    string BOT_Controlled = "", string clan_scoreboard = "", string clan_chat = "",
    string location = "", string message = "", string team_color = "")
    {
        return Message_format?
            .ReplaceIgnoreCase("{PLAYER_NAME}", PlayerName)
            .ReplaceIgnoreCase("{PLAYER_ID}", Player_ID)
            .ReplaceIgnoreCase("{PLAYER_IP}", PLAYER_IP)
            .ReplaceIgnoreCase("{PLAYER_IP_WITHOUT_PORT}", PLAYER_IP_WITHOUT_PORT)
            .ReplaceIgnoreCase("{PLAYER_STEAMID}", PLAYER_STEAMID)
            .ReplaceIgnoreCase("{PLAYER_STEAMID3}", PLAYER_STEAMID3)
            .ReplaceIgnoreCase("{PLAYER_STEAMID32}", PLAYER_STEAMID32)
            .ReplaceIgnoreCase("{PLAYER_STEAMID64}", PLAYER_STEAMID64)
            .ReplaceIgnoreCase("{BOT_NAME}", BOT_Controlled)
            .ReplaceIgnoreCase("{ClanTag_ScoreBoard}", clan_scoreboard)
            .ReplaceIgnoreCase("{ClanTag_Chat}", clan_chat)
            .ReplaceIgnoreCase("{PLAYER_LOCATION}", location)
            .ReplaceIgnoreCase("{PLAYER_MSG}", message)
            .ReplaceIgnoreCase("{TEAM_COLOR}", team_color)
            ?? string.Empty;
    }

    public static string ReplaceIgnoreCase(this string source, string oldValue, string newValue)
    {
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(oldValue))
            return source;
            
        return source.Replace(oldValue, newValue, StringComparison.OrdinalIgnoreCase);
    }
}