using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Miners;

public class Chunk : MonoBehaviour {
	public int chunkSize = 16;

	public List<Vector3> newVertices = new List<Vector3>();
	public List<int> newTriangles = new List<int>();
	public List<Vector2> newUV = new List<Vector2>();
	private Mesh mesh;
	
	private float tUnit = 0.25f;

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

	void CubeTop(int x, int y, int z, Block block) {
		newVertices.Add(new Vector3 (x,  y,  z + 1));
		newVertices.Add(new Vector3 (x + 1, y,  z + 1));
		newVertices.Add(new Vector3 (x + 1, y,  z ));
		newVertices.Add(new Vector3 (x,  y,  z ));
		
		Cube(block.cubeTex.top);
	}

	void CubeNorth(int x, int y, int z, Block block) {
		newVertices.Add(new Vector3 (x + 1, y-1, z + 1));
		newVertices.Add(new Vector3 (x + 1, y, z + 1));
		newVertices.Add(new Vector3 (x, y, z + 1));
		newVertices.Add(new Vector3 (x, y-1, z + 1));
		
		Cube (block.cubeTex.north);
	}

	void CubeEast(int x, int y, int z, Block block) {
		newVertices.Add(new Vector3 (x + 1, y - 1, z));
		newVertices.Add(new Vector3 (x + 1, y, z));
		newVertices.Add(new Vector3 (x + 1, y, z + 1));
		newVertices.Add(new Vector3 (x + 1, y - 1, z + 1));
		
		Cube (block.cubeTex.east);
	}

	void CubeSouth(int x, int y, int z, Block block) {
		newVertices.Add(new Vector3 (x, y - 1, z));
		newVertices.Add(new Vector3 (x, y, z));
		newVertices.Add(new Vector3 (x + 1, y, z));
		newVertices.Add(new Vector3 (x + 1, y - 1, z));

		Cube (block.cubeTex.south);
	}

	void CubeWest(int x, int y, int z, Block block) {
		newVertices.Add(new Vector3 (x, y- 1, z + 1));
		newVertices.Add(new Vector3 (x, y, z + 1));
		newVertices.Add(new Vector3 (x, y, z));
		newVertices.Add(new Vector3 (x, y - 1, z));
		
		Cube (block.cubeTex.west);
	}

	void CubeBot(int x, int y, int z, Block block) {
		newVertices.Add(new Vector3 (x,  y-1,  z ));
		newVertices.Add(new Vector3 (x + 1, y-1,  z ));
		newVertices.Add(new Vector3 (x + 1, y-1,  z + 1));
		newVertices.Add(new Vector3 (x,  y-1,  z + 1));
		
		Cube(block.cubeTex.bottom);
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

	public void GenerateMesh() {
		for (int x = 0; x < chunkSize; x++) {
			for (int y = 0; y < chunkSize; y++) {
				for (int z = 0; z < chunkSize; z++) {
					Block curBlock = GetBlock(x, y, z);
					if (curBlock.type != BlockType.AIR) {
						// Checking for exposed sides
						//above
						if (GetBlock(x, y + 1, z).type == BlockType.AIR) CubeTop(x, y, z, curBlock);
						//below
						if (GetBlock(x, y - 1, z).type == BlockType.AIR) CubeBot(x, y, z, curBlock);
						//east
						if (GetBlock(x + 1, y, z).type == BlockType.AIR) CubeEast(x, y, z, curBlock);
						//west
						if (GetBlock(x - 1, y, z).type == BlockType.AIR) CubeWest(x, y, z, curBlock);
						//north
						if (GetBlock(x, y, z + 1).type == BlockType.AIR) CubeNorth(x, y, z, curBlock);
						//south
						if (GetBlock(x, y, z - 1).type == BlockType.AIR) CubeSouth(x, y, z, curBlock);
					}
				}
			}
		}

		UpdateMesh();
	}

	Block GetBlock(int x, int y, int z) {
		return this.world.GetBlock(x + chunkX, y + chunkY, z + chunkZ);
	}

	public static Vector3 GetHitBlock(RaycastHit hit) {
		Vector3 hitPos = hit.point;
		hitPos += hit.normal * -0.5f;
		hitPos.x = Mathf.Round(hitPos.x);
		hitPos.y = Mathf.Round(hitPos.y);
		hitPos.z = Mathf.Round(hitPos.z);
		return hitPos;
	}
}
