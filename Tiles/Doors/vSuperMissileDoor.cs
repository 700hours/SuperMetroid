using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SuperMetroid;

namespace SuperMetroid.Tiles.Doors
{
	public class vSuperMissileDoor : ModTile
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
			name.SetDefault("Vertical Super Missile Door");
			AddMapEntry(new Color(25, 125, 0), name);
			disableSmartCursor = true;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.149f;
			g = 0.537f;
			b = 0.0f;
		}
		
		int type = 0;
		public override void HitWire(int i, int j)
		{
			type = mod.TileType("vEmptyDoor");
			Main.tile[i, j].type = (ushort)type;
		}
	}
}