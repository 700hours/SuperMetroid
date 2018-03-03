using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.GameInput;

namespace SuperMetroid.Projectiles
{
	public class powerbombExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Bomb Explosion");
		}
		public override void SetDefaults()
		{
			projectile.width = 1000;
			projectile.height = 750;
			projectile.aiStyle = 1;
			projectile.timeLeft = 200;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.scale = 0.2f;
			projectile.ranged = true;
			projectile.netUpdate = true;
		}
		int scaleSize = 0;
		float BombRadius = 200f;
		public void AI()
		{
		#region frames
		float ProjectileCenter = (projectile.width/2) + (projectile.height/2);
						this.projectile.frameCounter++;
						this.scaleSize++;
						if (this.projectile.frameCounter%1 == 0)
						{
							this.projectile.scale = this.scaleSize*0.05f;
							projectile.velocity.Y = 0;
						}
						if (this.projectile.frameCounter == 90)
						{
							projectile.active = false;
						}
		#endregion
			
			Vector2 PC = projectile.position+new Vector2(projectile.width/2,projectile.height/2);
			
			float DTX = 500f; //detonation radius x
			float DTY = 300f; //detonation radius y
			int Dmg = 60; //damage
			float KB = 1f; // knockback

			Rectangle MB = new Rectangle((int)(PC.X-DTX),(int)(PC.Y-DTY),(int)(DTX*2),(int)(DTY*2)); //my box
			foreach(NPC N in Main.npc)
			{
				//this is to skip some enemies
				if(!N.active) continue;
				if(N.boss) continue;
				if(N.life <= 0) continue;
				if(N.friendly) continue;
				if(N.dontTakeDamage) continue;

				Rectangle NB = new Rectangle((int)N.position.X,(int)N.position.Y,N.width,N.height); //npc box
				if(MB.Intersects(NB))  //if they touch each other somehow
				{
					  N.StrikeNPC(Dmg,KB,(int)projectile.direction); //strike the npc
				}
			}
		}
	}
}