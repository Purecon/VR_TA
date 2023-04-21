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
    public bool isPC = true;

    [Header("Lock Selection")]
    public bool lockSelection = false;

    [Header("Tooltip")]
    public GameObject toolTipObject;
    public ToolTip toolTip;
    public Vector3 toolTipOffset;

    [Header("UI")]
    //public TMP_Text codeText;
    public TMP_Text tabletCodeText;
    //Class Info
    [Header("Class Info")]
    public GameObject classInfoGameObject;
    public ClassInfoUI ciScript;

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
        classInfoGameObject.SetActive(false);
    }

    public void BuildingToolTip(GameObject buildingGameObject)
    {
        if (!lockSelection)
        {
            if (buildingGameObject != null)
            {
                toolTipObject.SetActive(true);
                classInfoGameObject.SetActive(true);
                toolTip.ToolTipText = buildingGameObject.name;
                //Vector3 originalPosition = buildingGameObject.GetComponent<BuildingScript>().buildingData.Position;
                BoxCollider buildingCollider = buildingGameObject.GetComponent<BoxCollider>();
                Vector3 originalPosition = buildingCollider.bounds.center;
                toolTip.AnchorPosition = originalPosition;
                toolTip.PivotPosition = new Vector3(buildingCollider.bounds.center.x + toolTipOffset.x,
                                                    buildingCollider.bounds.max.y + toolTipOffset.y,
                                                    buildingCollider.bounds.center.z + toolTipOffset.z);

                //Building ID Info
                BuildingScript bldScript = buildingGameObject.GetComponent<BuildingScript>();
                int bldID = bldScript.buildingData.ID;

                //Class info metrics
                Vector3 metrics = dataSpace.dataMetrics[bldID];

                ciScript.classInfoName.text = dataSpace.dataClasses[bldID];
                /*
                ciScript.classInfoLOC.text = metrics.y.ToString(); //4 color x
                ciScript.classInfoNOM.text = metrics.x.ToString(); //1 datapos x
                ciScript.classInfoNOA.text = metrics.z.ToString(); //7 localscale x
                Line of Code (Color) 
                Method (Height)
                Attribute (Width/Depth)
                */
                ciScript.classInfoLOC.text = metrics.y.ToString("F3");
                ciScript.classInfoNOM.text = (dataSpace.dataLocalScale[bldID].y / 0.625f).ToString("F3");
                ciScript.classInfoNOA.text = (dataSpace.dataLocalScale[bldID].x / 0.125f).ToString("F3");
                //ciScript.classInfoNOA.text = metrics.z.ToString(); //localscale x           
            }
            else
            {
                Debug.LogWarning("No game object reference");
            }

            //Modified from Merino,2017
            try
            {
                //Building ID Info
                BuildingScript bldScript = buildingGameObject.GetComponent<BuildingScript>();
                int bldID = bldScript.buildingData.ID;
                string classPath = dataSpace.dataSrc[bldID];
                //string m_Path = Application.dataPath;
                //string bldPath = m_Path + "/Data/" + classPath;

                //PC usage
                string bldPath;
                if (isPC)
                {
                    string m_Path = "C:/Gamedev/Backup/";
                    bldPath = m_Path + classPath;
                }
                else
                {
                    //Oculus usage
                    //bldPath = "/sdcard/Download/" + classPath;
                    //string bldPath = "/storage/emulated/0/Download/" + classPath;
                    bldPath = Application.persistentDataPath + "/" + classPath;
                }
                Debug.Log("Path to class : " + bldPath);
                ciScript.codeText.text = "";
                tabletCodeText.text = "";
                var fileStream = new FileStream(bldPath, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    //int i = 0;
                    string line;
                    bool isComment = false;
                    while ((line = streamReader.ReadLine()) != null /*&& i < 100 */&& ciScript.codeText.text.Length < 10000)
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
                            ciScript.codeText.text += line + "\n";
                            tabletCodeText.text += line + "\n";
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
                ciScript.codeText.text = "Exception: " + ex.ToString();
                tabletCodeText.text = "Exception: " + ex.ToString();
            }

            //Sound
            if (useSound)
            {
                audioSrc.Play();
            }
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
            //dataSpace.ReMergeWithException(building);
            BuildingToolTip(building);
        }
        else
        {
            Debug.LogWarning("Class not found");
        }
    }

    public void CodeSmell(int smellType)
    {
        switch (smellType)
        {
            case 0:
                //Data class
                foreach (GameObject building in dataSpace.gameObjectsGodClass)
                {
                    MeshRenderer mesh = building.GetComponent<MeshRenderer>();
                    mesh.enabled = true;
                    mesh.material.color = Color.yellow;
                }
                break;
            case 1:
                //Brain class
                foreach (GameObject building in dataSpace.gameObjectsBrainClass)
                {
                    MeshRenderer mesh = building.GetComponent<MeshRenderer>();
                    mesh.enabled = true;
                    mesh.material.color = Color.blue;
                }
                break;
            case 2:
                //God class
                foreach (GameObject building in dataSpace.gameObjectsGodClass)
                {
                    MeshRenderer mesh = building.GetComponent<MeshRenderer>();
                    mesh.enabled = true;
                    mesh.material.color = Color.red;
                }
                break;
        }
    }

    public void ToggleLockSelection()
    {
        lockSelection = !lockSelection;
    }
}
