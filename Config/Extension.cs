using System.Drawing;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace Game_Manager_GoldKingZ;
public static class Extension
{
    public static bool IsValid([NotNullWhen(true)] this CCSPlayerController? player, bool Excludebot = true, bool ExcludeHLTV = true)
    {
        if (player == null || !player.IsValid)
        {
            return false;
        }

        if (Excludebot && player.IsBot)
        {
            return false;
        }

        if (ExcludeHLTV && player.IsHLTV)
        {
            return false;
        }

        return true;
    }
    
    public static bool IsAlive(this CCSPlayerController player)
    {
        if(player.PlayerPawn.Value?.LifeState == (byte)LifeState_t.LIFE_ALIVE)
        {
            return true;
        }

        return false;
    }

    public static void PlayerRender(this CCSPlayerController player, int alpha, int red = 255, int green = 255, int blue = 255)
    {
        if(player.PlayerPawn == null || !player.PlayerPawn.IsValid)return;
        if(player.PlayerPawn.Value == null || !player.PlayerPawn.Value.IsValid)return;

        player.PlayerPawn.Value.Render = Color.FromArgb(alpha, red, green, blue);
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
    }

    public static void PlayerHideHUD(this CCSPlayerController player, uint HideHUD, bool Hide)
    {
        if(player.PlayerPawn == null || !player.PlayerPawn.IsValid)return;
        if(player.PlayerPawn.Value == null || !player.PlayerPawn.Value.IsValid)return;

        if(Hide)
        {
            player.PlayerPawn.Value.HideHUD |= HideHUD;
        }else
        {
            player.PlayerPawn.Value.HideHUD &= ~HideHUD;
        }
        
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBasePlayerPawn", "m_iHideHUD");
    }
}