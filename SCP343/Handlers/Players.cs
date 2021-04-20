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
using Qurre.API.Objects;
using Qurre.API.Controllers;

namespace SCP343.Handlers
{
    public partial class Players
    {

        private scp343 plugin;
        private Dictionary<int, Badge> deadplayers { get; } = new Dictionary<int, Badge>();
        internal Players(scp343 plugin)
        {
            this.plugin = plugin;
        }

        internal void WaitingForPlayers()
        {
            Map.SetElevatorsMovingSpeed(scp343.cfg.lift_moving_speed);
            if (string.IsNullOrEmpty(scp343.cfg.scp343_unitname)) Round.AddUnit(TeamUnitType.ClassD, scp343.cfg.scp343_unitname);
            Map.UnitUpdate();
        }

        internal void OnPlacingBlood(NewBloodEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343()) ev.Allowed = false;
        }

        internal void OnPlayerLeft(LeaveEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player?.UserId == null || ev.Player.IsHost || ev.Player.IP == "127.0.0.WAN" || ev.Player.IP == "127.0.0.1") return;
            if (ev.Player.IsSCP343()) KillSCP343(ev.Player);
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
                List<Player> mtf = Player.List.Where(p => p.Team == Team.MTF).ToList();
                List<Player> classd = Player.List.Where(p => p.Role == RoleType.ClassD && !p.IsSCP343()).ToList();
                List<Player> chaos = Player.List.Where(p => p.Role == RoleType.ChaosInsurgency).ToList();
                List<Player> scps = Player.List.Where(p => p.Team == Team.SCP).ToList(); ;
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
            if (ev.Name.ToLower() == "invis")
            {
                /*
                return;
                ev.Allowed = false;
                if(!adminsor343(ev.Player))
                {
                    ev.ReturnMessage = "Вы не scp343 или вы не Админ";
                    return;
                }
                //ev.Player.IsInvisible=!ev.Player.IsInvisible
                //ev.ReturnMessage = $"Вы стали {(ev.Player.IsInvisible()?"невидимым":"видимым")}";
                if(!ev.Player.IsSCP343())
                {
                    if (!ev.Player.HasItem(ItemType.Flashlight)) ev.Player.AddItem(ItemType.Flashlight);
                    if (ev.Player.Role == RoleType.Tutorial) return;
                    Vector3 pos = ev.Player.Position;
                    ev.Player.Role = RoleType.Tutorial;
                    Timing.CallDelayed(0.6f, () =>
                    {
                        ev.Player.Position = pos;
                        if (!ev.Player.HasItem(ItemType.Flashlight)) ev.Player.AddItem(ItemType.Flashlight);
                    });
                }
                return;
                //*/
            }
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
                    List<Player> players = Player.List.Where(e => !e.IsSCP343() && e.Role != RoleType.Spectator && e.Role != RoleType.None).ToList();
                    Player player = players[RNG.Next(players.Count)];
                    if (player == null || players.Count < 1)
                    {
                        ev.ReturnMessage = $"Не найден живой человек";
                    }
                    else
                    {
                        ev.Player.Position = player.Position;
                        ev.ReturnMessage = $"Вы телепортнулись к {player.Nickname} играющего за {player.Role}";
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
                        foreach (var ply in from x in Player.List where x.Role != RoleType.Spectator select x)
                        {
                            if (ply.IsSCP343()) continue;
                            bool boo = Vector3.Distance(ev.Player.Position, ply.Position) <= 5f;

                            //Log.Info($"Debug - {ply.Nickname} - {Vector3.Distance(ev.Player.Position, ply.Position)} - {boo}");
                            if (boo) ply.HP = ply.MaxHP;
                            count++;
                        }
                        if (count == 0)
                        {
                            ev.ReturnMessage = "Рядом с вам никого не было";
                        }
                        else
                        {
                            Badge badge = ev.Player.GetSCPBadge();
                            badge.canheal = false;
                            badge.SaveBadge343();
                            ev.ReturnMessage = "Вы восстановили игрокам hp";
                            Timing.CallDelayed(120f, () =>
                            {
                                badge.canheal = true;
                                badge.SaveBadge343();
                            });
                        }
                    }
                    else
                    {
                        ev.ReturnMessage = "Ожидайте кулдаун на восстановление игрокам hp";
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
                        ev.ReturnMessage = $"Вы больше не можете возродить людей";
                    }
                    else
                    {
                        Player player = null;
                        foreach (Player ply in from Player x in Player.List where x.Role == RoleType.Spectator && deadplayers.ContainsKey(x.Id) select x)
                        {
                            if (player != null) continue;
                            bool boo = Vector3.Distance(ev.Player.Position, ply.Position) <= 1f;
                            //    Log.Info($"Debug - {ply.Nickname} - {Vector3.Distance(ev.Player.Position, ply.Position)} - {boo}");
                            if (!boo) continue;
                            player = ply;
                        }
                        if (player == null) ev.ReturnMessage = "Рядом с вам никого нет или вам надо близко подойти к трупу";
                        else
                        {
                            player.Role = deadplayers[player.Id].role;
                            player.ClearInventory();
                            player.Broadcast(10, "scp343 только что возродил вас");
                            Timing.CallDelayed(0.6f, () =>
                            {
                                player.Position = deadplayers[player.Id].pos;
                                deadplayers.Remove(player.Id);
                            });
                            Badge badge = ev.Player.GetSCPBadge();
                            badge.revive343--;
                            badge.SaveBadge343();
                            ev.ReturnMessage = $"Вы возродили {player.Nickname}";
                        }
                    }
                }
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(10, ev.ReturnMessage);
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
            if (!deadplayers.ContainsKey(ev.Target.Id)) deadplayers.Add(ev.Target.Id, new Badge(ev.Target, ev.Target.Role, ev.Target.Position));


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
                    ev.Amount = ev.Target.HP;
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

            int chance = Player.UserIDPlayers.Count < 2 ? 10000 : RNG.Next(1, 100);
            int count = Player.List.Count();
            if (chance <= scp343.cfg.scp343_spawnchance) return;
            if (scp343.cfg.minplayers > count && count != 1) return;
            List<Player> ClassDList = new List<Player>();
            foreach (Player play in Player.List)
            {
                if (play.Role == RoleType.ClassD) ClassDList.Add(play);
            }
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
            if (ev.Player.IsSCP343() && scp343badgelist.Get(ev.Player).canopendoor)
            {
                ev.Allowed = true;
            }
        }

        internal void OnChangingRole(RoleChangeEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            List<ItemType> itemss = new List<ItemType>();
            foreach (var ite in ev.Player.Inventory.items)
            {
                itemss.Add(ite.id);
            }
            if (ev.Player.IsSCP343() && ev.NewRole != RoleType.Scp0492)
            {
                KillSCP343(ev.Player);
            }
            else if (ev.Player.IsSCP343() && ev.NewRole == RoleType.Scp0492)
            {
                ev.NewRole = RoleType.ClassD;
                Vector3 pos = ev.Player.Position;
                Timing.CallDelayed(0.4f, () => { spawn343(ev.Player, true); });
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
            if (ev.Player == null || ev.Player.IP == "127.0.0.WAN" || ev.Player.IP == "127.0.0.1") return;
            if (ev.Player.IsSCP343()) KillSCP343(ev.Player);
            if (deadplayers.ContainsKey(ev.Player.Id)) deadplayers.Remove(ev.Player.Id);
        }

        internal void OnContaining(ContainEvent ev)
        {
                if (scp343badgelist.Count() < 1) return;
                if (ev.Player.IsSCP343()) ev.Allowed = false;
        }

        internal void OnEnraging(EnrageEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343())
            {
                ev.Allowed = false;
            }
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
            if (ev.Player.IsSCP343()) ev.Allowed = false;
        }

        internal void OnEscaping(EscapeEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343())
            {
                ev.Allowed = scp343.cfg.scp343_canescape;
            }
        }

        internal void OnMedicalUsing(MedicalUsingEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (!ev.Player.IsSCP343()) return;
            if (ev.Item == ItemType.Adrenaline)
            {
                ev.Allowed = false;
                ev.Cooldown = 1000f;
            }
        }

        internal void OnDropingItem(DroppingItemEvent ev)
        {
            if (scp343badgelist.Count() < 0) return;
            if (!ev.Player.IsSCP343()) return;
            SendingConsoleEvent console = null;
            if (ev.Item.id == ItemType.Ammo556)
            {
                ev.Allowed = false;
                console = new SendingConsoleEvent(ev.Player, ".tp343", ".tp343", new string[] { }, true);
                IEnumerable<Player> players = Player.List.Where(e => !e.IsSCP343() && e.Role != RoleType.Spectator && e.Role != RoleType.None);
                //Log.Error("hello");
                if (players.Count() < 1)
                {
                    console.ReturnMessage = $"Не найден живой человек";
                }
                else
                {
                    Player player = players.ToList()[RNG.Next(players.Count())];
                    if (player == null)
                    {
                        console.ReturnMessage = $"Не найден живой человек";
                    }
                    else
                    {
                        ev.Player.Position = player.Position;
                        console.ReturnMessage = $"Вы телепортнулись к {player.Nickname} играющего за {player.Role}";
                    }
                }
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(10, console.ReturnMessage);
                //ev.Player.ExecuteCommand(".tp343", false);
            }
            else if (ev.Item.id == ItemType.Adrenaline)
            {
                ev.Allowed = false;
                console = new SendingConsoleEvent(ev.Player, ".heal343", ".heal343", new string[] { }, true);
                if (ev.Player.GetSCPBadge().canheal)
                {
                    int count = 0;
                    foreach (var ply in from x in Player.List where x.Role != RoleType.Spectator select x)
                    {
                        if (ply.IsSCP343()) continue;
                        bool boo = Vector3.Distance(ev.Player.Position, ply.Position) <= 5f;

                        //Log.Info($"Debug - {ply.Nickname} - {Vector3.Distance(ev.Player.Position, ply.Position)} - {boo}");
                        if (boo) ply.HP = ply.MaxHP;
                        count++;
                    }
                    if (count == 0)
                    {
                        console.ReturnMessage = "Рядом с вам никого не было";
                    }
                    else
                    {
                        Badge badge = ev.Player.GetSCPBadge();
                        badge.canheal = false;
                        badge.SaveBadge343();
                        console.ReturnMessage = "Вы восстановили игрокам hp";
                        Timing.CallDelayed(120f, () => ev.Player.GetSCPBadge().canheal = true);
                    }
                }
                else
                {
                    console.ReturnMessage = "Ожидайте кулдаун на восстановление игрокам hp";
                }
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(10, console.ReturnMessage);
            }
            else if (ev.Item.id == ItemType.Flashlight)
            {
                ev.Allowed = false;
                console = new SendingConsoleEvent(ev.Player, ".revive343", ".revive343", new string[] { }, true);
                if (ev.Player.GetSCPBadge().revive343 == 0)
                {
                    console.ReturnMessage = $"Вы больше не можете возродить людей";
                }
                else
                {
                    Player player = null;
                    foreach (Player ply in from Player x in Player.List where x.Role == RoleType.Spectator && deadplayers.ContainsKey(x.Id) select x)
                    {
                        if (player != null) continue;
                        bool boo = Vector3.Distance(ev.Player.Position, ply.Position) <= 1f;
                        //    Log.Info($"Debug - {ply.Nickname} - {Vector3.Distance(ev.Player.Position, ply.Position)} - {boo}");
                        if (!boo) continue;
                        player = ply;
                    }
                    if (player == null) console.ReturnMessage = "Рядом с вам никого нет или вам надо близко подойти к трупу";
                    else
                    {
                        player.Role = deadplayers[player.Id].role;
                        player.ClearInventory();
                        player.Broadcast(10, "scp343 только что возродил вас");
                        Timing.CallDelayed(0.6f, () =>
                        {
                            player.Position = deadplayers[player.Id].pos;
                            deadplayers.Remove(player.Id);
                        }); ;
                        console.ReturnMessage = $"Вы возродили {player.Nickname}";
                        Badge badge = ev.Player.GetSCPBadge();
                        badge.revive343--;
                        badge.SaveBadge343();
                    }
                }
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(10, console.ReturnMessage);
            }
        }

        internal void OnUnlockingGenerator(InteractGeneratorEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343() && ev.Player.GetSCPBadge().canopendoor) ev.Allowed = true;
        }
        internal void OnTriggeringTesla(TeslaTriggerEvent ev)
        {
            if (ev.Player.IsSCP343()) ev.Triggerable = false;
        }
        internal void OnPickingUpItem(PickupItemEvent ev)
        {
            if (scp343badgelist.Count() < 1) return;
            if (ev.Player.IsSCP343())
            {
                if (!scp343.cfg.scp343_itemconverttoggle)
                {
                    ev.Allowed = false;
                    return;
                }
                int itemid = (int)ev.Pickup.ItemId;
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
                            ev.Pickup.Delete();
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
