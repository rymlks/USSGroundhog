#nullable enable
using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneConsequence : AbstractConsequence
{
    public string sceneName;
    public override void Execute(TriggerData? data)
    {
        if (sceneName != null)
        {
            SceneManager.LoadScene(this.sceneName);
        }
    }
}
