using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbstractTrigger : MonoBehaviour, ITrigger
{
    private List<IConsequence> _allConsequences = new List<IConsequence>();

    protected virtual void Start()
    {
        this._allConsequences = this.GetComponentsInChildren<IConsequence>().ToList();
    }

    private void ExecuteAllConsequences()
    {
        foreach (IConsequence consequence in this._allConsequences)
        {
            consequence.execute();
        }
    }

    public virtual void Engage()
    {
        this.ExecuteAllConsequences();
    }
}
