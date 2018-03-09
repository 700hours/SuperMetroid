using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SuperMetroid.Tiles.Extensions
{
	public class PlasmaBeam : ModTile
	{
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
			name.SetDefault("Plasma Beam");
			AddMapEntry(new Color(200, 125, 50), name);
			disableSmartCursor = true;
			animationFrameHeight = 18;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<MetroidPlayer>(mod);
			
			if(modPlayer.isLighted)
			{
				r = 0.352f;
				g = 0.702f;
				b = 0.443f;
			}
		}
		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter > 8)
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