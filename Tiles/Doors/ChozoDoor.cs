using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SuperMetroid;

namespace SuperMetroid.Tiles.Doors
{
	public class ChozoDoor : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type]	= true;
			Main.tileLavaDeath[Type]		= false;
			Main.tileSolid[Type] 			= true;
			Main.tileMergeDirt[Type]		= false;
			Main.tileLighted[Type] 			= true;
			Main.tileBlockLight[Type]		= true;
			Main.tileNoSunLight[Type]		= false;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Chozo Door");
			AddMapEntry(new Color(200, 150, 100), name);
			disableSmartCursor = true;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
		//	var modPlayer = Main.LocalPlayer.GetModPlayer<MetroidPlayer>(mod);
			
		//	if(modPlayer.xrayOn)
		//	{
		//	}
			r = 0.164f;
			g = 0.258f;
			b = 0.478f;
		}
		
		int type = 0;
		public override void HitWire(int i, int j)
		{
			type = mod.TileType("EmptyChozoDoor");
			Main.tile[i, j].type = (ushort)type;
		}
	}
}