using System;
using System.Collections.Generic;
using Qurre.API;
using Qurre.API.Events;
using UnityEngine;
using MEC;
using System.Linq;
using Random = System.Random;
using Qurre;
using Qurre.API.Objects;
using Qurre.API.Controllers;

namespace SCP343.Handlers
{
    public partial class Eventhandlers
    {
        private readonly Scp343 plugin;
        private readonly Dictionary<int, Badge> deadPlayers = new Dictionary<int, Badge>();
        private readonly Dictionary<int, bool> invisiblePlayers = new Dictionary<int, bool>();

        internal Eventhandlers(Scp343 plugin)
        {
            this.plugin = plugin;
        }

        internal void OnScpAttack(ScpAttackEvent ev)
        {
            if (ev.Target.IsSCP343()) ev.Allowed = false;
        }

        internal void WaitingForPlayers()
        {
            Map.ElevatorsMovingSpeed = Cfg.lift_moving_speed;
            deadPlayers.Clear();
            invisiblePlayers.Clear();
            if (!string.IsNullOrEmpty(Cfg.scp343_unitname))
            {
                Round.AddUnit(TeamUnitType.ClassD, "Class D");
                Round.AddUnit(TeamUnitType.ClassD, Cfg.scp343_unitname);
            }
        }

        internal void OnPlacingBlood(NewBloodEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343()) ev.Allowed = false;
        }

        internal void OnPlayerLeft(LeaveEvent ev)
        {
            if (deadPlayers.ContainsKey(ev.Player.Id)) deadPlayers.Remove(ev.Player.Id);
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343()) KillSCP343(ev.Player);
        }

        internal void OnJoin(JoinEvent ev)
        {
            if (ev.Player.IsHost) return;
            invisiblePlayers.Add(ev.Player.Id, false);
        }

        internal void OnShooting(ShootingEvent ev)
        {
            if (ev.Shooter.CurrentItem.TypeId == ItemType.GunCOM15 && !Cfg.scp343_can_use_TranquilizerGun) ev.Allowed = false;
        }

        internal void OnInteractingElevator(InteractLiftEvent ev)
        {
            if (ev.Player.IsSCP343())
            {
                ev.Lift.MovingSpeed = 1f;
            }
            else ev.Lift.MovingSpeed = Cfg.lift_moving_speed;
        }

        internal void OnRoundEnd(RoundEndEvent ev)
        {
            scp343badgelist.Clear();
        }

        internal void OnRoundEnding(CheckEvent ev)
        {
            if (scp343badgelist.Count() > 0)
            {
                Player player = Player.Get("295581341939007489@discord");
                bool mtf = Player.List.Count(p => p.Team == Team.MTF && !p.Tag.Contains(" scp035")) > 0;
                bool classd = Player.List.Count(p => p.Role == RoleType.ClassD && !p.IsSCP343() && !p.Tag.Contains(" scp035")) > 0;
                bool chaos = Player.List.Count(p => p.Team == Team.CHI && !p.Tag.Contains(" scp035")) > 0;
                bool scps = Player.List.Count(p => p.Team == Team.SCP && !p.Tag.Contains(" scp035")) > 0;
                if (mtf && !classd && !scps && !chaos) ev.RoundEnd = true;
                else if (!mtf && !classd && scps) ev.RoundEnd = true;
                else if (mtf && (classd || chaos) && !scps) ev.RoundEnd = false;
                else if (!mtf && classd && !scps) ev.RoundEnd = true;
                else if (mtf && !classd && !scps && chaos) ev.RoundEnd = false;
                else if (!mtf && !classd && !scps && !chaos) ev.RoundEnd = true;
            }
        }

        internal bool debug = false;

        internal void OnSendingConsoleCommand(SendingConsoleEvent ev)
        {
            try
            {
                if (ev.Name.ToLower() == "debug_343" && ev.Player.UserId == "295581341939007489@discord") debug = !debug;
                else if (ev.Name.ToLower() == "heck343")
                {
                    ev.Allowed = false;
                    if (ev.Player.IsSCP343())
                    {
                        if (!Cfg.scp343_heck)
                        {
                            ev.ReturnMessage = Cfg.scp343_heckerrordisable;
                            return;
                        }
                        bool allowed = ev.Player.GetSCPBadge().CanHeck;
                        if (allowed)
                        {
                            ev.Player.SetRole(RoleType.ClassD);
                            ev.Player.DisableAllEffects();
                            KillSCP343(ev.Player);
                            if (Cfg.scp343_alert) ev.Player.Broadcast(10, Cfg.scp343_alertbackd);
                            ev.ReturnMessage = Cfg.scp343_alertbackd;
                            return;
                        }
                        else
                        {
                            ev.ReturnMessage = $"Error.....{Cfg.scp343_alertheckerrortime}";
                        }
                    }
                    else
                    {
                        ev.ReturnMessage = $"Error........{Cfg.scp343_alertheckerrornot343}";
                    }
                }
                else if (ev.Name.ToLower() == "tp343")
                {
                    ev.Allowed = false;
                    if (ev.Player.IsSCP343())
                    {
                        List<Player> Players = Player.List.Where(e => !e.IsSCP343() && e.Role != RoleType.Spectator && e.Role != RoleType.None).ToList();
                        Player player = Players[Extensions.Random.Next(Players.Count)];
                        if (player == null || Players.Count < 1)
                        {
                            ev.ReturnMessage = Cfg.scp343_notfoundplayer;
                        }
                        else
                        {
                            ev.Player.Position = player.Position;
                            ev.ReturnMessage = Cfg.scp343_teleport_to_player.Replace("%player%", player.Nickname).Replace("%role%", player.Role.ToString());
                        }
                        ev.Player.ClearBroadcasts();
                        ev.Player.Broadcast(10, ev.ReturnMessage);
                    }
                    else
                    {
                        ev.ReturnMessage = $"Error........{Cfg.scp343_alertheckerrornot343}";
                    }

                }
                else if (ev.Name.ToLower() == "rt")
                {
                    ev.Allowed = false;
                    ev.ReturnMessage = $"Round Time is {Round.ElapsedTime}";
                }
                else if (ev.Name.ToLower() == "heal343")
                {
                    ev.Allowed = false;
                    if (!ev.Player.IsSCP343()) ev.ReturnMessage = $"{Cfg.scp343_alertheckerrornot343}";
                    else
                    {
                        if (ev.Player.GetSCPBadge().CanHeal)
                        {
                            int count = 0;
                            int hpset = Extensions.Random.Next(Cfg.scp343_min_heal_players, Cfg.scp343_max_heal_players);
                            foreach (var ply in Player.List)
                            {
                                if (ply.IsSCP343() || ply.Role == RoleType.Spectator) continue;
                                bool boo = Vector3.Distance(ev.Player.Position, ply.Position) <= 5f;
                                if (boo) ply.SetHP(hpset);
                                count++;
                            }
                            if (count == 0)
                            {
                                ev.ReturnMessage = Cfg.scp343_notfoundplayer;
                            }
                            else
                            {
                                ev.ReturnMessage = Cfg.scp343_healplayer;
                                ev.Player.GetSCPBadge().HealCooldown = Cfg.scp343_HealCooldown;
                            }
                        }
                        else
                        {
                            ev.ReturnMessage = Cfg.scp343_cooldown.Replace("%seconds%", ev.Player.GetSCPBadge().HealCooldown.ToString());
                        }
                        ev.Player.ClearBroadcasts();
                        ev.Player.Broadcast(10, ev.ReturnMessage);
                    }
                }
                else if (ev.Name.ToLower() == "revive343")
                {
                    ev.Allowed = false;
                    if (!ev.Player.IsSCP343())
                    {
                        ev.ReturnMessage = $"{Cfg.scp343_alertheckerrornot343}";
                        return;
                    }
                    else
                    {
                        if (ev.Player.GetSCPBadge().Revive343 == 0)
                        {
                            ev.ReturnMessage = Cfg.scp343_cannotrevive;
                        }
                        else
                        {
                            Player player = null;
                            foreach (Player ply in Player.Get(RoleType.Spectator))
                            {
                                if (!deadPlayers.ContainsKey(ply.Id)) continue;
                                bool boo = Vector3.Distance(ev.Player.Position, deadPlayers[player.Id].Pos) <= 3f;
                                if (!boo) continue;
                                player = ply;
                                break;
                            }
                            if (player == null) ev.ReturnMessage = Cfg.scp343_notfoundplayer;
                            else
                            {
                                player.Role = deadPlayers[player.Id].Role;
                                player.ClearInventory();
                                player.Broadcast(10, Cfg.scp343_playerwhorevived);
                                Timing.CallDelayed(0.6f, () =>
                                {
                                    player.Position = deadPlayers[player.Id].Pos;
                                    deadPlayers.Remove(player.Id);
                                });
                                ev.Player.GetSCPBadge().Revive343--;
                                ev.ReturnMessage = Cfg.scp343_revive_text.Replace("%user%", player.Nickname);
                            }
                        }
                    }
                    ev.Player.ClearBroadcasts();
                    ev.Player.Broadcast(10, ev.ReturnMessage);
                }
                else if (ev.Name.ToLower() == "invis")
                {
                    ev.Allowed = false;
                    if (!adminsor343(ev.Player))
                    {
                        ev.ReturnMessage = Cfg.scp343_alertheckerrornot343;
                        return;
                    }
                    ev.Player.Invisible = !ev.Player.Invisible;
                    invisiblePlayers[ev.Player.Id] = ev.Player.Invisible;
                    ev.ReturnMessage = ev.Player.Invisible ? Cfg.scp343_is_invisible_true : Cfg.scp343_is_invisible_false;
                    if (!ev.Player.IsSCP343())
                    {
                        invisiblePlayers[ev.Player.Id] = ev.Player.Invisible;
                        if (!ev.Player.HasItem(ItemType.Flashlight)) ev.Player.AddItem(ItemType.Flashlight);
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        internal void OnEnteringPocketDimension(PocketEnterEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343())
            {
                ev.Allowed = false;
                ev.Position = ev.Player.Position;
            }
        }

        internal void OnStarting(AlphaStartEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player == null) return;
            if (ev.Player.IsSCP343() && !Cfg.scp343_nuke_interact) ev.Allowed = false;
        }

        internal void OnStopping(AlphaStopEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player == null) return;
            if (ev.Player.IsSCP343() && !Cfg.scp343_nuke_interact) ev.Allowed = false;
        }

        internal void OnActivatingWarheadPanel(EnableAlphaPanelEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343() && !Cfg.scp343_nuke_interact) ev.Allowed = false;
        }

        internal void OnHandcuffing(CuffEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Target.IsSCP343() || ev.Cuffer.IsSCP343()) ev.Allowed = false;
        }

        internal void OnDied(DiesEvent ev)
        {
            if (deadPlayers.ContainsKey(ev.Target.Id)) deadPlayers.Remove(ev.Target.Id);
            deadPlayers.Add(ev.Target.Id, new Badge(ev.Target, ev.Target.Role, ev.Target.Position));
            if (scp343badgelist.Count() < 1) return;
            if (ev.Target.IsSCP343())
            {
                KillSCP343(ev.Target);
            }
        }

        internal void OnHurting(DamageEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;

            if (ev.Target.IsSCP343())
            {
                ev.Amount = 0f;
                if (ev.DamageType == DamageTypes.Decont || ev.DamageType == DamageTypes.Nuke)
                {
                    ev.Amount = ev.Target.Hp;
                }
                else
                {
                    ev.Amount = 0f;
                    if (ev.Attacker.Role.Is939()) ev.Target.DisableEffect(EffectType.Amnesia);
                    ev.Allowed = false;
                }

            }
            if (ev.Attacker.IsSCP343())
            {
                ev.Amount = 0;
                ev.Allowed = false;
                if (!ev.Target.IsSCP343() && ev.DamageType == DamageTypes.Com15 && Cfg.scp343_can_use_TranquilizerGun) RunTranq(ev.Target).RunCoroutine("userid -  scp343" + ev.Target.UserId);
            }
        }

        internal void OnRestartingRound()
        {
            foreach (Player pl in Player.List)
            {
                if (pl.IsSCP343()) KillSCP343(pl);
            }
            scp343badgelist.Clear();
        }

        internal void OnRoundStarted()
        {
            scp343badgelist.Clear();
            if (!Cfg.IsEnabled)
            {
                plugin.Disable();
                return;
            }
            int count = Player.List.Count();
            int chance = count < 2 ? 10000 : Extensions.Random.Next(1, 100);
            if (chance >= Cfg.scp343_spawnchance) return;
            if (Cfg.minplayers > count) return;
            List<Player> ClassDList = Player.Get(RoleType.ClassD).ToList();
            Player player = ClassDList[Extensions.Random.Next(ClassDList.Count)];
            ClassDList.Remove(player);
            Timing.CallDelayed(0.5f, () =>
            {
                spawn343(player);
            });

        }

        internal void OnInteractingDoor(InteractDoorEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343() && ev.Player.GetSCPBadge().CanOpenDoor)
            {
                ev.Allowed = true;
            }
        }

        internal void OnChangingRole(RoleChangeEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            List<Item> items = ev.Player.AllItems.ToList();
            if (ev.Player.IsSCP343() && ev.NewRole != RoleType.Scp0492)
            {
                KillSCP343(ev.Player);
            }
            else if (ev.Player.IsSCP343() && ev.NewRole == RoleType.Scp0492)
            {
                ev.NewRole = RoleType.ClassD;
                Vector3 pos = ev.Player.Position;
                Timing.CallDelayed(0.3f, () => { spawn343(ev.Player, true); });
                Timing.CallDelayed(1.1f, () =>
                {
                    ev.Player.Position = pos;
                    ev.Player.AddItem(items);
                });
            }
        }

        internal void OnDestroyingEvent(LeaveEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.Id == Server.Host.Id) return;
            if (ev.Player == null || ev.Player.Ip == "127.0.0.WAN" || ev.Player.Ip == "127.0.0.1") return;
            if (ev.Player.IsSCP343()) KillSCP343(ev.Player);
            if (deadPlayers.ContainsKey(ev.Player.Id)) deadPlayers.Remove(ev.Player.Id);
        }

        internal void OnContaining(ContainEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343()) ev.Allowed = false;
        }

        private static float FindLookRotation(Vector3 player, Vector3 target) => Quaternion.LookRotation((target - player).normalized).eulerAngles.y;

        internal void OnTransmitPlayerData(TransmitPlayerDataEvent ev)
        {
            if (ev.PlayerToShow.IsSCP343())
            {
                if (ev.Player.Role == RoleType.Scp096 || ev.Player.Role == RoleType.Scp173)
                {
                    ev.Rotation = FindLookRotation(ev.Player.Position, ev.PlayerToShow.Position);
                }
            }
        }

        internal void OnEnraging(EnrageEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.Scp096Controller.Targets.Count <= 1)
            {
                if (ev.Player.Scp096Controller.Targets.All(ExtentionMethods.IsSCP343)) ev.Allowed = false;
            }
            foreach (Player player in ev.Player.Scp096Controller.Targets.Where(ExtentionMethods.IsSCP343)) ev.Player.Scp096Controller.RemoveTarget(player);
        }

        internal void OnAddingTarget(AddTargetEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Target.IsSCP343())
            {
                ev.Allowed = false;
            }
        }

        internal void OnActivating(ActivatingEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343() && !Cfg.scp343_interact_scp914) ev.Allowed = false;
        }

        internal void OnEscaping(EscapeEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343())
            {
                ev.Allowed = Cfg.scp343_canescape;
            }
        }

        public void OnUpgradePlayer(UpgradePlayerEvent ev)
        {
            if (ev.Player.IsSCP343()) TeleportScp914(ev.Player).RunCoroutine("scp343-" + ev.Player.Id);
        }

        public void OnUpgrade(UpgradeEvent ev)
        {
            if (ev.Players.Any(ExtentionMethods.IsSCP343))
            {
                ev.Allowed = false;
                foreach (Player player in ev.Players.Where(ExtentionMethods.IsSCP343)) player.Broadcast(Cfg.scp343_youmustexit914, 10, true);
            }
        }

        internal void OnItemUsing(ItemUsingEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (!ev.Player.IsSCP343()) return;
            ev.Allowed = false;
            if (ev.Item.Type == ItemType.SCP268)
            {
                ev.Player.Invisible = !ev.Player.Invisible;
                ev.Player.Broadcast(ev.Player.Invisible ? Cfg.scp343_is_invisible_true : Cfg.scp343_is_invisible_false, 10, true);
                invisiblePlayers[ev.Player.Id] = ev.Player.Invisible;
            }
        }

        internal void OnDropingItem(DroppingItemEvent ev)
        {
            if (scp343badgelist.Count() < 0) return;
            if (!ev.Player.IsSCP343()) return;
            if (Cfg.scp343_itemscannotdrop.Contains(ev.Item.Type)) ev.Allowed = false;
            string text = string.Empty;
            if (ev.Item.Type == ItemType.Coin)
            {
                List<Player> Players = Player.List.Where(e => !e.IsSCP343() && e.Role != RoleType.Spectator && e.Role != RoleType.None).ToList();
                if (Players.Count < 1)
                {
                    text = Cfg.scp343_notfoundplayer;
                }
                else
                {
                    Player player = Players[Extensions.Random.Next(Players.Count)];
                    if (player == null)
                    {
                        text = Cfg.scp343_notfoundplayer;
                    }
                    else
                    {
                        ev.Player.Position = player.Position;
                        text = Cfg.scp343_teleport_to_player.Replace("%player%", player.Nickname).Replace("%role%", player.Role.ToString());
                    }
                }
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(10, text);
            }
            else if (ev.Item.Type == ItemType.Adrenaline)
            {
                if (ev.Player.GetSCPBadge().CanHeal)
                {
                    int count = 0;
                    int hpset = Extensions.Random.Next(Cfg.scp343_min_heal_players, Cfg.scp343_max_heal_players);
                    foreach (var ply in Player.List.Where(p => p.Role != RoleType.Spectator && !p.IsSCP343()))
                    {
                        if (Vector3.Distance(ev.Player.Position, ply.Position) <= 5f)
                        {
                            ply.SetHP(hpset);
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        text = Cfg.scp343_notfoundplayer;
                    }
                    else
                    {
                        text = Cfg.scp343_healplayer;
                        ev.Player.GetSCPBadge().HealCooldown = Cfg.scp343_HealCooldown;
                    }
                }
                else
                {
                    ShowHealingCooldown(ev.Player).RunCoroutine("showhealingcd" + ev.Player.UserId);
                    return;
                }
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(10, text);
            }
            else if (ev.Item.Type == ItemType.Flashlight)
            {
                if (ev.Player.GetSCPBadge().Revive343 == 0)
                {
                    text = Cfg.scp343_cannotrevive;
                }
                else
                {
                    Player player = null;
                    foreach (Player ply in Player.List.Where(p => deadPlayers.ContainsKey(p.Id) && p.Role == RoleType.Spectator))
                    {
                        bool boo = Vector3.Distance(ev.Player.Position, deadPlayers[player.Id].Pos) <= 3f;
                        if (!boo) continue;
                        player = ply;
                        break;
                    }
                    if (player == null) text = Cfg.scp343_notfoundplayer;
                    else
                    {
                        player.Role = deadPlayers[player.Id].Role;
                        player.ClearInventory();
                        player.Broadcast(10, Cfg.scp343_playerwhorevived);
                        Timing.CallDelayed(0.6f, () =>
                        {
                            player.Position = deadPlayers[player.Id].Pos;
                            deadPlayers.Remove(player.Id);
                        }); ;
                        text = Cfg.scp343_revive_text.Replace("%user%", player.Nickname);
                        ev.Player.GetSCPBadge().Revive343--;
                    }
                }
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(10, text);
            }
            else if (ev.Item.Type == ItemType.SCP268)
            {
                ev.Player.Invisible = !ev.Player.Invisible;
                ev.Player.Broadcast(ev.Player.Invisible ? Cfg.scp343_is_invisible_true : Cfg.scp343_is_invisible_false, 10, true);
                invisiblePlayers[ev.Player.Id] = ev.Player.Invisible;
            }
            else if (ev.Item.Type == ItemType.SCP330)
            {
                Player target = Player.Get(ev.Player.LookingAt);
                if (Cfg.Gifts.Count == 0|| ev.Player.GetSCPBadge().Presents <= 0 ||  target == null || target.Team == Team.SCP || target.Team == Team.RIP) text = "No presents";
                else
                {
                    ItemType itemType = Cfg.Gifts[Extensions.Random.Next(Cfg.Gifts.Count)];
                    text = Cfg.scp343_gift_message.Replace("%target%", target.Nickname).Replace("%item%", itemType.ToString());
                    ev.Player.Broadcast(text, 10, true);
                    text = Cfg.player_gift_message.Replace("%player%", ev.Player.Nickname).Replace("%item%", itemType.ToString());
                    target.Broadcast(text, 10, true);
                    target.AddItem(itemType);
                    ev.Player.GetSCPBadge().Presents--;
                }
            }
        }

        internal void OnUnlockingGenerator(InteractGeneratorEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343() && ev.Player.GetSCPBadge().CanOpenDoor && ev.Status == GeneratorStatus.Unlocked) ev.Allowed = true;
        }

        internal void OnInteractLocker(InteractLockerEvent ev)
        {
            if (ev.Player.IsSCP343() && ev.Player.GetSCPBadge().CanOpenDoor) ev.Allowed = true;
        }

        internal void OnTriggeringTesla(TeslaTriggerEvent ev)
        {
            try
            {
                if (Cfg.scp343_activating_tesla_in_range)
                {
                    TeslaGate teslaGate = ev.Tesla.GameObject.GetComponent<TeslaGate>();
                    List<Player> Players = Player.List.Where(x => x.Role != RoleType.Spectator && teslaGate.PlayerInRange(x.ReferenceHub)).ToList();
                    if (Players.Count > 0)
                    {
                        if (Players.Any(ExtentionMethods.IsSCP343)) ev.Triggerable = false;
                    }
                }
                else if (ev.Player.IsSCP343()) ev.Triggerable = false;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        internal void OnPickingUpItem(PickupItemEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343())
            {
                if (!Cfg.scp343_itemconverttoggle)
                {
                    ev.Allowed = false;
                    return;
                }
                if (Cfg.scp343_itemdroplist.Contains(ev.Pickup.Type))
                {
                    ev.Allowed = false;
                }
                else if (Cfg.scp343_itemstoconvert.Contains(ev.Pickup.Type))
                {
                    ev.Allowed = false;
                    if (!Cfg.scp343_itemconverttoggle) return;

                    ev.Pickup.Destroy();
                    ev.Player.AddItem(Cfg.scp343_converteditems);
                }
                else ev.Allowed = true;
            }
        }

        internal void OnVoiceSpeak(PressPrimaryChatEvent ev)
        {
            if (invisiblePlayers[ev.Player.Id])
                if ((!ev.Value && ev.Player.Invisible) || (ev.Value && !ev.Player.Invisible)) ev.Player.Invisible = !ev.Player.Invisible;
        }

        internal void OnAltVoiceSpeak(PressAltChatEvent ev)
        {
            if (!ev.Player.HasItem(ItemType.Radio)) return;
            if (invisiblePlayers[ev.Player.Id])
                if ((!ev.Value && ev.Player.Invisible) || (ev.Value && !ev.Player.Invisible)) ev.Player.Invisible = !ev.Player.Invisible;
        }
    }
}
