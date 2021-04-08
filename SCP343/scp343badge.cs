using System;
using System.Collections.Generic;
using System.Linq;
using Qurre.API;
using MEC;
using UnityEngine;
using System.Collections;

namespace SCP343
{
    public class Badge
    {
        internal Badge(Player player, bool scp343 = false, string SCPName = "")
        {
            Player = player;
            RoleColor = player.RoleColor;
            RoleName = player.RoleName;
            Id = player.Id;
            IsSCP343 = scp343;
            if (scp343) scp343badgelist.Add(this);
            else this.SCPName = SCPName;
        }

        public Badge(Player player, string SCPName = "")
        {
            Player = player;
            RoleColor = player.RoleColor;
            RoleName = player.RoleName;
            Id = player.Id;
            IsSCP343 = false;
            this.SCPName = SCPName;
        }

        public Badge(Player player, RoleType role, Vector3 pos)
        {
            Player = player;
            this.role = role;
            RoleColor = player.RoleColor;
            RoleName = player.RoleName;
            this.pos = pos;
        }

        internal Badge(int PlayerId, bool scp343 = false, string SCPName = "")
        {
            Player = Player.Get(PlayerId);
            RoleColor = Player.RoleColor;
            RoleName = Player.RoleName;
            Id = Player.Id;
            IsSCP343 = scp343;
            if (scp343) scp343badgelist.Add(this);
            else this.SCPName = SCPName;
        }
        internal Badge(string args, bool scp343 = false, string SCPName = "")
        {
            Player = Player.Get(args);
            RoleColor = Player.RoleColor;
            RoleName = Player.RoleName;
            Id = Player.Id;
            IsSCP343 = scp343;
            if (scp343) scp343badgelist.Add(this);
            else this.SCPName = SCPName;
        }
        internal Badge(GameObject GameObject, bool scp343 = false, string SCPName = "")
        {
            Player = Player.Get(GameObject);
            RoleColor = Player.RoleColor;
            RoleName = Player.RoleName;
            Id = Player.Id;
            IsSCP343 = scp343;
            if (scp343) scp343badgelist.Add(this);
            else this.SCPName = SCPName;
        }
        public string RoleColor { get; } = "";
        public string RoleName { get; } = "";
        public Player Player { get; } = null;
        public int Id { get; } = 0;
        public string UserId { get; } = null;
        internal bool heck { get; set; } = false;
        internal bool opendoor { get; set; } = false;
        public bool canopendoor => opendoor;
        public bool canheck => heck;
        public GameObject GameObject => Player.GameObject;
        public bool IsSCP343 { get; } = false;
        public string SCPName { get; } = "";
        internal int revive343 { get; set; } = 0;
        public RoleType role { get; } = RoleType.None;
        public Vector3 pos { get; } = Vector3.zero;
        internal bool canheal { get; set; } = false;

    }
    public static class scp343badgelist
    {
        private static Dictionary<int, Badge> badges { get; } = new Dictionary<int, Badge>();

        internal static IEnumerable<Player> List
        {
            get
            {
                IEnumerable<Player> players = Player.List.Where(e => e.IsSCP343());
                return players;
            }
        }

        internal static IEnumerable<Badge> ListBadges
        {
            get
            {
                IEnumerable<Badge> badge = badges.Values;
                return badge;
            }
        }

        internal static void Add(Badge scp343)
        {
            scp343.revive343 = 3;
            scp343.canheal = true;
            badges.Add(scp343.Id, scp343);
        }
        internal static bool Remove(Player player) => badges.Remove(player.Id);
        internal static bool Remove(int PlayerId) => badges.Remove(PlayerId);
        internal static void Clear() => badges.Clear();
        /// <summary>
        /// This returns if <see cref="Player"/> is scp343 or isn`t
        /// </summary>
        public static bool Contains(Player player) => badges.ContainsKey(player.Id);
        /// <summary>
        /// This returns if <see cref="Player.Id"/> is scp343 or isn`t
        /// </summary>
        public static bool Contains(int PlayerId) => badges.ContainsKey(PlayerId);
        /// <summary>
        /// Count of <see cref="scp343"/>
        /// </summary>
        public static int Count() => badges.Count(e => e.Value.Player.IsSCP343());

        /// <summary>
        /// Count of <see cref="scp343"/>
        /// </summary>
        public static int Count(Func<Badge, bool> predicate) => ListBadges.Count(predicate);

        /// <summary>
        /// Get Badge by <see cref="Player"/>
        /// </summary>
        /// <returns><seealso cref="Badge"/></returns>
        public static Badge Get(Player player) => ListBadges.FirstOrDefault(e => e.Id == player.Id);

        /// <summary>
        /// Get Badge by <see cref="Player.Id"/> and returns <seealso cref="Badge"/>
        /// </summary>
        public static Badge Get(int PlayerId) => ListBadges.FirstOrDefault(e => e.Id == PlayerId);

        /// <summary>
        /// Get Badge by <see cref="GameObject"/> and returns <seealso cref="Badge"/>
        /// </summary>
        public static Badge Get(GameObject GameObject) => ListBadges.FirstOrDefault(e => e.GameObject == GameObject);

        /// <summary>
        /// Get Badge by <see cref="ReferenceHub"/> and returns <seealso cref="Badge"/>
        /// </summary>
        public static Badge Get(ReferenceHub ReferenceHub) => ListBadges.FirstOrDefault(e => e.Player.ReferenceHub == ReferenceHub);

        /// <summary>
        /// Get Badge by prediacte and returns <seealso cref="Badge"/>
        /// </summary>
        public static Badge Get(Func<Badge, bool> predicate) => ListBadges.FirstOrDefault(predicate);
    }
}
