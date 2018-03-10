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
	public class IceDiffusionBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Diffusion Beam");
		}
		public override void SetDefaults()
		{
			projectile.width = 1;
			projectile.height = 1;
			projectile.aiStyle = 1;
			projectile.timeLeft = 200;
			projectile.friendly = true;
			projectile.penetrate = 500;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.scale = 1.6f;
			projectile.magic = true;
			projectile.netUpdate = true;
		}		
		public override void AI()
		{
		//	projectile.AI(true);
			Color color = new Color();
			int dust = Dust.NewDust(new Vector2((float) projectile.position.X, (float) projectile.position.Y), projectile.width, projectile.height, 59, 0, 0, 100, color, 3.0f);
			Main.dust[dust].noGravity = true;
			foreach(NPC N in Main.npc)
			{
				if(!N.active) continue;
				if(N.life <= 0) continue;
				if(N.friendly) continue;
				if(N.dontTakeDamage) continue;
				if(N.boss) continue;
				Rectangle MB = new Rectangle((int)projectile.position.X+(int)projectile.velocity.X,(int)projectile.position.Y+(int)projectile.velocity.Y,projectile.width,projectile.height);
				Rectangle NB = new Rectangle((int)N.position.X,(int)N.position.Y,N.width,N.height);
				if(MB.Intersects(NB))
				{
					N.AddBuff(mod.BuffType("Frozen"),600,false);
				}
			}
		}
	}
}