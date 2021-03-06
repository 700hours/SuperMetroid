using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using SuperMetroid;
using SuperMetroid.Items;
using SuperMetroid.Tiles.Lights;

namespace SuperMetroid
{
	public class MetroidPlayer : ModPlayer
	{
		public static bool
			enabledSuit = false, Respawn, Switched = false,  morph, crit,
		//	(deprecated) to-be modified
			ballstate = false, somersault = false, xrayOn = false, 
			BootsOn = false, PowerArmor = false, VariaSuit = false, GravitySuit = false,
			PowerSuit = true, correctTile;
		public static int
			globalTime = 0, switchTime = 0, tileTime = 0,
			TileSize = 16, RespawnT, immuneT, counter = 0,
		//	(deprecated) to-be modified
			shineDirection = 0, ffTimer = 0, 
			springBall = 0, numPBombs = 10, missileUpg = 10, smissileUpg = 10, reserveTank, MBbombs, pbombUpg = 2,
			variaUpg = 1, gravityUpg = 1, upgradePercent, cbUpg;
		public static double 
			Time;
		public static float 
			ballrot = 0f, rotateCount = 0.075f, rotation = 0.0f, playervX = 0;
	
	//	useful for saving constant variable values
	//	public override TagCompound Save()
	//	{
	//		return new TagCompound {
	//			{"score", missileUpg}
	//		};
	//	}
	//	public override void Load(TagCompound tag)
	//	{
	//		missileUpg = tag.GetInt("score");
	//	}
		
		public override void PreUpdate()
		{
			player.width = 14;
		
		//	timers for queuing things
			if(globalTime >= 0) globalTime++;
			if(globalTime >= 1800) globalTime = 0;
			if(switchTime > 0) switchTime--;
		//	reference tileTime in region tile interactions
		//	deprecated	
			if(ffTimer > 0) ffTimer--; 
		
		// 	player run boost
			bool runBoost	= Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.LeftShift) < 0 && player.velocity.X != 0; 
			if(runBoost)	player.maxRunSpeed += 1.3f;
			
		#region wall-jump
			float PCentreY	= player.position.Y + player.height * 0.5f;
			float PRight 	= player.position.X + player.width;
			int TX 			= (int)(PRight + 1) / TileSize;
			int TY 			= (int)PCentreY / TileSize;
		//!	rebound left
		// 	if tile is solid and active, and P jumps and holds left
			if ((Main.tileSolid[Main.tile[TX, TY].type] && Main.tile[TX, TY].active()) && (player.controlJump && player.controlLeft))
			{
				player.velocity.Y 	= -Player.jumpSpeed * player.gravDir;
				player.jump 		= Player.jumpHeight;
			}
			float PLeft 	= player.position.X;
			int Tx 			= (int)(PLeft - 1) / TileSize;
		//!	rebound right	
		//	if tile is solid and active, and P jumps and holds right
			if ((Main.tileSolid[Main.tile[Tx, TY].type] && Main.tile[Tx, TY].active()) && (player.controlJump && player.controlRight))
			{
				player.velocity.Y 	= -Player.jumpSpeed * player.gravDir;
				player.jump		 	= Player.jumpHeight;
			}
		#endregion
		
		#region morphball
		//	need to make check for morphball upgrade
		//	Main.NewText("Player Height " + player.height, 200,150,100);			
		#endregion
			
		//	kill check
			if(player.statLife <= 0)
			{
				PlayerDeathReason PDR = new PlayerDeathReason();
				PDR = PlayerDeathReason.ByCustomReason(" was fried by the heat");
				player.KillMe(PDR, 1, (int)player.position.X, false);
				RespawnT = 90;
				Respawn = true;
				immuneT = 200;
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Dying"), player.position);
			}
		#region tile states
			Vector2 tilev = new Vector2(player.position.X/16, player.position.Y/16-1);
			int check = mod.TileType("CeresEmptyDoor");
			int check2 = mod.TileType("EmptyShutter");
			int check3 = mod.TileType("EmptyChozoDoor");
			int check4 = mod.TileType("vEmptyDoor");
			bool collision = (check == Main.tile[(int)tilev.X, (int)tilev.Y+1].type) || (check2 == Main.tile[(int)tilev.X, (int)tilev.Y+1].type) || (check3 == Main.tile[(int)tilev.X, (int)tilev.Y+1].type) || (check4 == Main.tile[(int)tilev.X, (int)tilev.Y+1].type);
			if(!collision && tileTime > 0) tileTime--;
		
		//	ceres station door
			int type = mod.TileType("CeresDoor");
		//	open from right
			for(int i = 0; i < 16; i++)
			{
				Tile T = Main.tile[(int)tilev.X-i, (int)tilev.Y+i];
				correctTile = (type == Main.tile[(int)tilev.X-i, (int)tilev.Y+i].type);
				if(correctTile)
				{
					DoorToggle((int)Math.Round(tilev.X-i), (int)Math.Round(tilev.Y+i));
				}
			}
		//	open from left
			for(int i = 0; i < 16; i++)
			{
				Tile T = Main.tile[(int)tilev.X+i, (int)tilev.Y+i];
				correctTile = (type == Main.tile[(int)tilev.X+i, (int)tilev.Y+i].type);
				if(correctTile)
				{
					DoorToggle((int)Math.Round(tilev.X+i), (int)Math.Round(tilev.Y+i));
				}
			}
			
		//	item-extension collision detection
			int type2 = mod.TileType("ChargeBeam");
			correctTile = (type2 == Main.tile[(int)tilev.X, (int)tilev.Y+3].type);
			if(correctTile)
			{
				cbUpg = 1;
				Item.NewItem((int)player.position.X,(int)player.position.Y,32,32,mod.ItemType("ChargeBeam"),1,false);
				upgradePercent++;
				WorldGen.KillTile((int)tilev.X, (int)tilev.Y+3, false, false, true);
				if(ffTimer <= 0)
				{
					ffTimer = 3600; //1 minute
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/ItemFanfare"), player.position);
				}
			}
		//	spikes detection		
			int spikes = mod.TileType("Spikes");
			bool Top = (spikes == Main.tile[(int)tilev.X, (int)tilev.Y+1].type);
			bool Middle = (spikes == Main.tile[(int)tilev.X, (int)tilev.Y+2].type);
			bool Bottom = (spikes == Main.tile[(int)tilev.X, (int)tilev.Y+3].type);
			if(Top || Middle || Bottom)
			{
				PlayerDeathReason PDR = new PlayerDeathReason();
				PDR = PlayerDeathReason.ByCustomReason(" was impaled upon spikes");
				int damage = 12;
				int hitDirection = 0;
				if(Main.rand.Next(-1, 2) == 0) crit = true;
				else crit = false;
				player.Hurt(PDR, damage, hitDirection, false, false, crit, 1);
			}
			
		#endregion
		#region missile launcher
			Item item = player.inventory[player.selectedItem];
			if(item.type == mod.ItemType("MissileLauncher"))
			{
				if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.R) < 0 && switchTime <= 0)
				{
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Click"), player.position);
					switchTime = 60;
					item.shoot = mod.ProjectileType("SuperMissile");
					item.useAmmo = mod.ItemType("SuperMissile");
					item.type = mod.ItemType("SuperMissileLauncher");
				}
			}
			if(item.type == mod.ItemType("SuperMissileLauncher"))
			{
				if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.R) < 0 && switchTime <= 0)
				{
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Click"), player.position);
					switchTime = 60;
					item.shoot = mod.ProjectileType("Missile");
					item.useAmmo = mod.ItemType("Missile");
					item.type = mod.ItemType("MissileLauncher");
				}
			}
		#endregion
	/*	#region accessory item
			if(item.name == "Accessories" && XRayUpg != 1 && grappleUpg != 1 )
			{
				if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.R) < 0 && !Switched)
				{
					Switched = true;
		//			Main.PlaySound(2,-1,-1,SoundHandler.soundID["Denied"]);
				}
			}
			if(item.name == "Accessories" && XRayUpg == 1 && grappleUpg != 1 )
			{
				if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.R) < 0 && !Switched)
				{
					Main.itemTexture[item.type] = Main.goreTexture[Config.goreID["X-Ray Scope"]];
					Switched = true;
					item.name = "X-Ray Scope";
					item.toolTip = "Reveals upgrade locations and block types";
					item.toolTip2 = "Press R to switch accessories";
					item.rare = 2;
					item.damage = 0;
					item.knockBack = 0;
					item.shootSpeed = 0;
					item.useStyle = 0;
					item.useAnimation = 0;
					item.useTime = 0;
					item.channel = false;
					item.noMelee = true;
				}
			}
			if(item.name == "Accessories" && grappleUpg == 1 && XRayUpg != 1)
			{
				if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.R) < 0 && !Switched)
				{
					Main.itemTexture[item.type] = Main.goreTexture[Config.goreID["Grapple Beam"]];
					Main.PlaySound(2,-1,-1,SoundHandler.soundID["Click"]);
					Switched = true;
					item.name = "Grapple Beam";
					item.shoot = Config.projDefs.byName["Grapple Beam"].type;
					item.useSound = SoundHandler.soundID["Grapple Beam"];
					item.toolTip = "Fire it at a grapple block, and then swing with it";
					item.toolTip2 = "You can ascend and descend the grapple by pressing up and down";
					item.toolTip3 = "Press R to switch accessories";
					item.rare = 2;
					item.damage = 10;
					item.knockBack = 3;
					item.shootSpeed = 17;
					item.useStyle = 5;
					item.useAnimation = 5;
					item.useTime = 5;
					item.channel = true;
					item.noMelee = true;
				}
			}
			if(item.name == "Accessories" && XRayUpg == 1 && grappleUpg == 1 )
			{
				if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.R) < 0 && !Switched)
				{
					Main.itemTexture[item.type] = Main.goreTexture[Config.goreID["X-Ray Scope"]];
					Switched = true;
					item.name = "X-Ray Scope";
					item.toolTip = "Reveals upgrade locations and block types";
					item.toolTip2 = "Press R to switch accessories";
					item.rare = 2;
					item.useStyle = 0;
					item.useAnimation = 0;
					item.useTime = 0;
					item.noMelee = true;
				}
			}
		/*	if(item.name == "Grapple Beam" && XRayUpg == 1)
			{
				if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.R) < 0 && !Switched)
				{
					Main.itemTexture[item.type] = Main.goreTexture[Config.goreID["X-Ray Scope"]];
					Switched = true;
					item.name = "X-Ray Scope";
					item.toolTip = "Reveals upgrade locations and block types";
					item.toolTip2 = "Press R to switch accessories";
					item.rare = 2;
					item.useStyle = 0;
					item.useAnimation = 0;
					item.useTime = 0;
					item.noMelee = true;
				}
			}
			if(item.name == "X-Ray Scope" && grappleUpg == 1)
			{
				if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.R) < 0 && !Switched)
				{
					Main.itemTexture[item.type] = Main.goreTexture[Config.goreID["Grapple Beam"]];
					Main.PlaySound(2,-1,-1,SoundHandler.soundID["Click"]);
					Switched = true;
					item.name = "Grapple Beam";
					item.shoot = Config.projDefs.byName["Grapple Beam"].type;
					item.useSound = SoundHandler.soundID["Grapple Beam"];
					item.toolTip = "Fire it at a grapple block, and then swing with it";
					item.toolTip2 = "You can ascend and descend the grapple by pressing up and down";
					item.toolTip3 = "Press R to switch accessories";
					item.rare = 2;
					item.damage = 10;
					item.knockBack = 3;
					item.shootSpeed = 17;
					item.useStyle = 5;
					item.useAnimation = 5;
					item.useTime = 5;
					item.channel = true;
					item.noMelee = true;
				}
			}
		#endregion */
		#region armor
			if(item.type == mod.ItemType("PowerArmor") || item.type == mod.ItemType("VariaArmor") || item.type == mod.ItemType("GravityArmor"))
			{
				PowerSuit = true;
				BootsOn = true;
			}
			if(item.type == mod.ItemType("PowerArmor") && variaUpg == 1)
			{
				if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.R) < 0 && switchTime <= 0)
				{
					VariaSuit = true;
					GravitySuit = false;
					
					switchTime = 45;
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Click"), player.position);
					item.type = mod.ItemType("VariaArmor");
					player.armor[0].SetDefaults(mod.ItemType("Varia Suit Helmet"), false);
					player.armor[1].SetDefaults(mod.ItemType("Varia Suit Breastplate"), false);
					player.armor[2].SetDefaults(mod.ItemType("Varia Suit Greaves"), false);
				}
			}
			if(item.type == mod.ItemType("PowerArmor") && variaUpg != 1 && gravityUpg == 1)
			{
				if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.R) < 0 && switchTime <= 0)
				{
					VariaSuit = false;
					GravitySuit = true;
					
					switchTime = 45;
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Click"), player.position);
					item.type = mod.ItemType("GravityArmor");
					player.armor[0].SetDefaults(mod.ItemType("Gravity Suit Helmet"), false);
					player.armor[1].SetDefaults(mod.ItemType("Gravity Suit Breastplate"), false);
					player.armor[2].SetDefaults(mod.ItemType("Gravity Suit Greaves"), false);
				}
			}
			if(item.type == mod.ItemType("PowerArmor") && variaUpg != 1 && gravityUpg != 1)
			{
				if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.R) < 0 && switchTime <= 0)
				{
					switchTime = 45;
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Click"), player.position);
					player.armor[0].SetDefaults(mod.ItemType("Power Suit Helmet"), false);
					player.armor[1].SetDefaults(mod.ItemType("Power Suit Breastplate"), false);
					player.armor[2].SetDefaults(mod.ItemType("Power Suit Greaves"), false);				
				}
			}
			if(item.type == mod.ItemType("VariaArmor") && gravityUpg == 1)
			{
				if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.R) < 0 && switchTime <= 0)
				{
					VariaSuit = true;
					GravitySuit = true;
					
					switchTime = 45;
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Click"), player.position);
					item.type = mod.ItemType("GravityArmor");
					player.armor[0].SetDefaults(mod.ItemType("Gravity Suit Helmet"), false);
					player.armor[1].SetDefaults(mod.ItemType("Gravity Suit Breastplate"), false);
					player.armor[2].SetDefaults(mod.ItemType("Gravity Suit Greaves"), false);
				}
			}
			if(item.type == mod.ItemType("GravityArmor") || item.type == mod.ItemType("VariaArmor") && gravityUpg != 1)
			{
				if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.R) < 0 && switchTime <= 0)
				{
					VariaSuit = false;
					GravitySuit = false;
					
					switchTime = 45;
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Click"), player.position);
					item.type = mod.ItemType("PowerArmor");
					player.armor[0].SetDefaults(mod.ItemType("Power Suit Helmet"), false);
					player.armor[1].SetDefaults(mod.ItemType("Power Suit Breastplate"), false);
					player.armor[2].SetDefaults(mod.ItemType("Power Suit Greaves"), false);
				}
			}
		#endregion
		#region reserve tank
		/*	if(regenTimer > 0)
			{
				regenTimer--;
			}
			if(item.name == "Reserve Tanks")
			{
				if(AutoReserve == 0 && reserveTank > 0 && Main.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && !Switched)
				{
					Switched = true;
					Main.itemTexture[item.type] = Main.goreTexture[Config.goreID["Reserve Tank A"]];
					AutoReserve = reserveTank;
					Main.NewText("Auto, "+AutoReserve+" Tanks", 248, 126, 0);
					Main.PlaySound(2,-1,-1,SoundHandler.soundID["Click"]);
				}
				if(AutoReserve >= 0 && Main.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && !Switched ||
					reserveTank == 0 && Main.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && !Switched)
				{
					Switched = true;
					Main.itemTexture[item.type] = Main.goreTexture[Config.goreID["Reserve Tank M"]];
					AutoReserve = 0;
					Main.PlaySound(2,-1,-1,SoundHandler.soundID["Click"]);
					Main.NewText("Manual", 248, 126, 0);
				}
			}
			if(AutoReserve > 0 && AutoReserve < 5 && P.statLife < 30 && regen <= 0)
			{
				regenTimer = (AutoReserve*100)+50;
				regen = 1;
			}
			if((float)RegenSound >= 7.68)
			{
				RegenSound = 0;
				Main.PlaySound(2,-1,-1,SoundHandler.soundID["Refill"]);
			}
			if(regen == 1)
			{
				RegenSound++;
				Main.player[Main.myPlayer].AddBuff(Config.buffID["Energy Refill"], 1, false);
			}
			if(P.statLife >= AutoReserve*100 && regen == 1 && regenTimer <= 0)
			{
				reserveTank = 0;
				AutoReserve = 0;
				regen = 0;	
			}
			if(P.statLife == P.statLifeMax)
			{
				reserveTank = maxreserveTank;
			}
			if(Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.B) < 0 && Main.GetKeyState((int)Microsoft.Xna.Framework.Input.Keys.V) < 0 && reserveTank > 0 && AutoReserve == 0 && P.statLife < P.statLifeMax/2)
			{
				Main.PlaySound(2,-1,-1,SoundHandler.soundID["Reserve Tank"]);
				P.statLife += reserveTank*100;
				reserveTank = 0;
			}
		#endregion
		#region xray
			soundLoop++;
			SoundStart++;
			if(item.name == "X-Ray Scope"){
				if(Main.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && XRayUpg == 1)
				{
					P.nightVision = true;
					xrayOn = true;
					
					if((float)soundLoop >= 40.92)
					{
						Main.PlaySound(2,-1,-1,SoundHandler.soundID["XRay"]);
						soundLoop = 0;
					}
					if(SoundStart > 1 && SoundStart < 5)
					{
						Main.PlaySound(2,-1,-1,SoundHandler.soundID["XRay Start"]);
						SoundStart = 4;
						if(SoundStart > 6)
						{
							SoundStart = 6;
						}
					}
				}
				if(Main.mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && XRayUpg == 1)
				{
					xrayOn = false;
				}
			}
		#endregion */
		#endregion
		
		//	door transition
			for(int i = (int)(player.position.X)/16; i < (int)(player.position.X+player.width)/16; i++)
			{
				for(int j = (int)(player.position.Y)/16; j < (int)(player.position.Y+player.height)/16; j++)
				{
					if(Main.tile[i, j].type == mod.TileType("DoorCorridor") && player.velocity.X > 0)
					{
						player.position.X += 16;
					}
					if(Main.tile[i, j].type == mod.TileType("DoorCorridor") && player.velocity.X < 0)
					{
						player.position.X -= 16;
					}
				}
			}
		}
		
		public void DoorToggle(int i, int j)
		{
			tileTime = 300;
			int type = mod.TileType("CeresDoor"); 
			int type2 = mod.TileType("CeresEmptyDoor");
			if(Main.tile[i, j].type == (ushort)type) Main.tile[i, j].type = (ushort)type2;
		}
		
		public static bool TileCollision(int x,int y,int Radius,int Type)
		{
			for(int i = x - Radius; i < x + Radius; i++){
			for(int j = y - Radius; j < y + Radius; j++){
			Vector2 Position = new Vector2(i, j);
			
			if(Main.tile[(int)Position.X, (int)Position.Y].active() && Main.tile[(int)Position.X/16, (int)Position.Y/16].type == (ushort)Type) return true;
					else return false;
				}
			}
			return false;
		}
		
		public override void DrawEffects(PlayerDrawInfo drawinfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			//!	armor draw code
			#region
	/*	//	drawinfo	
			PlayerHeadDrawInfo drawInfo = new PlayerHeadDrawInfo();
			drawInfo.spriteBatch = Main.spriteBatch;
			drawInfo.drawPlayer = player;
		//	draw variables
			SpriteEffects effects = SpriteEffects.None;
			Vector2 origin = new Vector2((float)player.legFrame.Width * 0.5f, (float)player.legFrame.Height * 0.5f);
			Color color = player.GetImmuneAlpha(Lighting.GetColor((int)((double)player.position.X + (double)player.width * 0.5) / 16, (int)(((double)player.position.Y + (double)player.height * 0.25) / 16.0), Color.White), 0f);
			//	legs
				Vector2 legsv = new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f)));
		//	textures
			Texture2D powerArmor = mod.GetTexture("Gores/Armor/PowerSuitBody");
			Texture2D powerLegs = mod.GetTexture("Gores/Armor/PowerSuitLegs");
		//	draw
			if(PowerSuit)
			{
				Main.spriteBatch.Draw(powerArmor, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), default(Color), rotation, origin, 1f, effects, 0f);
				Main.spriteBatch.Draw(powerLegs, legsv + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2)), new Rectangle?(player.legFrame), default(Color), 0f, origin, 1f, effects, 0f);
			}	*/
			#endregion
			//!	Yoraiz0r's morphball draw code
			//	Using it as a reference
			#region 
			/*
				Texture2D mytex = mod.GetTexture("Gores/Morphball");
				Texture2D mytex2 = mod.GetTexture("Gores/MorphballLight");

				if(player.active) 
	{	
				PlayerHeadDrawInfo drawInfo = new PlayerHeadDrawInfo();
				drawInfo.spriteBatch = Main.spriteBatch;
				drawInfo.drawPlayer = player;
				
					if(Morphball.morph && !player.dead)
					{
						float thisx = (float)((int)(player.position.X - Main.screenPosition.X + (float)(player.width / 2)));
						float thisy = (float)((int)(player.position.Y - Main.screenPosition.Y + (float)(player.height / 2)));
						Vector2 ballDims = new Vector2((float)(player.width),(float)(player.height));
						Vector2 thispos =  new Vector2(thisx,thisy) - ballDims/2;
					//	int timez = (int)(Main.time%60)/10;
						Time += 1.0;
						if(Time > 54000.0)
						{
							Time = 0;
						}
						int timez = (int)(Time%60)/10;
							SpriteEffects effects = SpriteEffects.None;
							if (player.direction == -1)
							{
								effects = SpriteEffects.FlipHorizontally;
							}
						float ballrotoffset = 0f;
						if(player.velocity.Y != Vector2.Zero.Y)
						{
							if(player.velocity.X != 0f)
							ballrotoffset+= 0.05f*player.velocity.X;
							else
							ballrotoffset += 0.25f*player.direction;
						}
						else if (player.velocity.X < 0f)
							ballrotoffset -= 0.2f;
						else if ( player.velocity.X > 0f)
							ballrotoffset += 0.2f;
						
						if(player.velocity.X != 0f)
							ballrotoffset+= 0.05f*player.velocity.X;
						else
							ballrotoffset += 0.25f*player.direction;
						
						ballrot+=ballrotoffset;
						if(ballrot > (float)(Math.PI)*2)
							ballrot -= (float)(Math.PI)*2;
						if(ballrot < -(float)(Math.PI)*2)
							ballrot += (float)(Math.PI)*2;
						//	Main.spriteBatch.Draw(Main.teamTexture,thispos, new Rectangle?(new Rectangle(0, 0, Main.teamTexture.Width, Main.teamTexture.Height)), Main.teamColor[1], 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
						Color brightColor = (player.shirtColor.R+player.shirtColor.G+player.shirtColor.B >= player.underShirtColor.R+player.underShirtColor.G+player.underShirtColor.B)?player.shirtColor:player.underShirtColor;
						Color darkColor = (player.shirtColor.R+player.shirtColor.G+player.shirtColor.B < player.underShirtColor.R+player.underShirtColor.G+player.underShirtColor.B)?player.shirtColor:player.underShirtColor;
						//	Color blueColor = new Color();
						for (int d = 3; d < 8; d++)
						{
							if(shineDirection != 0)
							{
								Main.spriteBatch.Draw(mytex, thispos+ballDims/2, new Rectangle?(new Rectangle(0,((int)ballDims.Y+2)*timez,(int)ballDims.X, (int)ballDims.Y)), Color.Yellow,ballrot,ballDims/2, 1.14f, effects, 0f);
								Main.spriteBatch.Draw(mytex2, thispos+ballDims/2, new Rectangle?(new Rectangle(0,((int)ballDims.Y+2)*timez,(int)ballDims.X, (int)ballDims.Y)), Color.Red,ballrot,ballDims/2, 1.14f, effects, 0f);
							}
							else
							{
								if(((player.velocity.X < -7 && player.controlLeft) || (player.velocity.X > 7 && player.controlRight)) && shineDirection == 0 && player.armor[d].type == mod.ItemType("Speed Booster"))
								{
									Main.spriteBatch.Draw(mytex, thispos+ballDims/2, new Rectangle?(new Rectangle(0,((int)ballDims.Y+2)*timez,(int)ballDims.X, (int)ballDims.Y)), Color.Blue,ballrot,ballDims/2, 1.14f, effects, 0f);
									Main.spriteBatch.Draw(mytex2, thispos+ballDims/2, new Rectangle?(new Rectangle(0,((int)ballDims.Y+2)*timez,(int)ballDims.X, (int)ballDims.Y)), Color.Green,ballrot,ballDims/2, 1.14f, effects, 0f);
								}
								else
								{
									Main.spriteBatch.Draw(mytex, thispos+ballDims/2, new Rectangle?(new Rectangle(0,((int)ballDims.Y+2)*timez,(int)ballDims.X, (int)ballDims.Y)), darkColor,ballrot,ballDims/2, 1.14f, effects, 0f);
									Main.spriteBatch.Draw(mytex2, thispos+ballDims/2, new Rectangle?(new Rectangle(0,((int)ballDims.Y+2)*timez,(int)ballDims.X, (int)ballDims.Y)), brightColor,ballrot,ballDims/2, 1.14f, effects, 0f);
								}
							}
						}
					}
				} */
		#endregion
			//! ScooterBoot's somersault draw code
			//	Using it as a reference
			#region
		/*	somersault = (player.velocity.Y != 0 && player.velocity.X != 0 && player.itemAnimation == 0 && player.releaseHook && player.grapCount == 0 && shineDirection == 0 && player.height == 42f);
			if(somersault && !player.dead)
			{
				PlayerHeadDrawInfo drawInfo = new PlayerHeadDrawInfo();
				drawInfo.spriteBatch = Main.spriteBatch;
				drawInfo.drawPlayer = player;
				
				rotation += rotateCount * 2 * player.direction;
				SpriteEffects effects = SpriteEffects.None;
				Color color = player.GetImmuneAlpha(Lighting.GetColor((int)((double)player.position.X + (double)player.width * 0.5) / 16, (int)(((double)player.position.Y + (double)player.height * 0.25) / 16.0), Color.White), 0f);
				Color color2 = player.GetImmuneAlpha(Lighting.GetColor((int)((double)player.position.X + (double)player.width * 0.5) / 16, (int)(((double)player.position.Y + (double)player.height * 0.25) / 16.0), player.eyeColor), 0f);
				Color color3 = player.GetImmuneAlpha(Lighting.GetColor((int)((double)player.position.X + (double)player.width * 0.5) / 16, (int)(((double)player.position.Y + (double)player.height * 0.25) / 16.0), player.hairColor), 0f);
				Color color4 = player.GetImmuneAlpha(Lighting.GetColor((int)((double)player.position.X + (double)player.width * 0.5) / 16, (int)(((double)player.position.Y + (double)player.height * 0.25) / 16.0), player.skinColor), 0f);
				Color color5 = player.GetImmuneAlpha(Lighting.GetColor((int)((double)player.position.X + (double)player.width * 0.5) / 16, (int)(((double)player.position.Y + (double)player.height * 0.5) / 16.0), player.skinColor), 0f);
				player.GetImmuneAlpha(Lighting.GetColor((int)((double)player.position.X + (double)player.width * 0.5) / 16, (int)(((double)player.position.Y + (double)player.height * 0.75) / 16.0), player.skinColor), 0f);
				Color color6 = player.GetImmuneAlphaPure(Lighting.GetColor((int)((double)player.position.X + (double)player.width * 0.5) / 16, (int)(((double)player.position.Y + (double)player.height * 0.5) / 16.0), player.shirtColor), 0f);
				Color color7 = player.GetImmuneAlphaPure(Lighting.GetColor((int)((double)player.position.X + (double)player.width * 0.5) / 16, (int)(((double)player.position.Y + (double)player.height * 0.5) / 16.0), player.underShirtColor), 0f);
				Color color8 = player.GetImmuneAlphaPure(Lighting.GetColor((int)((double)player.position.X + (double)player.width * 0.5) / 16, (int)(((double)player.position.Y + (double)player.height * 0.75) / 16.0), player.pantsColor), 0f);
				Color color9 = player.GetImmuneAlphaPure(Lighting.GetColor((int)((double)player.position.X + (double)player.width * 0.5) / 16, (int)(((double)player.position.Y + (double)player.height * 0.75) / 16.0), player.shoeColor), 0f);
				Color color10 = player.GetImmuneAlphaPure(Lighting.GetColor((int)((double)player.position.X + (double)player.width * 0.5) / 16, (int)((double)player.position.Y + (double)player.height * 0.25) / 16, Color.White), 0f);
				Color color11 = player.GetImmuneAlphaPure(Lighting.GetColor((int)((double)player.position.X + (double)player.width * 0.5) / 16, (int)((double)player.position.Y + (double)player.height * 0.5) / 16, Color.White), 0f);
				Color color12 = player.GetImmuneAlphaPure(Lighting.GetColor((int)((double)player.position.X + (double)player.width * 0.5) / 16, (int)((double)player.position.Y + (double)player.height * 0.75) / 16, Color.White), 0f);
				Vector2 origin = new Vector2((float)player.legFrame.Width * 0.5f, (float)player.legFrame.Height * 0.5f);
				if (player.gravDir == 1f)
				{
					if (player.direction == 1)
					{
						effects = SpriteEffects.None;
					}
					else
					{
						effects = SpriteEffects.FlipHorizontally;
					}
					if (!player.dead)
					{
						player.legPosition.Y = 0f;
						player.headPosition.Y = 0f;
						player.bodyPosition.Y = 0f;
					}
				}
				else
				{
					if (player.direction == 1)
					{
						effects = SpriteEffects.FlipVertically;
					}
					else
					{
						effects = (SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);
					}
					if (!player.dead)
					{
						player.legPosition.Y = 6f;
						player.headPosition.Y = 6f;
						player.bodyPosition.Y = 6f;
					}
				}
			//	wings
				if (player.wings > 0)
				{
					Main.spriteBatch.Draw(Main.wingsTexture[player.wings], new Vector2((float)((int)(player.position.X - Main.screenPosition.X + (float)(player.width / 2) - (float)(9 * player.direction))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)(player.height / 2) + 2f * player.gravDir))), new Rectangle?(new Rectangle(0, Main.wingsTexture[player.wings].Height / 4 * player.wingFrame, Main.wingsTexture[player.wings].Width, Main.wingsTexture[player.wings].Height / 4)), color11, rotation, new Vector2((float)(Main.wingsTexture[player.wings].Width / 2), (float)(Main.wingsTexture[player.wings].Height / 8)), 1f, effects, 0f);
				}
				//legs
				if (Main.armorLegTexture[player.legs] != null)
				{
					Main.spriteBatch.Draw(Main.armorLegTexture[player.legs], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.legFrame), color12, rotation, origin, 1f, effects, 0f);
				}
				else
				{
					if (!player.invis)
					{
						if (!player.Male)
						{
					//		Main.spriteBatch.Draw(Main.femalePantsTexture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.legFrame), color8, rotation, origin, 1f, effects, 0f);
					//		Main.spriteBatch.Draw(Main.femaleShoesTexture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.legFrame), color9, rotation, origin, 1f, effects, 0f);
						}
						else
						{
					//		Main.spriteBatch.Draw(Main.playerPantsTexture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.legFrame), color8, rotation, origin, 1f, effects, 0f);
					//		Main.spriteBatch.Draw(Main.playerShoesTexture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.legFrame), color9, rotation, origin, 1f, effects, 0f);
						}
					}
				}
				//body
				if (Main.armorBodyTexture[player.body] != null)
				{
					if (!player.Male)
					{
						Main.spriteBatch.Draw(Main.femaleBodyTexture[player.body], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color11, rotation, origin, 1f, effects, 0f);
					}
					else
					{
						Main.spriteBatch.Draw(Main.armorBodyTexture[player.body], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color11, rotation, origin, 1f, effects, 0f);
					}
				//	if ((player.body == 10 || player.body == 11 || player.body == 12 || player.body == 13 || player.body == 14 || player.body == 15 || player.body == 16 || player.body == 20) && !player.invis)
				//	{
				//		Main.spriteBatch.Draw(Main.playerHandsTexture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color5, rotation, origin, 1f, effects, 0f);
				//	}
				}
				else
				{
					if (!player.invis)
					{
						if (!player.Male)
						{
					//		Main.spriteBatch.Draw(Main.femaleUnderShirtTexture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color7, rotation, origin, 1f, effects, 0f);
					//		Main.spriteBatch.Draw(Main.femaleShirtTexture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color6, rotation, origin, 1f, effects, 0f);
						}
						else
						{
					//		Main.spriteBatch.Draw(Main.playerUnderShirtTexture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color7, rotation, origin, 1f, effects, 0f);
					//		Main.spriteBatch.Draw(Main.playerShirtTexture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color6, rotation, origin, 1f, effects, 0f);
						}
					//	Main.spriteBatch.Draw(Main.playerHandsTexture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color5, rotation, origin, 1f, effects, 0f);
					}
				}
				//head
				if (!player.invis && player.head != 38)
				{
				//	Main.spriteBatch.Draw(Main.armorHeadTexture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color4, rotation, origin, 1f, effects, 0f);
				//	Main.spriteBatch.Draw(Main.playerEyeWhitesTexture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color, rotation, origin, 1f, effects, 0f);
				//	Main.spriteBatch.Draw(Main.playerEyesTexture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color2, rotation, origin, 1f, effects, 0f);
				}
				if (player.head == 10 || player.head == 12 || player.head == 28)
				{
					Main.spriteBatch.Draw(Main.armorHeadTexture[player.head], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color10, rotation, origin, 1f, effects, 0f);
					if (!player.invis)
					{
						Rectangle bodyFrame = player.bodyFrame;
						bodyFrame.Y -= 336;
						if (bodyFrame.Y < 0)
						{
							bodyFrame.Y = 0;
						}
						Main.spriteBatch.Draw(Main.playerHairTexture[player.hair], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(bodyFrame), color3, rotation, origin, 1f, effects, 0f);
					}
				}
				if (player.head == 14 || player.head == 15 || player.head == 16 || player.head == 18 || player.head == 21 || player.head == 24 || player.head == 25 || player.head == 26 || player.head == 40 || player.head == 44)
				{
					Rectangle bodyFrame2 = player.bodyFrame;
					bodyFrame2.Y -= 336;
					if (bodyFrame2.Y < 0)
					{
						bodyFrame2.Y = 0;
					}
					if (!player.invis)
					{
						Main.spriteBatch.Draw(Main.playerHairAltTexture[player.hair], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(bodyFrame2), color3, rotation, origin, 1f, effects, 0f);
					}
				}
				if (player.head == 23)
				{
					Rectangle bodyFrame3 = player.bodyFrame;
					bodyFrame3.Y -= 336;
					if (bodyFrame3.Y < 0)
					{
						bodyFrame3.Y = 0;
					}
					if (!player.invis)
					{
						Main.spriteBatch.Draw(Main.playerHairTexture[player.hair], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(bodyFrame3), color3, rotation, origin, 1f, effects, 0f);
					}
					Main.spriteBatch.Draw(Main.armorHeadTexture[player.head], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color10, rotation, origin, 1f, effects, 0f);
				}
				else
				{
					if (player.head == 14)
					{
						Rectangle bodyFrame4 = player.bodyFrame;
						int num9 = 0;
						if (bodyFrame4.Y == bodyFrame4.Height * 6)
						{
							bodyFrame4.Height -= 2;
						}
						else
						{
							if (bodyFrame4.Y == bodyFrame4.Height * 7)
							{
								num9 = -2;
							}
							else
							{
								if (bodyFrame4.Y == bodyFrame4.Height * 8)
								{
									num9 = -2;
								}
								else
								{
									if (bodyFrame4.Y == bodyFrame4.Height * 9)
									{
										num9 = -2;
									}
									else
									{
										if (bodyFrame4.Y == bodyFrame4.Height * 10)
										{
											num9 = -2;
										}
										else
										{
											if (bodyFrame4.Y == bodyFrame4.Height * 13)
											{
												bodyFrame4.Height -= 2;
											}
											else
											{
												if (bodyFrame4.Y == bodyFrame4.Height * 14)
												{
													num9 = -2;
												}
												else
												{
													if (bodyFrame4.Y == bodyFrame4.Height * 15)
													{
														num9 = -2;
													}
													else
													{
														if (bodyFrame4.Y == bodyFrame4.Height * 16)
														{
															num9 = -2;
														}
													}
												}
											}
										}
									}
								}
							}
						}
						bodyFrame4.Y += num9;
						Main.spriteBatch.Draw(Main.armorHeadTexture[player.head], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(bodyFrame4), color10, rotation, origin, 1f, effects, 0f);
					}
					else
					{
						if ((Main.armorHeadTexture[player.head] != null || (player.head > 0 && player.head < 45)) && player.head != 28)
						{
							if (!player.invis)
							{
								Rectangle bodyFrame5 = player.bodyFrame;
								bodyFrame5.Y -= 336;
								if (bodyFrame5.Y < 0)
								{
									bodyFrame5.Y = 0;
								}
							//	if (Config.drawHairAlt[player.head])
							//	{
									Main.spriteBatch.Draw(Main.playerHairAltTexture[player.hair], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(bodyFrame5), color3, rotation, origin, 1f, effects, 0f);
							//	}
							//	else
							//	{
									Main.spriteBatch.Draw(Main.playerHairTexture[player.hair], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(bodyFrame5), color3, rotation, origin, 1f, effects, 0f);
							//	}
							}
							Main.spriteBatch.Draw(Main.armorHeadTexture[player.head], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color10, rotation, origin, 1f, effects, 0f);
							if (!player.invis)
							{
								Rectangle bodyFrame6 = player.bodyFrame;
								bodyFrame6.Y -= 336;
								if (bodyFrame6.Y < 0)
								{
									bodyFrame6.Y = 0;
								}
							//	if (Config.drawHairAlt[player.head])
							//	{
									Main.spriteBatch.Draw(Main.playerHairAltTexture[player.hair], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(bodyFrame6), color3, rotation, origin, 1f, effects, 0f);
							//	}
							//	else
							//	{
									Main.spriteBatch.Draw(Main.playerHairTexture[player.hair], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(bodyFrame6), color3, rotation, origin, 1f, effects, 0f);
							//	}
							}
						}
						else
						{
							if (!player.invis)
							{
								Rectangle bodyFrame7 = player.bodyFrame;
								bodyFrame7.Y -= 336;
								if (bodyFrame7.Y < 0)
								{
									bodyFrame7.Y = 0;
								}
								Main.spriteBatch.Draw(Main.playerHairTexture[player.hair], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(bodyFrame7), color3, rotation, origin, 1f, effects, 0f);
							}
						}
					}
				}
				//arms
				if (Main.armorArmTexture[player.body] != null)
				{
					Main.spriteBatch.Draw(Main.armorArmTexture[player.body], new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color11, rotation, origin, 1f, effects, 0f);
				//	if ((Config.hasHands[player.body] || player.body == 10 || player.body == 11 || player.body == 12 || player.body == 13 || player.body == 14 || player.body == 15 || player.body == 16 || player.body == 20) && !player.invis)
				//	{
				//		Main.spriteBatch.Draw(Main.playerHands2Texture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color5, rotation, origin, 1f, effects, 0f);
				//	}
				}
				else
				{
					if (!player.invis)
					{
						if (!player.Male)
						{
					//		Main.spriteBatch.Draw(Main.femaleUnderShirt2Texture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color7, rotation, origin, 1f, effects, 0f);
					//		Main.spriteBatch.Draw(Main.femaleShirt2Texture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color6, rotation, origin, 1f, effects, 0f);
						}
						else
						{
					//		Main.spriteBatch.Draw(Main.playerUnderShirt2Texture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color7, rotation, origin, 1f, effects, 0f);
						}
					//	Main.spriteBatch.Draw(Main.playerHands2Texture, new Vector2((float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2)), new Rectangle?(player.bodyFrame), color5, rotation, origin, 1f, effects, 0f);
					}
				}
		 
			//	LetDraw = false;
			//	return false;
				player.invis = true;
			}
			else
			{
				rotation = 0.0f;
				player.invis = false;
			//	player.head = 1;
			//	player.body = 1;
			//	player.legs = 1;
			//	player.wings = 1;
			//	LetDraw = true;
			//	return true;
			} */
			#endregion
		}
	}
}