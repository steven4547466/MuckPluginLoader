using MuckPluginLoader.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuckPluginLoader.Events.EventArgs
{
	public class SendingMessageEventArgs : System.EventArgs
	{

		public MuckPlayer Player { get; }

		public string Message { get; set; }

		public bool IsAllowed { get; set; }

		public SendingMessageEventArgs(MuckPlayer player, string message, bool isAllowed = true)
		{
			Player = player;
			Message = message;
			IsAllowed = isAllowed;
		}
	}
}
