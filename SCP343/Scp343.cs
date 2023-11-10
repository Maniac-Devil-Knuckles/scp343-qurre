
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
using SCP343.Handlers;

namespace SCP343
{
    [PluginInit("SCP-343","Maniac Devil Knuckles","5.0.0.1")]
    public static class Scp343
    {
        public static Harmony harmony { get; internal set; } = null;
        
        internal static int i = 0;

        internal static Assembly Scp035 { get; set; } = null;
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
                PatchScp035();
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
                try
                {
                    foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        PluginInit plugin = null;
                        foreach (var type in assembly.GetTypes())
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
                        Eventhandlers.Scp035 = assembly;
                        Log.Info("Assembly Scp035 was found and linked);
                        break;
                    }
                }
                catch (Exception ex) { Log.Error(ex); }
        }
    }
}
