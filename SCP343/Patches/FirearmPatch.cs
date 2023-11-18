using HarmonyLib;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.BasicMessages;
using Mirror;
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
            Firearm firearm = referenceHub.inventory.CurInstance as Firearm;
            if (firearm == null) return false;
            if (Scp343BadgeList.Contains(referenceHub))
            {
                bool result = true;
                Player player = referenceHub.GetPlayer();
                if (player.Inventory.Base.NetworkCurItem.TypeId == ItemType.GunCOM15)
                    if (!Config.Can_Use_TranquilizerGun) result = false;
                    else if (player.GetSCPBadge().ShootCooldown > 0)
                    {
                        result = false;
                        player.Client.ShowHint(Config.Translation.ShootCoolDownText.Replace("%seconds%", player.GetSCPBadge().ShootCooldown.ToString()), 5);
                    }
                    else player.GetSCPBadge().ShootCooldown = Config.ShootCooldown;
                return result;
            }
            return true;
        }
    }
}