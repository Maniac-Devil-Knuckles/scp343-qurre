using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.Events.Structs;
using SCP343.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SCP343.Patches
{
    internal static class Scp035
    {

        internal static Assembly Assembly { get; set; }

        internal static Type Logic
        {
            get => Assembly.GetTypes().First(t=> t.Name == "Logic" && t.IsClass);
        }

        internal static Type plugin
        {
            get => Assembly.GetTypes().First(t => t.IsClass && t.Name == "Plugin");
        }

        internal static bool Prefix(object __instance, PickupItemEvent ev)
        {
            if (!ev.Player.IsSCP343()) return true;
            Eventhandlers.PickupScp035 = true;
            if (Config.ItemsToConvert.Contains(ev.Pickup.Type) && Config.ItemsConvertToggle)
            {
                try
                {
                    var fieldinfo = plugin.GetField("Items", BindingFlags.Static | BindingFlags.Public);
                    var pickup = (List<Pickup>)fieldinfo.GetValue(null);
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
                    Logic.GetMethod("RefreshItems", BindingFlags.Static | BindingFlags.Public).Invoke(__instance, null);
                }
                catch (Exception exception)
                {
                    Log.Error(1);
                    Log.Info(exception);
                }
                ev.Pickup.Destroy();
                foreach(var item in Config.ConvertedItems) ev.Player.Inventory.AddItem(item);
                ev.Allowed = false;
            }
            return false;
        }
    }
}
