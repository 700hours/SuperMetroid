using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SuperMetroid;

namespace SuperMetroid.Tiles.Breakables
{
	public class CrackedBlock : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type]	= true;
			Main.tileLavaDeath[Type]		= false;
			Main.tileSolid[Type] 			= true;
			Main.tileMergeDirt[Type]		= false;
			Main.tileLighted[Type] 			= false;
			Main.tileBlockLight[Type]		= true;
			Main.tileNoSunLight[Type]		= false;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Cracked Block");
			AddMapEntry(new Color(200, 150, 100), name);
			disableSmartCursor = true;
		}
		//	public bool CheckPlaceTile(int x, int y) { return true;	}
		private Vector2 tilev;
		public static bool hit = false; 
		bool init = false;
		public static int crumble = 0, Bomb;
		public void Initialize(int x, int y)
		{
			tilev = new Vector2(x,y);
		}
		public override bool PreDraw(int i, int j, SpriteBatch SB)
		{
			if(!init) 
			{
				Initialize(i, j);
				init = true;
			}
			int x = (int)tilev.X;
			int y = (int)tilev.Y;
			Tile T = Main.tile[x, y];
			ushort tilename = Main.tile[x,y].type;
			byte frame = Main.tile[x, y].frameNumber();
		/*	if(GlobalPlayer.xrayOn)
			{
				frame = 1;
				SetTileFrame(x,y,tilename, frame);
			}
			if(!GlobalPlayer.xrayOn)
			{
				frame = 0;
				SetTileFrame(x,y,tilename, frame);
			}	*/
			
		//	Main.NewText("Crumble set "+GlobalPlayer.tileTime, 200, 150, 100);
			if(hit)
			{
				frame = 2;
				SetTileFrame(x,y,tilename, frame);
				if(GlobalPlayer.tileTime <= 0)
				{
					frame = 0;
					SetTileFrame(x,y,tilename, frame);
					Main.tileSolid[T.type] = true;
					hit = false;
				}
			}
		/*	if(crumble == 1 && hit)
			{
				Main.tile[x, y].active(true);
				hit = false;
			}	*/
		/*	int width = 32;
			int height = 32;
			Bomb = Projectile.mod.ProjectileType("MorphBomb");
			Rectangle tileBox = new Rectangle(x*16, y*16, 16, 16);
			Rectangle projBox = new Rectangle((int)Main.projectile[Bomb].position.X-112, (int)Main.projectile[Bomb].position.Y-112, (int)width+112, (int)height+112);
			if(projBox.Intersects(tileBox)) 
			{
				Main.NewText("Collision", 200, 150, 100);
				KillBlock(i, j);
			}	*/
			int animationFrameWidth = 18;
			SpriteEffects effects = SpriteEffects.None;
		/*	if (i % 2 == 1)
			{
				effects = SpriteEffects.FlipHorizontally;
			} */
			int k = Main.tileFrame[Type] + i % 3;
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
			k = k % 6; */

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

			return false; // return false to stop vanilla draw.
		}	
		
		public void SetTileFrame(int x, int y, ushort type, byte frame){
		 
		//	int width = 3;
		//	int height = 1;
		//	for(int i=0; i<width;i++)
		//	{
		//		for(int j=0; j<height;j++)
		//		{
				//	frame = Main.tile[x+i, y+j].frameNumber();
					Main.tile[x, y].frameX = (short)(frame*18);
		//			Main.tile[x+i, y+j].frameY = (short)(j*18);
		//		}
		//	}
		}
		
		public static void KillBlock(int x, int y)
		{
			if(!hit)
			{
			//	Main.tile[x, y].active(false);
			//	int a = Projectile.NewProjectile(x*16, y*16,0,0,mod.ProjectileType("Crumble"),0,0,Main.player[Main.myPlayer].whoAmI);
			//	Main.projectile[a].aiStyle = -1;
				Tile T = Main.tile[x, y];
				Main.tileSolid[T.type] = false;
				GlobalPlayer.tileTime = 60*5;
				hit = true;
			}
		}
	}
}