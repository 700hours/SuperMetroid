using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperMetroid;
using SuperMetroid.Tiles.Shutters;

namespace SuperMetroid.Projectiles.Beams
{
	public class ChargeBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charge Beam");
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.timeLeft = 8800;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
			projectile.ranged = true;
			projectile.netUpdate = true;
		}
		
		public override void AI()
		{
		#region tile functions
			Vector2 tilev = new Vector2(projectile.position.X/16, projectile.position.Y/16);
			int type = mod.TileType("CrackedBlock");
			int type2 = mod.TileType("BlueSwitchleft");
			int type3 = mod.TileType("vBlueDoor");
			int type4 = mod.TileType("ChozoDoor");
			Tile T = Main.tile[(int)tilev.X, (int)tilev.Y];
			bool collision = Main.tile[(int)tilev.X, (int)tilev.Y].active() && (Main.tileSolid[T.type] == true);
			bool correctTile = (type == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			bool blueSwitch = (type2 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			bool vblueDoor = (type3 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			bool blueDoor = (type4 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			if(collision) 
			{
				Kill(0);
				if(correctTile)
				{
					KillBlock((int)tilev.X, (int)tilev.Y);
				}
				if(blueSwitch)
				{
					ShutterSwitch((int)tilev.X, (int)tilev.Y);
				}
				if(vblueDoor || blueDoor)
				{
					DoorToggle((int)tilev.X, (int)tilev.Y);
				}
				projectile.active = false;
			}
		#endregion
		}
		public override void Kill(int timeleft)
		{
			for(int i = 0; i < 20; i++) 
			{
				int num54 = Projectile.NewProjectile(this.projectile.position.X, this.projectile.position.Y,Main.rand.Next(10)-5,Main.rand.Next(10)-5,mod.ProjectileType("DiffusionBeam"),1,0.1f,this.projectile.owner);
				Main.projectile[num54].timeLeft = 15;
				Main.projectile[num54].tileCollide = false;
			}
		}
		
		public void KillBlock(int x,int y)
		{
			GlobalPlayer.tileTime = 300;
			int type = mod.TileType("EmptyBlock");
			Main.tile[x, y].type = (ushort)type;
			Projectile.NewProjectile(x,y,0,0,mod.ProjectileType("Crumble"),0,0,Main.myPlayer);
		}
		public void ShutterSwitch(int x,int y)
		{
			GlobalPlayer.tileTime = 300;
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/ShutterDoor"), projectile.position);
			int type = mod.TileType("EmptyShutter");
			if(Main.tile[x, y].type == mod.TileType("BlueSwitchleft"))
			{
				for(int i = 0; i < 4; i++) Main.tile[x+1, (y+1)+i].type = (ushort)type;
			}
		}
		public void DoorToggle(int x, int y)
		{
			int type = mod.TileType("vBlueDoor");
			int type2 = mod.TileType("ChozoDoor");
			int transform = mod.TileType("vEmptyDoor");
			int transform2 = mod.TileType("ChozoDoorOpening");
			GlobalPlayer.tileTime = 300;
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/DoorOpening"), projectile.position);
			if(Main.tile[x, y].type == (ushort)type) Main.tile[x, y].type = (ushort)transform;
			if(Main.tile[x, y].type == (ushort)type2) Main.tile[x, y].type = (ushort)transform2;
		}
	}
}