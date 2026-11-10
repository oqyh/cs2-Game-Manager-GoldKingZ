using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;
using System.Drawing;
namespace Game_Manager_GoldKingZ;
public partial class Helper
{
    private static readonly Dictionary<string, string> OriginalCvars = new();
    private static readonly Dictionary<uint, (CCSPlayerPawn Pawn, uint Original, uint Added)> HudStates = new();
    private static readonly Dictionary<uint, (CCSPlayerPawn Pawn, Color Original, Color Applied)> RenderStates = new();
    internal static void TrackHud(CCSPlayerPawn pawn, uint mask)
    {
        if (HudStates.Count >= 128 && !HudStates.ContainsKey(pawn.EntityHandle.Raw))
            foreach (var key in HudStates.Where(x => !x.Value.Pawn.IsValid).Select(x => x.Key).ToArray()) HudStates.Remove(key);
        HudStates.TryGetValue(pawn.EntityHandle.Raw, out var old);
        HudStates[pawn.EntityHandle.Raw] = (pawn, old.Pawn == null ? pawn.HideHUD : old.Original, old.Added | (mask & ~pawn.HideHUD));
    }
    internal static void TrackRender(CCSPlayerPawn pawn, Color applied)
    {
        if (RenderStates.Count >= 128 && !RenderStates.ContainsKey(pawn.EntityHandle.Raw))
            foreach (var key in RenderStates.Where(x => !x.Value.Pawn.IsValid).Select(x => x.Key).ToArray()) RenderStates.Remove(key);
        RenderStates.TryGetValue(pawn.EntityHandle.Raw, out var old);
        RenderStates[pawn.EntityHandle.Raw] = (pawn, old.Pawn == null ? pawn.Render : old.Original, applied);
    }
    internal static void ExecuteOwnedCvars(string commands)
    {
        foreach (var command in commands.Split(';', StringSplitOptions.RemoveEmptyEntries))
        {
            var name = command.Trim().Split(' ', 2)[0];
            var cvar = ConVar.Find(name);
            if (cvar != null && !OriginalCvars.ContainsKey(name)) OriginalCvars[name] = cvar.StringValue;
        }
        Server.ExecuteCommand(commands);
    }
    internal static void RestoreOwnedState()
    {
        CustomHooks.Cleanup();
        foreach (var pair in OriginalCvars) Server.ExecuteCommand(pair.Key + " \"" + pair.Value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "").Replace("\r", "") + "\"");
        OriginalCvars.Clear();
        foreach (var state in HudStates.Values)
            if (state.Pawn.IsValid) { state.Pawn.HideHUD &= ~state.Added; Utilities.SetStateChanged(state.Pawn, "CBasePlayerPawn", "m_iHideHUD"); }
        HudStates.Clear();
        foreach (var state in RenderStates.Values)
            if (state.Pawn.IsValid && state.Pawn.Render == state.Applied) { state.Pawn.Render = state.Original; Utilities.SetStateChanged(state.Pawn, "CBaseModelEntity", "m_clrRender"); }
        RenderStates.Clear();
    }
}
