using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SuperMetroid.Tiles.WorldTiles
{
	public class NorfairRock : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			drop = mod.ItemType("NorfairRock");
			AddMapEntry(new Color(192, 152, 144));
		}
	}
}