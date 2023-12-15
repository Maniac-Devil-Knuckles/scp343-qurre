using HarmonyLib;
using Qurre.API;
using Qurre.API.Classification.Player;

namespace SCP343.Patches
{
    [HarmonyPatch(typeof(RoleInfomation), nameof(RoleInfomation.Team), MethodType.Getter)]
    internal static class PlayerTeam
    {
        internal static bool Prefix(RoleInfomation __instance, ref PlayerRoles.Team __result)
        {
            if (!__instance.ServerRoles.gameObject.GetPlayer().IsSCP343()) return true;
            __result = PlayerRoles.Team.OtherAlive;
            return false;
        }
    }
}