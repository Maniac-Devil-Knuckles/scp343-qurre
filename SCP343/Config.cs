using System;
using System.Collections.Generic;
using System.ComponentModel;
using Qurre;
namespace SCP343
{
    public sealed class Config
    {
        [Description("Indicates whether the plugin is enabled or not")]
        public bool IsEnabled { get; internal set; } = true;

        [Description("scp343 can escape?")]
        public bool scp343_canescape { get; internal set; } = false;

        [Description("scp343 can open doors?")]
        public bool scp343_canopenanydoor { get; internal set; } = true;

        [Description("What broadcasted who become scp343")]
        public string scp343_alerttext { get; internal set; } = "You are <color=red>SCP-343</color>. Check your client console on [~]";

        [Description("Will or will not broadcast")]
        public bool scp343_alert { get; internal set; } = true;
        [Description("What 343 is shown if scp343_broadcast is true.")]
        public string scp343_consoletext { get; internal set; } = "You are <color=red>scp343</color>:\n\n1. You can open all doors;\n\n2. You can transform weapons to first and kit;\n\n 3. You have a god mode.\n\n4. You can teleport to player by sending console command .tp343 or drop coin\n\n5.In 1 metre away you , you can heal players by sending command .heal343 or dropping adrenaline\n6. In 1 meter away you, you can revive any dead player sending command .revive343 or dropping flashlight\n7. You can be invisible sending command .invis\nOr you can use items dropping instead of sending commands";
        [Description("What 343 is shown if scp343 will back to usual class d")]
        public string scp343_alertbackd { get; internal set; } = "You stopped being scp-343";
        public string scp343_alertheckerrortime { get; internal set; } = "Time is left.";
        public string scp343_alertheckerrornot343 { get; internal set; } = "Wait, you are not scp-343";
        [Description("When 343 spawns should that person be given information about 343")]
        public bool scp343_console { get; internal set; } = true;
        [Description("Should players be allowed to use the .heck343 client command to respawn themselves as d-class within scp343_hecktime seconds of round start.")]
        public bool scp343_heck { get; internal set; } = true;
        [Description("How long people should beable to respawn themselves as d-class.")]
        public int scp343_hecktime { get; internal set; } = 30;
        [Description("If scp343_heck is false, what should send in console")]
        public string scp343_heckerrordisable { get; internal set; } = ".heck343 is disabled by config";
        [Description("Should SPC-343 beable to interact with the nuke.")]
        public bool scp343_nuke_interact { get; internal set; } = true;
        [Description("How long in seconds till SPC-343 can open any door.")]
        public int scp343_opendoortime { get; internal set; } = 30;
        [Description("Should SPC-343 convert items?")]
        public bool scp343_itemconverttoggle { get; internal set; } = true;
        [Description("Percent chance for SPC-343 to spawn at the start of the round.")]
        public float scp343_spawnchance { get; internal set; } = 30f;
        [Description("What items SCP-343 drops instead of picking up.")]
        public List<int> scp343_itemdroplist { get; internal set; } = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 15, 19, 12, 19, 22, 27, 28, 29, 32, 33 };
        [Description("What items SCP-343 converts.")]
        public List<int> scp343_itemstoconvert { get; internal set; } = new List<int> { 10, 13, 14, 16, 20, 21, 23, 24, 25, 26, 30, 35 };
        [Description("What a item should be converted to.")]
        public List<int> scp343_converteditems { get; internal set; } = new List<int> { 14 };
        [Description("Minimum players for spawn SCP-343")]
        public int minplayers { get; internal set; } = 5;
        [Description("What give scp-343 on spawn")]
        public List<int> scp343_itemsatspawn { get; internal set; } = new List<int> { 35, 33, 15, 32, 13 };
        [Description("Moving Speed lift for all players")]
        public float lift_moving_speed { get; internal set; } = 6.5f;

        [Description("UnitName for scp-343")]
        public string scp343_unitname { get; internal set; } = "SCP-343";

        [Description("Tesla is activating if SCP-343 in range?")]

        public bool scp343_activating_tesla_in_range { get; internal set; } = true;

        public bool scp343_turned_for_scp173_andscp096 { get; internal set; } = true;

        public bool scp343_invisible_for_173 { get; internal set; } = false;

        public bool scp343_show_timer_when_can_open_door { get; internal set; } = false;

        public string scp343_text_show_timer_when_can_open_door { get; internal set; } = "In {343_time_open_door} seconds you can open door";

        public bool scp343_interact_scp914 { get; internal set; } = false;

        public int scp343_min_heal_players { get; internal set; } = 30;

        public int scp343_max_heal_players { get; internal set; } = 70;

        public bool scp343_can_use_TranquilizerGun { get; internal set; } = true;

        public List<int> scp343_itemscannotdrop { get; internal set; } = new List<int> { 35, 33, 15, 32, 13  };

        public string scp343_notfoundplayer { get; internal set; } = "Not found players!";

        public string scp343_teleport_to_player { get; internal set; } = "You teleported to %player% playing as %role%";

        public string scp343_healplayer { get; internal set; } = "You healed players health";

        public string scp343_cooldown { get; internal set; } = "Please wait %seconds% seconds for healing another players";

        public string scp343_cannotrevive { get; internal set; } = "You can not revive players";

        public string scp343_playerwhorevived { get; internal set; } = "You was revived by SCP-343";

        public string scp343_revive_text { get; internal set; } = "You revived %user%";

        public string scp343_youmustexit914 { get; internal set; } = "You must exit SCP-914";

        public string scp343_youweretranq { get; internal set; } = "You were shooted by SCP-343 using TranquilizerGun";

        public int scp343_max_revive_count { get; internal set; } = 3;

        public string   scp343_is_invisible_true { get; internal set; } = "You are now is invisible for all";

        public string scp343_is_invisible_false { get; internal set; } = "You are not is invisible for all";

        public bool scp343_can_visibled_while_speaking { get; internal set; } = true;

        public int scp343_HealCooldown { get; internal set; } = 120;

        public string scp343_end_cooldown { get; internal set; } = "You can now heal another players!";

        internal void Reload()
        {
            Plugin.Config.Reload();
            var conf = Plugin.Config;
            IsEnabled = conf.GetBool("scp343_IsEnabled", true, "IsEnabled?");
            scp343_canescape = conf.GetBool("scp343_canescape", false);
            scp343_alerttext = conf.GetString("scp343_alerttext", scp343_alerttext);
            scp343_consoletext = conf.GetString("scp343_consoletext", scp343_consoletext);
            scp343_alertbackd = conf.GetString("scp343_alertbackd", scp343_alertbackd);
            scp343_alertheckerrortime = conf.GetString("scp343_alertheckerrortime", scp343_alertheckerrortime);
            scp343_alertheckerrornot343 = conf.GetString("scp343_alertheckerrornot343", scp343_alertheckerrornot343);
            scp343_hecktime = conf.GetInt("scp343_hecktime", scp343_hecktime);
            scp343_nuke_interact = conf.GetBool("scp343_nuke_interact", scp343_nuke_interact);
            scp343_spawnchance = conf.GetFloat("scp343_spawnchance", scp343_spawnchance);
            scp343_itemdroplist = conf.GetIntList("scp343_itemdroplist",scp343_itemdroplist);
            scp343_opendoortime = conf.GetInt("scp343_opendoortime", scp343_opendoortime);
            scp343_itemstoconvert = conf.GetIntList("scp343_itemstoconvert", scp343_itemstoconvert);
            scp343_converteditems = conf.GetIntList("scp343_converteditems", scp343_converteditems);
            scp343_itemsatspawn = conf.GetIntList("scp343_itemsatspawn", scp343_itemsatspawn);
            lift_moving_speed = conf.GetFloat("scp343_lift_moving_speed", lift_moving_speed);
            scp343_canopenanydoor = conf.GetBool("scp343_canopenanydoor", scp343_canopenanydoor);
            scp343_alert = conf.GetBool("scp343_alert", true);
            scp343_console = conf.GetBool("scp343_console", true);
            scp343_heck = conf.GetBool("scp343_heck", scp343_heck);
            scp343_heckerrordisable = conf.GetString("scp343_heckerrordisable", scp343_heckerrordisable);
            scp343_itemconverttoggle = conf.GetBool("scp343_itemconverttoggle", scp343_itemconverttoggle);
            minplayers = conf.GetInt("scp343_minplayers", minplayers);
            scp343_unitname = conf.GetString("scp343_unitname", scp343_unitname);
            scp343_activating_tesla_in_range = conf.GetBool("scp343_activating_tesla_in_range", scp343_activating_tesla_in_range, "If scp343 in range of the tesla");
            scp343_invisible_for_173 = conf.GetBool("scp343_invisible_for_173", false);
            scp343_turned_for_scp173_andscp096 = conf.GetBool("scp343_turned_for_scp173_andscp096", true);
            scp343_show_timer_when_can_open_door = conf.GetBool("scp343_show_timer_when_can_open_door", false);
            scp343_text_show_timer_when_can_open_door = conf.GetString("scp343_text_show_timer_when_can_open_door", scp343_text_show_timer_when_can_open_door);
            scp343_interact_scp914 = conf.GetBool("scp343_interact_scp914", false, "Can scp-343 interact with scp-914");
            scp343_min_heal_players = conf.GetInt("scp343_min_heal_players", 30);
            scp343_max_heal_players = conf.GetInt("scp343_max_heal_players", 70);
            scp343_can_use_TranquilizerGun = conf.GetBool("scp343_can_use_TranquilizerGun", true);
            scp343_itemscannotdrop = conf.GetIntList("scp343_itemscannotdrop", scp343_itemscannotdrop);
            scp343_notfoundplayer = conf.GetString("scp343_notfoundplayer", scp343_notfoundplayer);
            scp343_teleport_to_player = conf.GetString("scp343_teleport_to_player", scp343_teleport_to_player);
            scp343_healplayer = conf.GetString("scp343_healplayer", scp343_healplayer);
            scp343_cooldown = conf.GetString("scp343_cooldown", scp343_cooldown);
            scp343_cannotrevive = conf.GetString("scp343_cannotrevive", scp343_cannotrevive);
            scp343_playerwhorevived = conf.GetString("scp343_playerwhorevived", scp343_playerwhorevived);
            scp343_revive_text = conf.GetString("scp343_revive_text", scp343_revive_text);
            scp343_youmustexit914 = conf.GetString("scp343_youmustexit914", scp343_youmustexit914);
            scp343_max_revive_count = conf.GetInt("scp343_max_revive_count", 3, "How many SCP-343 can revive players?");
            scp343_can_visibled_while_speaking = conf.GetBool("scp343_can_visibled_while_speaking", true);
            scp343_HealCooldown = conf.GetInt("scp343_heal_cooldown", 120, "Cooldown after healing players");
            scp343_end_cooldown = conf.GetString("scp343_end_cooldown", scp343_end_cooldown);
        }
    }
}
