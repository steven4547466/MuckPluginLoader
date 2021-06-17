using MuckPluginLoader.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MuckPluginLoader.API.Extensions
{
	public static class InventoryUIExtensions
	{
		public static int AddItemToInventoryNoUi(this InventoryUI inventory, InventoryItem item, MuckPlayer player)
		{
			InventoryItem inventoryItem = ScriptableObject.CreateInstance<InventoryItem>();
			inventoryItem.Copy(item, item.amount);
			InventoryCell inventoryCell = null;
			player.UiSfx?.PlayPickup();
			foreach (InventoryCell inventoryCell2 in inventory.cells)
			{
				if (inventoryCell2.currentItem == null)
				{
					if (!(inventoryCell != null))
					{
						inventoryCell = inventoryCell2;
					}
				}
				else if (inventoryCell2.currentItem.Compare(inventoryItem) && inventoryCell2.currentItem.stackable)
				{
					if (inventoryCell2.currentItem.amount + inventoryItem.amount <= inventoryCell2.currentItem.max)
					{
						inventoryCell2.currentItem.amount += inventoryItem.amount;
						inventoryCell2.UpdateCell();
						return 0;
					}
					int num = inventoryCell2.currentItem.max - inventoryCell2.currentItem.amount;
					inventoryCell2.currentItem.amount += num;
					inventoryItem.amount -= num;
					inventoryCell2.UpdateCell();
				}
			}
			if (inventoryCell)
			{
				inventoryCell.currentItem = inventoryItem;
				inventoryCell.UpdateCell();
				MonoBehaviour.print("added to available cell");
				return 0;
			}
			return inventoryItem.amount;
		}
	}
}
