# [SCP-343](http://www.scp-wiki.net/scp-343)
# Important: This plugin for Qurre

## Install Instructions.
Put SCP343.dll under the release tab into %appdata%\Qurre\Plugins\ on Windows, or into ~/.config/Qurre/Plugins/ on Linux

# Config Options.
| Config Option              | Value Type      | Default Value | Description |
|   :---:                    |     :---:       |    :---:      |    :---:    |
| scp343_IsEnabled                  | Boolean         | true         |  Will loading this plugin on the server or not |
| scp343_spawnchance         | Float           | 10            | Percent chance for SPC-343 to spawn at the start of the round. |
| scp343_opendoortime        | Integer         | 60            | How many seconds after roundstart till SCP-343 can open any door in the game (Like door bypass).               |
| scp343_nuke_interact       | Boolean         | false         | Should SCP-343 beable to interact with the nuke?               |
| scp343_itemconverttoggle   | Boolean         | false         | Should SPC-343 convert items?                                  |
| scp343_itemdroplist        | Integer List    | 0,1,2,3,4,5,6,7,8,9,10,11,14,17,19,22,27,28,29 | What items SCP-343 drops instead of picking up.|
| scp343_itemstoconvert      | Integer List    | 13,16,20,21,23,24,25,26,30 | What items SCP-343 converts. |
| scp343_converteditems      | Integer List    | 14            | What a item should be converted to.       |
| scp343_console             | Boolean         | true          | When 343 spawns should that person be given information about 343 in console      |
| scp343_consoletext         | String          | You are <color=red>scp343</color>:\n\n1. You can open all doors;\n\n2. You can transform weapons to first and kit;\n\n 3. You have a god mode.\n\n4. You can teleport to player by sending console command .tp343 or drop ammo\n\n5.In 1 metre away you , you can heal players by sending command .heal343 or dropping adrenaline\n6. In 1 meter away you, you can revive any dead player sending command .revive343 or dropping flashlight          | What 343 is shown if scp343_console is true.       |
| scp343_heck                | Boolean         | True          | Should players be allowed to use the .heck343 client command to respawn themselves as d-class within scp343_hecktime seconds of round start.     |
| scp343_hecktime            | Integer         | 30            | How long people should beable to respawn themselves as d-class.     |
| scp343_alert               | Boolean         | true          | When 343 spawns should that person will be broadcast    |
| scp343_alerttext           | String          |You are <color=red>SCP-343</color>. Check your client console on [~]  | What 343 is shown if scp343_alert is true.       |
| scp343_alertbackd          | String          | You stopped being scp-343 | What 343 is shown if scp343 will back to usual class d|
| scp343_alertheckerrortime  | String          | Time is left.| What 343 is shown if scp343 will back to usual class d and time is left|
| scp343_alertheckerrornot343| String          | Wait, you are not SCP-343 |  What 343 is shown if not scp343 trying to back to usual class d |
| scp343_minplayers                 | Integer         | 3              | Minimum players for spawning scp343 |
| scp343_itemsatspawn        | Integer List    | 35, 33, 15, 32, 13             | What give scp-343 on spawn |
| scp343_lift_moving_speed          | float           | 6.5            | Moving Speed lift for all players |
| scp343_unitname             | String    | SCP-343             | UnitName for SCP-343 |


| Command(s)                 | Value Type      | Description                              |
|   :---:                    |     :---:       |    :---:                                 |
| spawn343                   | PlayerID        | Spawn SCP-343 from PlayerID (Number next to name with M menu). Example = "spawn343 2" |

## Technical Description  

To be clear this isn't the correct wiki version SCP-343, its just a passive SCP inspired by my experiences of people being Tutorial running around messing with people.

Technically speaking hes a D-Class with godmode enabled or HP with the config option and spawns with the D-Class. After a X seconds period set by the server he can open every door in the game. Also to make sure he is passive every weapon he picks up or spawns with is converted to a MedKit or something the server config can change, healing players and reviving players. So people can know who he is, their rank is set to a red "SCP-343" and if they die or are set to a different class their rank name and color are reset to what it was orginally.
SCP-343 doesn't affect who wins.
