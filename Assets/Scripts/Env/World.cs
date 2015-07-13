using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {
	public enum Block {
		AIR,
		ROCK,
		GRASS
	}

	public Block[,,] data;
	public static int worldX = 64;
	public static int worldY = 64;
	public static int worldZ = 64;

	public GameObject chunk;
	public GameObject[,,] chunks;
	public static int chunkSize = 16;

	// Use this for initialization
	void Start () {
		data = new Block[worldX, worldY, worldZ];
	
		for (int x = 0; x < worldX; x++) {
			for (int z = 0; z < worldZ; z++) {
				int stone = PerlinNoise (x, 0, z, 10, 3, 1.2f);
				stone += PerlinNoise (x, 300, z, 20, 4, 0) + 10;
				int dirt = PerlinNoise (x, 100, z, 50, 2, 0) + 1;

				for (int y = 0; y < worldY; y++) {
					if (y <= stone) data[x,y,z] = Block.ROCK;
					else if (y <=dirt+stone) data[x,y,z] = Block.GRASS;
				}
			}
		}

		chunks = new GameObject[Mathf.FloorToInt(worldX/chunkSize), Mathf.FloorToInt(worldY/chunkSize), Mathf.FloorToInt(worldZ/chunkSize)];

		for (int x = 0; x < chunks.GetLength(0); x++) {
			for (int y = 0; y < chunks.GetLength(1); y++) {
				for (int z = 0; z < chunks.GetLength(2); z++) {
					chunks[x, y, z] = Instantiate (chunk, new Vector3(x*chunkSize, y*chunkSize, z*chunkSize), new Quaternion(0,0,0,0)) as GameObject;

					Chunk newChunkScript = chunks[x,y,z].GetComponent("Chunk") as Chunk;
					newChunkScript.worldGO = this.gameObject;
					newChunkScript.chunkSize = chunkSize;
					newChunkScript.chunkX = x * chunkSize;
					newChunkScript.chunkY = y * chunkSize;
					newChunkScript.chunkZ = z * chunkSize;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Block GetBlock(int x, int y, int z){
		
		if( x>=worldX || x<0 || y>=worldY || y<0 || z>=worldZ || z<0){
			return Block.ROCK;
		}
		
		return data[x,y,z];
	}

	int PerlinNoise(int x, int y, int z, float scale, float height, float power) {
		float rValue = Noise.GetNoise (((double)x) / scale, ((double)y) / scale, ((double)z) / scale);
		rValue *= height;

		if (power != 0) rValue = Mathf.Pow (rValue, power);
		return (int) rValue;
	}
}
