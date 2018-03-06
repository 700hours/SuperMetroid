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
	public class ChozoDoorOpening : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type]	= true;
			Main.tileLavaDeath[Type]		= false;
			Main.tileSolid[Type] 			= false;
			Main.tileMergeDirt[Type]		= false;
			Main.tileLighted[Type] 			= false;
			Main.tileBlockLight[Type]		= true;
			Main.tileNoSunLight[Type]		= false;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Chozo Door Animation");
			AddMapEntry(new Color(200, 150, 100), name);
			disableSmartCursor = true;
		}
		int x, y;
		public override bool PreDraw(int i, int j, SpriteBatch SB)
		{
			x = i;
			y = j;
			Tile T = Main.tile[x, y];
			ushort tilename = Main.tile[x,y].type;
			byte frame = Main.tile[x, y].frameNumber();
			
			int animationFrameWidth = 18;
			SpriteEffects effects = SpriteEffects.None;
		
			int k = Main.tileFrame[Type] + i % 4;
			if (i % 2 == 0)
			{
				k += 3;
			}
			if (i % 3 == 0)
			{
				k += 3;
			}
			if (i % 4 == 0)
			{
				k += 3;
			}
			k = k % 4; 

			Texture2D texture = mod.GetTexture("Tiles/Doors/ChozoDoorOpening");
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
				new Rectangle(T.frameX + animate, T.frameY, 16, 16),
				Lighting.GetColor(i, j), 0f, default(Vector2), 1f, effects, 0f);

			return false; // return false to stop vanilla draw.
		}
		
		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter > 5)
			{
				frameCounter = 0;
				frame++;
				if (frame > 4)
				{
					frame = 6;
				}
			}
		}		
	}
}