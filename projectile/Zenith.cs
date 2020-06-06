using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.projectile
{
    class Zenith : ModProjectile
    {
        private static Microsoft.Xna.Framework.Rectangle _lanceHitboxBounds = new Microsoft.Xna.Framework.Rectangle(0, 0, 300, 300);

        public override void SetDefaults()
        {
            // name
            //projectile.name = "Zenith";
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

        private void AI_182_FinalFractal()
        {
            Player player = Main.player[projectile.owner];
            Vector2 mountedCenter = player.MountedCenter;
            float lerpValue1 = util.Utils.GetLerpValue(900f, 0.0f, projectile.velocity.Length() * 2f, true);
            projectile.localAI[0] += MathHelper.Lerp(0.7f, 2f, lerpValue1);
            if ((double)projectile.localAI[0] >= 120.0)
            {
                projectile.Kill();
            }
            else
            {
                float lerpValue2 = util.Utils.GetLerpValue(0.0f, 1f, projectile.localAI[0] / 60f, true);
                double num1 = (double)projectile.localAI[0] / 60.0;
                float num2 = projectile.ai[0];
                float rotation = projectile.velocity.ToRotation();
                float num3 = 3.141593f;
                float num4 = (double)projectile.velocity.X > 0.0 ? 1f : -1f;
                float num5 = num3 + (float)((double)num4 * (double)lerpValue2 * 6.28318548202515);
                float x = projectile.velocity.Length() + util.Utils.GetLerpValue(0.5f, 1f, lerpValue2, true) * 40f;
                float num6 = 60f;
                if ((double)x < (double)num6)
                    x = num6;
                Vector2 vector2_1 = mountedCenter + projectile.velocity + (new Vector2(1f, 0.0f).RotatedBy((double)num5, new Vector2()) * new Vector2(x, num2 * MathHelper.Lerp(2f, 1f, lerpValue1))).RotatedBy((double)rotation, new Vector2());
                Vector2 vector2_2 = (1f - util.Utils.GetLerpValue(0.0f, 0.5f, lerpValue2, true)) * new Vector2((float)(((double)projectile.velocity.X > 0.0 ? 1.0 : -1.0) * -(double)x * 0.100000001490116), (float)(-(double)projectile.ai[0] * 0.300000011920929));
                projectile.rotation = num5 + rotation + 1.570796f;
                projectile.Center = vector2_1 + vector2_2;
                projectile.spriteDirection = projectile.direction = (double)projectile.velocity.X > 0.0 ? 1 : -1;
                if ((double)num2 < 0.0)
                {
                    projectile.rotation = num3 + (float)((double)num4 * (double)lerpValue2 * -6.28318548202515) + rotation;
                    projectile.rotation += 1.570796f;
                    projectile.spriteDirection = projectile.direction = (double)projectile.velocity.X > 0.0 ? -1 : 1;
                }
                if (num1 < 1.0)
                {
                    Graphics.FinalFractalHelper.FinalFractalProfile finalFractalProfile = Graphics.FinalFractalHelper.GetFinalFractalProfile((int)projectile.ai[1]);
                    Vector2 rotationVector2 = (projectile.rotation - 1.570796f).ToRotationVector2();
                    Vector2 center = projectile.Center;
                    int num7 = (int)((double)(1 + (int)((double)projectile.velocity.Length() / 100.0)) * (double)util.Utils.GetLerpValue(0.0f, 0.5f, lerpValue2, true) * (double)util.Utils.GetLerpValue(1f, 0.5f, lerpValue2, true));
                    if (num7 < 1)
                        num7 = 1;
                    for (int index = 0; index < num7; ++index)
                        finalFractalProfile.dustMethod(center + rotationVector2 * finalFractalProfile.trailWidth * MathHelper.Lerp(0.5f, 1f, Main.rand.NextFloat()), (float)((double)projectile.rotation - 1.57079637050629 + 1.57079637050629 * (double)projectile.spriteDirection), player.velocity);
                    Vector3 vector3_1 = finalFractalProfile.trailColor.ToVector3();
                    Vector3 vector3_2 = Vector3.Lerp(Vector3.One, vector3_1, 0.7f);
                    Lighting.AddLight(projectile.Center, vector3_1 * 0.5f * projectile.Opacity);
                    Lighting.AddLight(mountedCenter, vector3_2 * projectile.Opacity * 0.15f);
                }
                projectile.Opacity = util.Utils.GetLerpValue(0.0f, 5f, projectile.localAI[0], true) * util.Utils.GetLerpValue(120f, 115f, projectile.localAI[0], true);
            }
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
            lightColor = Color.White * projectile.Opacity;

            float num11 = (float)((int)byte.MaxValue - projectile.alpha) / (float)byte.MaxValue;
            int r1 = (int)((double)lightColor.R * (double)num11);
            int g1 = (int)((double)lightColor.G * (double)num11);
            int b1 = (int)((double)lightColor.B * (double)num11);
            int a1 = (int)lightColor.A - projectile.alpha;

            return new Color(r1, g1, b1, a1);
        }
    }
}
