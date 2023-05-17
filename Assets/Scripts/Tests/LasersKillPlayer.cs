using Managers;
using UnityEngine;

namespace Tests
{
    public class LasersKillPlayer : MonoBehaviour
    {
        public GameObject scootyBeam;
        private ParticleSystem _particleSystem;

        void Start()
        {
            _particleSystem = scootyBeam.GetComponent<ParticleSystem>();
            if (!_particleSystem.emission.enabled)
            {
                Debug.Log("Test failed: laser not emitting");
                Die("");
            }
        }

        void Update()
        {
            if (Time.time > 5f)
            {
                Debug.Log(playerHasDiedLessThanOnce()
                    ? "<color=red>Test failed: player has not died from laser</color>"
                    : "<color=green>Test passed: player has died from laser</color>");
                Die("");
            }
        }

        void Die(string message)
        {
            Destroy(this.gameObject);
        }

        private static bool playerHasDiedLessThanOnce()
        {
            return ScoreManager.instance.getLevelScore().deathsByTime.Values.Count < 1;
        }
    }
}
