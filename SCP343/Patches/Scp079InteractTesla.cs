using System;
using System.Collections.Generic;
using Qurre.API;
using Qurre.API.Events;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using NorthwoodLib.Pools;
using Qurre;
using Qurre.API.Controllers.Items;
using SCP343.Handlers;

namespace SCP343.Patches
{
    [HarmonyPatch(typeof(Scp079PlayerScript), "UserCode_CmdInteract")]
    internal static class Scp079InteractTesla
    {
        private static bool Prefix(Scp079PlayerScript __instance, Command079 command, string args, GameObject target)
        { 
            if (__instance._interactRateLimit.CanExecute()) return false;
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

    internal static class Scp035
    {
        internal static bool Prefix(object __instance, PickupItemEvent ev)
        {
            if (!ev.Player.IsSCP343()) return true;
            Eventhandlers.PickupScp035 = true;
            if (Scp343.CustomConfig.itemstoconvert.Contains(ev.Pickup.Type) && Scp343.CustomConfig.itemconverttoggle)
            {
                try
                {
                    var fieldinfo = __instance.GetType().GetField("Items", BindingFlags.Static | BindingFlags.NonPublic);
                    var pickup = (List<Pickup>) fieldinfo.GetValue(null);
                    pickup.Remove(ev.Pickup);
                    fieldinfo.SetValue(null, pickup);
                }
                catch (Exception ex)
                {
                    Log.Error(0);
                    Log.Info(ex);
                }

                try
                {
                    __instance.GetType().GetMethod("RefreshItems", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(__instance, null);
                }
                catch (Exception exception)
                {
                    Log.Error(1);
                    Log.Info(exception);
                }
                ev.Pickup.Destroy();
                ev.Player.AddItem(Scp343.CustomConfig.converteditems);
                ev.Allowed = false;
            }
            return false;
        }
    }
}
