using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MuckPluginLoader.API.Extensions;

namespace MuckPluginLoader.API.Features
{
	public class MuckPlayer
	{
		public static List<MuckPlayer> List { get; } = new List<MuckPlayer>();

		public string Username => PlayerManager.username;
		
		public int Id => PlayerManager.id;

		public GameObject GameObject => PlayerManager.gameObject;

		public PlayerManager PlayerManager { get; private set; }


		private ClientSend clientSend;
		public ClientSend ClientSend 
		{ 
			get
			{
				if (clientSend != null) return clientSend;
				else
				{
					clientSend = GameObject.GetComponent<ClientSend>();
					return clientSend;
				}
			}
		}

		private PlayerStatus playerStatus;
		public PlayerStatus PlayerStatus
		{
			get
			{
				if (playerStatus != null) return playerStatus;
				else
				{
					playerStatus = GameObject.GetComponent<PlayerStatus>();
					return playerStatus;
				}
			}
		}

		public InventoryUI Inventory { 
			get
			{
				if (!IsLocalPlayer) return null;
				return InventoryUI.Instance;
			} 
		}

		public PowerupInventory PowerupInventory
		{
			get
			{
				if (!IsLocalPlayer) return null;
				return PowerupInventory.Instance;
			}
		}

		public PowerupUI PowerupUI
		{
			get
			{
				if (!IsLocalPlayer) return null;
				return PowerupUI.Instance;
			}
		}

		public UiEvents UiEvents
		{
			get
			{
				if (!IsLocalPlayer) return null;
				return UiEvents.Instance;
			}
		}

		public UiSfx UiSfx
		{
			get
			{
				if (!IsLocalPlayer) return null;
				return UiSfx.Instance;
			}
		}

		public bool IsLocalPlayer => LocalClient.instance.myId == Id;

		public ChatBox ChatBox => ChatBox.Instance;

		public MuckPlayer(PlayerManager player)
		{
			PlayerManager = player;
			List.Add(this);
		}

		public void SendMessage(string message, string username = "")
		{
			ChatBox.AppendMessage(-1, message, username);
		}

		public void Damage(int damage, bool ignoreProtection = false)
		{
			PlayerStatus.DealDamage(damage, ignoreProtection);
		}

		public bool GiveItem(string name, int amount = 1, bool show = true)
		{
			if (!IsLocalPlayer) return false;
			foreach (int id in ItemManager.Instance.allItems.Keys)
			{
				if (ItemManager.Instance.allItems[id].name == name)
				{
					InventoryItem item = ItemManager.Instance.allItems[id];
					item.amount = amount;
					if (show) Inventory?.AddItemToInventory(item);
					else Inventory?.AddItemToInventoryNoUi(item, this);
					return true;
				}
			}
			return false;
		}

		public bool GiveItem(int id, int amount = 1, bool show = true)
		{
			if (!IsLocalPlayer) return false;
			if (ItemManager.Instance.allItems.TryGetValue(id, out var tempItem))
			{
				InventoryItem item = tempItem;
				item.amount = amount;
				if (show) Inventory?.AddItemToInventory(item);
				else Inventory?.AddItemToInventoryNoUi(item, this);
				return true;
			}
			return false;
		}

		public bool GivePowerup(string name, int amount = 1, bool show = true)
		{
			if (!IsLocalPlayer) return false;
			
			foreach (int id in ItemManager.Instance.allPowerups.Keys)
			{
				if (ItemManager.Instance.allPowerups[id].name == name)
				{
					if (PowerupInventory != null)
					{
						for (int i = 0; i < amount; i++)
						{
							PowerupInventory.powerups[id]++;
							if (show) UiEvents.AddPowerup(ItemManager.Instance.allPowerups[id]);
							PlayerStatus.UpdateStats();
							PowerupUI.AddPowerup(id);
						}
					}
					return true;
				}
			}
			return false;
		}

		public bool GivePowerup(int id, int amount = 1, bool show = true)
		{
			if (!IsLocalPlayer) return false;

			if (ItemManager.Instance.allPowerups.TryGetValue(id, out var powerup))
			{
				if (PowerupInventory != null)
				{
					for (int i = 0; i < amount; i++)
					{
						PowerupInventory.powerups[id]++;
						if (show) UiEvents.AddPowerup(ItemManager.Instance.allPowerups[id]);
						PlayerStatus.UpdateStats();
						PowerupUI.AddPowerup(id);
					}
				}
				return true;
			}
			return false;
		}


		public static MuckPlayer Get(int id)
		{
			return List.FirstOrDefault(p => p.Id == id);
		}
	}
}
