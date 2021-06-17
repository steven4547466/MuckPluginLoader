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
			Log.Debug("Players");
			new MuckPlayer(__instance);
		}
	}

}
