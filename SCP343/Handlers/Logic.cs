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
                    player.Broadcast(Scp343.CustomConfig.Translation.end_cooldown, 5, true);
                }

                if (player.GetSCPBadge().ShootCooldown > 0) player.GetSCPBadge().ShootCooldown--;
            }
        }

        private static IEnumerator<float> ShowHealingCooldown(Player player)
        {
            yield return Timing.WaitForSeconds(1f);
            for (int i = 0; i <= 5; i++)
            {
                yield return Timing.WaitForSeconds(1f);
                int cooldown = player.GetSCPBadge().HealCooldown;
                if (cooldown > 0) player.Broadcast(Scp343.CustomConfig.Translation.cooldown.Replace("%seconds%", cooldown.ToString()), 1, true);
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
            player.Broadcast(Scp343.CustomConfig.Translation.youweretranq, 4);
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
            int _time = Scp343.CustomConfig.opendoortime;
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
                player.ShowHint(Scp343.CustomConfig.Translation.text_show_timer_when_can_open_door.Replace("{343_time_open_door}", _time.ToString()));
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
            if (Scp343.CustomConfig.invisible_for_173) 
                foreach (Player pl in Player.List)
                {
                    if (!pl.Scp173Controller.IgnoredPlayers.Contains(player)) pl.Scp173Controller.IgnoredPlayers.Add(player);
                }
            if (Scp343.CustomConfig.alert && !scp0492)
            {
                player.ClearBroadcasts();
                player.Broadcast(15, Scp343.CustomConfig.Translation.alerttext);
            }
            if (Scp343.CustomConfig.console && !scp0492) player.SendConsoleMessage("\n----------------------------------------------------------- \n" + Scp343.CustomConfig.Translation.consoletext.Replace("343DOORTIME", Scp343.CustomConfig.opendoortime.ToString()).Replace("343HECKTIME", Scp343.CustomConfig.hecktime.ToString()).Replace("\\n", "\n") + "\n-----------------------------------------------------------", "green");

            Timing.CallDelayed(0.5f, () =>
            {
                player.EnableEffect<CustomPlayerEffects.Scp207>(10000000000);
                player.EnableEffect<CustomPlayerEffects.Scp207>(10000000000, true);
                player.ClearInventory();
                if (!scp0492)
                {
                    player.AddItem(Scp343.CustomConfig.itemsatspawn);
                }
                if (Scp343.CustomConfig.heck)
                {
                    player.GetSCPBadge().CanHeck = true;
                }
                player.Hp = 100f;
            });
            if (Scp343.CustomConfig.canopenanydoor)
            {
                WhenOpenDoor(player).RunCoroutine("player_" + player.Id);
            }
            if (Scp343.CustomConfig.heck) CanHeck343(player).RunCoroutine("Canheck343das" + player.UserId);
            if (!string.IsNullOrEmpty(Scp343.CustomConfig.Translation.unitname))
            {
                player.UnitName = Scp343.CustomConfig.Translation.unitname;
            }
            HealingCooldown(player).RunCoroutine("healcd" + player.UserId);
            player.UseStamina = false;
            player.Dissonance.EnableListening(TriggerType.Role, Assets._Scripts.Dissonance.RoleType.SCP);
            player.Dissonance.EnableSpeaking(TriggerType.Role, Assets._Scripts.Dissonance.RoleType.SCP);
            player.Dissonance.SCPChat = true;
            return badge;
        }

        internal static IEnumerator<float> CanHeck343(Player player)
        {
            int time = Scp343.CustomConfig.hecktime;
            yield return 0.5f;
            for(; Scp343.CustomConfig.heck; )
            {
                if (!player.IsSCP343()) yield break;
                else if (time >= 1) yield return 1f;
                else
                {
                    player.GetSCPBadge().CanHeck = false;
                    yield break;
                }
                time--;
            }
        }

        internal static void KillSCP343(Player player)
        {
            if (!player.IsSCP343()) return;
            player.UseStamina = true;
            if (Scp343.CustomConfig.invisible_for_173) foreach(Player pl in Player.List)
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
