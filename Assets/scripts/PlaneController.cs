using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class PlaneController : MonoBehaviour {
    public List<Transform> connectedAtoms;
    Transform[] Atoms;
    List<Vector3> myVertices;
    MeshFilter myMeshFilter;
    Mesh myMesh;
    int[] Triangles = new int[6];
    bool set = false;
    Vector3 a, b, c;
    public Transform marker;
	// Use this for initialization
	void Start () {
        myVertices = new List<Vector3>();
        //connectedAtoms = new List<Transform>();
        //setupTest();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (marker == null)
            Destroy(gameObject);
        if (set)
        {
            foreach(Transform a in Atoms)
            {
                if (a == null)
                    Destroy(this.gameObject);
                
            }


            if (a != Atoms[0].position)
                updateVertices();
            if (b != Atoms[1].position)
                updateVertices();
            if (c != Atoms[2].position)
                updateVertices();
            a = Atoms[0].position;
            b = Atoms[1].position;
            c = Atoms[2].position;
           
        }
	}
    void updateVertices()
    {
        marker.position = Vector3.zero;
        myVertices.Clear();
        foreach(Transform point in Atoms)
        {
            myVertices.Add(point.position);
            
        }
        myMesh.vertices = myVertices.ToArray();
        marker.position += myVertices[0];
        marker.position += myVertices[1];
        marker.position += myVertices[2];
        marker.position /= 3;
        transform.position = Vector3.zero;
        Triangles[0] = 0;
        Triangles[1] = 1;
        Triangles[2] = 2;
        Triangles[3] = 2;
        Triangles[4] = 1;
        Triangles[5] = 0;
    
        myMesh.triangles = Triangles;
        myMesh.uv = UV_MaterialDisplay;

        myMesh.RecalculateNormals();
        //myMesh.Optimize();
        
    }
    public Vector2[] UV_MaterialDisplay = new Vector2[]
    {
         new Vector2(0,0),new Vector2(1,0),new Vector2(0,1),new Vector2(1,1) // 4 UV with all directions! (Plane has 4 uvMaps)
    };
    public void setup(List<Transform> verts)
    {
        connectedAtoms = new List<Transform>();
       
        connectedAtoms = verts;
       
        connectedAtoms.Add(verts[0]);
        connectedAtoms.Add(verts[1]);
        connectedAtoms.Add(verts[2]);
       
        myMeshFilter = GetComponent<MeshFilter>();
        myMesh = new Mesh();
        myMeshFilter.mesh = myMesh;
        myVertices = new List<Vector3>();
        set = true;
      
        Atoms = new Transform[6];
        Atoms = connectedAtoms.ToArray();
    }
    public void setupTest()
    {
        

        myMeshFilter = GetComponent<MeshFilter>();
        myMesh = new Mesh();
        myMeshFilter.mesh = myMesh;
        myVertices = new List<Vector3>();
        set = true;
    }
}
