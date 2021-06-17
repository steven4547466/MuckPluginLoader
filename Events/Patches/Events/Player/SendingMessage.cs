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
	[HarmonyPatch(typeof(ChatBox), nameof(ChatBox.SendMessage))]
	internal static class SendingMessage
	{
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			var newInstructions = instructions.ToList();

			var offset = newInstructions.FindIndex(ins => ins.opcode == OpCodes.Ret) + 1;

			var ev = generator.DeclareLocal(typeof(SendingMessageEventArgs));

			var retLabel = generator.DefineLabel();

			newInstructions[newInstructions.Count - 3].labels.Add(retLabel);

			newInstructions.InsertRange(offset, new[] 
			{
				new CodeInstruction(OpCodes.Ldsfld, Field(typeof(LocalClient), nameof(LocalClient.instance))).MoveLabelsFrom(newInstructions[offset]),
				new CodeInstruction(OpCodes.Ldfld, Field(typeof(LocalClient), nameof(LocalClient.myId))),
				new CodeInstruction(OpCodes.Call, Method(typeof(API.Features.MuckPlayer), nameof(API.Features.MuckPlayer.Get))),
				new CodeInstruction(OpCodes.Ldarg_1),
				new CodeInstruction(OpCodes.Ldc_I4_1),
				new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(SendingMessageEventArgs))[0]),
				new CodeInstruction(OpCodes.Stloc, ev.LocalIndex),
				new CodeInstruction(OpCodes.Ldloc, ev.LocalIndex),
				new CodeInstruction(OpCodes.Call, Method(typeof(Handlers.Player), nameof(Handlers.Player.OnSendingMessage))),
				new CodeInstruction(OpCodes.Ldloc, ev.LocalIndex),
				new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(SendingMessageEventArgs), nameof(SendingMessageEventArgs.IsAllowed))),
				new CodeInstruction(OpCodes.Brfalse_S, retLabel),
				new CodeInstruction(OpCodes.Ldloc, ev.LocalIndex),
				new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(SendingMessageEventArgs), nameof(SendingMessageEventArgs.Message))),
				new CodeInstruction(OpCodes.Starg_S, 1)
			});

			foreach (var instruction in newInstructions)
			{
				yield return instruction;
			}
		}
	}
}
