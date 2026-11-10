using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using PluginTimer = CounterStrikeSharp.API.Modules.Timers.Timer;

namespace Game_Manager_GoldKingZ;

public partial class MainPlugin
{
    private readonly Dictionary<PluginTimer, int> playerTimers = new();

    internal void QueueFrame(Action callback)
    {
        var generation = Lifetime.Capture();
        Server.NextFrame(() => { if (Lifetime.IsCurrent(generation)) callback(); });
    }

    private Func<bool> PlayerGuard(CCSPlayerController player)
    {
        var generation = Lifetime.Capture();
        var slot = player.Slot;
        var connection = Lifetime.CaptureConnection(slot);
        var userId = player.UserId;
        var steamId = player.SteamID;
        var handle = player.Handle;
        return () => Lifetime.IsCurrent(generation) && Lifetime.CaptureConnection(slot) == connection
            && player.IsValid(true) && player.Handle == handle && player.UserId == userId && player.SteamID == steamId;
    }

    internal void QueuePlayerFrame(CCSPlayerController player, Action callback)
    {
        var valid = PlayerGuard(player);
        Server.NextFrame(() => { if (valid()) callback(); });
    }

    internal PluginTimer AddPlayerTimer(float interval, CCSPlayerController player, Action callback, TimerFlags flags)
    {
        return AddGuardedTimer(interval, player.Slot, PlayerGuard(player), callback, flags);
    }

    internal PluginTimer AddLifetimeTimer(float interval, Action callback, TimerFlags flags)
    {
        var generation = Lifetime.Capture();
        return AddGuardedTimer(interval, -1, () => Lifetime.IsCurrent(generation), callback, flags);
    }

    private PluginTimer AddGuardedTimer(float interval, int slot, Func<bool> valid, Action callback, TimerFlags flags)
    {
        PluginTimer timer = null!;
        timer = AddTimer(interval, () =>
        {
            if (!valid()) { CancelPlayerTimer(timer); return; }
            try { callback(); }
            finally
            {
                if (!flags.HasFlag(TimerFlags.REPEAT))
                {
                    playerTimers.Remove(timer);
                    Timers.Remove(timer);
                }
            }
        }, flags);
        playerTimers.Add(timer, slot);
        return timer;
    }

    internal void CancelPlayerTimer(PluginTimer? timer)
    {
        if (timer == null || !playerTimers.Remove(timer)) return;
        timer.Kill();
        Timers.Remove(timer);
    }

    internal void CancelPlayerTimers(int? slot = null)
    {
        foreach (var entry in playerTimers.ToArray())
            if (slot == null || entry.Value == slot) CancelPlayerTimer(entry.Key);
    }
}
