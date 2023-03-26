using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using System.Text;
using System.IO;

public class BuildingInteractionManager : MonoBehaviour
{
    [Header("Tooltip")]
    public GameObject toolTipObject;
    public ToolTip toolTip;
    public Vector3 toolTipOffset;

    [Header("UI")]
    public GameObject codeGameObject;
    public TMP_Text codeText;
    public DataSpaceHandlerExperiment dataSpace;

    [Header("Audio")]
    public bool useSound = false;
    public AudioSource audioSrc;

    [Header("Material")]
    public bool changeMaterial = false;
    public Material highlightMaterial;

    private void OnEnable()
    {
        //Subscribe to event 
        BuildingScript.selectedEvent += BuildingToolTip;
        BuildingScript.selectedByNameEvent += BuildingTooltipByName;
    }

    private void OnDisable()
    {
        //Unsubscribe to event 
        BuildingScript.selectedEvent -= BuildingToolTip;
        BuildingScript.selectedByNameEvent -= BuildingTooltipByName;
    }

    private void Start()
    {
        toolTipObject.SetActive(false);
        codeGameObject.SetActive(false);
    }

    public void BuildingToolTip(GameObject buildingGameObject)
    {
        if (buildingGameObject != null)
        {
            toolTipObject.SetActive(true);
            codeGameObject.SetActive(true);
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

        //Modified from Merino,2017
        try
        {
            BuildingScript bldScript = buildingGameObject.GetComponent<BuildingScript>();
            int bldID = bldScript.buildingData.ID;
            string classPath = dataSpace.dataSrc[bldID];
            string m_Path = Application.dataPath;
            string bldPath = m_Path + "/Data/" + classPath;
            Debug.Log("Path to class : " + bldPath);
            codeText.text = "";
            var fileStream = new FileStream(bldPath, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                //int i = 0;
                string line;
                bool isComment = false;
                while ((line = streamReader.ReadLine()) != null /*&& i < 100 */&& codeText.text.Length < 10000)
                {
                    // i++;
                    bool skip = false;
                    if (line.IndexOf("/*") > -1)
                    {
                        isComment = true;
                    }
                    if (line.IndexOf("import") > -1 || line.IndexOf("//") > -1 || line.IndexOf("package") > -1 || line.Length == 0)
                    {
                        skip = true;
                    }
                    if (!isComment && !skip)
                    {
                        codeText.text += line + "\n";
                    }
                    if (line.IndexOf("*/") > -1)
                    {
                        isComment = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        //Sound
        if (useSound)
        {
            audioSrc.Play();
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

    public void BuildingTooltipByName(string className)
    {
        int classIndex = dataSpace.dataClasses.IndexOf(className);
        //Check if not found
        if(classIndex != -1)
        {
            GameObject building = dataSpace.dataClassGameObject[classIndex];
            if (changeMaterial)
            {
                MeshRenderer mesh = building.GetComponent<MeshRenderer>();
                mesh.enabled = true;
                mesh.material = highlightMaterial;
            }
            dataSpace.ReMergeWithException(building);
            BuildingToolTip(building);
        }
        else
        {
            Debug.LogWarning("Class not found");
        }
    }
}
