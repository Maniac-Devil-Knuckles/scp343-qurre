﻿using System.Collections.Generic;
using Qurre.API;
using UnityEngine;
using MEC;
using Qurre.API.Classification.Roles;
using System.Reflection;
using System.Linq;
using Qurre.API.Controllers;

namespace SCP343.Handlers
{
    internal static partial class Eventhandlers
    {
        internal static Assembly Scp035 { get; set; } = null;

        private static void RefreshItemsScp035()
        {
            if (Scp035 == null) return;
            Scp035.GetTypes().First(t => t.IsClass && t.Name == "Logic").GetMethod("RefreshItems", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
        }

        private static List<Pickup> List => (List<Pickup>)Scp035.GetTypes().First(t=> t.IsClass && t.Name == "Plugin").GetField("items", BindingFlags.Static | BindingFlags.Public).GetValue(null) ?? new();

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
                    player.Broadcasts.Add(new Qurre.API.Controllers.Broadcast(player, Config.Translation.End_Cooldown, 5), true);
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
                if (cooldown > 0) player.Broadcasts.Add(new Qurre.API.Controllers.Broadcast(player, Config.Translation.CoolDown.Replace("%seconds%", cooldown.ToString()), 1), true);
            }
        }

        private static IEnumerator<float> TeleportScp914(Player player)
        {
            yield return Timing.WaitForSeconds(1f);
            for (; Qurre.API.Controllers.Scp914.Working;)
            {
                yield return Timing.WaitForSeconds(1f);
                player.MovementState.Position = Qurre.API.Controllers.Scp914.Intake.position;
            }
        }

        internal static IEnumerator<float> RunTranq(Player player)
        {
            player.GamePlay.GodMode = true;
            player.Broadcasts.Add(new Qurre.API.Controllers.Broadcast(player, Config.Translation.YouWereTranq, 4), true);
            Ragdoll ragdoll = new(player.RoleInfomation.Role, player.MovementState.Position + Vector3.up, player.MovementState.CameraReference.rotation, new PlayerStatsSystem.CustomReasonDamageHandler("tranquilizer", 0), player);
            Vector3 pos = player.MovementState.Position;
            yield return Timing.WaitForSeconds(0.1f);
            player.MovementState.Position = new(0,0,0);
            yield return Timing.WaitForSeconds(5f);
            player.MovementState.Position = pos;
            yield return Timing.WaitForSeconds(0.1f);
            player.GamePlay.GodMode = false;
            player.Client.ShakeScreen();
            ragdoll.Destroy();
        }

        internal static IEnumerator<float> WhenOpenDoor(Player player)
        {
            int _time = Config.OpenDoorTime;
            yield return Timing.WaitForSeconds(1f);
            for (; ; )
            {
                if (!player.IsSCP343()) break;
                if (_time <= 0)
                {
                    player.GetSCPBadge().CanOpenDoor = true;
                    break;
                }
                player.Client.ShowHint(Config.Translation.Text_Show_Timer_When_Can_Open_Door.Replace("{343_time_open_door}", _time.ToString()));
                _time--;
                yield return Timing.WaitForSeconds(1f);
            }
        }

        internal static Badge Spawn343(Player player, bool scp0492 = false, Vector3 position = default)
        {
            if (scp0492)
            {
                KillSCP343(player);
            }
            if (position != default)
            {
                Timing.CallDelayed(0.7f, () =>
                {
                    player.MovementState.Position = position;
                });
            }
            if (player.IsSCP343()) return player.GetSCPBadge();
            Badge badge = new(player, true);
            Timing.CallDelayed(1f, () =>
            {
                if (player.Administrative.RoleName != "") player.Administrative.RoleName = "SCP-343" + (!string.IsNullOrEmpty(player.Administrative.RoleName) ? " | " + player.Administrative.RoleName : string.Empty);
                else player.Administrative.RoleName = "SCP-343";
                player.Administrative.RoleColor = "red";
            });
            if (Config.Invisible_For_173) Scp173.IgnoredPlayers.Add(player);
            if (Config.Alert && !scp0492)
            {
                player.Broadcasts.Clear();
                player.Broadcasts.Add(new Qurre.API.Controllers.Broadcast(player, Config.Translation.AlertText, 15), true);
            }
            if (Config.Console && !scp0492) player.Client.SendConsole("\n----------------------------------------------------------- \n" + Config.Translation.ConsoleText.Replace("343DOORTIME", Config.OpenDoorTime.ToString()).Replace("343HECKTIME", Config.HeckTime.ToString()).Replace("\\n", "\n") + "\n-----------------------------------------------------------", "green");

            Timing.CallDelayed(0.5f, () =>
            {
                player.Effects.Enable<CustomPlayerEffects.Scp207>(10000000000);
                player.Effects.Enable<CustomPlayerEffects.Scp207>(10000000000, true);
                if (!scp0492) player.Inventory.Reset(Config.ItemsAtSpawn);
                if (Config.Heck) player.GetSCPBadge().CanHeck = true;
                player.HealthInfomation.Hp = 100f;
            });
            Timing.CallDelayed(1f, () =>
            {
                player.Inventory.Ammo.Ammo556 = 300;
                player.Inventory.Ammo.Ammo762 = 300;
                player.Inventory.Ammo.Ammo9 = 300;
                player.Inventory.Ammo.Ammo12Gauge = 300;
                player.Inventory.Ammo.Ammo44Cal = 300;
            });
            if (Config.CanOpenAnyDoor)
            {
                WhenOpenDoor(player).RunCoroutine("player_" + player.UserInfomation.Id);
            }
            if (Config.Heck) CanHeck343(player).RunCoroutine("Canheck343das" + player.UserInfomation.UserId);
            HealingCooldown(player).RunCoroutine("healcd" + player.UserInfomation.UserId);
            player.HealthInfomation.Stamina = 1234567;
            return badge;
        }

        internal static IEnumerator<float> CanHeck343(Player player)
        {
            int time = Config.HeckTime;
            yield return 0.5f;
            for(; Config.Heck; )
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
            if (Config.Invisible_For_173) Scp173.IgnoredPlayers.Remove(player);
            player.Administrative.RoleColor = player.GetSCPBadge().RoleColor;
            player.Administrative.RoleName = player.GetSCPBadge().RoleName;
            Scp343BadgeList.Remove(player);
            player.Tag = "";
        }
    }
}
