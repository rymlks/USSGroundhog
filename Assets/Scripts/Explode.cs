using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Examples;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public float explosionStrength = 10;
    public bool persist = true;
    public GameObject ExplosionPrefab;

    public bool dontRespawn = false;
    [SerializeField] private AudioSource _audioSource;

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
            Debug.LogWarning($"{gameObject.name} is in Collision");
            foreach(var part in GetComponentsInChildren<ParticleSystem>())
            {
                part.Play();
            }
            if (ExplosionPrefab != null)
            {
                GameObject.FindWithTag("MainCamera").GetComponent<audiosfx>().Play_Small_explosion();

                var explode = Instantiate(ExplosionPrefab);
                explode.transform.position = transform.position;

            }
            GameManager.instance.Respawn(new Dictionary<string, object>() {
                {"explosion", (triggeredCollider.transform.position - transform.position).normalized * explosionStrength},
                {"ragdoll", true},
                {"dontRespawn", dontRespawn}
            });
            if (!persist)
            {
                Destroy(gameObject);
            }
        }
    }
}
