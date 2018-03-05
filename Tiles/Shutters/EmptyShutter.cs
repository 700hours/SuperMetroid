using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SuperMetroid;

namespace SuperMetroid.Tiles.Shutters
{
	public class EmptyShutter : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type]	= true;
			Main.tileLavaDeath[Type]		= false;
			Main.tileSolid[Type] 			= false;
			Main.tileMergeDirt[Type]		= false;
			Main.tileLighted[Type] 			= false;
			Main.tileBlockLight[Type]		= false;
			Main.tileNoSunLight[Type]		= true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Empty Shutter");
			AddMapEntry(new Color(200, 150, 100), name);
			disableSmartCursor = true;
		}
		
		int init = 1;
		public override void PostDraw(int i, int j, SpriteBatch SB)
		{
			if(GlobalPlayer.tileTime <= 0)
			{
				WorldGen.KillTile(i, j,	false, false, true);
				WorldGen.PlaceTile(i, j, mod.TileType("Shutter"));
			}
		}
	}
}