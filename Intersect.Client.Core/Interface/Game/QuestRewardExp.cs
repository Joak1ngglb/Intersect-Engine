using Intersect.Client.Framework.Content;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.General;
using Intersect.Client.Interface.Game;
using Intersect.Client.Localization;
using Intersect.Framework.Core.Config;

public class QuestRewardExp
{
    public ImagePanel Container;
    public ImagePanel ExpIcon;
    public Label ExpLabel;

    private QuestsWindow mQuestWindow;
    private QuestOfferWindow mQuestOfferWindow;
    private long mExpAmount;
    private bool mIsJobExp;
    private JobType? mJobType;

    // Constructor para QuestOfferWindow
    public QuestRewardExp(QuestOfferWindow questOfferWindow, long expAmount, bool isJobExp = false, JobType? jobType = null)
    {
        mQuestOfferWindow = questOfferWindow;
        mExpAmount = expAmount;
        mIsJobExp = isJobExp;
        mJobType = jobType;
    }

    // Constructor para QuestsWindow
    public QuestRewardExp(QuestsWindow questWindow, long expAmount, bool isJobExp = false, JobType? jobType = null)
    {
        mQuestWindow = questWindow;
        mExpAmount = expAmount;
        mIsJobExp = isJobExp;
        mJobType = jobType;
    }

    public void Setup()
    {
        // Determinar qué contenedor usar según la ventana actual
        Base parentContainer = mQuestWindow?.mRewardExpContainer ?? mQuestOfferWindow?.mRewardExpContainer;

        // Crear el contenedor principal de la recompensa de experiencia
        Container = new ImagePanel(parentContainer, "ExpRewardContainer");
        Container.SetSize(100, 24); // Definir tamaño estándar para alineación

        // Crear el icono de experiencia
        ExpIcon = new ImagePanel(Container, "ExpIcon");
        ExpIcon.SetSize(18, 18);
        ExpIcon.SetPosition(5, 3); // Ajuste fino para alineación correcta

        // Crear el label de experiencia
        ExpLabel = new Label(Container, "ExpLabel");
        ExpLabel.SetSize(70, 18);
        ExpLabel.SetPosition(28, 3); // Alineado a la derecha del icono

        Update(); // Cargar la información correctamente al inicio
    }

    public void Update(long newExpAmount = -1)
    {
        if (newExpAmount != -1)
        {
            mExpAmount = newExpAmount;
        }

        if (mIsJobExp && mJobType.HasValue)
        {
            var jobExpTexture = Globals.ContentManager.GetTexture(TextureType.Misc, "JobExpicon.png");
            if (jobExpTexture != null && ExpIcon.Texture != jobExpTexture)
            {
                ExpIcon.Texture = jobExpTexture;
            }

            var newText = $"{mExpAmount} ({mJobType})";
            if (ExpLabel.Text != newText)
            {
                ExpLabel.Text = newText;
            }
        }
        else
        {
            var expTexture = Globals.ContentManager.GetTexture(TextureType.Misc, "Expicon.png");
            if (expTexture != null && ExpIcon.Texture != expTexture)
            {
                ExpIcon.Texture = expTexture;
            }

            var newText = $"{mExpAmount}";
            if (ExpLabel.Text != newText)
            {
                ExpLabel.Text = newText;
            }
        }
    }
}
