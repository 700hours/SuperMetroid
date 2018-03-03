using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Projectiles
{
	public class SuperMissile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Missile");
		}
		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 15;
			projectile.aiStyle = 1;
			projectile.timeLeft = 600;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.scale = 1.5f;
			projectile.ranged = true;
			projectile.netUpdate = true;
		}
		bool soundInit = false;
		public override void AI()
		{
			if(!soundInit) {
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/SMissile"), projectile.position);
				soundInit = true;
			}
		//	projectile.AI(true);
			Color color = new Color();
			int dust = Dust.NewDust(new Vector2((float) projectile.position.X, (float) projectile.position.Y), projectile.width, projectile.height, 6, 0, 0, 100, color, 2.0f);
			Main.dust[dust].noGravity = true;
			foreach(NPC N in Main.npc)
			{
				if(!N.active) continue;
				if(N.life <= 0) continue;
				if(N.friendly) continue;
				if(N.dontTakeDamage) continue;
			//	array about npcs to nullify gravity effects
			//	npcs affected "Geemer", "Owtch", "Sova","Sciser","Work Robot","Zero"
				Rectangle projrec = new Rectangle((int)projectile.position.X+(int)projectile.velocity.X, (int)projectile.position.Y+(int)projectile.velocity.Y, projectile.width, projectile.height);
				Rectangle nrec = new Rectangle((int)N.position.X, (int)N.position.Y, (int)N.width,(int)N.height);
				if(projrec.Intersects(nrec))
				{
		//			N.AddBuff("Shaken", 10800, false);
					if(N.type == mod.NPCType("Gold Torizo"))
					{
						projectile.damage = 0;
					//	ModPlayer.catchMissile = true;
						projectile.active = false;
					}
					else
					{ 	projectile.damage = 60;		}
				}
			}
			int projX = (int)(((projectile.position.X)+(projectile.width*0.5))/16);
			int projY = (int)(projectile.position.Y/16);
			int type = Main.tile[projX, projY].type;
			if(Collision.SolidCollision(new Vector2(projectile.position.X+4,projectile.position.Y), projectile.width, projectile.height) && type == mod.TileType("Closed Left SMissile Door") ||
				Collision.SolidCollision(new Vector2(projectile.position.X-4,projectile.position.Y), projectile.width, projectile.height) && type == mod.TileType("Closed Left SMissile Door") ||
				Collision.SolidCollision(new Vector2(projectile.position.X+4,projectile.position.Y), projectile.width, projectile.height) && type == mod.TileType("Closed Right SMissile Door") ||
				Collision.SolidCollision(new Vector2(projectile.position.X-4,projectile.position.Y), projectile.width, projectile.height) && type == mod.TileType("Closed Right SMissile Door") ||
				Collision.SolidCollision(new Vector2(projectile.position.X+4,projectile.position.Y), projectile.width, projectile.height) && type == mod.TileType("Closed Left Missile Door") ||
				Collision.SolidCollision(new Vector2(projectile.position.X-4,projectile.position.Y), projectile.width, projectile.height) && type == mod.TileType("Closed Left Missile Door") ||
				Collision.SolidCollision(new Vector2(projectile.position.X+4,projectile.position.Y), projectile.width, projectile.height) && type == mod.TileType("Closed Right Missile Door") ||
				Collision.SolidCollision(new Vector2(projectile.position.X-4,projectile.position.Y), projectile.width, projectile.height) && type == mod.TileType("Closed Right Missile Door")) 
			{ 
		//		Main.PlaySound(2,projX,projY,SoundHandler.soundID["Door Opening"]);
		//		Config.OpenCustomDoor(projX+4, projY, 1, Config.tileDefs.doorToggle[type]);
		//		Config.OpenCustomDoor(projX-4, projY, 1, Config.tileDefs.doorToggle[type]);
		//		ModPlayer.openDoor = 180;
			}
		}
		float Radius = 20f;
		public override void Kill(int timeleft)
		{
			Main.PlaySound(mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/SMBurst"), projectile.position);
			
			for (int num70 = 0; num70 < 25; num70++)
			{
				int num71 = Dust.NewDust(new Vector2(this.projectile.position.X-Radius, this.projectile.position.Y-Radius), this.projectile.width+(int)Radius*2, this.projectile.height+(int)Radius*2, 6, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
				int num72 = Dust.NewDust(new Vector2(this.projectile.position.X-Radius, this.projectile.position.Y-Radius), this.projectile.width+(int)Radius*2, this.projectile.height+(int)Radius*2, 60, 0f, 0f, 100, default(Color), 5f);
				Main.dust[num72].velocity *= 1.4f;
				Main.dust[num72].noGravity = true;
			}
		}
	}
}