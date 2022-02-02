
using System;
using Qurre.API;
using SCP343.Handlers;
using PLAYER = Qurre.Events.Player;
using SERVER = Qurre.Events.Server;
using Scps914 = Qurre.Events.Scp914;
using SCP106 = Qurre.Events.Scp106;
using WARHEAD = Qurre.Events.Alpha;
using SCP096 = Qurre.Events.Scp096;
using Voice = Qurre.Events.Voice;
using ROUND = Qurre.Events.Round;
using HarmonyLib;
using System.Collections.Generic;
using Qurre;
using System.Linq;

namespace SCP343
{
    public static class API
    {
        /// <summary>
        /// <para>This spawns <see cref="Player"/> as scp343</para>
        /// </summary>
        /// <returns><see cref="Badge"/></returns>
        public static Badge Spawn343(Player player, UnityEngine.Vector3 position = default) => Eventhandlers.spawn343(player, position: position);
        /// <summary>
        /// <para>Just a list</para>
        /// </summary>
        public static IEnumerable<Player> AllScps343 => AllScp343Badges.Select(b => b.Player);
        public static IEnumerable<Badge> AllScp343Badges => scp343badgelist.Get(b => b.IsSCP343);
        /// <summary>
        /// <para>This kills scp343</para>
        /// </summary>
        public static void Kill343(Player player) => Eventhandlers.KillSCP343(player);
    }

    public class Scp343 : Plugin
    {
        public static Cfg CustomConfig { get; } = new Cfg();

        private static Eventhandlers Eventhandlers { get; set; } = null;
        public override int Priority => 10;
        public override string Name => "SCP-343";
        public override string Developer => "Maniac Devil Knuckles";
        public override Version Version => new Version(3, 2, 1);
        public override Version NeededQurreVersion => new Version(1, 11, 1);
        internal static Scp343 Instance { get; set; } = null;
        public Harmony harmony { get; internal set; } = null;
        internal int i = 0;

        public override void Enable()
        {
            try
            {
                CustomConfigs.Add(CustomConfig);
                if (!CustomConfig.IsEnabled)
                {
                    Log.Info("Disabled plugin by Config");
                    return;
                }
                try
                {
                    harmony = new Harmony("knuckles.scp343\nVersion " + i++);
                    harmony.PatchAll();
                    Log.Info("cool");
                }
                catch (Exception ex)
                {
                    Log.Info("error\n\n\n\n\n\n\n\\n\n");
                    Log.Info(ex);//
                }
                Instance = this;
                Eventhandlers = new Eventhandlers(this);
                Log.Info("Enabling SCP343 by Maniac Devil Knuckles");
                PLAYER.TransmitPlayerData += Eventhandlers.OnTransmitPlayerData;
                PLAYER.Join += Eventhandlers.OnJoin;
                PLAYER.Shooting += Eventhandlers.OnShooting;
                ROUND.Waiting += Eventhandlers.WaitingForPlayers;
                ROUND.Check += Eventhandlers.OnRoundEnding;
                PLAYER.TeslaTrigger += Eventhandlers.OnTriggeringTesla;
                ROUND.Start += Eventhandlers.OnRoundStarted;
                SERVER.SendingConsole += Eventhandlers.OnSendingConsoleCommand;
                PLAYER.InteractDoor += Eventhandlers.OnInteractingDoor;
                PLAYER.InteractLift += Eventhandlers.OnInteractingElevator;
                ROUND.Restart += Eventhandlers.OnRestartingRound;
                PLAYER.Leave += Eventhandlers.OnPlayerLeft;
                SCP106.Contain += Eventhandlers.OnContaining;
                SCP096.Enrage += Eventhandlers.OnEnraging;
                SCP096.AddTarget += Eventhandlers.OnAddingTarget;
                Scps914.Activating += Eventhandlers.OnActivating;
                PLAYER.PickupItem += Eventhandlers.OnPickingUpItem;
                WARHEAD.Starting += Eventhandlers.OnStarting;
                WARHEAD.Stopping += Eventhandlers.OnStopping;
                WARHEAD.EnablePanel += Eventhandlers.OnActivatingWarheadPanel;
                SCP106.PocketEnter += Eventhandlers.OnEnteringPocketDimension;
                Qurre.Events.Map.NewBlood += Eventhandlers.OnPlacingBlood;
                PLAYER.Cuff += Eventhandlers.OnHandcuffing;
                PLAYER.Damage += Eventhandlers.OnHurting;
                PLAYER.Dies += Eventhandlers.OnDied;
                PLAYER.RoleChange += Eventhandlers.OnChangingRole;
                PLAYER.Escape += Eventhandlers.OnEscaping;
                PLAYER.DroppingItem += Eventhandlers.OnDropingItem;
                PLAYER.ItemUsing += Eventhandlers.OnItemUsing;
                Scps914.UpgradePlayer += Eventhandlers.OnUpgradePlayer;
                Scps914.Upgrade += Eventhandlers.OnUpgrade;
                PLAYER.InteractGenerator += Eventhandlers.OnUnlockingGenerator;
                PLAYER.InteractLocker += Eventhandlers.OnInteractLocker;
                PLAYER.ScpAttack += Eventhandlers.OnScpAttack;
                Voice.PressPrimaryChat += Eventhandlers.OnVoiceSpeak;
                Voice.PressAltChat += Eventhandlers.OnAltVoiceSpeak;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public override void Disable()
        {
            CustomConfigs.Remove(CustomConfig);
            Log.Info("Disabling SCP343 by Maniac Devil Knuckles");
            harmony.UnpatchAll(harmony.Id);
            harmony = null;
            PLAYER.Shooting -= Eventhandlers.OnShooting;
            PLAYER.Join -= Eventhandlers.OnJoin;
            PLAYER.TransmitPlayerData -= Eventhandlers.OnTransmitPlayerData;
            ROUND.Waiting -= Eventhandlers.WaitingForPlayers;
            ROUND.Check -= Eventhandlers.OnRoundEnding;
            PLAYER.TeslaTrigger -= Eventhandlers.OnTriggeringTesla;
            ROUND.Start -= Eventhandlers.OnRoundStarted;
            SERVER.SendingConsole -= Eventhandlers.OnSendingConsoleCommand;
            PLAYER.InteractDoor -= Eventhandlers.OnInteractingDoor;
            PLAYER.InteractLift -= Eventhandlers.OnInteractingElevator;
            ROUND.Restart -= Eventhandlers.OnRestartingRound;
            PLAYER.Leave -= Eventhandlers.OnPlayerLeft;
            SCP106.Contain -= Eventhandlers.OnContaining;
            SCP096.Enrage -= Eventhandlers.OnEnraging;
            SCP096.AddTarget -= Eventhandlers.OnAddingTarget;
            Scps914.Activating -= Eventhandlers.OnActivating;
            PLAYER.PickupItem -= Eventhandlers.OnPickingUpItem;
            WARHEAD.Starting -= Eventhandlers.OnStarting;
            WARHEAD.Stopping -= Eventhandlers.OnStopping;
            WARHEAD.EnablePanel -= Eventhandlers.OnActivatingWarheadPanel;
            SCP106.PocketEnter -= Eventhandlers.OnEnteringPocketDimension;
            Qurre.Events.Map.NewBlood -= Eventhandlers.OnPlacingBlood;
            PLAYER.Cuff -= Eventhandlers.OnHandcuffing;
            PLAYER.Damage -= Eventhandlers.OnHurting;
            PLAYER.Dies -= Eventhandlers.OnDied;
            PLAYER.RoleChange -= Eventhandlers.OnChangingRole;
            PLAYER.Escape -= Eventhandlers.OnEscaping;
            PLAYER.DroppingItem -= Eventhandlers.OnDropingItem;
            PLAYER.ItemUsing -= Eventhandlers.OnItemUsing;
            Scps914.UpgradePlayer -= Eventhandlers.OnUpgradePlayer;
            Scps914.Upgrade -= Eventhandlers.OnUpgrade;
            PLAYER.InteractGenerator -= Eventhandlers.OnUnlockingGenerator;
            PLAYER.InteractLocker -= Eventhandlers.OnInteractLocker;
            Eventhandlers = null;
        }

    }
}
