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
	public class TorizoBomb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torizo Bomb");
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 48;
			projectile.aiStyle = 1;
			projectile.timeLeft = 300;
			projectile.damage = 15;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
			projectile.magic = true;
			projectile.netUpdate = true;
		}
		public override void Kill(int timeLeft){
			for (int i = 0; i < 10; i++) {
			int num54 = Projectile.NewProjectile(this.projectile.position.X, this.projectile.position.Y,Main.rand.Next(10)-5,Main.rand.Next(10)-5,mod.ProjectileType("TorizoBombDiffusion"),3,0.1f,this.projectile.owner);
			Main.projectile[num54].timeLeft = 12;
			Main.projectile[num54].tileCollide = false;
			}
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Custom/OneExplosion"), projectile.position);
			this.projectile.active=false;
		}
	}
}