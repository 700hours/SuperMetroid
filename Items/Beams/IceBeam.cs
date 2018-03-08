using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Items.Beams
{
	public class IceBeam : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Beam");
			Tooltip.SetDefault("Adds freeze effect");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = 2500;
			item.maxStack = 1;
			item.rare = 4;
			item.autoReuse = false;
			item.consumable = false;
			item.noMelee = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(9);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}