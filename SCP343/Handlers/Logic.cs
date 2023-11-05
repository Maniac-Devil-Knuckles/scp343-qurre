using System;
using System.Collections.Generic;
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

        private static List<Pickup> List => (List<Pickup>)Scp035.GetTypes().First(t=> t.IsClass && t.Name == "Plugin").GetField("items", BindingFlags.Static | BindingFlags.Public).GetValue(null);

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
                    new Qurre.API.Controllers.Broadcast(player, Config.Translation.End_Cooldown, 5);
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
                if (cooldown > 0) new Qurre.API.Controllers.Broadcast(player, Config.Translation.CoolDown.Replace("%seconds%", cooldown.ToString()), 1);
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
            player.Client.ShakeScreen();
            player.GamePlay.GodMode = true;
            new Qurre.API.Controllers.Broadcast(player, Config.Translation.YouWereTranq, 4);
            Qurre.API.Controllers.Ragdoll ragdoll = new Qurre.API.Controllers.Ragdoll(player.RoleInfomation.Role, player.MovementState.Position, player.MovementState.CameraReference.rotation, new PlayerStatsSystem.CustomReasonDamageHandler("tranquilizer", 0), player);
            Vector3 pos = player.MovementState.Position;
            player.MovementState.Position = new Vector3(1, 1, 1);
            yield return Timing.WaitForSeconds(5f);
            player.GamePlay.GodMode = false;
            player.MovementState.Position = pos;
            player.Client.ShakeScreen();
            ragdoll.Destroy();
        }

        internal static bool adminsor343(Player player) => player.IsSCP343() || player.Sender.CheckPermission(PlayerPermissions.AdminChat);

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
                    Log.Info(player.GetSCPBadge().CanOpenDoor);
                    break;
                }
                player.Client.ShowHint(Config.Translation.Text_Show_Timer_When_Can_Open_Door.Replace("{343_time_open_door}", _time.ToString()));
                _time--;
                yield return Timing.WaitForSeconds(1f);
            }
        }

        internal static Badge spawn343(Player player, bool scp0492 = false, Vector3 position = default)
        {
            string globalbadge = string.Empty;
            if (!string.IsNullOrEmpty(player.GlobalBadge())) globalbadge = " | " + player.GlobalBadge();
            player.Inventory.Clear();
            Timing.CallDelayed(1f, () =>
            {
                player.Inventory.Ammo.Ammo556 = 300;
                player.Inventory.Ammo.Ammo762 = 300;
                player.Inventory.Ammo.Ammo9 = 300;
                player.Inventory.Ammo.Ammo12Gauge = 300;
                player.Inventory.Ammo.Ammo44Cal = 300;
            });
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
            Badge badge = new Badge(player, true);
            Timing.CallDelayed(1f, () =>
            {
                if (player.Administrative.RoleName != "") player.Administrative.RoleName = "SCP-343" + (string.IsNullOrEmpty(globalbadge) ? " | " + player.Administrative.RoleName : globalbadge);
                else player.Administrative.RoleName = "SCP-343" + globalbadge;
                player.Administrative.RoleColor = "red";
            });
            if (Config.Invisible_For_173) Scp173.IgnoredPlayers.Contains(player);
            if (Config.Alert && !scp0492)
            {
                player.Broadcasts.Clear();
                new Qurre.API.Controllers.Broadcast(player,Config.Translation.AlertText,15);
            }
            if (Config.Console && !scp0492) player.Client.SendConsole("\n----------------------------------------------------------- \n" + Config.Translation.ConsoleText.Replace("343DOORTIME", Config.OpenDoorTime.ToString()).Replace("343HECKTIME", Config.HeckTime.ToString()).Replace("\\n", "\n") + "\n-----------------------------------------------------------", "green");

            Timing.CallDelayed(0.5f, () =>
            {
                player.Effects.Enable<CustomPlayerEffects.Scp207>(10000000000);
                player.Effects.Enable<CustomPlayerEffects.Scp207>(10000000000, true);
                player.Inventory.Clear();
                if (!scp0492)
                {
                   foreach(ItemType item in Config.ItemsAtSpawn)  player.Inventory.AddItem(item);
                }
                if (Config.Heck)
                {
                    player.GetSCPBadge().CanHeck = true;
                }
                player.HealthInfomation.Hp = 100f;
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
