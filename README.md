# [SCP-343](http://www.scp-wiki.net/scp-343)
# Important: This plugin for Qurre

## Install Instructions.
Put SCP343.dll under the release tab into %appdata%\Qurre\Plugins\ on Windows, or into ~/.config/Qurre/Plugins/ on Linux

Config in %appdata%\Qurre\Configs\Custom\SCP343-7777.yaml on windows or in ~/.config/Qurre/Configs/Custom/SCP343-7777.yaml on Linux

# Config Options.
```yaml
Name: SCP343
IsEnabled: true
canescape: false
canopenanydoor: true
alert: true
console: true
heck: true
hecktime: 30
nuke_interact: true
opendoortime: 30
itemconverttoggle: true
spawnchance: 30
itemdroplist:
- KeycardJanitor
- KeycardScientist
- KeycardResearchCoordinator
- KeycardZoneManager
- KeycardGuard
- KeycardNTFOfficer
- KeycardContainmentEngineer
- KeycardNTFLieutenant
- KeycardNTFCommander
- KeycardFacilityManager
- KeycardChaosInsurgency
- KeycardO5
- Flashlight
- Radio
- Ammo556x45
- Ammo44cal
- Ammo762x39
- Ammo9x19
- SCP268
- Adrenaline
itemstoconvert:
- GunCOM15
- Medkit
- MicroHID
- GunE11SR
- GunCrossvec
- GunFSP9
- GunLogicer
- GrenadeHE
- GrenadeFlash
- GunCOM18
- Coin
converteditems:
- Medkit
minplayers: 5
itemsatspawn:
- Coin
- Adrenaline
- Flashlight
- SCP268
- GunCOM15
- SCP330
lift_moving_speed: 6.5
activating_tesla_in_range: true
turned_for_scp173_andscp096: true
invisible_for_173: false
show_timer_when_can_open_door: false
interact_scp914: false
min_heal_players: 30
max_heal_players: 70
can_use_TranquilizerGun: true
itemscannotdrop:
- Coin
- Adrenaline
- Flashlight
- SCP268
- GunCOM15
- SCP330
can_visibled_while_speaking: true
HealCooldown: 120
shootcooldown: 30
max_revive_count: 3
Translation:
  alerttext: You are <color=red>SCP-343</color>. Check your client console on [~]
  consoletext: >-
    You are <color=red>scp343</color>:


    1. You can open all doors;


    2. You can transform weapons to first and kit;

     3. You have a god mode.

    4. You can teleport to player by sending console command .tp343 or drop coin


    5.In 1 metre away you , you can heal players by sending command .heal343 or dropping adrenaline

    6. In 1 meter away you, you can revive any dead player sending command .revive343 or dropping flashlight

    7. You can be invisible sending command .invis

    Or you can use items dropping instead of sending commands. If you drop scp-330 and looking at human then will gift random item
  alertbackd: You stopped being scp-343
  alertheckerrortime: Time is left.
  alertheckerrornot343: Wait, you are not scp-343
  heckerrordisable: .heck343 is disabled by config
  unitname: SCP-343
  text_show_timer_when_can_open_door: In {343_time_open_door} seconds you can open door
  notfoundplayer: Not found players!
  teleport_to_player: You teleported to %player% playing as %role%
  healplayer: You healed players health
  cooldown: Please wait %seconds% seconds for healing another players
  cannotrevive: You can not revive players
  playerwhorevived: You was revived by SCP-343
  revive_text: You revived %user%
  youmustexit914: You must exit SCP-914
  youweretranq: You were shooted by SCP-343 using TranquilizerGun
  is_invisible_true: You are now is invisible for all
  is_invisible_false: You are not is invisible for all
  end_cooldown: You can now heal another players!
  shootcooldowntext: Please wait %seconds% seconds before shooting
  class_d_unit: Class D
```


| Command(s)                 | Value Type      | Description                              |
|   :---:                    |     :---:       |    :---:                                 |
| spawn343                   | PlayerID        | Spawn SCP-343 from PlayerID (Number next to name with M menu). Example = "spawn343 2" |

## Technical Description  

To be clear this isn't the correct wiki version SCP-343, its just a passive SCP inspired by my experiences of people being Tutorial running around messing with people.

Technically speaking hes a D-Class with godmode enabled or HP with the config option and spawns with the D-Class. After a X seconds period set by the server he can open every door in the game. Also to make sure he is passive every weapon he picks up or spawns with is converted to a MedKit or something the server config can change, healing players and reviving players. So people can know who he is, their rank is set to a red "SCP-343" and if they die or are set to a different class their rank name and color are reset to what it was orginally.
SCP-343 doesn't affect who wins.
