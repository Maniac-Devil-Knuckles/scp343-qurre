using System;
using Qurre.API;
using Qurre.API.Events;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using Qurre;
using System.Linq;

namespace SCP343.Patche
{
    [HarmonyPatch(typeof(TeslaGateController), nameof(TeslaGateController.FixedUpdate))]
    internal static class ExtraTeslaPAtch
    {
        private static bool Prefix(TeslaGateController __instance)
        {
            try
            {
                foreach (TeslaGate teslaGate in __instance.TeslaGates)
                {
                    if (teslaGate.InProgress)
                        continue;
                    IEnumerable<ReferenceHub> hubs = ReferenceHub.GetAllHubs().Where(h => Player.Get(h.Value).Role != RoleType.Spectator && teslaGate.PlayerInRange(h.Value)).Select(h => h.Value);
                    bool allowed = false;
                    if (hubs.Count() > 0)
                    {
                        if (hubs.Any(h => Player.Get(h).IsSCP343())) allowed = !scp343.cfg.scp343_activating_tesla_in_range;
                    }
                    if (allowed) teslaGate.ServerSideCode();
                }
                return false;
            }
            catch (Exception e)
            {
                Log.Error($"TriggeringTesla patch: {e}\n{e.StackTrace}");

                return true;
            }
        }
    }
}
