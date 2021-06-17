using MuckPluginLoader.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuckPluginLoader.Events.EventArgs
{
	public class SendingCommandEventArgs : System.EventArgs
	{

		public MuckPlayer Player { get; }

		public string Command { get; }

		public List<string> Arguments { get; }

		public bool IsAllowed { get; set; }

		public SendingCommandEventArgs(MuckPlayer player, string[] arguments, bool isAllowed = true)
		{
			Player = player;
			Command = arguments[0];
			Arguments = arguments.Skip(1).ToList();
			IsAllowed = isAllowed;
		}
	}
}
