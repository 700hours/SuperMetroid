using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Items.Breakables
{
	public class CrackedBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cracked Block");
			Tooltip.SetDefault("Placeable tile");
		}
		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 99;
			item.useTime = 10;
			item.useAnimation = 15;
			item.useStyle = 1;
			item.value = 100;
			item.rare = 1;
			item.autoReuse = true;
			item.useTurn = true;
			item.consumable = false;
			item.noMelee = true;
			item.createTile = mod.TileType("CrackedBlock");
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