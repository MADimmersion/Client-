using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PointCloud : MonoBehaviour
{
    //Get HMD position
    GameObject hmd;
    //Used to draw a set of points
    public Mesh mesh;
    //Array of coordinates for each point in PointCloud
    private Vector3[] vertices;
    public Vector3[] Vertices
    {
        get { return vertices; }
        set { vertices = value; }
    }
    //Array of colors corresponding to each point in PointCloud
    private Color32[] colors;
    public Color32[] Colors
    {
        set { colors = value; }
    }
    //List of indexes of points to be rendered
    int[] indices;

    public Vector3 center;
    public float radius = 1f;

    public bool isUpdated = false;

    public int maxNbOfVertices;
    Transform smokeTransform;

    private void Start()
    {
        hmd = GameObject.Find("Main Camera");
        //smokeTransform = GameObject.Find("Smoke").transform;
    }

    private void Update()
    {
        if (isUpdated)
        {
            InitPointCloud();
        }
    }

    //Draw point cloud.
    public void InitPointCloud()
    {
        int nbOfVertices = vertices.Length;

        // Check if the mesh is already instantiated
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            gameObject.GetComponent<MeshFilter>().mesh = mesh;
        }
        //float t = Time.realtimeSinceStartup;
        //smokeTransform.localScale = transform.localScale * radius;

        // Update the vertices, colors, and indices directly on the existing mesh
        mesh.Clear();


        // Update the mesh with the filtered vertices and colors
        mesh.vertices = vertices;
        mesh.colors32 = colors;

        // Generate indices
        indices = new int[nbOfVertices];
        for (int i = 0; i < nbOfVertices; i++)
        {
            indices[i] = i;
        }
        mesh.SetIndices(indices, MeshTopology.Points, 0);

        // Recalculate bounds to ensure proper rendering
        mesh.RecalculateBounds();
        //Debug.Log("Mesh creation took : " + (Time.realtimeSinceStartup - t));


        isUpdated = false;
    }
}
