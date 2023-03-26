using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class TestInteraction : MonoBehaviour
{
    public ActionBasedController controller;
    private void Start()
    {
        controller = GetComponent<ActionBasedController>();

        //bool isPressed = controller.selectAction.action.ReadValue<bool>();

        controller.selectAction.action.performed += DebugMsgBool;
        controller.activateAction.action.performed += DebugMsgBool2;
    }

    public void DebugMsgBool(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Select is pressed");
    }

    public void DebugMsgBool2(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Activate is pressed");
    }

    public void DebugMsg(string message)
    {
        if (message != null)
        {
            Debug.Log(message);
        }
        else
        {
            Debug.Log("Work but no message");
        }
    }

    /*
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
    */
}
