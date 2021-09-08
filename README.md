# [SCP-343](http://www.scp-wiki.net/scp-343)
# Important: This plugin for Qurre

## Install Instructions.
Put SCP343.dll under the release tab into %appdata%\Qurre\Plugins\ on Windows, or into ~/.config/Qurre/Plugins/ on Linux

# Config Options.
```yaml
#IsEnabled?
scp343_IsEnabled: true
scp343_canescape: false
scp343_alerttext: You are <color=red>SCP-343</color>. Check your client console on [~]
scp343_consoletext: You are <color=red>scp343</color>:\n\n1. You can open all doors;\n\n2. You can transform weapons to first and kit;\n\n 3. You have a god mode.\n\n4. You can teleport to player by sending console command .tp343 or drop coin\n\n5.In 1 metre away you , you can heal players by sending command .heal343 or dropping adrenaline\n6. In 1 meter away you, you can revive any dead player sending command .revive343 or dropping flashlight\n7. You can be invisible sending command .invis\nOr you can use items dropping instead of sending commands
scp343_alertbackd: You stopped being scp-343
scp343_alertheckerrortime: Time is left.
scp343_alertheckerrornot343: Wait, you are not scp-343
scp343_hecktime: 30
scp343_nuke_interact: true
scp343_spawnchance: 30
scp343_itemdroplist: 0,1,2,3,4,5,6,7,8,9,11,15,19,12,19,22,27,28,29,32,33
scp343_opendoortime: 30
scp343_itemstoconvert: 10,13,14,16,20,21,23,24,25,26,30,35
scp343_converteditems: 14
scp343_itemsatspawn: 35,33,15,32,13
scp343_lift_moving_speed: 6,5
scp343_canopenanydoor: true
scp343_alert: true
scp343_console: true
scp343_heck: true
scp343_heckerrordisable: .heck343 is disabled by config
scp343_itemconverttoggle: true
scp343_minplayers: 5
scp343_unitname: SCP-343
#If scp343 in range of the tesla
scp343_activating_tesla_in_range: true
scp343_invisible_for_173: false
scp343_turned_for_scp173_andscp096: true
scp343_show_timer_when_can_open_door: false
scp343_text_show_timer_when_can_open_door: In {343_time_open_door} seconds you can open door
#Can scp-343 interact with scp-914
scp343_interact_scp914: false
scp343_min_heal_players: 30
scp343_max_heal_players: 70
scp343_can_use_TranquilizerGun: true
scp343_itemscannotdrop: 35,33,15,32,13
scp343_notfoundplayer: Not found players!
scp343_teleport_to_player: You teleported to %player% playing as %role%
scp343_healplayer: You healed players health
scp343_cooldown: Please wait %seconds% seconds for healing another players
scp343_cannotrevive: You can not revive players
scp343_playerwhorevived: You was revived by SCP-343
scp343_revive_text: You revived %user%
scp343_youmustexit914: You must exit SCP-914
#How many SCP-343 can revive players?
scp343_max_revive_count: 3
scp343_can_visibled_while_speaking: true
#Cooldown after healing players
scp343_heal_cooldown: 120
scp343_end_cooldown: You can now heal another players!
```


| Command(s)                 | Value Type      | Description                              |
|   :---:                    |     :---:       |    :---:                                 |
| spawn343                   | PlayerID        | Spawn SCP-343 from PlayerID (Number next to name with M menu). Example = "spawn343 2" |

## Technical Description  

To be clear this isn't the correct wiki version SCP-343, its just a passive SCP inspired by my experiences of people being Tutorial running around messing with people.

Technically speaking hes a D-Class with godmode enabled or HP with the config option and spawns with the D-Class. After a X seconds period set by the server he can open every door in the game. Also to make sure he is passive every weapon he picks up or spawns with is converted to a MedKit or something the server config can change, healing players and reviving players. So people can know who he is, their rank is set to a red "SCP-343" and if they die or are set to a different class their rank name and color are reset to what it was orginally.
SCP-343 doesn't affect who wins.
