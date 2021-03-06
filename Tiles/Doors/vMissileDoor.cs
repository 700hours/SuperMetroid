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
	public class vMissileDoor : ModTile
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
			name.SetDefault("Vertical Closed Missile Door");
			AddMapEntry(new Color(200, 150, 100), name);
			disableSmartCursor = true;
		}
		public static bool isLighted = false;
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if(isLighted)
			{
				r = 0.505f;
				g = 0.278f;
				b = 0.505f;
			}
			Player player = Main.player[Main.myPlayer];
			Rectangle PB = new Rectangle((int)player.position.X - 32, (int)player.position.Y - 32, player.width + 32, player.height + 32);
			Rectangle TB = new Rectangle(i*16, j*16, 16, 16);
			if(player.Hitbox.Intersects(TB)) isLighted = true;
			else isLighted = false;
		}
		
		int type = 0;
		public override void HitWire(int i, int j)
		{
			type = mod.TileType("vEmptyDoor");
			Main.tile[i, j].type = (ushort)type;
		}
	}
}