using System;
using System.Collections.Generic;
using Qurre.API;
using Qurre.API.Events;
using UnityEngine;
//using static SCP343.scp343;
using MEC;
using System.Linq;
using Random = System.Random;
using Interactables.Interobjects.DoorUtils;
using Respawning.NamingRules;
using Respawning;
using Qurre;
using System.Text.RegularExpressions;
using Qurre.API.Controllers;

namespace SCP343.Handlers
{
    public partial class Players
    {

        internal bool adminsor343(Player player)
        {
            bool flag = false;
            if (player.IsSCP343()) flag = true;
            if (player.Sender.CheckPermission(PlayerPermissions.AdminChat)) flag = true;
            return flag;
        }

        internal IEnumerator<float> WhenOpenDoor(Player player)
        {
            int _time = scp343.cfg.scp343_opendoortime;
            yield return Timing.WaitForSeconds(1f);
            for(; ; )
            {
                if (!player.IsSCP343()) break;
                if (_time<=0)
                {
                    player.GetSCPBadge().canopendoor = true;
                    Log.Info(player.GetSCPBadge().canopendoor);
                    break;
                }
                player.ShowHint(scp343.cfg.scp343_text_show_timer_when_can_open_door.Replace("{343_time_open_door}", _time.ToString()));
                _time--;
                yield return Timing.WaitForSeconds(1f);
            }
        }

        internal Badge spawn343(Player player, bool scp0492 = false, Vector3 position = default)
        {
            player.ClearInventory();
            Timing.CallDelayed(1f, () =>
            {
                player.Ammo[0] = 300;
                player.Ammo[1] = 300;
                player.Ammo[2] = 300;
            });
            if (scp0492)
            {
                KillSCP343(player);
            }
            if (position != default)
            {
                Timing.CallDelayed(0.3f, () =>
                {
                    player.Position = position;
                });
            }
            if (player.IsSCP343()) return player.GetSCPBadge();
            Badge badge = new Badge(player,true);
            Log.Info(badge.Player.Nickname + " | " + badge.IsSCP343);
            // player.ReferenceHub.characterClassManager.CurRole.team = Team.SCP;
            Timing.CallDelayed(1f, () =>
            {
                if (player.RoleName != "") player.RoleName = "SCP-343 | " + player.RoleName;
                else player.RoleName = "SCP-343";
                player.RoleColor = "red";
            });
            if (scp343.cfg.scp343_invisible_for_173) foreach (Player pl in Player.List)
            {
                if (!pl.Scp173Controller.IgnoredPlayers.Contains(player)) pl.Scp173Controller.IgnoredPlayers.Add(player);
            }
            if (scp343.cfg.scp343_alert && !scp0492)
            {
                player.ClearBroadcasts();
                player.Broadcast(15, scp343.cfg.scp343_alerttext.Replace("\\n","\n"));
            }
            if (scp343.cfg.scp343_console && !scp0492) player.SendConsoleMessage("\n----------------------------------------------------------- \n" + scp343.cfg.scp343_consoletext.Replace("343DOORTIME", scp343.cfg.scp343_opendoortime.ToString()).Replace("343HECKTIME", scp343.cfg.scp343_hecktime.ToString()).Replace("\\n", "\n") + "\n-----------------------------------------------------------", "green");

            Timing.CallDelayed(0.5f, () =>
            {
                player.EnableEffect<CustomPlayerEffects.Scp207>(10000000000);
                player.EnableEffect<CustomPlayerEffects.Scp207>(10000000000, true);
                player.ClearInventory();
                if (!scp0492)
                {
                    Log.Debug(scp343.cfg.scp343_itemsatspawn.Count);
                    foreach (int item in scp343.cfg.scp343_itemsatspawn) player.AddItem((ItemType)item);
                }
                if (scp343.cfg.scp343_heck)
                {
                    player.GetSCPBadge().canheck = true;
                }
                player.Health = 100f;
            });
            if (scp343.cfg.scp343_canopenanydoor)
            {
                Timing.RunCoroutine(WhenOpenDoor(player), "player_scp343_" + player.Id);
            }
            if (scp343.cfg.scp343_heck) Timing.CallDelayed(scp343.cfg.scp343_hecktime, () =>
            {
                if (!player.IsSCP343()) return;
                player.GetSCPBadge().canheck = false;
            });
            if(!string.IsNullOrEmpty(scp343.cfg.scp343_unitname))
            {
                player.UnitName = scp343.cfg.scp343_unitname;
            }
            return badge;
        }

        internal void KillSCP343(Player player)
        {
            if (!player.IsSCP343()) return;
            if(scp343.cfg.scp343_invisible_for_173) foreach(Player pl in Player.List)
            {
                if (pl.Scp173Controller.IgnoredPlayers.Contains(player)) pl.Scp173Controller.IgnoredPlayers.Remove(player);
            }
            player.UnitName = "";
            //if (Patches.GhostMode.TurnedPlayers.Contains(player)) Patches.GhostMode.TurnedPlayers.Remove(player);
            foreach (Player pl in Player.List) if (pl.Scp173Controller.IgnoredPlayers.Contains(player)) pl.Scp173Controller.IgnoredPlayers.Remove(player);
            player.RoleColor = player.GetSCPBadge().RoleColor;
            player.RoleName = player.GetSCPBadge().RoleName;
            scp343badgelist.Remove(player);
        }
    }
}
