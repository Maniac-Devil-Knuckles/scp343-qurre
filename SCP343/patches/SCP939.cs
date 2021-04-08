
using Qurre.API;
using HarmonyLib;
using UnityEngine;

namespace SCP343.Patches
{
	[HarmonyPatch(typeof(Scp939PlayerScript), nameof(Scp939PlayerScript.CallCmdShoot))]
	class Scp939Attack
	{
		public static void Postfix(Scp939PlayerScript __instance, GameObject target)
		{
			Player player = Player.Get(target);
			if (player.IsSCP343())
			{
				player.DisableEffect<CustomPlayerEffects.Amnesia>();
			}
		}
	}
}