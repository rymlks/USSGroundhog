using System;
using UnityEngine;

namespace Inventory
{
    public class GatherableItem : MonoBehaviour
    {
        public String itemName;
        public bool persistsThroughDeath = false;
        public bool destroyOnCollect = false;

        public void Start()
        {
            this.GetComponent<Collider>().isTrigger = true;
        }

        bool isCollisionWithPlayer(Collider triggeredCollider)
        {
            return triggeredCollider.gameObject.CompareTag("Player");
        }

        void OnTriggerEnter(Collider triggeredCollider)
        {
            if (isCollisionWithPlayer(triggeredCollider))
            {
                GameManager.instance.getInventory().Gather(itemName, persistsThroughDeath);
                if (destroyOnCollect)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
