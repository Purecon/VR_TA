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
        //public Vector3 Position;
    } 
    public BuildingData buildingData;

    public static event Action<GameObject> selectedEvent;
    public static event Action<string> selectedByNameEvent;

    [Header("Event")]
    public HoverEnterEvent eventForInteraction;
    public HoverExitEvent eventForExitInteraction;

    [Header("Outline")]
    public Outline outlineScript;
    public bool useOutline = true;

    private void Start()
    {
        XRSimpleInteractable simpleInteractable = GetComponent<XRSimpleInteractable>();
        simpleInteractable.hoverEntered = eventForInteraction;
        simpleInteractable.hoverExited = eventForExitInteraction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SphereInteractor"))
        {
            Debug.Log("Sphere Collider Trigger Enter");
            BuildingSelected();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SphereInteractor"))
        {
            Debug.Log("Sphere Collider Trigger Exit");
            BuildingDeselected();
        }
    }

    public void BuildingSelected()
    {
        BuildingEvent(gameObject);
        if (useOutline)
        {
            ChangeOutlineOn(true);
        }
    }

    public void BuildingDeselected()
    {
        BuildingEvent(gameObject);
        if (useOutline)
        {
            ChangeOutlineOn(false);
        }
    }

    public static void BuildingEvent(GameObject gameObject)
    {
        selectedEvent?.Invoke(gameObject);
    }

    public static void BuildingEventByName(string name)
    {
        selectedByNameEvent?.Invoke(name);
    }

    public void ChangeOutlineOn(bool condition)
    {
        if(outlineScript != null)
        {
            outlineScript.enabled = condition;
        }
        else
        {
            outlineScript = gameObject.AddComponent<Outline>();
            outlineScript.enabled = condition;
        }

        //Disable or enable mesh renderer
        GetComponent<MeshRenderer>().enabled = true;
    }
}
