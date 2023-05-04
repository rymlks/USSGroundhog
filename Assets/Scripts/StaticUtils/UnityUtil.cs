using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityUtil
{
    public static List<GameObject> GetChildGameObjects(GameObject parent)
    {
        List<GameObject> toReturn = new List<GameObject>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            toReturn.Add(parent.transform.GetChild(i).gameObject);
        }

        return toReturn;
    }
}
