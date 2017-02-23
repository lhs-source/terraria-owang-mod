using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader
{
	public class GlobalItem
	{
		public Mod mod
		{
			get;
			internal set;
		}

		public string Name
		{
			get;
			internal set;
		}

		public void AddTooltip(Item item, string tooltip)
		{
			if (string.IsNullOrEmpty(item.toolTip))
			{
				item.toolTip = tooltip;
			}
			else
			{
				item.toolTip += Environment.NewLine + tooltip;
			}
		}

		public void AddTooltip2(Item item, string tooltip)
		{
			if (string.IsNullOrEmpty(item.toolTip2))
			{
				item.toolTip2 = tooltip;
			}
			else
			{
				item.toolTip2 += Environment.NewLine + tooltip;
			}
		}

		public virtual bool Autoload(ref string name)
		{
			return mod.Properties.Autoload;
		}

		public virtual void SetDefaults(Item item)
		{
		}

		public virtual bool CanUseItem(Item item, Player player)
		{
			return true;
		}

		public virtual void UseStyle(Item item, Player player)
		{
		}

		public virtual void HoldStyle(Item item, Player player)
		{
		}

		public virtual void HoldItem(Item item, Player player)
		{
		}

		public virtual void GetWeaponDamage(Item item, Player player, ref int damage)
		{
		}

		public virtual void GetWeaponKnockback(Item item, Player player, ref float knockback)
		{
		}

		public virtual bool ConsumeAmmo(Item item, Player player)
		{
			return true;
		}

		public virtual bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return true;
		}

		public virtual void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
		{
		}

		public virtual void MeleeEffects(Item item, Player player, Rectangle hitbox)
		{
		}

		public virtual bool? CanHitNPC(Item item, Player player, NPC target)
		{
			return null;
		}

		public virtual void ModifyHitNPC(Item item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
		}

		public virtual void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
		{
		}

		public virtual bool CanHitPvp(Item item, Player player, Player target)
		{
			return true;
		}

		public virtual void ModifyHitPvp(Item item, Player player, Player target, ref int damage, ref bool crit)
		{
		}

		public virtual void OnHitPvp(Item item, Player player, Player target, int damage, bool crit)
		{
		}

		public virtual bool UseItem(Item item, Player player)
		{
			return false;
		}

		public virtual bool ConsumeItem(Item item, Player player)
		{
			return true;
		}

		public virtual bool UseItemFrame(Item item, Player player)
		{
			return false;
		}

		public virtual bool HoldItemFrame(Item item, Player player)
		{
			return false;
		}

		public virtual bool AltFunctionUse(Item item, Player player)
		{
			return false;
		}

		public virtual void UpdateInventory(Item item, Player player)
		{
		}

		public virtual void UpdateEquip(Item item, Player player)
		{
		}

		public virtual void UpdateAccessory(Item item, Player player, bool hideVisual)
		{
		}

		public virtual string IsArmorSet(Item head, Item body, Item legs)
		{
			return "";
		}

		public virtual void UpdateArmorSet(Player player, string set)
		{
		}

		public virtual string IsVanitySet(int head, int body, int legs)
		{
			Item headItem = new Item();
			if (head >= 0)
			{
				headItem.SetDefaults(Item.headType[head], true);
			}
			Item bodyItem = new Item();
			if (body >= 0)
			{
				bodyItem.SetDefaults(Item.bodyType[body], true);
			}
			Item legItem = new Item();
			if (legs >= 0)
			{
				legItem.SetDefaults(Item.legType[legs], true);
			}
			return IsArmorSet(headItem, bodyItem, legItem);
		}

		public virtual void PreUpdateVanitySet(Player player, string set)
		{
		}

		public virtual void UpdateVanitySet(Player player, string set)
		{
		}

		public virtual void ArmorSetShadows(Player player, string set)
		{
		}

		public virtual void SetMatch(int armorSlot, int type, bool male, ref int equipSlot, ref bool robes)
		{
		}

		public virtual bool CanRightClick(Item item)
		{
			return false;
		}

		public virtual void RightClick(Item item, Player player)
		{
		}

		public virtual bool PreOpenVanillaBag(string context, Player player, int arg)
		{
			return true;
		}

		public virtual void OpenVanillaBag(string context, Player player, int arg)
		{
		}

		public virtual void PreReforge(Item item)
		{
		}

		public virtual void PostReforge(Item item)
		{
		}

		public virtual void DrawHands(int body, ref bool drawHands, ref bool drawArms)
		{
		}

		public virtual void DrawHair(int head, ref bool drawHair, ref bool drawAltHair)
		{
		}

		public virtual bool DrawHead(int head)
		{
			return true;
		}

		public virtual bool DrawBody(int body)
		{
			return true;
		}

		public virtual bool DrawLegs(int legs, int shoes)
		{
			return true;
		}

		public virtual void DrawArmorColor(EquipType type, int slot, Player drawPlayer, float shadow, ref Color color,
			ref int glowMask, ref Color glowMaskColor)
		{
		}

		public virtual void ArmorArmGlowMask(int slot, Player drawPlayer, float shadow, ref int glowMask, ref Color color)
		{
		}

		[method: Obsolete("Use the overloaded method with the player parameter.")]
		public virtual void VerticalWingSpeeds(Item item, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
		}

		public virtual void VerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
	ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			VerticalWingSpeeds(item, ref ascentWhenFalling, ref ascentWhenRising, ref maxCanAscendMultiplier, ref maxAscentMultiplier, ref constantAscend);
		}

		[method: Obsolete("Use the overloaded method with the player parameter.")]
		public virtual void HorizontalWingSpeeds(Item item, ref float speed, ref float acceleration)
		{
		}

		public virtual void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
		{
			HorizontalWingSpeeds(item, ref speed, ref acceleration);
		}

		[method: Obsolete("WingUpdate will return a bool value later. (Use NewWingUpdate in the meantime.) False will keep everything the same. True, you need to handle all animations in your own code.")]
		public virtual void WingUpdate(int wings, Player player, bool inUse)
		{
		}

		public virtual bool NewWingUpdate(int wings, Player player, bool inUse)
		{
			WingUpdate(wings, player, inUse);
			return false;
		}

		public virtual void Update(Item item, ref float gravity, ref float maxFallSpeed)
		{
		}

		public virtual void PostUpdate(Item item)
		{
		}

		public virtual void GrabRange(Item item, Player player, ref int grabRange)
		{
		}

		public virtual bool GrabStyle(Item item, Player player)
		{
			return false;
		}

		public virtual bool CanPickup(Item item, Player player)
		{
			return true;
		}

		public virtual bool OnPickup(Item item, Player player)
		{
			return true;
		}

		public virtual bool ExtraPickupSpace(Item item, Player player)
		{
			return false;
		}

		public virtual Color? GetAlpha(Item item, Color lightColor)
		{
			return null;
		}

		public virtual bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			return true;
		}

		public virtual void PostDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
		}

		public virtual bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame,
			Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			return true;
		}

		public virtual void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame,
			Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
		}

		public virtual Vector2? HoldoutOffset(int type)
		{
			return null;
		}

		public virtual Vector2? HoldoutOrigin(int type)
		{
			return null;
		}

		public virtual bool CanEquipAccessory(Item item, Player player, int slot)
		{
			return true;
		}

		public virtual void ExtractinatorUse(int extractType, ref int resultType, ref int resultStack)
		{
		}

		public virtual void CaughtFishStack(int type, ref int stack)
		{
		}

		public virtual bool IsAnglerQuestAvailable(int type)
		{
			return true;
		}

		public virtual void AnglerChat(bool turningInFish, bool anglerQuestFinished, int type, ref string chat, ref string catchLocation)
		{
		}

		public virtual void OnCraft(Item item, Recipe recipe)
		{
		}

		public virtual void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
		}

		public virtual bool NeedsSaving(Item item)
		{
			return false;
		}

		public virtual TagCompound Save(Item item)
		{
			return null;
		}

		public virtual void Load(Item item, TagCompound tag)
		{
		}

		public virtual void LoadLegacy(Item item, BinaryReader reader)
		{
		}

		public virtual void NetSend(Item item, BinaryWriter writer)
		{
		}

		public virtual void NetReceive(Item item, BinaryReader reader)
		{
		}
	}
}
