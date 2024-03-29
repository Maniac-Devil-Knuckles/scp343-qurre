﻿using CommandSystem;
using MEC;
using System;
using Qurre.API;
using System.Linq;

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
                response = "Usage command : \"spawn343 PlayerId/UserId\"";
                return false;
            }

            Player player = Player.List.FirstOrDefault(p=>p.UserInfomation.Nickname == string.Join(" ", arguments) || p.UserInfomation.Id == int.Parse(string.Join(" ", arguments)) || p.UserInfomation.UserId == string.Join(" ", arguments)) ?? null;
            if (player == null || player.UserInfomation.Id == Server.Host.UserInfomation.Id)
            {
                response = "Incorrect PlayerId";
                return false;
            }
            if (player.IsSCP343())
            { 
                response = "This player already scp343";
                return false;
            }

            player.RoleInfomation.SetNew(PlayerRoles.RoleTypeId.ClassD, PlayerRoles.RoleChangeReason.RemoteAdmin);

            Timing.CallDelayed(0.5f, () =>
            {
                API.Spawn343(player);
            });
            response = $"Made {player.UserInfomation.Nickname} SCP-343";
            return true;
        }
    }
}
