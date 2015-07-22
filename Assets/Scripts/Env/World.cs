using UnityEngine;
using UnityEngine.Networking;
using Miners;

public class World : NetworkBehaviour {
	public Block[,,] data;
	public static int worldX = 64;
	public static int worldY = 64;
	public static int worldZ = 64;

	public GameObject chunk;
	public Chunk[,,] chunks;
	public static int chunkSize = 16;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

		data = new Block[worldX, worldY, worldZ];
	
		for (int x = 0; x < worldX; x++) {
			for (int z = 0; z < worldZ; z++) {
				int stone = PerlinNoise (x, 0, z, 10, 3, 1.2f);
				stone += PerlinNoise (x, 300, z, 20, 4, 0) + 10;
				int dirt = PerlinNoise (x, 100, z, 50, 6, 0) + 2;

				for (int y = 0; y < worldY; y++) {
					if (y <= stone) data[x,y,z] = new Rock();
					else if (y < dirt+stone) data[x,y,z] = new Dirt();
					else if (y == dirt+stone) data[x,y,z] = new Grass();
					else data[x, y, z] = new Air();
				}
			}
		}

		if (isClient) {
			Cursor.lockState = CursorLockMode.Locked;
			chunks = new Chunk[Mathf.FloorToInt(worldX/chunkSize), Mathf.FloorToInt(worldY/chunkSize), Mathf.FloorToInt(worldZ/chunkSize)];
			GenerateChunks();
		}
	}

	[Client]
	void GenerateChunks() {
		for (int x = 0; x < chunks.GetLength(0); x++) {
			for (int y = 0; y < chunks.GetLength(1); y++) {
				for (int z = 0; z < chunks.GetLength(2); z++) {
					chunks[x, y, z] = GenerateChunk(x, y, z);
				}
			}
		}
	}

	[Client]
	Chunk GenerateChunk(int x, int y, int z) {
		GameObject go = Instantiate(chunk, new Vector3(x*chunkSize - 0.5f, y*chunkSize + 0.5f, z*chunkSize - 0.5f), new Quaternion(0,0,0,0)) as GameObject;
		
		Chunk newChunkScript = go.GetComponent("Chunk") as Chunk;
		newChunkScript.worldGO = this.gameObject;
		newChunkScript.chunkSize = chunkSize;
		newChunkScript.chunkX = x * chunkSize;
		newChunkScript.chunkY = y * chunkSize;
		newChunkScript.chunkZ = z * chunkSize;

		return newChunkScript;
	}
	
	// Update is called once per frame
	void Update () {

	}

	[Command]
	public void CmdAddBlock(BlockType type, int x, int y, int z) {
		try {
			if (data[x, y, z].type == BlockType.AIR) {
				data[x, y, z] = GetNewBlock(type);

				RpcAddBlock(type, x, y, z);
			}
			else {
				Debug.LogError("Server could not add block " + x + " " + y + " " + z + ". The block is not air");
			}
		}
		catch (System.IndexOutOfRangeException e) {
			Debug.LogError("Server could not add block " + x + " " + y + " " + z + ". " + e.Message);
			return;
		}
	}

	[Command]
	public void CmdRemoveBlock(int x, int y, int z) {
		try {
			if (data[x, y, z].type != BlockType.AIR) {
				data[x,y,z] = new Air();

				RpcRemoveBlock(x, y, z);
			}
			else {
				Debug.LogError("Server could not remove block " + x + " " + y + " " + z + ". The block is air");
			}
		}
		catch (System.IndexOutOfRangeException e) {
			Debug.LogError("Server could not remove block " + x + " " + y + " " + z + ". " + e.Message);
			return;
		}
	}

	public void UpdateChunksForBlock(int x, int y, int z) {
		float dx = (float)x / (float)chunkSize;
		float dy = (float)y / (float)chunkSize;
		float dz = (float)z / (float)chunkSize;
		
		int chunkX = Mathf.FloorToInt(dx);
		int chunkY = Mathf.FloorToInt(dy);
		int chunkZ = Mathf.FloorToInt(dz);

		// regenerate chunk that contains block
		chunks[chunkX, chunkY, chunkZ].GenerateMesh();
		
		// if on edge of chunk, regenerate neighboring chunk
		if(x-(chunkSize*chunkX)==0 && chunkX!=0){
			chunks[chunkX-1,chunkY, chunkZ].GenerateMesh();
		}
		
		if(x-(chunkSize*chunkX)==15 && chunkX!=chunks.GetLength(0)-1){
			chunks[chunkX+1,chunkY, chunkZ].GenerateMesh();
		}
		
		if(y-(chunkSize*chunkY)==0 && chunkY!=0){
			chunks[chunkX,chunkY-1, chunkZ].GenerateMesh();
		}
		
		if(y-(chunkSize*chunkY)==15 && chunkY!=chunks.GetLength(1)-1){
			chunks[chunkX,chunkY+1, chunkZ].GenerateMesh();
		}
		
		if(z-(chunkSize*chunkZ)==0 && chunkZ!=0){
			chunks[chunkX,chunkY, chunkZ-1].GenerateMesh();
		}
		
		if(z-(chunkSize*chunkZ)==15 && chunkZ!=chunks.GetLength(2)-1){
			chunks[chunkX,chunkY, chunkZ+1].GenerateMesh();
		}
	}

	[ClientRpc]
	public void RpcAddBlock(BlockType type, int x, int y, int z) {
		data[x, y, z] = GetNewBlock(type);
		UpdateChunksForBlock(x, y, z);
	}
	
	[ClientRpc]
	public void RpcRemoveBlock(int x, int y, int z) {
		data[x,y,z] = new Air(); // Sync for client
		UpdateChunksForBlock(x, y, z);
	}

	public Block GetBlock(int x, int y, int z){
		if( x>=worldX || x<0 || y>=worldY || y<0 || z>=worldZ || z<0){
			return new Air();
		}
		
		return data[x,y,z];
	}

	public Block GetNewBlock(BlockType type) {
		switch(type) {
		case BlockType.AIR:
			return new Air();
		case BlockType.DIRT:
			return new Dirt();
		case BlockType.GRASS:
			return new Grass();
		case BlockType.ROCK:
			return new Rock();
		default:
			return new Air();
		}
	}

	static int PerlinNoise(int x, int y, int z, float scale, float height, float power) {
		float rValue = Noise.GetNoise (((double)x) / scale, ((double)y) / scale, ((double)z) / scale);
		rValue *= height;

		if (power != 0) rValue = Mathf.Pow (rValue, power);
		return (int) rValue;
	}
}
