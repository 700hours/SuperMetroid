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
	public class Electric : ModTile
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
			name.SetDefault("Electric Block");
			AddMapEntry(new Color(200, 150, 100), name);
			disableSmartCursor = true;
		}
		
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.1f;
			g = 0.1f;
			b = 0.1f;
		}
		public override bool PreDraw(int i, int j, SpriteBatch SB)
		{
			for(int k = 0; k < 2; k++)
			{
				int a = Projectile.NewProjectile(i*16+8, j*16+8, Main.rand.Next(-1,1), 0,mod.ProjectileType("GreenSpark"), 20, 2.0f, 255);
				Main.projectile[a].aiStyle = 1;
				Main.projectile[a].timeLeft = 45;
				Main.projectile[a].tileCollide = false;
			}
			return true;
		}
	}
}