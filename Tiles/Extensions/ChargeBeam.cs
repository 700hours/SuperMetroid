using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SuperMetroid;

namespace SuperMetroid.Tiles.Extensions
{
	public class ChargeBeam : ModTile
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
			name.SetDefault("Charge Beam");
			AddMapEntry(new Color(200, 200, 200), name);
			disableSmartCursor = true;
			animationFrameHeight = 18;
		}
		public static bool isLighted = false;
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if(isLighted)
			{
				r = 0.753f;
				g = 0.753f;
				b = 0.753f;
			}
			Player player = Main.player[Main.myPlayer];
			Vector2 tilev = new Vector2(i*16, j*16);
			Rectangle tileBox = new Rectangle((int)tilev.X, (int)tilev.Y, 16, 16);
			Rectangle playerBox = new Rectangle((int)player.position.X-112, (int)player.position.Y-112, (int)player.width+112, (int)player.height+112);
			if(playerBox.Intersects(tileBox)) isLighted = true;
			else isLighted = false;
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
		
	/*	public void UpgradeHit(int x,int y)
		{
			ModPlayer.cbUpg = 1;
			Item.NewItem((int)Main.player[Main.myPlayer].position.X,(int)Main.player[Main.myPlayer].position.Y,32,32,(int)Config.itemDefs.byName["Charge Beam"].type,1,false);
			ModPlayer.upgradePercent++;
			WorldGen.KillTile(x, y, false, false, true, null);
			if(ModPlayer.ffTimer <= 0)
			{
				ModPlayer.ffTimer = 3600; //1 minute
				Main.PlaySound(2,-1,-1,SoundHandler.soundID["Item Fanfare"]);
			}
		}	*/
	/*	int frame = 0;
		public void Update()
		{
			frame++;
			if(frame == 4)
			{
				Main.tileTexture[Config.tileDefs.ID["ChargeBeam"]] = Main.goreTexture[Config.goreID["Charge Beam 1"]];
			}
			if(frame == 8)
			{
				Main.tileTexture[Config.tileDefs.ID["ChargeBeam"]] = Main.goreTexture[Config.goreID["Charge Beam 2"]];
			}
			if(frame > 8)
			{
				frame = 0;
			}
		}	*/
	}
}