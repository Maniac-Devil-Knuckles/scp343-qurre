using System;
using System.Collections.Generic;
using System.ComponentModel;
using Qurre;
namespace SCP343
{
    public static class Cfg
    {
        public static bool IsEnabled { get; internal set; } = true;

        public static bool scp343_canescape { get; internal set; } = false;

        public static bool scp343_canopenanydoor { get; internal set; } = true;

        public static string scp343_alerttext { get; internal set; } = "You are <color=red>SCP-343</color>. Check your client console on [~]";

        public static bool scp343_alert { get; internal set; } = true;

        public static string scp343_consoletext { get; internal set; } = "You are <color=red>scp343</color>:\n\n1. You can open all doors;\n\n2. You can transform weapons to first and kit;\n\n 3. You have a god mode.\n\n4. You can teleport to player by sending console command .tp343 or drop coin\n\n5.In 1 metre away you , you can heal players by sending command .heal343 or dropping adrenaline\n6. In 1 meter away you, you can revive any dead player sending command .revive343 or dropping flashlight\n7. You can be invisible sending command .invis\nOr you can use items dropping instead of sending commands. If you drop scp-330 and looking at human then will gift random item";

        public static string scp343_alertbackd { get; internal set; } = "You stopped being scp-343";
        public static string scp343_alertheckerrortime { get; internal set; } = "Time is left.";
        public static string scp343_alertheckerrornot343 { get; internal set; } = "Wait, you are not scp-343";

        public static bool scp343_console { get; internal set; } = true;

        public static bool scp343_heck { get; internal set; } = true;

        public static int scp343_hecktime { get; internal set; } = 30;

        public static string scp343_heckerrordisable { get; internal set; } = ".heck343 is disabled by config";

        public static bool scp343_nuke_interact { get; internal set; } = true;

        public static int scp343_opendoortime { get; internal set; } = 30;

        public static bool scp343_itemconverttoggle { get; internal set; } = true;

        public static float scp343_spawnchance { get; internal set; } = 30f;

        public static List<ItemType> scp343_itemdroplist { get; internal set; } = new List<ItemType> { ItemType.KeycardJanitor, ItemType.KeycardScientist, ItemType.KeycardResearchCoordinator, ItemType.KeycardZoneManager, ItemType.KeycardGuard, ItemType.KeycardNTFOfficer, ItemType.KeycardContainmentEngineer, ItemType.KeycardNTFLieutenant, ItemType.KeycardNTFCommander, ItemType.KeycardFacilityManager, ItemType.KeycardChaosInsurgency , ItemType.KeycardO5, ItemType.Flashlight, ItemType.Radio, ItemType.Ammo556x45, ItemType.Ammo44cal, ItemType.Ammo762x39, ItemType.Ammo9x19, ItemType.SCP268, ItemType.Adrenaline };

        public static List<ItemType> scp343_itemstoconvert { get; internal set; } = new List<ItemType> { ItemType.GunCOM15, ItemType.Medkit, ItemType.MicroHID, ItemType.GunE11SR, ItemType.GunCrossvec, ItemType.GunFSP9, ItemType.GunLogicer, ItemType.GrenadeHE, ItemType.GrenadeFlash, ItemType.GunCOM18, ItemType.Coin };

        public static List<ItemType> scp343_converteditems { get; internal set; } = new List<ItemType> { ItemType.Medkit };

        public static int minplayers { get; internal set; } = 5;

        public static List<ItemType> scp343_itemsatspawn { get; internal set; } = new List<ItemType> { ItemType.Coin, ItemType.Adrenaline, ItemType.Flashlight, ItemType.SCP268, ItemType.GunCOM15, ItemType.SCP330 };

        public static float lift_moving_speed { get; internal set; } = 6.5f;

        public static string scp343_unitname { get; internal set; } = "SCP-343";

        public static bool scp343_activating_tesla_in_range { get; internal set; } = true;

        public static bool scp343_turned_for_scp173_andscp096 { get; internal set; } = true;

        public static bool scp343_invisible_for_173 { get; internal set; } = false;

        public static bool scp343_show_timer_when_can_open_door { get; internal set; } = false;

        public static string scp343_text_show_timer_when_can_open_door { get; internal set; } = "In {343_time_open_door} seconds you can open door";

        public static bool scp343_interact_scp914 { get; internal set; } = false;

        public static int scp343_min_heal_players { get; internal set; } = 30;

        public static int scp343_max_heal_players { get; internal set; } = 70;

        public static bool scp343_can_use_TranquilizerGun { get; internal set; } = true;

        public static List<ItemType> scp343_itemscannotdrop { get; internal set; } = new List<ItemType> { ItemType.Coin, ItemType.Adrenaline, ItemType.Flashlight, ItemType.SCP268, ItemType.GunCOM15, ItemType.SCP330 };

        public static string scp343_notfoundplayer { get; internal set; } = "Not found players!";

        public static string scp343_teleport_to_player { get; internal set; } = "You teleported to %player% playing as %role%";

        public static string scp343_healplayer { get; internal set; } = "You healed players health";

        public static string scp343_cooldown { get; internal set; } = "Please wait %seconds% seconds for healing another players";

        public static string scp343_cannotrevive { get; internal set; } = "You can not revive players";

        public static string scp343_playerwhorevived { get; internal set; } = "You was revived by SCP-343";

        public static string scp343_revive_text { get; internal set; } = "You revived %user%";

        public static string scp343_youmustexit914 { get; internal set; } = "You must exit SCP-914";

        public static string scp343_youweretranq { get; internal set; } = "You were shooted by SCP-343 using TranquilizerGun";

        public static int scp343_max_revive_count { get; internal set; } = 3;

        public static string scp343_is_invisible_true { get; internal set; } = "You are now is invisible for all";

        public static string scp343_is_invisible_false { get; internal set; } = "You are not is invisible for all";

        public static bool scp343_can_visibled_while_speaking { get; internal set; } = true;

        public static int scp343_HealCooldown { get; internal set; } = 120;

        public static string scp343_end_cooldown { get; internal set; } = "You can now heal another players!";

        internal static void Reload()
        {
            Log.Info("Loading Configs scp-343.....");
            Plugin.Config.Reload();
            IsEnabled = Plugin.Config.GetBool("scp343_IsEnabled", true, "IsEnabled?");
            scp343_canescape = Plugin.Config.GetBool("scp343_canescape", false);
            scp343_alerttext = Plugin.Config.GetString("scp343_alerttext", scp343_alerttext);
            scp343_consoletext = Plugin.Config.GetString("scp343_consoletext", scp343_consoletext);
            scp343_alertbackd = Plugin.Config.GetString("scp343_alertbackd", scp343_alertbackd);
            scp343_alertheckerrortime = Plugin.Config.GetString("scp343_alertheckerrortime", scp343_alertheckerrortime);
            scp343_alertheckerrornot343 = Plugin.Config.GetString("scp343_alertheckerrornot343", scp343_alertheckerrornot343);
            scp343_hecktime = Plugin.Config.GetInt("scp343_hecktime", scp343_hecktime);
            scp343_nuke_interact = Plugin.Config.GetBool("scp343_nuke_interact", scp343_nuke_interact);
            scp343_spawnchance = Plugin.Config.GetFloat("scp343_spawnchance", scp343_spawnchance);
            scp343_itemdroplist = Plugin.Config.GetListEnum("scp343_itemdroplist",scp343_itemdroplist);
            scp343_opendoortime = Plugin.Config.GetInt("scp343_opendoortime", scp343_opendoortime);
            scp343_itemstoconvert = Plugin.Config.GetListEnum("scp343_itemstoconvert", scp343_itemstoconvert);
            scp343_converteditems = Plugin.Config.GetListEnum("scp343_converteditems", scp343_converteditems);
            scp343_itemsatspawn = Plugin.Config.GetListEnum("scp343_itemsatspawn", scp343_itemsatspawn);
            lift_moving_speed = Plugin.Config.GetFloat("scp343_lift_moving_speed", lift_moving_speed);
            scp343_canopenanydoor = Plugin.Config.GetBool("scp343_canopenanydoor", scp343_canopenanydoor);
            scp343_alert = Plugin.Config.GetBool("scp343_alert", true);
            scp343_console = Plugin.Config.GetBool("scp343_console", true);
            scp343_heck = Plugin.Config.GetBool("scp343_heck", scp343_heck);
            scp343_heckerrordisable = Plugin.Config.GetString("scp343_heckerrordisable", scp343_heckerrordisable);
            scp343_itemconverttoggle = Plugin.Config.GetBool("scp343_itemconverttoggle", scp343_itemconverttoggle);
            minplayers = Plugin.Config.GetInt("scp343_minplayers", minplayers);
            scp343_unitname = Plugin.Config.GetString("scp343_unitname", scp343_unitname);
            scp343_activating_tesla_in_range = Plugin.Config.GetBool("scp343_activating_tesla_in_range", scp343_activating_tesla_in_range, "If scp343 in range of the tesla");
            scp343_invisible_for_173 = Plugin.Config.GetBool("scp343_invisible_for_173", false);
            scp343_turned_for_scp173_andscp096 = Plugin.Config.GetBool("scp343_turned_for_scp173_andscp096", true);
            scp343_show_timer_when_can_open_door = Plugin.Config.GetBool("scp343_show_timer_when_can_open_door", false);
            scp343_text_show_timer_when_can_open_door = Plugin.Config.GetString("scp343_text_show_timer_when_can_open_door", scp343_text_show_timer_when_can_open_door);
            scp343_interact_scp914 = Plugin.Config.GetBool("scp343_interact_scp914", false, "Can scp-343 interact with scp-914");
            scp343_min_heal_players = Plugin.Config.GetInt("scp343_min_heal_players", 30);
            scp343_max_heal_players = Plugin.Config.GetInt("scp343_max_heal_players", 70);
            scp343_can_use_TranquilizerGun = Plugin.Config.GetBool("scp343_can_use_TranquilizerGun", true);
            scp343_itemscannotdrop = Plugin.Config.GetListEnum("scp343_itemscannotdrop", scp343_itemscannotdrop);
            scp343_notfoundplayer = Plugin.Config.GetString("scp343_notfoundplayer", scp343_notfoundplayer);
            scp343_teleport_to_player = Plugin.Config.GetString("scp343_teleport_to_player", scp343_teleport_to_player);
            scp343_healplayer = Plugin.Config.GetString("scp343_healplayer", scp343_healplayer);
            scp343_cooldown = Plugin.Config.GetString("scp343_cooldown", scp343_cooldown);
            scp343_cannotrevive = Plugin.Config.GetString("scp343_cannotrevive", scp343_cannotrevive);
            scp343_playerwhorevived = Plugin.Config.GetString("scp343_playerwhorevived", scp343_playerwhorevived);
            scp343_revive_text = Plugin.Config.GetString("scp343_revive_text", scp343_revive_text);
            scp343_youmustexit914 = Plugin.Config.GetString("scp343_youmustexit914", scp343_youmustexit914);
            scp343_max_revive_count = Plugin.Config.GetInt("scp343_max_revive_count", 3, "How many SCP-343 can revive players?");
            scp343_can_visibled_while_speaking = Plugin.Config.GetBool("scp343_can_visibled_while_speaking", true);
            scp343_HealCooldown = Plugin.Config.GetInt("scp343_heal_cooldown", 120, "Cooldown after healing players");
        }
    }
}
