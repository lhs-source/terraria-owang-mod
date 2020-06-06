using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.projectile
{
    class SwordEffect3_3 : ModProjectile
    {
        bool set;
        float damRate;
        public override void SetDefaults()
        {
            //projectile.name = "SwordEffect3_3";
            projectile.scale = 1.3f;
            projectile.width = (int)(595 / projectile.scale);
            projectile.height = (int)(523 / projectile.scale);
            projectile.timeLeft = 30;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.friendly = true;
            projectile.alpha = 30;
            Main.projFrames[projectile.type] = 3;
            damRate = 1.5f;
            set = false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (set == false)
            {
                Player player = Main.player[projectile.owner];
                projectile.position -= (projectile.Center - player.position);
                projectile.velocity = player.velocity * 2;
                projectile.light = 0.9f;
                set = true;
            }
            return base.PreDraw(spriteBatch, lightColor);
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            for (int k = 0; k < 2; k++)
            {
                Dust.NewDust(
                (projectile.position + (player.position - projectile.position) / 2) + projectile.velocity,
                projectile.width * 2 / 3,
                projectile.height * 2 / 3,
                //mod.DustType("Sparkle"), 
                DustID.Vortex,
                projectile.oldVelocity.X * 0.5f,
                projectile.oldVelocity.Y * 0.5f
                );
            }
            if (set == false)
            {
                projectile.position -= (projectile.Center - player.position);
                projectile.velocity = player.velocity * 2;
                set = true;
            }
            projectile.velocity *= 0.90f;
            if (projectile.timeLeft < 30)
            {
                if (projectile.frameCounter < 10)
                {
                    projectile.frameCounter++;
                }
                else
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                }
            }
            if (20 < projectile.timeLeft)
            {
                projectile.alpha += 10;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (1 < damRate)
            {
                damRate -= 0.05f;
            }
            projectile.damage = (int)(projectile.damage * damRate);
            Projectile.NewProjectile(
                new Vector2(
                    (projectile.position.X + (float)(projectile.width / 2) + target.position.X) / 2 + Main.rand.Next(-target.width / 5, target.width / 2), 
                    (projectile.position.Y + (float)(projectile.height / 2) + target.position.Y) / 2 + Main.rand.Next(-target.height / 5, target.height / 2)
                    ),
                new Vector2(0, 0),
                mod.ProjectileType("HitEffect1"),
                0,
                1f,
                projectile.owner);
        }
    }
}
