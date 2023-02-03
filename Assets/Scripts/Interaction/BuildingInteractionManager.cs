using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.MixedReality.Toolkit.UI;

public class BuildingInteractionManager : MonoBehaviour
{
    public GameObject toolTipObject;
    public ToolTip toolTip;

    private void Start()
    {
        //BuildingScript.selectedEvent += DebugMsg;

        //Subscribe to event 
        BuildingScript.selectedEvent += BuildingToolTip;
        toolTipObject.SetActive(false);
    }

    public void BuildingToolTip(GameObject buildingGameObject)
    {
        if (buildingGameObject != null)
        {
            toolTipObject.SetActive(true);
            toolTip.ToolTipText = buildingGameObject.name;
            //toolTip.AnchorPosition = buildingGameObject.transform.position;
            Vector3 originalPosition = buildingGameObject.GetComponent<BuildingScript>().buildingData.Position;
            toolTip.AnchorPosition = originalPosition;
        }
        else
        {
            Debug.LogWarning("No game object reference");
        }
    }



    public void DebugMsg(GameObject gameObject)
    {
        if (gameObject != null)
        {
            Debug.Log("Manager event received: " + gameObject.name);
        }
        else
        {
            Debug.Log("Work but no message");
        }
    }
}
