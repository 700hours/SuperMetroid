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
	public class BlueSwitchleft : ModTile
	{
		private static int x, y, life;
		private static bool init = false;
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type]	= true;
			Main.tileLavaDeath[Type]		= false;
			Main.tileSolid[Type] 			= true;
			Main.tileMergeDirt[Type]		= false;
			Main.tileLighted[Type] 			= true;
			Main.tileBlockLight[Type]		= false;
			Main.tileNoSunLight[Type]		= false;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Shutter Switch L");
			AddMapEntry(new Color(200, 200, 200), name);
			disableSmartCursor = true;
		}
		public static bool isLighted = false;
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if(isLighted)
			{
				r = 0.0f;
				g = 0.2f;
				b = 0.4f;
			}
			Player player = Main.player[Main.myPlayer];
			Vector2 tilev = new Vector2(i*16, j*16);
			Rectangle tileBox = new Rectangle((int)tilev.X, (int)tilev.Y, 16, 16);
			Rectangle playerBox = new Rectangle((int)player.position.X-112, (int)player.position.Y-112, (int)player.width+112, (int)player.height+112);
			if(playerBox.Intersects(tileBox)) isLighted = true;
			else isLighted = false;
		}
		public override bool PreDraw(int i, int j, SpriteBatch SB)
		{
			Tile T = Main.tile[x, y];
			int animationFrameWidth = 18;
			SpriteEffects effects = SpriteEffects.None;
			int k = Main.tileFrame[Type] + i % 1;
		
			Texture2D texture = mod.GetTexture("Tiles/Breakables/CrackedBlock");
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
	}
}