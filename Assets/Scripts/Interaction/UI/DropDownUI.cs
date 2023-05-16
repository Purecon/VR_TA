using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Dropdown UI 
public class DropDownUI : MonoBehaviour
{
    public void SetInteractionConfigurationID(int ID)
    {
        DataConfigurationStatic.currentInteractionSelectionID = ID;
        Debug.Log("Interaction ID changed to "+ DataConfigurationStatic.currentInteractionSelectionID);
    }

    public void SetDataConfigurationID(int ID)
    {
        DataConfigurationStatic.currentDataSelectionID = ID;
        Debug.Log("Data ID changed to " + DataConfigurationStatic.currentDataSelectionID);
    }
}
