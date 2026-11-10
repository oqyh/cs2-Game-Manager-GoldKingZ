using System.Runtime.InteropServices;
using Game_Manager_GoldKingZ.Config;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Cvars;

namespace Game_Manager_GoldKingZ;

public static class CustomHooks
{
    private static MemoryFunctionWithReturn<IntPtr, int, IntPtr, IntPtr, IntPtr, byte>? OnConVarChanged;
    private static bool _isHooked = false;
    internal static bool IsHooked => _isHooked;

    public static void Init(CustomGameData gameData)
    {
        if (_isHooked || !Configs.Instance.UseOnConVarChangedHook) return;

        try
        {
            
            OnConVarChanged = gameData.CreateFunction<MemoryFunctionWithReturn<IntPtr, int, IntPtr, IntPtr, IntPtr, byte>>("OnConVarChanged");

            if (OnConVarChanged != null)
            {
                OnConVarChanged.Hook(OnConVarChanged_Hook, HookMode.Pre);
                _isHooked = true;
                Helper.ExectueCommands(true);
            }


            Helper.DebugMessage("All Hooks Started");
        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"Hook Init Error: {ex.Message}", 0);
        }
    }

    public static void Cleanup()
    {
        if (!_isHooked) return;

        try
        {
            if (OnConVarChanged != null)
            {
                OnConVarChanged.Unhook(OnConVarChanged_Hook, HookMode.Pre);
            }
        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"Hook Cleanup Error: {ex.Message}");
        }
        finally
        {
            OnConVarChanged = null;
            _isHooked      = false;
            Helper.DebugMessage("Hooks Removed");
        }
    }

    public static HookResult OnConVarChanged_Hook(DynamicHook hook)
    {
        try
        {
            var cvarRefPtr = hook.GetParam<IntPtr>(0);
            var slot       = hook.GetParam<int>(1);
            var valuePtr   = hook.GetParam<IntPtr>(2);

            if (cvarRefPtr == IntPtr.Zero || valuePtr == IntPtr.Zero)
                return HookResult.Continue;

            string cvarName = ReadConVarName(cvarRefPtr);
            if (string.IsNullOrEmpty(cvarName))
                return HookResult.Continue;

            string newValue = Marshal.PtrToStringAnsi(valuePtr) ?? "";

            if (MainPlugin.Instance.g_Main.HookConVars.TryGetValue(cvarName, out string? expectedValue))
            {
                if (!newValue.Equals(expectedValue, StringComparison.OrdinalIgnoreCase))
                {
                    Helper.DebugMessage($"[OnConVarChanged_Hook] Slot {slot} attempted to change \"{cvarName}\" from \"{expectedValue}\" to \"{newValue}\", forcing back to \"{expectedValue}\"", Configs.Instance.EnableDebug.ToDebugConfig(1));

                    MainPlugin.Instance.QueueFrame(() =>
                    {
                        Server.ExecuteCommand($"{cvarName} {expectedValue}");
                    });
                }
            }

            return HookResult.Continue;
        }
        catch (Exception ex)
        {
            Helper.DebugMessage("[OnConVarChanged_Hook] Error in hook callback");
            Helper.DebugMessage(ex.ToString());
            return HookResult.Continue;
        }
    }

    private static string ReadConVarName(IntPtr cvarRefPtr)
    {
        try
        {
            var cvarPtr = Marshal.ReadIntPtr(cvarRefPtr, 0x8);
            if (cvarPtr == IntPtr.Zero) return "";

            var namePtr = Marshal.ReadIntPtr(cvarPtr, 0x0);
            if (namePtr == IntPtr.Zero) return "";

            return Marshal.PtrToStringAnsi(namePtr) ?? "";
        }
        catch
        {
            return "";
        }
    }
    
    
}