using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace owang.projectile
{
    class Zenith : ModProjectile
    {
        private static Microsoft.Xna.Framework.Rectangle _lanceHitboxBounds = new Microsoft.Xna.Framework.Rectangle(0, 0, 300, 300);

        public override void SetDefaults()
        {
            // name
            projectile.name = "Zenith";
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = 0;
            //projectile.aiStyle = 182;

            // style
            projectile.friendly = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;

            // image
            projectile.alpha = (int)byte.MaxValue;

            // etc

            projectile.extraUpdates = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.manualDirectionChange = true;
            projectile.localNPCHitCooldown = 15;
            projectile.penetrate = -1;

            /// deprecated
            //projectile.noEnchantmentVisuals = true;

            //ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            //ProjectileID.Sets.TrailCacheLength[projectile.type] = 180;

            //Main.projFrames[projectile.type] = 14;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0.0f;
            float num1 = 40f;
            for (int index = 14; index < projectile.oldPos.Length; index += 15)
            {
                float num2 = projectile.localAI[0] - (float)index;
                if ((double)num2 >= 0.0 && (double)num2 <= 60.0)
                {
                    Vector2 vector2 = projectile.oldPos[index] + projectile.Size / 2f;
                    Vector2 rotationVector2 = (projectile.oldRot[index] + 1.570796f).ToRotationVector2();
                    _lanceHitboxBounds.X = (int)vector2.X - _lanceHitboxBounds.Width / 2;
                    _lanceHitboxBounds.Y = (int)vector2.Y - _lanceHitboxBounds.Height / 2;
                    if (_lanceHitboxBounds.Intersects(targetHitbox) && Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), vector2 - rotationVector2 * num1, vector2 + rotationVector2 * num1, 20f, ref collisionPoint))
                        return true;
                }
            }
            Vector2 rotationVector2_1 = (projectile.rotation + 1.570796f).ToRotationVector2();
            _lanceHitboxBounds.X = (int)projectile.position.X - _lanceHitboxBounds.Width / 2;

            _lanceHitboxBounds.Y = (int)projectile.position.Y - _lanceHitboxBounds.Height / 2;
            return _lanceHitboxBounds.Intersects(targetHitbox) && Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center - rotationVector2_1 * num1, projectile.Center + rotationVector2_1 * num1, 20f, ref collisionPoint);
            //return base.Colliding(projHitbox, targetHitbox);
        }
        public override bool PreAI()
        {
            return base.PreAI();
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {

        }
        public override void AI()
        {

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }
        public override void Kill(int timeLeft)
        {

        }
        public override Color? GetAlpha(Color lightColor)
        {

        }
    }
}
