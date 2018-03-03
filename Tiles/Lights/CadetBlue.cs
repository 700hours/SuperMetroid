using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SuperMetroid;

namespace SuperMetroid.Tiles.Lights
{
	public class CadetBlue : ModTile
	{
		private static int x, y, life;
		private static bool init = false;
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type]	= true;
			Main.tileLavaDeath[Type]		= false;
			Main.tileSolid[Type] 			= false;
			Main.tileMergeDirt[Type]		= false;
			Main.tileLighted[Type] 			= true;
			Main.tileBlockLight[Type]		= false;
			Main.tileNoSunLight[Type]		= false;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cadet Blue");
			AddMapEntry(new Color(200, 200, 200), name);
			disableSmartCursor = true;
		}
		public static bool isLighted = false;
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if(isLighted)
			{
				r = 0.373f;
				g = 0.62f;
				b = 0.62f;
			}
		//	x = i;
		//	y = j;
		//	Lighted(x, y);
/*		}
		public static void Lighted(int i, int j)
		{	*/
			Player player = Main.player[Main.myPlayer];
		//	Vector2 playerv = new Vector2(player.position.X, player.position.Y);
			Vector2 tilev = new Vector2(i*16, j*16);
			Rectangle tileBox = new Rectangle((int)tilev.X, (int)tilev.Y, 16, 16);
			Rectangle playerBox = new Rectangle((int)player.position.X-112, (int)player.position.Y-112, (int)player.width+112, (int)player.height+112);
			if(playerBox.Intersects(tileBox)) isLighted = true;
			else isLighted = false;
		//	Main.NewText("Tile X " + tilev.X + " Y " + tilev.Y, 200, 150, 100);
		//	Main.NewText("Player X " + playerv.X + " Y " + playerv.Y, 200, 150, 100);
		}
	}
}

//	deprecated
/*
public bool CheckPlaceTile(int x, int y) { return true;	}
int x, y;
public void Initialize(int i, int j)
{
	x = i;
	y = j;
}
*/