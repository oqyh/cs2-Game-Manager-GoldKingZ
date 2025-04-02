using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Core.Translations;
using Game_Manager_GoldKingZ.Config;

namespace Game_Manager_GoldKingZ;

public class PlayerChat
{
    public HookResult OnPlayerChat(CCSPlayerController? player, CommandInfo info, bool TeamChat)
    {
        if (!player.IsValid()) return HookResult.Continue;
        Helper.AddPlayerToGlobal(player);

        if (!GameManagerGoldKingZ.Instance.g_Main.Player_Data.ContainsKey(player)) return HookResult.Continue;

        var eventmessage = info.ArgString;
        eventmessage = eventmessage.TrimStart('"');
        eventmessage = eventmessage.TrimEnd('"');
        
        if (string.IsNullOrWhiteSpace(eventmessage)) return HookResult.Continue;
        string trimmedMessageStart = eventmessage.TrimStart();
        string message = trimmedMessageStart.TrimEnd();

        if (Configs.GetConfigData().Custom_ChatMessages_ExcludeStartWith.Any(prefix => 
        message.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
        {
            return HookResult.Continue;
        }


        string messageKey = Helper.DetermineMessageKey(GameManagerGoldKingZ.Instance.g_Main.Player_Data[player].Messagename, player);
        string location = player.PlayerPawn.Value?.LastPlaceName ?? "";

        var jsonData = GameManagerGoldKingZ.Instance.g_Main.JsonData;
        if (jsonData == null) return HookResult.Continue;

        bool messageSent = false;

        foreach (var entry in jsonData.Properties())
        {
            string groupKey = entry.Name;
            if (groupKey == "ANY") continue;

            var messageFormats = entry.Value.ToObject<Dictionary<string, string>>();
            if (messageFormats == null || !messageFormats.TryGetValue(messageKey, out var format)) continue;

            if (Helper.IsPlayerInGroupPermission(player, groupKey))
            {
                string tempFormat = format
                    .Replace("{0}", player.PlayerName)
                    .Replace("{1}", location)
                    .Replace("{2}", message)
                    .ReplaceColorTags();

                string[] keyParts = messageKey.Split('_');
                string chatType = keyParts.Length >= 3 ? keyParts[2] : "ALL";

                if (chatType == "TEAM")
                {
                    foreach (var teammate in Helper.GetPlayersController().Where(p => p.IsValid() && p.TeamNum == player.TeamNum))
                    {
                        teammate.PrintToChat(tempFormat);
                    }
                }
                else
                {
                    Server.PrintToChatAll(tempFormat);
                }

                messageSent = true;
                break;
            }
        }

        if (!messageSent)
        {
            var anyEntry = jsonData["ANY"];
            if (anyEntry != null)
            {
                var anyFormats = anyEntry.ToObject<Dictionary<string, string>>();
                if (anyFormats != null && anyFormats.TryGetValue(messageKey, out var anyFormat))
                {
                    string tempFormat = anyFormat
                        .Replace("{0}", player.PlayerName)
                        .Replace("{1}", location)
                        .Replace("{2}", message)
                        .ReplaceColorTags();

                    string[] keyParts = messageKey.Split('_');
                    string chatType = keyParts.Length >= 3 ? keyParts[2] : "ALL";

                    if (chatType == "TEAM")
                    {
                        foreach (var teammate in Helper.GetPlayersController().Where(p => p.IsValid() && p.TeamNum == player.TeamNum))
                        {
                            teammate.PrintToChat(tempFormat);
                        }
                    }
                    else
                    {
                        Server.PrintToChatAll(tempFormat);
                    }
                }
            }
        }

        return HookResult.Continue;
    }
    
}