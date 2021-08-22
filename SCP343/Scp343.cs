
using System;
using Qurre.API;
using SCP343.Handlers;
using PLAYER = Qurre.Events.Player;
using SERVER = Qurre.Events.Server;
using Scps914 = Qurre.Events.Scp914;
using SCP106 = Qurre.Events.Scp106;
using WARHEAD = Qurre.Events.Alpha;
using SCP096 = Qurre.Events.Scp096;
using HarmonyLib;
using System.Collections.Generic;
using cmd = SCP343.Commands;
using System.Linq;
using System.ComponentModel;
using MEC;
using Qurre;
using RemoteAdmin;
using System.Reflection;
using CommandSystem;

namespace SCP343
{
    public static class API
    {
        /// <summary>
        /// This spawns <see cref="Player"/> as scp343 and returns <see cref="Badge"/>
        /// </summary>
        public static Badge Spawn343(Player player) => scp343.Players.spawn343(player);
        /// <summary>
        /// This returns List of <see cref="Player"/>
        /// </summary>
        public static IEnumerable<Player> AllScps343
        {
            get
            {
                IEnumerable<Player> players = scp343badgelist.List;
                return players;
            }
        }
        public static IEnumerable<Badge> AllScp343Badges
        {
            get
            {
                IEnumerable<Badge> badges = scp343badgelist.ListBadges;
                return badges;
            }
        }
        /// <summary>
        /// This kills scp343
        /// </summary>
        public static void Kill343(Player player) => scp343.Players.KillSCP343(player);
    }

    public class scp343 : Plugin
    {
        public static Players Players { get; private set; } = null;
        public override int Priority => int.MaxValue;
        public override string Name => "SCP-343";
        public override string Developer => "Maniac Devil Knuckles";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version NeededQurreVersion => new Version(1, 5, 0);
        internal static scp343 Instance { get; set; } = null;
        public Harmony harmony { get; set; } = null;
        internal int i { get; set; } = 0;
        public scp343() { }
        internal static Config cfg { get; } = new Config();
        public override void Enable()
        {
            try
            {
                if (!cfg.IsEnabled)
                {
                    Disable();
                    return;
                }
                Instance = this;
                try
                {
                    //Config.betaitemsatspawn.ParseInventorySettings();
                    harmony = new Harmony("knuckles.scp343\nVersion " + i++);
                    harmony.PatchAll();
                    Log.Info("cool");
                }
                catch (Exception ex)
                {
                    Log.Info("error\n\n\n\n\n\n\n\\n\n");
                    Log.Info(ex);//
                }
                OnRegisteringCommands();
                Players = new Players(this);
                Log.Info("Enabling SCP343 by Maniac Devil Knuckles");
                SCP343.Config.Reload();
                PLAYER.TransmitPlayerData += Players.OnTransmitPlayerData;
                PLAYER.Shooting += Players.OnShooting;
                Qurre.Events.Round.WaitingForPlayers += Players.WaitingForPlayers;
                Qurre.Events.Round.Check += Players.OnRoundEnding;
                PLAYER.TeslaTrigger += Players.OnTriggeringTesla;
                Qurre.Events.Round.Start += Players.OnRoundStarted;
                SERVER.SendingConsole += Players.OnSendingConsoleCommand;
                PLAYER.InteractDoor += Players.OnInteractingDoor;
                PLAYER.InteractLift += Players.OnInteractingElevator;
                Qurre.Events.Round.Restart += Players.OnRestartingRound;
                PLAYER.Leave += Players.OnPlayerLeft;
                SCP106.Contain += Players.OnContaining;
                SCP096.Enrage += Players.OnEnraging;
                SCP096.AddTarget += Players.OnAddingTarget;
                Scps914.Activating += Players.OnActivating;
                PLAYER.InteractLocker += Players.OnInteractingLocker;
                PLAYER.PickupItem += Players.OnPickingUpItem;
                WARHEAD.Starting += Players.OnStarting;
                WARHEAD.Stopping += Players.OnStopping;
                WARHEAD.EnablePanel += Players.OnActivatingWarheadPanel;
                SCP106.PocketDimensionEnter += Players.OnEnteringPocketDimension;
                Qurre.Events.Map.NewBlood += Players.OnPlacingBlood;
                PLAYER.Cuff += Players.OnHandcuffing;
                PLAYER.Damage += Players.OnHurting;
                PLAYER.Dies += Players.OnDied;
                PLAYER.RoleChange += Players.OnChangingRole;
                PLAYER.Escape += Players.OnEscaping;
                PLAYER.DroppingItem += Players.OnDropingItem;
                PLAYER.ItemUsing += Players.OnMedicalUsing;
                Scps914.UpgradePlayer += Players.OnUpgradePlayer;
                Scps914.Upgrade += Players.OnUpgrade;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public override void Disable()
        {
            Log.Info("Disabling SCP343 by Maniac Devil Knuckles");
            harmony.UnpatchAll(harmony.Id);
            harmony = null;
            PLAYER.Shooting -= Players.OnShooting;
            PLAYER.TransmitPlayerData -= Players.OnTransmitPlayerData;
            Qurre.Events.Round.WaitingForPlayers -= Players.WaitingForPlayers;
            Qurre.Events.Round.Check -= Players.OnRoundEnding;
            PLAYER.TeslaTrigger -= Players.OnTriggeringTesla;
            Qurre.Events.Round.Start -= Players.OnRoundStarted;
            SERVER.SendingConsole -= Players.OnSendingConsoleCommand;
            PLAYER.InteractDoor -= Players.OnInteractingDoor;
            PLAYER.InteractLift -= Players.OnInteractingElevator;
            Qurre.Events.Round.Restart -= Players.OnRestartingRound;
            PLAYER.Leave -= Players.OnPlayerLeft;
            SCP106.Contain -= Players.OnContaining;
            SCP096.Enrage -= Players.OnEnraging;
            SCP096.AddTarget -= Players.OnAddingTarget;
            Scps914.Activating -= Players.OnActivating;
            PLAYER.InteractLocker -= Players.OnInteractingLocker;
            PLAYER.PickupItem -= Players.OnPickingUpItem;
            WARHEAD.Starting -= Players.OnStarting;
            WARHEAD.Stopping -= Players.OnStopping;
            WARHEAD.EnablePanel -= Players.OnActivatingWarheadPanel;
            SCP106.PocketDimensionEnter -= Players.OnEnteringPocketDimension;
            Qurre.Events.Map.NewBlood -= Players.OnPlacingBlood;
            PLAYER.Cuff -= Players.OnHandcuffing;
            PLAYER.Damage -= Players.OnHurting;
            PLAYER.Dies -= Players.OnDied;
            PLAYER.RoleChange -= Players.OnChangingRole;
            PLAYER.Escape -= Players.OnEscaping;
            PLAYER.DroppingItem -= Players.OnDropingItem;
            PLAYER.ItemUsing -= Players.OnMedicalUsing;
            Scps914.UpgradePlayer -= Players.OnUpgradePlayer;
            Scps914.Upgrade -= Players.OnUpgrade;
            Players = null; ;
        }
        //public override void OnReloaded() { }
        internal void RemoveGodMode()
        {
            if (Instance == null) return;
            foreach (Player player in Player.List) if (player.GodMode && player.Role != RoleType.Tutorial && player.Role != RoleType.Spectator) player.GodMode = false;
            Timing.CallDelayed(10f, () => RemoveGodMode());
        }
        public Dictionary<Type, Dictionary<Type, ICommand>> Commands { get; } = new Dictionary<Type, Dictionary<Type, ICommand>>()
        {
            { typeof(RemoteAdminCommandHandler), new Dictionary<Type, ICommand>() },
            { typeof(GameConsoleCommandHandler), new Dictionary<Type, ICommand>() },
            { typeof(ClientCommandHandler), new Dictionary<Type, ICommand>() },
        };
        Assembly Assembly => Instance.GetType().Assembly;
        public void OnRegisteringCommands()
        {
            foreach (Type type in Assembly.GetTypes())
            {
                if (type.GetInterface("ICommand") != typeof(ICommand))
                    continue;

                if (!Attribute.IsDefined(type, typeof(CommandHandlerAttribute)))
                    continue;
                    
                foreach (CustomAttributeData customAttributeData in type.CustomAttributes)
                {
                    try
                    {
                        if (customAttributeData.AttributeType != typeof(CommandHandlerAttribute))
                            continue;

                        Type commandType = (Type)customAttributeData.ConstructorArguments?[0].Value;

                        if (!Commands.TryGetValue(commandType, out Dictionary<Type, ICommand> typeCommands))
                            continue;

                        if (!typeCommands.TryGetValue(type, out ICommand command))
                            command = (ICommand)Activator.CreateInstance(type);

                        if (commandType == typeof(RemoteAdminCommandHandler))
                            CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(command);
                        else if (commandType == typeof(GameConsoleCommandHandler))
                            GameCore.Console.singleton.ConsoleCommandHandler.RegisterCommand(command);
                        else if (commandType == typeof(ClientCommandHandler))
                            QueryProcessor.DotCommandHandler.RegisterCommand(command);
                        Commands[commandType][type] = command;
                    }
                    catch (Exception exception)
                    {
                        Log.Error($"An error has occurred while registering a command: {exception}");
                    }
                }
            }
        }
    }
}
