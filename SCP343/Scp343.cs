
using System;
using Qurre.API;
using HarmonyLib;
using Qurre.API.Attributes;
using SCP343.Commands;
using CommandSystem;
using RemoteAdmin;
using System.IO;
using System.Reflection;
using System.Linq;
using SCP343.Patches;

namespace SCP343
{
    [PluginInit("SCP-343","Maniac Devil Knuckles","5.0.0.1")]
    public static class Scp343
    {
        public static Harmony harmony { get; internal set; } = null;
        
        internal static int i = 0;

        internal static Spawn343 Spawn343 { get; set; } = null;

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
                Spawn343 = new Spawn343();
                CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(Spawn343);
                GameCore.Console.singleton.ConsoleCommandHandler.RegisterCommand(Spawn343);
                try
                {
                    PatchScp035();
                }
                catch (Exception e)
                {
                    Log.Info("smth not working");
                    Log.Error(e);
                }
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
            if (Spawn343 != null)
            {
                CommandProcessor.RemoteAdminCommandHandler.UnregisterCommand(Spawn343);
                GameCore.Console.singleton.ConsoleCommandHandler.UnregisterCommand(Spawn343);
            }
            Spawn343 = null;
            if (harmony != null) harmony.UnpatchAll(harmony.Id);
            harmony = null;
        }

        internal static void PatchScp035()
        {
            foreach (string pluginString in Directory.GetFiles(Pathes.Plugins))
            {
                try
                {
                    Assembly pl = Assembly.Load(LoaderManager.ReadFile(pluginString));
                    if (pl == null) continue;
                    PluginInit plugin = null;
                    foreach (var type in pl.GetTypes())
                    {
                        PluginInit init = type.GetCustomAttribute<PluginInit>();
                        if (init == null)
                            continue;
                        if (init.Developer == "DaNoNe" && init.Name == "SCP035")
                        {
                            plugin = init;
                            break;
                        }
                    }
                    if (plugin == null) continue;
                    var pickupMethod =
                        AccessTools.Method(
                            pl.GetTypes().First(t => t.IsClass && t.Name == "EventHandler"),
                            "PickupItemEvent");

                    Log.Info("Patching method scp035...");
                    Scp035.Assembly = pl;
                    var patchprefix = AccessTools.Method(typeof(Scp035), nameof(Scp035.Prefix));
                    harmony.Patch(pickupMethod, new HarmonyMethod(patchprefix));
                    Log.Info("Successfully");
                    break;
                }
                catch (Exception ex) { Log.Error(ex); }
            }
        }
    }
}
