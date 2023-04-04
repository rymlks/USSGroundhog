using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Examples;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public float explosionStrength = 10;
    public bool persist = true;
    public GameObject ExplosionPrefab;
    public GameObject ExampleCamera;

    public bool dontRespawn = false;
    //[SerializeField] private AudioSource _audioSource;

    public void Start()
    {

        this.GetComponent<Collider>().isTrigger = true;

    }

    bool IsCollisionWithPlayer(Collider triggeredCollider)
    {
        return triggeredCollider.gameObject.CompareTag("Player");
    }

    void OnTriggerEnter(Collider triggeredCollider)
    {

        if (IsCollisionWithPlayer(triggeredCollider))
        {

            Debug.LogWarning($"{gameObject.name} is in Collision");
            Debug.LogWarning($"{gameObject.tag} Collision tag detected");

            foreach(var part in GetComponentsInChildren<ParticleSystem>())
            {

                part.Play();

            }

            if (gameObject.CompareTag("exploding_canister"))
            {
                gameObject.GetComponent<SoundFXManager>().Play_Small_explosion();

                var explode = Instantiate(ExplosionPrefab);
                explode.transform.position = transform.position;

            } else if (gameObject.CompareTag("exploding_tank"))
            {
                gameObject.GetComponent<SoundFXManager>().Play_Tank_explosion();

                var explode = Instantiate(ExplosionPrefab);
                explode.transform.position = transform.position;

            }  else if (gameObject.CompareTag("exploding_crate"))
            {

                gameObject.GetComponent<SoundFXManager>().Play_Crate_explosion();

                var explode = Instantiate(ExplosionPrefab);
                explode.transform.position = transform.position;

            }

            GameManager.instance.CommitDie(new Dictionary<string, object>() {
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
