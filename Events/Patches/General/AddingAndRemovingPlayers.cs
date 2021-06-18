using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MuckPluginLoader.API.Features;
using Steamworks;
using UnityEngine;

namespace MuckPluginLoader.Events.Patches.General
{
	[HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.Awake))]
	internal static class PlayerManagerPatch
	{
		static void Postfix(PlayerManager __instance)
		{
			Log.Debug("Add player");
			new MuckPlayer(__instance);
		}
	}

	[HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy), new[] { typeof(UnityEngine.Object) })]
	internal static class PlayerManagerDestroyPatch
	{
		static void Prefix(UnityEngine.Object obj)
		{
			if (obj is GameObject go)
			{
				var playerManager = go.GetComponent<PlayerManager>();
				if (playerManager != null)
				{
					MuckPlayer.List.Remove(MuckPlayer.Get(playerManager.id));
				}
			}
		}
	}

}
