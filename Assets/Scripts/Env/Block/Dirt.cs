using UnityEngine;
using System.Collections;
using Miners;

public class Dirt : Block {
	public static BlockType TYPE = BlockType.DIRT;

	private static Vector2 tex = new Vector2(0,2);
	public static CubeTexture cTex = new CubeTexture(tex, tex, tex, tex, tex, tex);
	
	public Dirt() : base(TYPE, cTex) {
		
	}
}
