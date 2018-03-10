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
	public class CeresDoor : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type]	= true;
			Main.tileLavaDeath[Type]		= false;
			Main.tileSolid[Type] 			= true;
			Main.tileMergeDirt[Type]		= false;
			Main.tileLighted[Type] 			= true;
			Main.tileBlockLight[Type]		= true;
			Main.tileNoSunLight[Type]		= false;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Ceres Door");
			AddMapEntry(new Color(200, 150, 100), name);
			disableSmartCursor = true;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.164f;
			g = 0.258f;
			b = 0.478f;
		}
		
		int type = 0;
		public override void HitWire(int i, int j)
		{
			type = mod.TileType("CeresEmptyDoor");
			Main.tile[i, j].type = (ushort)type;
		}
	}
}

// old code archive
/*
bool isLighted = false;
public void AddLight(int x, int y, ref float r, ref float g, ref float b)
{
	if(isLighted)
	{
  r = 0.0f;
  g = 0.14f;
  b = 0.45f;
  }
}
public void UseTile(Player P, int x, int y)
{
	int type = Main.tile[x, y].type;
	if (Config.tileDefs.doorToggle.ContainsKey(type))
	{
		if(Config.tileDefs.doorType[type] == 1)
		{
			Config.tileDefs.doorType[type] = 1;
		}
		if (Config.tileDefs.doorType[type] == 2 && Config.CloseCustomDoor(Player.tileTargetX, Player.tileTargetY, Config.tileDefs.doorToggle[type], false))
		{
			Config.tileDefs.doorType[type] = 2;
		}
	}
}
int x, y;
public void Initialize(int i, int j)
{
	x = i;
	y = j;
}
public void Update(int x,int y)
{
	int type = Main.tile[x, y].type;
	foreach(Player P in Main.player){
	if(!P.active) continue;
	if(P.dead) continue;
		Rectangle PB = new Rectangle((int)(P.position.X/16), (int)(P.position.Y/16), P.width*2, 3);
		Rectangle TB = new Rectangle(x, y, 8, 9);
		if(!PB.Intersects(TB))
		{
			isLighted = false;
			if(Config.tileDefs.doorType[type] == 1)
			{
				Main.PlaySound(2, -1, -1, SoundHandler.soundID["Door Opening"]);
				Config.OpenCustomDoor(Player.tileTargetX, Player.tileTargetY, P.direction, Config.tileDefs.doorToggle[type]);
			}
		}
		if(PB.Intersects(TB))
		{
			isLighted = true;
			if (Config.tileDefs.doorType[type] == 2 && Config.CloseCustomDoor(Player.tileTargetX, Player.tileTargetY, Config.tileDefs.doorToggle[type], false))
			{
				Main.PlaySound(2, -1, -1, SoundHandler.soundID["Door Closing"]);
				NetMessage.SendData(19, -1, -1, "", 0, (float)Player.tileTargetX, (float)Player.tileTargetY, (float)P.direction, 0);
			}
		}
	}
}	*/