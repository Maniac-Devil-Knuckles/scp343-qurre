using CommandSystem;
using MEC;
using RemoteAdmin;
using System;
using Qurre.API;
using System.Linq;
using UnityEngine;

namespace SCP343.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Spawn343 : ParentCommand, IUsageProvider
    {
        public Spawn343() => LoadGeneratedCommands();
        public override string Command => "spawn343";

        public override string[] Aliases => new string[] { "spawnscp343", "343" };

        public override string Description => "This command spawn scp343";

        public string[] Usage => new string[] { "PlayerName/PlayerId" };

        public override void LoadGeneratedCommands()
        {
            
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(PlayerPermissions.ForceclassWithoutRestrictions, out response))
            {
                return false;
            }
            
            if (arguments.Count < 1)
            {
                response = "Usage command : \"spawn343 PlayerId\"";
                return false;
            }

            Player player = null;

            if (int.TryParse(arguments.At(0), out int id)) player = Player.Get(id);
            else player = Player.Get(string.Join(" ", arguments));

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

            player.SetRole(RoleType.ClassD, false, CharacterClassManager.SpawnReason.ForceClass);

            Timing.CallDelayed(0.5f, () =>
            {
                API.Spawn343(player);
            });
            response = $"Made {player.Nickname} SCP-343";
            return true;
        }
    }
}
