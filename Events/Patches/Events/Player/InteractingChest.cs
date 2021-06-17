using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MuckPluginLoader.Events.EventArgs;
using static HarmonyLib.AccessTools;
using UnityEngine;

namespace MuckPluginLoader.Events.Patches.Events.Player
{
	[HarmonyPatch(typeof(LootContainerInteract), nameof(LootContainerInteract.Interact))]
	internal static class InteractingChest
	{
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			var newInstructions = instructions.ToList();

			newInstructions.RemoveRange(0, 6);

			var ev = generator.DeclareLocal(typeof(InteractingChestEventArgs));

			var offset = newInstructions.FindIndex(ins => ins.opcode == OpCodes.Brtrue_S) + 2;

			var returnLabel = generator.DefineLabel();
			newInstructions[newInstructions.Count - 1].labels.Add(returnLabel);

			newInstructions.InsertRange(offset, new[]
			{
				new CodeInstruction(OpCodes.Ldsfld, Field(typeof(LocalClient), nameof(LocalClient.instance))).MoveLabelsFrom(newInstructions[offset]),
				new CodeInstruction(OpCodes.Ldfld, Field(typeof(LocalClient), nameof(LocalClient.myId))),
				new CodeInstruction(OpCodes.Call, Method(typeof(API.Features.MuckPlayer), nameof(API.Features.MuckPlayer.Get))),
				new CodeInstruction(OpCodes.Ldarg_0),
				new CodeInstruction(OpCodes.Ldsfld, Field(typeof(InventoryUI), nameof(InventoryUI.Instance))),
				new CodeInstruction(OpCodes.Callvirt, Method(typeof(InventoryUI), nameof(InventoryUI.GetMoney))),
				new CodeInstruction(OpCodes.Ldarg_0),
				new CodeInstruction(OpCodes.Ldfld, Field(typeof(LootContainerInteract), nameof(LootContainerInteract.price))),
				new CodeInstruction(OpCodes.Clt),
				new CodeInstruction(OpCodes.Ldc_I4_0),
				new CodeInstruction(OpCodes.Ceq),
				new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(InteractingChestEventArgs))[0]),
				new CodeInstruction(OpCodes.Stloc, ev.LocalIndex),
				new CodeInstruction(OpCodes.Ldloc, ev.LocalIndex),
				new CodeInstruction(OpCodes.Call, Method(typeof(Handlers.Player), nameof(Handlers.Player.OnInteractingChest))),
				new CodeInstruction(OpCodes.Ldarg_0),
				new CodeInstruction(OpCodes.Ldloc, ev.LocalIndex),
				new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(InteractingChestEventArgs), nameof(InteractingChestEventArgs.Price))),
				new CodeInstruction(OpCodes.Stfld, Field(typeof(LootContainerInteract), nameof(LootContainerInteract.price))),
				new CodeInstruction(OpCodes.Ldloc, ev.LocalIndex),
				new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(InteractingChestEventArgs), nameof(InteractingChestEventArgs.IsAllowed))),
				new CodeInstruction(OpCodes.Brfalse_S, returnLabel)
			});

			var interactedCompleteOffset = newInstructions.FindLastIndex(ins => ins.opcode == OpCodes.Call) + 1;

			newInstructions.InsertRange(interactedCompleteOffset, new[]
			{
				new CodeInstruction(OpCodes.Ldsfld, Field(typeof(LocalClient), nameof(LocalClient.instance))),
				new CodeInstruction(OpCodes.Ldfld, Field(typeof(LocalClient), nameof(LocalClient.myId))),
				new CodeInstruction(OpCodes.Call, Method(typeof(API.Features.MuckPlayer), nameof(API.Features.MuckPlayer.Get))),
				new CodeInstruction(OpCodes.Ldarg_0),
				new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(InteractedChestEventArgs))[0]),
				new CodeInstruction(OpCodes.Dup),
				new CodeInstruction(OpCodes.Call, Method(typeof(Handlers.Player), nameof(Handlers.Player.OnInteractedChest))),
				new CodeInstruction(OpCodes.Pop)
			});


			foreach (var instruction in newInstructions)
			{
				yield return instruction;
			}
		}
	}
}
