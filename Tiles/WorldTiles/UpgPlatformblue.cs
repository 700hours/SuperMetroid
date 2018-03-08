using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SuperMetroid.Tiles.WorldTiles
{
	public class UpgPlatformblue : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type]	= true;
			Main.tileLavaDeath[Type]		= false;
			Main.tileSolid[Type] 			= true;
			Main.tileMergeDirt[Type]		= false;
			Main.tileLighted[Type] 			= false;
			Main.tileBlockLight[Type]		= true;
			Main.tileNoSunLight[Type]		= false;
			drop = mod.ItemType("UpgPlatformblue");
			AddMapEntry(new Color(120, 120, 120));
		}
	}
}