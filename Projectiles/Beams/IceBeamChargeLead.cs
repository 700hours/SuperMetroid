using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Projectiles.Beams
{
	public class IceBeamChargeLead : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Beam Charge Lead");
		}
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.timeLeft = 8800;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
			projectile.ranged = true;
			projectile.netUpdate = true;
		}
		float CHARGE = 0;
		int Pindex = -1;
		int Pindex2 = -1;
		int Pindex3 = -1;
		int Pindex4 = -1;
		int Pindex5 = -1;
		int Pindex6 = -1;
		public override void AI() 
		{
			CHARGE++;
			Projectile P = projectile;
			Player O = Main.player[P.owner];
			if(CHARGE > 101) CHARGE = 101;

			float PlayerCentreX = O.position.X + O.width/2;
			float PlayerCentreY = O.position.Y + O.height/2;
			float MY = Main.mouseY + Main.screenPosition.Y;
			float MX = Main.mouseX + Main.screenPosition.X;
			if (Main.myPlayer == P.owner/* && O.height == 42*/)
			{
				if (O.channel || O.controlUseItem)
				{
					float num119 = 0f;
					if (O.inventory[O.selectedItem].shoot == P.type)
					{
						num119 = O.inventory[O.selectedItem].shootSpeed * 14;
					}
					P.velocity.X = (float)Math.Cos(P.rotation)*num119;
					P.velocity.Y = (float)Math.Sin(P.rotation)*num119;
					P.timeLeft = 2;
				}
			}

			P.scale = CHARGE / 100;
			float targetrotation = (float)Math.Atan2((MY-PlayerCentreY),(MX-PlayerCentreX));
			P.rotation = targetrotation;
			O.itemTime = 2;
			O.itemAnimation = 2;
			P.position = new Vector2(O.itemLocation.X+(float)Math.Cos(targetrotation)*20+1,O.itemLocation.Y+(float)Math.Sin(targetrotation)*20-5);
			if(P.velocity.X < 0)
			{
				P.direction = -1;
			}
			else
			{
				P.direction = 1;
			}
			P.spriteDirection = P.direction;
			O.heldProj = P.whoAmI;
			O.direction = P.direction;
			O.itemRotation = (float)Math.Atan2((MY-PlayerCentreY)*O.direction,(MX-PlayerCentreX)*O.direction) -0.05f*O.direction;
		//	if(Pindex != -1 && Main.projectile[Pindex].active) Main.projectile[Pindex].RunMethod("TotalRotate",P.position-new Vector2(3f*O.direction,3f*O.gravDir),new Vector2((float)Math.Cos(targetrotation),(float)Math.Sin(targetrotation)));

			P.width = (int)(16f*P.scale);
			P.height = (int)(16f*P.scale);

		#region sounds and dust
			if(CHARGE > 9 && CHARGE < 11)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/ChargeStartup"), P.position);
			}
			else if(CHARGE > 97 && CHARGE < 99)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/ChargeMax"), P.position);
			}
			if(CHARGE > 99)
			{
				Color color = new Color();
				int dust = Dust.NewDust(P.position+P.velocity, P.width, P.height, 59, 0, 0, 100, color, 2.0f);
				Main.dust[dust].noGravity = true;
			}
		#endregion
		}
		public override void Kill(int timeLeft)
		{
			Projectile P = projectile;
			Player O = Main.player[P.owner];
			float PlayerCentreX = O.position.X + O.width/2;
			float PlayerCentreY = O.position.Y + O.height/2;
			if(CHARGE > 99 && O.statMana > (29 - (int)((float)O.inventory[O.selectedItem].mana*O.manaCost)) && O.height == 42)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/IceBeamChargeShot"), P.position);
				Pindex = Projectile.NewProjectile(PlayerCentreX,PlayerCentreY,P.velocity.X,P.velocity.Y,mod.ProjectileType("IceBeamChargeShot"),(int)((float)45*O.rangedDamage),5,P.owner);
				O.statMana -= 30 - (int)((float)O.inventory[O.selectedItem].mana*O.manaCost);
				O.manaRegenDelay = (int)O.maxRegenDelay;
			}
			else /*if(O.height == 42)*/
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/IceBeam"), P.position);
				Pindex = Projectile.NewProjectile(PlayerCentreX,PlayerCentreY,P.velocity.X,P.velocity.Y,mod.ProjectileType("IceBeam"),(int)((float)13*O.rangedDamage),5,P.owner);
			}
			if(O.height == 14 && CHARGE > 99 && O.statMana > 9)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/LayBomb"), P.position);
				Pindex2 = Projectile.NewProjectile(PlayerCentreX,PlayerCentreY+4,-5,-2,mod.ProjectileType("MorphBomb"),10,0,O.whoAmI);
				Pindex3 = Projectile.NewProjectile(PlayerCentreX,PlayerCentreY+4,-3,-4,mod.ProjectileType("MorphBomb"),10,0,O.whoAmI);
				Pindex4 = Projectile.NewProjectile(PlayerCentreX,PlayerCentreY+4,0,-5,mod.ProjectileType("MorphBomb"),10,0,O.whoAmI);
				Pindex5 = Projectile.NewProjectile(PlayerCentreX,PlayerCentreY+4,3,-4,mod.ProjectileType("MorphBomb"),10,0,O.whoAmI);
				Pindex6 = Projectile.NewProjectile(PlayerCentreX,PlayerCentreY+4,5,-2,mod.ProjectileType("MorphBomb"),10,0,O.whoAmI);
				Main.projectile[Pindex2].timeLeft = 90;
				Main.projectile[Pindex3].timeLeft = 90;
				Main.projectile[Pindex4].timeLeft = 90;
				Main.projectile[Pindex5].timeLeft = 90;
				Main.projectile[Pindex6].timeLeft = 90;
				O.statMana -= 10;
				O.manaRegenDelay = (int)O.maxRegenDelay;
			}
			else if(O.height == 14 && CHARGE > 99)
			{
			}
			P.active = false;
		}
	}
}