using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using Game_Manager_GoldKingZ.Config;

namespace Game_Manager_GoldKingZ;

public partial class MainPlugin
{
    private Func<string, int, string>? countdownFormatter;
    public void OnTick()
    {
        if (g_Main.ActiveNameBlocks.Count == 0) return;
        var now = DateTime.Now;
        var key = Configs.Instance.BlockNameChanger == 1 ? "PrintToCenterToPlayer.NameChanging.Mode1.Blocked" : "PrintToCenterToPlayer.NameChanging.Mode2.Blocked";
        var culture = System.Globalization.CultureInfo.CurrentUICulture.Name;
        foreach (var data in g_Main.ActiveNameBlocks)
        {
            var player = data.Player;
            if (!player.IsValid() || !g_Main.Player_Data.TryGetValue(player.Slot, out var current) || !ReferenceEquals(current, data)) continue;
            var remaining = Configs.Instance.BlockNameChanger_Block - (now - data.LastNameChangeTime).TotalSeconds;
            player.PrintToCenterHtml(data.Countdown.Get(remaining, key, culture, countdownFormatter ??= FormatCountdown));
        }
    }
    private string FormatCountdown(string key, int seconds) => string.Format(Localizer[key, seconds], Array.Empty<object>());

    public void OnClientDisconnect(int slot)
    {
        Lifetime.InvalidateConnection(slot);
        CancelPlayerTimers(slot);
        if (g_Main.Player_Data.Remove(slot, out var data))
        {
            data.PlayerName_Block = false;
            CancelPlayerTimer(data.Timer_DeadBody);
        }
        g_Main.ActiveReloadId.Remove(slot);
    }
}

public partial class Helper
{
    private static readonly List<Globals.PlayerDataClass> NameSnapshot = new();
    public static void Timer_Checker_Callback()
    {
        NameSnapshot.Clear();
        NameSnapshot.AddRange(MainPlugin.Instance.g_Main.Player_Data.Values);
        foreach (var getplayers in NameSnapshot)
        {
            if (getplayers == null) continue;

            var player = getplayers.Player;
            if (!player.IsValid() || player.IsBot) continue;

            var currentPlayerName = player.PlayerName;
            var storedPlayerName = getplayers.PlayerName;

            if (!getplayers.PlayerName_Block && currentPlayerName != storedPlayerName)
            {
                getplayers.PlayerName_Count++;
                getplayers.LastNameChangeTime = DateTime.Now;
                getplayers.PlayerName = currentPlayerName;

                if (getplayers.PlayerName_Count >= Configs.Instance.BlockNameChanger_Changes)
                {
                    PunishPlayer(player, getplayers);
                }
            }
            else if (getplayers.PlayerName_Block)
            {
                if (currentPlayerName != storedPlayerName)
                {
                    getplayers.PlayerName = currentPlayerName;

                    if (Configs.Instance.BlockNameChanger == 1)
                    {
                        getplayers.LastNameChangeTime = DateTime.Now;
                    }
                }

                var timeSinceLastChange = (DateTime.Now - getplayers.LastNameChangeTime).TotalSeconds;

                if (Configs.Instance.BlockNameChanger == 1 && timeSinceLastChange >= Configs.Instance.BlockNameChanger_Block)
                {
                    getplayers.PlayerName_Count = 0;
                    getplayers.PlayerName_Block = false;
                    getplayers.PlayerName_Block_Message = false;
                    getplayers.PlayerName = currentPlayerName;
                    AdvancedPlayerPrintToChat(player, null!, MainPlugin.Instance.Localizer["PrintToChatToPlayer.NameChanging.Mode1.UnBlocked"]);
                }
                else if (Configs.Instance.BlockNameChanger == 2 && timeSinceLastChange >= Configs.Instance.BlockNameChanger_Block)
                {
                    ExecuteBlockCommand(player);
                    getplayers.PlayerName_Count = 0;
                    getplayers.PlayerName_Block = false;
                    getplayers.PlayerName_Block_Message = false;
                    getplayers.PlayerName = currentPlayerName;
                }
            }
        }
    }

    private static void PunishPlayer(CCSPlayerController player, Globals.PlayerDataClass playerData)
    {
        if (!player.IsValid() || playerData.PlayerName_Block) return;

        playerData.PlayerName_Block = true;
        playerData.LastNameChangeTime = DateTime.Now;

        if (!playerData.PlayerName_Block_Message)
        {
            AdvancedPlayerPrintToChat(player, null!, MainPlugin.Instance.Localizer["PrintToChatToPlayer.NameChanging.Warning"], Configs.Instance.BlockNameChanger_Block);
            playerData.PlayerName_Block_Message = true;
        }

        player.ChangeTeam(CsTeam.Spectator);
    }

}
