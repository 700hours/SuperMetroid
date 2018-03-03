using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperMetroid;

namespace SuperMetroid.Items
{
	public class Morphball : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Press Z to morph");
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
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
		private int switchTime = 0, size = 14, bomb = 0, powerbomb;
		public bool Ibounce = true, trap = false, ballstate = false;
		public static bool morph = false;
		public const int SizeOffset = 0;
		public override void UpdateEquip(Player player)
		{
			if(switchTime > 0) switchTime--;
			if(switchTime <= 0 && Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.Z) < 0) 
			{
				morph = !morph;
				switchTime = 60;
			//!	Yoraiz0r's collision detection
			#region
				if(!this.trap)
				{
					bool executeChange = false;
					if(player.height == 14+SizeOffset)
					{
						float playerposX = player.width/2 + player.position.X;
						float playerposY = player.height/2 + player.position.Y;
						int pPosX = (int)(playerposX / 16f);
						int pPosY = (int)(playerposY / 16f);
						if (Main.tile[pPosX, pPosY - 1] == null)
						{
							Main.tile[pPosX, pPosY - 1] = new Tile();
						}
						if (Main.tile[pPosX, pPosY - 2] == null)
						{
							Main.tile[pPosX, pPosY - 2] = new Tile();
						}
						bool Inval1 = Main.tile[pPosX, pPosY - 1].active() && Main.tileSolid[(int)Main.tile[pPosX, pPosY - 1].type];
						bool Inval2 = Main.tile[pPosX, pPosY - 2].active() && Main.tileSolid[(int)Main.tile[pPosX, pPosY - 2].type];
						if (!(Inval1 || Inval2))
						{
							executeChange = true;
						}
					}
					else
					{
						executeChange = true;
					}
					if(executeChange)
					{
						this.ballstate = !this.ballstate;
						Vector2 PlayerDims = new Vector2(player.width,player.height);
						player.width = this.ballstate?14+SizeOffset:20;
						player.height = this.ballstate?14+SizeOffset:42;
						Vector2 NewDims = new Vector2(player.width,player.height);
						Vector2 TheDiff = PlayerDims - NewDims;
						player.position += new Vector2(0,this.ballstate?14+SizeOffset:-8+SizeOffset);
						player.position += TheDiff*0.5f;
					}
					this.trap = true;
				}
			}
			else
			{
				this.trap = false;
			}
			#endregion
			if(morph)
			{
				player.noItems = true;
				player.invis = true;
				player.noFallDmg = true;
			
				if(GlobalPlayer.springBall != 1) {
					Player.jumpHeight = 0;
					Player.jumpSpeed = 0;	
				}
				else {
					Player.jumpHeight = 20;
					Player.jumpSpeed = 6.51f;
				}
				//! Yoraiz0r's Ibounce
			#region
				if(Ibounce && !player.controlDown)
				{
					Vector2 value2 = player.velocity;
					player.velocity = Collision.TileCollision(player.position, player.velocity, player.width, player.height, false, false);        
					if (value2 != player.velocity)
					{
						if (player.velocity.Y != value2.Y && Math.Abs((double)value2.Y) > 7f)
						{
							player.velocity.Y = value2.Y * -0.7f;
						}
					}
				}
			#endregion
				if(this.bomb > 0) this.bomb--;
				if(this.bomb <= 0 && Main.mouseRight)
				{
					this.bomb = 20;
					int a = Projectile.NewProjectile(player.position.X+player.width/2,(player.position.Y+player.height/2)+16,0,0,mod.ProjectileType("MorphBomb"),10,0,player.whoAmI);
					Main.projectile[a].aiStyle = 0;
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Lay Bomb"), Main.projectile[a].position);
				}
				if(this.powerbomb > 0) this.powerbomb--;
				if(this.powerbomb <= 0 && Main.mouseMiddle && GlobalPlayer.numPBombs > 0)
				{
					GlobalPlayer.numPBombs -= 1;
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Lay Bomb"), player.position);
					this.powerbomb = 60;
					int b = Projectile.NewProjectile(player.position.X+player.width/2,(player.position.Y+player.height/2)+16,0,0,mod.ProjectileType("PowerBomb"),0,0,player.whoAmI);
					Main.projectile[b].aiStyle = 0;
					if(GlobalPlayer.numPBombs%5==0)
					{
						Main.NewText(""+GlobalPlayer.numPBombs+" Power Bombs left", 200,150,100);
					}
				}
			}
		}
	}
}