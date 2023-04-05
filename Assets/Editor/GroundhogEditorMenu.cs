using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundhogEditorMenu : MonoBehaviour
{
    [MenuItem("USSGroundhog/Move Player Here _INS")]
    static void MovePlayerHere()
    {
        GameObject.FindWithTag("Player").transform.position = SceneView.lastActiveSceneView.camera.transform.position;
    }
}
