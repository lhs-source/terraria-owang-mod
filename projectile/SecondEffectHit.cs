using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.projectile
{
    class SecondEffectHit : ModProjectile
    {
        public override void SetDefaults()
        {
            //projectile.name = "SecondEffectHit";
            projectile.width = 350;
            projectile.height = 337;
            projectile.timeLeft = 50;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1;
            Main.projFrames[projectile.type] = 10;

        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            
            if (projectile.frameCounter < 5)
            {
                projectile.frameCounter++;
            }
            else
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //projectile.damage *= 2;
            Projectile.NewProjectile(new Vector2(
                (projectile.position.X + (float)(projectile.width / 2) + target.position.X) / 2 + new Random().Next(-20, 20), (projectile.position.Y + (float)(projectile.height / 2) + target.position.Y) / 2 + new Random().Next(-20, 20)),
                new Vector2(0, 0),
                mod.ProjectileType("SecondEffectHitEffect"),
                0,
                1f,
                projectile.owner);
        }
        private Vector2 GetLightPosition(Player player)
        {
            Vector2 position = Main.screenPosition;
            position.X += Main.mouseX;
            position.Y += player.gravDir == 1 ? Main.mouseY : Main.screenHeight - Main.mouseY;
            return position;
        }
    }
}
