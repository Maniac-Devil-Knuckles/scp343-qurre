using System;

namespace SCP343
{
	public class Translation
	{
        public string AlertText { get; internal set; } = "You are <color=red>SCP-343</color>. Check your client console on [~]";

        public string ConsoleText { get; internal set; } = "You are <color=red>scp343</color>:\n\n1. You can open all doors;\n\n2. You can transform weapons to first and kit;\n\n 3. You have a god mode.\n\n4. You can teleport to player by sending console command .tp343 or drop coin\n\n5.In 1 metre away you , you can heal players by sending command .heal343 or dropping adrenaline\n6. In 1 meter away you, you can revive any dead player sending command .revive343 or dropping SCP-500\n7. You can be invisible sending command .invis\nOr you can use items dropping instead of sending commands. If you drop scp-330 and looking at human then will gift random item";

        public string AlertBackTo_DClass { get; internal set; } = "You stopped being scp-343";
        
        public string AlertHeckErrorTime { get; internal set; } = "Time is left.";
        
        public string AlertHeckError_IsNot343 { get; internal set; } = "Wait, you are not scp-343";

        public string HeckErrorDisable { get; internal set; } = ".heck343 is disabled by config";

        public string Text_Show_Timer_When_Can_Open_Door { get; internal set; } = "In {343_time_open_door} seconds you can open door";

        public string NotFoundPlayer { get; internal set; } = "Not found players!";

        public string Teleport_To_Player { get; internal set; } = "You teleported to %player% playing as %role%";

        public string HealPlayer { get; internal set; } = "You healed players health";

        public string CoolDown { get; internal set; } = "Please wait %seconds% seconds for healing another players";

        public string CanNotRevive { get; internal set; } = "You can not revive players";

        public string PlayerWhoRevived { get; internal set; } = "You was revived by SCP-343";

        public string Revive_Text { get; internal set; } = "You revived %user%";

        public string YouMustExit914 { get; internal set; } = "You must exit SCP-914";

        public string YouWereTranq { get; internal set; } = "You were shooted by SCP-343 using TranquilizerGun";

        public string Is_Invisible_True { get; internal set; } = "You are now is invisible for all";

        public string Is_Invisible_False { get; internal set; } = "You are not is invisible for all";

        public string End_Cooldown { get; internal set; } = "You can now heal another players!";

        public string ShootCoolDownText { get; internal set; } = "Please wait %seconds% seconds before shooting";
    }
}
