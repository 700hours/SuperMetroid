using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Items
{
	public class VariaArmor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varia Armor");
			Tooltip.SetDefault("Press R to toggle equip");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 20;
			item.maxStack = 1;
			item.consumable = false;
			item.value = 15;
			item.rare = 2;
			item.scale = 1f;
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