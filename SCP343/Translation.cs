using System;

namespace SCP343
{
	public class Translation
	{

        public string alerttext { get; internal set; } = "You are <color=red>SCP-343</color>. Check your client console on [~]";

        public string consoletext { get; internal set; } = "You are <color=red>scp343</color>:\n\n1. You can open all doors;\n\n2. You can transform weapons to first and kit;\n\n 3. You have a god mode.\n\n4. You can teleport to player by sending console command .tp343 or drop coin\n\n5.In 1 metre away you , you can heal players by sending command .heal343 or dropping adrenaline\n6. In 1 meter away you, you can revive any dead player sending command .revive343 or dropping flashlight\n7. You can be invisible sending command .invis\nOr you can use items dropping instead of sending commands. If you drop scp-330 and looking at human then will gift random item";

        public string alertbackd { get; internal set; } = "You stopped being scp-343";
        
        public string alertheckerrortime { get; internal set; } = "Time is left.";
        
        public string alertheckerrornot343 { get; internal set; } = "Wait, you are not scp-343";

        public string heckerrordisable { get; internal set; } = ".heck343 is disabled by config";

        public string unitname { get; internal set; } = "SCP-343";

        public string text_show_timer_when_can_open_door { get; internal set; } = "In {343_time_open_door} seconds you can open door";

        public string notfoundplayer { get; internal set; } = "Not found players!";

        public string teleport_to_player { get; internal set; } = "You teleported to %player% playing as %role%";

        public string healplayer { get; internal set; } = "You healed players health";

        public string cooldown { get; internal set; } = "Please wait %seconds% seconds for healing another players";

        public string cannotrevive { get; internal set; } = "You can not revive players";

        public string playerwhorevived { get; internal set; } = "You was revived by SCP-343";

        public string revive_text { get; internal set; } = "You revived %user%";

        public string youmustexit914 { get; internal set; } = "You must exit SCP-914";

        public string youweretranq { get; internal set; } = "You were shooted by SCP-343 using TranquilizerGun";

        public string is_invisible_true { get; internal set; } = "You are now is invisible for all";

        public string is_invisible_false { get; internal set; } = "You are not is invisible for all";

        public string end_cooldown { get; internal set; } = "You can now heal another players!";

        public string shootcooldowntext { get; internal set; } = "Please wait %seconds% seconds before shooting";

        public string class_d_unit { get; internal set; } = "Class D";
    }
}
