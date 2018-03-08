using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SuperMetroid.Tiles.WorldTiles
{
	public class CrateriaCrust : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			drop = mod.ItemType("CrateriaCrust");
			AddMapEntry(new Color(175, 250, 25));
		}
	}
}