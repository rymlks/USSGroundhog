using System.Linq;
using TMPro;
using UnityEngine;

namespace UI
{
    public class StatusEffectUIController : TextUIController
    {
        public string statusName;

        public static StatusEffectUIController GetByStatusName(string status)
        {
            return GameObject.FindObjectsOfType<StatusEffectUIController>().First(controller => controller.statusName == status);
        }
    }
}
