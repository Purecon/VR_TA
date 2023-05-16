using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class ClassListUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject content;
    public GameObject entryPrefab;

    [Header("Dataspace")]
    public DataSpaceHandlerExperiment dataSpace;

    //[Header("Event")]
    public static event Action classListEvent;

    private void OnEnable()
    {
        //Subscribe to event 
        classListEvent += InitiateClassList;
    }

    private void OnDisable()
    {
        //Unsubscribe to event 
        classListEvent -= InitiateClassList;
    }

    public static void InitiateClassEvent()
    {
        classListEvent?.Invoke();
    }

    private void InitiateClassList()
    {
        List<string> dataClassList = new List<string>(dataSpace.dataClasses);
        dataClassList.Sort();
        //List<GameObject> dataClassGameobjectList = dataSpace.dataClassGameObject;

        //Create a list of class
        for(int i = 0; i < dataClassList.Count; i++)
        {
            string className = dataClassList[i];
            //GameObject classBuilding = dataClassGameobjectList[i];
            GameObject entry = Instantiate(entryPrefab, content.transform);
            EntryClassListUI entryscript = entry.GetComponent<EntryClassListUI>();
            entryscript.className = className;
            entryscript.classNameLabel.text = className;
        }
    }
}
