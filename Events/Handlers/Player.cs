using MuckPluginLoader.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MuckPluginLoader.Events.Events;

namespace MuckPluginLoader.Events.Handlers
{
	public static class Player
	{
		public static event CustomEventHandler<InteractingChestEventArgs> InteractingChest;

		public static event CustomEventHandler<InteractedChestEventArgs> InteractedChest;

		public static void OnInteractingChest(InteractingChestEventArgs ev) => InteractingChest.InvokeSafely(ev);

		public static void OnInteractedChest(InteractedChestEventArgs ev) => InteractedChest.InvokeSafely(ev);
	}
}
