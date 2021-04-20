using CommandSystem;
using MEC;
using RemoteAdmin;
using System;
using Qurre.API;

namespace SCP343.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Spawn343 : ParentCommand
    {
        internal Spawn343() => LoadGeneratedCommands();
        public override string Command => "spawn343";

        public override string[] Aliases => new string[] { "spawnscp343", "343" };

        public override string Description => "This command spawn scp343";

        public override void LoadGeneratedCommands()
        {
            
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!sender.CheckPermission(PlayerPermissions.ForceclassWithoutRestrictions,out response))
            {
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Usage command : \"spawn343 PlayerId\"";
                return false;
            }
            string str = arguments.At(0);
                if (int.TryParse(str, out int PlayerId))
                {
                    if (PlayerId < 2)
                    {
                        response = "Usage command : \"spawn343 PlayerId\"";
                        return false;
                    }
                    Player player = Player.Get(PlayerId);
                    if (player == null)
                    {
                        response = "Incorrect PlayerId";
                        return false;
                    }
                    if (player.IsSCP343())
                    {
                        response = "This player already scp343";
                        return false;
                    }
                    player.SetRole(RoleType.ClassD, false, true);
                    Timing.CallDelayed(0.5f, () =>
                    {
                        scp343.Players.spawn343(player);
                    });
                    response = $"Made {player.Nickname} SCP-343";
                    return true;
                }
            response = "Usage command : \"spawn343 PlayerId\"";
            return false;
        }
    }
}
