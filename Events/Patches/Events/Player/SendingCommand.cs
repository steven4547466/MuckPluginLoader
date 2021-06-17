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
	[HarmonyPatch(typeof(ChatBox), nameof(ChatBox.ChatCommand))]
	internal static class SendingCommand
	{
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			var newInstructions = instructions.ToList();

			var retLabel = generator.DefineLabel();

			newInstructions.InsertRange(newInstructions.Count - 1, new[] 
			{
				new CodeInstruction(OpCodes.Ldarg_0).WithLabels(retLabel),
				new CodeInstruction(OpCodes.Call, Method(typeof(ChatBox), nameof(ChatBox.ClearMessage)))
			});

			var offset = newInstructions.FindIndex(ins => ins.opcode == OpCodes.Stloc_0) + 1;

			var argArray = generator.DeclareLocal(typeof(string[]));

			newInstructions.InsertRange(offset, new[] 
			{
				new CodeInstruction(OpCodes.Ldloc_0),
				new CodeInstruction(OpCodes.Ldc_I4_1),
				new CodeInstruction(OpCodes.Newarr, typeof(char)),
				new CodeInstruction(OpCodes.Dup),
				new CodeInstruction(OpCodes.Ldc_I4_0),
				new CodeInstruction(OpCodes.Ldc_I4_S, 0x20),
				new CodeInstruction(OpCodes.Stelem_I2),
				new CodeInstruction(OpCodes.Callvirt, Method(typeof(string), nameof(string.Split))),
				new CodeInstruction(OpCodes.Stloc_S, argArray.LocalIndex),
				new CodeInstruction(OpCodes.Ldsfld, Field(typeof(LocalClient), nameof(LocalClient.instance))).MoveLabelsFrom(newInstructions[offset]),
				new CodeInstruction(OpCodes.Ldfld, Field(typeof(LocalClient), nameof(LocalClient.myId))),
				new CodeInstruction(OpCodes.Call, Method(typeof(API.Features.MuckPlayer), nameof(API.Features.MuckPlayer.Get))),
				new CodeInstruction(OpCodes.Ldloc_S, argArray.LocalIndex),
				new CodeInstruction(OpCodes.Ldc_I4_1),
				new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(SendingCommandEventArgs))[0]),
				new CodeInstruction(OpCodes.Dup),
				new CodeInstruction(OpCodes.Call, Method(typeof(Handlers.Player), nameof(Handlers.Player.OnSendingCommand))),
				new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(SendingCommandEventArgs), nameof(SendingCommandEventArgs.IsAllowed))),
				new CodeInstruction(OpCodes.Brfalse_S, retLabel)
			});

			foreach (var instruction in newInstructions)
			{
				yield return instruction;
			}
		}
	}
}
