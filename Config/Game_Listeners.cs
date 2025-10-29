using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Localization;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Commands;
using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using System.Text;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Memory;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using CounterStrikeSharp.API.Modules.Entities;
using System;
using System.Globalization;
using System.Drawing;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Menu;
using Game_Manager_GoldKingZ;
using System.Reflection.Metadata;
using Game_Manager_GoldKingZ.Config;

namespace Game_Manager_GoldKingZ;

public class Game_Listeners
{
    public HookResult BlockRadio_Listener(CCSPlayerController? player, CommandInfo info)
    {
        if (!player.IsValid(true)) return HookResult.Continue;

        return HookResult.Handled;
    }

    public HookResult BlockChatwheel_Listener(CCSPlayerController? player, CommandInfo info)
    {
        if (!player.IsValid(true)) return HookResult.Continue;

        return HookResult.Handled;
    }

    public HookResult BlockPing_Listener(CCSPlayerController? player, CommandInfo info)
    {
        if (!player.IsValid(true)) return HookResult.Continue;

        return HookResult.Handled;
    }
    

    public HookResult BlockCommands_Listener(CCSPlayerController? player, CommandInfo info)
    {
        if (!player.IsValid()
        ||Configs.Instance.Block_Commands.Block_Commands_Ignore_Flags.HasValidPermissionData() && Helper.IsPlayerInGroupPermission(player, Configs.Instance.Block_Commands.Block_Commands_Ignore_Flags)) return HookResult.Continue;

        var commands = info.GetCommandString.ToString();
        if (Configs.Instance.Block_Commands.Block_Commands_StartWith.Any(blocked => commands.StartsWith(blocked, Helper.GetComparison(Configs.Instance.Block_Commands.Block_Commands_StartWith_IgnoreCase)))
        || Configs.Instance.Block_Commands.Block_Commands_Contains.Any(blocked => commands.Contains(blocked, Helper.GetComparison(Configs.Instance.Block_Commands.Block_Commands_Contains_IgnoreCase))))
        {
            Helper.AdvancedPlayerPrintToChat(player, info, MainPlugin.Instance.Localizer["PrintToChatToPlayer.BlockCommands", commands]);
            return HookResult.Handled;
        }

        return HookResult.Continue;
    }
}