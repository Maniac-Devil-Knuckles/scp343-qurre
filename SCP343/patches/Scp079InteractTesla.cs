using System;
using Qurre.API;
using Qurre.API.Events;
using HarmonyLib;
using UnityEngine;
using Interactables.Interobjects.DoorUtils;
using System.Collections.Generic;
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
            __instance.RefreshCurrentRoom();
            bool flag = target != null && target.TryGetComponent(out DoorVariant doorVariant);
            List<string> list = GameCore.ConfigFile.ServerConfig.GetStringList("scp079_door_blacklist") ?? new List<string>();
            bool result;
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
                                TeslaTriggerEvent ev = new TeslaTriggerEvent(Player.Get(__instance.gameObject), teslaGate.GetTesla(), API.AllScps343.Any(p => teslaGate.PlayerInHurtRange(p.GameObject)), scp343.cfg.scp343_activating_tesla_in_range);
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
}
