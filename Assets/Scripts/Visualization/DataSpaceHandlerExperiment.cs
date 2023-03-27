using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DataSpaceHandlerExperiment : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject dataObject;
    public GameObject buildingPrefab;

    [Header("Lists")]
    public List<Vector3> dataPositions;
    public List<Vector3> dataPositionsC1;
    public List<Vector3> dataPositionsC2;
    public List<string> dataClasses;
    public List<string> dataSrc;
    public List<GameObject> dataClassGameObject;
    public List<Vector3> dataMetrics;
    public List<Color> dataColors;
    public List<Color> dataColorsUnique;

    [Header("Data")]
    [SerializeField]
    public TextAsset data;

    [Header("Size")]
    public Vector3 localScale = new Vector3(1.25f,1.25f,1.25f);

    [SerializeField]
    public Material dataMappedMaterial;

    //Modified from Merino,2017

    // Start is called before the first frame update
    void Start()
    {
        //persiapan data split into lines
        string[] lines = data.text.Split('\r');

        int count = 0;

        List<GameObject> childCat1 = new List<GameObject>();

        for (int i = 0; i < lines.Length; i++)
        {
            //split line 
            string[] attributes = lines[i].Split(',');

            //prepare data point game objects
            GameObject dataPoint = Instantiate(dataObject);
            dataPoint.transform.parent = gameObject.transform;
            dataPoint.transform.localScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(
                (float.Parse(attributes[7], System.Globalization.CultureInfo.InvariantCulture)) * 0.1f,
                (float.Parse(attributes[9], System.Globalization.CultureInfo.InvariantCulture)) * 0.5f,
                (float.Parse(attributes[8], System.Globalization.CultureInfo.InvariantCulture)) * 0.1f));
            Vector3 dataPosition = new Vector3(//0.0f, 0.0f, 0.0f);
                (float.Parse(attributes[1], System.Globalization.CultureInfo.InvariantCulture)) * 1.0f,
                ((float.Parse(attributes[3], System.Globalization.CultureInfo.InvariantCulture)) * 0.8f),//-0.2f
                (float.Parse(attributes[2], System.Globalization.CultureInfo.InvariantCulture)) * 1.0f);

            Vector3 metrics = new Vector3(
                (float.Parse(attributes[1], System.Globalization.CultureInfo.InvariantCulture)),
                (float.Parse(attributes[4], System.Globalization.CultureInfo.InvariantCulture)),
                (float.Parse(attributes[7], System.Globalization.CultureInfo.InvariantCulture)));

            dataPoint.transform.localPosition = dataPosition;

            //add the data position
            dataPositions.Add(dataPosition);
            dataClasses.Add(attributes[0]);
            dataSrc.Add(attributes[10]);
            dataMetrics.Add(metrics);

            //set vertex color
            Mesh mesh = dataPoint.GetComponent<MeshFilter>().mesh;
            dataPositionsC1.Add(mesh.bounds.min);
            dataPositionsC2.Add(mesh.bounds.max);
            Vector3[] vertices = mesh.vertices;
            //Vector3[] newVertices = mesh.vertices;
            // Vector3[] normals = mesh.normals;
            Color[] colors = new Color[vertices.Length];
            float colorR = 0;
            float colorG = 0;
            float colorB = 0;
            for (int t = 0; t < vertices.Length; t++)
            {
                colorR += float.Parse(attributes[4]);
                colorG += float.Parse(attributes[5]);
                colorB += float.Parse(attributes[6]);

                /*
                colors[t] = new Color(float.Parse(attributes[4], System.Globalization.CultureInfo.InvariantCulture),
                    float.Parse(attributes[5], System.Globalization.CultureInfo.InvariantCulture),
                    float.Parse(attributes[6], System.Globalization.CultureInfo.InvariantCulture));
                //Debug.Log(colors[t]);
                // newVertices[t] += normals[i] * Mathf.Sin(Time.time);
                //colors[t] = new Color(0.2f, 0.6f, 0.4f);
                */
            }
            //mesh.colors = colors;
            //Color list
            dataColors.Add(new Color(colorR / vertices.Length, colorG / vertices.Length, colorB / vertices.Length));
            //mesh.vertices = newVertices;
            childCat1.Add(dataPoint);

            count++;
        }

        //Unique color
        dataColorsUnique = new List<Color>(dataColors.Distinct());

        createTiledCube(childCat1);

        gameObject.transform.localScale = localScale;

        //Gameobject interaction
        /*
        XRSimpleInteractable simpleInteractable = gameObject.AddComponent<XRSimpleInteractable>();
        simpleInteractable.hoverEntered = eventForInteraction;
        */

        //Class list for another way to access
        ClassListUI.InitiateClassEvent();
    }

    //Create a cube with all objects of that type and subdivide if needed
    private GameObject createTiledCube(List<GameObject> objects)
    {
        GameObject ret = null;

        //calculate max objects per cube (unity vertices limit)
        int vertexCount = dataObject.GetComponent<MeshFilter>().sharedMesh.vertexCount * objects.Count;
        Debug.Log("Total Vertices:" + vertexCount);
        Debug.Log("Max Value:" + System.UInt16.MaxValue);

        //Unity limitation. we need to split the object list
        if (vertexCount > System.UInt16.MaxValue)
        {
            Debug.Log("Tiling cubes. Needed subcubes:" + System.Math.Ceiling((double)vertexCount / System.UInt16.MaxValue));
            GameObject tiledCube = new GameObject("tiledCube");
            tiledCube.transform.parent = gameObject.transform;
            tiledCube.transform.localPosition = new Vector3(0, 0, 0);
            tiledCube.transform.localScale = new Vector3(1, 1, 1);

            int objectsPerRun = (int)System.Math.Floor(System.UInt16.MaxValue / (double)dataObject.GetComponent<MeshFilter>().sharedMesh.vertexCount);

            //iterate objects and create tiled cubes
            int index = 0;
            while (index < objects.Count)
            {
                createCubeObject(objects.GetRange(index, index + objectsPerRun >= objects.Count ? objects.Count - index : objectsPerRun), tiledCube);
                index += objectsPerRun;
            }
            ret = tiledCube;
        }
        else
        {
            ret = createCubeObject(objects, gameObject);
        }

        //destroy child objects
        foreach (GameObject o in objects)
        {
            Destroy(o);
        }

        return ret;
    }

    //create a single colored cube object out of the children
    private GameObject createCubeObject(List<GameObject> objects, GameObject parent)
    {
        //Color regarding object
        int count = 0;
        List<GameObject> tempObjectsAll = new List<GameObject>();
        foreach (GameObject individualObject in objects){
            //"realobject"
            //GameObject cube = new GameObject("Cube");
            //GameObject cube = new GameObject(dataClasses[count].ToString());
            GameObject cube = Instantiate(buildingPrefab);
            cube.name = dataClasses[count].ToString();
            cube.transform.parent = parent.transform;

            MeshFilter filter = cube.AddComponent<MeshFilter>();
            MeshRenderer renderer = cube.AddComponent<MeshRenderer>();

            renderer.material = dataMappedMaterial;
            renderer.material.color = dataColors[count];
            //renderer.material.SetColor(0,dataColors[0]);

            List<GameObject> tempObjects = new List<GameObject>();
            tempObjects.Add(individualObject);
            mergeChildren(cube, tempObjects, filter);

            cube.transform.parent = parent.transform;
            cube.transform.localPosition = new Vector3(0, 0, 0);
            cube.transform.localScale = new Vector3(1, 1, 1);

            //Collider
            BoxCollider boxCollider = cube.AddComponent<BoxCollider>();
            BoxCollider boxColliderTrigger = cube.AddComponent<BoxCollider>();
            boxColliderTrigger.isTrigger = true;
            //Interactable
            XRSimpleInteractable simpleInteractable = cube.AddComponent<XRSimpleInteractable>();

            //Building script
            BuildingScript buildingScript = cube.GetComponent<BuildingScript>();
            BuildingScript.BuildingData buildingData = new BuildingScript.BuildingData();
            buildingData.Name = dataClasses[count].ToString();
            buildingData.ID = count;
            //buildingData.Position = new Vector3(dataPositions[count].x * localScale.x, dataPositions[count].y * localScale.y, dataPositions[count].z * localScale.z);
            buildingScript.buildingData = buildingData;

            tempObjectsAll.Add(cube);
            dataClassGameObject.Add(cube);
            count++;
        }

        //For each unique color
        foreach(Color c in dataColorsUnique)
        {
            List<GameObject> tempObjects = new List<GameObject>();
            Material tempMaterial = null;
            foreach (GameObject o in tempObjectsAll)
            {
                MeshRenderer renderer = o.GetComponent<MeshRenderer>();
                //Group the same color
                if(renderer.material.color == c)
                {
                    tempObjects.Add(o);
                    tempMaterial = renderer.material;
                }
            }


            GameObject cubeParent = new GameObject("CubeParent");
            cubeParent.transform.parent = parent.transform;

            MeshFilter filterParent = cubeParent.AddComponent<MeshFilter>();
            MeshRenderer rendererParent = cubeParent.AddComponent<MeshRenderer>();

            rendererParent.material = tempMaterial;
            mergeChildren(cubeParent, tempObjects, filterParent);

            foreach(GameObject childObject in tempObjects)
            {
                //Disabled child gameobject
                childObject.GetComponent<MeshRenderer>().enabled = false;

                //Child interaction event
                //XRSimpleInteractable simpleInteractable = childObject.AddComponent<XRSimpleInteractable>();
                //simpleInteractable.hoverEntered = eventForInteraction;
            }

            //Parent interaction event
            //XRSimpleInteractable simpleInteractable = cubeParent.AddComponent<XRSimpleInteractable>();
            //simpleInteractable.hoverEntered = eventForInteraction;
        }

        /*
        GameObject cubeParent = tempObjectsAll[0];
        tempObjectsAll.Remove(cubeParent);
        mergeChildren(cubeParent, tempObjectsAll, cubeParent.GetComponent<MeshFilter>());
        */

        return parent;
    }

    private void mergeChildren(GameObject parent, List<GameObject> objects, MeshFilter target)
    {
        CombineInstance[] combine = new CombineInstance[objects.Count];
        //        System.Random rnd = new System.Random();
        for (int i = 0; i < objects.Count; i++)
        {
            //make sure the points are aligned with the scatterplot
            Vector3 localPos = objects[i].transform.localPosition;
            objects[i].transform.parent = parent.transform;
            objects[i].transform.localPosition = localPos;
            combine[i].mesh = objects[i].GetComponent<MeshFilter>().sharedMesh;
            combine[i].mesh.colors = objects[i].GetComponent<MeshFilter>().mesh.colors;
            combine[i].transform = objects[i].transform.localToWorldMatrix;

            //objects[i].SetActive(false);
        }

        target.mesh.CombineMeshes(combine);
    }

    public void ReMergeWithException(GameObject buildingCube)
    {
        GameObject parentCube = buildingCube.transform.parent.gameObject;
        List<GameObject> otherCube = new List<GameObject>();
        foreach (Transform child in parentCube.transform)
        {
            if (null == child)
                continue;
            GameObject children = child.gameObject;
            if(children != buildingCube)
            {
                otherCube.Add(children);
            }
        }
        MeshFilter filter = parentCube.GetComponent<MeshFilter>();
        mergeChildren(parentCube, otherCube, filter);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
