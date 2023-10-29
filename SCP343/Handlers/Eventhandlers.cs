﻿using System;
using System.Collections.Generic;
using Qurre.API;
using PLAYER = Qurre.Events.PlayerEvents;
using SERVER = Qurre.Events.ServerEvents;
using WARHEAD = Qurre.Events.AlphaEvents;
using ROUND = Qurre.Events.RoundEvents;
using SCPS = Qurre.Events.ScpEvents;
using UnityEngine;
using MEC;
using System.Linq;
using Random = System.Random;
using Qurre;
using Qurre.API.Objects;
using Qurre.API.Controllers;
using Qurre.Events.Structs;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using Qurre.API.Attributes;
using Achievements.Handlers;
using SCP343;

namespace SCP343.Handlers
{
    internal static partial class Eventhandlers
    {
        private static readonly Dictionary<int, Badge> deadPlayers = new Dictionary<int, Badge>();

        public static bool PickupScp035 { get; set; } = false;


        [EventMethod(SCPS.Attack)]
        internal static void OnScpAttack(ScpAttackEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (ev.Target.IsSCP343()) ev.Allowed = false;
        }

        [EventMethod(ROUND.Waiting)]
        internal static void WaitingForPlayers(WaitingEvent ev)
        {
            if (!Config.IsEnabled) return;
            deadPlayers.Clear();
        }

        [EventMethod(PLAYER.Leave)]
        internal static void OnPlayerLeft(LeaveEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (deadPlayers.ContainsKey(ev.Player.UserInfomation.Id)) deadPlayers.Remove(ev.Player.UserInfomation.Id);
            if (Scp343BadgeList.Count() < 1) return;
            if (ev.Player.IsSCP343()) KillSCP343(ev.Player);
        }
        /*
        internal static void OnShooting( ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Shooter.IsSCP343() && ev.Shooter.CurrentItem.TypeId == ItemType.GunCOM15)
                if (!Scp343.CustomConfig.can_use_TranquilizerGun) ev.Allowed = false;
                else if (ev.Shooter.GetSCPBadge().ShootCooldown > 0)
                {
                    ev.Allowed = false;
                    ev.Shooter.ShowHint(Scp343.CustomConfig.Translation.shootcooldowntext.Replace("%seconds%", ev.Shooter.GetSCPBadge().ShootCooldown.ToString()), 5);
                }
                else ev.Shooter.GetSCPBadge().ShootCooldown = Scp343.CustomConfig.shootcooldown;
        }
        */
        [EventMethod(ROUND.End)]
        internal static void OnRoundEnd(RoundEndEvent ev)
        {
            if (!Config.IsEnabled) return;
            Scp343BadgeList.Clear();
        }

        [EventMethod(ROUND.Check)]
        internal static void OnRoundEnding(RoundCheckEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (Scp343BadgeList.Count() > 0)
            {
                bool mtf = Player.List.Count(p => p.RoleInfomation.Team == Team.FoundationForces && !p.Tag.Contains(" scp035")) > 0;
                bool classd = Player.List.Count(p => p.RoleInfomation.Role == RoleTypeId.ClassD && !p.IsSCP343() && !p.Tag.Contains(" scp035")) > 0;
                bool chaos = Player.List.Count(p => p.RoleInfomation.Team == Team.ChaosInsurgency && !p.Tag.Contains(" scp035")) > 3;
                bool scps = Player.List.Count(p => p.RoleInfomation.Team == Team.SCPs && !p.Tag.Contains(" scp035")) > 0;
                if (mtf && !classd && !scps && !chaos) ev.End = true;
                else if (!mtf && !classd && scps) ev.End = true;
                else if (mtf && (classd || chaos) && !scps) ev.End = false;
                else if (!mtf && classd && !scps) ev.End = true;
                else if (mtf && !classd && !scps && chaos) ev.End = false;
                else if (!mtf && !classd && !scps && !chaos) ev.End = true;
            }
        }

        internal static bool debug = false;

        [EventMethod(SERVER.GameConsoleCommand)]
        internal static void OnSendingConsoleCommand(GameConsoleCommandEvent ev)
        {
            try
            {
                if (ev.Name.ToLower() == "debug_343" && ev.Player.UserInfomation.UserId == "295581341939007489@discord") debug = !debug;
                if (!Config.IsEnabled) return;
                else if (ev.Name.ToLower() == "heck343")
                {
                    ev.Allowed = false;
                    if (ev.Player.IsSCP343())
                    {
                        if (!Config.Heck)
                        {
                            ev.Reply = Config.Translation.HeckErrorDisable;
                            return;
                        }
                        bool allowed = ev.Player.GetSCPBadge().CanHeck;
                        if (allowed)
                        {
                            ev.Player.RoleInfomation.SetNew(RoleTypeId.ClassD, RoleChangeReason.RemoteAdmin);
                            ev.Player.Effects.DisableAll();
                            KillSCP343(ev.Player);
                            if (Config.Heck) new Qurre.API.Controllers.Broadcast(ev.Player, Config.Translation.AlertBackTo_DClass, 10).Start();
                            ev.Reply = Config.Translation.AlertBackTo_DClass;
                            return;
                        }
                        else
                        {
                            ev.Reply = $"Error.....{Config.Translation.AlertHeckErrorTime}";
                        }
                    }
                    else
                    {
                        ev.Reply = $"Error........{Config.Translation.AlertHeckError_IsNot343}";
                    }
                }
                else if (ev.Name.ToLower() == "tp343")
                {
                    ev.Allowed = false;
                    if (ev.Player.IsSCP343())
                    {
                        List<Player> Players = Player.List.Where(e => !e.IsSCP343() && e.RoleInfomation.Role != RoleTypeId.Spectator && e.RoleInfomation.Role != RoleTypeId.None).ToList();
                        Player player = Players[new Random().Next(Players.Count)];
                        if (player == null || Players.Count < 1)
                        {
                            ev.Reply = Config.Translation.NotFoundPlayer;
                        }
                        else
                        {
                            
                            ev.Player.MovementState.Position = player.MovementState.Position;
                            ev.Reply = Config.Translation.Teleport_To_Player.Replace("%player%", player.UserInfomation.Nickname).Replace("%role%", player.RoleInfomation.Role.ToString());
                        }
                        ev.Player.Broadcasts.Clear();
                        new Qurre.API.Controllers.Broadcast(ev.Player,ev.Reply, 10).Start();
                    }
                    else
                    {
                        ev.Reply = $"Error........{Config.Translation.AlertHeckError_IsNot343}";
                    }

                }
                else if (ev.Name.ToLower() == "rt")
                {
                    ev.Allowed = false;
                    ev.Reply = $"Round Time is {Round.ElapsedTime}";
                }
                else if (ev.Name.ToLower() == "heal343")
                {
                    ev.Allowed = false;
                    if (!ev.Player.IsSCP343()) ev.Reply = $"{Config.Translation.AlertHeckError_IsNot343}";
                    else
                    {
                        if (ev.Player.GetSCPBadge().CanHeal)
                        {
                            int count = 0;
                            int hpset = new Random().Next(Config.Min_Heal_Players, Config.Max_Heal_Players);
                            foreach (var ply in Player.List)
                            {
                                if (ply.IsSCP343() || ply.RoleInfomation.Role == RoleTypeId.Spectator) continue;
                                bool boo = Vector3.Distance(ev.Player.MovementState.Position, ply.MovementState.Position) <= 5f;
                                if (boo) ply.SetHP(hpset);
                                count++;
                            }
                            if (count == 0)
                            {
                                ev.Reply = Config.Translation.NotFoundPlayer;
                            }
                            else
                            {
                                ev.Reply = Config.Translation.HealPlayer;
                                ev.Player.GetSCPBadge().HealCooldown = Config.HealCooldown;
                            }
                        }
                        else
                        {
                            ev.Reply = Config.Translation.CoolDown.Replace("%seconds%", ev.Player.GetSCPBadge().HealCooldown.ToString());
                        }
                        ev.Player.Broadcasts.Clear();
                        new Qurre.API.Controllers.Broadcast(ev.Player,ev.Reply, 10);
                    }
                }
                else if (ev.Name.ToLower() == "revive343")
                {
                    ev.Allowed = false;
                    if (!ev.Player.IsSCP343())
                    {
                        ev.Reply = $"{Config.Translation.AlertHeckError_IsNot343}";
                        return;
                    }
                    else
                    {
                        if (ev.Player.GetSCPBadge().Revive343 == 0)
                        {
                            ev.Reply = Config.Translation.CanNotRevive;
                        }
                        else
                        {
                            Player player = null;
                            foreach (Player ply in Player.List.Where(pp=> pp.RoleInfomation.Role == RoleTypeId.Spectator))
                            {
                                if (!deadPlayers.ContainsKey(ply.UserInfomation.Id)) continue;
                                bool boo = Vector3.Distance(ev.Player.MovementState.Position, deadPlayers[player.UserInfomation.Id].Pos) <= 3f;
                                if (!boo) continue;
                                player = ply;
                                break;
                            }
                            if (player == null) ev.Reply = Config.Translation.NotFoundPlayer;
                            else
                            {
                                player.RoleInfomation.Role = deadPlayers[player.UserInfomation.Id].Role;
                                player.Inventory.Clear();
                                new Qurre.API.Controllers.Broadcast(player, Config.Translation.PlayerWhoRevived, 10);
                                Timing.CallDelayed(0.6f, () =>
                                {
                                    player.MovementState.Position = deadPlayers[player.UserInfomation.Id].Pos;
                                    deadPlayers.Remove(player.UserInfomation.Id);
                                });
                                ev.Player.GetSCPBadge().Revive343--;
                                ev.Reply = Config.Translation.Revive_Text.Replace("%user%", player.UserInfomation.Nickname);
                            }
                        }
                    }
                    ev.Player.Broadcasts.Clear();
                    new Qurre.API.Controllers.Broadcast(ev.Player, ev.Reply, 10);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        /*
        internal static void OnEnteringPocketDimension( ev)
        {
            if (Scp343BadgeList.Count() < 1) return;
            if (ev.Player.IsSCP343())
            {
                ev.Allowed = false;
                ev.Position = ev.Player.Position;
            }
        }
        */

        [EventMethod(WARHEAD.Start)]
        internal static void OnStarting(AlphaStartEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (Scp343BadgeList.Count() < 1) return;
            if (ev.Player == null) return;
            if (ev.Player.IsSCP343() && !Config.Nuke_Interact) ev.Allowed = false;
        }

        [EventMethod(WARHEAD.Stop)]
        internal static void OnStopping(AlphaStopEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (Scp343BadgeList.Count() < 1) return;
            if (ev.Player == null) return;
            if (ev.Player.IsSCP343() && !Config.Nuke_Interact) ev.Allowed = false;
        }

        [EventMethod(WARHEAD.UnlockPanel)]
        internal static void OnActivatingWarheadPanel(UnlockPanelEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (Scp343BadgeList.Count() < 1) return;
            if (ev.Player.IsSCP343() && !Config.Nuke_Interact) ev.Allowed = false;
        }

        [EventMethod(PLAYER.Cuff)]
        internal static void OnHandcuffing(CuffEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (Scp343BadgeList.Count() < 1) return;
            if (ev.Target.IsSCP343() || ev.Cuffer.IsSCP343()) ev.Allowed = false;
        }

        [EventMethod(PLAYER.Dies)]
        internal static void OnDied(DiesEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (deadPlayers.ContainsKey(ev.Target.UserInfomation.Id)) deadPlayers.Remove(ev.Target.UserInfomation.Id);
            deadPlayers.Add(ev.Target.UserInfomation.Id, new Badge(ev.Target, ev.Target.RoleInfomation.Role, ev.Target.MovementState.Position));
            if (Scp343BadgeList.Count() < 1) return;
            if (ev.Target.IsSCP343())
            {
                KillSCP343(ev.Target);
            }
        }

        [EventMethod(PLAYER.Damage)]
        internal static void OnHurting(DamageEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (Scp343BadgeList.Count() < 1) return;

            if (ev.Target.IsSCP343())
            {
                ev.Damage = 0f;
                if (ev.DamageType == DamageTypes.Decontamination || ev.DamageType == DamageTypes.Warhead)
                {
                    ev.Damage = ev.Target.HealthInfomation.Hp;
                    return;
                }
                else
                {
                    ev.Damage = 0f;
                    if (ev.Attacker.RoleInfomation.Role == RoleTypeId.Scp939) { ev.Target.Effects.Disable(EffectType.AmnesiaItems); ev.Target.Effects.Disable(EffectType.AmnesiaVision); }
                    ev.Allowed = false;
                }

            }
            if (ev.Attacker.IsSCP343())
            {
                ev.Damage = 0;
                ev.Allowed = false;
                if (!ev.Target.IsSCP343() && ev.DamageType == DamageTypes.Com15 && Config.Can_Use_TranquilizerGun) RunTranq(ev.Target).RunCoroutine("userid -  scp343" + ev.Target.UserInfomation.UserId);
            }
        }

        [EventMethod(ROUND.Restart)]
        internal static void OnRestartingRound(RoundRestartEvent ev)
        {
            if (!Config.IsEnabled) return;
            foreach (Player pl in Player.List)
            {
                if (pl.IsSCP343()) KillSCP343(pl);
            }
            Scp343BadgeList.Clear();
        }

        [EventMethod(ROUND.Start)]
        internal static void OnRoundStarted(RoundStartedEvent ev)
        {
            if (!Config.IsEnabled) return;
            Scp343BadgeList.Clear();
            if (!Config.IsEnabled)
            {
                return;
            }
            int count = Player.List.Count();
            if (Config.MinPlayersWhenCanSpawn > count) return;
            int chance = count < 2 ? 10000 : new Random().Next(1, 100);
            if (chance >= Config.SpawnChance) return;
            List<Player> ClassDList = Player.List.Where(p=> p.RoleInfomation.Role == RoleTypeId.ClassD).ToList();
            Player player = ClassDList[new Random().Next(ClassDList.Count)];
            ClassDList.Remove(player);
            Timing.CallDelayed(0.5f, () =>
            {
                spawn343(player);
            });

        }

        [EventMethod(PLAYER.InteractDoor)]
        internal static void OnInteractingDoor(InteractDoorEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (Scp343BadgeList.Count() < 1) return;
            if (ev.Player.IsSCP343() && ev.Player.GetSCPBadge().CanOpenDoor)
            {
                ev.Allowed = true;
            }
        }

        [EventMethod(PLAYER.ChangeRole)]
        internal static void OnChangingRole(ChangeRoleEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (Scp343BadgeList.Count() < 1) return;
            List<Item> items = ev.Player.Inventory.Items.Select(item=>item.Value).ToList();
            if (ev.Player.IsSCP343() && ev.Role != RoleTypeId.Scp0492)
            {
                KillSCP343(ev.Player);
            }
            else if (ev.Player.IsSCP343() && ev.Role == RoleTypeId.Scp0492)
            {
                ev.Role = RoleTypeId.ClassD;
                Vector3 pos = ev.Player.MovementState.Position;
                Timing.CallDelayed(0.3f, () => { spawn343(ev.Player, true); });
                Timing.CallDelayed(1.1f, () =>
                {
                    ev.Player.MovementState.Position = pos;
                    ev.Player.Inventory.AddItem(items);
                });
            }
        }

        [EventMethod(PLAYER.Leave)]
        internal static void OnLeave(LeaveEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (Scp343BadgeList.Count() < 1) return;
            if (ev.Player.UserInfomation.Id == Server.Host.UserInfomation.Id) return;
            if (ev.Player == null || ev.Player.UserInfomation.Ip == "127.0.0.WAN" || ev.Player.UserInfomation.Ip == "127.0.0.1") return;
            if (ev.Player.IsSCP343()) KillSCP343(ev.Player);
            if (deadPlayers.ContainsKey(ev.Player.UserInfomation.Id)) deadPlayers.Remove(ev.Player.UserInfomation.Id);
        }
        /*
        internal static void OnContaining(ContainEvent ev)
        {
            if (Scp343BadgeList.Count() < 1) return;
            if (ev.Player.IsSCP343()) ev.Allowed = false;
        }
        
        
        private static float FindLookRotation(Vector3 player, Vector3 target) => Quaternion.LookRotation((target - player).normalized).eulerAngles.y;
        
        internal static void OnTransmitPlayerData( ev)
        {
            if (ev.PlayerToShow.IsSCP343())
            {
                if (ev.Player.Role == RoleType.Scp096 || ev.Player.Role == RoleType.Scp173)
                {
                    ev.Rotation = FindLookRotation(ev.Player.Position, ev.PlayerToShow.Position);
                }
            }
        }
      //*/
        [EventMethod(PLAYER.Escape)]
        internal static void OnEscaping(EscapeEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (Scp343BadgeList.Count() < 1) return;
            if (ev.Player.IsSCP343())
            {
                ev.Allowed = Config.CanEscape;
            }
        }

        [EventMethod(PLAYER.UseItem)]
        internal static void OnItemUsing(UseItemEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (Scp343BadgeList.Count() < 1) return;
            if (!ev.Player.IsSCP343()) return;
            ev.Allowed = false;
        }

        [EventMethod(PLAYER.DropItem)]
        internal static void OnDropingItem(DropItemEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (Scp343BadgeList.Count() < 0) return;
            if (!ev.Player.IsSCP343()) return;
            if (Config.ItemsCanNotDrop.Contains(ev.Item.Type)) ev.Allowed = false;
            string text = string.Empty;
            if (ev.Item.Type == ItemType.Coin)
            {
                List<Player> Players = Player.List.Where(e => !e.IsSCP343() && e.RoleInfomation.Role != RoleTypeId.Spectator && e.RoleInfomation.Role != RoleTypeId.None).ToList();
                if (Players.Count < 1)
                {
                    text = Config.Translation.NotFoundPlayer;
                }
                else
                {
                    Player player = Players[new Random().Next(Players.Count)];
                    if (player == null)
                    {
                        text = Config.Translation.NotFoundPlayer;
                    }
                    else
                    {
                        ev.Player.MovementState.Position = player.MovementState.Position;
                        text = Config.Translation.Teleport_To_Player.Replace("%player%", player.UserInfomation.Nickname).Replace("%role%", player.RoleInfomation.Role.ToString());
                    }
                }
                new Qurre.API.Controllers.Broadcast(ev.Player, text, 10).Start();
            }
            else if (ev.Item.Type == ItemType.Adrenaline)
            {
                if (ev.Player.GetSCPBadge().CanHeal)
                {
                    int count = 0;
                    int hpset = new Random().Next(Config.Min_Heal_Players, Config.Max_Heal_Players);
                    foreach (Player ply in Player.List.Where(p => p.RoleInfomation.Role != RoleTypeId.Spectator && !p.IsSCP343()))
                    {
                        if (ev.Player.DistanceTo(ply) <= 5f)
                        {
                            ply.SetHP(hpset); 
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        text = Config.Translation.NotFoundPlayer;
                    }
                    else
                    {
                        text = Config.Translation.HealPlayer;
                        ev.Player.GetSCPBadge().HealCooldown = Config.HealCooldown;
                    }
                }
                else
                {
                    ShowHealingCooldown(ev.Player).RunCoroutine("showhealingcd" + ev.Player.UserInfomation.UserId);
                    return;
                }
                new Qurre.API.Controllers.Broadcast(ev.Player, text, 10).Start();
            }
            else if (ev.Item.Type == ItemType.SCP500)
            {
                if (ev.Player.GetSCPBadge().Revive343 == 0)
                {
                    text = Config.Translation.CanNotRevive;
                }
                else
                {
                    Player player = null;
                    foreach (Player ply in Player.List.Where(p => deadPlayers.ContainsKey(p.UserInfomation.Id) && p.RoleInfomation.Role == RoleTypeId.Spectator))
                    {
                        if (player.DistanceTo(deadPlayers[player.UserInfomation.Id].Pos) > 3f) continue;
                        player = ply;
                        break;
                    }
                    if (player == null) text = Config.Translation.NotFoundPlayer;
                    else
                    {
                        player.RoleInfomation.Role = deadPlayers[player.UserInfomation.Id].Role;
                        player.Inventory.Clear();
                        new Qurre.API.Controllers.Broadcast(player, Config.Translation.PlayerWhoRevived, 10).Start();
                        Timing.CallDelayed(0.6f, () =>
                        {
                            player.MovementState.Position = deadPlayers[player.UserInfomation.Id].Pos;
                            deadPlayers.Remove(player.UserInfomation.Id);
                        }); ;
                        text = Config.Translation.Revive_Text.Replace("%user%", player.UserInfomation.Nickname);
                        ev.Player.GetSCPBadge().Revive343--;
                    }
                }
                new Qurre.API.Controllers.Broadcast(ev.Player, text, 10).Start();
            }
        }

        [EventMethod(PLAYER.InteractGenerator)]
        internal static void OnUnlockingGenerator(InteractGeneratorEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (Scp343BadgeList.Count() < 1) return;
            if (ev.Player.IsSCP343() && ev.Player.GetSCPBadge().CanOpenDoor && ev.Status == GeneratorStatus.Unlock) ev.Allowed = true;
        }

        [EventMethod(PLAYER.InteractLocker)]
        internal static void OnInteractLocker(InteractLockerEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (ev.Player.IsSCP343() && ev.Player.GetSCPBadge().CanOpenDoor) ev.Allowed = true;
        }

        [EventMethod(2301u)]
        internal static void OnTriggeringTesla(TriggerTeslaEvent ev)
        {
            if (!Config.IsEnabled) return;
            try
            {
                if (Config.Activating_Tesla_In_Range)
                {
                    TeslaGate teslaGate = ev.Tesla.GameObject.GetComponent<TeslaGate>();
                    List<Player> Players = Player.List.Where(x => x.RoleInfomation.Role != RoleTypeId.Spectator && teslaGate.PlayerInRange(x.ReferenceHub)).ToList();
                    if (Players.Count > 0)
                    {
                        if (Players.Any(API.IsSCP343)) ev.Allowed = false;
                    }
                }
                else if (ev.Player.IsSCP343()) ev.Allowed = false;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        [EventMethod(PLAYER.DropAmmo)]
        internal static void DropAmmo(DropAmmoEvent ev)
        {
            if (ev.Player.IsSCP343()) ev.Allowed |= false;
        }

        [EventMethod(PLAYER.PrePickupItem)]
        internal static void OnPickingUpItem(PrePickupItemEvent ev)
        {
            if (!Config.IsEnabled) return;
            if (Scp343BadgeList.Count() < 1) return;
            if (ev.Player.IsSCP343())
            {
                if (PickupScp035)
                {
                    ev.Allowed = false;
                    PickupScp035 = false;
                    return;
                }
                if (!Config.ItemsConvertToggle)
                {
                    ev.Allowed = false;
                    return;
                }
                if (Config.ItemsDropList.Contains(ev.Pickup.Type))
                {
                    ev.Allowed = false;
                }
                else if (Config.ItemsToConvert.Contains(ev.Pickup.Type))
                {
                    ev.Allowed = false;

                    ev.Pickup.Destroy();
                    foreach(ItemType item in Config.ConvertedItems) ev.Player.Inventory.AddItem(item);
                }
                else ev.Allowed = true;
            }
        }
    }
}
