using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.Items
{
	public class SecondSword : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Second Sword";
			item.damage = 100;
			item.melee = true;
			item.width = 63;
			item.height = 85;
            //item.Bottom = new Vector2(5, 5);
            item.Center = new Vector2(9, 82);
            item.toolTip = "Owang..!";
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("SwordEffect3");
            item.shootSpeed = 0f;
        }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            //recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
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
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            //damage *= 2;
            base.OnHitNPC(player, target, damage, knockBack, crit);
        }
    }
}
