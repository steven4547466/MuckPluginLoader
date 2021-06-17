using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MuckPluginLoader.API.Features;

namespace MuckPluginLoader.Events.Patches.General
{
	[HarmonyPatch(typeof(GameLoop), nameof(GameLoop.NewDay))]
	internal static class ChatMessageOnStart
	{
		private static void Postfix(int day)
		{
			if (day == 0)
				MuckPlayer.Get(LocalClient.instance.myId).SendMessage($"<color=#00ff00>You're playing on MuckPluginLoader {Assembly.GetExecutingAssembly().GetName().Version}</color>", "[MPL]");
		}
	}
}
