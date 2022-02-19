using Qurre.API;
using Qurre.API.Events;
using HarmonyLib;
using UnityEngine;
using System.Linq;
namespace SCP343.Patches
{
    [HarmonyPatch(typeof(Scp079PlayerScript), "UserCode_CmdInteract")]
    internal static class Scp079InteractTesla
    {
        private static bool Prefix(Scp079PlayerScript __instance, Command079 command, string args, GameObject target)
        { 
            if (__instance._interactRateLimit.CanExecute(true)) return false;
            if (!__instance.iAm079) return false;
            GameCore.Console.AddDebugLog("SCP079", "Command received from a client: " + command, MessageImportance.LessImportant, false);
            bool result;
            __instance.RefreshCurrentRoom();
            switch (command)
            {
                case Command079.Tesla:
                    {
                        result = false;
                        float manaFromLabel = __instance.GetManaFromLabel("Tesla Gate Burst", __instance.abilities);
                        if (manaFromLabel > __instance.Network_curMana)
                        {
                            __instance.RpcNotEnoughMana(manaFromLabel, __instance.Network_curMana);
                        }
                        else if (__instance.CurrentRoom != null)
                        {
                            TeslaGate teslaGate = __instance.CurrentRoom.GetComponentInChildren<TeslaGate>();
                            if (teslaGate != null)
                            {
                                TeslaTriggerEvent ev = new TeslaTriggerEvent(Player.Get(__instance.gameObject), teslaGate.GetTesla(), API.AllScps343.Any(p => teslaGate.PlayerInHurtRange(p.GameObject)), Scp343.CustomConfig.activating_tesla_in_range);
                                if (!ev.InHurtingRange || !ev.Triggerable)
                                {
                                    ev.Tesla.Trigger(true);
                                    __instance.AddInteractionToHistory(ev.Tesla.GameObject, true);
                                    __instance.Network_curMana -= manaFromLabel;
                                }
                            }
                        }
                        break;
                    }
                default:
                    {
                        result = true;
                        break;
                    }
            }
            return result;
        }
    }

    //[HarmonyPatch(typeof(scp035.EventHandlers), "PickupItem")]
    internal static class Scp035
    {
        internal static bool Prefix(PickupItemEvent ev) => !ev.Player.IsSCP343();
    }
}
