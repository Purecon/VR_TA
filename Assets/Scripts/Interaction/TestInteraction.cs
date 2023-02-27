using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestInteraction : MonoBehaviour
{
    string m_Path;
    public TMP_Text testText;

    private void Start()
    {
        //Get the path of the Game data folder
        m_Path = Application.dataPath;

        //Output the Game data path to the console
        string dataText = "dataPath : " + m_Path;
        Debug.Log(dataText);

        //Test showing datapath
        testText.text = dataText;
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
