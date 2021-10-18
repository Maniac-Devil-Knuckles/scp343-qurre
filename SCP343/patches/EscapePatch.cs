using UnityEngine;
using Qurre.API;
using HarmonyLib;
namespace SCP343.Patches
{
    [HarmonyPatch(typeof(CharacterClassManager),"UserCode_CmdRegisterEscape")]
    public class EscapePatch
    {
        private static bool Prefix(CharacterClassManager __instance) => !Player.Get(__instance.gameObject).IsSCP343() ? true : Cfg.scp343_canescape;
    }
}
