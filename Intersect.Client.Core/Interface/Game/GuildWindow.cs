using System;
using Intersect.Client.Core;
using Intersect.Client.Entities;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Gwen;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.Framework.Gwen.Control.Layout;
using Intersect.Client.General;
using Intersect.Client.Interface.Shared;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Enums;
using Intersect.Network.Packets.Server;

namespace Intersect.Client.Interface.Game
{
    partial class GuildWindow : WindowControl
    {
        // Contenedores / Paneles
        private readonly ImagePanel _panelSearchArea;
        private readonly ImagePanel _panelMemberList;
        private readonly ImagePanel _panelActions;

        // TextBox Search
        private readonly ImagePanel _textboxContainer;
        private readonly TextBox _textboxSearch;

        // Lista de miembros
        private readonly ListBox _listGuildMembers;

        // Botones
        private readonly Button _buttonAdd;
        private readonly Button _buttonLeave;
        private readonly Button _buttonAddPopup;

        // Context Menu y opciones (promover, expulsar, etc.)
        private readonly Framework.Gwen.Control.Menu _contextMenu;
        private readonly MenuItem _privateMessageOption;
        private readonly MenuItem[] _promoteOptions;
        private readonly MenuItem[] _demoteOptions;
        private readonly MenuItem _kickOption;
        private readonly MenuItem _transferOption;
        private readonly MenuItem _expContributionOption;

        private readonly bool _addButtonUsed;
        private readonly bool _addPopupButtonUsed;
        private GuildMember? _selectedMember;

        // Controles para mostrar el logo
        private ImagePanel mLogoContainer;  // Contenedor principal del logo
        private ImagePanel mBackgroundLogo; // Panel para el fondo
        private ImagePanel mSymbolLogo;     // Panel para el símbolo

        public Label guildLevelLabel;
        private Label GuildExpTitle;
        private ImagePanel GuildExpBackground;
        private ImagePanel GuildExpBar;
        private Label GuildExpLabel;
        private ImagePanel _panelUpgrades;
        private Dictionary<GuildUpgradeType, Button> _upgradeButtons = new();
        private Dictionary<GuildUpgradeType, Label> _upgradeLabels = new();
        private Label pointsLabel;
        private Label spentLabel;
        private bool _isViewingUpgrades = false;
        private Label mGuildMembersLabel;

        public GuildWindow(Canvas gameCanvas) : base(gameCanvas, Globals.Me?.Guild, false, nameof(GuildWindow))
        {
            DisableResizing();
            // Ajusta el tamaño de la ventana a tu gusto
            this.SetSize(600, 400);

            // Área de búsqueda y botones (superior derecha)
            _textboxContainer = new ImagePanel(this, "SearchContainer");
            _textboxContainer.SetBounds(120, 10, 400, 30);

            _textboxSearch = new TextBox(_textboxContainer, "SearchTextbox");
            _textboxSearch.SetBounds(0, 0, 400, 30);
            Interface.FocusElements.Add(_textboxSearch);
            // Panel de Miembros
            _panelMemberList = new ImagePanel(this, "PanelMemberList");
            _panelMemberList.SetBounds(10, 120, 620, 270);
            _panelMemberList.IsHidden = false;

            #region Upgrade
            // Panel de Mejoras
            _panelUpgrades = new ImagePanel(this, "PanelUpgrades");
            _panelUpgrades.SetBounds(10, 120, 620, 270);
            _panelUpgrades.IsHidden = true;
            // Label: Puntos disponibles
           pointsLabel = new Label(_panelUpgrades, "GuildPointsLabel");
            pointsLabel.SetText($"Puntos disponibles: {Guild.GuildPoints}");
            pointsLabel.SetBounds(10, 10, 300, 20);

            // Label: Puntos gastados
            spentLabel = new Label(_panelUpgrades, "GuildSpentLabel");
            spentLabel.SetText($"Puntos usados: {Guild.GuildSpent}");
            spentLabel.SetBounds(10, 35, 300, 20);

            // Título para mejoras
            var upgradesTitle = new Label(_panelUpgrades, "GuildUpgradesTitle");
            upgradesTitle.SetText("Mejoras de Gremio:");
            upgradesTitle.SetBounds(10, 60, 200, 20);

            // Lista dinámica de mejoras
            int yOffset = 90;
          
            foreach (var upgradeType in Enum.GetValues(typeof(GuildUpgradeType)).Cast<GuildUpgradeType>())
            {
                var upgradeName = upgradeType.ToString();
                int currentLevel = Guild.GetUpgradeLevel(upgradeType);
                int maxLevel = Guild.MaxUpgradeLevels[upgradeType];
                int cost = Guild.UpgradeCosts[upgradeType].ElementAtOrDefault(currentLevel);

                var upgradeLabel = new Label(_panelUpgrades,"Upgrade_"+ upgradeName );
                upgradeLabel.SetText($"{upgradeName} - Nivel {currentLevel}/{maxLevel}");
                upgradeLabel.SetBounds(10, yOffset, 250, 20);
                _upgradeLabels[upgradeType] = upgradeLabel;
                upgradeLabel.SetToolTipText(GetUpgradeTooltip(upgradeType));

                var upgradeBtn = new Button(_panelUpgrades,upgradeName +"_UpButton")
                {
                    Text = $"Subir ({cost} pts)"
                };
                upgradeBtn.IsDisabled = currentLevel >= maxLevel || Guild.GuildPoints < cost || Globals.Me.Rank != 0;

                upgradeBtn.SetBounds(270, yOffset - 2, 100, 25);
                upgradeBtn.UserData = upgradeType;
                upgradeBtn.Clicked += (s, e) =>
                {
                    if (s is Button btn && btn.UserData is GuildUpgradeType type)
                    {
                        PacketSender.SendApplyGuildUpgrade(type);
                    }
                };
                _upgradeButtons[upgradeType] = upgradeBtn;

                yOffset += 30;
            }

            #endregion
            // Aquí puedes agregar luego los controles para mostrar mejoras dentro de _panelUpgrades

            // Botón para mostrar miembros
            var btnShowMembers = new Button(this, "ShowMembersButton")
            {
                Text = "👥 Miembros"
            };
            btnShowMembers.SetBounds(10, 90, 100, 25);
            btnShowMembers.Clicked += (s, e) =>
            {
                _panelMemberList.IsHidden = false;
                _panelUpgrades.IsHidden = true;
                _isViewingUpgrades = false;
            };

            // Botón para mostrar mejoras
            var btnShowUpgrades = new Button(this, "ShowUpgradesButton")
            {
                Text = "⭐ Mejoras"
            };
            btnShowUpgrades.SetBounds(120, 90, 100, 25);
            btnShowUpgrades.Clicked += (s, e) =>
            {
                _panelMemberList.IsHidden = true;
                _panelUpgrades.IsHidden = false;
                _isViewingUpgrades = true;
            };
            // Lista de miembros amplia (parte inferior)
            _listGuildMembers = new ListBox(_panelMemberList, "GuildMembers") { ColumnCount = 6 };
            _listGuildMembers.SetColumnWidth(0, 150); // Nombre
            _listGuildMembers.SetColumnWidth(1, 100); // Rango
            _listGuildMembers.SetColumnWidth(2, 50);  // Nivel
            _listGuildMembers.SetColumnWidth(3, 50);  // % XP
            _listGuildMembers.SetColumnWidth(4, 120); // XP Donada
            _listGuildMembers.SetColumnWidth(5, 100); // Mapa

            _listGuildMembers.SetBounds(10, 120, 620, 270);


            #region Action Buttons
            // Acciones (Invitar, Salir, etc.)
            _panelActions = new ImagePanel(_panelMemberList, "ActionsPanel");
            _panelActions.SetBounds(320, 5, 270, 70);

            _buttonAdd = new Button(_panelActions, "InviteButton") { Text = Strings.Guilds.Add };
            _buttonAdd.SetBounds(530, 10, 100, 30);
            _buttonAdd.Clicked += (s, e) => {
                if (_textboxSearch.Text.Trim().Length >= 3)
                    PacketSender.SendInviteGuild(_textboxSearch.Text);
            };

            _buttonLeave = new Button(_panelActions, "LeaveButton") { Text = Strings.Guilds.Leave };
            _buttonLeave.SetBounds(530, 50, 100, 30);
            _buttonLeave.Clicked += (s, e) => new InputBox(
                title: Strings.Guilds.LeaveTitle,
                prompt: Strings.Guilds.LeavePrompt.ToString(Globals.Me?.Guild),
                inputType: InputBox.InputType.YesNo,
                onSuccess: (s, e) => PacketSender.SendLeaveGuild()
            );


            // Add Popup Button
            _buttonAddPopup = new Button(_panelActions, "InvitePopupButton")
            {
                Text = Strings.Guilds.Invite,
                IsHidden = true
            };
            _buttonAddPopup.SetBounds(180, 0, 80, 30);
            _buttonAddPopup.Clicked += (s, e) =>
            {
                _ = new InputBox(
                    title: Strings.Guilds.InviteMemberTitle,
                    prompt: Strings.Guilds.InviteMemberPrompt.ToString(Globals.Me?.Guild),
                    inputType: InputBox.InputType.TextInput,
                    onSuccess: (s, e) =>
                    {
                        if (s is InputBox inputBox && inputBox.TextValue.Trim().Length >= 3)
                        {
                            PacketSender.SendInviteGuild(inputBox.TextValue);
                        }
                    }
                );
            };
            #endregion

            #region Context Menu Options
            // Context Menu
            _contextMenu = new Framework.Gwen.Control.Menu(gameCanvas, "GuildContextMenu")
            {
                IsHidden = true,
                IconMarginDisabled = true
            };

            _contextMenu.Children.Clear();

            // Private Message
            _privateMessageOption = _contextMenu.AddItem(Strings.Guilds.PM);
            _privateMessageOption.Clicked += (s, e) =>
            {
                if (_selectedMember?.Online == true && _selectedMember?.Id != Globals.Me?.Id)
                {
                    Interface.GameUi.SetChatboxText("/pm " + _selectedMember!.Name + " ");
                }
            };

            // Promote Options
            _promoteOptions = new MenuItem[Options.Instance.Guild.Ranks.Length - 2];
            for (int i = 1; i < Options.Instance.Guild.Ranks.Length - 1; i++)
            {
                _promoteOptions[i - 1] = _contextMenu.AddItem(Strings.Guilds.Promote.ToString(Options.Instance.Guild.Ranks[i].Title));
                _promoteOptions[i - 1].UserData = i;
                _promoteOptions[i - 1].Clicked += promoteOption_Clicked;
            }

            // Demote Options
            _demoteOptions = new MenuItem[Options.Instance.Guild.Ranks.Length - 2];
            for (int i = 2; i < Options.Instance.Guild.Ranks.Length; i++)
            {
                _demoteOptions[i - 2] = _contextMenu.AddItem(Strings.Guilds.Demote.ToString(Options.Instance.Guild.Ranks[i].Title));
                _demoteOptions[i - 2].UserData = i;
                _demoteOptions[i - 2].Clicked += demoteOption_Clicked;
            }

            // Kick Option
            _kickOption = _contextMenu.AddItem(Strings.Guilds.Kick);
            _kickOption.Clicked += kickOption_Clicked;

            // Transfer Option
            _transferOption = _contextMenu.AddItem(Strings.Guilds.Transfer);
            _transferOption.Clicked += transferOption_Clicked;
            // Change Experience Option
            _expContributionOption = _contextMenu.AddItem("Modificar contribución de XP");
            _expContributionOption.Clicked += (s, e) =>
            {
                // Convertimos el valor actual a int (puede estar como float)
                int currentValue = (int)Math.Round(Globals.Me.GuildXpContribution);

                _ = new InputBox(
                    title: "Configurar Contribución de XP",
                    prompt: $"Selecciona el porcentaje de XP que deseas donar al gremio (actual: {currentValue}%)",
                    inputType: InputBox.InputType.NumericSliderInput,
                    onSuccess: (sender, args) =>
                    {
                        if (sender is InputBox inputBox)
                        {
                            int newPercentage = (int)Math.Clamp(inputBox.Value, 0, 100);
                            PacketSender.SendUpdateGuildXpContribution(newPercentage);
                            PacketSender.SendChatMsg($"Has cambiado tu contribución de XP a {newPercentage}%.", 5);
                        }
                    },
                    quantity: currentValue,      // Valor inicial del slider
                    maxQuantity: 100             // Máximo del slider
                );
            };

            #endregion

            #region Logo
            // Contenedor principal para el logo del gremio (superior izquierda)
            mLogoContainer = new ImagePanel(this, "GuildLogoContainer");
            mLogoContainer.SetBounds(10, 10, 100, 100);

            mBackgroundLogo = new ImagePanel(mLogoContainer, "GuildBackgroundLogo");
            mBackgroundLogo.SetBounds(0, 0, 100, 100);
            mBackgroundLogo.Show();

            mSymbolLogo = new ImagePanel(mBackgroundLogo, "GuildSymbolLogo");
            mSymbolLogo.SetBounds(0, 0, 100, 100);
            mSymbolLogo.Show();
            // Nivel del gremio al lado del logo
            guildLevelLabel = new Label(this, "GuildLevelLabel")
            {
                Text = $"Nivel: {Guild.GuildLevel}",
                TextColor = Color.White
            };
            guildLevelLabel.SetBounds(80, 10, 150, 25);

            // Título de experiencia del gremio
            GuildExpTitle = new Label(this, "GuildExpTitle");
            GuildExpTitle.SetText("Exp:");
            GuildExpTitle.SetBounds(80, 40, 150, 20);
            GuildExpTitle.RenderColor = Color.FromArgb(255, 255, 255, 255);

            // Fondo de la barra de experiencia del gremio
            GuildExpBackground = new ImagePanel(this, "GuildExpBackground");
            GuildExpBackground.SetBounds(80, 65, 150, 20);
            GuildExpBackground.RenderColor = Color.FromArgb(255, 100, 100, 100);

            // Barra de experiencia actual
            GuildExpBar = new ImagePanel(GuildExpBackground, "GuildExpBar");
            float percentage = (float)Guild.GuildExp / Guild.GuildExpToNextLevel;
            GuildExpBar.SetBounds(0, 0, (int)(200 * percentage), 20);
            GuildExpBar.RenderColor = Color.FromArgb(255, 50, 150, 50);

            // Label con texto Exp/ExpTNL centrado en la barra
            GuildExpLabel = new Label(GuildExpBackground, "GuildExpLabel");
            GuildExpLabel.SetText($"{Guild.GuildExp} / {Guild.GuildExpToNextLevel}");
            GuildExpLabel.SetBounds(0, 0, 200, 20);
            GuildExpLabel.Alignment = Pos.Center;
            GuildExpLabel.RenderColor = Color.White;
            var current = Globals.Me?.GuildMembers?.Length ?? 0;
            var max = Guild.GetMaxMembers();
          

            // Miembros actuales / máximo
          mGuildMembersLabel = new Label(this, "GuildMembersLabel")
            {
                Text = $"Miembros: {current}/{max}",

                TextColor = Color.White
            };
            mGuildMembersLabel.SetBounds(80, 70, 200, 20);

            #endregion
            // Llama a UpdateList() para cargar la lista
            UpdateList();
            UpdateLogo();
            _contextMenu.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer?.GetResolutionString());
            LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer?.GetResolutionString());

            _addButtonUsed = !_buttonAdd.IsHidden;
            _addPopupButtonUsed = !_buttonAddPopup.IsHidden;
        }

        private string GetUpgradeTooltip(GuildUpgradeType type)
        {
            int level = Guild.GetUpgradeLevel(type);
            return type switch
            {
                GuildUpgradeType.ExtraMembers => $"Incrementa el número máximo de miembros en +{level * 10}.",
                GuildUpgradeType.ExtraBankSlots => $"Agrega +{level * 10} espacios al banco.",
                GuildUpgradeType.BonusXp => $"Bono de experiencia del {level * 5}%.",
                GuildUpgradeType.BonusDrop => $"Bono de botín del {level * 5}%.",
                GuildUpgradeType.BonusJobXp=> $"Bono de experiencia del {level * 5}%.",
                _ => "Mejora desconocida."
            };
        }


        public void UpdateUpgradesPanel()
        {
            foreach (var upgradeType in Enum.GetValues(typeof(GuildUpgradeType)).Cast<GuildUpgradeType>())
            {
                int currentLevel = Guild.GetUpgradeLevel(upgradeType);
                int maxLevel = Guild.MaxUpgradeLevels[upgradeType];
                int cost = Guild.UpgradeCosts[upgradeType].ElementAtOrDefault(currentLevel);

                if (_upgradeLabels.TryGetValue(upgradeType, out var label))
                {
                    label.Text = $"{upgradeType} - Nivel: {currentLevel}/{maxLevel}";
                }

                if (_upgradeButtons.TryGetValue(upgradeType, out var button))
                {
                    button.Text = $"Subir ({cost} pts)";
                    button.IsDisabled = currentLevel >= maxLevel || Guild.GuildPoints < cost;
                }
            }
        }

        public void UpdateLogo()
        {
            Globals.Me.ConsultGuildLogo();

            // 1) Fondo: usamos la ruta completa sin comodines
            string backgroundFolderPath = "resources/Guild/Background";
            if (!Directory.Exists(backgroundFolderPath))
                Directory.CreateDirectory(backgroundFolderPath);

            var backgroundPath = "resources/Guild/Background/" + Globals.Me.GuildBackgroundFile;
            var fileNameB = Path.GetFileName(backgroundPath);

            // Asignar textura al fondo
            mBackgroundLogo.Texture = Globals.ContentManager.GetTexture(
                Framework.Content.TextureType.Guild,
                fileNameB
            );

            if (mBackgroundLogo.Texture != null)
            {
                // Escalar la imagen para caber en 48x48 manteniendo proporción
                var (scaledW, scaledH) = ScaleToFit(mBackgroundLogo.Texture.Width, mBackgroundLogo.Texture.Height, 80, 80);
                mBackgroundLogo.SetSize(scaledW, scaledH);
                mBackgroundLogo.AddAlignment(Alignments.Center);
                // Aplicar color guardado
                mBackgroundLogo.RenderColor = new Color(255,
                    Globals.Me.GuildBackgroundR,
                    Globals.Me.GuildBackgroundG,
                    Globals.Me.GuildBackgroundB
                   
                );
                // Centrar la imagen
                Align.Center(mBackgroundLogo);

                //PacketSender.SendChatMsg($"Fondo cargado correctamente: {Globals.Me.GuildBackgroundFile}", 5);
            }
            else
            {
                //PacketSender.SendChatMsg($"Error al cargar el fondo: {Globals.Me.GuildBackgroundFile}", 5);
                mBackgroundLogo.SetSize(0, 0);
            }

          

            mBackgroundLogo.Show();
            string symbolFolderPath = "resources/Guild/Symbols";
            if (!Directory.Exists(symbolFolderPath))
                Directory.CreateDirectory(symbolFolderPath);

            var symbolPath = "resources/Guild/Symbols/" + Globals.Me.GuildSymbolFile;
            var fileName = Path.GetFileName(symbolPath);

            // Asignar textura al símbolo
            mSymbolLogo.Texture = Globals.ContentManager.GetTexture(
                Framework.Content.TextureType.Guild,
                fileName
            );

            if (mSymbolLogo.Texture != null)
            {
                // Escalar la imagen para caber en 48x48 manteniendo proporción
                var (scaledW, scaledH) = ScaleToFit(mSymbolLogo.Texture.Width, mSymbolLogo.Texture.Height,80, 80);
                mSymbolLogo.SetSize(scaledW, scaledH);
                // Escalar
                int baseSize = 50;
                int newW = baseSize;
                int newH = baseSize;
                mSymbolLogo.SetSize(newW, newH);

                // Centrar la imagen
                Align.Center(mSymbolLogo);
                mSymbolLogo.RenderColor = new Color(255,
                    Globals.Me.GuildSymbolR,
                    Globals.Me.GuildSymbolG,
                    Globals.Me.GuildSymbolB
                );
                //PacketSender.SendChatMsg($"Simbolo cargado correctamente: {Globals.Me.GuildSymbolFile}", 5);
            }
            else
            {
                mSymbolLogo.SetSize(0, 0);
            }
          

            mSymbolLogo.Show();
        
        }

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


        //Methods
        public void Update()
        {
            if (IsHidden)
            {
                return;
            }

            // Forzar el título de la ventana
            if (!string.IsNullOrEmpty(Globals.Me?.Guild) && Title != Globals.Me.Guild)
            {
                Title = Globals.Me.Guild;
            }

            // Refrescar el logo
            if (Globals.Me?.GuildBackgroundFile != mBackgroundLogo.Texture?.Name ||
                Globals.Me?.GuildSymbolFile != mSymbolLogo.Texture?.Name)
            {
                UpdateLogo();
            }
            // Actualizar Nivel del gremio

            if (guildLevelLabel != null)
            {
                guildLevelLabel.Text = $"Level: {Guild.GuildLevel}";
            }
            GuildExpBar.SetSize((int)(200 * (float)Guild.GuildExp / Guild.GuildExpToNextLevel), 20);
            GuildExpLabel.SetText($"{Guild.GuildExp} / {Guild.GuildExpToNextLevel}");
            GuildExpBar.Width = (int)(GuildExpBackground.Width * ((float)Guild.GuildExp / Guild.GuildExpToNextLevel));
            GuildExpBar.SetTextureRect(0, 0, (int)(200 * (float)Guild.GuildExp / Guild.GuildExpToNextLevel), 20);
            UpdateUpgradesPanel();
            pointsLabel?.SetText($"Puntos disponibles: {Guild.GuildPoints}");
            spentLabel?.SetText($"Puntos usados: {Guild.GuildSpent}");
            _panelMemberList.IsHidden = _isViewingUpgrades;
            _panelUpgrades.IsHidden = !_isViewingUpgrades;
            var current = Globals.Me?.GuildMembers?.Length ?? 0;
            var max = Guild.GetMaxMembers();
            mGuildMembersLabel.SetText($"Miembros: {current}/{max}");

        }

        public override void Hide()
        {
            _contextMenu?.Close();
            base.Hide();
        }

        #region "Member List"
        public void UpdateList()
        {
            _listGuildMembers.Clear();

            // Encabezado claro y organizado
            var header = _listGuildMembers.AddRow("Nombre", "Rango", "Nivel", "% XP", "XP Donada", "Mapa");

            header.SetTextColor(Color.White);
            header.RenderColor = new Color(80, 80, 80, 255);

            // Añadir miembros del gremio
            foreach (var member in Globals.Me?.GuildMembers ?? [])
            {
                // Manejo del nombre del mapa según estado online
                string mapName = "-";
                if (member.Online && !string.IsNullOrEmpty(member.Map))
                {
                    var mapSplit = member.Map.Split('-');
                    mapName = mapSplit.Length > 1 ? mapSplit[1].Trim() : member.Map;
                }

                // Crear fila completa con datos del miembro
                var row = _listGuildMembers.AddRow(
       member.Name,
       Options.Instance.Guild.Ranks[member.Rank].Title,
       member.Level.ToString(),
       $"{member.ExperiencePerc}%",
       $"{member.DonatedXp:N0}",
       mapName
   );


                // Añadir tooltip informativo (nivel y clase)
                row.SetToolTipText(Strings.Guilds.Tooltip.ToString(member.Level, member.Class));

                // Manejo de eventos para clics
                row.UserData = member;
                row.Clicked += member_Clicked;
                row.RightClicked += member_RightClicked;

                // Colorear según estado en línea
                row.SetTextColor(member.Online ? Color.Green : Color.Red);

                // Alternar colores de fondo por filas
                row.RenderColor = _listGuildMembers.RowCount % 2 == 0
                    ? new Color(210, 210, 210, 255)
                    : new Color(190, 190, 190, 255);
            }

            // Actualización de visibilidad botones según permisos
            var isInviteDenied = Globals.Me == null || Globals.Me.GuildRank == null || !Globals.Me.GuildRank.Permissions.Invite;
            _buttonAdd.IsHidden = isInviteDenied || !_addButtonUsed;
            _textboxContainer.IsHidden = isInviteDenied || !_addButtonUsed;
            _buttonAddPopup.IsHidden = isInviteDenied || !_addPopupButtonUsed;
            _buttonLeave.IsHidden = Globals.Me != null && Globals.Me.Rank == 0;
        }

        void member_Clicked(Base sender, ClickedEventArgs arguments)
        {
            if (sender is ListBoxRow row &&
                row.UserData is GuildMember member &&
                member.Online &&
                member.Id != Globals.Me?.Id
            )
            {
                Interface.GameUi.SetChatboxText("/pm " + member.Name + " ");
            }
        }

        private void member_RightClicked(Base sender, ClickedEventArgs arguments)
        {
            if (sender is not ListBoxRow row || row.UserData is not GuildMember member)
                return;

            if (Globals.Me == null)
                return;

            _selectedMember = member;

            var rank = Globals.Me.GuildRank;
            if (rank == null)
                return;

            foreach (var child in _contextMenu.Children.ToArray())
            {
                _contextMenu.RemoveChild(child, false);
            }

            var rankIndex = Globals.Me.Rank;
            var isOwner = rankIndex == 0;

            // Mensaje privado solo si no es uno mismo
            if (_selectedMember.Online && member.Id != Globals.Me.Id)
            {
                _contextMenu.AddChild(_privateMessageOption);
            }

            // Promote y Demote opciones (solo si no es uno mismo)
            if (member.Id != Globals.Me.Id)
            {
                //Promote Options
                foreach (var opt in _promoteOptions)
                {
                    var isAllowed = (isOwner || rank.Permissions.Promote);
                    var hasLowerRank = (int)opt.UserData > rankIndex;
                    var canRankChange = (int)opt.UserData < member.Rank && member.Rank > rankIndex;

                    if (isAllowed && hasLowerRank && canRankChange)
                    {
                        _contextMenu.AddChild(opt);
                    }
                }

                //Demote Options
                foreach (var opt in _demoteOptions)
                {
                    var isAllowed = (isOwner || rank.Permissions.Demote);
                    var hasLowerRank = (int)opt.UserData > rankIndex;
                    var canRankChange = (int)opt.UserData > member.Rank && member.Rank > rankIndex;

                    if (isAllowed && hasLowerRank && canRankChange)
                    {
                        _contextMenu.AddChild(opt);
                    }
                }

                // Kick y Transfer solo si no es uno mismo
                if ((rank.Permissions.Kick || isOwner) && member.Rank > rankIndex)
                {
                    _contextMenu.AddChild(_kickOption);
                }

                if (isOwner)
                {
                    _contextMenu.AddChild(_transferOption);
                }
            }

            // El Guild Master puede modificar la contribución de XP de cualquier miembro, incluyéndose a sí mismo.
            if (_selectedMember?.Id == Globals.Me?.Id)
            {
                _contextMenu.AddChild(_expContributionOption);
            }

            _contextMenu.SizeToChildren();
            _contextMenu.Open(Framework.Gwen.Pos.None);
        }

        #endregion

        #region Guild Actions

        private void promoteOption_Clicked(Base sender, ClickedEventArgs arguments)
        {
            if (Globals.Me == default || Globals.Me.GuildRank == default || _selectedMember == default)
            {
                return;
            }

            var rank = Globals.Me.GuildRank;
            var rankIndex = Globals.Me.Rank;
            var isOwner = rankIndex == 0;
            var newRank = (int)sender.UserData;

            if (!(rank.Permissions.Promote || isOwner) || _selectedMember.Rank <= rankIndex)
            {
                return;
            }

            _ = new InputBox(
                Strings.Guilds.PromoteTitle,
                Strings.Guilds.PromotePrompt.ToString(_selectedMember.Name, Options.Instance.Guild.Ranks[newRank].Title),
                InputBox.InputType.YesNo,
                userData: new Tuple<GuildMember, int>(_selectedMember, newRank),
                onSuccess: (s, e) =>
                {
                    if (s is InputBox inputBox && inputBox.UserData is Tuple<GuildMember, int> memberRankPair)
                    {
                        var (member, newRank) = memberRankPair;
                        PacketSender.SendPromoteGuildMember(member.Id, newRank);
                    }
                }
            );
        }

        private void demoteOption_Clicked(Base sender, ClickedEventArgs arguments)
        {
            if (Globals.Me == default || Globals.Me.GuildRank == default || _selectedMember == default)
            {
                return;
            }

            var rank = Globals.Me.GuildRank;
            var rankIndex = Globals.Me.Rank;
            var isOwner = rankIndex == 0;
            var newRank = (int)sender.UserData;

            if (!(rank.Permissions.Demote || isOwner) || _selectedMember.Rank <= rankIndex)
            {
                return;
            }

            _ = new InputBox(
                Strings.Guilds.DemoteTitle,
                Strings.Guilds.DemotePrompt.ToString(_selectedMember.Name, Options.Instance.Guild.Ranks[newRank].Title),
                InputBox.InputType.YesNo,
                userData: new Tuple<GuildMember, int>(_selectedMember, newRank),
                onSuccess: (s, e) =>
                {
                    if (s is InputBox inputBox && inputBox.UserData is Tuple<GuildMember, int> memberRankPair)
                    {
                        var (member, newRank) = memberRankPair;
                        PacketSender.SendDemoteGuildMember(member.Id, newRank);
                    }
                }
            );
        }

        private void kickOption_Clicked(Base sender, ClickedEventArgs arguments)
        {
            if (Globals.Me == default || Globals.Me.GuildRank == default || _selectedMember == default)
            {
                return;
            }

            var rank = Globals.Me.GuildRank;
            var rankIndex = Globals.Me.Rank;
            var isOwner = rankIndex == 0;

            if (!(rank.Permissions.Kick || isOwner) || _selectedMember.Rank <= rankIndex)
            {
                return;
            }

            _ = new InputBox(
                Strings.Guilds.KickTitle,
                Strings.Guilds.KickPrompt.ToString(_selectedMember?.Name),
                InputBox.InputType.YesNo,
                userData: _selectedMember,
                onSuccess: (s, e) =>
                {
                    if (s is InputBox inputBox && inputBox.UserData is GuildMember member)
                    {
                        PacketSender.SendKickGuildMember(member.Id);
                    }
                }
            );
        }

        private void transferOption_Clicked(Base sender, ClickedEventArgs arguments)
        {
            if (Globals.Me == default || Globals.Me.GuildRank == default || _selectedMember == default)
            {
                return;
            }

            var rank = Globals.Me.GuildRank;
            var rankIndex = Globals.Me.Rank;
            var isOwner = rankIndex == 0;

            if (!(rank.Permissions.Kick || isOwner) || _selectedMember.Rank <= rankIndex)
            {
                return;
            }

            _ = new InputBox(
                Strings.Guilds.TransferTitle,
                Strings.Guilds.TransferPrompt.ToString(_selectedMember?.Name, rank.Title, Globals.Me?.Guild),
                InputBox.InputType.TextInput,
                userData: _selectedMember,
                onSuccess: (s, e) =>
                {
                    if (s is InputBox inputBox && inputBox.TextValue == Globals.Me?.Guild && inputBox.UserData is GuildMember member)
                    {
                        PacketSender.SendTransferGuild(member.Id);
                    }
                }
            );
        }

        #endregion
    }
}
