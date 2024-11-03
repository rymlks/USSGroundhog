using System.Linq;
using TMPro;
using UnityEngine;

namespace UI
{
    public class StatusEffectUIController : MaskedTextUIController
    {
        public string statusName;
        protected PlayerHealthState healthState;

        protected override void Start()
        {
            base.Start();
            if (statusName == null || statusName.Length < 1)
            {
                Debug.Log("Status Effect UI Controller has not been told which status to show");
            }
            healthState = FindObjectOfType<PlayerHealthState>();

        }

        protected virtual void Update()
        {
            if (healthState == null)
            {
                healthState = FindObjectOfType<PlayerHealthState>();
            }
            var statusEffectProgressScalar = healthState.Diagnose(statusName);
            this.ShowNextFrame();
            this.MaskText(Mathf.Max(statusEffectProgressScalar, 0));
        }

        public static StatusEffectUIController GetByStatusName(string status)
        {
            return GameObject.FindObjectsOfType<StatusEffectUIController>().FirstOrDefault(controller => controller.statusName == status);
        }
    }
}
