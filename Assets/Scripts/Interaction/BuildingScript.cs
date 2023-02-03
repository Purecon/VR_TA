using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class BuildingScript : MonoBehaviour
{
    [System.Serializable]
    public struct BuildingData { 
        public string Name; 
        public int ID; 
        public Vector3 Position;
    } 
    public BuildingData buildingData;

    public static event Action<GameObject> selectedEvent;

    [Header("Event")]
    public HoverEnterEvent eventForInteraction;


    private void Start()
    {
        XRSimpleInteractable simpleInteractable = GetComponent<XRSimpleInteractable>();
        simpleInteractable.hoverEntered = eventForInteraction;
    }

    public void BuildingSelected()
    {
        //Debug.Log("Selected: " + buildingData.Name);
        BuildingEvent(gameObject);
    }

    public static void BuildingEvent(GameObject gameObject)
    {
        selectedEvent?.Invoke(gameObject);
    }
}
