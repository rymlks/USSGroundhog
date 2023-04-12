using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;

public class RotateConsequence : DoASpinny, IConsequence
{

    private float _speed;

    void Start()
    {
        _speed = speed;
        speed = 0;
    }

    public void execute(TriggerData? data)
    {
        speed = _speed;
    }
}
