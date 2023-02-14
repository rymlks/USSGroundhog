using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;
using KinematicCharacterController;
using KinematicCharacterController.Walkthrough.RootMotionExample;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public MyPlayer PlayerHandler;
    public GameObject playerPrefab;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this.gameObject);
        }

        PlayerHandler = FindObjectOfType<MyPlayer>();
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
        GameObject copy = Instantiate(playerPrefab);
        copy.transform.position = PlayerHandler.Character.transform.position;
        copy.transform.rotation = PlayerHandler.Character.transform.rotation;
        PlayerHandler.Character.gameObject.GetComponent<KinematicCharacterMotor>().SetPosition(new Vector3(-7.668f, 1.025f, 7.58f));
    }
}
