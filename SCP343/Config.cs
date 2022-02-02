using System.Collections.Generic;
using Qurre.API.Addons;

namespace SCP343
{
    public class Cfg : IConfig
    {
        public string Name { get; set; } = "SCP343";

        public bool IsEnabled { get; internal set; } = true;

        public bool canescape { get; internal set; } = false;

        public bool canopenanydoor { get; internal set; } = true;

        public bool alert { get; internal set; } = true;
        public bool console { get; internal set; } = true;

        public bool heck { get; internal set; } = true;

        public int hecktime { get; internal set; } = 30;

        public bool nuke_interact { get; internal set; } = true;

        public int opendoortime { get; internal set; } = 30;

        public bool itemconverttoggle { get; internal set; } = true;

        public float spawnchance { get; internal set; } = 30f;

        public List<ItemType> itemdroplist { get; internal set; } = new List<ItemType> { ItemType.KeycardJanitor, ItemType.KeycardScientist, ItemType.KeycardResearchCoordinator, ItemType.KeycardZoneManager, ItemType.KeycardGuard, ItemType.KeycardNTFOfficer, ItemType.KeycardContainmentEngineer, ItemType.KeycardNTFLieutenant, ItemType.KeycardNTFCommander, ItemType.KeycardFacilityManager, ItemType.KeycardChaosInsurgency , ItemType.KeycardO5, ItemType.Flashlight, ItemType.Radio, ItemType.Ammo556x45, ItemType.Ammo44cal, ItemType.Ammo762x39, ItemType.Ammo9x19, ItemType.SCP268, ItemType.Adrenaline };

        public List<ItemType> itemstoconvert { get; internal set; } = new List<ItemType> { ItemType.GunCOM15, ItemType.Medkit, ItemType.MicroHID, ItemType.GunE11SR, ItemType.GunCrossvec, ItemType.GunFSP9, ItemType.GunLogicer, ItemType.GrenadeHE, ItemType.GrenadeFlash, ItemType.GunCOM18, ItemType.Coin };

        public List<ItemType> converteditems { get; internal set; } = new List<ItemType> { ItemType.Medkit };

        public int minplayers { get; internal set; } = 5;

        public List<ItemType> itemsatspawn { get; internal set; } = new List<ItemType> { ItemType.Coin, ItemType.Adrenaline, ItemType.Flashlight, ItemType.SCP268, ItemType.GunCOM15, ItemType.SCP330 };

        public float lift_moving_speed { get; internal set; } = 6.5f;
        
        public bool activating_tesla_in_range { get; internal set; } = true;

        public bool turned_for_scp173_andscp096 { get; internal set; } = true;

        public bool invisible_for_173 { get; internal set; } = false;

        public bool show_timer_when_can_open_door { get; internal set; } = false;

        public bool interact_scp914 { get; internal set; } = false;

        public int min_heal_players { get; internal set; } = 30;

        public int max_heal_players { get; internal set; } = 70;

        public bool can_use_TranquilizerGun { get; internal set; } = true;

        public List<ItemType> itemscannotdrop { get; internal set; } = new List<ItemType> { ItemType.Coin, ItemType.Adrenaline, ItemType.Flashlight, ItemType.SCP268, ItemType.GunCOM15, ItemType.SCP330 };
        
        public bool can_visibled_while_speaking { get; internal set; } = true;

        public int HealCooldown { get; internal set; } = 120;

        public int shootcooldown { get; internal set; } = 30;

        public int max_revive_count { get; internal set; } = 3;

        public Translation Translation { get; internal set; } = new Translation();
    }
}
