using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace owang.projectile.soles
{
    class SolesSword : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.name = "ExamSkyFracture";
            projectile.width = 64;
            projectile.height = 64;
            projectile.aiStyle = 0;

            projectile.hostile = true;
            //projectile.friendly = true;
            projectile.alpha = (int)byte.MaxValue;

            projectile.timeLeft = 180;
            //projectile.melee = true;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;

            projectile.light = 1f;
        }
        public override void AI()
        {
            projectile.alpha -= 15;
            int num = 150;
            if ((double)projectile.Center.Y >= (double)projectile.ai[1])
                num = 0;
            if (projectile.alpha < num)
                projectile.alpha = num;
            projectile.localAI[0] += (float)((
                Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.00999999977648258) * projectile.direction;

            projectile.rotation = projectile.velocity.ToRotation() - 1.570796f;
            projectile.rotation += new Vector2(1, 1).ToRotation() * 3;

            if (Main.rand.Next(16) == 0)
            {
                Vector2 vector2 = Vector2.UnitX.RotatedByRandom(1.57079637050629).RotatedBy((double)projectile.velocity.ToRotation(), new Vector2());
                int index = Dust.NewDust(projectile.position, projectile.width, projectile.height, 58, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 150, new Color(), 1.2f);
                Main.dust[index].velocity = vector2 * 0.66f;
                Main.dust[index].position = projectile.Center + vector2 * 12f;
            }
            if (Main.rand.Next(48) == 0)
            {
                int index = Gore.NewGore(projectile.Center, new Vector2(projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f), 16, 1f);
                Main.gore[index].velocity *= 0.66f;
                Main.gore[index].velocity += projectile.velocity * 0.3f;
            }

            base.AI();
        }
    }
}
