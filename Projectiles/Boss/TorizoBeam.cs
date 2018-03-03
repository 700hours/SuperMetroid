using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Projectiles.Boss
{
	public class TorizoBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torizo Beam");
			Main.projFrames[projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 48;
			projectile.aiStyle = 1;
			projectile.timeLeft = 300;
			projectile.damage = 15;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
			projectile.magic = true;
			projectile.netUpdate = true;
		}
		int ticks;
		public override void AI()
		{
		//	projectile.AI(true);
		#region frames
			if(ticks < 40) ticks++;
			if(ticks%20 == 0 && ticks < 40) this.projectile.frame++ ;
			if (this.projectile.frame > 2) this.projectile.frame = 2;
		#endregion
			Lighting.AddLight((int)(((projectile.position.X + (float)(projectile.width*0.5))/16f)), ((int)(((projectile.position.Y + (float)projectile.height*0.5))/16f)), 0.5f, 0.4f, 0.0f);  
		}
	}
}