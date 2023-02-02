using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteraction : MonoBehaviour
{
    public void DebugMsg(string message)
    {
        if(message != null)
        {
            Debug.Log(message);
        }
        else
        {
            Debug.Log("Work but no message");
        }
    }
}
