using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbstractTrigger : MonoBehaviour
{
    private List<IConsequence> _allConsequences = new List<IConsequence>();

    protected virtual void Start()
    {
        this._allConsequences = this.GetComponentsInChildren<IConsequence>().ToList();
    }

    protected virtual void ExecuteAllConsequences()
    {
        foreach (IConsequence consequence in this._allConsequences)
        {
            consequence.execute();
        }
    }
}
