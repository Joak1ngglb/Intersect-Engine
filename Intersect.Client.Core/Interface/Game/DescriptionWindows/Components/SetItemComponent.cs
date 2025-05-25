using Newtonsoft.Json.Linq;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen;
using Intersect.Client.General;
using Intersect.GameObjects;
using Intersect.Client.Framework.Graphics;

namespace Intersect.Client.Interface.Game.DescriptionWindows.Components;

/// <summary>
/// Componente visual que representa una fila de íconos de ítems que componen un set, con un ✔ o ✗.
/// </summary>
public class SetItemComponent : ComponentBase
{
    private JObject mIconLayout;
    private int mOffsetX = 0;

    public SetItemComponent(Base parent, string name = "SetItemComponent") : base(parent, name)
    {
        GenerateComponents();
        LoadLayout();
        GetTemplate();
        DestroyTemplateInstance();
    }

    protected override void GenerateComponents()
    {
        base.GenerateComponents();
        mContainer.SetSize(0, 40); // altura estándar
    }

    private void GetTemplate()
    {
        var icon = new ImagePanel(mContainer);
        icon.SetSize(32, 32);
        mIconLayout = icon.GetJson();
        icon.Dispose();
    }

    private void DestroyTemplateInstance()
    {
        // Nada que destruir
    }

    /// <summary>
    /// Añade un ícono al contenedor del set, con un ✔ o una ✗ encima.
    /// </summary>
    private int mMaxPerRow = 5; // Puedes parametrizar esto
    private int mCurrentIndex = 0;

    public void AddItem(ItemBase item, bool equipped)
    {
        var iconContainer = new ImagePanel(mContainer);
        iconContainer.SetSize(32, 32);

        // Item icon
        var icon = new ImagePanel(iconContainer);
        icon.SetSize(32, 32);
        icon.Texture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Item, item.Icon);
        icon.RenderColor = Color.White;

        // Overlay: check or x
        var overlayIcon = new ImagePanel(iconContainer);
        overlayIcon.SetSize(16, 16);
        overlayIcon.SetPosition(18, 0);
        var overlayName = equipped ? "set_check.png" : "set_x.png";
        overlayIcon.Texture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Misc, overlayName);

        // Calcula posición en grilla
        int col = mCurrentIndex % mMaxPerRow;
        int row = mCurrentIndex / mMaxPerRow;

        int xPos = col * (iconContainer.Width + 4);
        int yPos = row * (iconContainer.Height + 4);

        iconContainer.SetPosition(xPos, yPos);
        mContainer.AddChild(iconContainer);

        mCurrentIndex++;
        SizeToChildren(true, true);
    }


    public JObject GetJson() => mContainer.GetJson();
    public void LoadJson(JObject json) => mContainer.LoadJson(json);
}
