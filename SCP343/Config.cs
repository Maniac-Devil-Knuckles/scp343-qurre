﻿using System;
namespace SCP343
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using Qurre;
    using Qurre.API;
    using SCP343;

    public sealed class Config
    {

        [Description("Indicates whether the plugin is enabled or not")]
        public bool IsEnabled { get; set; } = true;

        [Description("scp343 can escape?")]
        public bool scp343_canescape { get; set; } = false;

        [Description("scp343 can open doors?")]
        public bool scp343_canopenanydoor { get; set; } = true;

        [Description("What broadcasted who become scp343")]
        public string scp343_alerttext { get; set; } = "You are <color=red>SCP-343</color>. Check your client console on [~]";

        [Description("Will or will not broadcast")]
        public bool scp343_alert { get; set; } = true;
        [Description("What 343 is shown if scp343_broadcast is true.")]
        public string scp343_consoletext { get; set; } = "You are <color=red>scp343</color>:\n\n1. You can open all doors;\n\n2. You can transform weapons to first and kit;\n\n 3. You have a god mode.\n\n4. You can teleport to player by sending console command .tp343 or drop ammo\n\n5.In 1 metre away you , you can heal players by sending command .heal343 or dropping adrenaline\n6. In 1 meter away you, you can revive any dead player sending command .revive343 or dropping flashlight";
        [Description("What 343 is shown if scp343 will back to usual class d")]
        public string scp343_alertbackd { get; set; } = "You stopped being scp-343";
        public string scp343_alertheckerrortime { get; set; } = "Time is left.";
        public string scp343_alertheckerrornot343 { get; set; } = "Wait, you are not scp-343";
        [Description("When 343 spawns should that person be given information about 343")]
        public bool scp343_console { get; set; } = true;
        [Description("Should players be allowed to use the .heck343 client command to respawn themselves as d-class within scp343_hecktime seconds of round start.")]
        public bool scp343_heck { get; set; } = true;
        [Description("How long people should beable to respawn themselves as d-class.")]
        public int scp343_hecktime { get; set; } = 30;
        [Description("If scp343_heck is false, what should send in console")]
        public string scp343_heckerrordisable { get; set; } = ".heck343 is disabled by config";
        [Description("Should SPC-343 beable to interact with the nuke.")]
        public bool scp343_nuke_interact { get; set; } = true;
        [Description("How long in seconds till SPC-343 can open any door.")]
        public int scp343_opendoortime { get; set; } = 30;
        [Description("Should SPC-343 convert items?")]
        public bool scp343_itemconverttoggle { get; set; } = true;
        [Description("Percent chance for SPC-343 to spawn at the start of the round.")]
        public float scp343_spawnchance { get; set; } = 30f;
        [Description("What items SCP-343 drops instead of picking up.")]
        public int[] scp343_itemdroplist { get; set; } = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 15, 19, 12, 19, 22, 27, 28, 29, 32, 33 };
        [Description("What items SCP-343 converts.")]
        public int[] scp343_itemstoconvert { get; set; } = new int[] { 10, 13, 14, 16, 20, 21, 23, 24, 25, 26, 30, 35 };
        [Description("What a item should be converted to.")]
        public int[] scp343_converteditems { get; set; } = new int[] { 14 };
        [Description("Minimum players for ")]
        public int minplayers { get; set; } = 5;
        [Description("What give scp-343 on spawn")]
        public int[] scp343_itemsatspawn { get; set; } = new int[] { 22, 33, 15 };
        [Description("Moving Speed lift for all players")]
        public float lift_moving_speed { get; set; } = 6.5f;

        private static Config cfg => scp343.cfg;
        public static void Reload()
        {
            var conf = Plugin.Config;
            cfg.IsEnabled = conf.GetBool("scp343_IsEnabled", true);
            cfg.scp343_canescape = conf.GetBool("scp343_canescape", true);
            cfg.scp343_alerttext = conf.GetString("scp343_alerttext", cfg.scp343_alerttext);
            cfg.scp343_consoletext = conf.GetString("scp343_consoletext", cfg.scp343_consoletext);
            cfg.scp343_alertbackd = conf.GetString("scp343_alertbackd", cfg.scp343_alertbackd);
            cfg.scp343_alertheckerrortime = conf.GetString("scp343_alertheckerrortime", cfg.scp343_alertheckerrortime);
            cfg.scp343_alertheckerrornot343 = conf.GetString("scp343_alertheckerrornot343", cfg.scp343_alertheckerrornot343);
            cfg.scp343_hecktime = conf.GetInt("scp343_hecktime", cfg.scp343_hecktime);
            cfg.scp343_nuke_interact = conf.GetBool("scp343_nuke_interact", cfg.scp343_nuke_interact);
            cfg.scp343_spawnchance = conf.GetFloat("scp343_spawnchance", cfg.scp343_spawnchance);
            cfg.scp343_itemdroplist = conf.GetIntList("scp343_itemdroplist").ToArray();
            cfg.scp343_opendoortime = conf.GetInt("scp343_opendoortime", cfg.scp343_opendoortime);
            cfg.scp343_itemstoconvert = conf.GetIntList("scp343_itemstoconvert").ToArray();
            cfg.scp343_converteditems = conf.GetIntList("scp343_converteditems").ToArray();
            cfg.scp343_itemsatspawn = conf.GetIntList("scp343_itemsatspawn").ToArray();
            cfg.lift_moving_speed = conf.GetFloat("scp343_lift_moving_speed", cfg.lift_moving_speed);
            cfg.scp343_canopenanydoor = conf.GetBool("scp343_canopenanydoor", cfg.scp343_canopenanydoor);
            cfg.scp343_alert = conf.GetBool("scp343_alert", true);
            cfg.scp343_console = conf.GetBool("scp343_console", true);
            cfg.scp343_heck = conf.GetBool("scp343_heck", cfg.scp343_heck);
            cfg.scp343_heckerrordisable = conf.GetString("scp343_heckerrordisable", cfg.scp343_heckerrordisable);
            cfg.scp343_itemconverttoggle = conf.GetBool("scp343_itemconverttoggle", cfg.scp343_itemconverttoggle);
            cfg.minplayers = conf.GetInt("scp343_minplayers", cfg.minplayers);
        }
    }
}