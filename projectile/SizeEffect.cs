using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.projectile
{
    class SizeEffect : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.name = "SizeEffect";
            projectile.scale = 2;
            projectile.width = 144 / 2;
            projectile.height = 144 / 2;
            projectile.timeLeft = 40;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.light = 1;
            
            Main.projFrames[projectile.type] = 4;
        }
        public override void AI()
        {
            Dust.NewDust(
                projectile.position,
                0,
                0,
                DustID.PortalBolt,
                projectile.oldVelocity.X * 0.5f,
                projectile.oldVelocity.Y * 0.5f
                );
            Dust.NewDust(
                projectile.Center,
                0,
                0,
                DustID.PortalBolt,
                projectile.oldVelocity.X * 0.5f,
                projectile.oldVelocity.Y * 0.5f
                );
            Dust.NewDust(
                projectile.TopLeft,
                0,
                0,
                DustID.PortalBoltTrail,
                projectile.oldVelocity.X * 0.5f,
                projectile.oldVelocity.Y * 0.5f
                );
            Dust.NewDust(
                projectile.TopRight,
                0,
                0,
                DustID.PortalBoltTrail,
                projectile.oldVelocity.X * 0.5f,
                projectile.oldVelocity.Y * 0.5f
                );
            Dust.NewDust(
                projectile.BottomLeft,
                0,
                0,
                DustID.PortalBoltTrail,
                projectile.oldVelocity.X * 0.5f,
                projectile.oldVelocity.Y * 0.5f
                );
            Dust.NewDust(
                projectile.BottomRight,
                0,
                0,
                DustID.PortalBoltTrail,
                projectile.oldVelocity.X * 0.5f,
                projectile.oldVelocity.Y * 0.5f
                );
        }
    }
}
