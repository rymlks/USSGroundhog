using System.Collections;
using System.Collections.Generic;
using Triggers;
using UnityEngine;

public interface IConsequence
{
    void execute(TriggerData? data);
}
