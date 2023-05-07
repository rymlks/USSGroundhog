using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Triggers
{
    public class TriggerOnInputKeyPressed : AbstractTrigger
    {

        private bool playerIsInsideRange;

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                GameManager.instance.PlayerHandler.inputKeyTriggers.Add(this);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                GameManager.instance.PlayerHandler.inputKeyTriggers.Remove(this);
            }
        }
    }
}