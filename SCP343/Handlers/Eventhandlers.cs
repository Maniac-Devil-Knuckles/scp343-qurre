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
        private scp343 plugin;
        private Dictionary<int, Badge> deadPlayers { get; } = new Dictionary<int, Badge>();
        internal Eventhandlers(scp343 plugin)
        {
            this.plugin = plugin;
        }

        internal void WaitingForPlayers()
        {
            Map.ElevatorsMovingSpeed = scp343.cfg.lift_moving_speed;
            if (string.IsNullOrEmpty(scp343.cfg.scp343_unitname))
            {
                Round.AddUnit(TeamUnitType.ClassD, "Class D");
                Round.AddUnit(TeamUnitType.ClassD, scp343.cfg.scp343_unitname);
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
            if (ev.Player?.UserId == null || ev.Player.IsHost || ev.Player.Ip == "127.0.0.WAN" || ev.Player.Ip == "127.0.0.1") return;
            if (ev.Player.IsSCP343()) KillSCP343(ev.Player);
        }

        internal void OnShooting(ShootingEvent ev)
        {
            if (ev.Shooter.CurrentItem.TypeId == ItemType.GunCOM15 && !scp343.cfg.scp343_can_use_TranquilizerGun) ev.Allowed = false;
        }

        internal void OnInteractingElevator(InteractLiftEvent ev)
        {
            if (ev.Player.IsSCP343())
            {
                ev.Lift.MovingSpeed = 1f;
            }
            else ev.Lift.MovingSpeed = scp343.cfg.lift_moving_speed;
        }

        internal void OnRoundEnd(RoundEndEvent ev)
        {
            scp343badgelist.Clear();
        }

        internal void OnRoundEnding(CheckEvent ev)
        {
            if (scp343badgelist.Count() > 0)
            {
                List<Player> mtf = Player.List.Where(p => p.Team == Team.MTF && !p.Tag.Contains(" scp035")).ToList();
                List<Player> classd = Player.List.Where(p => p.Role == RoleType.ClassD && !p.IsSCP343() && !p.Tag.Contains(" scp035")).ToList();
                List<Player> chaos = Player.List.Where(p => p.Team == Team.CHI && !p.Tag.Contains(" scp035")).ToList();
                List<Player> scps = Player.List.Where(p => p.Team == Team.SCP && !p.Tag.Contains(" scp035")).ToList(); ;
                if (mtf.Count > 0 && classd.Count == 0 && scps.Count == 0 && chaos.Count == 0) ev.RoundEnd = true;
                else if (mtf.Count == 0 && classd.Count == 0 && scps.Count > 0) ev.RoundEnd = true;
                else if (mtf.Count > 0 && (classd.Count > 0 || chaos.Count > 0) && scps.Count == 0) ev.RoundEnd = false;
                else if (mtf.Count == 0 && classd.Count > 0 && scps.Count == 0) ev.RoundEnd = true;
                else if (mtf.Count > 0 && classd.Count == 0 && scps.Count == 0 && chaos.Count > 0) ev.RoundEnd = false;
                else if (mtf.Count == 0 && classd.Count == 0 && scps.Count == 0 && chaos.Count == 0) ev.RoundEnd = true;
            }
        }


        internal void OnSendingConsoleCommand(SendingConsoleEvent ev)
        {
            try
            {
                if (ev.Name.ToLower() == "heck343")
                {
                    ev.Allowed = false;
                    if (ev.Player.IsSCP343())
                    {
                        if (!scp343.cfg.scp343_heck)
                        {
                            ev.ReturnMessage = scp343.cfg.scp343_heckerrordisable;
                            return;
                        }
                        bool allowed = ev.Player.GetSCPBadge().canheck;
                        if (allowed)
                        {
                            ev.Player.SetRole(RoleType.ClassD);
                            ev.Player.DisableAllEffects();
                            KillSCP343(ev.Player);
                            if (scp343.cfg.scp343_alert) ev.Player.Broadcast(10, scp343.cfg.scp343_alertbackd);
                            ev.ReturnMessage = scp343.cfg.scp343_alertbackd;
                            return;
                        }
                        else
                        {
                            ev.ReturnMessage = $"Error.....{scp343.cfg.scp343_alertheckerrortime}";
                        }
                    }
                    else
                    {
                        ev.ReturnMessage = $"Error........{scp343.cfg.scp343_alertheckerrornot343}";
                    }
                }
                else if (ev.Name.ToLower() == "tp343")
                {
                    ev.Allowed = false;
                    if (ev.Player.IsSCP343())
                    {
                        List<Player> Players = Player.List.Where(e => !e.IsSCP343() && e.Role != RoleType.Spectator && e.Role != RoleType.None).ToList();
                        Player player = Players[RNG.Next(Players.Count)];
                        if (player == null || Players.Count < 1)
                        {
                            ev.ReturnMessage = scp343.cfg.scp343_notfoundplayer;
                        }
                        else
                        {
                            ev.Player.Position = player.Position;
                            ev.ReturnMessage = scp343.cfg.scp343_teleport_to_player.Replace("%user%", player.Nickname).Replace("%role%", player.Role.ToString());
                        }
                        ev.Player.ClearBroadcasts();
                        ev.Player.Broadcast(10, ev.ReturnMessage);
                    }
                    else
                    {
                        ev.ReturnMessage = $"Error........{scp343.cfg.scp343_alertheckerrornot343}";
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
                    if (!ev.Player.IsSCP343()) ev.ReturnMessage = $"{scp343.cfg.scp343_alertheckerrornot343}";
                    else
                    {
                        if (ev.Player.GetSCPBadge().canheal)
                        {
                            int count = 0;
                            int hpset = RNG.Next(scp343.cfg.scp343_min_heal_players, scp343.cfg.scp343_max_heal_players);
                            foreach (var ply in from x in Player.List where x.Role != RoleType.Spectator select x)
                            {
                                if (ply.IsSCP343()) continue;
                                bool boo = Vector3.Distance(ev.Player.Position, ply.Position) <= 5f;

                                //Log.Info($"Debug - {ply.Nickname} - {Vector3.Distance(ev.Player.Position, ply.Position)} - {boo}");
                                if (boo) ply.SetHP(hpset);
                                count++;
                            }
                            if (count == 0)
                            {
                                ev.ReturnMessage = scp343.cfg.scp343_notfoundplayer;
                            }
                            else
                            {
                                ev.Player.GetSCPBadge().canheal = false;
                                ev.ReturnMessage = scp343.cfg.scp343_healplayer;
                                Timing.CallDelayed(120f, () =>
                                {
                                    ev.Player.GetSCPBadge().canheal = true;
                                });
                            }
                        }
                        else
                        {
                            ev.ReturnMessage = scp343.cfg.scp343_cooldown;
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
                        ev.ReturnMessage = $"{scp343.cfg.scp343_alertheckerrornot343}";
                        return;
                    }
                    else
                    {
                        if (ev.Player.GetSCPBadge().revive343 == 0)
                        {
                            ev.ReturnMessage = scp343.cfg.scp343_cannotrevive;
                        }
                        else
                        {
                            Player player = null;
                            foreach (Player ply in Player.List.Where(p => deadPlayers.ContainsKey(p.Id) && p.Role == RoleType.Spectator))
                            {
                                if (player != null) continue;
                                bool boo = Vector3.Distance(ev.Player.Position, deadPlayers[player.Id].pos) <= 3f;
                                if (!boo) continue;
                                player = ply;
                            }
                            if (player == null) ev.ReturnMessage = scp343.cfg.scp343_notfoundplayer;
                            else
                            {
                                player.Role = deadPlayers[player.Id].role;
                                player.ClearInventory();
                                player.Broadcast(10, scp343.cfg.scp343_playerwhorevived);
                                Timing.CallDelayed(0.6f, () =>
                                {
                                    player.Position = deadPlayers[player.Id].pos;
                                    deadPlayers.Remove(player.Id);
                                });
                                ev.Player.GetSCPBadge().revive343--;
                                ev.ReturnMessage = scp343.cfg.scp343_revive_text.Replace("%user%", player.Nickname);
                            }
                        }
                    }
                    ev.Player.ClearBroadcasts();
                    ev.Player.Broadcast(10, ev.ReturnMessage);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        internal void OnEnteringPocketDimension(PocketDimensionEnterEvent ev)
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
            if (ev.Player.IsSCP343() && !scp343.cfg.scp343_nuke_interact) ev.Allowed = false;
        }

        internal void OnStopping(AlphaStopEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player == null) return;
            if (ev.Player.IsSCP343() && !scp343.cfg.scp343_nuke_interact) ev.Allowed = false;
        }

        internal void OnActivatingWarheadPanel(EnableAlphaPanelEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343() && !scp343.cfg.scp343_nuke_interact) ev.Allowed = false;
        }

        internal void OnHandcuffing(CuffEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Target.IsSCP343() || ev.Cuffer.IsSCP343()) ev.Allowed = false;
        }

        internal void OnInteractingLocker(InteractLockerEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343() && ev.Player.GetSCPBadge().canopendoor) ev.Allowed = scp343.cfg.scp343_canopenanydoor;
        }

        internal void OnDied(DiesEvent ev)
        {
            if (deadPlayers.ContainsKey(ev.Target.Id)) deadPlayers.Remove(ev.Target.Id);
            deadPlayers.Add(ev.Target.Id, new Badge(ev.Target, ev.Target.Role, ev.Target.Position));
            if (scp343badgelist.Count() < 1) return;
            Player player = ev.Target;
            if (player.IsSCP343())
            {
                KillSCP343(player);
            }
        }

        Random RNG => new Random();

        internal void OnHurting(DamageEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;

            if (ev.Target.IsSCP343() && ev.DamageType == DamageTypes.Scp207)
            {
                ev.Amount = 0f;
                ev.Allowed = false;
            }
            if (ev.Target.IsSCP343())
            {
                if (ev.DamageType == DamageTypes.Decont || ev.DamageType == DamageTypes.Nuke)
                {
                    ev.Amount = ev.Target.Hp;
                    return;
                }
                else
                {
                    ev.Amount = 0f;
                    if (ev.Attacker.Role.Is939()) ev.Target.DisableEffect(Qurre.API.Objects.EffectType.Amnesia);
                    ev.Allowed = false;
                }
            }
            if (ev.Attacker.IsSCP343())
            {
                ev.Amount = 0;
                ev.Allowed = false;
                if (!ev.Target.IsSCP343() && ev.DamageType == DamageTypes.Com15) RunTranq(ev.Target).RunCoroutine("userid -  scp343" + ev.Target.UserId);
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
            if (!scp343.cfg.IsEnabled)
            {
                plugin.Disable();
                return;
            }
            int count = Player.List.Count();
            int chance = count < 2 ? 10000 : RNG.Next(1, 100);
            if (chance <= scp343.cfg.scp343_spawnchance) return;
            if (scp343.cfg.minplayers > count) return;
            List<Player> ClassDList = Player.List.Where(p => p.Role == RoleType.ClassD).ToList();
            Player player = ClassDList[RNG.Next(ClassDList.Count)];
            ClassDList.Remove(player);
            Timing.CallDelayed(0.5f, () =>
            {
                spawn343(player);
            });

        }

        internal void OnInteractingDoor(InteractDoorEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343() && ev.Player.GetSCPBadge().canopendoor)
            {
                ev.Allowed = true;
            }
        }

        internal void OnChangingRole(RoleChangeEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            IReadOnlyCollection<Item> itemss = ev.Player.AllItems;
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
                    Player player = Player.Get(ev.Player.Id);
                    player.Position = pos;
                    foreach (var item in itemss)
                    {
                        player.AddItem(item);
                    }
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

        private static Vector3 FindLookRotation(Vector3 player, Vector3 target) => (target - player).normalized;

        internal void OnTransmitPlayerData(TransmitPlayerDataEvent ev)
        {
            if (ev.PlayerToShow.IsSCP343() && (ev.Player.Role == RoleType.Scp096 || ev.Player.Role == RoleType.Scp173))
            {
                Vector3 vector = FindLookRotation(ev.Player.Position, ev.PlayerToShow.Position);
                ev.Rotation = Quaternion.LookRotation(vector).eulerAngles.y;
            }
        }

        internal void OnEnraging(EnrageEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.Scp096Controller.Targets.Count <= 1)
            {
                if (ev.Player.Scp096Controller.Targets.All(p => p.IsSCP343())) ev.Allowed = false;
            }
            foreach (Player player in ev.Player.Scp096Controller.Targets.Where(p => p.IsSCP343())) ev.Player.Scp096Controller.RemoveTarget(player);
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
            if (ev.Player.IsSCP343() && !scp343.cfg.scp343_interact_scp914) ev.Allowed = false;
        }

        internal void OnEscaping(EscapeEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343())
            {
                ev.Allowed = scp343.cfg.scp343_canescape;
            }
        }

        public void OnUpgradePlayer(UpgradePlayerEvent ev)
        {
            if (ev.Player.IsSCP343()) GetEnumerator343(ev.Player).RunCoroutine("scp343-" + ev.Player.Id);
        }

        public void OnUpgrade(UpgradeEvent ev)
        {
            if (ev.Players.Any(p => p.IsSCP343()))
            {
                ev.Allowed = false;
                foreach (Player player in ev.Players.Where(p => p.IsSCP343())) player.Broadcast(scp343.cfg.scp343_youmustexit914, 10, true);
            }
        }

        internal void OnMedicalUsing(ItemUsingEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (!ev.Player.IsSCP343()) return;
            if (ev.Item.Type == ItemType.Adrenaline)
            {
                ev.Allowed = false;
            }
        }

        internal void OnDropingItem(DroppingItemEvent ev)
        {
            if (scp343badgelist.Count() < 0) return;
            if (!ev.Player.IsSCP343()) return;
            SendingConsoleEvent console = null;
            if (scp343.cfg.scp343_itemscannotdrop.Contains((int)ev.Item.Type)) ev.Allowed = false;
            if (ev.Item.Type == ItemType.Ammo556x45)
            {
                //ev.Allowed = false;
                console = new SendingConsoleEvent(ev.Player, ".tp343", ".tp343", new string[] { });
                IEnumerable<Player> Players = Player.List.Where(e => !e.IsSCP343() && e.Role != RoleType.Spectator && e.Role != RoleType.None);
                //Log.Error("hello");
                if (Players.Count() < 1)
                {
                    console.ReturnMessage = scp343.cfg.scp343_notfoundplayer;
                }
                else
                {
                    Player player = Players.ToList()[RNG.Next(Players.Count())];
                    if (player == null)
                    {
                        console.ReturnMessage = scp343.cfg.scp343_notfoundplayer;
                    }
                    else
                    {
                        ev.Player.Position = player.Position;
                        console.ReturnMessage = scp343.cfg.scp343_teleport_to_player.Replace("%user%", player.Nickname).Replace("%role%", player.Role.ToString());
                    }
                }
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(10, console.ReturnMessage);
                //ev.Player.ExecuteCommand(".tp343", false);
            }
            else if (ev.Item.Type == ItemType.Adrenaline)
            {
                //ev.Allowed = false;
                console = new SendingConsoleEvent(ev.Player, ".heal343", ".heal343", new string[] { });
                if (ev.Player.GetSCPBadge().canheal)
                {
                    int count = 0;
                    int hpset = RNG.Next(scp343.cfg.scp343_min_heal_players, scp343.cfg.scp343_max_heal_players);
                    foreach (var ply in Player.List.Where(p => p.Role != RoleType.Spectator))
                    {
                        if (Vector3.Distance(ev.Player.Position, ply.Position) <= 5f)
                        {
                            ply.SetHP(hpset);
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        console.ReturnMessage = scp343.cfg.scp343_notfoundplayer;
                    }
                    else
                    {
                        ev.Player.GetSCPBadge().canheal = false;
                        console.ReturnMessage = scp343.cfg.scp343_healplayer;
                        Timing.CallDelayed(120f, () => ev.Player.GetSCPBadge().canheal = true);
                    }
                }
                else
                {
                    console.ReturnMessage = scp343.cfg.scp343_cooldown;
                }
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(10, console.ReturnMessage);
            }
            else if (ev.Item.Type == ItemType.Flashlight)
            {
                //ev.Allowed = false;
                console = new SendingConsoleEvent(ev.Player, ".revive343", ".revive343", new string[] { });
                if (ev.Player.GetSCPBadge().revive343 == 0)
                {
                    console.ReturnMessage = scp343.cfg.scp343_cannotrevive;
                }
                else
                {
                    Player player = null;
                    foreach (Player ply in Player.List.Where(p => deadPlayers.ContainsKey(p.Id) && p.Role == RoleType.Spectator))
                    {
                        bool boo = Vector3.Distance(ev.Player.Position, deadPlayers[player.Id].pos) <= 3f;
                        if (!boo) continue;
                        player = ply;
                        break;
                    }
                    if (player == null) console.ReturnMessage = scp343.cfg.scp343_notfoundplayer;
                    else
                    {
                        player.Role = deadPlayers[player.Id].role;
                        player.ClearInventory();
                        player.Broadcast(10, scp343.cfg.scp343_playerwhorevived);
                        Timing.CallDelayed(0.6f, () =>
                        {
                            player.Position = deadPlayers[player.Id].pos;
                            deadPlayers.Remove(player.Id);
                        }); ;
                        console.ReturnMessage = scp343.cfg.scp343_revive_text.Replace("%user%", player.Nickname);
                        ev.Player.GetSCPBadge().revive343--;
                    }
                }
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(10, console.ReturnMessage);
            }
        }

        internal void OnUnlockingGenerator(InteractGeneratorEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343() && ev.Player.GetSCPBadge().canopendoor) ev.Generator.Locked = !ev.Generator.Locked;
        }
        internal void OnTriggeringTesla(TeslaTriggerEvent ev)
        {
            if (scp343.cfg.scp343_activating_tesla_in_range)
            {
                TeslaGate teslaGate = ev.Tesla.GameObject.GetComponent<TeslaGate>();
                IEnumerable<Player> Players = ReferenceHub.GetAllHubs().Values.Select(h => Player.Get(h)).Where(x => x.Role != RoleType.Spectator && teslaGate.PlayerInRange(x.ReferenceHub));
                if (Players.Count() > 0)
                {
                    if (Players.Any(p => p.IsSCP343())) ev.Triggerable = false;
                }
            }
            else if (ev.Player.IsSCP343()) ev.Triggerable = false;
        }

        internal void OnPickingUpItem(PickupItemEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343())
            {
                //if (ev.Pickup.Serial == 1337035) ev.Allowed = false;
                if (!scp343.cfg.scp343_itemconverttoggle)
                {
                    ev.Allowed = false;
                    return;
                }
                int itemid = (int)ev.Pickup.Type;
                if (scp343.cfg.scp343_itemdroplist.IndexOf(itemid) > 0)
                {
                    ev.Allowed = false;
                }
                else if (scp343.cfg.scp343_itemstoconvert.IndexOf(itemid) > 0)
                {
                    if (!scp343.cfg.scp343_itemconverttoggle)
                    {
                        ev.Allowed = false;
                        return;
                    }
                    ev.Allowed = false;
                    foreach (int i in scp343.cfg.scp343_converteditems)
                    {
                        if (i >= 0)
                        {
                            ev.Allowed = false;
                            ev.Pickup.Destroy();
                            ItemType item = (ItemType)i;
                            ev.Player.AddItem(item);
                        }
                    }
                }
                else ev.Allowed = true;
            }
        }

    }
}
