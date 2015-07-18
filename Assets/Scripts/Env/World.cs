﻿using UnityEngine;
using UnityEngine.Networking;

public class World : NetworkBehaviour {
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
		Application.targetFrameRate = 60;

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

		if (isClient) {
			Cursor.lockState = CursorLockMode.Locked;
			chunks = new GameObject[Mathf.FloorToInt(worldX/chunkSize), Mathf.FloorToInt(worldY/chunkSize), Mathf.FloorToInt(worldZ/chunkSize)];
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
	GameObject GenerateChunk(int x, int y, int z) {
		GameObject go = Instantiate(chunk, new Vector3(x*chunkSize, y*chunkSize, z*chunkSize), new Quaternion(0,0,0,0)) as GameObject;
		
		Chunk newChunkScript = go.GetComponent("Chunk") as Chunk;
		newChunkScript.worldGO = this.gameObject;
		newChunkScript.chunkSize = chunkSize;
		newChunkScript.chunkX = x * chunkSize;
		newChunkScript.chunkY = y * chunkSize;
		newChunkScript.chunkZ = z * chunkSize;

		return go;
	}
	
	// Update is called once per frame
	void Update () {

	}

	[Command]
	public void CmdRemoveBlock(int x, int y, int z) {
		try {
			if (data[x, y, z] != Block.AIR) {
				data[x,y,z] = Block.AIR;
				//Debug.Log("Server Removed block " + x + " " + y + " " + z);

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
	
	[ClientRpc]
	public void RpcRemoveBlock(int x, int y, int z) {
		data[x,y,z] = Block.AIR; // Sync for client
		int chunkX = Mathf.FloorToInt(x / chunkSize);
		int chunkY = Mathf.FloorToInt(y / chunkSize);
		int chunkZ = Mathf.FloorToInt(z / chunkSize);
		Chunk chunk = chunks[chunkX, chunkY, chunkZ].GetComponent("Chunk") as Chunk;
		chunk.GenerateMesh();
		//Debug.Log("Client Removed block " + x + " " + y + " " + z);
	}

	public Block GetBlock(int x, int y, int z){
		
		if( x>=worldX || x<0 || y>=worldY || y<0 || z>=worldZ || z<0){
			return Block.AIR;
		}
		
		return data[x,y,z];
	}

	static int PerlinNoise(int x, int y, int z, float scale, float height, float power) {
		float rValue = Noise.GetNoise (((double)x) / scale, ((double)y) / scale, ((double)z) / scale);
		rValue *= height;

		if (power != 0) rValue = Mathf.Pow (rValue, power);
		return (int) rValue;
	}
}
