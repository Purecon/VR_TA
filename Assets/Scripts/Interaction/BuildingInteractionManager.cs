using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInteractionManager : MonoBehaviour
{
    public void DebugMsg(Object gameObject)
    {
        if (gameObject != null)
        {
            Debug.Log(gameObject.name);
        }
        else
        {
            Debug.Log("Work but no message");
        }
    }
}
