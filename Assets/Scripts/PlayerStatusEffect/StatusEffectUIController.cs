using TMPro;
using UnityEngine;

namespace PlayerStatusEffect
{
    public class StatusEffectUIController : MonoBehaviour
    {

        public TextMeshProUGUI textMesh;
        private float lastAffectedTime = -1;
        public Color alertColor = Color.yellow;
        public string statusName;

        private PlayerHealthState healthState;
        
        void Start()
        {
            healthState = FindObjectOfType<PlayerHealthState>();
            if (textMesh == null)
            {
                this.textMesh = this.GetComponent<TextMeshProUGUI>();
            }
        }

        bool affectedNow()
        {
            float timeSinceLastAffected = Time.time - this.lastAffectedTime;
            return timeSinceLastAffected > 0 && timeSinceLastAffected < 1;
        }

        void LateUpdate()
        {
            if (this.affectedNow())
            {
                this.textMesh.color = alertColor;
            }
            else
            {
                if(this.textMesh)
                    this.textMesh.color = Color.clear;
            }

        }

        public void showStatusNextFrame()
        {   
            this.lastAffectedTime = Time.time;
        }
    }
}
