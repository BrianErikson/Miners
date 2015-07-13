using UnityEngine;
using System.Collections.Generic;

public class GenerateMesh : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Mesh mesh = new Mesh();
		mesh.name = "World";

		List<Vector3> verts = new List<Vector3>();
		verts.Add(new Vector3(0.0f, 0.0f, 0.0f));
		verts.Add(new Vector3(0.0f, -1.0f, 0.0f));
		verts.Add(new Vector3(-1.0f, -1.0f, 0.0f));

		mesh.vertices = new Vector3[verts.Count];
		for (int i = 0; i < mesh.vertices.Length - 1; i++) {
			mesh.vertices[i] = verts[i];
		}

		mesh.triangles = new int[3];
		mesh.triangles[0] = 0;
		mesh.triangles[1] = 1;
		mesh.triangles[2] = 2;
		
		mesh.uv = new Vector2[3];
		mesh.uv[0] = new Vector2(0,0);
		mesh.uv[1] = new Vector2(0,1);
		mesh.uv[2] = new Vector2(1,1);

		GetComponent<MeshFilter>().mesh = mesh;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
