using System;
using Qurre.API;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Reflection;
using System.Linq;
using scp = SCP343.scp343;
using SCP343.Handlers;
using Qurre;

namespace SCP343
{
    public static class ExtentionMethods
    {
        public static bool IsSCP343(this Player player) => scp343badgelist.Contains(player);

        public static Badge GetSCPBadge(this Player player)
        {
            Badge badge = null;
            if (player.IsSCP343()) badge = API.AllScp343Badges.FirstOrDefault(x => x.Player.Id == player.Id);
            return badge;
        }

    }
}
