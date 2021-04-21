using System;
using Qurre.API;
using HarmonyLib;
namespace SCP343.Patches
{
    [HarmonyPatch(typeof(CharacterClassManager),nameof(CharacterClassManager.CallCmdRegisterEscape))]
    public class EscapePatch
    {
        private static bool Prefix(CharacterClassManager __instance)
        {
            if (!Player.Get(__instance.gameObject).IsSCP343()) return true;
            return scp343.cfg.scp343_canescape;
        }
    }
}
