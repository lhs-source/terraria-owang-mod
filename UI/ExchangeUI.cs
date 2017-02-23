using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ID;

namespace owang.UI
{
    class ExchangeUI : UIState
    {
        // 가장 밑바탕의 판넬
        private UIPanel basePanel;
        // UI에 띄울 제목
        private UIText titleText;
        // 다른 텍스트들
        private UIText srcValue;
        private UIText globalValue;
        private UIText destValue;

        // 닫기 버튼
        private UIImageButton closeBtn;
        // 아이템 놓는 곳의 판넬
        private UIItemPanel srcItemPanel;
        private UIItemPanel destProtoItemPanel;
        private UIItemPanel getItemPanel;
        // 뷰?
        private readonly UIElement _UIView;
        // 마우스 툴팁
        private string hoverString;

        // 패딩
        const float vpadding = 10;
        // ui 넓이
        const float vwidth = 600;
        // ui 높이
        const float vheight = 400;

        // 보임?
        internal bool visible = false;
        // 드래그중
        internal bool dragging = false;
        // 몰라
        private Vector2 offset;

        public ExchangeUI()
        {
            base.SetPadding(vpadding);
            base.Width.Set(vwidth, 0f);
            base.Height.Set(vheight, 0f);

            _UIView = new UIElement();
            _UIView.CopyStyle(this);
            _UIView.Left.Set(Main.screenWidth / 2f - _UIView.Width.Pixels / 2f, 0f);
            _UIView.Top.Set(Main.screenHeight / 2f - _UIView.Height.Pixels / 2f, 0f);
            base.Append(_UIView);
        }
        public override void OnInitialize()
        {
            // 가장 밑단 판넬 생성
            basePanel = new UIPanel();

            // 드래그 
            basePanel.OnMouseUp += (s, e) =>
            {
                _Recalculate(s.MousePosition);
                dragging = false;
            };
            basePanel.OnMouseDown += (s, e) =>
            {
                offset = new Vector2(s.MousePosition.X - _UIView.Left.Pixels, s.MousePosition.Y - _UIView.Top.Pixels);
                dragging = true;
            };
            basePanel.CopyStyle(this);
            basePanel.SetPadding(vpadding);
            // _UIView에다가 덧붙인다
            _UIView.Append(basePanel);

            // ui에 붙일 제목 만들어서 basePanel에 붙인다
            titleText = new UIText("Exchange!!", 0.85f, true);
            basePanel.Append(titleText);

            // 닫기 버튼 만들어서 basePanel에 붙이기
            closeBtn = new UIImageButton(owang.instance.GetTexture("UI/closeButton"));
            closeBtn.OnClick += (s, e) => { visible = false; };
            closeBtn.Width.Set(20f, 0f);
            closeBtn.Height.Set(20f, 0f);
            closeBtn.Left.Set(basePanel.Width.Pixels - closeBtn.Width.Pixels * 2f - vpadding * 4f, 0f);
            closeBtn.Top.Set(closeBtn.Height.Pixels / 2f, 0f);
            basePanel.Append(closeBtn);
            
            // 아이템 슬롯 판넬을 만든다
            srcItemPanel = new UIItemPanel();
            destProtoItemPanel = new UIItemPanel();
            getItemPanel = new UIItemPanel();
            //getItemPanel.displayOnly = true;

            // 클릭했을 때 이벤트
            srcItemPanel.OnClick += (s, e) =>
            {
                var panel = (e as UIItemPanel);
                // 인벤토리를 킨다.
                Main.playerInventory = true;
                // 판넬 위에도 아이템이 있고, 마우스에도 아이템이 있으면
                if (panel?.item.type != 0 && Main.mouseItem.type != 0)
                {
                    // 그리고 두 개가 다른 아이템이라면
                    if (panel?.item.type != Main.mouseItem.type)
                    {
                        // 마우스에 있던 아이템을 복사해놓고
                        var tempItem = Main.mouseItem.Clone();
                        // 판넬에 있던 아이템을 복사해놓고.
                        var tempItem2 = panel.item.Clone();
                        // 판넬에 마우스에 있던 거를
                        panel.item = tempItem;
                        // 마우스에 판넬에 있던 거를
                        Main.mouseItem = tempItem2;
                    }
                    // 같은 아이템이라면
                    else
                    {
                        // 최대 개수보다 넘어가면 그냥 리턴
                        if (panel.item.maxStack <= 1) return;
                        // 아니면 판넬의 아이템의 스택에 갯수 추가
                        panel.item.stack += Main.mouseItem.stack;
                        // 마우스에는 아이템 없음
                        Main.mouseItem.SetDefaults(0);
                    }
                }
                // 판넬에만 뭔가 있으면 
                else if (panel?.item.type != 0)
                {
                    // 판넬에 있던 거 마우스로 옮기고 판넬 아이템은 제거
                    Main.mouseItem = panel.item.Clone();
                    panel?.item.SetDefaults(0);
                }
                // 마우스만 있으면
                else if (Main.mouseItem != null)
                {
                    // ㅇㅇ
                    panel.item = Main.mouseItem.Clone();
                    Main.mouseItem.SetDefaults(0);
                    srcValue.SetText((srcItemPanel.item.value * srcItemPanel.item.stack).ToString());
                }
            };
            destProtoItemPanel.OnClick += (s, e) =>
            {
                var panel = (e as UIItemPanel);
                // 인벤토리를 킨다.
                Main.playerInventory = true;
                // 판넬 위에도 아이템이 있고, 마우스에도 아이템이 있으면
                if (panel?.item.type != 0 && Main.mouseItem.type != 0)
                {
                    // 그리고 두 개가 다른 아이템이라면
                    if (panel?.item.type != Main.mouseItem.type)
                    {
                        // 마우스에 있던 아이템을 복사해놓고
                        var tempItem = Main.mouseItem.Clone();
                        // 판넬에 있던 아이템을 복사해놓고.
                        var tempItem2 = panel.item.Clone();
                        // 판넬에 마우스에 있던 거를
                        panel.item = tempItem;
                        // 템꺼넬 판넬에도 똑같이 놓는다.
                        getItemPanel.item = tempItem;
                        getItemPanel.item.stack = 1;
                        // 마우스에 판넬에 있던 거를
                        Main.mouseItem = tempItem2;
                    }
                    // 같은 아이템이라면
                    else
                    {
                        // 최대 개수보다 넘어가면 그냥 리턴
                        if (panel.item.maxStack <= 1) return;
                        // 아니면 판넬의 아이템의 스택에 갯수 추가
                        panel.item.stack += Main.mouseItem.stack;
                        // 마우스에는 아이템 없음
                        Main.mouseItem.SetDefaults(0);
                    }
                }
                // 판넬에만 뭔가 있으면 
                else if (panel?.item.type != 0)
                {
                    // 판넬에 있던 거 마우스로 옮기고 판넬 아이템은 제거
                    Main.mouseItem = panel.item.Clone();
                    panel?.item.SetDefaults(0);
                    // 템꺼넬 판넬에도 똑같이 제거.
                    getItemPanel.item.SetDefaults(0);
                }
                // 마우스만 있으면
                else if (Main.mouseItem != null)
                {
                    // ㅇㅇ
                    panel.item = Main.mouseItem.Clone();
                    // 템꺼넬 판넬에도 똑같이 놓는다.
                    getItemPanel.item = Main.mouseItem.Clone();
                    getItemPanel.item.stack = 1;
                    Main.mouseItem.SetDefaults(0);
                    destValue.SetText((destProtoItemPanel.item.value).ToString());
                }
            };
            getItemPanel.OnClick += (s, e) => {
                var panel = (e as UIItemPanel);
                Main.playerInventory = true;
                if (panel?.item.type != 0 && Main.mouseItem.type != 0)
                {
                    if (panel?.item.type != Main.mouseItem.type)
                    {
                    }
                    else
                    {
                        if (panel.item.maxStack <= 1) return;
                        Main.mouseItem.stack++;
                    }
                }
                else if (panel?.item.type != 0)
                {
                    Main.mouseItem = panel.item.Clone();
                }
                else if (Main.mouseItem != null)
                {
                }
            };
            // 붙이기
            srcItemPanel.Top.Set(srcItemPanel.Height.Pixels + vpadding, 0f);
            basePanel.Append(srcItemPanel);
            destProtoItemPanel.Top.Set((destProtoItemPanel.Height.Pixels + vpadding), 0f);
            destProtoItemPanel.Left.Set(3 * (destProtoItemPanel.Width.Pixels + vpadding), 0f);
            basePanel.Append(destProtoItemPanel);
            getItemPanel.Top.Set(2 * (getItemPanel.Height.Pixels + vpadding), 0f);
            getItemPanel.Left.Set(3 * (getItemPanel.Width.Pixels + vpadding), 0f);
            basePanel.Append(getItemPanel);

            // 테스트
            srcValue = new UIText("0", 1f, false);
            //globalValue = new UIText(Main.LocalPlayer.GetModPlayer<owangPlayer>(owang.instance).matterValues.ToString(), 1f, false);\
            globalValue = new UIText("0", 1f, false);
            destValue = new UIText("0", 1f, false);
            srcValue.Top.Set(srcItemPanel.Top.Pixels + srcItemPanel.Height.Pixels + vpadding, 0f);
            srcValue.Left.Set(srcItemPanel.Left.Pixels, 0f);
            destValue.Top.Set(destProtoItemPanel.Top.Pixels + destProtoItemPanel.Height.Pixels + vpadding, 0f);
            destValue.Left.Set(destProtoItemPanel.Left.Pixels, 0f);
            globalValue.Top.Set(srcItemPanel.Top.Pixels - vpadding * 2, 0f);
            globalValue.Left.Set((srcItemPanel.Left.Pixels + destProtoItemPanel.Left.Pixels) / 2, 0f);
            basePanel.Append(srcValue);
            basePanel.Append(globalValue);
            basePanel.Append(destValue);

            //base.OnInitialize();
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 mousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (basePanel.ContainsPoint(mousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (dragging)
            {
                _Recalculate(mousePosition);
            }
        }
        public void ToggleUI(bool force = false)
        {
            if (!srcItemPanel.item.IsAir && (!visible || force))
            {
                Main.LocalPlayer.GetItem(Main.myPlayer, srcItemPanel.item.Clone()); // does not seem to generate item text
                srcItemPanel.item.SetDefaults(0);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // :s which bools do I need??
            if (Main.inputTextEscape || Main.LocalPlayer.dead || Main.gameMenu)
            {
                visible = false;
                ToggleUI(true);
            }
        }
        public void _Recalculate(Vector2 mousePos, float precent = 0f)
        {
            _UIView.Left.Set(Math.Max(0, Math.Min(mousePos.X - offset.X, Main.screenWidth - basePanel.Width.Pixels)), precent);
            _UIView.Top.Set(Math.Max(0, Math.Min(mousePos.Y - offset.Y, Main.screenHeight - basePanel.Height.Pixels)), precent);
            Recalculate();
        }
    }
    public struct ItemValue
    {
        public int RawValue { get; private set; }
        public int Copper { get; private set; }
        public int Silver { get; private set; }
        public int Gold { get; private set; }
        public int Platinum { get; private set; }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (Platinum > 0)
                stringBuilder.Append($"{Platinum}p");
            if (Gold > 0)
                stringBuilder.Append($"{Gold}g");
            if (Silver > 0)
                stringBuilder.Append($"{Silver}s");
            if (Copper > 0)
                stringBuilder.Append($"{Copper}c");

            if (stringBuilder.Length <= 0)
                return "No value";

            return string.Concat(stringBuilder.ToString().Select(c => $"{c}" + (char.IsLetter(c) ? " " : ""))).TrimEnd(' ');
        }

        public string ToTagString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (Platinum > 0)
                stringBuilder.Append($"[i/s1:{ItemID.PlatinumCoin}]{Platinum}");
            if (Gold > 0)
                stringBuilder.Append($"[i/s1:{ItemID.GoldCoin}]{Gold}");
            if (Silver > 0)
                stringBuilder.Append($"[i/s1:{ItemID.SilverCoin}]{Silver}");
            if (Copper > 0)
                stringBuilder.Append($"[i/s1:{ItemID.CopperCoin}]{Copper}");

            if (stringBuilder.Length <= 0)
                return "No value";

            return $"{stringBuilder}";
        }

        public ItemValue SetValues(int copper, int silver = 0, int gold = 0, int platinum = 0)
        {
            SetFromCopperValue(GetRawCopperValue(copper, silver, gold, platinum));
            return this;
        }

        public ItemValue AddValues(int copper, int silver = 0, int gold = 0, int platinum = 0)
        {
            SetFromCopperValue(RawValue + GetRawCopperValue(copper, silver, gold, platinum));
            return this;
        }

        private int GetRawCopperValue(int copper, int silver = 0, int gold = 0, int platinum = 0)
        {
            return (int)(copper + silver * 100 + gold * Math.Pow(100, 2) + platinum * Math.Pow(100, 3));
        }

        public ItemValue ToSellValue()
        {
            RawValue /= 5;
            SetFromCopperValue(RawValue);
            return this;
        }

        public ItemValue ApplyDiscount(Player player)
        {
            if (player.discount)
            {
                RawValue = (int)(RawValue * 0.8f);
                SetFromCopperValue(RawValue);
            }
            return this;
        }

        public ItemValue SetFromCopperValue(int value)
        {
            RawValue = value;
            int copper = value;
            int silver = 0;
            int gold = 0;
            int platinum = 0;

            if (copper >= 100)
            {
                silver = copper / 100;
                copper %= 100;

                if (silver >= 100)
                {
                    gold = silver / 100;
                    silver %= 100;

                    if (gold >= 100)
                    {
                        platinum = gold / 100;
                        gold %= 100;
                    }
                }
            }

            this.Copper = copper;
            this.Silver = silver;
            this.Gold = gold;
            this.Platinum = platinum;
            return this;
        }

        public static implicit operator ItemValue(int rawValue)
        {
            return new ItemValue().SetFromCopperValue(rawValue);
        }

        public static ItemValue operator +(ItemValue first, ItemValue second)
        {
            return new ItemValue().SetFromCopperValue(first.RawValue + second.RawValue);
        }

        public static ItemValue operator +(ItemValue first, float second)
        {
            return new ItemValue().SetFromCopperValue((int)(first.RawValue + second));
        }

        public static ItemValue operator -(ItemValue first, ItemValue second)
        {
            return new ItemValue().SetFromCopperValue(first.RawValue - second.RawValue);
        }

        public static ItemValue operator -(ItemValue first, float second)
        {
            return new ItemValue().SetFromCopperValue((int)(first.RawValue - second));
        }

        public static ItemValue operator *(ItemValue first, ItemValue second)
        {
            return new ItemValue().SetFromCopperValue(first.RawValue * second.RawValue);
        }

        public static ItemValue operator *(ItemValue first, float second)
        {
            return new ItemValue().SetFromCopperValue((int)(first.RawValue * second));
        }

        public static ItemValue operator /(ItemValue first, ItemValue second)
        {
            return new ItemValue().SetFromCopperValue(first.RawValue / second.RawValue);
        }

        public static ItemValue operator /(ItemValue first, float second)
        {
            return new ItemValue().SetFromCopperValue((int)(first.RawValue / second));
        }

        public static ItemValue operator %(ItemValue first, ItemValue second)
        {
            return new ItemValue().SetFromCopperValue(first.RawValue % second.RawValue);
        }

        public static ItemValue operator %(ItemValue first, float second)
        {
            return new ItemValue().SetFromCopperValue((int)(first.RawValue % second));
        }
    }
}
