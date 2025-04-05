using System;
using System.Collections.Generic;
using System.IO;
using Intersect.Client.Core;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.GenericClasses;
using Intersect.Client.Framework.Graphics;
using Intersect.Client.Framework.Gwen;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Utilities;
using Graphics = Intersect.Client.Core.Graphics;

namespace Intersect.Client.Interface.Game.Guilds
{
    public class GuildCreationInterface
    {
        private WindowControl mCreateGuildWindow;
        private TextBox mGuildNameTextbox;

        // Panel principal para elegir símbolo/fondo
        private ImagePanel mLogoPanel;
        private Button mSymbolButton;
        private Button mBackgroundButton;
        private ScrollControl mSymbolPanel;    // Lista de símbolos
        private ScrollControl mBackgroundPanel; // Lista de fondos
     private ColorPicker mBgColorPicker;
        private Button mCreateGuildButton;
        // Panel de previsualización
        private ImagePanel mLogoCompositionPanel;
        private ImagePanel mBackgroundPreview;
        private ImagePanel mSymbolPreview;

        // Array de texturas: [0] => fondo, [1] => símbolo
        private List<GameTexture> mLogoElements;
        private List<GameTexture> mOriginalLogoElements;

        private bool mInitializedSymbols = false;
        private bool mInitializedBackgrounds = false;

        // =============================
        //  Sección: Sliders de color para el FONDO
        // =============================
        private Label mBackgroundColorLabel;
        private HorizontalSlider mBgRedSlider, mBgGreenSlider, mBgBlueSlider;
        private TextBoxNumeric mBgRedText, mBgGreenText, mBgBlueText;

        private byte bgR = 255, bgG = 255, bgB = 255; // Color actual del fondo (por defecto blanco)

        // =============================
        //  Sección: Sliders de color para el SÍMBOLO
        // =============================
        private Label mSymbolColorLabel;
        private HorizontalSlider mSymRedSlider, mSymGreenSlider, mSymBlueSlider;
        private TextBoxNumeric mSymRedText, mSymGreenText, mSymBlueText;

        private byte symR = 0, symG = 0, symB = 0; // Color actual del símbolo (por defecto blanco)

        // =============================
        //  Sliders para mover (solo Y) y escalar el símbolo
        // =============================
        private Label mTransformLabel;
        private HorizontalSlider mSymbolPosYSlider;
        private HorizontalSlider mSymbolScaleSlider;

        private int symbolPosY = 0;      // Offset en vertical
        private float symbolScale = 1f;  // 1.0 = tamaño normal
        private string selectedBackgroundFile = string.Empty;
        private string selectedSymbolFile = string.Empty;

        // Tamaño del panel donde se dibuja el logo
        private const int COMPOSITION_SIZE = 100;

        public GuildCreationInterface(Canvas gameCanvas)
        {
            // 1) Ventana principal
            mCreateGuildWindow = new WindowControl(gameCanvas, Strings.Inventory.Title, false, "GuildCreationWindow");
            mCreateGuildWindow.DisableResizing();
            mCreateGuildWindow.SetSize(850, 400);

            // 2) Campo de texto: Nombre del gremio
            mGuildNameTextbox = new TextBox(mCreateGuildWindow, "GuildNameTextbox");
            mGuildNameTextbox.SetBounds(20, 10, 810, 30);

            // 3) Panel principal (arriba) con botones “Símbolo” y “Fondo”
            mLogoPanel = new ImagePanel(mCreateGuildWindow, "LogoPanel");
            mLogoPanel.SetBounds(20, 50, 810, 230);

            mSymbolButton = new Button(mLogoPanel, "SymbolButton");
            mSymbolButton.Text = "Símbolos";
            mSymbolButton.SetBounds(0, 0, 405, 40);

            mBackgroundButton = new Button(mLogoPanel, "BackgroundButton");
            mBackgroundButton.Text = "Fondos";
            mBackgroundButton.SetBounds(405, 0, 405, 40);

            // Panel de símbolos
            mSymbolPanel = new ScrollControl(mLogoPanel, "SymbolPanel");
            mSymbolPanel.SetBounds(0, 40, 810, 190);
            mSymbolPanel.EnableScroll(false, true);
            // Panel de fondos
            mBackgroundPanel = new ScrollControl(mLogoPanel, "BackgroundPanel");
            mBackgroundPanel.SetBounds(0, 40, 810, 190);
            mBackgroundPanel.EnableScroll(false, true);

            mSymbolButton.Clicked += (s, e) =>
            {
                mSymbolPanel.Show();
                mBackgroundPanel.Hide();
            };
            mBackgroundButton.Clicked += (s, e) =>
            {
                mSymbolPanel.Hide();
                mBackgroundPanel.Show();
            };

            // 4) Panel de previsualización (fondo + símbolo)
            mLogoCompositionPanel = new ImagePanel(mCreateGuildWindow, "LogoCompositionPanel");
            mLogoCompositionPanel.SetBounds(20, 290, COMPOSITION_SIZE + 5, COMPOSITION_SIZE + 5);

            mBackgroundPreview = new ImagePanel(mLogoCompositionPanel, "BackgroundPreview");
            mBackgroundPreview.SetBounds(0, 0, COMPOSITION_SIZE, COMPOSITION_SIZE);
            mBackgroundPreview.Show();

            mSymbolPreview = new ImagePanel(mLogoCompositionPanel, "SymbolPreview");
            mSymbolPreview.SetBounds(0, 0, 56, 56);
            mSymbolPreview.Show();
            mCreateGuildButton = new Button(mCreateGuildWindow, "CreateGuildButton");
            mCreateGuildButton.Text = "Crear Gremio";
            // Ajusta su posición y tamaño donde mejor quede en tu UI
            mCreateGuildButton.SetBounds(650, 620, 150, 40);
            mCreateGuildButton.Clicked += OnCreateGuildButtonClicked;


            // 5) Estructuras para las texturas
            mLogoElements = new List<GameTexture> { null, null };
            mOriginalLogoElements = new List<GameTexture> { null, null };

            // Inicializamos los paneles
            InitializeSymbolPanel();
            InitializeBackgroundPanel();
            // Por defecto, mostramos la lista de símbolos
            mSymbolPanel.Show();
            mBackgroundPanel.Hide();

            // 6) Sliders de color
            InitializeBackgroundColorSliders();
            InitializeSymbolColorSliders();

            // 7) Sliders para mover en Y + escalar el símbolo
            InitializeSymbolTransformSliders();

            // Cargamos la UI
            mCreateGuildWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
        }

        private void OnCreateGuildButtonClicked(Base sender, ClickedEventArgs arguments)
        {
            // 1) Tomar el nombre del gremio
            string guildName = mGuildNameTextbox.Text.Trim();
            if (string.IsNullOrEmpty(guildName))
            {
                PacketSender.SendChatMsg("El nombre del gremio está vacío.", 5);
                return;
            }

            // 2) Verificar que tenemos archivos para fondo / símbolo
            if (string.IsNullOrEmpty(selectedBackgroundFile))
            {
                PacketSender.SendChatMsg("No se ha seleccionado un fondo.", 5);
                return;
            }
            if (string.IsNullOrEmpty(selectedSymbolFile))
            {
                PacketSender.SendChatMsg("No se ha seleccionado un símbolo.", 5);
                return;
            }

            // 3) Llamar a PacketSender.SendCreateGuild con todos los parámetros
            PacketSender.SendCreateGuild(
                guildName,
                selectedBackgroundFile,
                bgR, bgG, bgB,
                selectedSymbolFile,
                symR, symG, symB,
                symbolPosY,
                symbolScale
            );

            // Opcional: Cerrar la ventana, o dejarla abierta
            mCreateGuildWindow.Close();
        }

        // =====================================================
        //  Inicializar la lista de símbolos
        // =====================================================
        private void InitializeSymbolPanel()
        {
            string symbolFolderPath = "resources/Guild/Symbols";
            if (!Directory.Exists(symbolFolderPath))
                Directory.CreateDirectory(symbolFolderPath);

            var files = Directory.GetFiles(symbolFolderPath, "*.png");
            
            mSymbolPanel.Children.Clear();

            int containerSize = 48;
            int spacing = 5;
            int xPadding = 5, yPadding = 5;
            int columns = mSymbolPanel.Width / (containerSize + spacing + xPadding);
            if (columns < 1) columns = 1;

            int index = 0;
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);

                var container = new ImagePanel(mSymbolPanel, "SymbolContainer");
                container.SetSize(containerSize, containerSize);

                int posX = (index % columns) * (containerSize + spacing);
                int posY = (index / columns) * (containerSize + spacing);
                container.SetPosition(posX + xPadding, posY + yPadding);
                container.Show();

                var symbolImg = new ImagePanel(container, "SymbolImage");
                symbolImg.Texture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Guild, fileName);

                if (symbolImg.Texture != null)
                {
                    var (scaledW, scaledH) = ScaleToFit(symbolImg.Texture.Width, symbolImg.Texture.Height, containerSize, containerSize);
                    symbolImg.SetSize(scaledW, scaledH);
                    Align.Center(symbolImg);
                }
                else
                {
                    symbolImg.SetSize(containerSize, containerSize);
                }

                symbolImg.Clicked += (s, e) =>
                {
                    // Al hacer clic, seleccionamos esa textura como símbolo
                    mOriginalLogoElements[1] = symbolImg.Texture;
                    mLogoElements[1] = symbolImg.Texture;

                    selectedSymbolFile = fileName; // Guardar el nombre

                    UpdateLogoPreview();
                };


                index++;
            }
        }

        // =====================================================
        //  Inicializar la lista de fondos
        // =====================================================
        private void InitializeBackgroundPanel()
        {
            string backgroundFolderPath = "resources/Guild/Background";
            if (!Directory.Exists(backgroundFolderPath))
                Directory.CreateDirectory(backgroundFolderPath);

            var files = Directory.GetFiles(backgroundFolderPath, "*.png");
            mBackgroundPanel.Children.Clear();

            int containerSize = 48;
            int spacing = 5;
            int xPadding = 5, yPadding = 5;
            int columns = mBackgroundPanel.Width / (containerSize + spacing + xPadding);
            if (columns < 1) columns = 1;

            int index = 0;
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);

                var container = new ImagePanel(mBackgroundPanel, "BackgroundContainer");
                container.SetSize(containerSize, containerSize);

                int posX = (index % columns) * (containerSize + spacing);
                int posY = (index / columns) * (containerSize + spacing);
                container.SetPosition(posX + xPadding, posY + yPadding);
                container.Show();

                var bgImg = new ImagePanel(container, "BackgroundImage");
                bgImg.Texture = Globals.ContentManager.GetTexture(Framework.Content.TextureType.Guild, fileName);

                if (bgImg.Texture != null)
                {
                    var (scaledW, scaledH) = ScaleToFit(bgImg.Texture.Width, bgImg.Texture.Height, containerSize, containerSize);
                    bgImg.SetSize(scaledW, scaledH);
                    Align.Center(bgImg);
                }
                else
                {
                    bgImg.SetSize(containerSize, containerSize);
                }

                bgImg.Clicked += (s, e) =>
                {
                    // Al hacer clic, seleccionamos esa textura como fondo
                    mOriginalLogoElements[0] = bgImg.Texture;
                    mLogoElements[0] = bgImg.Texture;

                    selectedBackgroundFile = fileName; // Guardar el nombre

                    UpdateLogoPreview();
                };

                index++;
            }
        }

        // =====================================================
        //  Sliders de color para el FONDO (3 sliders)
        // =====================================================
        private void InitializeBackgroundColorSliders()
        {
            // Título "Color de Fondo"
            mBackgroundColorLabel = new Label(mCreateGuildWindow, "BackgroundColorLabel");
            mBackgroundColorLabel.Text = "Color de Fondo:";
            mBackgroundColorLabel.SetBounds(220, 290, 200, 30);

            // Slider R
            mBgRedSlider = new HorizontalSlider(mCreateGuildWindow, "BgRedSlider");
            mBgRedSlider.SetBounds(220, 320, 150, 20);
            mBgRedSlider.SetRange(0, 255);
            mBgRedSlider.Value = bgR;
            mBgRedSlider.ValueChanged += OnBgColorSliderChanged;

            // Text R
            mBgRedText = new TextBoxNumeric(mCreateGuildWindow, "BgRedText");
            mBgRedText.SetBounds(380, 320, 50, 20);
            mBgRedText.SetText(bgR.ToString());
            mBgRedText.SetMaxLength(255);
            mBgRedText.SubmitPressed += OnBgColorTextChanged;

            // Slider G
            mBgGreenSlider = new HorizontalSlider(mCreateGuildWindow, "BgGreenSlider");
            mBgGreenSlider.SetBounds(220, 350, 150, 20);
            mBgGreenSlider.SetRange(0, 255);
            mBgGreenSlider.Value = bgG;
            mBgGreenSlider.ValueChanged += OnBgColorSliderChanged;

            // Text G
            mBgGreenText = new TextBoxNumeric(mCreateGuildWindow, "BgGreenText");
            mBgGreenText.SetBounds(380, 350, 50, 20);
            mBgGreenText.SetText(bgG.ToString());
            mBgGreenText.SetMaxLength(255);
            mBgGreenText.SubmitPressed += OnBgColorTextChanged;

            // Slider B
            mBgBlueSlider = new HorizontalSlider(mCreateGuildWindow, "BgBlueSlider");
            mBgBlueSlider.SetBounds(220, 380, 150, 20);
            mBgBlueSlider.SetRange(0, 255);
            mBgBlueSlider.Value = bgB;
            mBgBlueSlider.ValueChanged += OnBgColorSliderChanged;

            // Text B
            mBgBlueText = new TextBoxNumeric(mCreateGuildWindow, "BgBlueText");
            mBgBlueText.SetBounds(380, 380, 50, 20);
            mBgBlueText.SetText(bgB.ToString());
            mBgBlueText.SetMaxLength(255);
            mBgBlueText.SubmitPressed += OnBgColorTextChanged;
        }

        // =====================================================
        //  Sliders de color para el SÍMBOLO (3 sliders)
        // =====================================================
        private void InitializeSymbolColorSliders()
        {
            // Título "Color de Símbolo"
            mSymbolColorLabel = new Label(mCreateGuildWindow, "SymbolColorLabel");
            mSymbolColorLabel.Text = "Color del Símbolo:";
            mSymbolColorLabel.SetBounds(220, 410, 200, 30);

            // Slider R
            mSymRedSlider = new HorizontalSlider(mCreateGuildWindow, "SymRedSlider");
            mSymRedSlider.SetBounds(220, 440, 150, 20);
            mSymRedSlider.SetRange(0, 255);
            mSymRedSlider.Value = symR;
            mSymRedSlider.ValueChanged += OnSymColorSliderChanged;

            // Text R
            mSymRedText = new TextBoxNumeric(mCreateGuildWindow, "SymRedText");
            mSymRedText.SetBounds(380, 440, 50, 20);
            mSymRedText.SetText(symR.ToString());
            mSymRedText.SetMaxLength(255);
            mSymRedText.SubmitPressed += OnSymColorTextChanged;

            // Slider G
            mSymGreenSlider = new HorizontalSlider(mCreateGuildWindow, "SymGreenSlider");
            mSymGreenSlider.SetBounds(220, 470, 150, 20);
            mSymGreenSlider.SetRange(0, 255);
            mSymGreenSlider.Value = symG;
            mSymGreenSlider.ValueChanged += OnSymColorSliderChanged;

            // Text G
            mSymGreenText = new TextBoxNumeric(mCreateGuildWindow, "SymGreenText");
            mSymGreenText.SetBounds(380, 470, 50, 20);
            mSymGreenText.SetText(symG.ToString());
            mSymGreenText.SetMaxLength(255);
            mSymGreenText.SubmitPressed += OnSymColorTextChanged;

            // Slider B
            mSymBlueSlider = new HorizontalSlider(mCreateGuildWindow, "SymBlueSlider");
            mSymBlueSlider.SetBounds(220, 500, 150, 20);
            mSymBlueSlider.SetRange(0, 255);
            mSymBlueSlider.Value = symB;
            mSymBlueSlider.ValueChanged += OnSymColorSliderChanged;

            // Text B
            mSymBlueText = new TextBoxNumeric(mCreateGuildWindow, "SymBlueText");
            mSymBlueText.SetBounds(380, 500, 50, 20);
            mSymBlueText.SetText(symB.ToString());
            mSymBlueText.SetMaxLength(255);
            mSymBlueText.SubmitPressed += OnSymColorTextChanged;
        }

        // =====================================================
        //  Sliders para mover en Y y escalar el símbolo
        // =====================================================
        private void InitializeSymbolTransformSliders()
        {
            mTransformLabel = new Label(mCreateGuildWindow, "TransformLabel");
            mTransformLabel.Text = "Transformar Símbolo:";
            mTransformLabel.SetBounds(220, 530, 200, 30);

            // Slider de posición Y
            mSymbolPosYSlider = new HorizontalSlider(mCreateGuildWindow, "SymbolPosYSlider");
            mSymbolPosYSlider.SetBounds(220, 560, 150, 20);
            mSymbolPosYSlider.SetRange(-50, 50);
            mSymbolPosYSlider.Value = 0;
            mSymbolPosYSlider.ValueChanged += OnSymbolTransformChanged;

            // Slider de escala (50% a 200%)
            mSymbolScaleSlider = new HorizontalSlider(mCreateGuildWindow, "SymbolScaleSlider");
            mSymbolScaleSlider.SetBounds(220, 590, 150, 20);
            mSymbolScaleSlider.SetRange(100, 150);
            mSymbolScaleSlider.Value = 100; // 100% = 1.0
            mSymbolScaleSlider.ValueChanged += OnSymbolTransformChanged;
        }

        // =====================================================
        //  Eventos de color para FONDO
        // =====================================================
        private void OnBgColorSliderChanged(Base sender, EventArgs e)
        {
            if (sender == mBgRedSlider)
            {
                mBgRedText.Value = mBgRedSlider.Value;
            }
            else if (sender == mBgGreenSlider)
            {
                mBgGreenText.Value = mBgGreenSlider.Value;
            }
            else if (sender == mBgBlueSlider)
            {
                mBgBlueText.Value = mBgBlueSlider.Value;
            }

            UpdateBackgroundColor();
        }

        private void OnBgColorTextChanged(Base sender, EventArgs e)
        {
            if (sender == mBgRedText)
            {
                mBgRedSlider.Value = mBgRedText.Value;
            }
            else if (sender == mBgGreenText)
            {
                mBgGreenSlider.Value = mBgGreenText.Value;
            }
            else if (sender == mBgBlueText)
            {
                mBgBlueSlider.Value = mBgBlueText.Value;
            }

            UpdateBackgroundColor();
        }

        private void UpdateBackgroundColor()
        {
            // Guardamos
            bgR = (byte)mBgRedSlider.Value;
            bgG = (byte)mBgGreenSlider.Value;
            bgB = (byte)mBgBlueSlider.Value;

            // Aplicamos al preview
            mBackgroundPreview.RenderColor = Color.FromArgb(255, bgR, bgG, bgB);
        }

        // =====================================================
        //  Eventos de color para SÍMBOLO
        // =====================================================
        private void OnSymColorSliderChanged(Base sender, EventArgs e)
        {
            if (sender == mSymRedSlider)
            {
                mSymRedText.Value = mSymRedSlider.Value;
            }
            else if (sender == mSymGreenSlider)
            {
                mSymGreenText.Value = mSymGreenSlider.Value;
            }
            else if (sender == mSymBlueSlider)
            {
                mSymBlueText.Value = mSymBlueSlider.Value;
            }

            UpdateSymbolColor();
        }

        private void OnSymColorTextChanged(Base sender, EventArgs e)
        {
            if (sender == mSymRedText)
            {
                mSymRedSlider.Value = mSymRedText.Value;
            }
            else if (sender == mSymGreenText)
            {
                mSymGreenSlider.Value = mSymGreenText.Value;
            }
            else if (sender == mSymBlueText)
            {
                mSymBlueSlider.Value = mSymBlueText.Value;
            }

            UpdateSymbolColor();
        }

        private void UpdateSymbolColor()
        {
            symR = (byte)mSymRedSlider.Value;
            symG = (byte)mSymGreenSlider.Value;
            symB = (byte)mSymBlueSlider.Value;

            // Aplicamos al preview
            mSymbolPreview.RenderColor = Color.FromArgb(255, symR, symG, symB);
        }

        // =====================================================
        //  Evento al cambiar sliders de posición Y o escala
        // =====================================================
        private void OnSymbolTransformChanged(Base sender, EventArgs e)
        {
            symbolPosY = (int)mSymbolPosYSlider.Value;
            symbolScale = (float)(mSymbolScaleSlider.Value / 100f);
            UpdateSymbolTransform();
        }

        private void UpdateSymbolTransform()
        {
            if (mLogoElements[1] == null) return; // No hay símbolo

            // Toma el tamaño actual del symbolPreview
            int w = mSymbolPreview.Width;
            int h = mSymbolPreview.Height;

            // Aplicamos la escala
            int newW = (int)(w * symbolScale);
            int newH = (int)(h * symbolScale);

            mSymbolPreview.SetSize(newW, newH);

            // Centrar en X
            int centerX = (COMPOSITION_SIZE - newW) / 2;
            // Centrar en Y y sumarle symbolPosY
            int centerY = (COMPOSITION_SIZE - newH) / 2;
            int finalY = centerY + symbolPosY;

            mSymbolPreview.SetPosition(centerX, finalY);
        }

        // =====================================================
        //  Actualizar la previsualización general
        // =====================================================
        private void UpdateLogoPreview()
        {
            // Ajustamos a COMPOSITION_SIZE (100)
            int previewSize = COMPOSITION_SIZE;

            // Fondo
            if (mLogoElements[0] != null)
            {
                mBackgroundPreview.Texture = mLogoElements[0];
                var (scaledW, scaledH) = ScaleToFit(mLogoElements[0].Width, mLogoElements[0].Height, previewSize, previewSize);
                mBackgroundPreview.SetSize(scaledW, scaledH);
                Align.Center(mBackgroundPreview);
            }
            else
            {
                mBackgroundPreview.SetSize(0, 0);
            }

            // Símbolo
            if (mLogoElements[1] != null)
            {
                mSymbolPreview.Texture = mLogoElements[1];
                // Base scale -> 60% de 100
                var baseSize = (int)(previewSize * 0.6f);
                var (scaledW, scaledH) = ScaleToFit(mLogoElements[1].Width, mLogoElements[1].Height, baseSize, baseSize);
                mSymbolPreview.SetSize(scaledW, scaledH);

                int centerX = (previewSize - scaledW) / 2;
                int centerY = (previewSize - scaledH) / 2;
                Align.Center(mSymbolPreview);
            }
            else
            {
                mSymbolPreview.SetSize(0, 0);
            }

            // Reaplicamos el transform (pos Y / escala) que el usuario haya movido
            UpdateSymbolTransform();

            // Volvemos a aplicar los colores que tengan los sliders
            // (por si se cambió la textura recién)
            mBackgroundPreview.RenderColor = Color.FromArgb(255, bgR, bgG, bgB);
            mSymbolPreview.RenderColor = Color.FromArgb(255, symR, symG, symB);
        }

        // =====================================================
        //  Lógica de update del UI
        // =====================================================
        public void Update()
        {
            if (mCreateGuildWindow.IsHidden) return;

            if (!mInitializedSymbols)
            {
                mInitializedSymbols = true;
                InitializeSymbolPanel();
            }
            if (!mInitializedBackgrounds)
            {
                mInitializedBackgrounds = true;
                InitializeBackgroundPanel();
            }

            // Limitar nombre del gremio
            if (!string.IsNullOrEmpty(mGuildNameTextbox.Text) && mGuildNameTextbox.Text.Length > 20)
            {
                mGuildNameTextbox.Text = mGuildNameTextbox.Text.Substring(0, 20);
            }
        }

        // =====================================================
        //  Utilidades para Mostrar/Ocultar
        // =====================================================
        public void Hide()
        {
            mCreateGuildWindow.IsHidden = true;
        }

        public void Show()
        {
            mCreateGuildWindow.IsHidden = false;

        }

        // =====================================================
        //  Función auxiliar: escalar una imagen a un contenedor
        // =====================================================
        private (int w, int h) ScaleToFit(int originalW, int originalH, int maxW, int maxH)
        {
            if (originalW <= 0 || originalH <= 0)
                return (maxW, maxH); // Evitamos divisiones por cero

            float ratioW = (float)maxW / originalW;
            float ratioH = (float)maxH / originalH;
            float ratio = Math.Min(ratioW, ratioH);

            int newW = (int)(originalW * ratio);
            int newH = (int)(originalH * ratio);

            return (newW, newH);
        }
    }
}
