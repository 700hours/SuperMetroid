using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperMetroid;

namespace SuperMetroid.Items.Extensions
{
	public class HiJump : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hi-Jump");
			Tooltip.SetDefault("Increases jump height by 2/3");
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
				
		public override void HoldItem(Player player)
		{
			var modPlayer = player.GetModPlayer<MetroidPlayer>(mod);
			
			if(Main.mouseLeft) modPlayer.isLighted = true;
			else modPlayer.isLighted = false;
		}
		int soundLoop = 0, SoundStart = 0;
		public override void UseItem(Player player)
		{
			player.nightVision = true;
			player.findTreasure = true;
			
			soundLoop++;
			SoundStart++;
				
			if((float)soundLoop >= 40.92)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/XRay"), new Vector2(Main.mouseX, Main.mouseY));
				soundLoop = 0;
			}
			if(SoundStart > 1 && SoundStart < 5)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/XRayStart"), new Vector2(Main.mouseX, Main.mouseY));
				SoundStart = 4;
				if(SoundStart > 6)
				{
					SoundStart = 6;
				}
			}
		}
		public override bool OnPickup(Player player)
		{
			var modPlayer = player.GetModPlayer<MetroidPlayer>(mod);
			if(modPlayer.ffTimer <= 0)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/ItemFanfare"), player.position);
				modPlayer.ffTimer = 3600;
			}
			return true;
		}
	}
}