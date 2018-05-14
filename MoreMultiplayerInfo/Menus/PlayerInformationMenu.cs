using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoreMultiplayerInfo.Helpers;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;

namespace MoreMultiplayerInfo
{
    public class PlayerInformationMenu : IClickableMenu
    {
        public long PlayerId;

        private readonly IMonitor _monitor;

        private readonly IModHelper _helper;

        private Farmer Player => PlayerHelpers.GetPlayerWithUniqueId(PlayerId);

        private InventoryMenu _inventory;

        private PlayerSkillInfo _skillInfo;
        private PlayerEquipmentInfo _equipmentInfo;

        private static int Width => 850;
        private static int Height => 580;

        private static int Xposition => (Game1.viewport.Width / 2) - (Width / 2);
        private static int Yposition => (Game1.viewport.Height / 2) - (Height / 2);

        private static Item HoveredItem { get; set; }

        private static string HoverText { get; set; }

        private static int GenericHeightSpacing => 25;

        public PlayerInformationMenu(long playerUniqueMultiplayerId, IMonitor monitor, IModHelper helper) : base(Xposition, Yposition, Width, Height, true)
        {
            PlayerId = playerUniqueMultiplayerId;
            _monitor = monitor;
            _helper = helper;

            GraphicsEvents.Resize += Resize;

        }

        private void Resize(object sender, EventArgs e)
        {
            this.xPositionOnScreen = Xposition;
            this.yPositionOnScreen = Yposition;
        }
        
        public override void draw(SpriteBatch b)
        {
            Game1.mouseCursor = 0;

            DrawBackground(b);

            DrawTitle(b);

            DrawInventory(b);

            DrawLocationInfo(b);

            DrawSkills(b);

            /* DrawEquipment(b); */

            /* DrawHealth(b); */

            DrawHoverText(b);

            drawMouse(b);
        }

        private void DrawHealth(SpriteBatch b)
        {
            var healthBar = new PlayerHealthInfo(Player.health, Player.maxHealth, (int) Player.Stamina, Player.maxStamina, new Rectangle(_skillInfo.xPositionOnScreen + _skillInfo.Width, _skillInfo.yPositionOnScreen, 45, 11));
            healthBar.draw(b);
        }

        private void DrawSkills(SpriteBatch b)
        {
            _skillInfo = new PlayerSkillInfo(Player, new Vector2(_inventory.xPositionOnScreen, _inventory.yPositionOnScreen + _inventory.height + GenericHeightSpacing));
            _skillInfo.draw(b);
        }

        private void DrawLocationInfo(SpriteBatch b)
        {
            var yPos = _inventory.yPositionOnScreen + _inventory.height + GenericHeightSpacing;

            var text = $"Location: {Player.currentLocation.Name}";

            var font = Game1.smallFont;

            var textWidth = font.MeasureString(text).X;

            var xPos = _inventory.xPositionOnScreen + (_inventory.width * 3 / 4) - (textWidth / 2);

            b.DrawString(font, text, new Vector2(xPos, yPos), Color.Black);

        }

        private void DrawTitle(SpriteBatch b)
        {
            var text = $"{Player.Name}'s info";

            var font = Game1.dialogueFont;

            var titleWidth = font.MeasureString(text).X;

            var xPos = xPositionOnScreen + (Width / 2) - (titleWidth /2);

            b.DrawString(font, text, new Vector2(xPos, this.yPositionOnScreen + 25), Color.Black);
        }

        private void DrawBackground(SpriteBatch b)
        {
            IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, Color.White, Game1.pixelZoom);

            this.upperRightCloseButton.draw(b);
        }

        private void DrawHoverText(SpriteBatch b)
        {
            if (!string.IsNullOrEmpty(HoverText))
            {
                IClickableMenu.drawToolTip(b, HoverText, HoverText, HoveredItem);
            }
        }
        

        private void DrawInventory(SpriteBatch b)
        {
            _inventory = new InventoryMenu(this.xPositionOnScreen + 25, this.yPositionOnScreen + 100, false, Player.Items)
            {
                showGrayedOutSlots = true
            };

            _inventory.draw(b);
        }

        public override void performHoverAction(int x, int y)
        {
            Game1.mouseCursor = 0;

            HoveredItem = null;
            HoverText = string.Empty;

            if (_inventory.isWithinBounds(x, y))
            {
                SetHoverTextFromInventory(x, y);
            }

            base.performHoverAction(x, y);
        }

        private void SetHoverTextFromInventory(int x, int y)
        {
            foreach (ClickableComponent c in _inventory.inventory)
            {
                if (c.containsPoint(x, y))
                {
                    var item = _inventory.getItemFromClickableComponent(c);
                    if (item != null)
                    {
                        HoverText = $"{item.Name} x {item.Stack}";

                        HoveredItem = item;
                    }
                }
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (this.upperRightCloseButton.containsPoint(x, y))
            {
                UnloadMenu(playSound);
            }

            base.receiveLeftClick(x, y, playSound);
        }

        private void UnloadMenu(bool playSound)
        {
            this.exitThisMenu(playSound);

            Game1.onScreenMenus.Remove(this);
            GraphicsEvents.Resize -= Resize;
        }
    }
}