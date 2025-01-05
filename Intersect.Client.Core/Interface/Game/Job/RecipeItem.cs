using System;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Input;
using Intersect.Client.Framework.Input;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.DescriptionWindows;
using Intersect.Client.Networking;
using Intersect.GameObjects;
using Intersect.GameObjects.Crafting;

namespace Intersect.Client.Interface.Game.Job
{
    public partial class RecipeItem
    {
        public ImagePanel Container;

        public ItemDescriptionWindow DescWindow;

        public bool IsDragging;

        // Dragging
        private bool mCanDrag;

        // References
        private JobsWindow mJobsWindow;

        private Draggable mDragIcon;

        // Slot info
        CraftIngredient mIngredient;

        // Mouse Event Variables
        private bool mMouseOver;

        private int mMouseX = -1;

        private int mMouseY = -1;

        public ImagePanel Pnl;


        public RecipeItem(JobsWindow skillsWindow, CraftIngredient ingredient)
        {
            mJobsWindow = skillsWindow;
            mIngredient = ingredient;
        }

        public void Setup(string name)
        {
            Pnl = new ImagePanel(Container, name);
            if (Pnl == null)
            {
                PacketSender.SendChatMsg($"Error: Pnl not created for {name}", 5);
            }
            else
            {
                PacketSender.SendChatMsg($"Pnl created for {name}", 5);
            }
            Pnl.HoverEnter += pnl_HoverEnter;
            Pnl.HoverLeave += pnl_HoverLeave;
        }

        public void LoadItem()
        {
            var item = ItemBase.Get(mIngredient.ItemId);

            if (item != null)
            {
                var itemTex = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, item.Icon);
                if (itemTex != null)
                {
                    Pnl.Texture = itemTex;
                    Pnl.RenderColor = item.Color;
                    PacketSender.SendChatMsg($"Texture loaded for item: {item.Name}, Icon: {item.Icon}", 5);
                }
                else
                {
                    PacketSender.SendChatMsg($"Texture missing for item: {item.Name}, Icon: {item.Icon}", 5);
                    Pnl.Texture = null;
                }
            }
            else
            {
                PacketSender.SendChatMsg($"Item not found with ID: {mIngredient.ItemId}", 5);
                Pnl.Texture = null;
            }
        }


        void pnl_HoverLeave(Base sender, EventArgs arguments)
        {
            mMouseOver = false;
            mMouseX = -1;
            mMouseY = -1;
            if (DescWindow != null)
            {
                DescWindow.Dispose();
                DescWindow = null;
            }
        }

        void pnl_HoverEnter(Base sender, EventArgs arguments)
        {
            if (InputHandler.MouseFocus != null)
            {
                return;
            }

            mMouseOver = true;
            mCanDrag = true;
            if (Globals.InputManager.MouseButtonDown(MouseButtons.Left))
            {
                mCanDrag = false;

                return;
            }

            if (DescWindow != null)
            {
                DescWindow.Dispose();
                DescWindow = null;
            }

            if (mIngredient != null && ItemBase.TryGet(mIngredient.ItemId, out var itemDescriptor))
            {
                DescWindow = new ItemDescriptionWindow(
                    itemDescriptor, mIngredient.Quantity, mJobsWindow.X, mJobsWindow.Y, null
                );
            }
        }
    }
}
