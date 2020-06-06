using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.projectile
{
    class ExamStarfury : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.InfluxWaver);
            //projectile.name = "ExamStarfury";
            //aiType = ProjectileID.InfluxWaver;
            
            /*
            projectile.width = 24;
            projectile.height = 24;
            //projectile.timeLeft = 20;
            projectile.penetrate = 2;
            projectile.friendly = true;
            //projectile.aiStyle = 27;
            projectile.light = 1f;
            projectile.alpha = 50;
            projectile.scale = 0.8f;
            projectile.tileCollide = false;
            projectile.melee = true;
            */
        }
        /*
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
            int num2 = 10;
            int num3 = 3;
            for (int index = 0; index < num2; ++index)
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 58, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 150, new Color(), 1.2f);
            for (int index = 0; index < num3; ++index)
            {
                int Type = Main.rand.Next(16, 18);
                Gore.NewGore(projectile.position, new Vector2(projectile.velocity.X * 0.05f, projectile.velocity.Y * 0.05f), Type, 1f);
            }
            if (projectile.type == 12 && projectile.damage < 100)
            {
                for (int index = 0; index < 10; ++index)
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, 57, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 150, new Color(), 1.2f);
                for (int index = 0; index < 3; ++index)
                    Gore.NewGore(projectile.position, new Vector2(projectile.velocity.X * 0.05f, projectile.velocity.Y * 0.05f), Main.rand.Next(16, 18), 1f);
            }
            base.Kill(timeLeft);
        }
        */
    }
}
