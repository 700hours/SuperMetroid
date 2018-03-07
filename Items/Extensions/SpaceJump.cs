using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperMetroid;

namespace SuperMetroid.Items.Extensions
{
	public class SpaceJump : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Speed Booster");
			Tooltip.SetDefault("Time a jump mid-air to jump again");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.maxStack = 1;
			item.consumable = false;
			item.value = 100;
			item.rare = 5;
			item.scale = 1f;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(9);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
				
		int jumpTimer = 0;
		public override void UpdateAccessory(Player player, bool hideVisuals)
		{	
			if(player.velocity.Y > 0)
			{
				jumpTimer++;
			}
			else
			{
				jumpTimer = 0;
			}
			if(player.velocity.X != 0/* && !ModPlayer.ballstate*/)
			{
				//player.doubleJump = true; //testing to override the original code? 
				if(player.controlJump && player.releaseJump && player.velocity.Y != 0 && player.velocity.Y >= 0.5f && jumpTimer < 60)
				{
					jumpTimer = 0;
					//player.jumpAgain = true; //testing to override the original code? 
					//original code
					player.velocity.Y = -Player.jumpSpeed * player.gravDir;
					player.jump = Player.jumpHeight;
				}
			}
		}
	}
}