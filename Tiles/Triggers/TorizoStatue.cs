using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using SuperMetroid;

namespace SuperMetroid.Tiles.Triggers
{
	public class TorizoStatue : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type]	= true;
			Main.tileLavaDeath[Type]		= false;
			Main.tileSolid[Type] 			= true;
			Main.tileMergeDirt[Type]		= false;
			Main.tileLighted[Type] 			= true;
			Main.tileBlockLight[Type]		= false;
			Main.tileNoSunLight[Type]		= false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Statue");
			AddMapEntry(new Color(150, 100, 50), name);
			disableSmartCursor = true;
		}
		public static bool isLighted = false;
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if(isLighted)
			{
				SpawnBoss(i, j);
			}
			Player player = Main.player[Main.myPlayer];
			Vector2 tilev = new Vector2(i*16, j*16);
			Rectangle tileBox = new Rectangle((int)tilev.X - 2, (int)tilev.Y - 3, 16, 16);
			Rectangle playerBox = new Rectangle((int)player.position.X-112, (int)player.position.Y-112, (int)player.width+112, (int)player.height+112);
			if(playerBox.Intersects(tileBox)) isLighted = true;
		}
		public void SpawnBoss(int x, int y) 
		{
			Player player = Main.player[Main.myPlayer];
			int NPCwidth = 100;
			int NPCheight = 93;
			int SpawnX = (int)(player.position.X - 32 + (NPCwidth - player.width) * 0.5f);
			int SpawnY = (int)(player.position.Y + player.height - 2);
			int TorizoStatue = NPC.NewNPC(SpawnX, SpawnY, mod.NPCType("TorizoStatue"));
			
			Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType("MissileLauncher"), 1, false, -1, true, false);
			
			WorldGen.KillTile(x, y, false, false, true);
		}
	}
}