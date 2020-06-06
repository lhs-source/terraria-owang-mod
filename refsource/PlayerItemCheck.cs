using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Hsmod.refsource
{
    class PlayerItemCheck : Item
    {
        public void ItemCheck(int i)
        {
            if (!PlayerHooks.PreItemCheck(this))
            {
                return;
            }
            if (this.webbed || this.frozen || this.stoned)
            {
                return;
            }
            bool flag = false;
            float num = (float)this.mount.PlayerOffsetHitbox;
            Item item = this.inventory[this.selectedItem];
            if (this.mount.Active)
            {
                MountLoader.UseAbility(this, Vector2.Zero, false);
                if (this.mount.Type == 8)
                {
                    this.noItems = true;
                    if (this.controlUseItem)
                    {
                        this.channel = true;
                        if (this.releaseUseItem)
                        {
                            this.mount.UseAbility(this, Vector2.Zero, true);
                        }
                        this.releaseUseItem = false;
                    }
                }
                if (this.whoAmI == Main.myPlayer && this.gravDir == -1f)
                {
                    this.mount.Dismount(this);
                }
            }
            int weaponDamage = this.GetWeaponDamage(item);
            if (item.autoReuse && !this.noItems)
            {
                this.releaseUseItem = true;
                if (this.itemAnimation == 1 && item.stack > 0)
                {
                    if (item.shoot > 0 && this.whoAmI != Main.myPlayer && this.controlUseItem && item.useStyle == 5)
                    {
                        this.ApplyAnimation(item);
                        if (item.useSound > 0)
                        {
                            Main.PlaySound(2, (int)this.position.X, (int)this.position.Y, item.useSound);
                        }
                    }
                    else
                    {
                        this.itemAnimation = 0;
                    }
                }
            }
            if (item.fishingPole > 0)
            {
                item.holdStyle = 0;
                if (this.itemTime == 0 && this.itemAnimation == 0)
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        if (Main.projectile[j].active && Main.projectile[j].owner == this.whoAmI && Main.projectile[j].bobber)
                        {
                            item.holdStyle = 1;
                        }
                    }
                }
            }
            if (this.itemAnimation == 0 && this.altFunctionUse == 2)
            {
                this.altFunctionUse = 0;
            }
            if (this.itemAnimation == 0 && this.reuseDelay > 0)
            {
                this.itemAnimation = this.reuseDelay;
                this.itemTime = this.reuseDelay;
                this.reuseDelay = 0;
            }
            if (this.controlUseItem && this.releaseUseItem && (item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0))
            {
                if (item.useStyle == 0)
                {
                    this.releaseUseItem = false;
                }
                if (this.position.X / 16f - (float)Player.tileRangeX - (float)item.tileBoost <= (float)Player.tileTargetX && (this.position.X + (float)this.width) / 16f + (float)Player.tileRangeX + (float)item.tileBoost - 1f >= (float)Player.tileTargetX && this.position.Y / 16f - (float)Player.tileRangeY - (float)item.tileBoost <= (float)Player.tileTargetY && (this.position.Y + (float)this.height) / 16f + (float)Player.tileRangeY + (float)item.tileBoost - 2f >= (float)Player.tileTargetY)
                {
                    int num2 = Player.tileTargetX;
                    int num3 = Player.tileTargetY;
                    if (Main.tile[num2, num3].active() && (Main.tile[num2, num3].type == 128 || Main.tile[num2, num3].type == 269))
                    {
                        int num4 = (int)Main.tile[num2, num3].frameY;
                        int k = 0;
                        if (item.bodySlot >= 0)
                        {
                            k = 1;
                        }
                        if (item.legSlot >= 0)
                        {
                            k = 2;
                        }
                        num4 /= 18;
                        while (k > num4)
                        {
                            num3++;
                            num4 = (int)Main.tile[num2, num3].frameY;
                            num4 /= 18;
                        }
                        while (k < num4)
                        {
                            num3--;
                            num4 = (int)Main.tile[num2, num3].frameY;
                            num4 /= 18;
                        }
                        int l;
                        for (l = (int)Main.tile[num2, num3].frameX; l >= 100; l -= 100)
                        {
                        }
                        if (l >= 36)
                        {
                            l -= 36;
                        }
                        num2 -= l / 18;
                        int m = (int)Main.tile[num2, num3].frameX;
                        WorldGen.KillTile(num2, num3, true, false, false);
                        if (Main.netMode == 1)
                        {
                            NetMessage.SendData(17, -1, -1, "", 0, (float)num2, (float)num3, 1f, 0, 0, 0);
                        }
                        while (m >= 100)
                        {
                            m -= 100;
                        }
                        if (num4 == 0 && item.headSlot >= 0)
                        {
                            Main.blockMouse = true;
                            Main.tile[num2, num3].frameX = (short)(m + item.headSlot * 100);
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendTileSquare(-1, num2, num3, 1);
                            }
                            item.stack--;
                            if (item.stack <= 0)
                            {
                                item.SetDefaults(0, false);
                                Main.mouseItem.SetDefaults(0, false);
                            }
                            if (this.selectedItem == 58)
                            {
                                Main.mouseItem = item.Clone();
                            }
                            this.releaseUseItem = false;
                            this.mouseInterface = true;
                        }
                        else if (num4 == 1 && item.bodySlot >= 0)
                        {
                            Main.blockMouse = true;
                            Main.tile[num2, num3].frameX = (short)(m + item.bodySlot * 100);
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendTileSquare(-1, num2, num3, 1);
                            }
                            item.stack--;
                            if (item.stack <= 0)
                            {
                                item.SetDefaults(0, false);
                                Main.mouseItem.SetDefaults(0, false);
                            }
                            if (this.selectedItem == 58)
                            {
                                Main.mouseItem = item.Clone();
                            }
                            this.releaseUseItem = false;
                            this.mouseInterface = true;
                        }
                        else if (num4 == 2 && item.legSlot >= 0 && !ArmorIDs.Legs.Sets.MannequinIncompatible.Contains(item.legSlot))
                        {
                            Main.blockMouse = true;
                            Main.tile[num2, num3].frameX = (short)(m + item.legSlot * 100);
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendTileSquare(-1, num2, num3, 1);
                            }
                            item.stack--;
                            if (item.stack <= 0)
                            {
                                item.SetDefaults(0, false);
                                Main.mouseItem.SetDefaults(0, false);
                            }
                            if (this.selectedItem == 58)
                            {
                                Main.mouseItem = item.Clone();
                            }
                            this.releaseUseItem = false;
                            this.mouseInterface = true;
                        }
                    }
                }
            }
            if (Main.myPlayer == i && this.itemAnimation == 0 && TileObjectData.CustomPlace(item.createTile, item.placeStyle))
            {
                TileObject tileObject;
                int hackCreateTile = item.createTile;
                int hackPlaceStyle = item.placeStyle;
                if (hackCreateTile == TileID.Saplings)
                {
                    Tile soil = Main.tile[Player.tileTargetX, Player.tileTargetY + 1];
                    if (soil.active())
                    {
                        TileLoader.SaplingGrowthType(soil.type, ref hackCreateTile, ref hackPlaceStyle);
                    }
                }
                TileObject.CanPlace(Player.tileTargetX, Player.tileTargetY, hackCreateTile, hackPlaceStyle, this.direction, out tileObject, true);
            }
            if (this.controlUseItem && this.itemAnimation == 0 && this.releaseUseItem && item.useStyle > 0)
            {
                if (this.altFunctionUse == 1)
                {
                    this.altFunctionUse = 2;
                }
                bool flag2 = ItemLoader.CanUseItem(item, this);
                if (item.shoot == 0)
                {
                    this.itemRotation = 0f;
                }
                if (item.type == 3335 && (this.extraAccessory || !Main.expertMode))
                {
                    flag2 = false;
                }
                if (this.pulley && item.fishingPole > 0)
                {
                    flag2 = false;
                }
                if (item.type == 3611 && (WiresUI.Settings.ToolMode & (WiresUI.Settings.MultiToolMode.Red | WiresUI.Settings.MultiToolMode.Green | WiresUI.Settings.MultiToolMode.Blue | WiresUI.Settings.MultiToolMode.Yellow | WiresUI.Settings.MultiToolMode.Actuator)) == (WiresUI.Settings.MultiToolMode)0)
                {
                    flag2 = false;
                }
                if ((item.type == 3611 || item.type == 3625) && this.wireOperationsCooldown > 0)
                {
                    flag2 = false;
                }
                if (this.wet && (item.shoot == 85 || item.shoot == 15 || item.shoot == 34))
                {
                    flag2 = false;
                }
                if (item.makeNPC > 0 && !NPC.CanReleaseNPCs(this.whoAmI))
                {
                    flag2 = false;
                }
                if (this.whoAmI == Main.myPlayer && item.type == 603 && !Main.cEd)
                {
                    flag2 = false;
                }
                if (item.type == 1071 || item.type == 1072)
                {
                    bool flag3 = false;
                    for (int n = 0; n < 58; n++)
                    {
                        if (this.inventory[n].paint > 0)
                        {
                            flag3 = true;
                            break;
                        }
                    }
                    if (!flag3)
                    {
                        flag2 = false;
                    }
                }
                if (this.noItems)
                {
                    flag2 = false;
                }
                if (item.tileWand > 0)
                {
                    int tileWand = item.tileWand;
                    flag2 = false;
                    for (int num5 = 0; num5 < 58; num5++)
                    {
                        if (tileWand == this.inventory[num5].type && this.inventory[num5].stack > 0)
                        {
                            flag2 = true;
                            break;
                        }
                    }
                }
                if (item.fishingPole > 0)
                {
                    for (int num6 = 0; num6 < 1000; num6++)
                    {
                        if (Main.projectile[num6].active && Main.projectile[num6].owner == this.whoAmI && Main.projectile[num6].bobber)
                        {
                            flag2 = false;
                            if (this.whoAmI == Main.myPlayer && Main.projectile[num6].ai[0] == 0f)
                            {
                                Main.projectile[num6].ai[0] = 1f;
                                float num7 = -10f;
                                if (Main.projectile[num6].wet && Main.projectile[num6].velocity.Y > num7)
                                {
                                    Main.projectile[num6].velocity.Y = num7;
                                }
                                Main.projectile[num6].netUpdate2 = true;
                                if (Main.projectile[num6].ai[1] < 0f && Main.projectile[num6].localAI[1] != 0f)
                                {
                                    bool flag4 = false;
                                    int num8 = 0;
                                    for (int num9 = 0; num9 < 58; num9++)
                                    {
                                        if (this.inventory[num9].stack > 0 && this.inventory[num9].bait > 0)
                                        {
                                            bool flag5 = false;
                                            int num10 = 1 + this.inventory[num9].bait / 5;
                                            if (num10 < 1)
                                            {
                                                num10 = 1;
                                            }
                                            if (this.accTackleBox)
                                            {
                                                num10++;
                                            }
                                            if (Main.rand.Next(num10) == 0)
                                            {
                                                flag5 = true;
                                            }
                                            if (Main.projectile[num6].localAI[1] < 0f)
                                            {
                                                flag5 = true;
                                            }
                                            if (Main.projectile[num6].localAI[1] > 0f)
                                            {
                                                Item item2 = new Item();
                                                item2.SetDefaults((int)Main.projectile[num6].localAI[1], false);
                                                if (item2.rare < 0)
                                                {
                                                    flag5 = false;
                                                }
                                            }
                                            if (flag5)
                                            {
                                                num8 = this.inventory[num9].type;
                                                this.inventory[num9].stack--;
                                                if (this.inventory[num9].stack <= 0)
                                                {
                                                    this.inventory[num9].SetDefaults(0, false);
                                                }
                                            }
                                            flag4 = true;
                                            break;
                                        }
                                    }
                                    if (flag4)
                                    {
                                        if (num8 == 2673)
                                        {
                                            if (Main.netMode != 1)
                                            {
                                                NPC.SpawnOnPlayer(this.whoAmI, 370);
                                            }
                                            else
                                            {
                                                NetMessage.SendData(61, -1, -1, "", this.whoAmI, 370f, 0f, 0f, 0, 0, 0);
                                            }
                                            Main.projectile[num6].ai[0] = 2f;
                                        }
                                        else if (Main.rand.Next(7) == 0 && !this.accFishingLine)
                                        {
                                            Main.projectile[num6].ai[0] = 2f;
                                        }
                                        else
                                        {
                                            Main.projectile[num6].ai[1] = Main.projectile[num6].localAI[1];
                                        }
                                        Main.projectile[num6].netUpdate = true;
                                    }
                                }
                            }
                        }
                    }
                }
                //  부메랑류 같음
                //  인챈트 부메랑 / Flamarang / 	Thorn Chakram
                //  Wooden Boomerang / 	Ice Boomerang / Bloody Machete
                //  Fruitcake Chakram / Anchor / Flying Knife
                if (item.shoot == 6 || item.shoot == 19 || item.shoot == 33 ||
                    item.shoot == 52 || item.shoot == 113 || item.shoot == 320 ||
                    item.shoot == 333 || item.shoot == 383 || item.shoot == 491)
                {
                    for (int num11 = 0; num11 < 1000; num11++)
                    {
                        if (Main.projectile[num11].active && Main.projectile[num11].owner == Main.myPlayer && Main.projectile[num11].type == item.shoot)
                        {
                            flag2 = false;
                        }
                    }
                }
                //  Light Disc
                if (item.shoot == 106)
                {
                    int num12 = 0;
                    for (int num13 = 0; num13 < 1000; num13++)
                    {
                        if (Main.projectile[num13].active && Main.projectile[num13].owner == Main.myPlayer && Main.projectile[num13].type == item.shoot)
                        {
                            num12++;
                        }
                    }
                    if (num12 >= item.stack)
                    {
                        flag2 = false;
                    }
                }
                //  	Bananarang
                if (item.shoot == 272)
                {
                    int num14 = 0;
                    for (int num15 = 0; num15 < 1000; num15++)
                    {
                        if (Main.projectile[num15].active && Main.projectile[num15].owner == Main.myPlayer && Main.projectile[num15].type == item.shoot)
                        {
                            num14++;
                        }
                    }
                    if (num14 >= item.stack)
                    {
                        flag2 = false;
                    }
                }
                //  	Grappling Hook / 	Ivy Whip / 보석 Hook
                //  Bat Hook / Candy Cane Hook / Fish Hook
                if (item.shoot == 13 || item.shoot == 32 || (item.shoot >= 230 && item.shoot <= 235) ||
                    item.shoot == 315 || item.shoot == 331 || item.shoot == 372)
                {
                    for (int num16 = 0; num16 < 1000; num16++)
                    {
                        if (Main.projectile[num16].active && Main.projectile[num16].owner == Main.myPlayer && Main.projectile[num16].type == item.shoot && Main.projectile[num16].ai[0] != 2f)
                        {
                            flag2 = false;
                        }
                    }
                }
                //  Christmas Hook
                if (item.shoot == 332)
                {
                    int num17 = 0;
                    for (int num18 = 0; num18 < 1000; num18++)
                    {
                        if (Main.projectile[num18].active && Main.projectile[num18].owner == Main.myPlayer && Main.projectile[num18].type == item.shoot && Main.projectile[num18].ai[0] != 2f)
                        {
                            num17++;
                        }
                    }
                    if (num17 >= 3)
                    {
                        flag2 = false;
                    }
                }
                if (item.potion && flag2)
                {
                    if (this.potionDelay <= 0)
                    {
                        if (item.type == 227)
                        {
                            this.potionDelay = this.restorationDelayTime;
                            this.AddBuff(21, this.potionDelay, true);
                        }
                        else
                        {
                            this.potionDelay = this.potionDelayTime;
                            this.AddBuff(21, this.potionDelay, true);
                        }
                    }
                    else
                    {
                        flag2 = false;
                    }
                }
                if (item.mana > 0 && this.silence)
                {
                    flag2 = false;
                }
                if (item.mana > 0 && flag2)
                {
                    bool flag6 = false;
                    if (item.type == 2795)
                    {
                        flag6 = true;
                    }
                    if (item.type != 127 || !this.spaceGun)
                    {
                        if (this.statMana >= (int)((float)item.mana * this.manaCost))
                        {
                            if (!flag6)
                            {
                                this.statMana -= (int)((float)item.mana * this.manaCost);
                            }
                        }
                        else if (this.manaFlower)
                        {
                            this.QuickMana();
                            if (this.statMana >= (int)((float)item.mana * this.manaCost))
                            {
                                if (!flag6)
                                {
                                    this.statMana -= (int)((float)item.mana * this.manaCost);
                                }
                            }
                            else
                            {
                                flag2 = false;
                            }
                        }
                        else
                        {
                            flag2 = false;
                        }
                    }
                    if (this.whoAmI == Main.myPlayer && item.buffType != 0 && flag2)
                    {
                        this.AddBuff(item.buffType, item.buffTime, true);
                    }
                }
                //
                //  펫들이다
                //
                if (this.whoAmI == Main.myPlayer && item.type == 603 && Main.cEd)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 669)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 115)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 3060)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 3628)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 3062)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 3577)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 425)
                {
                    int num19 = Main.rand.Next(3);
                    if (num19 == 0)
                    {
                        num19 = 27;
                    }
                    if (num19 == 1)
                    {
                        num19 = 101;
                    }
                    if (num19 == 2)
                    {
                        num19 = 102;
                    }
                    for (int num20 = 0; num20 < 22; num20++)
                    {
                        if (this.buffType[num20] == 27 || this.buffType[num20] == 101 || this.buffType[num20] == 102)
                        {
                            this.DelBuff(num20);
                            num20--;
                        }
                    }
                    this.AddBuff(num19, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 753)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 994)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1169)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1170)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1171)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1172)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1180)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1181)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1182)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1183)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1242)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1157)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1309)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1311)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1837)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1312)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1798)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1799)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1802)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1810)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1927)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 1959)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 2364)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 2365)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 3043)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 2420)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 2535)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 2551)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 2584)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 2587)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 2621)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 2749)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 3249)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 3474)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && item.type == 3531)
                {
                    this.AddBuff(item.buffType, 3600, true);
                }
                if (this.whoAmI == Main.myPlayer && this.gravDir == 1f && item.mountType != -1 && this.mount.CanMount(item.mountType, this))
                {
                    this.mount.SetMount(item.mountType, this, false);
                }
                //
                //  보스 소환 아이템
                //
                if (item.type == 43 && Main.dayTime)
                {
                    flag2 = false;
                }
                if (item.type == 544 && Main.dayTime)
                {
                    flag2 = false;
                }
                if (item.type == 556 && Main.dayTime)
                {
                    flag2 = false;
                }
                if (item.type == 557 && Main.dayTime)
                {
                    flag2 = false;
                }
                if (item.type == 70 && !this.ZoneCorrupt)
                {
                    flag2 = false;
                }
                if (item.type == 1133 && !this.ZoneJungle)
                {
                    flag2 = false;
                }
                if (item.type == 1844 && (Main.dayTime || Main.pumpkinMoon || Main.snowMoon))
                {
                    flag2 = false;
                }
                if (item.type == 1958 && (Main.dayTime || Main.pumpkinMoon || Main.snowMoon))
                {
                    flag2 = false;
                }
                if (item.type == 2767 && (!Main.dayTime || Main.eclipse || !Main.hardMode))
                {
                    flag2 = false;
                }
                if (item.type == 3601 && (!NPC.downedGolemBoss || !Main.hardMode || NPC.AnyDanger() || NPC.AnyoneNearCultists()))
                {
                    flag2 = false;
                }
                if (!this.SummonItemCheck())
                {
                    flag2 = false;
                }
                //  흙 볼
                if (item.shoot == 17 && flag2 && i == Main.myPlayer)
                {
                    int num21 = (int)((float)Main.mouseX + Main.screenPosition.X) / 16;
                    int num22 = (int)((float)Main.mouseY + Main.screenPosition.Y) / 16;
                    if (this.gravDir == -1f)
                    {
                        num22 = (int)(Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY) / 16;
                    }
                    Tile tile = Main.tile[num21, num22];
                    if (tile.active() && (tile.type == 0 || tile.type == 2 || tile.type == 23 || tile.type == 109 || tile.type == 199))
                    {
                        WorldGen.KillTile(num21, num22, false, false, true);
                        if (!Main.tile[num21, num22].active())
                        {
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendData(17, -1, -1, "", 4, (float)num21, (float)num22, 0f, 0, 0, 0);
                            }
                        }
                        else
                        {
                            flag2 = false;
                        }
                    }
                    else
                    {
                        flag2 = false;
                    }
                }
                if (flag2)
                {
                    flag2 = this.HasAmmo(item, flag2);
                }
                if (flag2)
                {
                    if (item.pick > 0 || item.axe > 0 || item.hammer > 0)
                    {
                        this.toolTime = 1;
                    }
                    if (this.grappling[0] > -1)
                    {
                        this.pulley = false;
                        this.pulleyDir = 1;
                        if (this.controlRight)
                        {
                            this.direction = 1;
                        }
                        else if (this.controlLeft)
                        {
                            this.direction = -1;
                        }
                    }
                    this.channel = item.channel;
                    this.attackCD = 0;
                    this.ApplyAnimation(item);
                    if (item.useSound > 0)
                    {
                        Main.PlaySound(2, (int)this.position.X, (int)this.position.Y, item.useSound);
                    }
                }
                if (flag2 && this.whoAmI == Main.myPlayer && item.shoot >= 0 && (ProjectileID.Sets.LightPet[item.shoot] || Main.projPet[item.shoot]))
                {
                    if (ProjectileID.Sets.MinionSacrificable[item.shoot])
                    {
                        List<int> list = new List<int>();
                        float num23 = 0f;
                        for (int num24 = 0; num24 < 1000; num24++)
                        {
                            if (Main.projectile[num24].active && Main.projectile[num24].owner == i && Main.projectile[num24].minion)
                            {
                                int num25;
                                for (num25 = 0; num25 < list.Count; num25++)
                                {
                                    if (Main.projectile[list[num25]].minionSlots > Main.projectile[num24].minionSlots)
                                    {
                                        list.Insert(num25, num24);
                                        break;
                                    }
                                }
                                if (num25 == list.Count)
                                {
                                    list.Add(num24);
                                }
                                num23 += Main.projectile[num24].minionSlots;
                            }
                        }
                        float num26 = (float)ItemID.Sets.StaffMinionSlotsRequired[item.type];
                        float num27 = 0f;
                        int num28 = 388;
                        int num29 = -1;
                        for (int num30 = 0; num30 < list.Count; num30++)
                        {
                            int type = Main.projectile[list[num30]].type;
                            if (type == 626)
                            {
                                list.RemoveAt(num30);
                                num30--;
                            }
                            if (type == 627)
                            {
                                if (Main.projectile[(int)Main.projectile[list[num30]].localAI[1]].type == 628)
                                {
                                    num29 = list[num30];
                                }
                                list.RemoveAt(num30);
                                num30--;
                            }
                        }
                        if (num29 != -1)
                        {
                            list.Add(num29);
                            list.Add(Projectile.GetByUUID(Main.projectile[num29].owner, Main.projectile[num29].ai[0]));
                        }
                        int num31 = 0;
                        while (num31 < list.Count && num23 - num27 > (float)this.maxMinions - num26)
                        {
                            int type2 = Main.projectile[list[num31]].type;
                            if (type2 != num28 && type2 != 625 && type2 != 628 && type2 != 623)
                            {
                                if (type2 == 388 && num28 == 387)
                                {
                                    num28 = 388;
                                }
                                if (type2 == 387 && num28 == 388)
                                {
                                    num28 = 387;
                                }
                                num27 += Main.projectile[list[num31]].minionSlots;
                                if (type2 == 626 || type2 == 627)
                                {
                                    int byUUID = Projectile.GetByUUID(Main.projectile[list[num31]].owner, Main.projectile[list[num31]].ai[0]);
                                    if (byUUID >= 0)
                                    {
                                        Projectile projectile = Main.projectile[byUUID];
                                        if (projectile.type != 625)
                                        {
                                            projectile.localAI[1] = Main.projectile[list[num31]].localAI[1];
                                        }
                                        projectile = Main.projectile[(int)Main.projectile[list[num31]].localAI[1]];
                                        projectile.ai[0] = Main.projectile[list[num31]].ai[0];
                                        projectile.ai[1] = 1f;
                                        projectile.netUpdate = true;
                                    }
                                }
                                Main.projectile[list[num31]].Kill();
                            }
                            num31++;
                        }
                        list.Clear();
                        if (num23 + num26 >= 9f)
                        {
                            AchievementsHelper.HandleSpecialEvent(this, 6);
                        }
                    }
                    else
                    {
                        for (int num32 = 0; num32 < 1000; num32++)
                        {
                            if (Main.projectile[num32].active && Main.projectile[num32].owner == i && Main.projectile[num32].type == item.shoot)
                            {
                                Main.projectile[num32].Kill();
                            }
                            if (item.shoot == 72)
                            {
                                if (Main.projectile[num32].active && Main.projectile[num32].owner == i && Main.projectile[num32].type == 86)
                                {
                                    Main.projectile[num32].Kill();
                                }
                                if (Main.projectile[num32].active && Main.projectile[num32].owner == i && Main.projectile[num32].type == 87)
                                {
                                    Main.projectile[num32].Kill();
                                }
                            }
                        }
                    }
                }
            }
            if (!this.controlUseItem)
            {
                bool arg_1FA5_0 = this.channel;
                this.channel = false;
            }
            if (this.itemAnimation > 0)
            {
                if (item.melee)
                {
                    this.itemAnimationMax = (int)((float)item.useAnimation * this.meleeSpeed);
                }
                else
                {
                    this.itemAnimationMax = item.useAnimation;
                }
                if (item.mana > 0 && !flag && (item.type != 127 || !this.spaceGun))
                {
                    this.manaRegenDelay = (int)this.maxRegenDelay;
                }
                if (Main.dedServ)
                {
                    this.itemHeight = item.height;
                    this.itemWidth = item.width;
                }
                else
                {
                    this.itemHeight = Main.itemTexture[item.type].Height;
                    this.itemWidth = Main.itemTexture[item.type].Width;
                }
                this.itemAnimation--;
                if (!Main.dedServ)
                {
                    if (item.useStyle == 1)
                    {
                        if (item.type > -1 && Item.claw[item.type])
                        {
                            if ((double)this.itemAnimation < (double)this.itemAnimationMax * 0.333)
                            {
                                float num33 = 10f;
                                this.itemLocation.X = this.position.X + (float)this.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - num33) * (float)this.direction;
                                this.itemLocation.Y = this.position.Y + 26f + num;
                            }
                            else if ((double)this.itemAnimation < (double)this.itemAnimationMax * 0.666)
                            {
                                float num34 = 8f;
                                this.itemLocation.X = this.position.X + (float)this.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - num34) * (float)this.direction;
                                num34 = 24f;
                                this.itemLocation.Y = this.position.Y + num34 + num;
                            }
                            else
                            {
                                float num35 = 6f;
                                this.itemLocation.X = this.position.X + (float)this.width * 0.5f - ((float)Main.itemTexture[item.type].Width * 0.5f - num35) * (float)this.direction;
                                num35 = 20f;
                                this.itemLocation.Y = this.position.Y + num35 + num;
                            }
                            this.itemRotation = ((float)this.itemAnimation / (float)this.itemAnimationMax - 0.5f) * (float)(-(float)this.direction) * 3.5f - (float)this.direction * 0.3f;
                        }
                        else
                        {
                            if ((double)this.itemAnimation < (double)this.itemAnimationMax * 0.333)
                            {
                                float num36 = 10f;
                                if (Main.itemTexture[item.type].Width > 32)
                                {
                                    num36 = 14f;
                                }
                                if (Main.itemTexture[item.type].Width >= 52)
                                {
                                    num36 = 24f;
                                }
                                if (Main.itemTexture[item.type].Width >= 64)
                                {
                                    num36 = 28f;
                                }
                                if (Main.itemTexture[item.type].Width >= 92)
                                {
                                    num36 = 38f;
                                }
                                if (item.type == 2330 || item.type == 2320 || item.type == 2341)
                                {
                                    num36 += 8f;
                                }
                                this.itemLocation.X = this.position.X + (float)this.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - num36) * (float)this.direction;
                                this.itemLocation.Y = this.position.Y + 24f + num;
                            }
                            else if ((double)this.itemAnimation < (double)this.itemAnimationMax * 0.666)
                            {
                                float num37 = 10f;
                                if (Main.itemTexture[item.type].Width > 32)
                                {
                                    num37 = 18f;
                                }
                                if (Main.itemTexture[item.type].Width >= 52)
                                {
                                    num37 = 24f;
                                }
                                if (Main.itemTexture[item.type].Width >= 64)
                                {
                                    num37 = 28f;
                                }
                                if (Main.itemTexture[item.type].Width >= 92)
                                {
                                    num37 = 38f;
                                }
                                if (item.type == 2330 || item.type == 2320 || item.type == 2341)
                                {
                                    num37 += 4f;
                                }
                                this.itemLocation.X = this.position.X + (float)this.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - num37) * (float)this.direction;
                                num37 = 10f;
                                if (Main.itemTexture[item.type].Height > 32)
                                {
                                    num37 = 8f;
                                }
                                if (Main.itemTexture[item.type].Height > 52)
                                {
                                    num37 = 12f;
                                }
                                if (Main.itemTexture[item.type].Height > 64)
                                {
                                    num37 = 14f;
                                }
                                if (item.type == 2330 || item.type == 2320 || item.type == 2341)
                                {
                                    num37 += 4f;
                                }
                                this.itemLocation.Y = this.position.Y + num37 + num;
                            }
                            else
                            {
                                float num38 = 6f;
                                if (Main.itemTexture[item.type].Width > 32)
                                {
                                    num38 = 14f;
                                }
                                if (Main.itemTexture[item.type].Width >= 48)
                                {
                                    num38 = 18f;
                                }
                                if (Main.itemTexture[item.type].Width >= 52)
                                {
                                    num38 = 24f;
                                }
                                if (Main.itemTexture[item.type].Width >= 64)
                                {
                                    num38 = 28f;
                                }
                                if (Main.itemTexture[item.type].Width >= 92)
                                {
                                    num38 = 38f;
                                }
                                if (item.type == 2330 || item.type == 2320 || item.type == 2341)
                                {
                                    num38 += 4f;
                                }
                                this.itemLocation.X = this.position.X + (float)this.width * 0.5f - ((float)Main.itemTexture[item.type].Width * 0.5f - num38) * (float)this.direction;
                                num38 = 10f;
                                if (Main.itemTexture[item.type].Height > 32)
                                {
                                    num38 = 10f;
                                }
                                if (Main.itemTexture[item.type].Height > 52)
                                {
                                    num38 = 12f;
                                }
                                if (Main.itemTexture[item.type].Height > 64)
                                {
                                    num38 = 14f;
                                }
                                if (item.type == 2330 || item.type == 2320 || item.type == 2341)
                                {
                                    num38 += 4f;
                                }
                                this.itemLocation.Y = this.position.Y + num38 + num;
                            }
                            this.itemRotation = ((float)this.itemAnimation / (float)this.itemAnimationMax - 0.5f) * (float)(-(float)this.direction) * 3.5f - (float)this.direction * 0.3f;
                        }
                        if (this.gravDir == -1f)
                        {
                            this.itemRotation = -this.itemRotation;
                            this.itemLocation.Y = this.position.Y + (float)this.height + (this.position.Y - this.itemLocation.Y);
                        }
                    }
                    else if (item.useStyle == 2)
                    {
                        this.itemRotation = (float)this.itemAnimation / (float)this.itemAnimationMax * (float)this.direction * 2f + -1.4f * (float)this.direction;
                        if ((double)this.itemAnimation < (double)this.itemAnimationMax * 0.5)
                        {
                            this.itemLocation.X = this.position.X + (float)this.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - 9f - this.itemRotation * 12f * (float)this.direction) * (float)this.direction;
                            this.itemLocation.Y = this.position.Y + 38f + this.itemRotation * (float)this.direction * 4f + num;
                        }
                        else
                        {
                            this.itemLocation.X = this.position.X + (float)this.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - 9f - this.itemRotation * 16f * (float)this.direction) * (float)this.direction;
                            this.itemLocation.Y = this.position.Y + 38f + this.itemRotation * (float)this.direction + num;
                        }
                        if (this.gravDir == -1f)
                        {
                            this.itemRotation = -this.itemRotation;
                            this.itemLocation.Y = this.position.Y + (float)this.height + (this.position.Y - this.itemLocation.Y);
                        }
                    }
                    else if (item.useStyle == 3)
                    {
                        if ((double)this.itemAnimation > (double)this.itemAnimationMax * 0.666)
                        {
                            this.itemLocation.X = -1000f;
                            this.itemLocation.Y = -1000f;
                            this.itemRotation = -1.3f * (float)this.direction;
                        }
                        else
                        {
                            this.itemLocation.X = this.position.X + (float)this.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - 4f) * (float)this.direction;
                            this.itemLocation.Y = this.position.Y + 24f + num;
                            float num39 = (float)this.itemAnimation / (float)this.itemAnimationMax * (float)Main.itemTexture[item.type].Width * (float)this.direction * item.scale * 1.2f - (float)(10 * this.direction);
                            if (num39 > -4f && this.direction == -1)
                            {
                                num39 = -8f;
                            }
                            if (num39 < 4f && this.direction == 1)
                            {
                                num39 = 8f;
                            }
                            this.itemLocation.X = this.itemLocation.X - num39;
                            this.itemRotation = 0.8f * (float)this.direction;
                        }
                        if (this.gravDir == -1f)
                        {
                            this.itemRotation = -this.itemRotation;
                            this.itemLocation.Y = this.position.Y + (float)this.height + (this.position.Y - this.itemLocation.Y);
                        }
                    }
                    else if (item.useStyle == 4)
                    {
                        int num40 = 0;
                        if (item.type == 3601)
                        {
                            num40 = 10;
                        }
                        this.itemRotation = 0f;
                        this.itemLocation.X = this.position.X + (float)this.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - 9f - this.itemRotation * 14f * (float)this.direction - 4f - (float)num40) * (float)this.direction;
                        this.itemLocation.Y = this.position.Y + (float)Main.itemTexture[item.type].Height * 0.5f + 4f + num;
                        if (this.gravDir == -1f)
                        {
                            this.itemRotation = -this.itemRotation;
                            this.itemLocation.Y = this.position.Y + (float)this.height + (this.position.Y - this.itemLocation.Y);
                        }
                    }
                    else if (item.useStyle == 5)
                    {
                        if (item.type == 3779)
                        {
                            this.itemRotation = 0f;
                            this.itemLocation.X = base.Center.X + (float)(6 * this.direction);
                            this.itemLocation.Y = this.MountedCenter.Y + 6f;
                        }
                        else if (Item.staff[item.type])
                        {
                            float scaleFactor = 6f;
                            if (item.type == 3476)
                            {
                                scaleFactor = 14f;
                            }
                            this.itemLocation = this.MountedCenter;
                            this.itemLocation += this.itemRotation.ToRotationVector2() * scaleFactor * (float)this.direction;
                        }
                        else
                        {
                            this.itemLocation.X = this.position.X + (float)this.width * 0.5f - (float)Main.itemTexture[item.type].Width * 0.5f - (float)(this.direction * 2);
                            this.itemLocation.Y = this.MountedCenter.Y - (float)Main.itemTexture[item.type].Height * 0.5f;
                        }
                    }
                    ItemLoader.UseStyle(item, this);
                }
            }
            else if (item.holdStyle == 1 && !this.pulley)
            {
                if (Main.dedServ)
                {
                    this.itemLocation.X = this.position.X + (float)this.width * 0.5f + 20f * (float)this.direction;
                }
                else if (item.type == 930)
                {
                    this.itemLocation.X = this.position.X + (float)(this.width / 2) * 0.5f - 12f - (float)(2 * this.direction);
                    float num41 = this.position.X + (float)(this.width / 2) + (float)(38 * this.direction);
                    if (this.direction == 1)
                    {
                        num41 -= 10f;
                    }
                    float num42 = this.MountedCenter.Y - 4f * this.gravDir;
                    if (this.gravDir == -1f)
                    {
                        num42 -= 8f;
                    }
                    this.RotateRelativePoint(ref num41, ref num42);
                    int num43 = 0;
                    for (int num44 = 54; num44 < 58; num44++)
                    {
                        if (this.inventory[num44].stack > 0 && this.inventory[num44].ammo == 931)
                        {
                            num43 = this.inventory[num44].type;
                            break;
                        }
                    }
                    if (num43 == 0)
                    {
                        for (int num45 = 0; num45 < 54; num45++)
                        {
                            if (this.inventory[num45].stack > 0 && this.inventory[num45].ammo == 931)
                            {
                                num43 = this.inventory[num45].type;
                                break;
                            }
                        }
                    }
                    if (num43 == 931)
                    {
                        num43 = 127;
                    }
                    else if (num43 == 1614)
                    {
                        num43 = 187;
                    }
                    if (num43 > 0)
                    {
                        int num46 = Dust.NewDust(new Vector2(num41, num42 + this.gfxOffY), 6, 6, num43, 0f, 0f, 100, default(Color), 1.6f);
                        Main.dust[num46].noGravity = true;
                        Dust expr_2FB3_cp_0 = Main.dust[num46];
                        expr_2FB3_cp_0.velocity.Y = expr_2FB3_cp_0.velocity.Y - 4f * this.gravDir;
                    }
                }
                else if (item.type == 968)
                {
                    this.itemLocation.X = this.position.X + (float)this.width * 0.5f + (float)(8 * this.direction);
                    if (this.whoAmI == Main.myPlayer)
                    {
                        int num47 = (int)(this.itemLocation.X + (float)Main.itemTexture[item.type].Width * 0.8f * (float)this.direction) / 16;
                        int num48 = (int)(this.itemLocation.Y + num + (float)(Main.itemTexture[item.type].Height / 2)) / 16;
                        if (Main.tile[num47, num48] == null)
                        {
                            Main.tile[num47, num48] = new Tile();
                        }
                        if (Main.tile[num47, num48].active() && Main.tile[num47, num48].type == 215 && Main.tile[num47, num48].frameY < 54)
                        {
                            this.miscTimer++;
                            if (Main.rand.Next(5) == 0)
                            {
                                this.miscTimer++;
                            }
                            if (this.miscTimer > 900)
                            {
                                this.miscTimer = 0;
                                item.SetDefaults(969, false);
                                if (this.selectedItem == 58)
                                {
                                    Main.mouseItem.SetDefaults(969, false);
                                }
                                for (int num49 = 0; num49 < 58; num49++)
                                {
                                    if (this.inventory[num49].type == item.type && num49 != this.selectedItem && this.inventory[num49].stack < this.inventory[num49].maxStack)
                                    {
                                        Main.PlaySound(7, -1, -1, 1);
                                        this.inventory[num49].stack++;
                                        item.SetDefaults(0, false);
                                        if (this.selectedItem == 58)
                                        {
                                            Main.mouseItem.SetDefaults(0, false);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.miscTimer = 0;
                        }
                    }
                }
                else if (item.type == 856)
                {
                    this.itemLocation.X = this.position.X + (float)this.width * 0.5f + (float)(4 * this.direction);
                }
                else if (item.fishingPole > 0)
                {
                    this.itemLocation.X = this.position.X + (float)this.width * 0.5f + (float)Main.itemTexture[item.type].Width * 0.18f * (float)this.direction;
                }
                else
                {
                    this.itemLocation.X = this.position.X + (float)this.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f + 2f) * (float)this.direction;
                    if (item.type == 282 || item.type == 286 || item.type == 3112)
                    {
                        this.itemLocation.X = this.itemLocation.X - (float)(this.direction * 2);
                        this.itemLocation.Y = this.itemLocation.Y + 4f;
                    }
                    else if (item.type == 3002)
                    {
                        this.itemLocation.X = this.itemLocation.X - (float)(4 * this.direction);
                        this.itemLocation.Y = this.itemLocation.Y + 2f;
                    }
                }
                this.itemLocation.Y = this.position.Y + 24f + num;
                if (item.type == 856)
                {
                    this.itemLocation.Y = this.position.Y + 34f + num;
                }
                if (item.type == 930)
                {
                    this.itemLocation.Y = this.position.Y + 9f + num;
                }
                if (item.fishingPole > 0)
                {
                    this.itemLocation.Y = this.itemLocation.Y + 4f;
                }
                else if (item.type == 3476)
                {
                    this.itemLocation.X = base.Center.X + (float)(14 * this.direction);
                    this.itemLocation.Y = this.MountedCenter.Y;
                }
                else if (item.type == 3779)
                {
                    this.itemLocation.X = base.Center.X + (float)(6 * this.direction);
                    this.itemLocation.Y = this.MountedCenter.Y + 6f;
                }
                this.itemRotation = 0f;
                if (this.gravDir == -1f)
                {
                    this.itemRotation = -this.itemRotation;
                    this.itemLocation.Y = this.position.Y + (float)this.height + (this.position.Y - this.itemLocation.Y) + num;
                    if (item.type == 930)
                    {
                        this.itemLocation.Y = this.itemLocation.Y - 24f;
                    }
                }
            }
            else if (item.holdStyle == 2 && !this.pulley)
            {
                if (item.type == 946)
                {
                    this.itemRotation = 0f;
                    this.itemLocation.X = this.position.X + (float)this.width * 0.5f - (float)(16 * this.direction);
                    this.itemLocation.Y = this.position.Y + 22f + num;
                    this.fallStart = (int)(this.position.Y / 16f);
                    if (this.gravDir == -1f)
                    {
                        this.itemRotation = -this.itemRotation;
                        this.itemLocation.Y = this.position.Y + (float)this.height + (this.position.Y - this.itemLocation.Y);
                        if (this.velocity.Y < -2f)
                        {
                            this.velocity.Y = -2f;
                        }
                    }
                    else if (this.velocity.Y > 2f)
                    {
                        this.velocity.Y = 2f;
                    }
                }
                else
                {
                    this.itemLocation.X = this.position.X + (float)this.width * 0.5f + (float)(6 * this.direction);
                    this.itemLocation.Y = this.position.Y + 16f + num;
                    this.itemRotation = 0.79f * (float)(-(float)this.direction);
                    if (this.gravDir == -1f)
                    {
                        this.itemRotation = -this.itemRotation;
                        this.itemLocation.Y = this.position.Y + (float)this.height + (this.position.Y - this.itemLocation.Y);
                    }
                }
            }
            else if (item.holdStyle == 3 && !this.pulley && !Main.dedServ)
            {
                this.itemLocation.X = this.position.X + (float)this.width * 0.5f - (float)Main.itemTexture[item.type].Width * 0.5f - (float)(this.direction * 2);
                this.itemLocation.Y = this.MountedCenter.Y - (float)Main.itemTexture[item.type].Height * 0.5f;
                this.itemRotation = 0f;
            }
            ItemLoader.HoldStyle(item, this);
            if ((((item.type == 974 || item.type == 8 || item.type == 1245 || item.type == 2274 || item.type == 3004 || item.type == 3045 || item.type == 3114 || (item.type >= 427 && item.type <= 433)) && !this.wet) || item.type == 523 || item.type == 1333) && !this.pulley)
            {
                float num50 = 1f;
                float num51 = 0.95f;
                float num52 = 0.8f;
                int num53 = 0;
                if (item.type == 523)
                {
                    num53 = 8;
                }
                else if (item.type == 974)
                {
                    num53 = 9;
                }
                else if (item.type == 1245)
                {
                    num53 = 10;
                }
                else if (item.type == 1333)
                {
                    num53 = 11;
                }
                else if (item.type == 2274)
                {
                    num53 = 12;
                }
                else if (item.type == 3004)
                {
                    num53 = 13;
                }
                else if (item.type == 3045)
                {
                    num53 = 14;
                }
                else if (item.type == 3114)
                {
                    num53 = 15;
                }
                else if (item.type >= 427)
                {
                    num53 = item.type - 426;
                }
                if (num53 == 1)
                {
                    num50 = 0f;
                    num51 = 0.1f;
                    num52 = 1.3f;
                }
                else if (num53 == 2)
                {
                    num50 = 1f;
                    num51 = 0.1f;
                    num52 = 0.1f;
                }
                else if (num53 == 3)
                {
                    num50 = 0f;
                    num51 = 1f;
                    num52 = 0.1f;
                }
                else if (num53 == 4)
                {
                    num50 = 0.9f;
                    num51 = 0f;
                    num52 = 0.9f;
                }
                else if (num53 == 5)
                {
                    num50 = 1.3f;
                    num51 = 1.3f;
                    num52 = 1.3f;
                }
                else if (num53 == 6)
                {
                    num50 = 0.9f;
                    num51 = 0.9f;
                    num52 = 0f;
                }
                else if (num53 == 7)
                {
                    num50 = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
                    num51 = 0.3f;
                    num52 = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
                }
                else if (num53 == 8)
                {
                    num52 = 0.7f;
                    num50 = 0.85f;
                    num51 = 1f;
                }
                else if (num53 == 9)
                {
                    num52 = 1f;
                    num50 = 0.7f;
                    num51 = 0.85f;
                }
                else if (num53 == 10)
                {
                    num52 = 0f;
                    num50 = 1f;
                    num51 = 0.5f;
                }
                else if (num53 == 11)
                {
                    num52 = 0.8f;
                    num50 = 1.25f;
                    num51 = 1.25f;
                }
                else if (num53 == 12)
                {
                    num50 *= 0.75f;
                    num51 *= 1.3499999f;
                    num52 *= 1.5f;
                }
                else if (num53 == 13)
                {
                    num50 = 0.95f;
                    num51 = 0.65f;
                    num52 = 1.3f;
                }
                else if (num53 == 14)
                {
                    num50 = (float)Main.DiscoR / 255f;
                    num51 = (float)Main.DiscoG / 255f;
                    num52 = (float)Main.DiscoB / 255f;
                }
                else if (num53 == 15)
                {
                    num50 = 1f;
                    num51 = 0f;
                    num52 = 1f;
                }
                int num54 = num53;
                if (num54 == 0)
                {
                    num54 = 6;
                }
                else if (num54 == 8)
                {
                    num54 = 75;
                }
                else if (num54 == 9)
                {
                    num54 = 135;
                }
                else if (num54 == 10)
                {
                    num54 = 158;
                }
                else if (num54 == 11)
                {
                    num54 = 169;
                }
                else if (num54 == 12)
                {
                    num54 = 156;
                }
                else if (num54 == 13)
                {
                    num54 = 234;
                }
                else if (num54 == 14)
                {
                    num54 = 66;
                }
                else if (num54 == 15)
                {
                    num54 = 242;
                }
                else
                {
                    num54 = 58 + num54;
                }
                int maxValue = 30;
                if (this.itemAnimation > 0)
                {
                    maxValue = 7;
                }
                if (this.direction == -1)
                {
                    if (Main.rand.Next(maxValue) == 0)
                    {
                        int num55 = Dust.NewDust(new Vector2(this.itemLocation.X - 16f, this.itemLocation.Y - 14f * this.gravDir), 4, 4, num54, 0f, 0f, 100, default(Color), 1f);
                        if (Main.rand.Next(3) != 0)
                        {
                            Main.dust[num55].noGravity = true;
                        }
                        Main.dust[num55].velocity *= 0.3f;
                        Dust expr_3C8C_cp_0 = Main.dust[num55];
                        expr_3C8C_cp_0.velocity.Y = expr_3C8C_cp_0.velocity.Y - 1.5f;
                        Main.dust[num55].position = this.RotatedRelativePoint(Main.dust[num55].position, true);
                        if (num54 == 66)
                        {
                            Main.dust[num55].color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
                            Main.dust[num55].noGravity = true;
                        }
                    }
                    Vector2 position = this.RotatedRelativePoint(new Vector2(this.itemLocation.X - 12f + this.velocity.X, this.itemLocation.Y - 14f + this.velocity.Y), true);
                    Lighting.AddLight(position, num50, num51, num52);
                }
                else
                {
                    if (Main.rand.Next(maxValue) == 0)
                    {
                        int num56 = Dust.NewDust(new Vector2(this.itemLocation.X + 6f, this.itemLocation.Y - 14f * this.gravDir), 4, 4, num54, 0f, 0f, 100, default(Color), 1f);
                        if (Main.rand.Next(3) != 0)
                        {
                            Main.dust[num56].noGravity = true;
                        }
                        Main.dust[num56].velocity *= 0.3f;
                        Dust expr_3DFB_cp_0 = Main.dust[num56];
                        expr_3DFB_cp_0.velocity.Y = expr_3DFB_cp_0.velocity.Y - 1.5f;
                        Main.dust[num56].position = this.RotatedRelativePoint(Main.dust[num56].position, true);
                        if (num54 == 66)
                        {
                            Main.dust[num56].color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
                            Main.dust[num56].noGravity = true;
                        }
                    }
                    Vector2 position2 = this.RotatedRelativePoint(new Vector2(this.itemLocation.X + 12f + this.velocity.X, this.itemLocation.Y - 14f + this.velocity.Y), true);
                    Lighting.AddLight(position2, num50, num51, num52);
                }
            }
            if ((item.type == 105 || item.type == 713) && !this.wet && !this.pulley)
            {
                int maxValue2 = 20;
                if (this.itemAnimation > 0)
                {
                    maxValue2 = 7;
                }
                if (this.direction == -1)
                {
                    if (Main.rand.Next(maxValue2) == 0)
                    {
                        int num57 = Dust.NewDust(new Vector2(this.itemLocation.X - 12f, this.itemLocation.Y - 20f * this.gravDir), 4, 4, 6, 0f, 0f, 100, default(Color), 1f);
                        if (Main.rand.Next(3) != 0)
                        {
                            Main.dust[num57].noGravity = true;
                        }
                        Main.dust[num57].velocity *= 0.3f;
                        Dust expr_3FB0_cp_0 = Main.dust[num57];
                        expr_3FB0_cp_0.velocity.Y = expr_3FB0_cp_0.velocity.Y - 1.5f;
                        Main.dust[num57].position = this.RotatedRelativePoint(Main.dust[num57].position, true);
                    }
                    Vector2 position3 = this.RotatedRelativePoint(new Vector2(this.itemLocation.X - 16f + this.velocity.X, this.itemLocation.Y - 14f), true);
                    Lighting.AddLight(position3, 1f, 0.95f, 0.8f);
                }
                else
                {
                    if (Main.rand.Next(maxValue2) == 0)
                    {
                        int num58 = Dust.NewDust(new Vector2(this.itemLocation.X + 4f, this.itemLocation.Y - 20f * this.gravDir), 4, 4, 6, 0f, 0f, 100, default(Color), 1f);
                        if (Main.rand.Next(3) != 0)
                        {
                            Main.dust[num58].noGravity = true;
                        }
                        Main.dust[num58].velocity *= 0.3f;
                        Dust expr_40E6_cp_0 = Main.dust[num58];
                        expr_40E6_cp_0.velocity.Y = expr_40E6_cp_0.velocity.Y - 1.5f;
                        Main.dust[num58].position = this.RotatedRelativePoint(Main.dust[num58].position, true);
                    }
                    Vector2 position4 = this.RotatedRelativePoint(new Vector2(this.itemLocation.X + 6f + this.velocity.X, this.itemLocation.Y - 14f), true);
                    Lighting.AddLight(position4, 1f, 0.95f, 0.8f);
                }
            }
            else if (item.type == 148 && !this.wet)
            {
                int maxValue3 = 10;
                if (this.itemAnimation > 0)
                {
                    maxValue3 = 7;
                }
                if (this.direction == -1)
                {
                    if (Main.rand.Next(maxValue3) == 0)
                    {
                        int num59 = Dust.NewDust(new Vector2(this.itemLocation.X - 12f, this.itemLocation.Y - 20f * this.gravDir), 4, 4, 172, 0f, 0f, 100, default(Color), 1f);
                        if (Main.rand.Next(3) != 0)
                        {
                            Main.dust[num59].noGravity = true;
                        }
                        Main.dust[num59].velocity *= 0.3f;
                        Dust expr_4257_cp_0 = Main.dust[num59];
                        expr_4257_cp_0.velocity.Y = expr_4257_cp_0.velocity.Y - 1.5f;
                        Main.dust[num59].position = this.RotatedRelativePoint(Main.dust[num59].position, true);
                    }
                    Vector2 position5 = this.RotatedRelativePoint(new Vector2(this.itemLocation.X - 16f + this.velocity.X, this.itemLocation.Y - 14f), true);
                    Lighting.AddLight(position5, 0f, 0.5f, 1f);
                }
                else
                {
                    if (Main.rand.Next(maxValue3) == 0)
                    {
                        int num60 = Dust.NewDust(new Vector2(this.itemLocation.X + 4f, this.itemLocation.Y - 20f * this.gravDir), 4, 4, 172, 0f, 0f, 100, default(Color), 1f);
                        if (Main.rand.Next(3) != 0)
                        {
                            Main.dust[num60].noGravity = true;
                        }
                        Main.dust[num60].velocity *= 0.3f;
                        Dust expr_4391_cp_0 = Main.dust[num60];
                        expr_4391_cp_0.velocity.Y = expr_4391_cp_0.velocity.Y - 1.5f;
                        Main.dust[num60].position = this.RotatedRelativePoint(Main.dust[num60].position, true);
                    }
                    Vector2 position6 = this.RotatedRelativePoint(new Vector2(this.itemLocation.X + 6f + this.velocity.X, this.itemLocation.Y - 14f), true);
                    Lighting.AddLight(position6, 0f, 0.5f, 1f);
                }
            }
            else if (item.type == 3117 && !this.wet)
            {
                this.itemLocation.X = this.itemLocation.X - (float)(this.direction * 4);
                int maxValue4 = 10;
                if (this.itemAnimation > 0)
                {
                    maxValue4 = 7;
                }
                if (this.direction == -1)
                {
                    if (Main.rand.Next(maxValue4) == 0)
                    {
                        int num61 = Dust.NewDust(new Vector2(this.itemLocation.X - 10f, this.itemLocation.Y - 20f * this.gravDir), 4, 4, 242, 0f, 0f, 100, default(Color), 1f);
                        if (Main.rand.Next(3) != 0)
                        {
                            Main.dust[num61].noGravity = true;
                        }
                        Main.dust[num61].velocity *= 0.3f;
                        Dust expr_451D_cp_0 = Main.dust[num61];
                        expr_451D_cp_0.velocity.Y = expr_451D_cp_0.velocity.Y - 1.5f;
                        Main.dust[num61].position = this.RotatedRelativePoint(Main.dust[num61].position, true);
                    }
                    Vector2 position7 = this.RotatedRelativePoint(new Vector2(this.itemLocation.X - 16f + this.velocity.X, this.itemLocation.Y - 14f), true);
                    Lighting.AddLight(position7, 0.9f, 0.1f, 0.75f);
                }
                else
                {
                    if (Main.rand.Next(maxValue4) == 0)
                    {
                        int num62 = Dust.NewDust(new Vector2(this.itemLocation.X + 6f, this.itemLocation.Y - 20f * this.gravDir), 4, 4, 242, 0f, 0f, 100, default(Color), 1f);
                        if (Main.rand.Next(3) != 0)
                        {
                            Main.dust[num62].noGravity = true;
                        }
                        Main.dust[num62].velocity *= 0.3f;
                        Dust expr_4657_cp_0 = Main.dust[num62];
                        expr_4657_cp_0.velocity.Y = expr_4657_cp_0.velocity.Y - 1.5f;
                        Main.dust[num62].position = this.RotatedRelativePoint(Main.dust[num62].position, true);
                    }
                    Vector2 position8 = this.RotatedRelativePoint(new Vector2(this.itemLocation.X + 6f + this.velocity.X, this.itemLocation.Y - 14f), true);
                    Lighting.AddLight(position8, 0.9f, 0.1f, 0.75f);
                }
            }
            if (item.type == 282 && !this.pulley)
            {
                if (this.direction == -1)
                {
                    Vector2 position9 = this.RotatedRelativePoint(new Vector2(this.itemLocation.X - 16f + this.velocity.X, this.itemLocation.Y - 14f), true);
                    Lighting.AddLight(position9, 0.7f, 1f, 0.8f);
                }
                else
                {
                    Vector2 position10 = this.RotatedRelativePoint(new Vector2(this.itemLocation.X + 6f + this.velocity.X, this.itemLocation.Y - 14f), true);
                    Lighting.AddLight(position10, 0.7f, 1f, 0.8f);
                }
            }
            if (item.type == 3002 && !this.pulley)
            {
                float r = 1.05f;
                float g = 0.95f;
                float b = 0.55f;
                if (this.direction == -1)
                {
                    Vector2 position11 = this.RotatedRelativePoint(new Vector2(this.itemLocation.X - 16f + this.velocity.X, this.itemLocation.Y - 14f), true);
                    Lighting.AddLight(position11, r, g, b);
                }
                else
                {
                    Vector2 position12 = this.RotatedRelativePoint(new Vector2(this.itemLocation.X + 6f + this.velocity.X, this.itemLocation.Y - 14f), true);
                    Lighting.AddLight(position12, r, g, b);
                }
                this.spelunkerTimer += 1;
                if (this.spelunkerTimer >= 10)
                {
                    this.spelunkerTimer = 0;
                    int num63 = 30;
                    int num64 = (int)base.Center.X / 16;
                    int num65 = (int)base.Center.Y / 16;
                    for (int num66 = num64 - num63; num66 <= num64 + num63; num66++)
                    {
                        for (int num67 = num65 - num63; num67 <= num65 + num63; num67++)
                        {
                            if (Main.rand.Next(4) == 0)
                            {
                                Vector2 vector = new Vector2((float)(num64 - num66), (float)(num65 - num67));
                                if (vector.Length() < (float)num63 && num66 > 0 && num66 < Main.maxTilesX - 1 && num67 > 0 && num67 < Main.maxTilesY - 1 && Main.tile[num66, num67] != null && Main.tile[num66, num67].active())
                                {
                                    bool flag7 = false;
                                    if (Main.tile[num66, num67].type == 185 && Main.tile[num66, num67].frameY == 18)
                                    {
                                        if (Main.tile[num66, num67].frameX >= 576 && Main.tile[num66, num67].frameX <= 882)
                                        {
                                            flag7 = true;
                                        }
                                    }
                                    else if (Main.tile[num66, num67].type == 186 && Main.tile[num66, num67].frameX >= 864 && Main.tile[num66, num67].frameX <= 1170)
                                    {
                                        flag7 = true;
                                    }
                                    if (flag7 || Main.tileSpelunker[(int)Main.tile[num66, num67].type] || (Main.tileAlch[(int)Main.tile[num66, num67].type] && Main.tile[num66, num67].type != 82))
                                    {
                                        int num68 = Dust.NewDust(new Vector2((float)(num66 * 16), (float)(num67 * 16)), 16, 16, 204, 0f, 0f, 150, default(Color), 0.3f);
                                        Main.dust[num68].fadeIn = 0.75f;
                                        Main.dust[num68].velocity *= 0.1f;
                                        Main.dust[num68].noLight = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (item.type == 286 && !this.pulley)
            {
                if (this.direction == -1)
                {
                    Vector2 position13 = this.RotatedRelativePoint(new Vector2(this.itemLocation.X - 16f + this.velocity.X, this.itemLocation.Y - 14f), true);
                    Lighting.AddLight(position13, 0.7f, 0.8f, 1f);
                }
                else
                {
                    Vector2 position14 = this.RotatedRelativePoint(new Vector2(this.itemLocation.X + 6f + this.velocity.X, this.itemLocation.Y - 14f), true);
                    Lighting.AddLight(position14, 0.7f, 0.8f, 1f);
                }
            }
            if (item.type == 3112 && !this.pulley)
            {
                if (this.direction == -1)
                {
                    Vector2 position15 = this.RotatedRelativePoint(new Vector2(this.itemLocation.X - 16f + this.velocity.X, this.itemLocation.Y - 14f), true);
                    Lighting.AddLight(position15, 1f, 0.6f, 0.85f);
                }
                else
                {
                    Vector2 position16 = this.RotatedRelativePoint(new Vector2(this.itemLocation.X + 6f + this.velocity.X, this.itemLocation.Y - 14f), true);
                    Lighting.AddLight(position16, 1f, 0.6f, 0.85f);
                }
            }
            ItemLoader.HoldItem(item, this);
            if (this.controlUseItem)
            {
                this.releaseUseItem = false;
            }
            else
            {
                this.releaseUseItem = true;
            }
            if (this.itemTime > 0)
            {
                this.itemTime--;
                if (this.itemTime == 0 && this.whoAmI == Main.myPlayer)
                {
                    int type3 = item.type;
                    if (type3 == 65 || type3 == 676 || type3 == 723 || type3 == 724 || type3 == 989 || type3 == 1226 || type3 == 1227)
                    {
                        Main.PlaySound(25, -1, -1, 1);
                        for (int num69 = 0; num69 < 5; num69++)
                        {
                            int num70 = Dust.NewDust(this.position, this.width, this.height, 45, 0f, 0f, 255, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
                            Main.dust[num70].noLight = true;
                            Main.dust[num70].noGravity = true;
                            Main.dust[num70].velocity *= 0.5f;
                        }
                    }
                }
            }
            if (i == Main.myPlayer)
            {
                bool flag8 = true;
                int type4 = item.type;
                if ((type4 == 65 || type4 == 676 || type4 == 723 || type4 == 724 || type4 == 757 || type4 == 674 || type4 == 675 || type4 == 989 || type4 == 1226 || type4 == 1227) && this.itemAnimation != this.itemAnimationMax - 1)
                {
                    flag8 = false;
                }
                flag8 = flag8 && ItemLoader.CheckProjOnSwing(this, item);
                if (item.shoot > 0 && this.itemAnimation > 0 && this.itemTime == 0 && flag8)
                {
                    int num71 = item.shoot;
                    float num72 = item.shootSpeed;
                    if (this.inventory[this.selectedItem].thrown && num72 < 16f)
                    {
                        num72 *= this.thrownVelocity;
                        if (num72 > 16f)
                        {
                            num72 = 16f;
                        }
                    }
                    if (item.melee && num71 != 25 && num71 != 26 && num71 != 35)
                    {
                        num72 /= this.meleeSpeed;
                    }
                    bool flag9 = false;
                    int num73 = weaponDamage;
                    float num74 = item.knockBack;
                    if (num71 == 13 || num71 == 32 || num71 == 315 || (num71 >= 230 && num71 <= 235) || num71 == 331)
                    {
                        this.grappling[0] = -1;
                        this.grapCount = 0;
                        for (int num75 = 0; num75 < 1000; num75++)
                        {
                            if (Main.projectile[num75].active && Main.projectile[num75].owner == i)
                            {
                                if (Main.projectile[num75].type == 13)
                                {
                                    Main.projectile[num75].Kill();
                                }
                                if (Main.projectile[num75].type == 331)
                                {
                                    Main.projectile[num75].Kill();
                                }
                                if (Main.projectile[num75].type == 315)
                                {
                                    Main.projectile[num75].Kill();
                                }
                                if (Main.projectile[num75].type >= 230 && Main.projectile[num75].type <= 235)
                                {
                                    Main.projectile[num75].Kill();
                                }
                            }
                        }
                    }
                    if (item.useAmmo > 0)
                    {
                        this.PickAmmo(item, ref num71, ref num72, ref flag9, ref num73, ref num74, ItemID.Sets.gunProj[item.type]);
                    }
                    else
                    {
                        flag9 = true;
                    }
                    if (item.type == 3475 || item.type == 3540)
                    {
                        num74 = item.knockBack;
                        num73 = weaponDamage;
                        num72 = item.shootSpeed;
                    }
                    if (item.type == 71)
                    {
                        flag9 = false;
                    }
                    if (item.type == 72)
                    {
                        flag9 = false;
                    }
                    if (item.type == 73)
                    {
                        flag9 = false;
                    }
                    if (item.type == 74)
                    {
                        flag9 = false;
                    }
                    if (item.type == 1254 && num71 == 14)
                    {
                        num71 = 242;
                    }
                    if (item.type == 1255 && num71 == 14)
                    {
                        num71 = 242;
                    }
                    if (item.type == 1265 && num71 == 14)
                    {
                        num71 = 242;
                    }
                    if (item.type == 3542)
                    {
                        bool flag10 = Main.rand.Next(100) < 20;
                        if (flag10)
                        {
                            num71++;
                            num73 *= 3;
                        }
                        else
                        {
                            num72 -= 1f;
                        }
                    }
                    if (num71 == 73)
                    {
                        for (int num76 = 0; num76 < 1000; num76++)
                        {
                            if (Main.projectile[num76].active && Main.projectile[num76].owner == i)
                            {
                                if (Main.projectile[num76].type == 73)
                                {
                                    num71 = 74;
                                }
                                if (num71 == 74 && Main.projectile[num76].type == 74)
                                {
                                    flag9 = false;
                                }
                            }
                        }
                    }
                    if (flag9)
                    {
                        num74 = this.GetWeaponKnockback(item, num74);
                        if (num71 == 228)
                        {
                            num74 = 0f;
                        }
                        if (num71 == 1 && item.type == 120)
                        {
                            num71 = 2;
                        }
                        if (item.type == 682)
                        {
                            num71 = 117;
                        }
                        if (item.type == 725)
                        {
                            num71 = 120;
                        }
                        if (item.type == 2796)
                        {
                            num71 = 442;
                        }
                        if (item.type == 2223)
                        {
                            num71 = 357;
                        }
                        this.itemTime = item.useTime;
                        Vector2 vector2 = this.RotatedRelativePoint(this.MountedCenter, true);
                        bool flag11 = true;
                        int type5 = item.type;
                        if (type5 == 3611)
                        {
                            flag11 = false;
                        }
                        Vector2 value = Vector2.UnitX.RotatedBy((double)this.fullRotation, default(Vector2));
                        Vector2 vector3 = Main.MouseWorld - vector2;
                        if (vector3 != Vector2.Zero)
                        {
                            vector3.Normalize();
                        }
                        float num77 = Vector2.Dot(value, vector3);
                        if (flag11)
                        {
                            if (num77 > 0f)
                            {
                                this.ChangeDir(1);
                            }
                            else
                            {
                                this.ChangeDir(-1);
                            }
                        }
                        if (item.type == 3094 || item.type == 3378 || item.type == 3543)
                        {
                            vector2.Y = this.position.Y + (float)(this.height / 3);
                        }
                        if (item.type == 2611)
                        {
                            Vector2 vector4 = vector3;
                            if (vector4 != Vector2.Zero)
                            {
                                vector4.Normalize();
                            }
                            vector2 += vector4;
                        }
                        if (num71 == 9)
                        {
                            vector2 = new Vector2(this.position.X + (float)this.width * 0.5f + (float)(Main.rand.Next(201) * -(float)this.direction) + ((float)Main.mouseX + Main.screenPosition.X - this.position.X), this.MountedCenter.Y - 600f);
                            num74 = 0f;
                            num73 *= 2;
                        }
                        if (item.type == 986 || item.type == 281)
                        {
                            vector2.X += (float)(6 * this.direction);
                            vector2.Y -= 6f * this.gravDir;
                        }
                        if (item.type == 3007)
                        {
                            vector2.X -= (float)(4 * this.direction);
                            vector2.Y -= 1f * this.gravDir;
                        }
                        float num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                        float num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                        if (this.gravDir == -1f)
                        {
                            num79 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector2.Y;
                        }
                        float num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
                        float num81 = num80;
                        if ((float.IsNaN(num78) && float.IsNaN(num79)) || (num78 == 0f && num79 == 0f))
                        {
                            num78 = (float)this.direction;
                            num79 = 0f;
                            num80 = num72;
                        }
                        else
                        {
                            num80 = num72 / num80;
                        }
                        if (item.type == 1929 || item.type == 2270)
                        {
                            num78 += (float)Main.rand.Next(-50, 51) * 0.03f / num80;
                            num79 += (float)Main.rand.Next(-50, 51) * 0.03f / num80;
                        }
                        num78 *= num80;
                        num79 *= num80;
                        if (item.type == 757)
                        {
                            num73 = (int)((float)num73 * 1.25f);
                        }
                        if (num71 == 250)
                        {
                            for (int num82 = 0; num82 < 1000; num82++)
                            {
                                if (Main.projectile[num82].active && Main.projectile[num82].owner == this.whoAmI && (Main.projectile[num82].type == 250 || Main.projectile[num82].type == 251))
                                {
                                    Main.projectile[num82].Kill();
                                }
                            }
                        }
                        if (num71 == 12)
                        {
                            vector2.X += num78 * 3f;
                            vector2.Y += num79 * 3f;
                        }
                        if (item.useStyle == 5)
                        {
                            if (item.type == 3029)
                            {
                                Vector2 vector5 = new Vector2(num78, num79);
                                vector5.X = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                                vector5.Y = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y - 1000f;
                                this.itemRotation = (float)Math.Atan2((double)(vector5.Y * (float)this.direction), (double)(vector5.X * (float)this.direction));
                                NetMessage.SendData(13, -1, -1, "", this.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                                NetMessage.SendData(41, -1, -1, "", this.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                            }
                            else if (item.type == 3779)
                            {
                                this.itemRotation = 0f;
                                NetMessage.SendData(13, -1, -1, "", this.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                                NetMessage.SendData(41, -1, -1, "", this.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                            }
                            else
                            {
                                this.itemRotation = (float)Math.Atan2((double)(num79 * (float)this.direction), (double)(num78 * (float)this.direction)) - this.fullRotation;
                                NetMessage.SendData(13, -1, -1, "", this.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                                NetMessage.SendData(41, -1, -1, "", this.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                            }
                        }
                        if (num71 == 17)
                        {
                            vector2.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            if (this.gravDir == -1f)
                            {
                                vector2.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
                            }
                        }
                        if (num71 == 76)
                        {
                            num71 += Main.rand.Next(3);
                            num81 /= (float)(Main.screenHeight / 2);
                            if (num81 > 1f)
                            {
                                num81 = 1f;
                            }
                            float num83 = num78 + (float)Main.rand.Next(-40, 41) * 0.01f;
                            float num84 = num79 + (float)Main.rand.Next(-40, 41) * 0.01f;
                            num83 *= num81 + 0.25f;
                            num84 *= num81 + 0.25f;
                            int num85 = Projectile.NewProjectile(vector2.X, vector2.Y, num83, num84, num71, num73, num74, i, 0f, 0f);
                            Main.projectile[num85].ai[1] = 1f;
                            num81 = num81 * 2f - 1f;
                            if (num81 < -1f)
                            {
                                num81 = -1f;
                            }
                            if (num81 > 1f)
                            {
                                num81 = 1f;
                            }
                            Main.projectile[num85].ai[0] = num81;
                            NetMessage.SendData(27, -1, -1, "", num85, 0f, 0f, 0f, 0, 0, 0);
                        }
                        else if (item.type == 3029)
                        {
                            int num86 = 3;
                            if (Main.rand.Next(3) == 0)
                            {
                                num86++;
                            }
                            for (int num87 = 0; num87 < num86; num87++)
                            {
                                vector2 = new Vector2(this.position.X + (float)this.width * 0.5f + (float)(Main.rand.Next(201) * -(float)this.direction) + ((float)Main.mouseX + Main.screenPosition.X - this.position.X), this.MountedCenter.Y - 600f);
                                vector2.X = (vector2.X * 10f + base.Center.X) / 11f + (float)Main.rand.Next(-100, 101);
                                vector2.Y -= (float)(150 * num87);
                                num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                                num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                                if (num79 < 0f)
                                {
                                    num79 *= -1f;
                                }
                                if (num79 < 20f)
                                {
                                    num79 = 20f;
                                }
                                num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
                                num80 = num72 / num80;
                                num78 *= num80;
                                num79 *= num80;
                                float num88 = num78 + (float)Main.rand.Next(-40, 41) * 0.03f;
                                float speedY = num79 + (float)Main.rand.Next(-40, 41) * 0.03f;
                                num88 *= (float)Main.rand.Next(75, 150) * 0.01f;
                                vector2.X += (float)Main.rand.Next(-50, 51);
                                int num89 = Projectile.NewProjectile(vector2.X, vector2.Y, num88, speedY, num71, num73, num74, i, 0f, 0f);
                                Main.projectile[num89].noDropItem = true;
                            }
                        }
                        else if (item.type == 98 || item.type == 533)
                        {
                            float speedX = num78 + (float)Main.rand.Next(-40, 41) * 0.01f;
                            float speedY2 = num79 + (float)Main.rand.Next(-40, 41) * 0.01f;
                            Projectile.NewProjectile(vector2.X, vector2.Y, speedX, speedY2, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 1319)
                        {
                            float speedX2 = num78 + (float)Main.rand.Next(-40, 41) * 0.02f;
                            float speedY3 = num79 + (float)Main.rand.Next(-40, 41) * 0.02f;
                            int num90 = Projectile.NewProjectile(vector2.X, vector2.Y, speedX2, speedY3, num71, num73, num74, i, 0f, 0f);
                            Main.projectile[num90].ranged = true;
                            Main.projectile[num90].thrown = false;
                        }
                        else if (item.type == 3107)
                        {
                            float speedX3 = num78 + (float)Main.rand.Next(-40, 41) * 0.02f;
                            float speedY4 = num79 + (float)Main.rand.Next(-40, 41) * 0.02f;
                            Projectile.NewProjectile(vector2.X, vector2.Y, speedX3, speedY4, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 3053)
                        {
                            Vector2 value2 = new Vector2(num78, num79);
                            value2.Normalize();
                            Vector2 value3 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                            value3.Normalize();
                            value2 = value2 * 4f + value3;
                            value2.Normalize();
                            value2 *= item.shootSpeed;
                            float num91 = (float)Main.rand.Next(10, 80) * 0.001f;
                            if (Main.rand.Next(2) == 0)
                            {
                                num91 *= -1f;
                            }
                            float num92 = (float)Main.rand.Next(10, 80) * 0.001f;
                            if (Main.rand.Next(2) == 0)
                            {
                                num92 *= -1f;
                            }
                            Projectile.NewProjectile(vector2.X, vector2.Y, value2.X, value2.Y, num71, num73, num74, i, num92, num91);
                        }
                        else if (item.type == 3019)
                        {
                            Vector2 value4 = new Vector2(num78, num79);
                            float num93 = value4.Length();
                            value4.X += (float)Main.rand.Next(-100, 101) * 0.01f * num93 * 0.15f;
                            value4.Y += (float)Main.rand.Next(-100, 101) * 0.01f * num93 * 0.15f;
                            float num94 = num78 + (float)Main.rand.Next(-40, 41) * 0.03f;
                            float num95 = num79 + (float)Main.rand.Next(-40, 41) * 0.03f;
                            value4.Normalize();
                            value4 *= num93;
                            num94 *= (float)Main.rand.Next(50, 150) * 0.01f;
                            num95 *= (float)Main.rand.Next(50, 150) * 0.01f;
                            Vector2 value5 = new Vector2(num94, num95);
                            value5.X += (float)Main.rand.Next(-100, 101) * 0.025f;
                            value5.Y += (float)Main.rand.Next(-100, 101) * 0.025f;
                            value5.Normalize();
                            value5 *= num93;
                            num94 = value5.X;
                            num95 = value5.Y;
                            Projectile.NewProjectile(vector2.X, vector2.Y, num94, num95, num71, num73, num74, i, value4.X, value4.Y);
                        }
                        else if (item.type == 2797)
                        {
                            Vector2 value6 = Vector2.Normalize(new Vector2(num78, num79)) * 40f * item.scale;
                            if (Collision.CanHit(vector2, 0, 0, vector2 + value6, 0, 0))
                            {
                                vector2 += value6;
                            }
                            float ai = new Vector2(num78, num79).ToRotation();
                            float num96 = 2.09439516f;
                            int num97 = Main.rand.Next(4, 5);
                            if (Main.rand.Next(4) == 0)
                            {
                                num97++;
                            }
                            for (int num98 = 0; num98 < num97; num98++)
                            {
                                float scaleFactor2 = (float)Main.rand.NextDouble() * 0.2f + 0.05f;
                                Vector2 vector6 = new Vector2(num78, num79).RotatedBy((double)(num96 * (float)Main.rand.NextDouble() - num96 / 2f), default(Vector2)) * scaleFactor2;
                                int num99 = Projectile.NewProjectile(vector2.X, vector2.Y, vector6.X, vector6.Y, 444, num73, num74, i, ai, 0f);
                                Main.projectile[num99].localAI[0] = (float)num71;
                                Main.projectile[num99].localAI[1] = num72;
                            }
                        }
                        else if (item.type == 2270)
                        {
                            float num100 = num78 + (float)Main.rand.Next(-40, 41) * 0.05f;
                            float num101 = num79 + (float)Main.rand.Next(-40, 41) * 0.05f;
                            if (Main.rand.Next(3) == 0)
                            {
                                num100 *= 1f + (float)Main.rand.Next(-30, 31) * 0.02f;
                                num101 *= 1f + (float)Main.rand.Next(-30, 31) * 0.02f;
                            }
                            Projectile.NewProjectile(vector2.X, vector2.Y, num100, num101, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 1930)
                        {
                            int num102 = 2 + Main.rand.Next(3);
                            for (int num103 = 0; num103 < num102; num103++)
                            {
                                float num104 = num78;
                                float num105 = num79;
                                float num106 = 0.025f * (float)num103;
                                num104 += (float)Main.rand.Next(-35, 36) * num106;
                                num105 += (float)Main.rand.Next(-35, 36) * num106;
                                num80 = (float)Math.Sqrt((double)(num104 * num104 + num105 * num105));
                                num80 = num72 / num80;
                                num104 *= num80;
                                num105 *= num80;
                                float x = vector2.X + num78 * (float)(num102 - num103) * 1.75f;
                                float y = vector2.Y + num79 * (float)(num102 - num103) * 1.75f;
                                Projectile.NewProjectile(x, y, num104, num105, num71, num73, num74, i, (float)Main.rand.Next(0, 10 * (num103 + 1)), 0f);
                            }
                        }
                        else if (item.type == 1931)
                        {
                            int num107 = 2;
                            for (int num108 = 0; num108 < num107; num108++)
                            {
                                vector2 = new Vector2(this.position.X + (float)this.width * 0.5f + (float)(Main.rand.Next(201) * -(float)this.direction) + ((float)Main.mouseX + Main.screenPosition.X - this.position.X), this.MountedCenter.Y - 600f);
                                vector2.X = (vector2.X + base.Center.X) / 2f + (float)Main.rand.Next(-200, 201);
                                vector2.Y -= (float)(100 * num108);
                                num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                                num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                                if (num79 < 0f)
                                {
                                    num79 *= -1f;
                                }
                                if (num79 < 20f)
                                {
                                    num79 = 20f;
                                }
                                num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
                                num80 = num72 / num80;
                                num78 *= num80;
                                num79 *= num80;
                                float speedX4 = num78 + (float)Main.rand.Next(-40, 41) * 0.02f;
                                float speedY5 = num79 + (float)Main.rand.Next(-40, 41) * 0.02f;
                                Projectile.NewProjectile(vector2.X, vector2.Y, speedX4, speedY5, num71, num73, num74, i, 0f, (float)Main.rand.Next(5));
                            }
                        }
                        else if (item.type == 2750)
                        {
                            int num109 = 1;
                            for (int num110 = 0; num110 < num109; num110++)
                            {
                                vector2 = new Vector2(this.position.X + (float)this.width * 0.5f + (float)(Main.rand.Next(201) * -(float)this.direction) + ((float)Main.mouseX + Main.screenPosition.X - this.position.X), this.MountedCenter.Y - 600f);
                                vector2.X = (vector2.X + base.Center.X) / 2f + (float)Main.rand.Next(-200, 201);
                                vector2.Y -= (float)(100 * num110);
                                num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X + (float)Main.rand.Next(-40, 41) * 0.03f;
                                num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                                if (num79 < 0f)
                                {
                                    num79 *= -1f;
                                }
                                if (num79 < 20f)
                                {
                                    num79 = 20f;
                                }
                                num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
                                num80 = num72 / num80;
                                num78 *= num80;
                                num79 *= num80;
                                float num111 = num78;
                                float num112 = num79 + (float)Main.rand.Next(-40, 41) * 0.02f;
                                Projectile.NewProjectile(vector2.X, vector2.Y, num111 * 0.75f, num112 * 0.75f, num71 + Main.rand.Next(3), num73, num74, i, 0f, 0.5f + (float)Main.rand.NextDouble() * 0.3f);
                            }
                        }
                        else if (item.type == 3570)
                        {
                            int num113 = 3;
                            for (int num114 = 0; num114 < num113; num114++)
                            {
                                vector2 = new Vector2(this.position.X + (float)this.width * 0.5f + (float)(Main.rand.Next(201) * -(float)this.direction) + ((float)Main.mouseX + Main.screenPosition.X - this.position.X), this.MountedCenter.Y - 600f);
                                vector2.X = (vector2.X + base.Center.X) / 2f + (float)Main.rand.Next(-200, 201);
                                vector2.Y -= (float)(100 * num114);
                                num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                                num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                                float ai2 = num79 + vector2.Y;
                                if (num79 < 0f)
                                {
                                    num79 *= -1f;
                                }
                                if (num79 < 20f)
                                {
                                    num79 = 20f;
                                }
                                num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
                                num80 = num72 / num80;
                                num78 *= num80;
                                num79 *= num80;
                                Vector2 vector7 = new Vector2(num78, num79) / 2f;
                                Projectile.NewProjectile(vector2.X, vector2.Y, vector7.X, vector7.Y, num71, num73, num74, i, 0f, ai2);
                            }
                        }
                        else if (item.type == 3065)
                        {
                            Vector2 value7 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
                            float num115 = value7.Y;
                            if (num115 > base.Center.Y - 200f)
                            {
                                num115 = base.Center.Y - 200f;
                            }
                            for (int num116 = 0; num116 < 3; num116++)
                            {
                                vector2 = base.Center + new Vector2((float)(-(float)Main.rand.Next(0, 401) * this.direction), -600f);
                                vector2.Y -= (float)(100 * num116);
                                Vector2 value8 = value7 - vector2;
                                if (value8.Y < 0f)
                                {
                                    value8.Y *= -1f;
                                }
                                if (value8.Y < 20f)
                                {
                                    value8.Y = 20f;
                                }
                                value8.Normalize();
                                value8 *= num72;
                                num78 = value8.X;
                                num79 = value8.Y;
                                float speedX5 = num78;
                                float speedY6 = num79 + (float)Main.rand.Next(-40, 41) * 0.02f;
                                Projectile.NewProjectile(vector2.X, vector2.Y, speedX5, speedY6, num71, num73 * 2, num74, i, 0f, num115);
                            }
                        }
                        else if (item.type == 2624)
                        {
                            float num117 = 0.314159274f;
                            int num118 = 5;
                            Vector2 vector8 = new Vector2(num78, num79);
                            vector8.Normalize();
                            vector8 *= 40f;
                            bool flag12 = Collision.CanHit(vector2, 0, 0, vector2 + vector8, 0, 0);
                            for (int num119 = 0; num119 < num118; num119++)
                            {
                                float num120 = (float)num119 - ((float)num118 - 1f) / 2f;
                                Vector2 value9 = vector8.RotatedBy((double)(num117 * num120), default(Vector2));
                                if (!flag12)
                                {
                                    value9 -= vector8;
                                }
                                int num121 = Projectile.NewProjectile(vector2.X + value9.X, vector2.Y + value9.Y, num78, num79, num71, num73, num74, i, 0f, 0f);
                                Main.projectile[num121].noDropItem = true;
                            }
                        }
                        else if (item.type == 1929)
                        {
                            float speedX6 = num78 + (float)Main.rand.Next(-40, 41) * 0.03f;
                            float speedY7 = num79 + (float)Main.rand.Next(-40, 41) * 0.03f;
                            Projectile.NewProjectile(vector2.X, vector2.Y, speedX6, speedY7, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 1553)
                        {
                            float speedX7 = num78 + (float)Main.rand.Next(-40, 41) * 0.005f;
                            float speedY8 = num79 + (float)Main.rand.Next(-40, 41) * 0.005f;
                            Projectile.NewProjectile(vector2.X, vector2.Y, speedX7, speedY8, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 518)
                        {
                            float num122 = num78;
                            float num123 = num79;
                            num122 += (float)Main.rand.Next(-40, 41) * 0.04f;
                            num123 += (float)Main.rand.Next(-40, 41) * 0.04f;
                            Projectile.NewProjectile(vector2.X, vector2.Y, num122, num123, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 1265)
                        {
                            float num124 = num78;
                            float num125 = num79;
                            num124 += (float)Main.rand.Next(-30, 31) * 0.03f;
                            num125 += (float)Main.rand.Next(-30, 31) * 0.03f;
                            Projectile.NewProjectile(vector2.X, vector2.Y, num124, num125, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 534)
                        {
                            int num126 = Main.rand.Next(4, 6);
                            for (int num127 = 0; num127 < num126; num127++)
                            {
                                float num128 = num78;
                                float num129 = num79;
                                num128 += (float)Main.rand.Next(-40, 41) * 0.05f;
                                num129 += (float)Main.rand.Next(-40, 41) * 0.05f;
                                Projectile.NewProjectile(vector2.X, vector2.Y, num128, num129, num71, num73, num74, i, 0f, 0f);
                            }
                        }
                        else if (item.type == 2188)
                        {
                            int num130 = 4;
                            if (Main.rand.Next(3) == 0)
                            {
                                num130++;
                            }
                            if (Main.rand.Next(4) == 0)
                            {
                                num130++;
                            }
                            if (Main.rand.Next(5) == 0)
                            {
                                num130++;
                            }
                            for (int num131 = 0; num131 < num130; num131++)
                            {
                                float num132 = num78;
                                float num133 = num79;
                                float num134 = 0.05f * (float)num131;
                                num132 += (float)Main.rand.Next(-35, 36) * num134;
                                num133 += (float)Main.rand.Next(-35, 36) * num134;
                                num80 = (float)Math.Sqrt((double)(num132 * num132 + num133 * num133));
                                num80 = num72 / num80;
                                num132 *= num80;
                                num133 *= num80;
                                float x2 = vector2.X;
                                float y2 = vector2.Y;
                                Projectile.NewProjectile(x2, y2, num132, num133, num71, num73, num74, i, 0f, 0f);
                            }
                        }
                        else if (item.type == 1308)
                        {
                            int num135 = 3;
                            if (Main.rand.Next(3) == 0)
                            {
                                num135++;
                            }
                            for (int num136 = 0; num136 < num135; num136++)
                            {
                                float num137 = num78;
                                float num138 = num79;
                                float num139 = 0.05f * (float)num136;
                                num137 += (float)Main.rand.Next(-35, 36) * num139;
                                num138 += (float)Main.rand.Next(-35, 36) * num139;
                                num80 = (float)Math.Sqrt((double)(num137 * num137 + num138 * num138));
                                num80 = num72 / num80;
                                num137 *= num80;
                                num138 *= num80;
                                float x3 = vector2.X;
                                float y3 = vector2.Y;
                                Projectile.NewProjectile(x3, y3, num137, num138, num71, num73, num74, i, 0f, 0f);
                            }
                        }
                        else if (item.type == 1258)
                        {
                            float num140 = num78;
                            float num141 = num79;
                            num140 += (float)Main.rand.Next(-40, 41) * 0.01f;
                            num141 += (float)Main.rand.Next(-40, 41) * 0.01f;
                            vector2.X += (float)Main.rand.Next(-40, 41) * 0.05f;
                            vector2.Y += (float)Main.rand.Next(-45, 36) * 0.05f;
                            Projectile.NewProjectile(vector2.X, vector2.Y, num140, num141, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 964)
                        {
                            int num142 = Main.rand.Next(3, 5);
                            for (int num143 = 0; num143 < num142; num143++)
                            {
                                float num144 = num78;
                                float num145 = num79;
                                num144 += (float)Main.rand.Next(-35, 36) * 0.04f;
                                num145 += (float)Main.rand.Next(-35, 36) * 0.04f;
                                Projectile.NewProjectile(vector2.X, vector2.Y, num144, num145, num71, num73, num74, i, 0f, 0f);
                            }
                        }
                        else if (item.type == 1569)
                        {
                            int num146 = 4;
                            if (Main.rand.Next(2) == 0)
                            {
                                num146++;
                            }
                            if (Main.rand.Next(4) == 0)
                            {
                                num146++;
                            }
                            if (Main.rand.Next(8) == 0)
                            {
                                num146++;
                            }
                            if (Main.rand.Next(16) == 0)
                            {
                                num146++;
                            }
                            for (int num147 = 0; num147 < num146; num147++)
                            {
                                float num148 = num78;
                                float num149 = num79;
                                float num150 = 0.05f * (float)num147;
                                num148 += (float)Main.rand.Next(-35, 36) * num150;
                                num149 += (float)Main.rand.Next(-35, 36) * num150;
                                num80 = (float)Math.Sqrt((double)(num148 * num148 + num149 * num149));
                                num80 = num72 / num80;
                                num148 *= num80;
                                num149 *= num80;
                                float x4 = vector2.X;
                                float y4 = vector2.Y;
                                Projectile.NewProjectile(x4, y4, num148, num149, num71, num73, num74, i, 0f, 0f);
                            }
                        }
                        else if (item.type == 1572 || item.type == 2366 || item.type == 3571 || item.type == 3569)
                        {
                            int shoot = item.shoot;
                            for (int num151 = 0; num151 < 1000; num151++)
                            {
                                if (Main.projectile[num151].owner == this.whoAmI && Main.projectile[num151].type == shoot)
                                {
                                    Main.projectile[num151].Kill();
                                }
                            }
                            bool flag13 = item.type == 3571 || item.type == 3569;
                            int num152 = (int)((float)Main.mouseX + Main.screenPosition.X) / 16;
                            int num153 = (int)((float)Main.mouseY + Main.screenPosition.Y) / 16;
                            if (this.gravDir == -1f)
                            {
                                num153 = (int)(Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY) / 16;
                            }
                            if (!flag13)
                            {
                                while (num153 < Main.maxTilesY - 10 && Main.tile[num152, num153] != null && !WorldGen.SolidTile2(num152, num153) && Main.tile[num152 - 1, num153] != null && !WorldGen.SolidTile2(num152 - 1, num153) && Main.tile[num152 + 1, num153] != null && !WorldGen.SolidTile2(num152 + 1, num153))
                                {
                                    num153++;
                                }
                                num153--;
                            }
                            Projectile.NewProjectile((float)Main.mouseX + Main.screenPosition.X, (float)(num153 * 16 - 24), 0f, 15f, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 1244 || item.type == 1256)
                        {
                            int num154 = Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71, num73, num74, i, 0f, 0f);
                            Main.projectile[num154].ai[0] = (float)Main.mouseX + Main.screenPosition.X;
                            Main.projectile[num154].ai[1] = (float)Main.mouseY + Main.screenPosition.Y;
                        }
                        else if (item.type == 1229)
                        {
                            int num155 = Main.rand.Next(2, 4);
                            if (Main.rand.Next(5) == 0)
                            {
                                num155++;
                            }
                            for (int num156 = 0; num156 < num155; num156++)
                            {
                                float num157 = num78;
                                float num158 = num79;
                                if (num156 > 0)
                                {
                                    num157 += (float)Main.rand.Next(-35, 36) * 0.04f;
                                    num158 += (float)Main.rand.Next(-35, 36) * 0.04f;
                                }
                                if (num156 > 1)
                                {
                                    num157 += (float)Main.rand.Next(-35, 36) * 0.04f;
                                    num158 += (float)Main.rand.Next(-35, 36) * 0.04f;
                                }
                                if (num156 > 2)
                                {
                                    num157 += (float)Main.rand.Next(-35, 36) * 0.04f;
                                    num158 += (float)Main.rand.Next(-35, 36) * 0.04f;
                                }
                                int num159 = Projectile.NewProjectile(vector2.X, vector2.Y, num157, num158, num71, num73, num74, i, 0f, 0f);
                                Main.projectile[num159].noDropItem = true;
                            }
                        }
                        else if (item.type == 1121)
                        {
                            int num160 = Main.rand.Next(1, 4);
                            if (Main.rand.Next(6) == 0)
                            {
                                num160++;
                            }
                            if (Main.rand.Next(6) == 0)
                            {
                                num160++;
                            }
                            if (this.strongBees && Main.rand.Next(3) == 0)
                            {
                                num160++;
                            }
                            for (int num161 = 0; num161 < num160; num161++)
                            {
                                float num162 = num78;
                                float num163 = num79;
                                num162 += (float)Main.rand.Next(-35, 36) * 0.02f;
                                num163 += (float)Main.rand.Next(-35, 36) * 0.02f;
                                int num164 = Projectile.NewProjectile(vector2.X, vector2.Y, num162, num163, this.beeType(), this.beeDamage(num73), this.beeKB(num74), i, 0f, 0f);
                                Main.projectile[num164].magic = true;
                            }
                        }
                        else if (item.type == 1155)
                        {
                            int num165 = Main.rand.Next(2, 5);
                            if (Main.rand.Next(5) == 0)
                            {
                                num165++;
                            }
                            if (Main.rand.Next(5) == 0)
                            {
                                num165++;
                            }
                            for (int num166 = 0; num166 < num165; num166++)
                            {
                                float num167 = num78;
                                float num168 = num79;
                                num167 += (float)Main.rand.Next(-35, 36) * 0.02f;
                                num168 += (float)Main.rand.Next(-35, 36) * 0.02f;
                                Projectile.NewProjectile(vector2.X, vector2.Y, num167, num168, num71, num73, num74, i, 0f, 0f);
                            }
                        }
                        else if (item.type == 1801)
                        {
                            int num169 = Main.rand.Next(1, 4);
                            for (int num170 = 0; num170 < num169; num170++)
                            {
                                float num171 = num78;
                                float num172 = num79;
                                num171 += (float)Main.rand.Next(-35, 36) * 0.05f;
                                num172 += (float)Main.rand.Next(-35, 36) * 0.05f;
                                Projectile.NewProjectile(vector2.X, vector2.Y, num171, num172, num71, num73, num74, i, 0f, 0f);
                            }
                        }
                        else if (item.type == 679)
                        {
                            for (int num173 = 0; num173 < 6; num173++)
                            {
                                float num174 = num78;
                                float num175 = num79;
                                num174 += (float)Main.rand.Next(-40, 41) * 0.05f;
                                num175 += (float)Main.rand.Next(-40, 41) * 0.05f;
                                Projectile.NewProjectile(vector2.X, vector2.Y, num174, num175, num71, num73, num74, i, 0f, 0f);
                            }
                        }
                        else if (item.type == 2623)
                        {
                            for (int num176 = 0; num176 < 3; num176++)
                            {
                                float num177 = num78;
                                float num178 = num79;
                                num177 += (float)Main.rand.Next(-40, 41) * 0.1f;
                                num178 += (float)Main.rand.Next(-40, 41) * 0.1f;
                                Projectile.NewProjectile(vector2.X, vector2.Y, num177, num178, num71, num73, num74, i, 0f, 0f);
                            }
                        }
                        else if (item.type == 3210)
                        {
                            Vector2 value10 = new Vector2(num78, num79);
                            value10.X += (float)Main.rand.Next(-30, 31) * 0.04f;
                            value10.Y += (float)Main.rand.Next(-30, 31) * 0.03f;
                            value10.Normalize();
                            value10 *= (float)Main.rand.Next(70, 91) * 0.1f;
                            value10.X += (float)Main.rand.Next(-30, 31) * 0.04f;
                            value10.Y += (float)Main.rand.Next(-30, 31) * 0.03f;
                            Projectile.NewProjectile(vector2.X, vector2.Y, value10.X, value10.Y, num71, num73, num74, i, (float)Main.rand.Next(20), 0f);
                        }
                        else if (item.type == 434)
                        {
                            float num179 = num78;
                            float num180 = num79;
                            if (this.itemAnimation < 5)
                            {
                                num179 += (float)Main.rand.Next(-40, 41) * 0.01f;
                                num180 += (float)Main.rand.Next(-40, 41) * 0.01f;
                                num179 *= 1.1f;
                                num180 *= 1.1f;
                            }
                            else if (this.itemAnimation < 10)
                            {
                                num179 += (float)Main.rand.Next(-20, 21) * 0.01f;
                                num180 += (float)Main.rand.Next(-20, 21) * 0.01f;
                                num179 *= 1.05f;
                                num180 *= 1.05f;
                            }
                            Projectile.NewProjectile(vector2.X, vector2.Y, num179, num180, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 1157)
                        {
                            num71 = Main.rand.Next(191, 195);
                            num78 = 0f;
                            num79 = 0f;
                            vector2.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            int num181 = Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71, num73, num74, i, 0f, 0f);
                            Main.projectile[num181].localAI[0] = 30f;
                        }
                        else if (item.type == 1802)
                        {
                            num78 = 0f;
                            num79 = 0f;
                            vector2.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 2364 || item.type == 2365)
                        {
                            num78 = 0f;
                            num79 = 0f;
                            vector2.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 2535)
                        {
                            num78 = 0f;
                            num79 = 0f;
                            vector2.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            Vector2 spinningpoint = new Vector2(num78, num79);
                            spinningpoint = spinningpoint.RotatedBy(1.5707963705062866, default(Vector2));
                            Projectile.NewProjectile(vector2.X + spinningpoint.X, vector2.Y + spinningpoint.Y, spinningpoint.X, spinningpoint.Y, num71, num73, num74, i, 0f, 0f);
                            spinningpoint = spinningpoint.RotatedBy(-3.1415927410125732, default(Vector2));
                            Projectile.NewProjectile(vector2.X + spinningpoint.X, vector2.Y + spinningpoint.Y, spinningpoint.X, spinningpoint.Y, num71 + 1, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 2551)
                        {
                            num78 = 0f;
                            num79 = 0f;
                            vector2.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71 + Main.rand.Next(3), num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 2584)
                        {
                            num78 = 0f;
                            num79 = 0f;
                            vector2.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71 + Main.rand.Next(3), num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 2621)
                        {
                            num78 = 0f;
                            num79 = 0f;
                            vector2.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 2749 || item.type == 3249 || item.type == 3474)
                        {
                            num78 = 0f;
                            num79 = 0f;
                            vector2.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 3531)
                        {
                            int num182 = -1;
                            int num183 = -1;
                            for (int num184 = 0; num184 < 1000; num184++)
                            {
                                if (Main.projectile[num184].active && Main.projectile[num184].owner == Main.myPlayer)
                                {
                                    if (num182 == -1 && Main.projectile[num184].type == 625)
                                    {
                                        num182 = num184;
                                    }
                                    if (num183 == -1 && Main.projectile[num184].type == 628)
                                    {
                                        num183 = num184;
                                    }
                                    if (num182 != -1 && num183 != -1)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (num182 == -1 && num183 == -1)
                            {
                                num78 = 0f;
                                num79 = 0f;
                                vector2.X = (float)Main.mouseX + Main.screenPosition.X;
                                vector2.Y = (float)Main.mouseY + Main.screenPosition.Y;
                                int num185 = Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71, num73, num74, i, 0f, 0f);
                                num185 = Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71 + 1, num73, num74, i, (float)num185, 0f);
                                int num186 = num185;
                                num185 = Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71 + 2, num73, num74, i, (float)num185, 0f);
                                Main.projectile[num186].localAI[1] = (float)num185;
                                num186 = num185;
                                num185 = Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71 + 3, num73, num74, i, (float)num185, 0f);
                                Main.projectile[num186].localAI[1] = (float)num185;
                            }
                            else if (num182 != -1 && num183 != -1)
                            {
                                int num187 = Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71 + 1, num73, num74, i, (float)Projectile.GetByUUID(Main.myPlayer, Main.projectile[num183].ai[0]), 0f);
                                int num188 = num187;
                                num187 = Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71 + 2, num73, num74, i, (float)num187, 0f);
                                Main.projectile[num188].localAI[1] = (float)num187;
                                Main.projectile[num188].netUpdate = true;
                                Main.projectile[num188].ai[1] = 1f;
                                Main.projectile[num187].localAI[1] = (float)num183;
                                Main.projectile[num187].netUpdate = true;
                                Main.projectile[num187].ai[1] = 1f;
                                Main.projectile[num183].ai[0] = (float)Main.projectile[num187].projUUID;
                                Main.projectile[num183].netUpdate = true;
                                Main.projectile[num183].ai[1] = 1f;
                            }
                        }
                        else if (item.type == 1309)
                        {
                            num78 = 0f;
                            num79 = 0f;
                            vector2.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector2.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.shoot > 0 && (Main.projPet[item.shoot] || item.shoot == 72 || item.shoot == 18 || item.shoot == 500 || item.shoot == 650) && !item.summon)
                        {
                            for (int num189 = 0; num189 < 1000; num189++)
                            {
                                if (Main.projectile[num189].active && Main.projectile[num189].owner == this.whoAmI)
                                {
                                    if (item.shoot == 72)
                                    {
                                        if (Main.projectile[num189].type == 72 || Main.projectile[num189].type == 86 || Main.projectile[num189].type == 87)
                                        {
                                            Main.projectile[num189].Kill();
                                        }
                                    }
                                    else if (item.shoot == Main.projectile[num189].type)
                                    {
                                        Main.projectile[num189].Kill();
                                    }
                                }
                            }
                            Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 3006)
                        {
                            Vector2 vector9;
                            vector9.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector9.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            while (Collision.CanHitLine(this.position, this.width, this.height, vector2, 1, 1))
                            {
                                vector2.X += num78;
                                vector2.Y += num79;
                                if ((vector2 - vector9).Length() < 20f + Math.Abs(num78) + Math.Abs(num79))
                                {
                                    vector2 = vector9;
                                    break;
                                }
                            }
                            Projectile.NewProjectile(vector2.X, vector2.Y, 0f, 0f, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 3014)
                        {
                            Vector2 vector10;
                            vector10.X = (float)Main.mouseX + Main.screenPosition.X;
                            vector10.Y = (float)Main.mouseY + Main.screenPosition.Y;
                            while (Collision.CanHitLine(this.position, this.width, this.height, vector2, 1, 1))
                            {
                                vector2.X += num78;
                                vector2.Y += num79;
                                if ((vector2 - vector10).Length() < 20f + Math.Abs(num78) + Math.Abs(num79))
                                {
                                    vector2 = vector10;
                                    break;
                                }
                            }
                            bool flag14 = false;
                            int num190 = (int)vector2.Y / 16;
                            int num191 = (int)vector2.X / 16;
                            int num192 = num190;
                            while (num190 < Main.maxTilesY - 10 && num190 - num192 < 30 && !WorldGen.SolidTile(num191, num190) && !TileID.Sets.Platforms[(int)Main.tile[num191, num190].type])
                            {
                                num190++;
                            }
                            if (!WorldGen.SolidTile(num191, num190) && !TileID.Sets.Platforms[(int)Main.tile[num191, num190].type])
                            {
                                flag14 = true;
                            }
                            float num193 = (float)(num190 * 16);
                            num190 = num192;
                            while (num190 > 10 && num192 - num190 < 30 && !WorldGen.SolidTile(num191, num190))
                            {
                                num190--;
                            }
                            float num194 = (float)(num190 * 16 + 16);
                            float num195 = num193 - num194;
                            int num196 = 10;
                            if (num195 > (float)(16 * num196))
                            {
                                num195 = (float)(16 * num196);
                            }
                            num194 = num193 - num195;
                            vector2.X = (float)((int)(vector2.X / 16f) * 16);
                            if (!flag14)
                            {
                                Projectile.NewProjectile(vector2.X, vector2.Y, 0f, 0f, num71, num73, num74, i, num194, num195);
                            }
                        }
                        else if (item.type == 3384)
                        {
                            int num197 = (this.altFunctionUse == 2) ? 1 : 0;
                            Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71, num73, num74, i, 0f, (float)num197);
                        }
                        else if (item.type == 3473)
                        {
                            float ai3 = (Main.rand.NextFloat() - 0.5f) * 0.7853982f;
                            Vector2 vector11 = new Vector2(num78, num79);
                            Projectile.NewProjectile(vector2.X, vector2.Y, vector11.X, vector11.Y, num71, num73, num74, i, 0f, ai3);
                        }
                        else if (item.type == 3542)
                        {
                            float num198 = (Main.rand.NextFloat() - 0.5f) * 0.7853982f * 0.7f;
                            int num199 = 0;
                            while (num199 < 10 && !Collision.CanHit(vector2, 0, 0, vector2 + new Vector2(num78, num79).RotatedBy((double)num198, default(Vector2)) * 100f, 0, 0))
                            {
                                num198 = (Main.rand.NextFloat() - 0.5f) * 0.7853982f * 0.7f;
                                num199++;
                            }
                            Vector2 vector12 = new Vector2(num78, num79).RotatedBy((double)num198, default(Vector2)) * (0.95f + Main.rand.NextFloat() * 0.3f);
                            Projectile.NewProjectile(vector2.X, vector2.Y, vector12.X, vector12.Y, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 3779)
                        {
                            float num200 = Main.rand.NextFloat() * 6.28318548f;
                            int num201 = 0;
                            while (num201 < 10 && !Collision.CanHit(vector2, 0, 0, vector2 + new Vector2(num78, num79).RotatedBy((double)num200, default(Vector2)) * 100f, 0, 0))
                            {
                                num200 = Main.rand.NextFloat() * 6.28318548f;
                                num201++;
                            }
                            Vector2 value11 = new Vector2(num78, num79).RotatedBy((double)num200, default(Vector2)) * (0.95f + Main.rand.NextFloat() * 0.3f);
                            Projectile.NewProjectile(vector2 + value11 * 30f, Vector2.Zero, num71, num73, num74, i, -2f, 0f);
                        }
                        else if (item.type == 3787)
                        {
                            float f = Main.rand.NextFloat() * 6.28318548f;
                            float value12 = 20f;
                            float value13 = 60f;
                            Vector2 vector13 = vector2 + f.ToRotationVector2() * MathHelper.Lerp(value12, value13, Main.rand.NextFloat());
                            for (int num202 = 0; num202 < 50; num202++)
                            {
                                vector13 = vector2 + f.ToRotationVector2() * MathHelper.Lerp(value12, value13, Main.rand.NextFloat());
                                if (Collision.CanHit(vector2, 0, 0, vector13 + (vector13 - vector2).SafeNormalize(Vector2.UnitX) * 8f, 0, 0))
                                {
                                    break;
                                }
                                f = Main.rand.NextFloat() * 6.28318548f;
                            }
                            Vector2 mouseWorld = Main.MouseWorld;
                            Vector2 vector14 = mouseWorld - vector13;
                            Vector2 vector15 = new Vector2(num78, num79).SafeNormalize(Vector2.UnitY) * num72;
                            vector14 = vector14.SafeNormalize(vector15) * num72;
                            vector14 = Vector2.Lerp(vector14, vector15, 0.25f);
                            Projectile.NewProjectile(vector13, vector14, num71, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 3788)
                        {
                            Vector2 vector16 = new Vector2(num78, num79);
                            float num203 = 0.7853982f;
                            for (int num204 = 0; num204 < 2; num204++)
                            {
                                Projectile.NewProjectile(vector2, vector16 + vector16.SafeNormalize(Vector2.Zero).RotatedBy((double)(num203 * (Main.rand.NextFloat() * 0.5f + 0.5f)), default(Vector2)) * Main.rand.NextFloatDirection() * 2f, num71, num73, num74, i, 0f, 0f);
                                Projectile.NewProjectile(vector2, vector16 + vector16.SafeNormalize(Vector2.Zero).RotatedBy((double)(-(double)num203 * (Main.rand.NextFloat() * 0.5f + 0.5f)), default(Vector2)) * Main.rand.NextFloatDirection() * 2f, num71, num73, num74, i, 0f, 0f);
                            }
                            Projectile.NewProjectile(vector2, vector16.SafeNormalize(Vector2.UnitX * (float)this.direction) * (num72 * 1.3f), 661, num73 * 2, num74, i, 0f, 0f);
                        }
                        else if (item.type == 3475)
                        {
                            Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, 615, num73, num74, i, (float)(5 * Main.rand.Next(0, 20)), 0f);
                        }
                        else if (item.type == 3540)
                        {
                            Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, 630, num73, num74, i, 0f, 0f);
                        }
                        else if (item.type == 3546)
                        {
                            for (int num205 = 0; num205 < 2; num205++)
                            {
                                float num206 = num78;
                                float num207 = num79;
                                num206 += (float)Main.rand.Next(-40, 41) * 0.05f;
                                num207 += (float)Main.rand.Next(-40, 41) * 0.05f;
                                Vector2 vector17 = vector2 + Vector2.Normalize(new Vector2(num206, num207).RotatedBy((double)(-1.57079637f * (float)this.direction), default(Vector2))) * 6f;
                                Projectile.NewProjectile(vector17.X, vector17.Y, num206, num207, 167 + Main.rand.Next(4), num73, num74, i, 0f, 1f);
                            }
                        }
                        else if (item.type == 3350)
                        {
                            float num208 = num78;
                            float num209 = num79;
                            num208 += (float)Main.rand.Next(-1, 2) * 0.5f;
                            num209 += (float)Main.rand.Next(-1, 2) * 0.5f;
                            if (Collision.CanHitLine(base.Center, 0, 0, vector2 + new Vector2(num208, num209) * 2f, 0, 0))
                            {
                                vector2 += new Vector2(num208, num209);
                            }
                            Projectile.NewProjectile(vector2.X, vector2.Y - this.gravDir * 4f, num208, num209, num71, num73, num74, i, 0f, (float)Main.rand.Next(12) / 6f);
                        }
                        else if (PlayerHooks.Shoot(this, item, ref vector2, ref num78, ref num79, ref num71, ref num73, ref num74)
                            && ItemLoader.Shoot(item, this, ref vector2, ref num78, ref num79, ref num71, ref num73, ref num74))
                        {
                            int num210 = Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, num71, num73, num74, i, 0f, 0f);
                            if (item.type == 726)
                            {
                                Main.projectile[num210].magic = true;
                            }
                            if (item.type == 724 || item.type == 676)
                            {
                                Main.projectile[num210].melee = true;
                            }
                            if (num71 == 80)
                            {
                                Main.projectile[num210].ai[0] = (float)Player.tileTargetX;
                                Main.projectile[num210].ai[1] = (float)Player.tileTargetY;
                            }
                            if (num71 == 442)
                            {
                                Main.projectile[num210].ai[0] = (float)Player.tileTargetX;
                                Main.projectile[num210].ai[1] = (float)Player.tileTargetY;
                            }
                            if ((this.thrownCost50 || this.thrownCost33) && this.inventory[this.selectedItem].thrown)
                            {
                                Main.projectile[num210].noDropItem = true;
                            }
                            if (Main.projectile[num210].aiStyle == 99)
                            {
                                AchievementsHelper.HandleSpecialEvent(this, 7);
                            }
                        }
                    }
                    else if (item.useStyle == 5)
                    {
                        this.itemRotation = 0f;
                        NetMessage.SendData(41, -1, -1, "", this.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
                if (this.whoAmI == Main.myPlayer && (item.type == 509 || item.type == 510 || item.type == 849 || item.type == 850 || item.type == 851 || item.type == 3612 || item.type == 3620 || item.type == 3625) && this.position.X / 16f - (float)Player.tileRangeX - (float)item.tileBoost - (float)this.blockRange <= (float)Player.tileTargetX && (this.position.X + (float)this.width) / 16f + (float)Player.tileRangeX + (float)item.tileBoost - 1f + (float)this.blockRange >= (float)Player.tileTargetX && this.position.Y / 16f - (float)Player.tileRangeY - (float)item.tileBoost - (float)this.blockRange <= (float)Player.tileTargetY && (this.position.Y + (float)this.height) / 16f + (float)Player.tileRangeY + (float)item.tileBoost - 2f + (float)this.blockRange >= (float)Player.tileTargetY)
                {
                    if (!Main.GamepadDisableCursorItemIcon)
                    {
                        this.showItemIcon = true;
                        Main.ItemIconCacheUpdate(item.type);
                    }
                    if (this.itemAnimation > 0 && this.itemTime == 0 && this.controlUseItem)
                    {
                        int num211 = Player.tileTargetX;
                        int num212 = Player.tileTargetY;
                        if (item.type == 509)
                        {
                            int num213 = -1;
                            for (int num214 = 0; num214 < 58; num214++)
                            {
                                if (this.inventory[num214].stack > 0 && this.inventory[num214].type == 530)
                                {
                                    num213 = num214;
                                    break;
                                }
                            }
                            if (num213 >= 0 && WorldGen.PlaceWire(num211, num212))
                            {
                                this.inventory[num213].stack--;
                                if (this.inventory[num213].stack <= 0)
                                {
                                    this.inventory[num213].SetDefaults(0, false);
                                }
                                this.itemTime = item.useTime;
                                NetMessage.SendData(17, -1, -1, "", 5, (float)Player.tileTargetX, (float)Player.tileTargetY, 0f, 0, 0, 0);
                            }
                        }
                        else if (item.type == 850)
                        {
                            int num215 = -1;
                            for (int num216 = 0; num216 < 58; num216++)
                            {
                                if (this.inventory[num216].stack > 0 && this.inventory[num216].type == 530)
                                {
                                    num215 = num216;
                                    break;
                                }
                            }
                            if (num215 >= 0 && WorldGen.PlaceWire2(num211, num212))
                            {
                                this.inventory[num215].stack--;
                                if (this.inventory[num215].stack <= 0)
                                {
                                    this.inventory[num215].SetDefaults(0, false);
                                }
                                this.itemTime = item.useTime;
                                NetMessage.SendData(17, -1, -1, "", 10, (float)Player.tileTargetX, (float)Player.tileTargetY, 0f, 0, 0, 0);
                            }
                        }
                        if (item.type == 851)
                        {
                            int num217 = -1;
                            for (int num218 = 0; num218 < 58; num218++)
                            {
                                if (this.inventory[num218].stack > 0 && this.inventory[num218].type == 530)
                                {
                                    num217 = num218;
                                    break;
                                }
                            }
                            if (num217 >= 0 && WorldGen.PlaceWire3(num211, num212))
                            {
                                this.inventory[num217].stack--;
                                if (this.inventory[num217].stack <= 0)
                                {
                                    this.inventory[num217].SetDefaults(0, false);
                                }
                                this.itemTime = item.useTime;
                                NetMessage.SendData(17, -1, -1, "", 12, (float)Player.tileTargetX, (float)Player.tileTargetY, 0f, 0, 0, 0);
                            }
                        }
                        if (item.type == 3612)
                        {
                            int num219 = -1;
                            for (int num220 = 0; num220 < 58; num220++)
                            {
                                if (this.inventory[num220].stack > 0 && this.inventory[num220].type == 530)
                                {
                                    num219 = num220;
                                    break;
                                }
                            }
                            if (num219 >= 0 && WorldGen.PlaceWire4(num211, num212))
                            {
                                this.inventory[num219].stack--;
                                if (this.inventory[num219].stack <= 0)
                                {
                                    this.inventory[num219].SetDefaults(0, false);
                                }
                                this.itemTime = item.useTime;
                                NetMessage.SendData(17, -1, -1, "", 16, (float)Player.tileTargetX, (float)Player.tileTargetY, 0f, 0, 0, 0);
                            }
                        }
                        else if (item.type == 510)
                        {
                            if (WorldGen.KillActuator(num211, num212))
                            {
                                this.itemTime = item.useTime;
                                NetMessage.SendData(17, -1, -1, "", 9, (float)Player.tileTargetX, (float)Player.tileTargetY, 0f, 0, 0, 0);
                            }
                            else if (WorldGen.KillWire4(num211, num212))
                            {
                                this.itemTime = item.useTime;
                                NetMessage.SendData(17, -1, -1, "", 17, (float)Player.tileTargetX, (float)Player.tileTargetY, 0f, 0, 0, 0);
                            }
                            else if (WorldGen.KillWire3(num211, num212))
                            {
                                this.itemTime = item.useTime;
                                NetMessage.SendData(17, -1, -1, "", 13, (float)Player.tileTargetX, (float)Player.tileTargetY, 0f, 0, 0, 0);
                            }
                            else if (WorldGen.KillWire2(num211, num212))
                            {
                                this.itemTime = item.useTime;
                                NetMessage.SendData(17, -1, -1, "", 11, (float)Player.tileTargetX, (float)Player.tileTargetY, 0f, 0, 0, 0);
                            }
                            else if (WorldGen.KillWire(num211, num212))
                            {
                                this.itemTime = item.useTime;
                                NetMessage.SendData(17, -1, -1, "", 6, (float)Player.tileTargetX, (float)Player.tileTargetY, 0f, 0, 0, 0);
                            }
                        }
                        else if (item.type == 849 && item.stack > 0 && WorldGen.PlaceActuator(num211, num212))
                        {
                            this.itemTime = item.useTime;
                            NetMessage.SendData(17, -1, -1, "", 8, (float)Player.tileTargetX, (float)Player.tileTargetY, 0f, 0, 0, 0);
                            item.stack--;
                            if (item.stack <= 0)
                            {
                                item.SetDefaults(0, false);
                            }
                        }
                        if (item.type == 3620)
                        {
                            Tile tile2 = Main.tile[num211, num212];
                            if (tile2 != null && tile2.actuator())
                            {
                                bool flag15 = tile2.inActive();
                                if ((!this.ActuationRodLock || this.ActuationRodLockSetting == tile2.inActive()) && Wiring.Actuate(num211, num212) && flag15 != tile2.inActive())
                                {
                                    this.ActuationRodLock = true;
                                    this.ActuationRodLockSetting = !tile2.inActive();
                                    this.itemTime = item.useTime;
                                    NetMessage.SendData(17, -1, -1, "", 19, (float)Player.tileTargetX, (float)Player.tileTargetY, 0f, 0, 0, 0);
                                }
                            }
                        }
                        if (item.type == 3625)
                        {
                            Point point = new Point(Player.tileTargetX, Player.tileTargetY);
                            this.itemTime = item.useTime;
                            WiresUI.Settings.MultiToolMode toolMode = WiresUI.Settings.ToolMode;
                            WiresUI.Settings.ToolMode &= ~WiresUI.Settings.MultiToolMode.Actuator;
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendData(109, -1, -1, "", point.X, (float)point.Y, (float)point.X, (float)point.Y, (int)WiresUI.Settings.ToolMode, 0, 0);
                            }
                            else
                            {
                                Wiring.MassWireOperation(point, point, this);
                            }
                            WiresUI.Settings.ToolMode = toolMode;
                        }
                    }
                }
                if (this.itemAnimation > 0 && this.itemTime == 0 && (item.type == 507 || item.type == 508))
                {
                    this.itemTime = item.useTime;
                    Vector2 vector18 = new Vector2(this.position.X + (float)this.width * 0.5f, this.position.Y + (float)this.height * 0.5f);
                    float num221 = (float)Main.mouseX + Main.screenPosition.X - vector18.X;
                    float num222 = (float)Main.mouseY + Main.screenPosition.Y - vector18.Y;
                    float num223 = (float)Math.Sqrt((double)(num221 * num221 + num222 * num222));
                    num223 /= (float)(Main.screenHeight / 2);
                    if (num223 > 1f)
                    {
                        num223 = 1f;
                    }
                    num223 = num223 * 2f - 1f;
                    if (num223 < -1f)
                    {
                        num223 = -1f;
                    }
                    if (num223 > 1f)
                    {
                        num223 = 1f;
                    }
                    Main.harpNote = num223;
                    int style = 26;
                    if (item.type == 507)
                    {
                        style = 35;
                    }
                    Main.PlaySound(2, (int)this.position.X, (int)this.position.Y, style);
                    NetMessage.SendData(58, -1, -1, "", this.whoAmI, num223, 0f, 0f, 0, 0, 0);
                }
                if (((item.type >= 205 && item.type <= 207) || item.type == 1128 || item.type == 3031 || item.type == 3032) && this.position.X / 16f - (float)Player.tileRangeX - (float)item.tileBoost <= (float)Player.tileTargetX && (this.position.X + (float)this.width) / 16f + (float)Player.tileRangeX + (float)item.tileBoost - 1f >= (float)Player.tileTargetX && this.position.Y / 16f - (float)Player.tileRangeY - (float)item.tileBoost <= (float)Player.tileTargetY && (this.position.Y + (float)this.height) / 16f + (float)Player.tileRangeY + (float)item.tileBoost - 2f >= (float)Player.tileTargetY)
                {
                    if (!Main.GamepadDisableCursorItemIcon)
                    {
                        this.showItemIcon = true;
                        Main.ItemIconCacheUpdate(item.type);
                    }
                    if (this.itemTime == 0 && this.itemAnimation > 0 && this.controlUseItem)
                    {
                        if (item.type == 205 || (item.type == 3032 && Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType() == 0))
                        {
                            int num224 = (int)Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType();
                            int num225 = 0;
                            for (int num226 = Player.tileTargetX - 1; num226 <= Player.tileTargetX + 1; num226++)
                            {
                                for (int num227 = Player.tileTargetY - 1; num227 <= Player.tileTargetY + 1; num227++)
                                {
                                    if ((int)Main.tile[num226, num227].liquidType() == num224)
                                    {
                                        num225 += (int)Main.tile[num226, num227].liquid;
                                    }
                                }
                            }
                            if (Main.tile[Player.tileTargetX, Player.tileTargetY].liquid > 0 && (num225 > 100 || item.type == 3032))
                            {
                                int liquidType = (int)Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType();
                                if (item.type != 3032)
                                {
                                    if (!Main.tile[Player.tileTargetX, Player.tileTargetY].lava())
                                    {
                                        if (Main.tile[Player.tileTargetX, Player.tileTargetY].honey())
                                        {
                                            item.stack--;
                                            this.PutItemInInventory(1128, this.selectedItem);
                                        }
                                        else
                                        {
                                            item.stack--;
                                            this.PutItemInInventory(206, this.selectedItem);
                                        }
                                    }
                                    else
                                    {
                                        item.stack--;
                                        this.PutItemInInventory(207, this.selectedItem);
                                    }
                                }
                                Main.PlaySound(19, (int)this.position.X, (int)this.position.Y, 1);
                                this.itemTime = item.useTime;
                                int num228 = (int)Main.tile[Player.tileTargetX, Player.tileTargetY].liquid;
                                Main.tile[Player.tileTargetX, Player.tileTargetY].liquid = 0;
                                Main.tile[Player.tileTargetX, Player.tileTargetY].lava(false);
                                Main.tile[Player.tileTargetX, Player.tileTargetY].honey(false);
                                WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, false);
                                if (Main.netMode == 1)
                                {
                                    NetMessage.sendWater(Player.tileTargetX, Player.tileTargetY);
                                }
                                else
                                {
                                    Liquid.AddWater(Player.tileTargetX, Player.tileTargetY);
                                }
                                for (int num229 = Player.tileTargetX - 1; num229 <= Player.tileTargetX + 1; num229++)
                                {
                                    for (int num230 = Player.tileTargetY - 1; num230 <= Player.tileTargetY + 1; num230++)
                                    {
                                        if (num228 < 256 && (int)Main.tile[num229, num230].liquidType() == num224)
                                        {
                                            int num231 = (int)Main.tile[num229, num230].liquid;
                                            if (num231 + num228 > 255)
                                            {
                                                num231 = 255 - num228;
                                            }
                                            num228 += num231;
                                            Tile expr_A0F9 = Main.tile[num229, num230];
                                            expr_A0F9.liquid -= (byte)num231;
                                            Main.tile[num229, num230].liquidType(liquidType);
                                            if (Main.tile[num229, num230].liquid == 0)
                                            {
                                                Main.tile[num229, num230].lava(false);
                                                Main.tile[num229, num230].honey(false);
                                            }
                                            WorldGen.SquareTileFrame(num229, num230, false);
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.sendWater(num229, num230);
                                            }
                                            else
                                            {
                                                Liquid.AddWater(num229, num230);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (Main.tile[Player.tileTargetX, Player.tileTargetY].liquid < 200 && (!Main.tile[Player.tileTargetX, Player.tileTargetY].nactive() || !Main.tileSolid[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type] || Main.tileSolidTop[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type]))
                        {
                            if (item.type == 207)
                            {
                                if (Main.tile[Player.tileTargetX, Player.tileTargetY].liquid == 0 || Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType() == 1)
                                {
                                    Main.PlaySound(19, (int)this.position.X, (int)this.position.Y, 1);
                                    Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType(1);
                                    Main.tile[Player.tileTargetX, Player.tileTargetY].liquid = 255;
                                    WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
                                    item.stack--;
                                    this.PutItemInInventory(205, this.selectedItem);
                                    this.itemTime = item.useTime;
                                    if (Main.netMode == 1)
                                    {
                                        NetMessage.sendWater(Player.tileTargetX, Player.tileTargetY);
                                    }
                                }
                            }
                            else if (item.type == 206 || item.type == 3031)
                            {
                                if (Main.tile[Player.tileTargetX, Player.tileTargetY].liquid == 0 || Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType() == 0)
                                {
                                    Main.PlaySound(19, (int)this.position.X, (int)this.position.Y, 1);
                                    Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType(0);
                                    Main.tile[Player.tileTargetX, Player.tileTargetY].liquid = 255;
                                    WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
                                    if (item.type != 3031)
                                    {
                                        item.stack--;
                                        this.PutItemInInventory(205, this.selectedItem);
                                    }
                                    this.itemTime = item.useTime;
                                    if (Main.netMode == 1)
                                    {
                                        NetMessage.sendWater(Player.tileTargetX, Player.tileTargetY);
                                    }
                                }
                            }
                            else if (item.type == 1128 && (Main.tile[Player.tileTargetX, Player.tileTargetY].liquid == 0 || Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType() == 2))
                            {
                                Main.PlaySound(19, (int)this.position.X, (int)this.position.Y, 1);
                                Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType(2);
                                Main.tile[Player.tileTargetX, Player.tileTargetY].liquid = 255;
                                WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
                                item.stack--;
                                this.PutItemInInventory(205, this.selectedItem);
                                this.itemTime = item.useTime;
                                if (Main.netMode == 1)
                                {
                                    NetMessage.sendWater(Player.tileTargetX, Player.tileTargetY);
                                }
                            }
                        }
                    }
                }
                if (!this.channel)
                {
                    this.toolTime = this.itemTime;
                }
                else
                {
                    this.toolTime--;
                    if (this.toolTime < 0)
                    {
                        if (item.pick > 0)
                        {
                            this.toolTime = item.useTime;
                        }
                        else
                        {
                            this.toolTime = (int)((float)item.useTime * this.pickSpeed);
                        }
                    }
                }
                if (item.pick > 0 || item.axe > 0 || item.hammer > 0)
                {
                    bool flag16 = this.position.X / 16f - (float)Player.tileRangeX - (float)item.tileBoost <= (float)Player.tileTargetX && (this.position.X + (float)this.width) / 16f + (float)Player.tileRangeX + (float)item.tileBoost - 1f >= (float)Player.tileTargetX && this.position.Y / 16f - (float)Player.tileRangeY - (float)item.tileBoost <= (float)Player.tileTargetY && (this.position.Y + (float)this.height) / 16f + (float)Player.tileRangeY + (float)item.tileBoost - 2f >= (float)Player.tileTargetY;
                    if (flag16)
                    {
                        int num232 = 0;
                        bool flag17 = true;
                        if (!Main.GamepadDisableCursorItemIcon)
                        {
                            this.showItemIcon = true;
                            Main.ItemIconCacheUpdate(item.type);
                        }
                        if (this.toolTime == 0 && this.itemAnimation > 0 && this.controlUseItem && (!Main.tile[Player.tileTargetX, Player.tileTargetY].active() || (!Main.tileHammer[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type] && !Main.tileSolid[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type] && Main.tile[Player.tileTargetX, Player.tileTargetY].type != 314 && Main.tile[Player.tileTargetX, Player.tileTargetY].type != 424 && Main.tile[Player.tileTargetX, Player.tileTargetY].type != 442 && Main.tile[Player.tileTargetX, Player.tileTargetY].type != 351)))
                        {
                            this.poundRelease = false;
                        }
                        if (Main.tile[Player.tileTargetX, Player.tileTargetY].active())
                        {
                            if ((item.pick > 0 && !Main.tileAxe[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type] && !Main.tileHammer[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type]) || (item.axe > 0 && Main.tileAxe[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type]) || (item.hammer > 0 && Main.tileHammer[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type]))
                            {
                                flag17 = false;
                            }
                            if (this.toolTime == 0 && this.itemAnimation > 0 && this.controlUseItem)
                            {
                                int tileId = this.hitTile.HitObject(Player.tileTargetX, Player.tileTargetY, 1);
                                if (Main.tileNoFail[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type])
                                {
                                    num232 = 100;
                                }
                                if (Main.tileHammer[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type])
                                {
                                    flag17 = false;
                                    if (item.hammer > 0)
                                    {
                                        TileLoader.MineDamage(item.hammer, ref num232);
                                        num232 += item.hammer;
                                        if (!WorldGen.CanKillTile(Player.tileTargetX, Player.tileTargetY))
                                        {
                                            num232 = 0;
                                        }
                                        if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 26 && (item.hammer < 80 || !Main.hardMode))
                                        {
                                            num232 = 0;
                                            this.Hurt(this.statLife / 2, -this.direction, false, false, Lang.deathMsg(-1, -1, -1, 4, 0, 0), false, -1);
                                        }
                                        AchievementsHelper.CurrentlyMining = true;
                                        if (this.hitTile.AddDamage(tileId, num232, true) >= 100)
                                        {
                                            this.hitTile.Clear(tileId);
                                            WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY, false, false, false);
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendData(17, -1, -1, "", 0, (float)Player.tileTargetX, (float)Player.tileTargetY, 0f, 0, 0, 0);
                                            }
                                        }
                                        else
                                        {
                                            WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY, true, false, false);
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendData(17, -1, -1, "", 0, (float)Player.tileTargetX, (float)Player.tileTargetY, 1f, 0, 0, 0);
                                            }
                                        }
                                        if (num232 != 0)
                                        {
                                            this.hitTile.Prune();
                                        }
                                        this.itemTime = item.useTime;
                                        AchievementsHelper.CurrentlyMining = false;
                                    }
                                }
                                else if (Main.tileAxe[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type])
                                {
                                    if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 80)
                                    {
                                        num232 += item.axe * 3;
                                    }
                                    else
                                    {
                                        TileLoader.MineDamage(item.axe, ref num232);
                                    }
                                    if (item.axe > 0)
                                    {
                                        AchievementsHelper.CurrentlyMining = true;
                                        if (!WorldGen.CanKillTile(Player.tileTargetX, Player.tileTargetY))
                                        {
                                            num232 = 0;
                                        }
                                        if (this.hitTile.AddDamage(tileId, num232, true) >= 100)
                                        {
                                            this.hitTile.Clear(tileId);
                                            WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY, false, false, false);
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendData(17, -1, -1, "", 0, (float)Player.tileTargetX, (float)Player.tileTargetY, 0f, 0, 0, 0);
                                            }
                                        }
                                        else
                                        {
                                            WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY, true, false, false);
                                            if (Main.netMode == 1)
                                            {
                                                NetMessage.SendData(17, -1, -1, "", 0, (float)Player.tileTargetX, (float)Player.tileTargetY, 1f, 0, 0, 0);
                                            }
                                        }
                                        if (num232 != 0)
                                        {
                                            this.hitTile.Prune();
                                        }
                                        this.itemTime = item.useTime;
                                        AchievementsHelper.CurrentlyMining = false;
                                    }
                                }
                                else if (item.pick > 0)
                                {
                                    this.PickTile(Player.tileTargetX, Player.tileTargetY, item.pick);
                                    this.itemTime = (int)((float)item.useTime * this.pickSpeed);
                                }
                                if (item.pick > 0)
                                {
                                    this.itemTime = (int)((float)item.useTime * this.pickSpeed);
                                }
                                if (item.hammer > 0 && Main.tile[Player.tileTargetX, Player.tileTargetY].active() && ((Main.tileSolid[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type] && Main.tile[Player.tileTargetX, Player.tileTargetY].type != 10) || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 314 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 351 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 424 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 442) && this.poundRelease)
                                {
                                    flag17 = false;
                                    this.itemTime = item.useTime;
                                    num232 += (int)((double)item.hammer * 1.25);
                                    num232 = 100;
                                    if (Main.tile[Player.tileTargetX, Player.tileTargetY - 1].active() && Main.tile[Player.tileTargetX, Player.tileTargetY - 1].type == 10)
                                    {
                                        num232 = 0;
                                    }
                                    if (Main.tile[Player.tileTargetX, Player.tileTargetY + 1].active() && Main.tile[Player.tileTargetX, Player.tileTargetY + 1].type == 10)
                                    {
                                        num232 = 0;
                                    }
                                    if (this.hitTile.AddDamage(tileId, num232, true) >= 100)
                                    {
                                        this.hitTile.Clear(tileId);
                                        if (this.poundRelease)
                                        {
                                            int num233 = Player.tileTargetX;
                                            int num234 = Player.tileTargetY;
                                            if (TileLoader.Slope(num233, num234, Main.tile[num233, num234].type))
                                            {
                                            }
                                            else if (TileID.Sets.Platforms[(int)Main.tile[num233, num234].type])
                                            {
                                                if (Main.tile[num233, num234].halfBrick())
                                                {
                                                    WorldGen.PoundTile(num233, num234);
                                                    if (Main.netMode == 1)
                                                    {
                                                        NetMessage.SendData(17, -1, -1, "", 7, (float)Player.tileTargetX, (float)Player.tileTargetY, 1f, 0, 0, 0);
                                                    }
                                                }
                                                else
                                                {
                                                    int num235 = 1;
                                                    int slope = 2;
                                                    if (TileID.Sets.Platforms[(int)Main.tile[num233 + 1, num234 - 1].type] || TileID.Sets.Platforms[(int)Main.tile[num233 - 1, num234 + 1].type] || (WorldGen.SolidTile(num233 + 1, num234) && !WorldGen.SolidTile(num233 - 1, num234)))
                                                    {
                                                        num235 = 2;
                                                        slope = 1;
                                                    }
                                                    if (Main.tile[num233, num234].slope() == 0)
                                                    {
                                                        WorldGen.SlopeTile(num233, num234, num235);
                                                        int num236 = (int)Main.tile[num233, num234].slope();
                                                        if (Main.netMode == 1)
                                                        {
                                                            NetMessage.SendData(17, -1, -1, "", 14, (float)Player.tileTargetX, (float)Player.tileTargetY, (float)num236, 0, 0, 0);
                                                        }
                                                    }
                                                    else if ((int)Main.tile[num233, num234].slope() == num235)
                                                    {
                                                        WorldGen.SlopeTile(num233, num234, slope);
                                                        int num237 = (int)Main.tile[num233, num234].slope();
                                                        if (Main.netMode == 1)
                                                        {
                                                            NetMessage.SendData(17, -1, -1, "", 14, (float)Player.tileTargetX, (float)Player.tileTargetY, (float)num237, 0, 0, 0);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        WorldGen.SlopeTile(num233, num234, 0);
                                                        int num238 = (int)Main.tile[num233, num234].slope();
                                                        if (Main.netMode == 1)
                                                        {
                                                            NetMessage.SendData(17, -1, -1, "", 14, (float)Player.tileTargetX, (float)Player.tileTargetY, (float)num238, 0, 0, 0);
                                                        }
                                                        WorldGen.PoundTile(num233, num234);
                                                        if (Main.netMode == 1)
                                                        {
                                                            NetMessage.SendData(17, -1, -1, "", 7, (float)Player.tileTargetX, (float)Player.tileTargetY, 1f, 0, 0, 0);
                                                        }
                                                    }
                                                }
                                            }
                                            else if (Main.tile[num233, num234].type == 314)
                                            {
                                                if (Minecart.FrameTrack(num233, num234, true, false) && Main.netMode == 1)
                                                {
                                                    NetMessage.SendData(17, -1, -1, "", 15, (float)Player.tileTargetX, (float)Player.tileTargetY, 1f, 0, 0, 0);
                                                }
                                            }
                                            else if (Main.tile[num233, num234].type == 137)
                                            {
                                                int num239 = 0;
                                                switch (Main.tile[num233, num234].frameY / 18)
                                                {
                                                    case 0:
                                                    case 1:
                                                    case 2:
                                                        switch (Main.tile[num233, num234].frameX / 18)
                                                        {
                                                            case 0:
                                                                num239 = 2;
                                                                break;
                                                            case 1:
                                                                num239 = 3;
                                                                break;
                                                            case 2:
                                                                num239 = 4;
                                                                break;
                                                            case 3:
                                                                num239 = 5;
                                                                break;
                                                            case 4:
                                                                num239 = 1;
                                                                break;
                                                            case 5:
                                                                num239 = 0;
                                                                break;
                                                        }
                                                        break;
                                                    case 3:
                                                    case 4:
                                                        switch (Main.tile[num233, num234].frameX / 18)
                                                        {
                                                            case 0:
                                                            case 1:
                                                                num239 = 3;
                                                                break;
                                                            case 2:
                                                                num239 = 4;
                                                                break;
                                                            case 3:
                                                                num239 = 2;
                                                                break;
                                                            case 4:
                                                                num239 = 0;
                                                                break;
                                                        }
                                                        break;
                                                }
                                                Main.tile[num233, num234].frameX = (short)(num239 * 18);
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);
                                                }
                                            }
                                            else if (Main.tile[num233, num234].type == 424)
                                            {
                                                if (Main.tile[num233, num234].frameX == 0)
                                                {
                                                    Main.tile[num233, num234].frameX = 18;
                                                }
                                                else if (Main.tile[num233, num234].frameX == 18)
                                                {
                                                    Main.tile[num233, num234].frameX = 36;
                                                }
                                                else
                                                {
                                                    Main.tile[num233, num234].frameX = 0;
                                                }
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);
                                                }
                                            }
                                            else if (Main.tile[num233, num234].type == 442)
                                            {
                                                Tile tile3 = Main.tile[num233, num234 - 1];
                                                Tile tile4 = Main.tile[num233, num234 + 1];
                                                Tile tile5 = Main.tile[num233 - 1, num234];
                                                Tile tile6 = Main.tile[num233 + 1, num234];
                                                Tile tile7 = Main.tile[num233 - 1, num234 + 1];
                                                Tile tile8 = Main.tile[num233 + 1, num234 + 1];
                                                Tile tile9 = Main.tile[num233 - 1, num234 - 1];
                                                Tile tile10 = Main.tile[num233 + 1, num234 - 1];
                                                int num240 = -1;
                                                int num241 = -1;
                                                int num242 = -1;
                                                int num243 = -1;
                                                int num244 = -1;
                                                int num245 = -1;
                                                int num246 = -1;
                                                int num247 = -1;
                                                if (tile3 != null && tile3.nactive() && !tile3.bottomSlope())
                                                {
                                                    num241 = (int)tile3.type;
                                                }
                                                if (tile4 != null && tile4.nactive() && !tile4.halfBrick() && !tile4.topSlope())
                                                {
                                                    num240 = (int)tile4.type;
                                                }
                                                if (tile5 != null && tile5.nactive() && (tile5.slope() == 0 || tile5.slope() % 2 != 1))
                                                {
                                                    num242 = (int)tile5.type;
                                                }
                                                if (tile6 != null && tile6.nactive() && (tile6.slope() == 0 || tile6.slope() % 2 != 0))
                                                {
                                                    num243 = (int)tile6.type;
                                                }
                                                if (tile7 != null && tile7.nactive())
                                                {
                                                    num244 = (int)tile7.type;
                                                }
                                                if (tile8 != null && tile8.nactive())
                                                {
                                                    num245 = (int)tile8.type;
                                                }
                                                if (tile9 != null && tile9.nactive())
                                                {
                                                    num246 = (int)tile9.type;
                                                }
                                                if (tile10 != null && tile10.nactive())
                                                {
                                                    num247 = (int)tile10.type;
                                                }
                                                bool flag18 = false;
                                                bool flag19 = false;
                                                bool flag20 = false;
                                                bool flag21 = false;
                                                if (num240 >= 0 && Main.tileSolid[num240] && (!Main.tileNoAttach[num240] || TileID.Sets.Platforms[num240]) && (tile4.bottomSlope() || tile4.slope() == 0) && !tile4.halfBrick())
                                                {
                                                    flag21 = true;
                                                }
                                                if (num241 >= 0 && Main.tileSolid[num241] && (!Main.tileNoAttach[num241] || (TileID.Sets.Platforms[num241] && tile3.halfBrick())) && (tile3.topSlope() || tile3.slope() == 0 || tile3.halfBrick()))
                                                {
                                                    flag18 = true;
                                                }
                                                if ((num242 >= 0 && Main.tileSolid[num242] && !Main.tileNoAttach[num242] && (tile5.leftSlope() || tile5.slope() == 0) && !tile5.halfBrick()) || num242 == 124 || (num242 == 5 && num246 == 5 && num244 == 5))
                                                {
                                                    flag19 = true;
                                                }
                                                if ((num243 >= 0 && Main.tileSolid[num243] && !Main.tileNoAttach[num243] && (tile6.rightSlope() || tile6.slope() == 0) && !tile6.halfBrick()) || num243 == 124 || (num243 == 5 && num247 == 5 && num245 == 5))
                                                {
                                                    flag20 = true;
                                                }
                                                int num248 = (int)(Main.tile[num233, num234].frameX / 22);
                                                short num249 = -2;
                                                switch (num248)
                                                {
                                                    case 0:
                                                        if (flag19)
                                                        {
                                                            num249 = 2;
                                                        }
                                                        else if (flag18)
                                                        {
                                                            num249 = 1;
                                                        }
                                                        else if (flag20)
                                                        {
                                                            num249 = 3;
                                                        }
                                                        else
                                                        {
                                                            num249 = -1;
                                                        }
                                                        break;
                                                    case 1:
                                                        if (flag20)
                                                        {
                                                            num249 = 3;
                                                        }
                                                        else if (flag21)
                                                        {
                                                            num249 = 0;
                                                        }
                                                        else if (flag19)
                                                        {
                                                            num249 = 2;
                                                        }
                                                        else
                                                        {
                                                            num249 = -1;
                                                        }
                                                        break;
                                                    case 2:
                                                        if (flag18)
                                                        {
                                                            num249 = 1;
                                                        }
                                                        else if (flag20)
                                                        {
                                                            num249 = 3;
                                                        }
                                                        else if (flag21)
                                                        {
                                                            num249 = 0;
                                                        }
                                                        else
                                                        {
                                                            num249 = -1;
                                                        }
                                                        break;
                                                    case 3:
                                                        if (flag21)
                                                        {
                                                            num249 = 0;
                                                        }
                                                        else if (flag19)
                                                        {
                                                            num249 = 2;
                                                        }
                                                        else if (flag18)
                                                        {
                                                            num249 = 1;
                                                        }
                                                        else
                                                        {
                                                            num249 = -1;
                                                        }
                                                        break;
                                                }
                                                if (num249 != -1)
                                                {
                                                    if (num249 == -2)
                                                    {
                                                        num249 = 0;
                                                    }
                                                    Main.tile[num233, num234].frameX = (short)(22 * num249);
                                                    if (Main.netMode == 1)
                                                    {
                                                        NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1);
                                                    }
                                                }
                                            }
                                            else if ((Main.tile[num233, num234].halfBrick() || Main.tile[num233, num234].slope() != 0) && !Main.tileSolidTop[(int)Main.tile[Player.tileTargetX, Player.tileTargetY].type])
                                            {
                                                int num250 = 1;
                                                int num251 = 1;
                                                int num252 = 2;
                                                if ((WorldGen.SolidTile(num233 + 1, num234) || Main.tile[num233 + 1, num234].slope() == 1 || Main.tile[num233 + 1, num234].slope() == 3) && !WorldGen.SolidTile(num233 - 1, num234))
                                                {
                                                    num251 = 2;
                                                    num252 = 1;
                                                }
                                                if (WorldGen.SolidTile(num233, num234 - 1) && !WorldGen.SolidTile(num233, num234 + 1))
                                                {
                                                    num250 = -1;
                                                }
                                                if (num250 == 1)
                                                {
                                                    if (Main.tile[num233, num234].slope() == 0)
                                                    {
                                                        WorldGen.SlopeTile(num233, num234, num251);
                                                    }
                                                    else if ((int)Main.tile[num233, num234].slope() == num251)
                                                    {
                                                        WorldGen.SlopeTile(num233, num234, num252);
                                                    }
                                                    else if ((int)Main.tile[num233, num234].slope() == num252)
                                                    {
                                                        WorldGen.SlopeTile(num233, num234, num251 + 2);
                                                    }
                                                    else if ((int)Main.tile[num233, num234].slope() == num251 + 2)
                                                    {
                                                        WorldGen.SlopeTile(num233, num234, num252 + 2);
                                                    }
                                                    else
                                                    {
                                                        WorldGen.SlopeTile(num233, num234, 0);
                                                    }
                                                }
                                                else if (Main.tile[num233, num234].slope() == 0)
                                                {
                                                    WorldGen.SlopeTile(num233, num234, num251 + 2);
                                                }
                                                else if ((int)Main.tile[num233, num234].slope() == num251 + 2)
                                                {
                                                    WorldGen.SlopeTile(num233, num234, num252 + 2);
                                                }
                                                else if ((int)Main.tile[num233, num234].slope() == num252 + 2)
                                                {
                                                    WorldGen.SlopeTile(num233, num234, num251);
                                                }
                                                else if ((int)Main.tile[num233, num234].slope() == num251)
                                                {
                                                    WorldGen.SlopeTile(num233, num234, num252);
                                                }
                                                else
                                                {
                                                    WorldGen.SlopeTile(num233, num234, 0);
                                                }
                                                int num253 = (int)Main.tile[num233, num234].slope();
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendData(17, -1, -1, "", 14, (float)Player.tileTargetX, (float)Player.tileTargetY, (float)num253, 0, 0, 0);
                                                }
                                            }
                                            else
                                            {
                                                WorldGen.PoundTile(num233, num234);
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendData(17, -1, -1, "", 7, (float)Player.tileTargetX, (float)Player.tileTargetY, 1f, 0, 0, 0);
                                                }
                                            }
                                            this.poundRelease = false;
                                        }
                                    }
                                    else
                                    {
                                        WorldGen.KillTile(Player.tileTargetX, Player.tileTargetY, true, true, false);
                                        Main.PlaySound(0, Player.tileTargetX * 16, Player.tileTargetY * 16, 1);
                                    }
                                }
                                else
                                {
                                    this.poundRelease = false;
                                }
                            }
                        }
                        if (this.releaseUseItem)
                        {
                            this.poundRelease = true;
                        }
                        int num254 = Player.tileTargetX;
                        int num255 = Player.tileTargetY;
                        bool flag22 = true;
                        if (Main.tile[num254, num255].wall > 0)
                        {
                            if (!Main.wallHouse[(int)Main.tile[num254, num255].wall])
                            {
                                for (int num256 = num254 - 1; num256 < num254 + 2; num256++)
                                {
                                    for (int num257 = num255 - 1; num257 < num255 + 2; num257++)
                                    {
                                        if (Main.tile[num256, num257].wall != Main.tile[num254, num255].wall)
                                        {
                                            flag22 = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                flag22 = false;
                            }
                        }
                        if (flag22 && !Main.tile[num254, num255].active())
                        {
                            int num258 = -1;
                            if ((double)(((float)Main.mouseX + Main.screenPosition.X) / 16f) < Math.Round((double)(((float)Main.mouseX + Main.screenPosition.X) / 16f)))
                            {
                                num258 = 0;
                            }
                            int num259 = -1;
                            if ((double)(((float)Main.mouseY + Main.screenPosition.Y) / 16f) < Math.Round((double)(((float)Main.mouseY + Main.screenPosition.Y) / 16f)))
                            {
                                num259 = 0;
                            }
                            for (int num260 = Player.tileTargetX + num258; num260 <= Player.tileTargetX + num258 + 1; num260++)
                            {
                                for (int num261 = Player.tileTargetY + num259; num261 <= Player.tileTargetY + num259 + 1; num261++)
                                {
                                    if (flag22)
                                    {
                                        num254 = num260;
                                        num255 = num261;
                                        if (Main.tile[num254, num255].wall > 0)
                                        {
                                            if (!Main.wallHouse[(int)Main.tile[num254, num255].wall])
                                            {
                                                for (int num262 = num254 - 1; num262 < num254 + 2; num262++)
                                                {
                                                    for (int num263 = num255 - 1; num263 < num255 + 2; num263++)
                                                    {
                                                        if (Main.tile[num262, num263].wall != Main.tile[num254, num255].wall)
                                                        {
                                                            flag22 = false;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                flag22 = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (flag17 && Main.tile[num254, num255].wall > 0 && (!Main.tile[num254, num255].active() || num254 != Player.tileTargetX || num255 != Player.tileTargetY || (!Main.tileHammer[(int)Main.tile[num254, num255].type] && !this.poundRelease)) && this.toolTime == 0 && this.itemAnimation > 0 && this.controlUseItem && item.hammer > 0)
                        {
                            bool flag23 = true;
                            if (!Main.wallHouse[(int)Main.tile[num254, num255].wall])
                            {
                                flag23 = false;
                                for (int num264 = num254 - 1; num264 < num254 + 2; num264++)
                                {
                                    for (int num265 = num255 - 1; num265 < num255 + 2; num265++)
                                    {
                                        if (Main.tile[num264, num265].wall == 0 || Main.wallHouse[(int)Main.tile[num264, num265].wall])
                                        {
                                            flag23 = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (flag23)
                            {
                                int tileId = this.hitTile.HitObject(num254, num255, 2);
                                num232 += (int)((float)item.hammer * 1.5f);
                                if (this.hitTile.AddDamage(tileId, num232, true) >= 100)
                                {
                                    this.hitTile.Clear(tileId);
                                    WorldGen.KillWall(num254, num255, false);
                                    if (Main.netMode == 1)
                                    {
                                        NetMessage.SendData(17, -1, -1, "", 2, (float)num254, (float)num255, 0f, 0, 0, 0);
                                    }
                                }
                                else
                                {
                                    WorldGen.KillWall(num254, num255, true);
                                    if (Main.netMode == 1)
                                    {
                                        NetMessage.SendData(17, -1, -1, "", 2, (float)num254, (float)num255, 1f, 0, 0, 0);
                                    }
                                }
                                if (num232 != 0)
                                {
                                    this.hitTile.Prune();
                                }
                                this.itemTime = item.useTime / 2;
                            }
                        }
                    }
                }
                if (Main.myPlayer == this.whoAmI && item.type == 1326 && this.itemAnimation > 0 && this.itemTime == 0)
                {
                    this.itemTime = item.useTime;
                    Vector2 vector19;
                    vector19.X = (float)Main.mouseX + Main.screenPosition.X;
                    if (this.gravDir == 1f)
                    {
                        vector19.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)this.height;
                    }
                    else
                    {
                        vector19.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
                    }
                    vector19.X -= (float)(this.width / 2);
                    if (vector19.X > 50f && vector19.X < (float)(Main.maxTilesX * 16 - 50) && vector19.Y > 50f && vector19.Y < (float)(Main.maxTilesY * 16 - 50))
                    {
                        int num266 = (int)(vector19.X / 16f);
                        int num267 = (int)(vector19.Y / 16f);
                        if ((Main.tile[num266, num267].wall != 87 || (double)num267 <= Main.worldSurface || NPC.downedPlantBoss) && !Collision.SolidCollision(vector19, this.width, this.height))
                        {
                            this.Teleport(vector19, 1, 0);
                            NetMessage.SendData(65, -1, -1, "", 0, (float)this.whoAmI, vector19.X, vector19.Y, 1, 0, 0);
                            if (this.chaosState)
                            {
                                this.statLife -= this.statLifeMax2 / 7;
                                if (Lang.lang <= 1)
                                {
                                    string deathText = " didn't materialize";
                                    if (Main.rand.Next(2) == 0)
                                    {
                                        if (this.Male)
                                        {
                                            deathText = "'s legs appeared where his head should be";
                                        }
                                        else
                                        {
                                            deathText = "'s legs appeared where her head should be";
                                        }
                                    }
                                    if (this.statLife <= 0)
                                    {
                                        this.KillMe(1.0, 0, false, deathText);
                                    }
                                }
                                else if (this.statLife <= 0)
                                {
                                    this.KillMe(1.0, 0, false, "");
                                }
                                this.lifeRegenCount = 0;
                                this.lifeRegenTime = 0;
                            }
                            this.AddBuff(88, 360, true);
                        }
                    }
                }
                if (item.type == 29 && this.itemAnimation > 0 && this.statLifeMax < 400 && this.itemTime == 0)
                {
                    this.itemTime = item.useTime;
                    this.statLifeMax += 20;
                    this.statLifeMax2 += 20;
                    this.statLife += 20;
                    if (Main.myPlayer == this.whoAmI)
                    {
                        this.HealEffect(20, true);
                    }
                    AchievementsHelper.HandleSpecialEvent(this, 0);
                }
                if (item.type == 1291 && this.itemAnimation > 0 && this.statLifeMax >= 400 && this.statLifeMax < 500 && this.itemTime == 0)
                {
                    this.itemTime = item.useTime;
                    this.statLifeMax += 5;
                    this.statLifeMax2 += 5;
                    this.statLife += 5;
                    if (Main.myPlayer == this.whoAmI)
                    {
                        this.HealEffect(5, true);
                    }
                    AchievementsHelper.HandleSpecialEvent(this, 2);
                }
                if (item.type == 109 && this.itemAnimation > 0 && this.statManaMax < 200 && this.itemTime == 0)
                {
                    this.itemTime = item.useTime;
                    this.statManaMax += 20;
                    this.statManaMax2 += 20;
                    this.statMana += 20;
                    if (Main.myPlayer == this.whoAmI)
                    {
                        this.ManaEffect(20);
                    }
                    AchievementsHelper.HandleSpecialEvent(this, 1);
                }
                if (item.type == 3335 && this.itemAnimation > 0 && !this.extraAccessory && Main.expertMode && this.itemTime == 0)
                {
                    this.itemTime = item.useTime;
                    this.extraAccessory = true;
                    NetMessage.SendData(4, -1, -1, Main.player[this.whoAmI].name, this.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                }
                this.PlaceThing();
            }
            if (item.type == 3542)
            {
                Vector2 vector20 = Main.OffsetsPlayerOnhand[this.bodyFrame.Y / 56] * 2f;
                if (this.direction != 1)
                {
                    vector20.X = (float)this.bodyFrame.Width - vector20.X;
                }
                if (this.gravDir != 1f)
                {
                    vector20.Y = (float)this.bodyFrame.Height - vector20.Y;
                }
                vector20 -= new Vector2((float)(this.bodyFrame.Width - this.width), (float)(this.bodyFrame.Height - 42)) / 2f;
                Vector2 position17 = this.RotatedRelativePoint(this.position + vector20, true) - this.velocity;
                for (int num268 = 0; num268 < 4; num268++)
                {
                    Dust dust = Main.dust[Dust.NewDust(base.Center, 0, 0, 242, (float)(this.direction * 2), 0f, 150, default(Color), 1.3f)];
                    dust.position = position17;
                    dust.velocity *= 0f;
                    dust.noGravity = true;
                    dust.fadeIn = 1f;
                    dust.velocity += this.velocity;
                    if (Main.rand.Next(2) == 0)
                    {
                        dust.position += Utils.RandomVector2(Main.rand, -4f, 4f);
                        dust.scale += Main.rand.NextFloat();
                        if (Main.rand.Next(2) == 0)
                        {
                            dust.customData = this;
                        }
                    }
                }
            }
            if (((item.damage >= 0 && item.type > 0 && !item.noMelee) || item.type == 1450 || item.type == 1991 || item.type == 3183 || item.type == 3542 || item.type == 3779) && this.itemAnimation > 0)
            {
                bool flag24 = false;
                Rectangle r2 = new Rectangle((int)this.itemLocation.X, (int)this.itemLocation.Y, 32, 32);
                if (!Main.dedServ)
                {
                    r2 = new Rectangle((int)this.itemLocation.X, (int)this.itemLocation.Y, Main.itemTexture[item.type].Width, Main.itemTexture[item.type].Height);
                }
                r2.Width = (int)((float)r2.Width * item.scale);
                r2.Height = (int)((float)r2.Height * item.scale);
                if (this.direction == -1)
                {
                    r2.X -= r2.Width;
                }
                if (this.gravDir == 1f)
                {
                    r2.Y -= r2.Height;
                }
                if (item.useStyle == 1)
                {
                    if ((double)this.itemAnimation < (double)this.itemAnimationMax * 0.333)
                    {
                        if (this.direction == -1)
                        {
                            r2.X -= (int)((double)r2.Width * 1.4 - (double)r2.Width);
                        }
                        r2.Width = (int)((double)r2.Width * 1.4);
                        r2.Y += (int)((double)r2.Height * 0.5 * (double)this.gravDir);
                        r2.Height = (int)((double)r2.Height * 1.1);
                    }
                    else if ((double)this.itemAnimation >= (double)this.itemAnimationMax * 0.666)
                    {
                        if (this.direction == 1)
                        {
                            r2.X -= (int)((double)r2.Width * 1.2);
                        }
                        r2.Width *= 2;
                        r2.Y -= (int)(((double)r2.Height * 1.4 - (double)r2.Height) * (double)this.gravDir);
                        r2.Height = (int)((double)r2.Height * 1.4);
                    }
                }
                else if (item.useStyle == 3)
                {
                    //patch file: flag24
                    if ((double)this.itemAnimation > (double)this.itemAnimationMax * 0.666)
                    {
                        flag24 = true;
                    }
                    else
                    {
                        if (this.direction == -1)
                        {
                            r2.X -= (int)((double)r2.Width * 1.4 - (double)r2.Width);
                        }
                        r2.Width = (int)((double)r2.Width * 1.4);
                        r2.Y += (int)((double)r2.Height * 0.6);
                        r2.Height = (int)((double)r2.Height * 0.6);
                    }
                }
                ItemLoader.UseItemHitbox(item, this, ref r2, ref flag24);
                float arg_CB50_0 = this.gravDir;
                if (item.type == 1450 && Main.rand.Next(3) == 0)
                {
                    int num269 = -1;
                    float x5 = (float)(r2.X + Main.rand.Next(r2.Width));
                    float y5 = (float)(r2.Y + Main.rand.Next(r2.Height));
                    if (Main.rand.Next(500) == 0)
                    {
                        num269 = Gore.NewGore(new Vector2(x5, y5), default(Vector2), 415, (float)Main.rand.Next(51, 101) * 0.01f);
                    }
                    else if (Main.rand.Next(250) == 0)
                    {
                        num269 = Gore.NewGore(new Vector2(x5, y5), default(Vector2), 414, (float)Main.rand.Next(51, 101) * 0.01f);
                    }
                    else if (Main.rand.Next(80) == 0)
                    {
                        num269 = Gore.NewGore(new Vector2(x5, y5), default(Vector2), 413, (float)Main.rand.Next(51, 101) * 0.01f);
                    }
                    else if (Main.rand.Next(10) == 0)
                    {
                        num269 = Gore.NewGore(new Vector2(x5, y5), default(Vector2), 412, (float)Main.rand.Next(51, 101) * 0.01f);
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        num269 = Gore.NewGore(new Vector2(x5, y5), default(Vector2), 411, (float)Main.rand.Next(51, 101) * 0.01f);
                    }
                    if (num269 >= 0)
                    {
                        Gore expr_CD62_cp_0 = Main.gore[num269];
                        expr_CD62_cp_0.velocity.X = expr_CD62_cp_0.velocity.X + (float)(this.direction * 2);
                        Gore expr_CD86_cp_0 = Main.gore[num269];
                        expr_CD86_cp_0.velocity.Y = expr_CD86_cp_0.velocity.Y * 0.3f;
                    }
                }
                if (item.type == 3542)
                {
                    flag24 = true;
                }
                if (item.type == 3779)
                {
                    flag24 = true;
                    Vector2 vector21 = this.itemLocation + new Vector2((float)(this.direction * 30), -8f);
                    int arg_CDF0_0 = this.itemAnimation;
                    int arg_CDEF_0 = this.itemAnimationMax - 2;
                    Vector2 value14 = vector21 - this.position;
                    for (float num270 = 0f; num270 < 1f; num270 += 0.2f)
                    {
                        Vector2 position18 = Vector2.Lerp(this.oldPosition + value14 + new Vector2(0f, this.gfxOffY), vector21, num270);
                        Dust dust2 = Main.dust[Dust.NewDust(vector21 - Vector2.One * 8f, 16, 16, 27, 0f, -2f, 0, default(Color), 1f)];
                        dust2.noGravity = true;
                        dust2.position = position18;
                        dust2.velocity = new Vector2(0f, -this.gravDir * 2f);
                        dust2.scale = 1.2f;
                        dust2.alpha = 200;
                    }
                }
                if (!flag24)
                {
                    if (item.type == 989 && Main.rand.Next(5) == 0)
                    {
                        int num271 = Main.rand.Next(3);
                        if (num271 == 0)
                        {
                            num271 = 15;
                        }
                        else if (num271 == 1)
                        {
                            num271 = 57;
                        }
                        else
                        {
                            num271 = 58;
                        }
                        int num272 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, num271, (float)(this.direction * 2), 0f, 150, default(Color), 1.3f);
                        Main.dust[num272].velocity *= 0.2f;
                    }
                    if (item.type == 2880 && Main.rand.Next(2) == 0)
                    {
                        int type6 = Utils.SelectRandom<int>(Main.rand, new int[]
                            {
                                226,
                                229
                            });
                        int num273 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, type6, (float)(this.direction * 2), 0f, 150, default(Color), 1f);
                        Main.dust[num273].velocity *= 0.2f;
                        Main.dust[num273].noGravity = true;
                    }
                    if ((item.type == 44 || item.type == 45 || item.type == 46 || item.type == 103 || item.type == 104) && Main.rand.Next(15) == 0)
                    {
                        Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 14, (float)(this.direction * 2), 0f, 150, default(Color), 1.3f);
                    }
                    if (item.type == 273 || item.type == 675)
                    {
                        if (Main.rand.Next(5) == 0)
                        {
                            Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 14, (float)(this.direction * 2), 0f, 150, default(Color), 1.4f);
                        }
                        int num274 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 27, this.velocity.X * 0.2f + (float)(this.direction * 3), this.velocity.Y * 0.2f, 100, default(Color), 1.2f);
                        Main.dust[num274].noGravity = true;
                        Dust expr_D26A_cp_0 = Main.dust[num274];
                        expr_D26A_cp_0.velocity.X = expr_D26A_cp_0.velocity.X / 2f;
                        Dust expr_D28A_cp_0 = Main.dust[num274];
                        expr_D28A_cp_0.velocity.Y = expr_D28A_cp_0.velocity.Y / 2f;
                    }
                    if (item.type == 723 && Main.rand.Next(2) == 0)
                    {
                        int num275 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 64, 0f, 0f, 150, default(Color), 1.2f);
                        Main.dust[num275].noGravity = true;
                    }
                    if (item.type == 65)
                    {
                        if (Main.rand.Next(5) == 0)
                        {
                            Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 58, 0f, 0f, 150, default(Color), 1.2f);
                        }
                        if (Main.rand.Next(10) == 0)
                        {
                            Gore.NewGore(new Vector2((float)r2.X, (float)r2.Y), default(Vector2), Main.rand.Next(16, 18), 1f);
                        }
                    }
                    if (item.type == 3065)
                    {
                        int num276 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 58, 0f, 0f, 150, default(Color), 1.2f);
                        Main.dust[num276].velocity *= 0.5f;
                        if (Main.rand.Next(8) == 0)
                        {
                            int num277 = Gore.NewGore(new Vector2((float)r2.Center.X, (float)r2.Center.Y), default(Vector2), 16, 1f);
                            Main.gore[num277].velocity *= 0.5f;
                            Main.gore[num277].velocity += new Vector2((float)this.direction, 0f);
                        }
                    }
                    if (item.type == 190)
                    {
                        int num278 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 40, this.velocity.X * 0.2f + (float)(this.direction * 3), this.velocity.Y * 0.2f, 0, default(Color), 1.2f);
                        Main.dust[num278].noGravity = true;
                    }
                    else if (item.type == 213)
                    {
                        int num279 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 3, this.velocity.X * 0.2f + (float)(this.direction * 3), this.velocity.Y * 0.2f, 0, default(Color), 1.2f);
                        Main.dust[num279].noGravity = true;
                    }
                    if (item.type == 121)
                    {
                        for (int num280 = 0; num280 < 2; num280++)
                        {
                            int num281 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 6, this.velocity.X * 0.2f + (float)(this.direction * 3), this.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                            Main.dust[num281].noGravity = true;
                            Dust expr_D6D8_cp_0 = Main.dust[num281];
                            expr_D6D8_cp_0.velocity.X = expr_D6D8_cp_0.velocity.X * 2f;
                            Dust expr_D6F8_cp_0 = Main.dust[num281];
                            expr_D6F8_cp_0.velocity.Y = expr_D6F8_cp_0.velocity.Y * 2f;
                        }
                    }
                    if (item.type == 122 || item.type == 217)
                    {
                        int num282 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 6, this.velocity.X * 0.2f + (float)(this.direction * 3), this.velocity.Y * 0.2f, 100, default(Color), 1.9f);
                        Main.dust[num282].noGravity = true;
                    }
                    if (item.type == 155)
                    {
                        int num283 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 172, this.velocity.X * 0.2f + (float)(this.direction * 3), this.velocity.Y * 0.2f, 100, default(Color), 0.9f);
                        Main.dust[num283].noGravity = true;
                        Main.dust[num283].velocity *= 0.1f;
                    }
                    if (item.type == 676 && Main.rand.Next(3) == 0)
                    {
                        int num284 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 67, this.velocity.X * 0.2f + (float)(this.direction * 3), this.velocity.Y * 0.2f, 90, default(Color), 1.5f);
                        Main.dust[num284].noGravity = true;
                        Main.dust[num284].velocity *= 0.2f;
                    }
                    if (item.type == 3063)
                    {
                        int num285 = Dust.NewDust(r2.TopLeft(), r2.Width, r2.Height, 66, 0f, 0f, 150, Color.Transparent, 0.85f);
                        Main.dust[num285].color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f);
                        Main.dust[num285].noGravity = true;
                        Main.dust[num285].velocity /= 2f;
                    }
                    if (item.type == 724 && Main.rand.Next(5) == 0)
                    {
                        int num286 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 67, this.velocity.X * 0.2f + (float)(this.direction * 3), this.velocity.Y * 0.2f, 90, default(Color), 1.5f);
                        Main.dust[num286].noGravity = true;
                        Main.dust[num286].velocity *= 0.2f;
                    }
                    if (item.type >= 795 && item.type <= 802 && Main.rand.Next(3) == 0)
                    {
                        int num287 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 115, this.velocity.X * 0.2f + (float)(this.direction * 3), this.velocity.Y * 0.2f, 140, default(Color), 1.5f);
                        Main.dust[num287].noGravity = true;
                        Main.dust[num287].velocity *= 0.25f;
                    }
                    if (item.type == 367 || item.type == 368 || item.type == 674)
                    {
                        if (Main.rand.Next(3) == 0)
                        {
                            int num288 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 57, this.velocity.X * 0.2f + (float)(this.direction * 3), this.velocity.Y * 0.2f, 100, default(Color), 1.1f);
                            Main.dust[num288].noGravity = true;
                            Dust expr_DC56_cp_0 = Main.dust[num288];
                            expr_DC56_cp_0.velocity.X = expr_DC56_cp_0.velocity.X / 2f;
                            Dust expr_DC76_cp_0 = Main.dust[num288];
                            expr_DC76_cp_0.velocity.Y = expr_DC76_cp_0.velocity.Y / 2f;
                            Dust expr_DC96_cp_0 = Main.dust[num288];
                            expr_DC96_cp_0.velocity.X = expr_DC96_cp_0.velocity.X + (float)(this.direction * 2);
                        }
                        if (Main.rand.Next(4) == 0)
                        {
                            int num288 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 43, 0f, 0f, 254, default(Color), 0.3f);
                            Main.dust[num288].velocity *= 0f;
                        }
                    }
                    if ((item.type >= 198 && item.type <= 203) || (item.type >= 3764 && item.type <= 3769))
                    {
                        float num289 = 0.5f;
                        float num290 = 0.5f;
                        float num291 = 0.5f;
                        if (item.type == 198 || item.type == 3764)
                        {
                            num289 *= 0.1f;
                            num290 *= 0.5f;
                            num291 *= 1.2f;
                        }
                        else if (item.type == 199 || item.type == 3765)
                        {
                            num289 *= 1f;
                            num290 *= 0.2f;
                            num291 *= 0.1f;
                        }
                        else if (item.type == 200 || item.type == 3766)
                        {
                            num289 *= 0.1f;
                            num290 *= 1f;
                            num291 *= 0.2f;
                        }
                        else if (item.type == 201 || item.type == 3767)
                        {
                            num289 *= 0.8f;
                            num290 *= 0.1f;
                            num291 *= 1f;
                        }
                        else if (item.type == 202 || item.type == 3768)
                        {
                            num289 *= 0.8f;
                            num290 *= 0.9f;
                            num291 *= 1f;
                        }
                        else if (item.type == 203 || item.type == 3769)
                        {
                            num289 *= 0.9f;
                            num290 *= 0.9f;
                            num291 *= 0.1f;
                        }
                        Lighting.AddLight((int)((this.itemLocation.X + 6f + this.velocity.X) / 16f), (int)((this.itemLocation.Y - 14f) / 16f), num289, num290, num291);
                    }
                    if (this.frostBurn && item.melee && !item.noMelee && !item.noUseGraphic && Main.rand.Next(2) == 0)
                    {
                        int num292 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 135, this.velocity.X * 0.2f + (float)(this.direction * 3), this.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                        Main.dust[num292].noGravity = true;
                        Main.dust[num292].velocity *= 0.7f;
                        Dust expr_E073_cp_0 = Main.dust[num292];
                        expr_E073_cp_0.velocity.Y = expr_E073_cp_0.velocity.Y - 0.5f;
                    }
                    if (item.melee && !item.noMelee && !item.noUseGraphic && this.meleeEnchant > 0)
                    {
                        if (this.meleeEnchant == 1)
                        {
                            if (Main.rand.Next(3) == 0)
                            {
                                int num293 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 171, 0f, 0f, 100, default(Color), 1f);
                                Main.dust[num293].noGravity = true;
                                Main.dust[num293].fadeIn = 1.5f;
                                Main.dust[num293].velocity *= 0.25f;
                            }
                        }
                        else if (this.meleeEnchant == 2)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                int num294 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 75, this.velocity.X * 0.2f + (float)(this.direction * 3), this.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                                Main.dust[num294].noGravity = true;
                                Main.dust[num294].velocity *= 0.7f;
                                Dust expr_E23E_cp_0 = Main.dust[num294];
                                expr_E23E_cp_0.velocity.Y = expr_E23E_cp_0.velocity.Y - 0.5f;
                            }
                        }
                        else if (this.meleeEnchant == 3)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                int num295 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 6, this.velocity.X * 0.2f + (float)(this.direction * 3), this.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                                Main.dust[num295].noGravity = true;
                                Main.dust[num295].velocity *= 0.7f;
                                Dust expr_E324_cp_0 = Main.dust[num295];
                                expr_E324_cp_0.velocity.Y = expr_E324_cp_0.velocity.Y - 0.5f;
                            }
                        }
                        else if (this.meleeEnchant == 4)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                int num296 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 57, this.velocity.X * 0.2f + (float)(this.direction * 3), this.velocity.Y * 0.2f, 100, default(Color), 1.1f);
                                Main.dust[num296].noGravity = true;
                                Dust expr_E3F1_cp_0 = Main.dust[num296];
                                expr_E3F1_cp_0.velocity.X = expr_E3F1_cp_0.velocity.X / 2f;
                                Dust expr_E411_cp_0 = Main.dust[num296];
                                expr_E411_cp_0.velocity.Y = expr_E411_cp_0.velocity.Y / 2f;
                            }
                        }
                        else if (this.meleeEnchant == 5)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                int num297 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 169, 0f, 0f, 100, default(Color), 1f);
                                Dust expr_E4AA_cp_0 = Main.dust[num297];
                                expr_E4AA_cp_0.velocity.X = expr_E4AA_cp_0.velocity.X + (float)this.direction;
                                Dust expr_E4CC_cp_0 = Main.dust[num297];
                                expr_E4CC_cp_0.velocity.Y = expr_E4CC_cp_0.velocity.Y + 0.2f;
                                Main.dust[num297].noGravity = true;
                            }
                        }
                        else if (this.meleeEnchant == 6)
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                int num298 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 135, 0f, 0f, 100, default(Color), 1f);
                                Dust expr_E575_cp_0 = Main.dust[num298];
                                expr_E575_cp_0.velocity.X = expr_E575_cp_0.velocity.X + (float)this.direction;
                                Dust expr_E597_cp_0 = Main.dust[num298];
                                expr_E597_cp_0.velocity.Y = expr_E597_cp_0.velocity.Y + 0.2f;
                                Main.dust[num298].noGravity = true;
                            }
                        }
                        else if (this.meleeEnchant == 7)
                        {
                            if (Main.rand.Next(20) == 0)
                            {
                                int type7 = Main.rand.Next(139, 143);
                                int num299 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, type7, this.velocity.X, this.velocity.Y, 0, default(Color), 1.2f);
                                Dust expr_E663_cp_0 = Main.dust[num299];
                                expr_E663_cp_0.velocity.X = expr_E663_cp_0.velocity.X * (1f + (float)Main.rand.Next(-50, 51) * 0.01f);
                                Dust expr_E699_cp_0 = Main.dust[num299];
                                expr_E699_cp_0.velocity.Y = expr_E699_cp_0.velocity.Y * (1f + (float)Main.rand.Next(-50, 51) * 0.01f);
                                Dust expr_E6CF_cp_0 = Main.dust[num299];
                                expr_E6CF_cp_0.velocity.X = expr_E6CF_cp_0.velocity.X + (float)Main.rand.Next(-50, 51) * 0.05f;
                                Dust expr_E6FF_cp_0 = Main.dust[num299];
                                expr_E6FF_cp_0.velocity.Y = expr_E6FF_cp_0.velocity.Y + (float)Main.rand.Next(-50, 51) * 0.05f;
                                Main.dust[num299].scale *= 1f + (float)Main.rand.Next(-30, 31) * 0.01f;
                            }
                            if (Main.rand.Next(40) == 0)
                            {
                                int type8 = Main.rand.Next(276, 283);
                                int num300 = Gore.NewGore(new Vector2((float)r2.X, (float)r2.Y), this.velocity, type8, 1f);
                                Gore expr_E7BA_cp_0 = Main.gore[num300];
                                expr_E7BA_cp_0.velocity.X = expr_E7BA_cp_0.velocity.X * (1f + (float)Main.rand.Next(-50, 51) * 0.01f);
                                Gore expr_E7F0_cp_0 = Main.gore[num300];
                                expr_E7F0_cp_0.velocity.Y = expr_E7F0_cp_0.velocity.Y * (1f + (float)Main.rand.Next(-50, 51) * 0.01f);
                                Main.gore[num300].scale *= 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
                                Gore expr_E857_cp_0 = Main.gore[num300];
                                expr_E857_cp_0.velocity.X = expr_E857_cp_0.velocity.X + (float)Main.rand.Next(-50, 51) * 0.05f;
                                Gore expr_E887_cp_0 = Main.gore[num300];
                                expr_E887_cp_0.velocity.Y = expr_E887_cp_0.velocity.Y + (float)Main.rand.Next(-50, 51) * 0.05f;
                            }
                        }
                        else if (this.meleeEnchant == 8 && Main.rand.Next(4) == 0)
                        {
                            int num301 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 46, 0f, 0f, 100, default(Color), 1f);
                            Main.dust[num301].noGravity = true;
                            Main.dust[num301].fadeIn = 1.5f;
                            Main.dust[num301].velocity *= 0.25f;
                        }
                    }
                    if (this.magmaStone && item.melee && !item.noMelee && !item.noUseGraphic && Main.rand.Next(3) != 0)
                    {
                        int num302 = Dust.NewDust(new Vector2((float)r2.X, (float)r2.Y), r2.Width, r2.Height, 6, this.velocity.X * 0.2f + (float)(this.direction * 3), this.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                        Main.dust[num302].noGravity = true;
                        Dust expr_EA32_cp_0 = Main.dust[num302];
                        expr_EA32_cp_0.velocity.X = expr_EA32_cp_0.velocity.X * 2f;
                        Dust expr_EA52_cp_0 = Main.dust[num302];
                        expr_EA52_cp_0.velocity.Y = expr_EA52_cp_0.velocity.Y * 2f;
                    }
                    ItemLoader.MeleeEffects(item, this, r2);
                    PlayerHooks.MeleeEffects(this, item, r2);
                    if (Main.myPlayer == i && (item.type == 1991 || item.type == 3183))
                    {
                        for (int num303 = 0; num303 < 200; num303++)
                        {
                            if (Main.npc[num303].active && Main.npc[num303].catchItem > 0)
                            {
                                Rectangle value15 = new Rectangle((int)Main.npc[num303].position.X, (int)Main.npc[num303].position.Y, Main.npc[num303].width, Main.npc[num303].height);
                                if (r2.Intersects(value15) && (item.type == 3183 || Main.npc[num303].noTileCollide || this.CanHit(Main.npc[num303])))
                                {
                                    NPC.CatchNPC(num303, i);
                                }
                            }
                        }
                    }
                    if (Main.myPlayer == i && (item.damage > 0 || item.type == 3183))
                    {
                        int num304 = (int)((float)item.damage * this.meleeDamage);
                        float num305 = item.knockBack;
                        float num306 = 1f;
                        if (this.kbGlove)
                        {
                            num306 += 1f;
                        }
                        if (this.kbBuff)
                        {
                            num306 += 0.5f;
                        }
                        num305 *= num306;
                        if (this.inventory[this.selectedItem].type == 3106)
                        {
                            num305 += num305 * (1f - this.stealth);
                        }
                        List<ushort> list2 = null;
                        int type5 = item.type;
                        if (type5 == 213)
                        {
                            list2 = new List<ushort>(new ushort[]
                                {
                                    3,
                                    24,
                                    52,
                                    61,
                                    62,
                                    71,
                                    73,
                                    74,
                                    82,
                                    83,
                                    84,
                                    110,
                                    113,
                                    115,
                                    184,
                                    205,
                                    201
                                });
                        }
                        int num307 = r2.X / 16;
                        int num308 = (r2.X + r2.Width) / 16 + 1;
                        int num309 = r2.Y / 16;
                        int num310 = (r2.Y + r2.Height) / 16 + 1;
                        for (int num311 = num307; num311 < num308; num311++)
                        {
                            for (int num312 = num309; num312 < num310; num312++)
                            {
                                if (Main.tile[num311, num312] != null && Main.tileCut[(int)Main.tile[num311, num312].type] && (list2 == null || !list2.Contains(Main.tile[num311, num312].type)) && Main.tile[num311, num312 + 1] != null && Main.tile[num311, num312 + 1].type != 78 && Main.tile[num311, num312 + 1].type != 380)
                                {
                                    if (item.type == 1786)
                                    {
                                        int type9 = (int)Main.tile[num311, num312].type;
                                        WorldGen.KillTile(num311, num312, false, false, false);
                                        if (!Main.tile[num311, num312].active())
                                        {
                                            int num313 = 0;
                                            if (type9 == 3 || type9 == 24 || type9 == 61 || type9 == 110 || type9 == 201)
                                            {
                                                num313 = Main.rand.Next(1, 3);
                                            }
                                            if (type9 == 73 || type9 == 74 || type9 == 113)
                                            {
                                                num313 = Main.rand.Next(2, 5);
                                            }
                                            if (num313 > 0)
                                            {
                                                int number = Item.NewItem(num311 * 16, num312 * 16, 16, 16, 1727, num313, false, 0, false, false);
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendData(21, -1, -1, "", number, 1f, 0f, 0f, 0, 0, 0);
                                                }
                                            }
                                        }
                                        if (Main.netMode == 1)
                                        {
                                            NetMessage.SendData(17, -1, -1, "", 0, (float)num311, (float)num312, 0f, 0, 0, 0);
                                        }
                                    }
                                    else
                                    {
                                        WorldGen.KillTile(num311, num312, false, false, false);
                                        if (Main.netMode == 1)
                                        {
                                            NetMessage.SendData(17, -1, -1, "", 0, (float)num311, (float)num312, 0f, 0, 0, 0);
                                        }
                                    }
                                }
                            }
                        }
                        if (item.type != 3183)
                        {
                            for (int num314 = 0; num314 < 200; num314++)
                            {
                                if (Main.npc[num314].active && Main.npc[num314].immune[i] == 0 && this.attackCD == 0)
                                {
                                    if (!Main.npc[num314].dontTakeDamage)
                                    {
                                        bool? modCanHit = ItemLoader.CanHitNPC(item, this, Main.npc[num314]);
                                        if (modCanHit.HasValue && !modCanHit.Value)
                                        {
                                            continue;
                                        }
                                        bool? modCanBeHit = NPCLoader.CanBeHitByItem(Main.npc[num314], this, item);
                                        if (modCanBeHit.HasValue && !modCanBeHit.Value)
                                        {
                                            continue;
                                        }
                                        bool? modCanHit2 = PlayerHooks.CanHitNPC(this, item, Main.npc[num314]);
                                        if (modCanHit2.HasValue && !modCanHit2.Value)
                                        {
                                            continue;
                                        }
                                        bool canHitFlag = (modCanHit.HasValue && modCanHit.Value) || (modCanBeHit.HasValue && modCanBeHit.Value) || (modCanHit2.HasValue && modCanHit2.Value);
                                        if (!Main.npc[num314].friendly || (Main.npc[num314].type == 22 && this.killGuide) || (Main.npc[num314].type == 54 && this.killClothier) || canHitFlag)
                                        {
                                            Rectangle value16 = new Rectangle((int)Main.npc[num314].position.X, (int)Main.npc[num314].position.Y, Main.npc[num314].width, Main.npc[num314].height);
                                            if (r2.Intersects(value16) && (Main.npc[num314].noTileCollide || this.CanHit(Main.npc[num314])))
                                            {
                                                bool flag25 = false;
                                                if (Main.rand.Next(1, 101) <= this.meleeCrit)
                                                {
                                                    flag25 = true;
                                                }
                                                int num315 = Item.NPCtoBanner(Main.npc[num314].BannerID());
                                                if (num315 > 0 && this.NPCBannerBuff[num315])
                                                {
                                                    if (Main.expertMode)
                                                    {
                                                        num304 *= 2;
                                                    }
                                                    else
                                                    {
                                                        num304 = (int)((double)num304 * 1.5);
                                                    }
                                                }
                                                int num316 = Main.DamageVar((float)num304);
                                                ItemLoader.ModifyHitNPC(item, this, Main.npc[num314], ref num316, ref num305, ref flag25);
                                                NPCLoader.ModifyHitByItem(Main.npc[num314], this, item, ref num316, ref num305, ref flag25);
                                                PlayerHooks.ModifyHitNPC(this, item, Main.npc[num314], ref num316, ref num305, ref flag25);
                                                this.StatusNPC(item.type, num314);
                                                this.OnHit(Main.npc[num314].Center.X, Main.npc[num314].Center.Y, Main.npc[num314]);
                                                if (this.armorPenetration > 0)
                                                {
                                                    num316 += Main.npc[num314].checkArmorPenetration(this.armorPenetration);
                                                }
                                                //patch file: num305, flag25, num317
                                                int num317 = (int)Main.npc[num314].StrikeNPC(num316, num305, this.direction, flag25, false, false);
                                                if (this.inventory[this.selectedItem].type == 3211)
                                                {
                                                    Vector2 value17 = new Vector2((float)(this.direction * 100 + Main.rand.Next(-25, 26)), (float)Main.rand.Next(-75, 76));
                                                    value17.Normalize();
                                                    value17 *= (float)Main.rand.Next(30, 41) * 0.1f;
                                                    Vector2 value18 = new Vector2((float)(r2.X + Main.rand.Next(r2.Width)), (float)(r2.Y + Main.rand.Next(r2.Height)));
                                                    value18 = (value18 + Main.npc[num314].Center * 2f) / 3f;
                                                    Projectile.NewProjectile(value18.X, value18.Y, value17.X, value17.Y, 524, (int)((double)num304 * 0.7), num305 * 0.7f, this.whoAmI, 0f, 0f);
                                                }
                                                bool flag26 = !Main.npc[num314].immortal;
                                                if (this.beetleOffense && flag26)
                                                {
                                                    this.beetleCounter += (float)num317;
                                                    this.beetleCountdown = 0;
                                                }
                                                if (item.type == 1826 && (Main.npc[num314].value > 0f || (Main.npc[num314].damage > 0 && !Main.npc[num314].friendly)))
                                                {
                                                    this.pumpkinSword(num314, (int)((double)num304 * 1.5), num305);
                                                }
                                                if (this.meleeEnchant == 7)
                                                {
                                                    Projectile.NewProjectile(Main.npc[num314].Center.X, Main.npc[num314].Center.Y, Main.npc[num314].velocity.X, Main.npc[num314].velocity.Y, 289, 0, 0f, this.whoAmI, 0f, 0f);
                                                }
                                                if (this.inventory[this.selectedItem].type == 3106)
                                                {
                                                    this.stealth = 1f;
                                                    if (Main.netMode == 1)
                                                    {
                                                        NetMessage.SendData(84, -1, -1, "", this.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                                                    }
                                                }
                                                if (item.type == 1123 && flag26)
                                                {
                                                    int num318 = Main.rand.Next(1, 4);
                                                    if (this.strongBees && Main.rand.Next(3) == 0)
                                                    {
                                                        num318++;
                                                    }
                                                    for (int num319 = 0; num319 < num318; num319++)
                                                    {
                                                        float num320 = (float)(this.direction * 2) + (float)Main.rand.Next(-35, 36) * 0.02f;
                                                        float num321 = (float)Main.rand.Next(-35, 36) * 0.02f;
                                                        num320 *= 0.2f;
                                                        num321 *= 0.2f;
                                                        Projectile.NewProjectile((float)(r2.X + r2.Width / 2), (float)(r2.Y + r2.Height / 2), num320, num321, this.beeType(), this.beeDamage(num316 / 3), this.beeKB(0f), i, 0f, 0f);
                                                    }
                                                }
                                                if (Main.npc[num314].value > 0f && this.coins && Main.rand.Next(5) == 0)
                                                {
                                                    int type10 = 71;
                                                    if (Main.rand.Next(10) == 0)
                                                    {
                                                        type10 = 72;
                                                    }
                                                    if (Main.rand.Next(100) == 0)
                                                    {
                                                        type10 = 73;
                                                    }
                                                    int num322 = Item.NewItem((int)Main.npc[num314].position.X, (int)Main.npc[num314].position.Y, Main.npc[num314].width, Main.npc[num314].height, type10, 1, false, 0, false, false);
                                                    Main.item[num322].stack = Main.rand.Next(1, 11);
                                                    Main.item[num322].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
                                                    Main.item[num322].velocity.X = (float)Main.rand.Next(10, 31) * 0.2f * (float)this.direction;
                                                    if (Main.netMode == 1)
                                                    {
                                                        NetMessage.SendData(21, -1, -1, "", num322, 0f, 0f, 0f, 0, 0, 0);
                                                    }
                                                }
                                                ItemLoader.OnHitNPC(item, this, Main.npc[num314], num317, num305, flag25);
                                                NPCLoader.OnHitByItem(Main.npc[num314], this, item, num317, num305, flag25);
                                                PlayerHooks.OnHitNPC(this, item, Main.npc[num314], num317, num305, flag25);
                                                int num323 = Item.NPCtoBanner(Main.npc[num314].BannerID());
                                                if (num323 >= 0)
                                                {
                                                    this.lastCreatureHit = num323;
                                                }
                                                if (Main.netMode != 0)
                                                {
                                                    if (flag25)
                                                    {
                                                        NetMessage.SendData(28, -1, -1, "", num314, (float)num316, num305, (float)this.direction, 1, 0, 0);
                                                    }
                                                    else
                                                    {
                                                        NetMessage.SendData(28, -1, -1, "", num314, (float)num316, num305, (float)this.direction, 0, 0, 0);
                                                    }
                                                }
                                                if (this.accDreamCatcher)
                                                {
                                                    this.addDPS(num316);
                                                }
                                                Main.npc[num314].immune[i] = this.itemAnimation;
                                                this.attackCD = (int)((double)this.itemAnimationMax * 0.33);
                                            }
                                        }
                                    }
                                    else if (Main.npc[num314].type == 63 || Main.npc[num314].type == 64 || Main.npc[num314].type == 103 || Main.npc[num314].type == 242)
                                    {
                                        Rectangle value19 = new Rectangle((int)Main.npc[num314].position.X, (int)Main.npc[num314].position.Y, Main.npc[num314].width, Main.npc[num314].height);
                                        if (r2.Intersects(value19) && (Main.npc[num314].noTileCollide || this.CanHit(Main.npc[num314])))
                                        {
                                            this.Hurt((int)((double)Main.npc[num314].damage * 1.3), -this.direction, false, false, " was slain...", false, -1);
                                            Main.npc[num314].immune[i] = this.itemAnimation;
                                            this.attackCD = (int)((double)this.itemAnimationMax * 0.33);
                                        }
                                    }
                                }
                            }
                            if (this.hostile)
                            {
                                for (int num324 = 0; num324 < 255; num324++)
                                {
                                    if (num324 != i && Main.player[num324].active && Main.player[num324].hostile && !Main.player[num324].immune && !Main.player[num324].dead && (Main.player[i].team == 0 || Main.player[i].team != Main.player[num324].team))
                                    {
                                        Rectangle value20 = new Rectangle((int)Main.player[num324].position.X, (int)Main.player[num324].position.Y, Main.player[num324].width, Main.player[num324].height);
                                        if (ItemLoader.CanHitPvp(item, this, Main.player[num324]) && PlayerHooks.CanHitPvp(this, item, Main.player[num324]) && r2.Intersects(value20) && this.CanHit(Main.player[num324]))
                                        {
                                            bool flag27 = false;
                                            if (Main.rand.Next(1, 101) <= 10)
                                            {
                                                flag27 = true;
                                            }
                                            int num325 = Main.DamageVar((float)num304);
                                            ItemLoader.ModifyHitPvp(item, this, Main.player[num324], ref num325, ref flag27);
                                            PlayerHooks.ModifyHitPvp(this, item, Main.player[num324], ref num325, ref flag27);
                                            this.StatusPvP(item.type, num324);
                                            this.OnHit(Main.player[num324].Center.X, Main.player[num324].Center.Y, Main.player[num324]);
                                            int num326 = (int)Main.player[num324].Hurt(num325, this.direction, true, false, "", flag27, -1);
                                            if (this.inventory[this.selectedItem].type == 3211)
                                            {
                                                Vector2 value21 = new Vector2((float)(this.direction * 100 + Main.rand.Next(-25, 26)), (float)Main.rand.Next(-75, 76));
                                                value21.Normalize();
                                                value21 *= (float)Main.rand.Next(30, 41) * 0.1f;
                                                Vector2 value22 = new Vector2((float)(r2.X + Main.rand.Next(r2.Width)), (float)(r2.Y + Main.rand.Next(r2.Height)));
                                                value22 = (value22 + Main.player[num324].Center * 2f) / 3f;
                                                Projectile.NewProjectile(value22.X, value22.Y, value21.X, value21.Y, 524, (int)((double)num304 * 0.7), num305 * 0.7f, this.whoAmI, 0f, 0f);
                                            }
                                            if (this.beetleOffense)
                                            {
                                                this.beetleCounter += (float)num326;
                                                this.beetleCountdown = 0;
                                            }
                                            if (this.meleeEnchant == 7)
                                            {
                                                Projectile.NewProjectile(Main.player[num324].Center.X, Main.player[num324].Center.Y, Main.player[num324].velocity.X, Main.player[num324].velocity.Y, 289, 0, 0f, this.whoAmI, 0f, 0f);
                                            }
                                            if (item.type == 1123)
                                            {
                                                int num327 = Main.rand.Next(1, 4);
                                                if (this.strongBees && Main.rand.Next(3) == 0)
                                                {
                                                    num327++;
                                                }
                                                for (int num328 = 0; num328 < num327; num328++)
                                                {
                                                    float num329 = (float)(this.direction * 2) + (float)Main.rand.Next(-35, 36) * 0.02f;
                                                    float num330 = (float)Main.rand.Next(-35, 36) * 0.02f;
                                                    num329 *= 0.2f;
                                                    num330 *= 0.2f;
                                                    Projectile.NewProjectile((float)(r2.X + r2.Width / 2), (float)(r2.Y + r2.Height / 2), num329, num330, this.beeType(), this.beeDamage(num325 / 3), this.beeKB(0f), i, 0f, 0f);
                                                }
                                            }
                                            if (this.inventory[this.selectedItem].type == 3106)
                                            {
                                                this.stealth = 1f;
                                                if (Main.netMode == 1)
                                                {
                                                    NetMessage.SendData(84, -1, -1, "", this.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                                                }
                                            }
                                            if (item.type == 1826 && Main.npc[num324].value > 0f)
                                            {
                                                this.pumpkinSword(num324, (int)((double)num304 * 1.5), num305);
                                            }
                                            ItemLoader.OnHitPvp(item, this, Main.player[num324], num326, flag27);
                                            PlayerHooks.OnHitPvp(this, item, Main.player[num324], num326, flag27);
                                            if (Main.netMode != 0)
                                            {
                                                if (flag27)
                                                {
                                                    NetMessage.SendData(26, -1, -1, Lang.deathMsg(this.whoAmI, -1, -1, -1, 0, 0), num324, (float)this.direction, (float)num325, 1f, 1, 0, 0);
                                                }
                                                else
                                                {
                                                    NetMessage.SendData(26, -1, -1, Lang.deathMsg(this.whoAmI, -1, -1, -1, 0, 0), num324, (float)this.direction, (float)num325, 1f, 0, 0, 0);
                                                }
                                            }
                                            this.attackCD = (int)((double)this.itemAnimationMax * 0.33);
                                        }
                                    }
                                }
                            }
                            if (item.type == 787 && (this.itemAnimation == (int)((double)this.itemAnimationMax * 0.1) || this.itemAnimation == (int)((double)this.itemAnimationMax * 0.3) || this.itemAnimation == (int)((double)this.itemAnimationMax * 0.5) || this.itemAnimation == (int)((double)this.itemAnimationMax * 0.7) || this.itemAnimation == (int)((double)this.itemAnimationMax * 0.9)))
                            {
                                float num331 = 0f;
                                float num332 = 0f;
                                float num333 = 0f;
                                float num334 = 0f;
                                if (this.itemAnimation == (int)((double)this.itemAnimationMax * 0.9))
                                {
                                    num331 = -7f;
                                }
                                if (this.itemAnimation == (int)((double)this.itemAnimationMax * 0.7))
                                {
                                    num331 = -6f;
                                    num332 = 2f;
                                }
                                if (this.itemAnimation == (int)((double)this.itemAnimationMax * 0.5))
                                {
                                    num331 = -4f;
                                    num332 = 4f;
                                }
                                if (this.itemAnimation == (int)((double)this.itemAnimationMax * 0.3))
                                {
                                    num331 = -2f;
                                    num332 = 6f;
                                }
                                if (this.itemAnimation == (int)((double)this.itemAnimationMax * 0.1))
                                {
                                    num332 = 7f;
                                }
                                if (this.itemAnimation == (int)((double)this.itemAnimationMax * 0.7))
                                {
                                    num334 = 26f;
                                }
                                if (this.itemAnimation == (int)((double)this.itemAnimationMax * 0.3))
                                {
                                    num334 -= 4f;
                                    num333 -= 20f;
                                }
                                if (this.itemAnimation == (int)((double)this.itemAnimationMax * 0.1))
                                {
                                    num333 += 6f;
                                }
                                if (this.direction == -1)
                                {
                                    if (this.itemAnimation == (int)((double)this.itemAnimationMax * 0.9))
                                    {
                                        num334 -= 8f;
                                    }
                                    if (this.itemAnimation == (int)((double)this.itemAnimationMax * 0.7))
                                    {
                                        num334 -= 6f;
                                    }
                                }
                                num331 *= 1.5f;
                                num332 *= 1.5f;
                                num334 *= (float)this.direction;
                                num333 *= this.gravDir;
                                Projectile.NewProjectile((float)(r2.X + r2.Width / 2) + num334, (float)(r2.Y + r2.Height / 2) + num333, (float)this.direction * num332, num331 * this.gravDir, 131, num304 / 2, 0f, i, 0f, 0f);
                            }
                        }
                    }
                }
            }
            if (this.itemTime == 0 && this.itemAnimation > 0)
            {
                if (ItemLoader.UseItem(item, this))
                {
                    this.itemTime = item.useTime;
                }
                if (item.hairDye >= 0)
                {
                    this.itemTime = item.useTime;
                    if (this.whoAmI == Main.myPlayer)
                    {
                        this.hairDye = (byte)item.hairDye;
                        NetMessage.SendData(4, -1, -1, Main.player[this.whoAmI].name, this.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
                if (item.healLife > 0)
                {
                    this.statLife += item.healLife;
                    this.itemTime = item.useTime;
                    if (Main.myPlayer == this.whoAmI)
                    {
                        this.HealEffect(item.healLife, true);
                    }
                }
                if (item.healMana > 0)
                {
                    this.statMana += item.healMana;
                    this.itemTime = item.useTime;
                    if (Main.myPlayer == this.whoAmI)
                    {
                        this.AddBuff(94, Player.manaSickTime, true);
                        this.ManaEffect(item.healMana);
                    }
                }
                if (item.buffType > 0)
                {
                    if (this.whoAmI == Main.myPlayer && item.buffType != 90 && item.buffType != 27)
                    {
                        this.AddBuff(item.buffType, item.buffTime, true);
                    }
                    this.itemTime = item.useTime;
                }
                if (item.type == 678)
                {
                    this.itemTime = item.useTime;
                    if (this.whoAmI == Main.myPlayer)
                    {
                        this.AddBuff(20, 216000, true);
                        this.AddBuff(22, 216000, true);
                        this.AddBuff(23, 216000, true);
                        this.AddBuff(24, 216000, true);
                        this.AddBuff(30, 216000, true);
                        this.AddBuff(31, 216000, true);
                        this.AddBuff(32, 216000, true);
                        this.AddBuff(33, 216000, true);
                        this.AddBuff(35, 216000, true);
                        this.AddBuff(36, 216000, true);
                        this.AddBuff(68, 216000, true);
                    }
                }
            }
            if (this.whoAmI == Main.myPlayer)
            {
                if (this.itemTime == 0 && this.itemAnimation > 0 && item.type == 361 && Main.CanStartInvasion(1, true))
                {
                    this.itemTime = item.useTime;
                    Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0);
                    if (Main.netMode != 1)
                    {
                        if (Main.invasionType == 0)
                        {
                            Main.invasionDelay = 0;
                            Main.StartInvasion(1);
                        }
                    }
                    else
                    {
                        NetMessage.SendData(61, -1, -1, "", this.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                    }
                }
                if (this.itemTime == 0 && this.itemAnimation > 0 && item.type == 602 && Main.CanStartInvasion(2, true))
                {
                    this.itemTime = item.useTime;
                    Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0);
                    if (Main.netMode != 1)
                    {
                        if (Main.invasionType == 0)
                        {
                            Main.invasionDelay = 0;
                            Main.StartInvasion(2);
                        }
                    }
                    else
                    {
                        NetMessage.SendData(61, -1, -1, "", this.whoAmI, -2f, 0f, 0f, 0, 0, 0);
                    }
                }
                if (this.itemTime == 0 && this.itemAnimation > 0 && item.type == 1315 && Main.CanStartInvasion(3, true))
                {
                    this.itemTime = item.useTime;
                    Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0);
                    if (Main.netMode != 1)
                    {
                        if (Main.invasionType == 0)
                        {
                            Main.invasionDelay = 0;
                            Main.StartInvasion(3);
                        }
                    }
                    else
                    {
                        NetMessage.SendData(61, -1, -1, "", this.whoAmI, -3f, 0f, 0f, 0, 0, 0);
                    }
                }
                if (this.itemTime == 0 && this.itemAnimation > 0 && item.type == 1844 && !Main.dayTime && !Main.pumpkinMoon && !Main.snowMoon)
                {
                    this.itemTime = item.useTime;
                    Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0);
                    if (Main.netMode != 1)
                    {
                        Main.NewText(Lang.misc[31], 50, 255, 130, false);
                        Main.startPumpkinMoon();
                    }
                    else
                    {
                        NetMessage.SendData(61, -1, -1, "", this.whoAmI, -4f, 0f, 0f, 0, 0, 0);
                    }
                }
                if (this.itemTime == 0 && this.itemAnimation > 0 && item.type == 2767 && Main.dayTime && !Main.eclipse)
                {
                    Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0);
                    this.itemTime = item.useTime;
                    if (Main.netMode == 0)
                    {
                        Main.eclipse = true;
                        Main.NewText(Lang.misc[20], 50, 255, 130, false);
                    }
                    else
                    {
                        NetMessage.SendData(61, -1, -1, "", this.whoAmI, -6f, 0f, 0f, 0, 0, 0);
                    }
                }
                if (this.itemTime == 0 && this.itemAnimation > 0 && item.type == 3601 && NPC.downedGolemBoss && Main.hardMode && !NPC.AnyDanger() && !NPC.AnyoneNearCultists())
                {
                    Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0);
                    this.itemTime = item.useTime;
                    if (Main.netMode == 0)
                    {
                        WorldGen.StartImpendingDoom();
                    }
                    else
                    {
                        NetMessage.SendData(61, -1, -1, "", this.whoAmI, -8f, 0f, 0f, 0, 0, 0);
                    }
                }
                if (this.itemTime == 0 && this.itemAnimation > 0 && item.type == 1958 && !Main.dayTime && !Main.pumpkinMoon && !Main.snowMoon)
                {
                    this.itemTime = item.useTime;
                    Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0);
                    if (Main.netMode != 1)
                    {
                        Main.NewText(Lang.misc[34], 50, 255, 130, false);
                        Main.startSnowMoon();
                    }
                    else
                    {
                        NetMessage.SendData(61, -1, -1, "", this.whoAmI, -5f, 0f, 0f, 0, 0, 0);
                    }
                }
                if (this.itemTime == 0 && this.itemAnimation > 0 && item.makeNPC > 0 && this.controlUseItem && this.position.X / 16f - (float)Player.tileRangeX - (float)item.tileBoost <= (float)Player.tileTargetX && (this.position.X + (float)this.width) / 16f + (float)Player.tileRangeX + (float)item.tileBoost - 1f >= (float)Player.tileTargetX && this.position.Y / 16f - (float)Player.tileRangeY - (float)item.tileBoost <= (float)Player.tileTargetY && (this.position.Y + (float)this.height) / 16f + (float)Player.tileRangeY + (float)item.tileBoost - 2f >= (float)Player.tileTargetY)
                {
                    int num335 = Main.mouseX + (int)Main.screenPosition.X;
                    int num336 = Main.mouseY + (int)Main.screenPosition.Y;
                    this.itemTime = item.useTime;
                    int i2 = num335 / 16;
                    int j2 = num336 / 16;
                    if (!WorldGen.SolidTile(i2, j2))
                    {
                        NPC.ReleaseNPC(num335, num336, (int)item.makeNPC, item.placeStyle, this.whoAmI);
                    }
                }
                if (this.itemTime == 0 && this.itemAnimation > 0 && (item.type == 43 || item.type == 70 || item.type == 544 || item.type == 556 || item.type == 557 || item.type == 560 || item.type == 1133 || item.type == 1331) && this.SummonItemCheck())
                {
                    if (item.type == 560)
                    {
                        this.itemTime = item.useTime;
                        Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0);
                        if (Main.netMode != 1)
                        {
                            NPC.SpawnOnPlayer(i, 50);
                        }
                        else
                        {
                            NetMessage.SendData(61, -1, -1, "", this.whoAmI, 50f, 0f, 0f, 0, 0, 0);
                        }
                    }
                    else if (item.type == 43)
                    {
                        if (!Main.dayTime)
                        {
                            this.itemTime = item.useTime;
                            Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0);
                            if (Main.netMode != 1)
                            {
                                NPC.SpawnOnPlayer(i, 4);
                            }
                            else
                            {
                                NetMessage.SendData(61, -1, -1, "", this.whoAmI, 4f, 0f, 0f, 0, 0, 0);
                            }
                        }
                    }
                    else if (item.type == 70)
                    {
                        if (this.ZoneCorrupt)
                        {
                            this.itemTime = item.useTime;
                            Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0);
                            if (Main.netMode != 1)
                            {
                                NPC.SpawnOnPlayer(i, 13);
                            }
                            else
                            {
                                NetMessage.SendData(61, -1, -1, "", this.whoAmI, 13f, 0f, 0f, 0, 0, 0);
                            }
                        }
                    }
                    else if (item.type == 544)
                    {
                        if (!Main.dayTime)
                        {
                            this.itemTime = item.useTime;
                            Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0);
                            if (Main.netMode != 1)
                            {
                                NPC.SpawnOnPlayer(i, 125);
                                NPC.SpawnOnPlayer(i, 126);
                            }
                            else
                            {
                                NetMessage.SendData(61, -1, -1, "", this.whoAmI, 125f, 0f, 0f, 0, 0, 0);
                                NetMessage.SendData(61, -1, -1, "", this.whoAmI, 126f, 0f, 0f, 0, 0, 0);
                            }
                        }
                    }
                    else if (item.type == 556)
                    {
                        if (!Main.dayTime)
                        {
                            this.itemTime = item.useTime;
                            Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0);
                            if (Main.netMode != 1)
                            {
                                NPC.SpawnOnPlayer(i, 134);
                            }
                            else
                            {
                                NetMessage.SendData(61, -1, -1, "", this.whoAmI, 134f, 0f, 0f, 0, 0, 0);
                            }
                        }
                    }
                    else if (item.type == 557)
                    {
                        if (!Main.dayTime)
                        {
                            this.itemTime = item.useTime;
                            Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0);
                            if (Main.netMode != 1)
                            {
                                NPC.SpawnOnPlayer(i, 127);
                            }
                            else
                            {
                                NetMessage.SendData(61, -1, -1, "", this.whoAmI, 127f, 0f, 0f, 0, 0, 0);
                            }
                        }
                    }
                    else if (item.type == 1133)
                    {
                        this.itemTime = item.useTime;
                        Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0);
                        if (Main.netMode != 1)
                        {
                            NPC.SpawnOnPlayer(i, 222);
                        }
                        else
                        {
                            NetMessage.SendData(61, -1, -1, "", this.whoAmI, 222f, 0f, 0f, 0, 0, 0);
                        }
                    }
                    else if (item.type == 1331 && this.ZoneCrimson)
                    {
                        this.itemTime = item.useTime;
                        Main.PlaySound(15, (int)this.position.X, (int)this.position.Y, 0);
                        if (Main.netMode != 1)
                        {
                            NPC.SpawnOnPlayer(i, 266);
                        }
                        else
                        {
                            NetMessage.SendData(61, -1, -1, "", this.whoAmI, 266f, 0f, 0f, 0, 0, 0);
                        }
                    }
                }
            }
            if ((item.type == 50 || item.type == 3124 || item.type == 3199) && this.itemAnimation > 0)
            {
                if (Main.rand.Next(2) == 0)
                {
                    Dust.NewDust(this.position, this.width, this.height, 15, 0f, 0f, 150, default(Color), 1.1f);
                }
                if (this.itemTime == 0)
                {
                    this.itemTime = item.useTime;
                }
                else if (this.itemTime == item.useTime / 2)
                {
                    for (int num337 = 0; num337 < 70; num337++)
                    {
                        Dust.NewDust(this.position, this.width, this.height, 15, this.velocity.X * 0.5f, this.velocity.Y * 0.5f, 150, default(Color), 1.5f);
                    }
                    this.grappling[0] = -1;
                    this.grapCount = 0;
                    for (int num338 = 0; num338 < 1000; num338++)
                    {
                        if (Main.projectile[num338].active && Main.projectile[num338].owner == i && Main.projectile[num338].aiStyle == 7)
                        {
                            Main.projectile[num338].Kill();
                        }
                    }
                    this.Spawn();
                    for (int num339 = 0; num339 < 70; num339++)
                    {
                        Dust.NewDust(this.position, this.width, this.height, 15, 0f, 0f, 150, default(Color), 1.5f);
                    }
                }
            }
            if (item.type == 2350 && this.itemAnimation > 0)
            {
                if (this.itemTime == 0)
                {
                    this.itemTime = item.useTime;
                }
                else if (this.itemTime == 2)
                {
                    for (int num340 = 0; num340 < 70; num340++)
                    {
                        Main.dust[Dust.NewDust(this.position, this.width, this.height, 15, this.velocity.X * 0.2f, this.velocity.Y * 0.2f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
                    }
                    this.grappling[0] = -1;
                    this.grapCount = 0;
                    for (int num341 = 0; num341 < 1000; num341++)
                    {
                        if (Main.projectile[num341].active && Main.projectile[num341].owner == i && Main.projectile[num341].aiStyle == 7)
                        {
                            Main.projectile[num341].Kill();
                        }
                    }
                    bool flag28 = this.immune;
                    int num342 = this.immuneTime;
                    this.Spawn();
                    this.immune = flag28;
                    this.immuneTime = num342;
                    for (int num343 = 0; num343 < 70; num343++)
                    {
                        Main.dust[Dust.NewDust(this.position, this.width, this.height, 15, 0f, 0f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
                    }
                    if (item.stack > 0)
                    {
                        item.stack--;
                    }
                }
            }
            if (item.type == 2351 && this.itemAnimation > 0)
            {
                if (this.itemTime == 0)
                {
                    this.itemTime = item.useTime;
                }
                else if (this.itemTime == 2)
                {
                    if (Main.netMode == 0)
                    {
                        this.TeleportationPotion();
                    }
                    else if (Main.netMode == 1 && this.whoAmI == Main.myPlayer)
                    {
                        NetMessage.SendData(73, -1, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
                    }
                    if (item.stack > 0)
                    {
                        item.stack--;
                    }
                }
            }
            if (item.type == 2756 && this.itemAnimation > 0)
            {
                if (this.itemTime == 0)
                {
                    this.itemTime = item.useTime;
                }
                else if (this.itemTime == 2)
                {
                    if (this.whoAmI == Main.myPlayer)
                    {
                        this.Male = !this.Male;
                        if (Main.netMode == 1)
                        {
                            NetMessage.SendData(4, -1, -1, this.name, this.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                        }
                    }
                    if (item.stack > 0)
                    {
                        item.stack--;
                    }
                }
                else
                {
                    float num344 = (float)item.useTime;
                    num344 = (num344 - (float)this.itemTime) / num344;
                    float x6 = 15f;
                    float num345 = 44f;
                    float num346 = 9.424778f;
                    Vector2 vector22 = new Vector2(x6, 0f).RotatedBy((double)(num346 * num344), default(Vector2));
                    vector22.X *= (float)this.direction;
                    for (int num347 = 0; num347 < 2; num347++)
                    {
                        int type11 = 221;
                        if (num347 == 1)
                        {
                            vector22.X *= -1f;
                            type11 = 219;
                        }
                        Vector2 vector23 = new Vector2(vector22.X, num345 * (1f - num344) - num345 + (float)(this.height / 2));
                        vector23 += base.Center;
                        int num348 = Dust.NewDust(vector23, 0, 0, type11, 0f, 0f, 100, default(Color), 1f);
                        Main.dust[num348].position = vector23;
                        Main.dust[num348].noGravity = true;
                        Main.dust[num348].velocity = Vector2.Zero;
                        Main.dust[num348].scale = 1.3f;
                        Main.dust[num348].customData = this;
                    }
                }
            }
            if (i == Main.myPlayer)
            {
                if (this.itemTime == (int)((float)item.useTime * this.tileSpeed) && item.tileWand > 0)
                {
                    int tileWand2 = item.tileWand;
                    int num349 = 0;
                    while (num349 < 58)
                    {
                        if (tileWand2 == this.inventory[num349].type && this.inventory[num349].stack > 0)
                        {
                            this.inventory[num349].stack--;
                            if (this.inventory[num349].stack <= 0)
                            {
                                this.inventory[num349] = new Item();
                                break;
                            }
                            break;
                        }
                        else
                        {
                            num349++;
                        }
                    }
                }
                int num350;
                if (item.createTile >= 0)
                {
                    num350 = (int)((float)item.useTime * this.tileSpeed);
                }
                else if (item.createWall > 0)
                {
                    num350 = (int)((float)item.useTime * this.wallSpeed);
                }
                else
                {
                    num350 = item.useTime;
                }
                if (this.itemTime == num350 && item.consumable)
                {
                    bool flag29 = true;
                    if (item.type == 2350 || item.type == 2351)
                    {
                        flag29 = false;
                    }
                    if (item.type == 2756)
                    {
                        flag29 = false;
                    }
                    if (item.ranged)
                    {
                        if (this.ammoCost80 && Main.rand.Next(5) == 0)
                        {
                            flag29 = false;
                        }
                        if (this.ammoCost75 && Main.rand.Next(4) == 0)
                        {
                            flag29 = false;
                        }
                    }
                    if (item.thrown)
                    {
                        if (this.thrownCost50 && Main.rand.Next(100) < 50)
                        {
                            flag29 = false;
                        }
                        if (this.thrownCost33 && Main.rand.Next(100) < 33)
                        {
                            flag29 = false;
                        }
                    }
                    if (item.type >= 71 && item.type <= 74)
                    {
                        flag29 = true;
                    }
                    if (flag29 && ItemLoader.ConsumeItem(item, this))
                    {
                        if (item.stack > 0)
                        {
                            item.stack--;
                        }
                        if (item.stack <= 0)
                        {
                            this.itemTime = this.itemAnimation;
                            Main.blockMouse = true;
                        }
                    }
                }
                if (item.stack <= 0 && this.itemAnimation == 0)
                {
                    this.inventory[this.selectedItem] = new Item();
                }
                if (this.selectedItem == 58)
                {
                    if (this.itemAnimation == 0)
                    {
                        return;
                    }
                    Main.mouseItem = item.Clone();
                }
            }
            PlayerHooks.PostItemCheck(this);
        }
    }
}
