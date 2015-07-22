using UnityEngine;
using System.Collections;

namespace Miners {
	public struct CubeTexture {
		public Vector2 top;
		public Vector2 bottom;
		public Vector2 north;
		public Vector2 south;
		public Vector2 east;
		public Vector2 west;

		public CubeTexture(
			Vector2 top, Vector2 bottom,
			Vector2 north, Vector2 south,
			Vector2 east, Vector2 west) {

			this.top = top;
			this.bottom = bottom;
			this.north = north;
			this.south = south;
			this.east = east;
			this.west = west;
		}
	}

	public enum BlockType {
		AIR,
		GRASS,
		DIRT,
		ROCK
	}

	public class Block {
		public BlockType type;
		public CubeTexture cubeTex;

		public Block(BlockType type) {
			this.type = type;
		}
		
		public Block(BlockType type, CubeTexture tex) {
			this.type = type;
			this.cubeTex = tex;
		}
	}
}

