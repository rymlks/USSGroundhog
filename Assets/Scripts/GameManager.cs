using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;
using KinematicCharacterController;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ExamplePlayer PlayerHandler;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this.gameObject);
        }

        PlayerHandler = FindObjectOfType<ExamplePlayer>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        Instantiate(PlayerHandler.Character.gameObject);
        PlayerHandler.Character.gameObject.GetComponent<KinematicCharacterMotor>().SetPosition(new Vector3(-7.668f, 1.025f, 7.58f));
    }
}
