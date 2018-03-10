using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperMetroid;
using SuperMetroid.Tiles.Shutters;

namespace SuperMetroid.Projectiles.Beams
{
	public class IcePlasma : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Plasma");
		}
		public override void SetDefaults()
		{
			projectile.width = 36;
			projectile.height = 4;
			projectile.aiStyle = -1;
			projectile.timeLeft = 210;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
			projectile.magic = true;
			projectile.netUpdate = true;
		}
		public void Initialize()
		{
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/TripleBeam"));
		}
		bool init = false;
		public override void AI()
		{
		//	projectile.AI(true);
			if(!init)
			{
				Initialize();
				init = true;
			}
			Player player = Main.player[projectile.owner];
			float PlayerCentreX = player.position.X + player.width * 0.5f;
			float PlayerCentreY = player.position.Y + player.height * 0.5f;
			float Angle = (float)Math.Atan2(Main.screenPosition.Y + Main.mouseY - PlayerCentreY,
												Main.screenPosition.X + Main.mouseX - PlayerCentreX);
			projectile.rotation = Angle;
			Color color = new Color();
			int dust = Dust.NewDust(new Vector2((float) projectile.position.X, (float) projectile.position.Y), projectile.width, projectile.height, 59, 0, 0, 100, color, 2.0f);
			Main.dust[dust].noGravity = false;
			foreach(NPC N in Main.npc)
			{
				if(!N.active) continue;
				if(N.life <= 0) continue;
				if(N.friendly) continue;
				if(N.dontTakeDamage) continue;
				if(N.boss) continue;
				if(N.type == 143 || N.type == 144 || N.type == 145 || N.type == 146) continue;
				Rectangle MB = new Rectangle((int)projectile.position.X+(int)projectile.velocity.X,(int)projectile.position.Y+(int)projectile.velocity.Y,projectile.width,projectile.height);
				Rectangle NB = new Rectangle((int)N.position.X,(int)N.position.Y,N.width,N.height);
				if(MB.Intersects(NB))
				{
					N.AddBuff(mod.BuffType("Frozen"),600,false);
				}
			}
		#region tile functions
			Vector2 tilev = new Vector2(projectile.position.X/16, projectile.position.Y/16);
			
		//	blocks
			int type = mod.TileType("CrackedBlock");
			int type8 = mod.TileType("UpgradeBall");
		//	shutter
			int type2 = mod.TileType("BlueSwitchleft");
			int type3 = mod.TileType("BlueSwitchright");
		//	doors
			int type4 = mod.TileType("vBlueDoor");
			int type5 = mod.TileType("ChozoDoor");
			
			Tile T = Main.tile[(int)tilev.X, (int)tilev.Y];
		
		//  collision
			bool collision = Main.tile[(int)tilev.X, (int)tilev.Y].active() && (Main.tileSolid[T.type] == true);
			bool correctTile = (type == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			bool blueSwitchL = (type2 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			bool blueSwitchR = (type3 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			bool vblueDoor = (type4 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			bool blueDoor = (type5 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			bool ball = (type8 == Main.tile[(int)tilev.X, (int)tilev.Y].type);
			if(collision) 
			{
				if(correctTile)
				{
					KillBlock((int)tilev.X, (int)tilev.Y);
				}
				if(blueSwitchL || blueSwitchR)
				{
					ShutterSwitch((int)tilev.X, (int)tilev.Y);
				}
				if(vblueDoor || blueDoor)
				{
					DoorToggle((int)tilev.X, (int)tilev.Y);
				}
				if(ball)
				{
					DropRandomUpgrade((int)tilev.X, (int)tilev.Y, Main.player[projectile.owner]);
				}
				projectile.active = false;
			}
		#endregion
		}
		#region tile action
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
			
			Wiring.TripWire(x, y, 1, 1);
			
			modPlayer.tileTime = 300;
			
			int type = mod.TileType("vBlueDoor");
			int type2 = mod.TileType("ChozoDoor");
			int transform = mod.TileType("vEmptyDoor");
			int transform2 = mod.TileType("EmptyChozoDoor");
			
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/DoorOpening"), projectile.position);
			
			if(Main.tile[x, y].type == (ushort)type) Main.tile[x, y].type = (ushort)transform;
			if(Main.tile[x, y].type == (ushort)type2) Main.tile[x, y].type = (ushort)transform2;
		}
		#endregion
		#region expansions
		string[] ItemArray = new string[] 
			{	"ChargeBeam" , "IceBeam" , "WaveBeam" , "PlasmaBeam" , "Spazer" , 
				"HiJump" , "ScrewAttack" , "SpaceJump" , "SpeedBooster" , "XRayScope" ,
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
				if(RandomItem == "VariaSuit" && !modPlayer.haveVaria)
				{
					Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType(RandomItem), 1, false, -1, true, false);
					modPlayer.haveVaria = true;
				}
				if(RandomItem == "GravitySuit" && !modPlayer.haveGravity)
				{
					Item.NewItem(new Vector2(x*16, y*16), 8, 8, mod.ItemType(RandomItem), 1, false, -1, true, false);
					modPlayer.haveGravity = true;
				}
			}
			else 
			{
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