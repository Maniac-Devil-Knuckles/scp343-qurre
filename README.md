# [SCP-343](http://www.scp-wiki.net/scp-343)
# Important: This plugin for Qurre

Download Qurre - https://github.com/Qurre-sl/Qurre

## Install Instructions.
Put SCP343.dll under the release tab into %appdata%\Qurre\Plugins\ on Windows, or into ~/.config/Qurre/Plugins/ on Linux

Config in %appdata%\Qurre\Configs\Custom\SCP343-7777.yaml on windows or in ~/.config/Qurre/Configs/Custom/SCP343-7777.yaml on Linux

# Config Options.
```yaml
"SCP_343": {
    "IsEnabled": true,
    "CanEscape": false,
    "CanOpenAnyDoor": true,
    "Alert": true,
    "Console": true,
    "Heck": true,
    "HeckTime": 30,
    "Nuke_Interact": true,
    "OpenDoorTime": 30,
    "ItemsConvertToggle": true,
    "SpawnChance": 30,
    "ItemDropList": [
      "0",
      "1",
      "2",
      "3",
      "4",
      "8",
      "6",
      "7",
      "5",
      "9",
      "10",
      "11",
      "15",
      "12",
      "22",
      "27",
      "28",
      "29",
      "32",
      "33"
    ],
    "ItemsToConvert": [
      "13",
      "14",
      "16",
      "20",
      "21",
      "23",
      "24",
      "25",
      "26",
      "30",
      "35"
    ],
    "ConvertedItems": [
      "14"
    ],
    "MinPlayersWhenCanSpawn": 5,
    "ItemsAtSpawn": [
      "35",
      "33",
      "17",
      "32",
      "13",
      "42"
    ],
    "Activating_Tesla_In_Range": true,
    "Translation": {
      "AlertText": "You are <color=red>SCP-343</color>. Check your client console on [~]",
      "ConsoleText": "You are <color=red>scp343</color>:\n\n1. You can open all doors;\n\n2. You can transform weapons to first and kit;\n\n 3. You have a god mode.\n\n4. You can teleport to player by sending console command .tp343 or drop coin\n\n5.In 1 metre away you , you can heal players by sending command .heal343 or dropping adrenaline\n6. In 1 meter away you, you can revive any dead player sending command .revive343 or dropping flashlight\n7. You can be invisible sending command .invis\nOr you can use items dropping instead of sending commands. If you drop scp-330 and looking at human then will gift random item",
      "AlertBackTo_DClass": "You stopped being scp-343",
      "AlertHeckErrorTime": "Time is left.",
      "AlertHeckError_IsNot343": "Wait, you are not scp-343",
      "HeckErrorDisable": ".heck343 is disabled by config",
      "Text_Show_Timer_When_Can_Open_Door": "In {343_time_open_door} seconds you can open door",
      "NotFoundPlayer": "Not found players!",
      "Teleport_To_Player": "You teleported to %player% playing as %role%",
      "HealPlayer": "You healed players health",
      "CoolDown": "Please wait %seconds% seconds for healing another players",
      "CanNotRevive": "You can not revive players",
      "PlayerWhoRevived": "You was revived by SCP-343",
      "Revive_Text": "You revived %user%",
      "YouMustExit914": "You must exit SCP-914",
      "YouWereTranq": "You were shooted by SCP-343 using TranquilizerGun",
      "Is_Invisible_True": "You are now is invisible for all",
      "Is_Invisible_False": "You are not is invisible for all",
      "End_Cooldown": "You can now heal another players!",
      "ShootCoolDownText": "Please wait %seconds% seconds before shooting"
    }
  }
```


| Command(s)                 | Value Type      | Description                              |
|   :---:                    |     :---:       |    :---:                                 |
| spawn343                   | PlayerID        | Spawn SCP-343 from PlayerID (Number next to name with M menu). Example = "spawn343 2" |

## Technical Description  

To be clear this isn't the correct wiki version SCP-343, its just a passive SCP inspired by my experiences of people being Tutorial running around messing with people.

Technically speaking hes a D-Class with godmode enabled or HP with the config option and spawns with the D-Class. After a X seconds period set by the server he can open every door in the game. Also to make sure he is passive every weapon he picks up or spawns with is converted to a MedKit or something the server config can change, healing players and reviving players. So people can know who he is, their rank is set to a red "SCP-343" and if they die or are set to a different class their rank name and color are reset to what it was orginally.
SCP-343 doesn't affect who wins.
