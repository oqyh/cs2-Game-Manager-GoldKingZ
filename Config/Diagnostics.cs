using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using Game_Manager_GoldKingZ.Config;
namespace Game_Manager_GoldKingZ;
public partial class MainPlugin
{
    internal string LastReload = "startup";
    private bool CanDiagnose(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null || AdminManager.PlayerHasPermissions(player, "@css/root")) return true;
        info.ReplyToCommand("GameManager: root permission required."); return false;
    }
    private void StatusCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!CanDiagnose(player, info)) return;
        info.ReplyToCommand($"GameManager {ModuleVersion}; map={Server.MapName}; prefs={(_prefs != null ? "ready" : "unavailable")}; gamedata={CustomGameData.Status}; cvarHook={CustomHooks.IsHooked}; chatRules={g_Main.JsonData != null}; weaponRules={g_Main.JsonData_Weapon != null}; sessions={g_Main.Player_Data.Count}; nameBlocks={g_Main.ActiveNameBlocks.Count}; timers={playerTimers.Count}; reload={LastReload}");
    }
    private void ReloadCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!CanDiagnose(player, info)) return;
        try { ReloadConfiguration(); info.ReplyToCommand("GameManager: configuration reloaded."); }
        catch (InvalidDataException ex) { info.ReplyToCommand("GameManager: reload rejected: " + ex.Message); }
    }
    internal void ReloadConfiguration()
    {
        try
        {
            var rules = Helper.PrepareRules();
            Configs.Load(ModuleDirectory, () => {
                Helper.RemoveRegisterCommandsAndHooks(); Helper.RemoveOnEntityTakeDamagePre();
                Helper.RestoreOwnedState(); Helper.ClearVariables();
            });
            Helper.ApplyRules(rules);
            Helper.RegisterCommandsAndHooks(); Helper.RegisterHookOnEntityTakeDamagePre();
            Helper.ExectueCommands(); Helper.ReloadPlayersGlobals(); Helper.StartTimer(); Helper.DownloadMissingFiles();
            foreach (var connected in Helper.GetPlayersController(true))
            {
                Helper.HideChatHUD(connected); Helper.HideWeaponsHUD(connected); Helper.RemoveHideDeadBody(connected);
            }
            LastReload = "success " + DateTime.UtcNow.ToString("O");
        }
        catch (InvalidDataException ex) { LastReload = "rejected " + DateTime.UtcNow.ToString("O") + " " + ex.Message; throw; }
    }
}
