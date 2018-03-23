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
	public class Shutter : ModTile
	{
		private static int x, y, life;
		private static bool init = false;
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type]	= true;
			Main.tileLavaDeath[Type]		= false;
			Main.tileSolid[Type] 			= true;
			Main.tileMergeDirt[Type]		= false;
			Main.tileLighted[Type] 			= false;
			Main.tileBlockLight[Type]		= false;
			Main.tileNoSunLight[Type]		= false;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Shutter");
			AddMapEntry(new Color(200, 200, 200), name);
			disableSmartCursor = true;
		}
		public override bool PreDraw(int i, int j, SpriteBatch SB)
		{
			Tile T = Main.tile[x, y];
			int animationFrameWidth = 18;
			SpriteEffects effects = SpriteEffects.None;
			int k = Main.tileFrame[Type] + i % 2;
		
			Texture2D texture = mod.GetTexture("Tiles/Shutters/Shutter");
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
				
	//		Update(i, j);
			
			return false;
		}
	/*	private static int time = 180;
		private static Tile T;
		public static bool triggered = false;
		private byte frame = 0;
		public void Update(int i, int j)
		{
			x = i;
			y = j;
			T = Main.tile[x, y];
			if(time > 0) time--;
			if(triggered)
			{
				frame = 1;
				SetTileFrame(x,y, 0, frame);
			}
			if(time <= 0)
			{
				Main.tileSolid[T.type] = true;
				frame = 0;
				SetTileFrame(x,y, 0, frame);
				triggered = false;
				time = 180;
			}
		}
		public static void OpenDoor(int i, int j)
		{
			time = 180;
			Main.tileSolid[T.type] = false;
			if(!triggered) triggered = true;
		} */
		
		public void SetTileFrame(int i, int j, ushort type, byte frame)
		{ 
			Main.tile[x, y].frameX = (short)(frame*18);
		}
	/*	public void KillSDoor(int i,int j)
		{
			WorldGen.KillTile(x, y, false, false, true, null);
		}	*/
	}
}