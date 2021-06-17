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

		public static event CustomEventHandler<AttemptingReviveEventArgs> AttemptingRevive;

		public static event CustomEventHandler<SendingMessageEventArgs> SendingMessage;

		public static event CustomEventHandler<SendingCommandEventArgs> SendingCommand;

		public static void OnInteractingChest(InteractingChestEventArgs ev) => InteractingChest.InvokeSafely(ev);

		public static void OnInteractedChest(InteractedChestEventArgs ev) => InteractedChest.InvokeSafely(ev);

		public static void OnAttemptingRevive(AttemptingReviveEventArgs ev) => AttemptingRevive.InvokeSafely(ev);

		public static void OnSendingMessage(SendingMessageEventArgs ev) => SendingMessage.InvokeSafely(ev);

		public static void OnSendingCommand(SendingCommandEventArgs ev) => SendingCommand.InvokeSafely(ev);
	}
}
