using HarmonyLib;
using MuckPluginLoader.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static HarmonyLib.AccessTools;

namespace MuckPluginLoader.Events.Patches.Events.Player
{
	[HarmonyPatch(typeof(ClientSend), nameof(ClientSend.RevivePlayer))]
	internal static class AttemptingRevive
	{
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			var newInstructions = instructions.ToList();

			var retLabel = newInstructions[newInstructions.Count - 1].labels[0];

			newInstructions.InsertRange(0, new[] 
			{
				new CodeInstruction(OpCodes.Ldsfld, Field(typeof(LocalClient), nameof(LocalClient.instance))),
				new CodeInstruction(OpCodes.Ldfld, Field(typeof(LocalClient), nameof(LocalClient.myId))),
				new CodeInstruction(OpCodes.Call, Method(typeof(API.Features.MuckPlayer), nameof(API.Features.MuckPlayer.Get))),
				new CodeInstruction(OpCodes.Ldarg_0),
				new CodeInstruction(OpCodes.Call, Method(typeof(API.Features.MuckPlayer), nameof(API.Features.MuckPlayer.Get))),
				new CodeInstruction(OpCodes.Ldc_I4_1),
				new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(AttemptingReviveEventArgs))[0]),
				new CodeInstruction(OpCodes.Dup),
				new CodeInstruction(OpCodes.Call, Method(typeof(Handlers.Player), nameof(Handlers.Player.OnAttemptingRevive))),
				new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(AttemptingReviveEventArgs), nameof(AttemptingReviveEventArgs.IsAllowed))),
				new CodeInstruction(OpCodes.Brfalse_S, retLabel)
			});

			foreach (var instruction in newInstructions)
			{
				yield return instruction;
			}
		}
	}
}
