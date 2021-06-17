using MuckPluginLoader.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuckPluginLoader.Events.EventArgs
{
	public class InteractedChestEventArgs : System.EventArgs
	{

		public MuckPlayer Player { get; }

		public int Price 
		{ 
			get 
			{
				return Chest.price;
			} 
		}

		public LootContainerInteract Chest { get; private set; }

		public InteractedChestEventArgs(MuckPlayer player, LootContainerInteract chest)
		{
			Player = player;
			Chest = chest;
		}


	}
}
