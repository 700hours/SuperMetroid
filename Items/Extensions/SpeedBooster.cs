using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using SuperMetroid;

namespace SuperMetroid.Items.Extensions
{
	public class SpeedBooster : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Speed Booster");
			Tooltip.SetDefault("Run fast enough to activate it."
				+ 	"\nWhen it's active, press 'DOWN' to charge a Shine-Spark."
				+ 	"\nRelease the charge by pressing 'JUMP'--allowing you to super jump."
				+	"\nBefore you super jump completely, you can hold which direction you want to go.");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.maxStack = 1;
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
		
		static bool shineCharge = false;
		static bool shineActive = false;
		static int shineDeActive = 0;
		static int shineDeCharge = 0;
		public static int shineDir = 0;
		static int manaDelay = 0;
		static int proj = -1;
		
		Vector2 tilev;
		int TileX, TileY;

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
		//	ModPlayer.shineDirection = shineDir;
			if(/* !ModPlayer.ballstate && */Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.LeftShift) < 0)
			{
				tilev = new Vector2(player.position.X/16, player.position.Y/16);
				TileX = (int)tilev.X;
				TileY = (int)tilev.Y;
				bool onGround = (Main.tileSolid[Main.tile[TileX,TileY+3].type] && Main.tile[TileX, TileY+3].active());
				bool onGroundDeep = (Main.tileSolid[Main.tile[TileX,TileY+4].type] && Main.tile[TileX, TileY+4].active());
				if (player.controlLeft && (onGround || onGroundDeep)) 
				{
				if (player.velocity.X > -3) player.velocity.X -= (float) (player.moveSpeed-1f)/10;
				if (player.velocity.X < -3 && player.velocity.X > -8*player.moveSpeed)
				{
				if (player.velocity.Y != 0) player.velocity.X -= 0.1f;
				else player.velocity.X -= 0.2f;
				player.velocity.X -= (float) 0.02+((player.moveSpeed-1f)/10);
				}
				}
				if (player.controlRight && onGround)
				{
				if (player.velocity.X < 3) player.velocity.X += (float) (player.moveSpeed-1f)/10;
				if (player.velocity.X > 3 && player.velocity.X < 8*player.moveSpeed)
				{
				if (player.velocity.Y != 0) player.velocity.X += 0.1f;
				else player.velocity.X += 0.2f;
				player.velocity.X += (float) 0.02+((player.moveSpeed-1f)/10);
				}
				}
				if(player.velocity.X > 7 && player.controlRight)
				{
					player.armorEffectDrawShadow = true;
					bool SpeedBoostR = false;
					foreach(Projectile P in Main.projectile)
					{
						if(P.active && P.owner==player.whoAmI && P.type == mod.ProjectileType("SpeedBoosterRight"))
						{
							SpeedBoostR = true;
							break;
						}
					}
					if(!SpeedBoostR)
					{
					Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,mod.ProjectileType("SpeedBoosterRight"),250,0,player.whoAmI);
					}
				}
				if(player.velocity.X < -7 && player.controlLeft)
				{
					player.armorEffectDrawShadow = true;
					bool SpeedBoostL = false;
					foreach(Projectile P in Main.projectile)
					{
						if(P.active && P.owner==player.whoAmI && P.type == mod.ProjectileType("SpeedBoosterLeft"))
						{
							SpeedBoostL = true;
							break;
						}
					}
					if(!SpeedBoostL)
					{
					Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,mod.ProjectileType("SpeedBoosterLeft"),250,0,player.whoAmI);
					}
				}
			}
		#region morph ball
		/*	if(player.height == 14)
			{
			if(player.velocity.X < -7 && player.controlLeft || player.velocity.X > 7 && player.controlRight)
			{
				bool SpeedBall = false;
				foreach(Projectile P in Main.projectile)
				{
					if(P.active && P.owner==player.whoAmI && P.type == Config.projDefs.byName["Speed Ball"].type)
					{
						SpeedBall = true;
						break;
					}
				}
				if(!SpeedBall)
				{
				Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,"Speed Ball",250,0,player.whoAmI);
				}
			}
			}*/
		#endregion
		#region shine-spark
			if(player.velocity.X < -7 && player.controlLeft || player.velocity.X > 7 && player.controlRight)
			{
				if(player.controlDown && player.velocity.Y == 0 && !shineCharge)
				{
					shineCharge = true;
					player.velocity.X = 0;
				}
			}
			if(shineCharge)
			{
				shineDeCharge++;
				if(player.controlJump && !player.controlRight && !player.controlLeft && player.statMana > 0)
				{
					shineActive = true;
					if(shineDeActive > 0 && shineDeActive < 2)
					{
						player.position.Y -= 16*player.gravDir;
						Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/ShineSparkChargerelease"), player.position);
					}
				}
				else
				{
					Color color = new Color();
					int dust = Dust.NewDust(player.position, player.width, player.height, 64, 0, 0, 100, color, 2.0f);
					Main.dust[dust].noGravity = true;
				}
			}
			if(shineActive)
			{
				player.velocity.Y = -0.4f*player.gravDir;
				player.velocity.X = 0;
				if(player.controlRight || player.velocity.X > 0)
				{
					player.velocity.X = player.velocity.X - player.runAcceleration;
				}
				if(player.controlLeft || player.velocity.X < 0)
				{
					player.velocity.X = player.velocity.X + player.runAcceleration;
				}
				player.noItems = true;
				player.controlJump = false;
				player.releaseJump = true;
				player.armorEffectDrawShadow = true;
				shineDeActive++;
				if(shineDeActive > 29 && player.statMana > 0)
				{
					player.statMana--;
					player.manaRegenDelay = (int)player.maxRegenDelay;
					if(player.controlRight && !player.controlUp && !player.controlDown) //right
					{
						shineDir = 1;
					}
					if(player.controlRight && player.controlUp) //right and up
					{
						shineDir = 2;
					}
					if(player.controlLeft && !player.controlUp && !player.controlDown) //left
					{
						shineDir = 3;
					}
					if(player.controlLeft && player.controlUp) //left and up
					{
						shineDir = 4;
					}
					if(!player.controlRight && !player.controlLeft) //default direction is up
					{
						shineDir = 5;
					}
				}
			}

			if(shineDeCharge > 299 && !shineActive)
			{
				shineCharge = false;
				shineDeCharge = 0;
			}


			if(shineDir == 1) //right
			{
				player.velocity.X = 16;
				player.velocity.Y = -0.4f*player.gravDir;
				player.direction = 1;
				shineDeActive = 0;
			}
			if(shineDir == 2) //right and up
			{
				player.velocity.X = 16;
				player.velocity.Y = -16*player.gravDir;
				player.direction = 1;
				shineDeActive = 0;
			}
			if(shineDir == 3) //left
			{
				player.velocity.X = -16;
				player.velocity.Y = -0.4f*player.gravDir;
				player.direction = -1;
				shineDeActive = 0;
			}
			if(shineDir == 4) //left and up
			{
				player.velocity.X = -16;
				player.velocity.Y = -16*player.gravDir;
				player.direction = -1;
				shineDeActive = 0;
			}
			if(shineDir == 5) //up
			{
				player.velocity.X = 0;
				player.velocity.Y = -16*player.gravDir;
				shineDeActive = 0;
			}

			if(shineDir != 0)
			{
				manaDelay++;
				if(manaDelay > 2)
				{
					player.statMana--;
					manaDelay = 0;
				}
				player.manaRegenDelay = (int)player.maxRegenDelay;
				shineDeCharge = 300;
				if(player.height == 42)
				{
					bool shineSpark = false;
					foreach(Projectile P in Main.projectile)
					{
						if(P.active && P.owner==player.whoAmI && P.type == mod.ProjectileType("ShineSpark"))
						{
							shineSpark = true;
						}
					}
					if(!shineSpark)
					{
						proj = Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,mod.ProjectileType("ShineSpark"),250,0,player.whoAmI);
					}
				}
				else if(player.height == 14)
				{
					bool shineBall = false;
					foreach(Projectile P in Main.projectile)
					{
						if(P.active && P.owner==player.whoAmI && P.type == mod.ProjectileType("ShineBall"))
						{
							shineBall = true;
							break;
						}
					}
					if(!shineBall)
					{
						proj = Projectile.NewProjectile(player.position.X+player.width/2,player.position.Y+player.height/2,0,0,mod.ProjectileType("ShineBall"),250,0,player.whoAmI);
					}
				}
			}
			
		#region tile functions
			tilev = new Vector2(player.position.X/16, player.position.Y/16);
			TileX = (int)tilev.X;
			TileY = (int)tilev.Y;
			bool collisionTop = (Main.tileSolid[Main.tile[TileX-1,TileY].type] && Main.tile[TileX-1, TileY].active() || Main.tileSolid[Main.tile[TileX+1, TileY].type] && Main.tile[TileX+1, TileY].active());
			bool collisionMiddle = (Main.tileSolid[Main.tile[TileX-1,TileY+1].type] && Main.tile[TileX-1, TileY+1].active() || Main.tileSolid[Main.tile[TileX+1, TileY+1].type] && Main.tile[TileX+1, TileY+1].active());
			bool collisionBottom = (Main.tileSolid[Main.tile[TileX-1,TileY+2].type] && Main.tile[TileX-1, TileY+2].active() || Main.tileSolid[Main.tile[TileX+1, TileY+2].type] && Main.tile[TileX+1, TileY+2].active());
			
			if(shineDir != 0 && (collisionTop || collisionMiddle || collisionBottom))
			{
			//	troubleshoot
			//	Main.NewText("Check!",200,150,100);
			
				KillBlock((int)TileX-1, (int)TileY);
				KillBlock((int)TileX-1, (int)TileY+1);
				KillBlock((int)TileX-1, (int)TileY+2);
				KillBlock((int)TileX+1, (int)TileY);
				KillBlock((int)TileX+1, (int)TileY+1);
				KillBlock((int)TileX+1, (int)TileY+2);
			}
		#endregion
		
		//cancel shine-spark
			//stop movement
			int noMap = Lighting.offScreenTiles * 16 + 16;
			if(shineDir != 0 && (collisionTop || collisionMiddle || collisionBottom)|| player.statMana <= 0 || 
			(player.position.X + player.width) >= (Main.rightWorld - noMap - 16))
			{
				shineDir = 0;
				shineDeActive = 0;
				shineActive = false;
			//	Main.projectile[proj].Kill();
			}
		#endregion
			if(player.height == 42)
			{
				if(((player.velocity.X < -7 && player.controlLeft) || (player.velocity.X > 7 && player.controlRight)) && !shineActive)
				{
					player.head = mod.ItemType("BlueSuitHelmet");
					player.body = mod.ItemType("BlueSuitBreastplate");
					player.legs = mod.ItemType("BlueSuitGreaves");
					Color color = new Color();
					int dust = Dust.NewDust(player.position, player.width, player.height, 59, 0, 0, 100, color, 2.0f);
					Main.dust[dust].noGravity = true;
				}

				if(shineActive || shineDir != 0)
				{
					player.head = mod.ItemType("BlueSuitHelmet");
					player.body = mod.ItemType("BlueSuitBreastplate");
					player.legs = mod.ItemType("BlueSuitGreaves");
					Color color = new Color();
					int dust = Dust.NewDust(player.position, player.width, player.height, 59, 0, 0, 100, color, 2.0f);
					Main.dust[dust].noGravity = true;
				}
			}
		}
		
	/*	public void KillBlock(int x,int y)
		{
			MetroidPlayer.tileTime = 300;
			int type = mod.TileType("CrackedBlock");
			int type2 = mod.TileType("sbBlock");
			WorldGen.KillTile(x, y, false, false, true);
			Projectile.NewProjectile(x,y,0,0,mod.ProjectileType("Crumble"),0,0,Main.myPlayer);
		}	*/
		public void KillBlock(int x,int y)
		{
			int block = mod.TileType("CrackedBlock");
			int block2 = mod.TileType("sbBlock");
			if(Main.tile[x, y].type == block || Main.tile[x, y].type == block2)
			{
				WorldGen.KillTile(x, y,	false, false, true);
			}
			Projectile.NewProjectile(x,y,0,0,mod.ProjectileType("Crumble"),0,0,Main.myPlayer);
		}
		
		public bool CheckLeft(int i, int j, int type, Player player)
		{
			int TileX = i;
			int TileY = j;
			
			bool Active = Main.tile[TileX-1, TileY].active() || Main.tile[TileX-1, TileY+1].active() || Main.tile[TileX-1, TileY +2].active();
			bool Solid = Main.tileSolid[Main.tile[TileX-1, TileY].type] == true || Main.tileSolid[Main.tile[TileX-1, TileY+1].type] == true || Main.tileSolid[Main.tile[TileX-1, TileY+2].type] == true;
			bool Type = Main.tile[TileX-1, TileY].type == type || Main.tile[TileX-1, TileY+1].type == type || Main.tile[TileX-1, TileY+2].type == type;
			
			if(Active && Solid) return true;
			else return false;
		}
		public bool CheckRight(int i, int j, int type, Player player)
		{
			int TileX = i;
			int TileY = j;
			
			bool Active = (Main.tile[TileX+1, TileY].active() || Main.tile[TileX+1, TileY+1].active() || Main.tile[TileX+1, TileY +2].active());
			bool Solid = ((Main.tileSolid[Main.tile[TileX+1, TileY].type] == true) || (Main.tileSolid[Main.tile[TileX+1, TileY+1].type] == true) || (Main.tileSolid[Main.tile[TileX+1, TileY+2].type] == true));
			bool Type = (Main.tile[TileX+1, TileY].type == type || Main.tile[TileX+1, TileY+1].type == type || Main.tile[TileX+1, TileY+2].type == type);
			
			if(Active && Solid) return true;
			else return false;
		} 
	}
}