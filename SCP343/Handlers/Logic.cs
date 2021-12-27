using System;
using System.Collections.Generic;
using Qurre.API;
using UnityEngine;
using MEC;
using Qurre;
using System.Linq;
using Assets._Scripts.Dissonance;

namespace SCP343.Handlers
{
    public partial class Eventhandlers
    {
        private static IEnumerator<float> HealingCooldown(Player player)
        {
            yield return Timing.WaitForSeconds(1f);
            bool IsShowed = false;
            for (; player.IsSCP343();)
            {
                yield return Timing.WaitForSeconds(1f);
                if (!player.GetSCPBadge().CanHeal)
                {
                    player.GetSCPBadge().HealCooldown--;
                    IsShowed = false;
                }
                else if (!IsShowed)
                {
                    IsShowed = true;
                    player.Broadcast(Cfg.scp343_end_cooldown, 5, true);
                }
            }
        }

        private static IEnumerator<float> ShowHealingCooldown(Player player)
        {
            yield return Timing.WaitForSeconds(1f);
            for (int i = 0; i <= 5; i++)
            {
                yield return Timing.WaitForSeconds(1f);
                int cooldown = player.GetSCPBadge().HealCooldown;
                if (cooldown > 0) player.Broadcast(Cfg.scp343_cooldown.Replace("%seconds%", cooldown.ToString()), 1, true);
            }
        }

        private static IEnumerator<float> TeleportScp914(Player player)
        {
            yield return Timing.WaitForSeconds(1f);
            for (; Qurre.API.Controllers.Scp914.Working;)
            {
                yield return Timing.WaitForSeconds(1f);
                player.Position = Qurre.API.Controllers.Scp914.Intake.position;
            }
        }

        internal IEnumerator<float> RunTranq(Player player)
        {
            player.Invisible = true;
            player.GodMode = true;
            player.Broadcast(Cfg.scp343_youweretranq, 4);
            Qurre.API.Controllers.Ragdoll ragdoll = Qurre.API.Controllers.Ragdoll.Create(player.Role, player.Position, player.CameraTransform.rotation, new PlayerStatsSystem.CustomReasonDamageHandler("tranquilizer", 0), "SCP-343", player.Id);
            Vector3 pos = player.Position;
            player.Position = new Vector3(1, 1, 1);
            yield return Timing.WaitForSeconds(5f);
            player.Invisible = false;
            player.GodMode = false;
            player.Position = pos;
            ragdoll.Destroy();
        }

        internal static bool adminsor343(Player player) => player.IsSCP343() || player.Sender.CheckPermission(PlayerPermissions.AdminChat);

        internal static IEnumerator<float> WhenOpenDoor(Player player)
        {
            int _time = Cfg.scp343_opendoortime;
            yield return Timing.WaitForSeconds(1f);
            for (; ; )
            {
                if (!player.IsSCP343()) break;
                if (_time <= 0)
                {
                    player.GetSCPBadge().CanOpenDoor = true;
                    Log.Info(player.GetSCPBadge().CanOpenDoor);
                    break;
                }
                player.ShowHint(Cfg.scp343_text_show_timer_when_can_open_door.Replace("{343_time_open_door}", _time.ToString()));
                _time--;
                yield return Timing.WaitForSeconds(1f);
            }
        }

        internal static Badge spawn343(Player player, bool scp0492 = false, Vector3 position = default)
        {
            string globalbadge = string.Empty;
            if (!string.IsNullOrEmpty(player.GlobalBadge)) globalbadge = " | " + player.GlobalBadge;
            player.ClearInventory();
            Timing.CallDelayed(1f, () =>
            {
                player.Ammo556 = 300;
                player.Ammo762 = 300;
                player.Ammo9 = 300;
                player.Ammo12Gauge = 300;
                player.Ammo44Cal = 300;
            });
            if (scp0492)
            {
                KillSCP343(player);
            }
            if (position != default)
            {
                Timing.CallDelayed(0.7f, () =>
                {
                    player.Position = position;
                });
            }
            if (player.IsSCP343()) return player.GetSCPBadge();
            Badge badge = new Badge(player, true);
            Timing.CallDelayed(1f, () =>
            {
                if (player.RoleName != "") player.RoleName = "SCP-343" + (string.IsNullOrEmpty(globalbadge) ? " | " + player.RoleName : globalbadge);
                else player.RoleName = "SCP-343" + globalbadge;
                player.RoleColor = "red";
            });
            if (Cfg.scp343_invisible_for_173) foreach (Player pl in Player.List)
                {
                    if (!pl.Scp173Controller.IgnoredPlayers.Contains(player)) pl.Scp173Controller.IgnoredPlayers.Add(player);
                }
            if (Cfg.scp343_alert && !scp0492)
            {
                player.ClearBroadcasts();
                player.Broadcast(15, Cfg.scp343_alerttext);
            }
            if (Cfg.scp343_console && !scp0492) player.SendConsoleMessage("\n----------------------------------------------------------- \n" + Cfg.scp343_consoletext.Replace("343DOORTIME", Cfg.scp343_opendoortime.ToString()).Replace("343HECKTIME", Cfg.scp343_hecktime.ToString()).Replace("\\n", "\n") + "\n-----------------------------------------------------------", "green");

            Timing.CallDelayed(0.5f, () =>
            {
                player.EnableEffect<CustomPlayerEffects.Scp207>(10000000000);
                player.EnableEffect<CustomPlayerEffects.Scp207>(10000000000, true);
                player.ClearInventory();
                if (!scp0492)
                {
                    player.AddItem(Cfg.scp343_itemsatspawn);
                }
                if (Cfg.scp343_heck)
                {
                    player.GetSCPBadge().CanHeck = true;
                }
                player.Hp = 100f;
            });
            if (Cfg.scp343_canopenanydoor)
            {
                WhenOpenDoor(player).RunCoroutine("player_scp343_" + player.Id);
            }
            if (Cfg.scp343_heck) CanHeck343(player).RunCoroutine("Canheck343das" + player.UserId);
            if (!string.IsNullOrEmpty(Cfg.scp343_unitname))
            {
                player.UnitName = Cfg.scp343_unitname;
            }
            HealingCooldown(player).RunCoroutine("healcd" + player.UserId);
            player.UseStamina = false;
            return badge;
        }

        internal static IEnumerator<float> CanHeck343(Player player)
        {
            float time = Cfg.scp343_hecktime;
            yield return 0.5f;
            for(; ; )
            {
                if (!player.IsSCP343()) yield break;
                else if (time <= 0f) yield return 1f;
                else
                {
                    player.GetSCPBadge().CanHeck = false;
                    yield break;
                }
                time -= 1f;
            }
        }

        internal static void KillSCP343(Player player)
        {
            if (!player.IsSCP343()) return;
            player.UseStamina = true;
            if (Cfg.scp343_invisible_for_173) foreach(Player pl in Player.List)
            {
                if (pl.Scp173Controller.IgnoredPlayers.Contains(player)) pl.Scp173Controller.IgnoredPlayers.Remove(player);
            }
            player.UnitName = "";
            player.RoleColor = player.GetSCPBadge().RoleColor;
            player.RoleName = player.GetSCPBadge().RoleName;
            scp343badgelist.Remove(player);
            player.Tag = "";
        }
    }
}
