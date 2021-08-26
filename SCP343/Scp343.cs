
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
        public static Badge Spawn343(Player player, UnityEngine.Vector3 position = default) => scp343.Eventhandlers.spawn343(player, position: position);
        /// <summary>
        /// This returns List of <see cref="Player"/>
        /// </summary>
        public static IEnumerable<Player> AllScps343
        {
            get
            {
                IEnumerable<Player> Eventhandlers = scp343badgelist.List;
                return Eventhandlers;
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
        public static void Kill343(Player player) => scp343.Eventhandlers.KillSCP343(player);
    }

    public class scp343 : Plugin
    {
        internal static Eventhandlers Eventhandlers { get; private set; } = null;
        public override int Priority => 0;
        public override string Name => "SCP-343";
        public override string Developer => "Maniac Devil Knuckles";
        public override Version Version { get; } = new Version(1, 2, 3);
        public override Version NeededQurreVersion => new Version(1, 8, 4);
        internal static scp343 Instance { get; set; } = null;
        public Harmony harmony { get; set; } = null;
        internal int i { get; set; } = 0;
        public scp343() { }
        internal static Config cfg { get; } = new Config();
        public override void Enable()
        {
            try
            {
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
                SCP343.Config.Reload();
                if (!cfg.IsEnabled)
                {
                    Disable();
                    return;
                }
                Instance = this;
                OnRegisteringCommands();
                Eventhandlers = new Eventhandlers(this);
                Log.Info("Enabling SCP343 by Maniac Devil Knuckles");
                PLAYER.TransmitPlayerData += Eventhandlers.OnTransmitPlayerData;
                PLAYER.Shooting += Eventhandlers.OnShooting;
                Qurre.Events.Round.WaitingForPlayers += Eventhandlers.WaitingForPlayers;
                Qurre.Events.Round.Check += Eventhandlers.OnRoundEnding;
                PLAYER.TeslaTrigger += Eventhandlers.OnTriggeringTesla;
                Qurre.Events.Round.Start += Eventhandlers.OnRoundStarted;
                SERVER.SendingConsole += Eventhandlers.OnSendingConsoleCommand;
                PLAYER.InteractDoor += Eventhandlers.OnInteractingDoor;
                PLAYER.InteractLift += Eventhandlers.OnInteractingElevator;
                Qurre.Events.Round.Restart += Eventhandlers.OnRestartingRound;
                PLAYER.Leave += Eventhandlers.OnPlayerLeft;
                SCP106.Contain += Eventhandlers.OnContaining;
                SCP096.Enrage += Eventhandlers.OnEnraging;
                SCP096.AddTarget += Eventhandlers.OnAddingTarget;
                Scps914.Activating += Eventhandlers.OnActivating;
                PLAYER.InteractLocker += Eventhandlers.OnInteractingLocker;
                PLAYER.PickupItem += Eventhandlers.OnPickingUpItem;
                WARHEAD.Starting += Eventhandlers.OnStarting;
                WARHEAD.Stopping += Eventhandlers.OnStopping;
                WARHEAD.EnablePanel += Eventhandlers.OnActivatingWarheadPanel;
                SCP106.PocketDimensionEnter += Eventhandlers.OnEnteringPocketDimension;
                Qurre.Events.Map.NewBlood += Eventhandlers.OnPlacingBlood;
                PLAYER.Cuff += Eventhandlers.OnHandcuffing;
                PLAYER.Damage += Eventhandlers.OnHurting;
                PLAYER.Dies += Eventhandlers.OnDied;
                PLAYER.RoleChange += Eventhandlers.OnChangingRole;
                PLAYER.Escape += Eventhandlers.OnEscaping;
                PLAYER.DroppingItem += Eventhandlers.OnDropingItem;
                PLAYER.ItemUsing += Eventhandlers.OnMedicalUsing;
                Scps914.UpgradePlayer += Eventhandlers.OnUpgradePlayer;
                Scps914.Upgrade += Eventhandlers.OnUpgrade;
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
            PLAYER.Shooting -= Eventhandlers.OnShooting;
            PLAYER.TransmitPlayerData -= Eventhandlers.OnTransmitPlayerData;
            Qurre.Events.Round.WaitingForPlayers -= Eventhandlers.WaitingForPlayers;
            Qurre.Events.Round.Check -= Eventhandlers.OnRoundEnding;
            PLAYER.TeslaTrigger -= Eventhandlers.OnTriggeringTesla;
            Qurre.Events.Round.Start -= Eventhandlers.OnRoundStarted;
            SERVER.SendingConsole -= Eventhandlers.OnSendingConsoleCommand;
            PLAYER.InteractDoor -= Eventhandlers.OnInteractingDoor;
            PLAYER.InteractLift -= Eventhandlers.OnInteractingElevator;
            Qurre.Events.Round.Restart -= Eventhandlers.OnRestartingRound;
            PLAYER.Leave -= Eventhandlers.OnPlayerLeft;
            SCP106.Contain -= Eventhandlers.OnContaining;
            SCP096.Enrage -= Eventhandlers.OnEnraging;
            SCP096.AddTarget -= Eventhandlers.OnAddingTarget;
            Scps914.Activating -= Eventhandlers.OnActivating;
            PLAYER.InteractLocker -= Eventhandlers.OnInteractingLocker;
            PLAYER.PickupItem -= Eventhandlers.OnPickingUpItem;
            WARHEAD.Starting -= Eventhandlers.OnStarting;
            WARHEAD.Stopping -= Eventhandlers.OnStopping;
            WARHEAD.EnablePanel -= Eventhandlers.OnActivatingWarheadPanel;
            SCP106.PocketDimensionEnter -= Eventhandlers.OnEnteringPocketDimension;
            Qurre.Events.Map.NewBlood -= Eventhandlers.OnPlacingBlood;
            PLAYER.Cuff -= Eventhandlers.OnHandcuffing;
            PLAYER.Damage -= Eventhandlers.OnHurting;
            PLAYER.Dies -= Eventhandlers.OnDied;
            PLAYER.RoleChange -= Eventhandlers.OnChangingRole;
            PLAYER.Escape -= Eventhandlers.OnEscaping;
            PLAYER.DroppingItem -= Eventhandlers.OnDropingItem;
            PLAYER.ItemUsing -= Eventhandlers.OnMedicalUsing;
            Scps914.UpgradePlayer -= Eventhandlers.OnUpgradePlayer;
            Scps914.Upgrade -= Eventhandlers.OnUpgrade;
            Eventhandlers = null;
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
                        //else if (commandType == typeof(ClientCommandHandler))
                            //QueryProcessor.DotCommandHandler.RegisterCommand(command);
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
