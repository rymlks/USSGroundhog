using System.Collections;
using System.Collections.Generic;
using Dialogue;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneSkipper : MonoBehaviour
{
    public float holdLengthBeforeSkip = 2.0f;
    public GameObject cutsceneObject;

    protected float heldLengthCurently = 0.0f;
    
    void Start()
    {
        this.heldLengthCurently = 0.0f;
        if (cutsceneObject == null)
        {
            cutsceneObject = GetComponentInParent<Monologue>().gameObject;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            this.heldLengthCurently += Time.deltaTime;
            if (this.heldLengthCurently >= holdLengthBeforeSkip)
            {
                SkipCutscene();
            }
        }
        else if(heldLengthCurently > 0f)
        {
            heldLengthCurently = 0.0f;
        }
    }

    protected void SkipCutscene()
    {
        cutsceneObject.GetComponent<Monologue>().EndMonologue();
        cutsceneObject.GetComponent<PlayableDirector>().Stop();
    }
}
