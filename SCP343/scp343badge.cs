using System;
using System.Collections.Generic;
using System.Linq;
using Qurre.API;
using UnityEngine;
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
            this.SCPName = SCPName;
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

        public Badge(Player player, RoleType Role, Vector3 Pos)
        {
            Player = player;
            this.Role = Role;
            RoleColor = player.RoleColor;
            RoleName = player.RoleName;
            this.Pos = Pos;
        }

        internal Badge(int PlayerId, bool scp343 = false, string SCPName = "")
        {
            Player = Player.Get(PlayerId);
            RoleColor = Player.RoleColor;
            RoleName = Player.RoleName;
            Id = Player.Id;
            IsSCP343 = scp343;
            this.SCPName = SCPName;
        }

        internal Badge(string args, bool scp343 = false, string SCPName = "")
        {
            Player = Player.Get(args);
            RoleColor = Player.RoleColor;
            RoleName = Player.RoleName;
            Id = Player.Id;
            IsSCP343 = scp343;
            this.SCPName = SCPName;
        }

        internal Badge(GameObject GameObject, bool scp343 = false, string SCPName = "")
        {
            Player = Player.Get(GameObject);
            RoleColor = Player.RoleColor;
            RoleName = Player.RoleName;
            Id = Player.Id;
            IsSCP343 = scp343;
            this.SCPName = SCPName;
        }

        public string RoleColor { get; } = "";
        public string RoleName { get; } = "";
        public Player Player { get; } = null;
        public int Id { get; } = 0;
        public string UserId => Player.UserId;
        public bool CanOpenDoor { get; internal set; } = false;
        public bool CanHeck { get; internal set; } = false;
        public GameObject GameObject => Player.GameObject;
        public ReferenceHub ReferenceHub => Player.ReferenceHub;
        public bool IsSCP343
        {
            get => _IsScp343;
            private set
            {
                _IsScp343 = value;
                if (value) scp343badgelist.Add(this);
            }
        }
        public string SCPName { get; internal set; } = "";
        public int Revive343 { get; internal set; } = 0;
        public RoleType Role { get; } = RoleType.None;
        public Vector3 Pos { get; } = Vector3.zero;
        public bool CanHeal => HealCooldown <= 0;
        public int HealCooldown { get; internal set; } = 120;

        private bool _IsScp343 = false;
    }

    public static class scp343badgelist
    {
        private static readonly HashSet<Badge> badges = new HashSet<Badge>();

        internal static void Add(Badge scp343)
        {
            if (Contains(scp343.Id)) return;
            scp343.Revive343 = Cfg.scp343_max_revive_count;
            scp343.HealCooldown = 20;
            scp343.Player.Tag = " scp343-knuckles";
            badges.Add(scp343);
        }

        internal static bool Remove(Player player) => Remove(player.Id);

        internal static bool Remove(int PlayerId)
        {
            Get(PlayerId).Player.Tag = "";
            return badges.RemoveWhere(b => b.Id == PlayerId) > 0;
        }

        internal static void Clear() => badges.Clear();
        /// <summary>
        /// This returns if <see cref="Player"/> is scp343 or isn`t
        /// </summary>
        public static bool Contains(Player player) => Contains(player.Id);
        /// <summary>
        /// This returns if someone or all <see cref="Player"/> is scp343 or isn`t
        /// </summary>
        public static bool Contains(bool allcontains = false, params Player[] players) => allcontains ? players.All(Contains) : players.Any(Contains);

        /// <summary>
        /// This returns if <see cref="Player.Id"/> is scp343 or isn`t
        /// </summary>
        public static bool Contains(int PlayerId) => badges.Any(b => b.Id == PlayerId);
        /// <summary>
        /// Count of <see cref="Scp343"/>
        /// </summary>
        public static int Count() => Count(b => b.IsSCP343);

        /// <summary>
        /// Count of <see cref="Scp343"/>
        /// </summary>
        public static int Count(Func<Badge, bool> predicate) => badges.Count(predicate);

        /// <summary>
        /// Get Badge by <see cref="Player"/>
        /// </summary>
        /// <returns><seealso cref="Badge"/></returns>
        public static Badge Get(Player player) => Contains(player) ? Get(b => b.Id == player.Id).First() : null;

        /// <summary>
        /// Get Badge by <see cref="Player.Id"/> 
        /// </summary>
        /// <returns><seealso cref="Badge"/></returns>
        public static Badge Get(int PlayerId) => Contains(PlayerId) ? Get(b => b.Id == PlayerId).First() : null;

        /// <summary>
        /// Get Badge by <see cref="GameObject"/>
        /// </summary>
        /// <returns><seealso cref="Badge"/></returns>
        public static Badge Get(GameObject GameObject) => ReferenceHub.TryGetHub(GameObject, out ReferenceHub _) ? Get(b => b.GameObject == GameObject).First() : null;

        /// <summary>
        /// Get Badge by <see cref="ReferenceHub"/>
        /// </summary>
        /// <returns><seealso cref="Badge"/></returns>
        public static Badge Get(ReferenceHub ReferenceHub) => ReferenceHub.GetAllHubs().ContainsValue(ReferenceHub) ? Get(b => b.ReferenceHub == ReferenceHub).First() : null;

        /// <summary>
        /// Get Badge by predicate
        /// </summary>
        /// <returns><seealso cref="Badge"/></returns>
        public static IEnumerable<Badge> Get(Func<Badge, bool> predicate) => badges.Where(predicate);
    }
}
