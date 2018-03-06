using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SuperMetroid.Items
{
	public class GrappleBeam : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grapple Beam");
			Tooltip.SetDefault("Hooks onto grapple blocks"
					+ "\nYou can ascend and descend the grapple by pressing up and down");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 10;
			item.useTime = 5;
			item.useAnimation = 5;
			item.useStyle = 5;
			item.value = 100;
			item.rare = 5;
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/GrappleBeam");
			item.autoReuse = false;
			item.consumable = false;
			item.noMelee = true;
			item.ranged = true;
			item.channel = true;
			item.damage = 10;
			item.knockBack = 4f;
			item.shootSpeed = 17f;
			item.shoot = mod.ProjectileType("GrappleBeam");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(9);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
		public override void UseStyle(Player P)
		{
			float PlayerCentreX = P.position.X + P.width/2;
			float PlayerCentreY = P.position.Y + P.height/2;
			float HalfPi = (float)(Math.PI/2f);
			if (Main.MouseWorld.X-P.position.X > 0) P.direction = 1;
			else P.direction = -1;
			P.itemRotation = (float)Math.Atan2((Main.MouseWorld.Y-PlayerCentreY)*P.direction,(Main.MouseWorld.X-PlayerCentreX)*P.direction);
			P.itemLocation.X-=P.direction*(P.width/8)*(1f-(float)Math.Abs(P.itemRotation)/HalfPi);
		}
	}
}