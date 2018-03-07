using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SuperMetroid;

namespace SuperMetroid.Tiles.Effects
{
	public class Spikes : ModTile
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
		public void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.1f;
			g = 0.1f;
			b = 0.1f;
		}
		
		Texture2D texture;
		public override bool PreDraw(int i, int j, SpriteBatch SB)
		{
			x = i;
			y = j;
			Tile T = Main.tile[x, y];
			ushort tilename = Main.tile[x,y].type;
			byte frame = Main.tile[x, y].frameNumber();
			
			int animationFrameWidth = 18;
			SpriteEffects effects = SpriteEffects.None;
			
			int k = Main.tileFrame[Type] + i % 1;
		/*	if (i % 2 == 0)
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
			k = k % 4; */

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
		
		//	draw spikes pointing up
			if(Main.tileSolid[Main.tile[i, j+1].type] && Main.tile[i, j+1].active())
			{
				Texture2D texture = mod.GetTexture("Gores/TilesAnimate/SpikeUp");
				Main.spriteBatch.Draw(
					texture,
					new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
					new Rectangle(T.frameX + animate, T.frameY, 16, 16),
					Lighting.GetColor(i, j), 0f, default(Vector2), 1f, effects, 0f);
			}
		//	draw spikes pointing right
			else if(Main.tileSolid[Main.tile[i-1, j].type] && Main.tile[i-1, j].active())
			{
				Texture2D texture = mod.GetTexture("Gores/TilesAnimate/SpikeRight");
				Main.spriteBatch.Draw(
					texture,
					new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
					new Rectangle(T.frameX + animate, T.frameY, 16, 16),
					Lighting.GetColor(i, j), 0f, default(Vector2), 1f, effects, 0f);
			}
		//	draw spikes pointing down	
			else if(Main.tileSolid[Main.tile[i, j-1].type] && Main.tile[i, j-1].active())
			{
				effects = SpriteEffects.FlipVertically;
				Texture2D texture = mod.GetTexture("Gores/TilesAnimate/SpikeUp");
				Main.spriteBatch.Draw(
					texture,
					new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
					new Rectangle(T.frameX + animate, T.frameY, 16, 16),
					Lighting.GetColor(i, j), 0f, default(Vector2), 1f, effects, 0f);
			}
		//	draw spikes pointing left	
			else if(Main.tileSolid[Main.tile[i+1, j].type] && Main.tile[i+1, j].active())
			{
				effects = SpriteEffects.FlipHorizontally;
				Texture2D texture = mod.GetTexture("Gores/TilesAnimate/SpikeRight");
				Main.spriteBatch.Draw(
					texture,
					new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
					new Rectangle(T.frameX + animate, T.frameY, 16, 16),
					Lighting.GetColor(i, j), 0f, default(Vector2), 1f, effects, 0f);
			}
			
			return false;
		}
		
		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter > 6)
			{
				frameCounter = 0;
				frame++;
				if (frame > 1)
				{
					frame = 0;
				}
			}
		}		
	}
}