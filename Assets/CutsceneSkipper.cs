using System.Collections;
using System.Collections.Generic;
using Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CutsceneSkipper : MonoBehaviour
{
    public float holdLengthBeforeSkip = 2.0f;
    public GameObject cutsceneObject;
    public GameObject skipIndicatorObject;

    protected float heldLengthCurently = 0.0f;
    
    void Start()
    {
        this.heldLengthCurently = 0.0f;
        if (cutsceneObject == null)
        {
            cutsceneObject = GetComponentInParent<Monologue>().gameObject;
        }

        if (skipIndicatorObject == null)
        {
            this.skipIndicatorObject = GameObject.Find("SkipProgressIndicator");
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            this.heldLengthCurently += Time.deltaTime;
            ToggleSkipIndicator(true);
            UpdateProgressBar();
            if (this.heldLengthCurently >= holdLengthBeforeSkip)
            {
                SkipCutscene();
            }
        }
        else if(heldLengthCurently > 0f)
        {
            heldLengthCurently = 0.0f;
            ToggleSkipIndicator(false);
        }
    }

    private void UpdateProgressBar()
    {
        this.skipIndicatorObject.GetComponentInChildren<Image>().fillAmount = GetSkipProgressPercentageNormalized();
    }

    private void ToggleSkipIndicator(bool onOrOff)
    {
        this.skipIndicatorObject.GetComponentInChildren<TextMeshProUGUI>().enabled = onOrOff;
        this.skipIndicatorObject.GetComponentInChildren<Image>().enabled = onOrOff;
    }

    public float GetSkipProgressPercentageNormalized()
    {
        return Mathf.Min(1.0f, this.heldLengthCurently / this.holdLengthBeforeSkip);
    }

    protected void SkipCutscene()
    {
        cutsceneObject.GetComponent<Monologue>().EndMonologue();
        cutsceneObject.GetComponent<PlayableDirector>().Stop();
        ToggleSkipIndicator(false);
        this.enabled = false;
    }
}
