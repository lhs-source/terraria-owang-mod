using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hsmod.Items;
using ReLogic.Graphics;

namespace Hsmod.UI
{
    class UIItemPanel : UIPanel
    {
        internal const float panelwidth = 50f;
        internal const float panelheight = 50f;
        internal const float panelpadding = 0f;
        private bool rightClicking = false;
        public Item item;
        public bool displayOnly;

        public UIItemPanel(int type = 0, int stack = 0)
        {
            base.Width.Set(panelwidth, 0f);
            base.Height.Set(panelheight, 0f);
            base.SetPadding(panelpadding);
            item = new Item();
            item.SetDefaults(type);
            item.stack = stack;
        }

        public override void Update(GameTime gameTime)
        {
            if (displayOnly) return;

            // Is right clicking?
            rightClicking = Main.mouseRight && base.IsMouseHovering;

            // If right clicking, and is a good item
            if (rightClicking && item.type != 0)
            {
                // Open inventory
                Main.playerInventory = true;

                // Handle stack splitting here
                if (Main.stackSplit <= 1 && item.type != 0 && (Main.mouseItem.IsTheSameAs(item) || Main.mouseItem.type == 0))
                {
                    int num2 = Main.superFastStack + 1;
                    for (int j = 0; j < num2; j++)
                    {
                        if ((Main.mouseItem.stack < Main.mouseItem.maxStack || Main.mouseItem.type == 0) && item.stack > 0)
                        {
                            if (j == 0)
                            {
                                Main.PlaySound(18, -1, -1, 1);
                            }
                            if (Main.mouseItem.type == 0)
                            {
                                Main.mouseItem.netDefaults(item.netID);
                                if (item.prefix != 0)
                                {
                                    Main.mouseItem.Prefix((int)item.prefix);
                                }
                                Main.mouseItem.stack = 0;
                            }
                            Main.mouseItem.stack++;
                            item.stack--;
                            if (Main.stackSplit == 0)
                            {
                                Main.stackSplit = 15;
                            }
                            else
                            {
                                Main.stackSplit = Main.stackDelay;
                            }

                            if (item.stack <= 0)
                            {
                                item.SetDefaults(0);
                            }
                        }
                    }
                }
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (item == null || item.type == 0) return;

            if (base.IsMouseHovering)
            {
                Main.hoverItemName = item.Name;
                //Main.toolTip = item.Clone();
                //Main.toolTip.GetModInfo<HsmodItemInfo>(Hsmod.instance).addValueTooltip = true;
                //ItemValue value = new ItemValue().SetFromCopperValue(item.value*item.stack);
                //Main.toolTip.name = $"{Main.toolTip.name}{Main.toolTip.modItem?.mod.Name.Insert((int)Main.toolTip.modItem?.mod.Name.Length, "]").Insert(0, " [")}";
            }

            CalculatedStyle innerDimensions = base.GetInnerDimensions();
            Texture2D texture2D = Main.itemTexture[this.item.type];
            Rectangle frame;
            if (Main.itemAnimations[item.type] != null)
            {
                frame = Main.itemAnimations[item.type].GetFrame(texture2D);
            }
            else
            {
                frame = texture2D.Frame(1, 1, 0, 0);
            }
            float drawScale = 1f;
            float num2 = (float)innerDimensions.Width * 1f;
            if ((float)frame.Width > num2 || (float)frame.Height > num2)
            {
                if (frame.Width > frame.Height)
                {
                    drawScale = num2 / (float)frame.Width;
                }
                else
                {
                    drawScale = num2 / (float)frame.Height;
                }
            }
            Vector2 drawPosition = new Vector2(innerDimensions.X, innerDimensions.Y);
            drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
            drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

            this.item.GetColor(Color.White);
            spriteBatch.Draw(texture2D, drawPosition, new Rectangle?(frame), this.item.GetAlpha(Color.White), 0f,
                Vector2.Zero, drawScale, SpriteEffects.None, 0f);
            if (this.item.color != default(Color))
            {
                spriteBatch.Draw(texture2D, drawPosition, new Rectangle?(frame), this.item.GetColor(Color.White), 0f,
                    Vector2.Zero, drawScale, SpriteEffects.None, 0f);
            }

            // Draw stack count
            if (this.item.stack > 1)
            {
                spriteBatch.DrawString(Main.fontItemStack, Math.Min(9999, item.stack).ToString(), new Vector2(innerDimensions.Position().X + 10f * drawScale, innerDimensions.Position().Y + 26f * drawScale), Color.White, 0f, Vector2.Zero, drawScale, SpriteEffects.None, 0f);
               
            }
        }
    }
}
