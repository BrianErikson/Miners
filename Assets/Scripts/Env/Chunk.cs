using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour {
	public int chunkSize = 16;

	public List<Vector3> newVertices = new List<Vector3>();
	public List<int> newTriangles = new List<int>();
	public List<Vector2> newUV = new List<Vector2>();
	private Mesh mesh;
	
	private float tUnit = 0.25f;
	private Vector2 tStone = new Vector2(0,0);
	private Vector2 tGrass = new Vector2(0,1);
	private Vector2 tGrassTop = new Vector2(1,1);


	private int faceCount;
	private MeshCollider col;
	
	public bool update = false;

	public GameObject worldGO;
	private World world;

	public int chunkX;
	public int chunkY;
	public int chunkZ;

	// Use this for initialization
	void Start () {
		world = worldGO.GetComponent("World") as World;

		mesh = GetComponent<MeshFilter>().mesh;
		col = GetComponent<MeshCollider>();

		GenerateMesh();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CubeTop(int x, int y, int z, World.Block block) {
		newVertices.Add(new Vector3 (x,  y,  z + 1));
		newVertices.Add(new Vector3 (x + 1, y,  z + 1));
		newVertices.Add(new Vector3 (x + 1, y,  z ));
		newVertices.Add(new Vector3 (x,  y,  z ));
		
		Vector2 texturePos = new Vector2(0,0);

		if (GetBlock(x,y,z) == World.Block.ROCK) texturePos = tStone;
		else if (GetBlock (x,y,z) == World.Block.GRASS) texturePos = tGrassTop;
		
		Cube(texturePos);
	}

	void CubeNorth(int x, int y, int z, World.Block block) {
		newVertices.Add(new Vector3 (x + 1, y-1, z + 1));
		newVertices.Add(new Vector3 (x + 1, y, z + 1));
		newVertices.Add(new Vector3 (x, y, z + 1));
		newVertices.Add(new Vector3 (x, y-1, z + 1));
		
		Vector2 texturePos = new Vector2(0,0);
		
		if (GetBlock(x,y,z) == World.Block.ROCK) texturePos = tStone;
		else if (GetBlock (x,y,z) == World.Block.GRASS) texturePos = tGrass;
		
		Cube (texturePos);
	}

	void CubeEast(int x, int y, int z, World.Block block) {
		newVertices.Add(new Vector3 (x + 1, y - 1, z));
		newVertices.Add(new Vector3 (x + 1, y, z));
		newVertices.Add(new Vector3 (x + 1, y, z + 1));
		newVertices.Add(new Vector3 (x + 1, y - 1, z + 1));
		
		Vector2 texturePos = new Vector2(0,0);
		
		if (GetBlock(x,y,z) == World.Block.ROCK) texturePos = tStone;
		else if (GetBlock (x,y,z) == World.Block.GRASS) texturePos = tGrass;
		
		Cube (texturePos);
	}

	void CubeSouth(int x, int y, int z, World.Block block) {
		newVertices.Add(new Vector3 (x, y - 1, z));
		newVertices.Add(new Vector3 (x, y, z));
		newVertices.Add(new Vector3 (x + 1, y, z));
		newVertices.Add(new Vector3 (x + 1, y - 1, z));
		
		Vector2 texturePos = new Vector2(0,0);
		
		if (GetBlock(x,y,z) == World.Block.ROCK) texturePos = tStone;
		else if (GetBlock (x,y,z) == World.Block.GRASS) texturePos = tGrass;
		
		Cube (texturePos);
	}

	void CubeWest(int x, int y, int z, World.Block block) {
		newVertices.Add(new Vector3 (x, y- 1, z + 1));
		newVertices.Add(new Vector3 (x, y, z + 1));
		newVertices.Add(new Vector3 (x, y, z));
		newVertices.Add(new Vector3 (x, y - 1, z));
		
		Vector2 texturePos = new Vector2(0,0);
		
		if (GetBlock(x,y,z) == World.Block.ROCK) texturePos = tStone;
		else if (GetBlock (x,y,z) == World.Block.GRASS) texturePos = tGrass;
		
		Cube (texturePos);
	}

	void CubeBot(int x, int y, int z, World.Block block) {
		newVertices.Add(new Vector3 (x,  y-1,  z ));
		newVertices.Add(new Vector3 (x + 1, y-1,  z ));
		newVertices.Add(new Vector3 (x + 1, y-1,  z + 1));
		newVertices.Add(new Vector3 (x,  y-1,  z + 1));
		
		Vector2 texturePos = new Vector2(0,0);
		
		if (GetBlock(x,y,z) == World.Block.ROCK) texturePos = tStone;
		else if (GetBlock (x,y,z) == World.Block.GRASS) texturePos = tGrass;
		
		Cube (texturePos);
	}

	void UpdateMesh() {
		mesh.Clear ();
		mesh.vertices = newVertices.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.Optimize ();
		mesh.RecalculateNormals ();
		
		col.sharedMesh = null;
		col.sharedMesh = mesh;
		
		newVertices.Clear();
		newUV.Clear();
		newTriangles.Clear();
		
		faceCount = 0;
	}

	void Cube(Vector2 texturePos) {
		newTriangles.Add(faceCount * 4  ); //1
		newTriangles.Add(faceCount * 4 + 1 ); //2
		newTriangles.Add(faceCount * 4 + 2 ); //3
		newTriangles.Add(faceCount * 4  ); //1
		newTriangles.Add(faceCount * 4 + 2 ); //3
		newTriangles.Add(faceCount * 4 + 3 ); //4
		
		newUV.Add(new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
		newUV.Add(new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
		newUV.Add(new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
		newUV.Add(new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y));
		
		faceCount++;
	}

	void GenerateMesh() {
		for (int x = 0; x < chunkSize; x++) {
			for (int y = 0; y < chunkSize; y++) {
				for (int z = 0; z < chunkSize; z++) {
					if (GetBlock(x, y, z) != World.Block.AIR) {
						// Checking for exposed sides
						//above
						if (GetBlock(x, y + 1, z) == World.Block.AIR) CubeTop(x, y, z, GetBlock(x, y, z));
						//below
						if (GetBlock(x, y - 1, z) == World.Block.AIR) CubeBot(x, y, z, GetBlock(x, y, z));
						//east
						if (GetBlock(x + 1, y, z) == World.Block.AIR) CubeEast(x, y, z, GetBlock(x, y, z));
						//west
						if (GetBlock(x - 1, y, z) == World.Block.AIR) CubeWest(x, y, z, GetBlock(x, y, z));
						//north
						if (GetBlock(x, y, z + 1) == World.Block.AIR) CubeNorth(x, y, z, GetBlock(x, y, z));
						//south
						if (GetBlock(x, y, z - 1) == World.Block.AIR) CubeSouth(x, y, z, GetBlock(x, y, z));
					}
				}
			}
		}

		UpdateMesh();
	}

	World.Block GetBlock(int x, int y, int z) {
		return this.world.GetBlock(x + chunkX, y + chunkY, z + chunkZ);
	}
}
