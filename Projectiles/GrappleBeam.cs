using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SuperMetroid;

namespace SuperMetroid.Projectiles
{
	public class GrappleBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grapple Beam");
		}
		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.aiStyle = -1;
			projectile.timeLeft = 30;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1f;
			projectile.magic = true;
			projectile.netUpdate = true;
		}
		private Player owner;
		private bool isOwner, isServer, isHooked = false;
		private Texture2D texture;
		private float maxDist;
		public void Initialize() 
		{
			owner = Main.player[projectile.owner];
			isOwner = (Main.myPlayer == projectile.owner);
			isServer = (Main.netMode == 2);
			
			if (!isServer)
				texture = mod.GetTexture("Gores/EnergyChain");
		}
		int soundDelay = 42;
		bool initialize = false;
		public override void AI()
		{
			if(!initialize)
			{
				Initialize();
				initialize = true;
			}
			Projectile P = projectile;
			Vector2 ProjPos = new Vector2(P.position.X+P.width/2, P.position.Y+P.height/2);
			
			if(isHooked)
			{
				projectile.velocity = default(Vector2);
				projectile.timeLeft = 2;
				
				Vector2 v = owner.position - projectile.position;
				float dist = Vector2.Distance(owner.position, projectile.position);
				if(soundDelay > 41)
				{
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/GrappleLoop"), owner.position);
					soundDelay = 0;
				}
				soundDelay++;
			// limit length of grapple
				if(owner.controlUp && maxDist > 1)
				{
					maxDist -= 2.5f;
				}
				if(owner.controlDown && maxDist < 465)
				{
					maxDist += 2.5f;
				}
			// pull in player to less than max distance
				if (maxDist > 150)
				{
					maxDist -= 5f;
				}
				if (maxDist < 25)
				{
					maxDist += 2.5f;
				}
			// increase player movement by 20%
				if(owner.controlLeft) owner.velocity.X -= 0.20f;
				if(owner.controlRight) owner.velocity.X += 0.20f;
				
				owner.velocity = Collision.TileCollision(owner.position, owner.velocity, owner.width, owner.height, owner.controlDown, false);
				owner.moveSpeed += 1.5f;
				owner.accRunSpeed += 1.1f;
				if (dist > maxDist)
				{
					float dif = (dist - maxDist)/maxDist;
					float ndist = Vector2.Distance(owner.position + owner.velocity, projectile.position);
					float ddist = ndist - dist;
					v /= dist;
					owner.velocity -= v * ddist;
					v *= maxDist;
					owner.position = projectile.position + v;
				}
			}
			else
			{	
				//Hit test
				int projX = (int)ProjPos.X >> 4;
				int projY = (int)ProjPos.Y >> 4;
				int type = mod.TileType("GrappleBlock");
				//Block check in case of swing only on certain tile
				if(BlockCheck(projX, projY, (ushort) type, owner))
				{
					isHooked = true;
					MetroidPlayer.grappled = true;
					projectile.velocity = default(Vector2);
					Vector2 dif = projectile.position - owner.position;
					float dist = (float)Math.Sqrt (dif.X * dif.X + dif.Y *dif.Y);
					maxDist = dist;
				}
				else if(Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
				{
					projectile.active = false;
				}
			}
			if ((owner.channel || owner.controlUseItem) && Main.myPlayer == projectile.owner)
			{
			}
			else
			{
				projectile.timeLeft = 0;
				MetroidPlayer.grappled = false;
			}
			Vector2 OwnerPos = new Vector2(owner.position.X+owner.width/2, owner.position.Y+owner.height/2);
			owner.itemTime = 2;
			owner.itemAnimation = 2;
			owner.itemRotation = (float)Math.Atan2((ProjPos.Y-OwnerPos.Y)*owner.direction,(ProjPos.X-OwnerPos.X)*owner.direction) -0.05f*owner.direction;
			owner.heldProj = P.whoAmI;
		}

		public override bool PreDraw(SpriteBatch s, Color lightColor)
		{
			owner = Main.player[projectile.owner];
			Vector2 OwnerPos = new Vector2(owner.position.X+owner.width/2, owner.position.Y+owner.height/2);
			Vector2 ProjPos = new Vector2(projectile.position.X+projectile.width/2, projectile.position.Y+projectile.height/2);
			float targetrotation = (float)Math.Atan2((ProjPos.Y-OwnerPos.Y),(ProjPos.X-OwnerPos.X));
			Vector2 ItemPos = new Vector2(owner.itemLocation.X+(float)Math.Cos(targetrotation)*20+8,owner.itemLocation.Y+(float)Math.Sin(targetrotation)*20+5);
			MetroidWorld.DrawChain(ItemPos, ProjPos, texture, s);
			
			return true;
		}
		
		public bool BlockCheck(int i, int j, ushort type, Player player)
		{
			int TileX = i;
			int TileY = j;
			
			bool Active = Main.tile[TileX, TileY].active() || Main.tile[TileX-1, TileY].active() || Main.tile[TileX+1, TileY].active() || Main.tile[TileX, TileY+1].active() || Main.tile[TileX, TileY -1].active();
			bool Solid = Main.tileSolid[Main.tile[TileX, TileY].type] == true  || Main.tileSolid[Main.tile[TileX-1, TileY].type] == true || Main.tileSolid[Main.tile[TileX+1, TileY].type] == true || Main.tileSolid[Main.tile[TileX, TileY+1].type] == true || Main.tileSolid[Main.tile[TileX, TileY-1].type] == true;
			bool Type = (Main.tile[TileX, TileY].type == type || Main.tile[TileX-1, TileY].type == type || Main.tile[TileX+1, TileY].type == type || Main.tile[TileX, TileY+1].type == type || Main.tile[TileX, TileY-1].type == type);
			
			if(Active && Solid && Type) return true;
			else return false;
		}
	}
}