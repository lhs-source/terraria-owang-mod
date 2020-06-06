using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.projectile
{
    class SecondEffect : ModProjectile
    {
        Vector2 vel;
        int dir;
        Vector2 angle;
        bool set;
        public override void SetDefaults()
        {
            //projectile.name = "SecondEffect";
            projectile.width = 350;
            projectile.height = 337;
            projectile.timeLeft = 40;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            Main.projFrames[projectile.type] = 8;

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
            if (new Random().Next(3) == 1)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, mod.DustType("Sparkle"), projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
            }

        }
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(new Vector2(
                projectile.position.X + (float)(projectile.width / 2), projectile.position.Y),
                new Vector2(0, 0),
                mod.ProjectileType("SecondEffectHit"),
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
