using UnityEngine;
using System.Collections;
using Miners;

public class Rock : Block {
	public static BlockType TYPE = BlockType.DIRT;
	
	private static Vector2 tex = new Vector2(0,0);
	public static CubeTexture cTex = new CubeTexture(tex, tex, tex, tex, tex, tex);
	
	public Rock() : base(TYPE, cTex) {
		
	}
}
