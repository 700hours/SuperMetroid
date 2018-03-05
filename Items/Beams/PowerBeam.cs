using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using SuperMetroid;

namespace SuperMetroid.Items.Beams
{
	public class PowerBeam : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Beam");
			Tooltip.SetDefault("Press Q to open a menu to place and use extra beams"
				+ 	"\nBe sure you put the beams in the proper order:"
				+ 	"\nCharge, Ice, Wave, Spazer, Plasma."
				+ 	"\nRemember, Plasma and Spazer can't be combined."
				+	"\nIf there's any sort of mess-up, the Power Beam will shoot normal shots.");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.maxStack = 1;
			item.consumable = false;
			item.value = 100;
			item.rare = 5;
			item.scale = 1f;
		//	item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/PowerBeam");
			item.shoot = mod.ProjectileType("PowerBeam");
			item.damage = 10;
			item.shootSpeed = 17;
		//	item.mana = 4;
			item.useTime = 15;
			item.useAnimation = 15;
		//	item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(9);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
		static int Width = 1;
		static int Height = 5;

		Item[] contents;

	/*	public void Initialize()
		{
			contents = new Item[Width*Height];
			for(int i = 0; i < contents.Length ; i++) contents[i] = new Item();
		}	*/

	/*	public override void HoldStyle(Player P)
		{
			if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.Q) < 0)
			{
				Beam_Menu.Create(item,Width,Height);
				Config.tileInterface.itemSlots = contents;
				Config.tileInterface.SetLocation(new Vector2((float)P.position.X/16,(float)P.position.Y/16));
				Main.playerInventory = true;
			}
		}	*/
		bool UsingWaveBeam = false;
		bool UsingSpazer = false;
		bool UsingWaveSpazer = false;
		bool UsingIceWave = false;
		bool UsingWavePlasma = false;
		bool UsingAllBeam1 = false;
		bool UsingAllBeam2 = false;
		public override void HoldItem(Player player)
		{
			UsingWaveBeam = false;
			UsingSpazer = false;
			UsingWaveSpazer = false;
			UsingIceWave = false;
			UsingWavePlasma = false;
			UsingAllBeam1 = false;
			UsingAllBeam2 = false;
			// Power
			if (player.inventory[11].type != mod.ItemType("IceBeam") &&
				player.inventory[12].type != mod.ItemType("WaveBeam") &&
				player.inventory[13].type != mod.ItemType("Spazer") &&
				player.inventory[14].type != mod.ItemType("PlasmaBeam"))
			{
				// Charge
				if(player.inventory[10].type == mod.ItemType("ChargeBeam"))
				{
					item.shoot = mod.ProjectileType("ChargeBeamLead");
				//	item.UseSound = 0;
					item.shootSpeed = 1;
				//	item.toolTip = "Hold Click to charge.("+(int)((float)40*P.rangedDamage)+" Damage, uses 25 mana)";
					item.damage = 10;
					item.mana = 4;
					item.useTime = 15;
					item.useAnimation = 15;
				}
				else
				{
					item.shoot = mod.ProjectileType("PowerBeam");
					item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/PowerBeam");
					item.shootSpeed = 17;
					item.damage = 10;
					item.mana = 4;
					item.useTime = 15;
					item.useAnimation = 15;
				}
			}
		}

		public override void UseStyle(Player player)
		{
			if(UsingWaveBeam || UsingIceWave || UsingWaveSpazer || UsingWavePlasma || UsingAllBeam1 || UsingAllBeam2)
			{
				int Pspeed = 10;
				float VX = ((Main.mouseX + Main.screenPosition.X) - (player.position.X + player.width * 0.5f));
				float VY = ((Main.mouseY + Main.screenPosition.Y) - (player.position.Y + player.height * 0.5f));
				float distance = (float) Math.Sqrt((double) ((VX * VX) + (VY * VY)));
				distance = Pspeed / distance;
				VX *= distance;
				VY *= distance;
				player.itemLocation.X = player.position.X + (float)player.width * 0.5f - (float)Main.itemTexture[item.type].Width * 0.5f - (float)(player.direction * 2 * 0.8f);
				player.itemLocation.Y = player.position.Y + (float)player.height * 0.5f - (float)Main.itemTexture[item.type].Height * 0.5f;
				player.itemRotation = (float)Math.Atan2((double)(VY * (float)player.direction), (double)(VX * (float)player.direction));
				if ((float)Main.mouseX + Main.screenPosition.X > player.position.X + (float)player.width * 0.5f)
				{
					player.direction = 1;
				}
				else
				{
					player.direction = -1;
				}
			}
		}
		public override bool Shoot(Player player,ref Vector2 ShootPos,ref float ShootVelocityX,ref float ShootVelocityY,ref int projType,ref int Damage,ref float knockback)
		{
			double sideangle = Math.Atan2(ShootVelocityY, ShootVelocityX) + (Math.PI/2);
			float shX = (float)Math.Cos(sideangle) * 12f;
			float shY = (float)Math.Sin(sideangle) * 12f;
			if(UsingSpazer)
			{
				int s1 = Projectile.NewProjectile(ShootPos.X-shX, ShootPos.Y-shY, ShootVelocityX, ShootVelocityY, projType, Damage, knockback, Main.myPlayer);
				int s2 = Projectile.NewProjectile(ShootPos.X, ShootPos.Y, ShootVelocityX, ShootVelocityY, projType, Damage, knockback, Main.myPlayer);
				int s3 = Projectile.NewProjectile(ShootPos.X+shX, ShootPos.Y+shY, ShootVelocityX, ShootVelocityY, projType, Damage, knockback, Main.myPlayer);
				return false;
			}
			else return true;
		}
	}
}