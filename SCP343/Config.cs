using System;
using System.Collections.Generic;
using System.Linq;
using Qurre.API.Addons;
using Qurre.API.Objects;

namespace SCP343
{
    public static class Config
    {
        public static bool IsEnabled { get; internal set; } = true;

        public static bool CanEscape { get; internal set; } = false;

        public static bool CanOpenAnyDoor { get; internal set; } = true;

        public static bool Alert { get; internal set; } = true;
        public static bool Console { get; internal set; } = true;

        public static bool Heck { get; internal set; } = true;

        public static int HeckTime { get; internal set; } = 30;

        public static bool Nuke_Interact { get; internal set; } = true;

        public static int OpenDoorTime { get; internal set; } = 30;

        public static bool ItemsConvertToggle { get; internal set; } = true;

        public static float SpawnChance { get; internal set; } = 30f;

       public static List<ItemType> ItemsDropList { get; internal set; } = new List<ItemType> { ItemType.KeycardJanitor, ItemType.KeycardScientist, ItemType.KeycardResearchCoordinator, ItemType.KeycardZoneManager, ItemType.KeycardGuard, ItemType.KeycardMTFCaptain, ItemType.KeycardContainmentEngineer, ItemType.KeycardMTFOperative, ItemType.KeycardMTFPrivate, ItemType.KeycardFacilityManager, ItemType.KeycardChaosInsurgency , ItemType.KeycardO5, ItemType.Flashlight, ItemType.Radio, ItemType.Ammo556x45, ItemType.Ammo44cal, ItemType.Ammo762x39, ItemType.Ammo9x19, ItemType.SCP268, ItemType.Adrenaline };

        public static List<ItemType> ItemsToConvert { get; internal set; } = new List<ItemType> { ItemType.GunCOM15, ItemType.Medkit, ItemType.MicroHID, ItemType.GunE11SR, ItemType.GunCrossvec, ItemType.GunFSP9, ItemType.GunLogicer, ItemType.GrenadeHE, ItemType.GrenadeFlash, ItemType.GunCOM18, ItemType.Coin };

        public static List<ItemType> ConvertedItems { get; internal set; } = new List<ItemType> { ItemType.Medkit };

        public static int MinPlayersWhenCanSpawn { get; internal set; } = 5;

        public static List<ItemType> ItemsAtSpawn { get; internal set; } = new List<ItemType> { ItemType.Coin, ItemType.Adrenaline, ItemType.SCP500, ItemType.SCP268, ItemType.GunCOM15, ItemType.SCP330 };
        
        public static bool Activating_Tesla_In_Range { get; internal set; } = true;

        public static bool Turned_For_Scp173_And_Scp096 { get; internal set; } = true;

        public static bool Invisible_For_173 { get; internal set; } = false;

        public static bool Show_Timer_When_Can_Open_Door { get; internal set; } = false;

        public static bool Interact_Scp914 { get; internal set; } = false;

        public static int Min_Heal_Players { get; internal set; } = 30;

        public static int Max_Heal_Players { get; internal set; } = 70;

        public static bool Can_Use_TranquilizerGun { get; internal set; } = true;

        public static List<ItemType> ItemsCanNotDrop { get; internal set; } = new List<ItemType> { ItemType.Coin, ItemType.Adrenaline, ItemType.SCP500, ItemType.SCP268, ItemType.GunCOM15};
        
        public static bool Can_Visibled_While_Speaking { get; internal set; } = true;

        public static int HealCooldown { get; internal set; } = 120;

        public static int ShootCooldown { get; internal set; } = 30;

        public static int Max_Revive_Count { get; internal set; } = 3;

        public static Translation Translation { get; internal set; } = new Translation();

        private static readonly JsonConfig jsonConfig = new JsonConfig("SCP_343");

        internal static void Reload()
        {
            IsEnabled = jsonConfig.SafeGetValue("IsEnabled", IsEnabled);
            CanEscape = jsonConfig.SafeGetValue("CanEscape", CanEscape);
            CanOpenAnyDoor = jsonConfig.SafeGetValue("CanOpenAnyDoor", CanOpenAnyDoor);
            Alert = jsonConfig.SafeGetValue("Alert", Alert);
            Console = jsonConfig.SafeGetValue("Console", Console);
            Heck = jsonConfig.SafeGetValue("Heck", Heck);
            HeckTime = jsonConfig.SafeGetValue("HeckTime", HeckTime);
            Nuke_Interact = jsonConfig.SafeGetValue("Nuke_Interact", Nuke_Interact);
            OpenDoorTime = jsonConfig.SafeGetValue("OpenDoorTime", OpenDoorTime);
            ItemsConvertToggle = jsonConfig.SafeGetValue("ItemsConvertToggle", ItemsConvertToggle);
            SpawnChance = jsonConfig.SafeGetValue("SpawnChance", SpawnChance);
            string[] items = jsonConfig.SafeGetValue("ItemDropList", ItemsDropList.Select(ii => ((int)ii).ToString()).ToArray());
            ItemsDropList = items.Select(dd => (ItemType)Enum.Parse(typeof(ItemType), dd)).ToList();
            items = jsonConfig.SafeGetValue("ItemsToConvert", ItemsToConvert.Select(ii => ((int)ii).ToString()).ToArray());
            ItemsToConvert = items.Select(dd => (ItemType)Enum.Parse(typeof(ItemType), dd)).ToList();
            items = jsonConfig.SafeGetValue("ConvertedItems", ConvertedItems.Select(ii => ((int)ii).ToString()).ToArray());
            ConvertedItems = items.Select(dd => (ItemType)Enum.Parse(typeof(ItemType), dd)).ToList();
            MinPlayersWhenCanSpawn = jsonConfig.SafeGetValue("MinPlayersWhenCanSpawn", MinPlayersWhenCanSpawn);
            items = jsonConfig.SafeGetValue("ItemsAtSpawn", ItemsAtSpawn.Select(ii => ((int)ii).ToString()).ToArray());
            ItemsAtSpawn = items.Select(dd=> (ItemType)Enum.Parse(typeof(ItemType), dd)).ToList();
            Activating_Tesla_In_Range = jsonConfig.SafeGetValue("Activating_Tesla_In_Range", Activating_Tesla_In_Range);
            Translation = jsonConfig.SafeGetValue("Translation", new Translation());
            JsonConfig.UpdateFile();
        }
    }
}
