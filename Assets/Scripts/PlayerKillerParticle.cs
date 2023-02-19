using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillerParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public string deathReason;

    void Start()
    {
        if (particleSystem != null)
        {
            particleSystem = GetComponentInParent<ParticleSystem>();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("yoooo!");
        if (other.CompareTag("Player"))
        {
            GameManager.instance.CommitDie(deathReason);
        }
    }
}
