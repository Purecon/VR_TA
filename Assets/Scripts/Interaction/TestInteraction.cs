using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteraction : MonoBehaviour
{
    string m_Path;

    private void Start()
    {
        //Get the path of the Game data folder
        m_Path = Application.dataPath;

        //Output the Game data path to the console
        Debug.Log("dataPath : " + m_Path);
    }

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
