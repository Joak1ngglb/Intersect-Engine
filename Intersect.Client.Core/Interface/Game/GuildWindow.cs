using System;
using Intersect.Client.Core;
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

        public GuildWindow(Canvas gameCanvas) : base(gameCanvas, Globals.Me?.Guild, false, nameof(GuildWindow))
        {
            DisableResizing();
            // Ajusta el tamaño de la ventana a tu gusto
            this.SetSize(600, 400);

            // 1) Panel principal o contenedor (ya en la Window).
           
           _panelSearchArea = new ImagePanel(this, "SearchArea");
         

            // Textbox Search
            _textboxContainer = new ImagePanel(this, "SearchContainer");
            _textboxContainer.SetBounds(10, 5, 300, 30);

            _textboxSearch = new TextBox(_textboxContainer, "SearchTextbox");
            _textboxSearch.SetBounds(0, 0, 300, 30);
            Interface.FocusElements.Add(_textboxSearch);

            // List of Guild Members
    
            _listGuildMembers = new ListBox(this, "GuildMembers");
            _listGuildMembers.SetBounds(2, 60, 300, 300);

            #region Action Buttons
            // Acciones (Invitar, Salir, etc.)
            _panelActions = new ImagePanel(this, "ActionsPanel");
            _panelActions.SetBounds(320, 5, 270, 70);

            // Add Button
            _buttonAdd = new Button(_panelActions, "InviteButton")
            {
                Text = Strings.Guilds.Add
            };
            _buttonAdd.SetBounds(0, 0, 80, 30);
            _buttonAdd.Clicked += (s, e) =>
            {
                if (_textboxSearch.Text.Trim().Length >= 3)
                {
                    PacketSender.SendInviteGuild(_textboxSearch.Text);
                }
            };

            // Leave Button
            _buttonLeave = new Button(_panelActions, "LeaveButton")
            {
                Text = Strings.Guilds.Leave
            };
            _buttonLeave.SetBounds(90, 0, 80, 30);
            _buttonLeave.Clicked += (s, e) =>
            {
                _ = new InputBox(
                    title: Strings.Guilds.LeaveTitle,
                    prompt: Strings.Guilds.LeavePrompt.ToString(Globals.Me?.Guild),
                    inputType: InputBox.InputType.YesNo,
                    onSuccess: (s, e) => PacketSender.SendLeaveGuild()
                );
            };

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
                _ = new InputBox(
                    title: "Configurar Contribución de XP",
                    prompt: "Ingresa el porcentaje de XP que deseas donar al gremio:",
                    inputType: InputBox.InputType.NumericInput,
                    userData: null,
                    onSuccess: (sender, args) =>
                    {
                        if (sender is InputBox inputBox && float.TryParse(inputBox.TextValue, out float newPercentage))
                        {
                            newPercentage = Math.Clamp(newPercentage, 0f, 100f);
                            PacketSender.SendUpdateGuildXpContribution(newPercentage);
                            PacketSender.SendChatMsg($"Has cambiado tu contribución de XP a {newPercentage}%.",5);
                        }
                    }
                );
            };
            #endregion

            // 2) Crear contenedor para el logo
            mLogoContainer = new ImagePanel(this, "GuildLogoContainer");
            // Ajusta su posición: lo ponemos a la derecha.
            mLogoContainer.SetBounds(320, 80, 270, 260);

            // Panel para el fondo
            mBackgroundLogo = new ImagePanel(mLogoContainer, "GuildBackgroundLogo");
            // Ejemplo: 128×128 en el contenedor
           
            mBackgroundLogo.Show();

            // Panel para el símbolo
            mSymbolLogo = new ImagePanel(mBackgroundLogo, "GuildSymbolLogo");
        
            mSymbolLogo.Show();

            // Llama a UpdateList() para cargar la lista
            UpdateList();
            UpdateLogo();
            _contextMenu.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer?.GetResolutionString());
            LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer?.GetResolutionString());

            _addButtonUsed = !_buttonAdd.IsHidden;
            _addPopupButtonUsed = !_buttonAddPopup.IsHidden;
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
                var (scaledW, scaledH) = ScaleToFit(mBackgroundLogo.Texture.Width, mBackgroundLogo.Texture.Height, 48, 48);
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

                PacketSender.SendChatMsg($"Fondo cargado correctamente: {Globals.Me.GuildBackgroundFile}", 5);
            }
            else
            {
                PacketSender.SendChatMsg($"Error al cargar el fondo: {Globals.Me.GuildBackgroundFile}", 5);
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
                var (scaledW, scaledH) = ScaleToFit(mSymbolLogo.Texture.Width, mSymbolLogo.Texture.Height,48, 48);
                mSymbolLogo.SetSize(scaledW, scaledH);
                // Escalar
                int baseSize = 32;
                int newW = (int)(baseSize * Globals.Me.GuildSymbolScale);
                int newH = (int)(baseSize * Globals.Me.GuildSymbolScale);
                mSymbolLogo.SetSize(newW, newH);

                // Centrar la imagen
                Align.Center(mSymbolLogo);
                mSymbolLogo.RenderColor = new Color(255,
                    Globals.Me.GuildSymbolR,
                    Globals.Me.GuildSymbolG,
                    Globals.Me.GuildSymbolB
                );
                PacketSender.SendChatMsg($"Simbolo cargado correctamente: {Globals.Me.GuildSymbolFile}", 5);
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

            // Añadir encabezado
            var header = _listGuildMembers.AddRow("Nombre", "Rango", "Nivel", "% XP");
            header.SetTextColor(Color.White);
            header.RenderColor = new Color(80, 80, 80, 255);

            // Añadir miembros del gremio
            foreach (var member in Globals.Me?.GuildMembers ?? [])
            {
                var row = _listGuildMembers.AddRow(
                    member.Name,
                    Options.Instance.Guild.Ranks[member.Rank].Title,
                    member.Level.ToString(),
                    $"{member.ExperiencePerc}%"
                );

                row.UserData = member;
                row.Clicked += member_Clicked;
                row.RightClicked += member_RightClicked;

                row.SetTextColor(member.Online ? Color.Green : Color.Red);
                row.RenderColor = _listGuildMembers.RowCount % 2 == 0
                    ? new Color(210, 210, 210, 255)
                    : new Color(190, 190, 190, 255);
            }
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
            if (isOwner)
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
