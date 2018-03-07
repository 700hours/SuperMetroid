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
	public class EmptyChozoDoor : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type]	= true;
			Main.tileLavaDeath[Type]		= false;
			Main.tileSolid[Type] 			= false;
			Main.tileMergeDirt[Type]		= false;
			Main.tileLighted[Type] 			= true;
			Main.tileBlockLight[Type]		= true;
			Main.tileNoSunLight[Type]		= false;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Chozo Door");
			AddMapEntry(new Color(200, 150, 100), name);
			disableSmartCursor = true;
		}
		public static bool isLighted = false;
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if(isLighted)
			{
				r = 0.164f;
				g = 0.258f;
				b = 0.478f;
			}
			Player player = Main.player[Main.myPlayer];
			Rectangle PB = new Rectangle((int)player.position.X - 32, (int)player.position.Y - 32, player.width + 32, player.height + 32);
			Rectangle TB = new Rectangle(i*16, j*16, 16, 16);
			if(player.Hitbox.Intersects(TB)) isLighted = true;
			else isLighted = false;
		}
		
		int ticks = 300;
		bool hit = false;
		public override bool PreDraw(int i, int j, SpriteBatch SB)
		{
			int x = i;
			int y = j;
			Tile T = Main.tile[x, y];
			ushort tilename = Main.tile[x,y].type;
			byte frame = Main.tile[x, y].frameNumber();
			if(hit)
			{
				frame = 1;
				SetTileFrame(x,y,tilename, frame);
				ticks--;
				if(ticks <= 0)
				{
					ticks = 300;
					frame = 0;
					SetTileFrame(x,y,tilename, frame);
					Main.tileSolid[T.type] = true;
					hit = false;
				}
			}
			int animationFrameWidth = 18;
			SpriteEffects effects = SpriteEffects.None;
			int k = Main.tileFrame[Type] + i % 2;
		
			Texture2D texture = mod.GetTexture("Tiles/Doors/ChozoDoor");
			if (Main.canDrawColorTile(i, j))
			{
				texture = Main.tileAltTexture[Type, (int)T.color()];
			}
			else
			{
				texture = Main.tileTexture[Type];
			}
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int animate = k * animationFrameWidth;

			Main.spriteBatch.Draw(
				texture,
				new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
				new Rectangle(T.frameX, T.frameY, 16, 16),
				Lighting.GetColor(i, j), 0f, default(Vector2), 1f, effects, 0f);

			return false;
		}	
		
		public void SetTileFrame(int x, int y, ushort type, byte frame)
		{
			Main.tile[x, y].frameX = (short)(frame*18);
		//	Main.tile[x, y].frameY = (short)(j*18);
		}
	}
}