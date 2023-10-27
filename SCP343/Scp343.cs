
using System;
using Qurre.API;
using HarmonyLib;
using Qurre.API.Attributes;

namespace SCP343
{
    [PluginInit("SCP-343","Maniac Devil Knuckles","5.0.0")]
    public static class Scp343
    {
        public static Harmony harmony { get; internal set; } = null;
        
        internal static int i = 0;

        [PluginEnable]
        public static void Enable()
        {
            try
            {
                Config.Reload();
                if (!Config.IsEnabled)
                {
                    Log.Info("Disabled plugin by Config");
                    Disable();
                    return;
                }
                try
                {
                    harmony = new Harmony("knuckles.scp343\nVersion " + i++);
                    harmony.PatchAll();
                    Log.Info("cool");
                }
                catch (Exception ex)
                {
                    Log.Info("error\n\n\n\n\n\n\n\\n\n");
                    Log.Info(ex);//
                }
                /*
                try
                {
                    if (Pathes.Plugins.Contains("scp035"))
                    {
                        Log.Info("Patching method scp035...");
                        PluginInit plugin = PluginManager.plugins.First(p => p.Name == "scp035" && p.Developer == "fydne");
                        var pickupMethod =
                            AccessTools.Method(
                                plugin.GetType().Assembly.GetTypes().First(t => t.IsClass && t.Name == "EventHandlers"),
                                "PickupItem");

                        var patchprefix = AccessTools.Method(typeof(Scp035), nameof(Scp035.Prefix));
                        harmony.Patch(pickupMethod, new HarmonyMethod(patchprefix));
                        Log.Info("Successfully");
                    }
                }
                catch (Exception e)
                {
                    Log.Info("smth not working");
                    Log.Error(e);
                }
                */
                Log.Info("Enabling SCP343 by Maniac Devil Knuckles");
                
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        [PluginDisable]
        public static void Disable()
        {
            Log.Info("Disabling SCP343 by Maniac Devil Knuckles");
            if (harmony != null) harmony.UnpatchAll(harmony.Id);
            harmony = null;
        }

    }
}
