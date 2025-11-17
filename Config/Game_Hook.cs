using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Game_Manager_GoldKingZ.Config;
using Newtonsoft.Json.Linq;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Modules.Timers;

namespace Game_Manager_GoldKingZ;

public class Game_Hook
{

    public HookResult OnTakeDamage(DynamicHook hook)
    {
        try
        {
            var g_Main = MainPlugin.Instance.g_Main;
            var ent = hook.GetParam<CEntityInstance>(0);
            if (ent == null || !ent.IsValid || ent.DesignerName != "player") return HookResult.Continue;

            var damageinfo = hook.GetParam<CTakeDamageInfo>(1);
            if (damageinfo == null) return HookResult.Continue;

            if (damageinfo.Attacker.Value == null) return HookResult.Continue;
            var attackerPawn = damageinfo.Attacker.Value.As<CBasePlayerPawn>();
            if (attackerPawn == null || !attackerPawn.IsValid) return HookResult.Continue;
            
            var GetAttacker = attackerPawn.Controller.Value;
            if (GetAttacker == null || !GetAttacker.IsValid) return HookResult.Continue;

            var pawn = ent.As<CCSPlayerPawn>();
            if (pawn == null || !pawn.IsValid) return HookResult.Continue;

            var GetAttackerController = Utilities.GetPlayerFromIndex((int)GetAttacker.Index);
            if (!GetAttackerController.IsValid(true)) return HookResult.Continue;
            var attacker = GetAttackerController.CheckPlayerController();
            if (!attacker.IsValid(true)) return HookResult.Continue;

            Helper.CheckPlayerInGlobals(attacker);

            var victimController = pawn.OriginalController?.Get();
            if (victimController == null || !victimController.IsValid(true)) return HookResult.Continue;
            var victim = victimController;

            Helper.CheckPlayerInGlobals(victim);

            if (Configs.Instance.Custom_MuteSounds_1.Custom_MuteSounds1 > 1 || Configs.Instance.Custom_MuteSounds_2.Custom_MuteSounds2 > 1 || Configs.Instance.Custom_MuteSounds_3.Custom_MuteSounds3 > 1 ||Configs.Instance.Sounds_MuteKnife == 2)
            {
                if (g_Main.Player_Data.TryGetValue(victim, out var victim_handle))
                {
                    victim_handle.Attacker = attacker;
                }
            }

            bool teammatesAreEnemies = ConVar.Find("mp_teammates_are_enemies")!.GetPrimitiveValue<bool>();
            bool ValidHit = victim.IsValid(true) && attacker.IsValid(true) && attacker != victim && (teammatesAreEnemies || attacker.TeamNum != victim.TeamNum);
            
            if(Configs.Instance.Sounds_MuteKnife == 2 && attacker != victim && g_Main.Player_Data.TryGetValue(attacker, out var attacker_handle))
            {
                if(damageinfo.BitsDamageType == DamageTypes_t.DMG_SLASH)
                {
                    if (ValidHit)
                    {
                        attacker_handle.StabedHisTeamMate = false;

                    }else
                    {
                        attacker_handle.StabedHisTeamMate = true;

                        MainPlugin.Instance.AddTimer(0.01f, () =>
                        {
                            if (attacker.IsValid(true) && g_Main.Player_Data.TryGetValue(attacker, out var attacker_handle2))
                            {
                                attacker_handle2.StabedHisTeamMate = false;
                            }
                        }, TimerFlags.STOP_ON_MAPCHANGE);
                    }
                }
            }

            if(Configs.Instance.DisableKnifeDamage && attacker != victim)
            {
                if(ValidHit && damageinfo.BitsDamageType == DamageTypes_t.DMG_SLASH)
                {
                    damageinfo.Damage = 0;
                }
            }

            if(Configs.Instance.DisableZeusDamage && attacker != victim)
            {
                if(ValidHit && damageinfo.BitsDamageType == DamageTypes_t.DMG_SHOCK)
                {
                    damageinfo.Damage = 0;
                }
            }
            
            if (Configs.Instance.Disable_AimPunch.DisableAimPunch == 1 || (Configs.Instance.Disable_AimPunch.DisableAimPunch > 1 && g_Main.Player_Data.TryGetValue(victim, out var handle) && (handle.Toggle_AimPunch == -1 || handle.Toggle_AimPunch == 1)))
            {
                Server.NextFrame(() =>
                {
                    var victimNextFrame = victim;
                    if (!victimNextFrame.IsValid(true)) return;

                    if (Configs.Instance.Disable_AimPunch.DisableAimPunch == 1 || (Configs.Instance.Disable_AimPunch.DisableAimPunch > 1 && g_Main.Player_Data.TryGetValue(victimNextFrame, out var handle) && (handle.Toggle_AimPunch == -1 || handle.Toggle_AimPunch == 1)))
                    {
                        var VictimPlayerPawn = victimNextFrame.PlayerPawn;
                        if (VictimPlayerPawn == null || !VictimPlayerPawn.IsValid) return;

                        var VictimPlayerPawnValue = VictimPlayerPawn.Value;
                        if (VictimPlayerPawnValue == null || !VictimPlayerPawnValue.IsValid) return;

                        VictimPlayerPawnValue.AimPunchAngle.X = 0;
                        VictimPlayerPawnValue.AimPunchAngle.Y = 0;
                        VictimPlayerPawnValue.AimPunchAngle.Z = 0;
                    }
                });
            }
            return HookResult.Continue;
        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"OnTakeDamage Error : {ex.Message}", Configs.Instance.EnableDebug.ToDebugConfig(1));
            Helper.DebugMessage($"[StackTrace] OnTakeDamage Error: {ex.StackTrace}", Configs.Instance.EnableDebug.ToDebugConfig(1));
            return HookResult.Continue;
        }
    }
}