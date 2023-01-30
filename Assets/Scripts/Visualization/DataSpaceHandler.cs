using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSpaceHandler : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject dataObject;

    [Header("Lists")]
    public List<Vector3> dataPositions;
    public List<Vector3> dataPositionsC1;
    public List<Vector3> dataPositionsC2;
    public List<string> dataClasses;
    public List<string> dataSrc;
    public List<Vector3> dataMetrics;

    [Header("Data")]
    [SerializeField]
    public TextAsset data;

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
            dataPoint.transform.localScale = Vector3.Scale(transform.localScale, new Vector3(
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
            for (int t = 0; t < vertices.Length; t++)
            {
                colors[t] = new Color(float.Parse(attributes[4], System.Globalization.CultureInfo.InvariantCulture),
                    float.Parse(attributes[5], System.Globalization.CultureInfo.InvariantCulture),
                    float.Parse(attributes[6], System.Globalization.CultureInfo.InvariantCulture));
                //Debug.Log(colors[t]);
                // newVertices[t] += normals[i] * Mathf.Sin(Time.time);
                //colors[t] = new Color(0.2f, 0.6f, 0.4f);
            }
            mesh.colors = colors;
            //mesh.vertices = newVertices;
            childCat1.Add(dataPoint);

            count++;
        }
        createTiledCube(childCat1);
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
            Debug.Log("Tiling cubes. Needed subcubes:"+ System.Math.Ceiling((double)vertexCount/ System.UInt16.MaxValue));
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
        //"realobject"
        GameObject cube = new GameObject("Cube");
        cube.transform.parent = parent.transform;


        MeshFilter filter = cube.AddComponent<MeshFilter>();
        MeshRenderer renderer = cube.AddComponent<MeshRenderer>();


        renderer.material = dataMappedMaterial;

        mergeChildren(cube, objects, filter);

        cube.transform.parent = parent.transform;
        cube.transform.localPosition = new Vector3(0, 0, 0);
        cube.transform.localScale = new Vector3(1, 1, 1);
        // cube.SetActive(true);

        return cube;
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
            combine[i].transform = objects[i].transform.localToWorldMatrix;
            objects[i].SetActive(false);
        }

        target.mesh.CombineMeshes(combine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
