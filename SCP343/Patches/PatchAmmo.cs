using HarmonyLib;
using InventorySystem;
using Qurre.API;

namespace SCP343.Patches
{
    [HarmonyPatch(typeof(InventoryExtensions), "ServerDropAmmo")]
    internal static class DroppingAmmoPatch
    {
        private static bool Prefix(Inventory inv) => !Scp343BadgeList.Contains(inv.gameObject);
    }
}
