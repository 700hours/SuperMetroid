using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SuperMetroid.Tiles.WorldTiles
{
	public class BrinstarMetal : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			drop = mod.ItemType("BrinstarMetal");
			AddMapEntry(new Color(200, 200, 200));
		}
	}
}