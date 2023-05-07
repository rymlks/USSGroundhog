using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        private HashSet<string> permanentItems = new HashSet<string>();
        private HashSet<string> transientItems = new HashSet<string>();
        
        public void Gather(string gatheredItemName, bool persistsThroughDeath = false)
        {
            if (persistsThroughDeath)
            {
                this.permanentItems.Add(gatheredItemName);
            }
            else
            {
                this.transientItems.Add(gatheredItemName);
            }
        }

        public bool IsItemPossessed(string itemName)
        {
            return this.permanentItems.Contains(itemName) || this.transientItems.Contains(itemName);
        }
    }
}