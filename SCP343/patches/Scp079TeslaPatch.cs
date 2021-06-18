
#pragma warning disable SA1313
#pragma warning disable CS0618
#pragma warning disable CS0436
using System;
using System.Collections.Generic;

using Qurre;
using Qurre.API;
using Qurre.API.Events;
using GameCore;

using HarmonyLib;

using Interactables.Interobjects.DoorUtils;

using NorthwoodLib.Pools;

using UnityEngine;
using Console = GameCore.Console;
using Log = Qurre.Log;
using System.Linq;

namespace SCP343.Patches
{
    [HarmonyPatch(typeof(Scp079PlayerScript), nameof(Scp079PlayerScript.CallCmdInteract))]
    internal class Scp079InteractTesla
    {
        internal static bool Prefix(Scp079PlayerScript __instance, string command, GameObject target)
        {
            try
            {
                if (!__instance._interactRateLimit.CanExecute())
                {
                    return false;
                }

                if (!__instance.iAm079)
                {
                    return false;
                }

                //Console.AddDebugLog("SCP079", "Command received from a client: " + command, MessageImportance.LessImportant);
                if (!command.Contains(":"))
                {
                    return false;
                }

                string[] array = command.Split(':');
                __instance.RefreshCurrentRoom();
                if (!__instance.CheckInteractableLegitness(__instance.currentRoom, __instance.currentZone, target, true))
                {
                    return false;
                }

                List<string> list = ListPool<string>.Shared.Rent();
                ConfigFile.ServerConfig.GetStringCollection("scp079_door_blacklist", list);

                bool result = true;
                Log.Info(array[0]);
                switch (array[0])
                {
                    case "TESLA":
                        {
                            GameObject gameObject3 = GameObject.Find(__instance.currentZone + "/" + __instance.currentRoom + "/Gate");
                            if (gameObject3 == null)
                            {
                                result = false;
                                break;
                            }
                            Player player = Player.Get(__instance.gameObject);
                            TeslaGate teslaGate = gameObject3.GetComponent<TeslaGate>();
                            float apDrain = __instance.GetManaFromLabel("Tesla Gate Burst", __instance.abilities);
                            bool isAllowed = apDrain <= __instance.curMana;
                            TeslaTriggerEvent ev = new TeslaTriggerEvent(player, teslaGate, isAllowed);
                            List<ReferenceHub> players = new List<ReferenceHub>();
                            foreach (KeyValuePair<GameObject, ReferenceHub> allHub in ReferenceHub.GetAllHubs())
                            {
                                if (allHub.Value.characterClassManager.CurClass == RoleType.Spectator)
                                    continue;
                                if (teslaGate.PlayerInRange(allHub.Value)) players.Add(allHub.Value);
                            }
                            if (players.Count()>0)
                            {
                                bool allowed = false;
                              if (players.Any(d=>Player.Get(d).IsSCP343())) allowed =  scp343.cfg.scp343_activating_tesla_in_range;
                                ev.Triggerable = !allowed;
                            }
                            if (!ev.Triggerable)
                            {
                                if (apDrain > __instance.curMana)
                                {
                                    __instance.RpcNotEnoughMana(apDrain, __instance.curMana);
                                    result = false;
                                    break;
                                }
                            }
                            else
                            {
                                teslaGate.RpcInstantBurst();
                                __instance.AddInteractionToHistory(gameObject3, array[0], addMana: true);
                                __instance.Mana -= apDrain;
                                result = false;
                                break;
                            }

                            result = false;
                            break;
                        }


                    default:
                        result = true;
                        break;
                }

                ListPool<string>.Shared.Return(list);
                return result;
            }
            catch (Exception e)
            {
                Log.Error($"{typeof(Scp079InteractTesla).FullName}.{nameof(Prefix)}:\n{e}");
                return true;
            }
        }
    }
}
