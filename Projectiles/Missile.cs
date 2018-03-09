using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperMetroid;
using SuperMetroid.Tiles.Shutters;

namespace SuperMetroid.Projectiles
{
	public class Missile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Missile");
		}
		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 15;
			projectile.aiStyle = 1;
			projectile.timeLeft = 600;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1.5f;
			projectile.ranged = true;
			projectile.netUpdate = true;
		}
		int counter = 0;
		bool soundInit = false;
		
		public override void AI()
		{
			if(!soundInit) 
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Missile"), projectile.position);
				soundInit = true;
			}
			
		#region tile functions
			Vector2 tilev = new Vector2(projectile.position.X/16, projectile.position.Y/16);
			
		//	blocks	
			int type = mod.TileType("CrackedBlock");
			int type8 = mod.TileType("UpgradeBall");
		//	doors
			int type2 = mod.TileType("BlueSwitchleft");
			int type3 = mod.TileType("BlueSwitchright");
			int type4 = mod.TileType("vBlueDoor");
			int type5 = mod.TileType("ChozoDoor");
			int type6 = mod.TileType("MissileDoor");
			int type7 = mod.TileType("vMissileDoor");
			
			Tile T = Main.tile[(int)tilev.X, (int)tilev.Y];
			var modPlayer = Main.player[projectile.owner].GetModPlayer<MetroidPlayer>(mod);
			
		//  collision	
			bool collision = Main.tile[(int)tilev.X, (int)tilev.Y].active() && (Main.tileSolid[T.type] == true);
			bool correctTile = (type == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			bool blueSwitch = (type2 == Main.tile[(int)tilev.X, (int)tilev.Y].type) || (type3 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			bool chozoDoor = (type4 == Main.tile[(int)tilev.X, (int)tilev.Y].type) || (type5 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			bool missileDoor = (type6 == Main.tile[(int)tilev.X, (int)tilev.Y].type) || (type7 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			bool ball = (type8 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			if(collision)
			{
				PreKill(0);
				Kill(0);
				if(correctTile)
				{
					KillBlock((int)tilev.X, (int)tilev.Y);
				}
				if(blueSwitch)
				{
					ShutterSwitch((int)tilev.X, (int)tilev.Y);
				}
				if(chozoDoor)
				{
					DoorToggle((int)tilev.X, (int)tilev.Y);
				}
				if(missileDoor)
				{
					modPlayer.counter++;
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/MDoorHit"), projectile.position);
					if(modPlayer.counter >= 5)
					{
						DoorToggle((int)tilev.X, (int)tilev.Y);
						modPlayer.counter = 0;
					}
				}
				if(ball)
				{
					DropRandomUpgrade((int)tilev.X, (int)tilev.Y, Main.player[projectile.owner]);
				}
				projectile.active = false;
			}
		#endregion
		
			Color color = new Color();
			int dust = Dust.NewDust(new Vector2((float) projectile.position.X, (float) projectile.position.Y), projectile.width, projectile.height, 6, 0, 0, 100, color, 2.0f);
			Main.dust[dust].noGravity = true;
			
		//	Gold Torizo immunity check
		/*	foreach(NPC N in Main.npc)
			{
				if(!N.active) continue;
				if(N.life <= 0) continue;
				if(N.friendly) continue;
				if(N.dontTakeDamage) continue;
				Rectangle projrec = new Rectangle((int)projectile.position.X+(int)projectile.velocity.X, (int)projectile.position.Y+(int)projectile.velocity.Y, projectile.width, projectile.height);
				Rectangle nrec = new Rectangle((int)N.position.X, (int)N.position.Y, (int)N.width,(int)N.height);
				if (projrec.Intersects(nrec))
				{
					if(N.type == mod.NPCType("GoldTorizo"))
					{
						projectile.damage = 0;
					}
					else
					{ 	projectile.damage = 20;		}
				}
			} */
		}

		public override bool PreKill(int timeleft)
		{
			Main.PlaySound(mod.GetSoundSlot(SoundType.Item, "Sounds/Item/MBurst"), projectile.position);
			return true;
		}
		float Radius = 12f;
		public override void Kill(int timeleft)
		{
			for (int num70 = 0; num70 < 25; num70++)
			{
				int num71 = Dust.NewDust(new Vector2(this.projectile.position.X-Radius, this.projectile.position.Y-Radius), this.projectile.width+(int)Radius*2, this.projectile.height+(int)Radius*2, 6, 0f, 0f, 100, default(Color), 4f);
				Main.dust[num71].velocity *= 1.4f;
				Main.dust[num71].noGravity = true;
				int num72 = Dust.NewDust(new Vector2(this.projectile.position.X-Radius, this.projectile.position.Y-Radius), this.projectile.width+(int)Radius*2, this.projectile.height+(int)Radius*2, 1, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num72].velocity *= 1.4f;
				Main.dust[num72].noGravity = true;
			}
		}
	#region tile interaction
		public void KillBlock(int x,int y)
		{
			WorldGen.KillTile(x, y,	false, false, true);
			Projectile.NewProjectile(x,y,0,0,mod.ProjectileType("Crumble"),0,0,Main.myPlayer);
		}
		public void ShutterSwitch(int x,int y)
		{
			var modPlayer = Main.player[projectile.owner].GetModPlayer<MetroidPlayer>(mod);
			modPlayer.tileTime = 300;
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/ShutterDoor"), projectile.position);
			int type = mod.TileType("EmptyShutter");
			if(Main.tile[x, y].type == mod.TileType("BlueSwitchleft"))
			{
				for(int i = 0; i < 4; i++) Main.tile[x+1, (y+1)+i].type = (ushort)type;
			}
			if(Main.tile[x, y].type == mod.TileType("BlueSwitchright"))
			{
				for(int i = 0; i < 4; i++) Main.tile[x-1, (y+1)+i].type = (ushort)type;
			}
		}
		public void DoorToggle(int x, int y)
		{
			var modPlayer = Main.player[projectile.owner].GetModPlayer<MetroidPlayer>(mod);
		
		//	to open entire door at once
			Wiring.TripWire(x, y, 1, 1);
				
			modPlayer.tileTime = 300;
			
			int type = mod.TileType("vBlueDoor");
			int type2 = mod.TileType("ChozoDoor");
			int type3 = mod.TileType("MissileDoor");
			int type4 = mod.TileType("vMissileDoor");
			int transform = mod.TileType("vEmptyDoor");
			int transform2 = mod.TileType("ChozoDoorOpening");
			int transform3 = mod.TileType("EmptyChozoDoor");
						
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/DoorOpening"), projectile.position);
			
			if(Main.tile[x, y].type == (ushort)type || Main.tile[x, y].type == (ushort)type4) Main.tile[x, y].type = (ushort)transform;
			if(Main.tile[x, y].type == (ushort)type2) Main.tile[x, y].type = (ushort)transform3;
			if(Main.tile[x, y].type == (ushort)type3) Main.tile[x, y].type = (ushort)transform3;
		}
		
		string[] ItemArray = new string[] 
			{	"ChargeBeam" , "IceBeam" , "WaveBeam" , "PlasmaBeam" , "Spazer" , 
				"HiJump" , "ScrewAttack" , "SpaceJump" , "SpeedBooster" , "XRayScope" 
				"VariaSuit" , "GravitySuit" , 
				"Missile" , "SuperMissile" , "EnergyTank" , "ReserveTank" };
		string RandomItem;
		public void DropRandomUpgrade(int x, int y, Player player)
		{
			var modPlayer = player.GetModPlayer<MetroidPlayer>(mod);
			
			RandomItem = ItemArray[Main.rand.Next(0, ItemArray.Length - 1)];
			if(RandomItem != "VariaSuit" && RandomItem != "GravitySuit" && RandomItem != "Missile" && RandomItem != "SuperMissile" && RandomItem != "EnergyTank" && RandomItem != "ReserveTank")
			{
				if(RandomItem == "ChargeBeam" && !modPlayer.haveCharge)
				{
					Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType(RandomItem), 1, false, -1, true, false);
					modPlayer.haveCharge = true;
				}
				if(RandomItem == "IceBeam" && !modPlayer.haveIce)
				{
					Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType(RandomItem), 1, false, -1, true, false);
					modPlayer.haveIce = true;
				}
				if(RandomItem == "WaveBeam" && !modPlayer.haveWave)
				{
					Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType(RandomItem), 1, false, -1, true, false);
					modPlayer.haveWave = true;
				}
				if(RandomItem == "Spazer" && !modPlayer.haveSpazer)
				{
					Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType(RandomItem), 1, false, -1, true, false);
					modPlayer.haveSpazer = true;
				}
				if(RandomItem == "PlasmaBeam" && !modPlayer.havePlasma)
				{
					Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType(RandomItem), 1, false, -1, true, false);
					modPlayer.havePlasma = true;
				}
				if(RandomItem == "SpeedBooster" && !modPlayer.haveSpeed)
				{
					Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType(RandomItem), 1, false, -1, true, false);
					modPlayer.haveSpeed = true;
				}
				if(RandomItem == "HiJump" && !modPlayer.haveJump)
				{
					Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType(RandomItem), 1, false, -1, true, false);
					modPlayer.haveJump = true;
				}
				if(RandomItem == "ScrewAttack" && !modPlayer.haveScrew)
				{
					Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType(RandomItem), 1, false, -1, true, false);
					modPlayer.haveScrew = true;
				}
				if(RandomItem == "SpaceJump" && !modPlayer.haveSpaceJump)
				{
					Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType(RandomItem), 1, false, -1, true, false);
					modPlayer.haveSpaceJump = true;
				}
				if(RandomItem == "XRayScope" && !modPlayer.haveScope)
				{
					Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType(RandomItem), 1, false, -1, true, false);
					modPlayer.haveSpaceJump = true;
				}
				if(RandomItem == "GrappleBeam" && !modPlayer.haveGrapple)
				{
					Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType(RandomItem), 1, false, -1, true, false);
					modPlayer.haveGrapple = true;
				}
			}
			else 
			{
				if(RandomItem == "VariaSuit" && modPlayer.variaUpg != 1)
				{
					modPlayer.variaUpg = 1;
				}
				if(RandomItem == "GravitySuit" && modPlayer.gravityUpg != 1)
				{
					modPlayer.gravityUpg = 1;
				}
				if(RandomItem == "Missile" && modPlayer.missileUpg < 50)
				{
					Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType("Missile"), 5, false, -1, true, false);
					modPlayer.missileUpg += 1;
				}
				if(RandomItem == "SuperMissile" && modPlayer.smissileUpg < 10)
				{
					Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType("SuperMissile"), 5, false, -1, true, false);
					modPlayer.smissileUpg += 1;
				}
				if(RandomItem == "EnergyTank" && player.statLifeMax < 400)
				{
					player.statLifeMax += 20;
					player.statLife += 20;
				}
				if(RandomItem == "ReserveTank" && modPlayer.reserveTank < 5)
				{
					modPlayer.reserveTank += 1;
				}
			}
			WorldGen.KillTile(x, y,	false, false, true);
		}
	#endregion
	}
}
//	Old Killblock() code
//	for block respawn
/*	modPlayer.tileTime = 300;
	int type = mod.TileType("EmptyBlock");
	Main.tile[x, y].type = (ushort)type;
	Projectile.NewProjectile(x,y,0,0,mod.ProjectileType("Crumble"),0,0,Main.myPlayer);	*/		