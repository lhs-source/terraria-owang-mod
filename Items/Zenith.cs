using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.Items
{
    class Zenith : ModItem
    {
        public override void SetDefaults()
        {
            // name
            item.name = "Zenith";
            item.shoot = mod.ProjectileType("Zenith");
            item.toolTip = "This is just copy of Zenith..";

            // image
            item.width = 24;
            item.height = 24;

            // sound
            item.UseSound = SoundID.Item1;

            // spec 
            item.damage = 190;
            item.knockBack = 6.5f;
            item.value = Item.sellPrice(0, 20, 0, 0);
            item.crit = 25;
            item.rare = 10;

            // melee
            item.useAnimation = 30;
            item.useTime = item.useAnimation / 3;
            item.useStyle = 1; // swing
            item.autoReuse = true;
            item.noUseGraphic = true;
            item.noMelee = true;

            // shoot projectile
            item.shoot = mod.ProjectileType("Zenith");
            item.shootSpeed = 16f;

            base.SetDefaults();
        }

        // recipe of this
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        // when you use this sword
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        // effect of swing
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            base.MeleeEffects(player, hitbox);
            if (Main.rand.Next(3) == 0)
            {
                int dust = Dust.NewDust(
                new Vector2(hitbox.X, hitbox.Y),
                hitbox.Width,
                hitbox.Height,
                mod.DustType("Sparkle"));
                Lighting.AddLight(
                    new Vector2(hitbox.X, hitbox.Y)
                    , 0.5f, 0.5f, 1.0f);
            }
        }

    }
}
