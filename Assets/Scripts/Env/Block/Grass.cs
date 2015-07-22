using UnityEngine;
using System.Collections;
using Miners;

public class Grass : Block {
	public static BlockType TYPE = BlockType.DIRT;

	private static Vector2 side = new Vector2(0, 1);
	public static CubeTexture tex = new CubeTexture(new Vector2(1,1), new Vector2(0,2), 
	                                                    side, side, side, side);
	
	public Grass() : base(TYPE, tex) {

	}
}
		

