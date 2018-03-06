using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperMetroid;
using SuperMetroid.Tiles.Shutters;

namespace SuperMetroid.Projectiles
{
	public class Missile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Missile");
		}
		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 15;
			projectile.aiStyle = 1;
			projectile.timeLeft = 600;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1.5f;
			projectile.ranged = true;
			projectile.netUpdate = true;
		}
		int mHitDoor = 0;
		bool soundInit = false;
		public override void AI()
		{
			if(!soundInit) 
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Missile"), projectile.position);
				soundInit = true;
			}
			
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
				PreKill(0);
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
		
			Color color = new Color();
			int dust = Dust.NewDust(new Vector2((float) projectile.position.X, (float) projectile.position.Y), projectile.width, projectile.height, 6, 0, 0, 100, color, 2.0f);
			Main.dust[dust].noGravity = true;
			

		/*	foreach(NPC N in Main.npc)
			{
				if(!N.active) continue;
				if(N.life <= 0) continue;
				if(N.friendly) continue;
				if(N.dontTakeDamage) continue;
				Rectangle projrec = new Rectangle((int)projectile.position.X+(int)projectile.velocity.X, (int)projectile.position.Y+(int)projectile.velocity.Y, projectile.width, projectile.height);
				Rectangle nrec = new Rectangle((int)N.position.X, (int)N.position.Y, (int)N.width,(int)N.height);
				if (projrec.Intersects(nrec))
				{
					if(N.type == mod.NPCType("GoldTorizo"))
					{
						projectile.damage = 0;
					}
					else
					{ 	projectile.damage = 20;		}
				}
			}
			int projX = (int)((projectile.position.X+(projectile.width*0.5))/16);
			int projY = (int)(projectile.position.Y/16);
			int type = Main.tile[projX, projY].type;
			if(Collision.SolidCollision(new Vector2(projectile.position.X,projectile.position.Y), projectile.width, projectile.height) && type == mod.TileType("Closed Left Missile Door") ||
				Collision.SolidCollision(new Vector2(projectile.position.X,projectile.position.Y), projectile.width, projectile.height) && type == mod.TileType("Closed Left Missile Door") ||
				Collision.SolidCollision(new Vector2(projectile.position.X,projectile.position.Y), projectile.width, projectile.height) && type == mod.TileType("Closed Right Missile Door") ||
				Collision.SolidCollision(new Vector2(projectile.position.X,projectile.position.Y), projectile.width, projectile.height) && type == mod.TileType("Closed Right Missile Door")) 
			{
				mHitDoor++;
				if(mHitDoor > 0 && mHitDoor < 5)
				{
					Main.NewText(""+mHitDoor, 176, 196, 222);
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/MBoorHit"), projectile.position);
				}
				if(mHitDoor == 5)
				{
					mHitDoor = 0;
				//	Main.PlaySound(2,projX,projY,SoundHandler.soundID["Door Opening"]);
				//	Config.OpenCustomDoor(projX+4, projY, 1, Config.tileDefs.doorToggle[type]);
				//	Config.OpenCustomDoor(projX-4, projY, 1, Config.tileDefs.doorToggle[type]);
				//	ModPlayer.openDoor = 180;
				}
			}*/
		}

		public override bool PreKill(int timeleft)
		{
			Main.PlaySound(mod.GetSoundSlot(SoundType.Item, "Sounds/Item/MBurst"), projectile.position);
			return true;
		}
		float Radius = 12f;
		public override void Kill(int timeleft)
		{
			for (int num70 = 0; num70 < 25; num70++)
			{
				int num71 = Dust.NewDust(new Vector2(this.projectile.position.X-Radius, this.projectile.position.Y-Radius), this.projectile.width+(int)Radius*2, this.projectile.height+(int)Radius*2, 6, 0f, 0f, 100, default(Color), 4f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
				int num72 = Dust.NewDust(new Vector2(this.projectile.position.X-Radius, this.projectile.position.Y-Radius), this.projectile.width+(int)Radius*2, this.projectile.height+(int)Radius*2, 1, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num72].velocity *= 1.4f;
				Main.dust[num72].noGravity = true;
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
			GlobalPlayer.tileTime = 300;
			int type = mod.TileType("vBlueDoor");
			int type2 = mod.TileType("ChozoDoor");
			int transform = mod.TileType("vEmptyDoor");
			int transform2 = mod.TileType("ChozoDoorOpening");
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/DoorOpening"), projectile.position);
			if(Main.tile[x, y].type == (ushort)type) Main.tile[x, y].type = (ushort)transform;
			if(Main.tile[x, y].type == (ushort)type2) Main.tile[x, y].type = (ushort)transform2;
		}
	}
}