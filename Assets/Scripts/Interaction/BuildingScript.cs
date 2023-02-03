using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BuildingScript : MonoBehaviour
{
    [System.Serializable]
    public struct BuildingData { 
        public string Name; 
        public int ID; 
    } 
    public BuildingData buildingData;

    private void Start()
    {
        //XRSimpleInteractable simpleInteractable = gameObject.AddComponent<XRSimpleInteractable>();
        //simpleInteractable.hoverEntered = eventForInteraction;
    }
}
