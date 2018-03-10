using Terraria.ModLoader;

namespace SuperMetroid.Items.Walls
{
	public class Wall_6 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Cavern Rock");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 7;
			item.useStyle = 1;
			item.consumable = false;
			item.createWall = mod.WallType("Wall_6");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(9);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}
	}
}