using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.MixedReality.Toolkit.UI;

public class BuildingInteractionManager : MonoBehaviour
{
    [Header("Tooltip")]
    public GameObject toolTipObject;
    public ToolTip toolTip;
    public Vector3 toolTipOffset;
    

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
            //Vector3 originalPosition = buildingGameObject.GetComponent<BuildingScript>().buildingData.Position;
            BoxCollider buildingCollider = buildingGameObject.GetComponent<BoxCollider>();
            Vector3 originalPosition = buildingCollider.bounds.center;
            toolTip.AnchorPosition = originalPosition;
            toolTip.PivotPosition = new Vector3(buildingCollider.bounds.center.x + toolTipOffset.x,
                                                buildingCollider.bounds.max.y + toolTipOffset.y,
                                                buildingCollider.bounds.center.z + toolTipOffset.z);
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
