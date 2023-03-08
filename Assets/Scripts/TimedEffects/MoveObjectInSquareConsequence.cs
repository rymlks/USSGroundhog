using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectInSquareConsequence : MoveObjectAlongWaypoints, IConsequence
{
    public void execute()
    {
        this.active = true;
    }
}
