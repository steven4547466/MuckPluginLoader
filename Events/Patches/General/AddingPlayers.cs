using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Steamworks;
using UnityEngine;

namespace MuckPluginLoader.Events.Patches.General
{
	[HarmonyPatch(typeof(Player), MethodType.Constructor, new[] { typeof(int), typeof(string), typeof(Color), typeof(SteamId) })]
	internal static class PlayerConstructorPatch
	{
		static void Postfix(Player __instance)
		{
			new API.Features.MuckPlayer(__instance);
		}
	}

	[HarmonyPatch(typeof(Player), MethodType.Constructor, new[] { typeof(int), typeof(string), typeof(Color) })]
	internal static class PlayerConstructorPatch2
	{
		static void Postfix(Player __instance)
		{
			new API.Features.MuckPlayer(__instance);
		}
	}

}
