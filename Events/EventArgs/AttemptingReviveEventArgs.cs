using MuckPluginLoader.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuckPluginLoader.Events.EventArgs
{
	public class AttemptingReviveEventArgs : System.EventArgs
	{
		public MuckPlayer Player { get; }

		public MuckPlayer Target { get; }

		public bool IsAllowed { get; set; }

		public AttemptingReviveEventArgs(MuckPlayer player, MuckPlayer target, bool isAllowed = true)
		{
			Player = player;
			Target = target;
			IsAllowed = isAllowed;
		}
	}
}
