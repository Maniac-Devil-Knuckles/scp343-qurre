using SCP343.Handlers;
using Qurre.API;
using System.Collections.Generic;
using System.Linq;

namespace SCP343
{
    public static class API
    {
        public static void SetHP(this Player player, float value)
        {
            if (value + player.Hp >= player.MaxHp) player.Hp = player.MaxHp;
            else player.Hp += value;
        }

        public static bool IsSCP343(this Player player) => scp343badgelist.Contains(player);

        public static Badge GetSCPBadge(this Player player) => player.IsSCP343() ? scp343badgelist.Get(player) : null;

        /// <summary>
        /// <para>This spawns <see cref="Player"/> as scp343</para>
        /// </summary>
        /// <returns><see cref="Badge"/></returns>
        public static Badge Spawn343(Player player, UnityEngine.Vector3 position = default) => Eventhandlers.spawn343(player, position: position);
        /// <summary>
        /// <para>Just a list</para>
        /// </summary>
        public static IEnumerable<Player> AllScps343 => AllScp343Badges.Select(b => b.Player);
        public static IEnumerable<Badge> AllScp343Badges => scp343badgelist.Get(b => b.IsSCP343);
        /// <summary>
        /// <para>This kills scp343</para>
        /// </summary>
        public static void Kill343(Player player) => Eventhandlers.KillSCP343(player);

    }
}
