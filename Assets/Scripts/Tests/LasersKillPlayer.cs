using System.Collections.Generic;
using System.Linq;
using Managers;
using Player.Death;
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
            _particleSystem.Play(true);
        }

        void Update()
        {
            if (Time.time > 5f)
            {
                if (playerWasKilledByLaserRespawnably())
                {
                    Pass("player has died from laser");
                }
                else
                {
                    Fail("player has not died from laser");
                }
            }
        }

        void Fail(string message)
        {
            Debug.Log("<color=red>Test failed: " + message + "</color>");
            Destroy(this.gameObject);
        }

        void Pass(string message)
        {
            Debug.Log("<color=green>Test passed: " + message + "</color>");
            Destroy(this.gameObject);
        }

        private static bool playerWasKilledByLaserRespawnably()
        {
            Dictionary<float,DeathCharacteristics>.ValueCollection valueCollection = ScoreManager.instance.getLevelScore().deathsByTime.Values;
            return valueCollection.Count > 0 && valueCollection.First().getReason() == "laser" &&
                   valueCollection.First().shouldRespawn();
        }
    }
}