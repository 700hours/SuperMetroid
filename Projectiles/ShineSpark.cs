using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using SuperMetroid.Items.Extensions;

namespace SuperMetroid.Projectiles
{
	public class ShineSpark : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shine-Spark");
			Main.projFrames[projectile.type] = 9;
		}
		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 50;
			projectile.aiStyle = 0;
			projectile.timeLeft = 9000;
			projectile.friendly = true;
			projectile.penetrate = 500;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
			projectile.ranged = true;
			projectile.netUpdate = true;
		}
		int DelayTime = 0;
		int ShineSoundStart = 0;
		public override void AI()
		{
			Player P = Main.player[projectile.owner];
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			Vector2 PC = P.position +new Vector2(P.width/2,P.height/2);
			projectile.position.X=PC.X-projectile.width/2;
			projectile.position.Y=PC.Y-projectile.height/2;
			projectile.velocity = P.velocity/16;
			DelayTime++;
			ShineSoundStart++;
			if(DelayTime > 55)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/ShineSparkLoop"), projectile.position);
				DelayTime = 0;
			}
			if(ShineSoundStart > 3 && ShineSoundStart < 5)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/ShineSparkStartup"), projectile.position);
				ShineSoundStart = 6;
				if(ShineSoundStart > 6)
				{
					ShineSoundStart = 6;
				}
			}
			if(SpeedBooster.shineDir == 0 || P.height == 14)
			{
				projectile.Kill();
			}
			projectile.frameCounter++;
			if(projectile.frameCounter >= 2)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			if (projectile.frame >= 9)
			{
				projectile.frame = 0;
			}
		}
	}
}