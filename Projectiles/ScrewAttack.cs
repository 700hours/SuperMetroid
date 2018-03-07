using System;
using System.IO;
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
	public class ScrewAttack : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Screw Attack");
			Main.projFrames[projectile.type] = 4;
		}
		public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 48;
			projectile.aiStyle = 0;
			projectile.timeLeft = 9000;
			projectile.friendly = true;
			projectile.penetrate = 500;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1.5f;
			projectile.ranged = true;
			projectile.netUpdate = true;
		}
		int DelayTime = 0;
		int ScrewSoundStart = 0;
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			
			Lighting.AddLight(player.position, 0.2f, 0.7f, 0.2f);
			projectile.position.X=player.position.X-player.width/2;
			projectile.position.Y=player.position.Y+player.height/2 - 8;
			projectile.direction = player.direction;
			
			bool checkKill = (player.velocity.X == 0 || player.velocity.Y == 0);
			if(checkKill) projectile.Kill();
			
			DelayTime++;
			ScrewSoundStart++;
			if((float)DelayTime > 14.4)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/ScrewAttack"), projectile.position);
				DelayTime = 0;
			}
			if(ScrewSoundStart > 1 && ScrewSoundStart < 5)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/ScrewAttackStart"), projectile.position);
				ScrewSoundStart = 6;
				if(ScrewSoundStart > 6)
				{
					ScrewSoundStart = 6;
				}
			}

			if(player.velocity.Y == 0 && player.velocity.X == 0 && (player.height == 14 || player.itemAnimation != 0 || player.controlHook || player.grapCount != 0/* || ModPlayer.grappled*/))
			{
				projectile.Kill();
			}
			foreach(Projectile Pr in Main.projectile) if (Pr!= null)
			{
				if(Pr.active && Pr.type == mod.ProjectileType("ShineSpark") ||
					Pr.active && Pr.type == mod.ProjectileType("ScrewAttackLeft"))
				{
					  projectile.Kill();
				}
			}
			foreach(NPC N in Main.npc)
			{
				if(!N.active) continue;
				if(!N.boss) continue;
				Rectangle projrec = new Rectangle((int)projectile.position.X+(int)projectile.velocity.X, (int)projectile.position.Y+(int)projectile.velocity.Y, projectile.width, projectile.height);
				Rectangle nrec = new Rectangle((int)N.position.X, (int)N.position.Y, (int)N.width,(int)N.height);
				if(projrec.Intersects(nrec))
				{
					projectile.damage = 0;
				}
			}
			
			projectile.frameCounter++;
			if(projectile.frameCounter >= 3)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			if (projectile.frame >= 4)
			{
				projectile.frame = 0;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			Texture2D texture = Main.projectileTexture[projectile.type];
			SpriteEffects effects = SpriteEffects.None;
			effects = SpriteEffects.FlipHorizontally;
			
			int frameHeight = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
			int startY = frameHeight * projectile.frame;
			
			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			Vector2 origin = sourceRectangle.Size();
			
			if(player.velocity.X < 0)
			{
				spriteBatch.Draw(Main.projectileTexture[projectile.type], 
					projectile.Center - Main.screenPosition, 
					sourceRectangle, Color.White, projectile.rotation, origin, projectile.scale, effects, 0f);
			}
			if(player.velocity.X > 0)
			{
				spriteBatch.Draw(Main.projectileTexture[projectile.type], 
					projectile.Center - Main.screenPosition, 
					sourceRectangle, Color.White, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
			}	
			return false;
		}
	}
}