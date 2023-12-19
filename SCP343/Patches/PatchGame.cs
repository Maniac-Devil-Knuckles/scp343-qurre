using CustomPlayerEffects;
using HarmonyLib;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.BasicMessages;
using Mirror;
using PlayerRoles;
using Qurre.API;
using System.Collections.Generic;

namespace SCP343.Patches
{
    [HarmonyPatch(typeof(FirearmBasicMessagesHandler), "ServerShotReceived")]
    internal static class FirearmPatch
    {
        internal static bool Prefix(NetworkConnection conn, ShotMessage msg)
        {
            if (!ReferenceHub.TryGetHub(conn.identity.gameObject, out ReferenceHub referenceHub)) return false;
            if (msg.ShooterWeaponSerial != referenceHub.inventory.NetworkCurItem.SerialNumber) return false;
            if (referenceHub.inventory.CurInstance is not Firearm) return false;
            if (!referenceHub.IsSCP343()) return true;
            bool result = true;
            Player player = referenceHub.GetPlayer();
            if (!Config.Can_Use_TranquilizerGun) result = false;
            else if (player.GetSCPBadge().ShootCooldown > 0)
            {
                result = false;
                player.Client.ShowHint(Config.Translation.ShootCoolDownText.Replace("%seconds%", player.GetSCPBadge().ShootCooldown.ToString()), 5);
            }
            else player.GetSCPBadge().ShootCooldown = Config.ShootCooldown;
            return result;
        }
    }
    
    [HarmonyPatch(typeof(StatusEffectBase), "Intensity", MethodType.Setter)]
    internal static class InvisiblePatch
    {
        private static readonly List<RoleTypeId> _roles = new() { RoleTypeId.Spectator, RoleTypeId.Overwatch, RoleTypeId.Filmmaker, RoleTypeId.Spectator, RoleTypeId.None};
        internal static bool Prefix(StatusEffectBase __instance, ref byte value)
        {
            if (_roles.Contains(__instance.Hub.GetRoleId())) return true;
            Player player = __instance.Hub.GetPlayer();
            if (player.IsSCP343() && __instance is Invisible && player.Effects.CheckActive<Invisible>())
            {
                if (!__instance.Hub.GetPlayer().GetSCPBadge().IsInvisible) return true;
                value = 1;
                return false;
            }
            return true;
        }
    }
    
}