using System;
using Qurre.API;
using System.Collections.Generic;
using System.Linq;
using Qurre;

namespace SCP343
{
    public static class ExtentionMethods
    {
        public static void SetHP(this Player player, float value)
        {
            if (value + player.Hp >= player.MaxHp) player.Hp = player.MaxHp;
            else player.Hp += value;
        }

        public static bool IsSCP343(this Player player) => scp343badgelist.Contains(player);

        public static Badge GetSCPBadge(this Player player) => player.IsSCP343() ? scp343badgelist.Get(player) : null;

    }
}
