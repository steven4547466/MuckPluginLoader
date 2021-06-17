using MuckPluginLoader.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuckPluginLoader.Events.EventArgs
{
	public class InteractingChestEventArgs : System.EventArgs
	{

		public MuckPlayer Player { get; }

		public int Price 
		{
			get 
			{
				return Chest.price;
			}
			set
			{
				Chest.price = value;
			}
		}

		public LootContainerInteract Chest { get; private set; }

		public bool IsAllowed { get; set; }

		public InteractingChestEventArgs(MuckPlayer player, LootContainerInteract chest, bool isAllowed = true)
		{
			Player = player;
			Chest = chest;
			IsAllowed = IsAllowed;
		}


	}
}
