using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DarkUI.Forms;
using Intersect.Editor.Content;
using Intersect.Editor.General;
using Intersect.Editor.Localization;
using Intersect.Editor.Networking;
using Intersect.Core;           // Para acceder a PetBase, PetPersonality, PetRarity, etc.
using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.Utilities;

namespace Intersect.Editor.Forms.Editors
{
    public partial class FrmPet : EditorForm
    {
        // Lista de elementos modificados
        private List<PetBase> mChanged = new List<PetBase>();

        // Variable para copiar/pegar objetos
        private string mCopiedItem;

        // El objeto que estamos editando
        private PetBase mEditorItem;

        // Lista de carpetas conocidas (para organizar los objetos)
        private List<string> mKnownFolders = new List<string>();

        public FrmPet()
        {
            ApplyHooks();
            InitializeComponent();
            Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            // Inicializa la lista de objetos en el editor (similar al editor de Pet)
            lstGameObjects.Init(UpdateToolStripItems, AssignEditorItem, toolStripItemNew_Click,
                toolStripItemCopy_Click, toolStripItemUndo_Click, toolStripItemPaste_Click, toolStripItemDelete_Click);
        }

        /// <summary>
        /// Asigna el objeto a editar (usando su Guid).
        /// </summary>
        private void AssignEditorItem(Guid id)
        {
            mEditorItem = PetBase.Get(id);
            UpdateEditor();
        }

        /// <summary>
        /// Se llama cuando algún objeto ha sido actualizado en el sistema.
        /// Si el tipo es Pet, reinicia la lista y la edición.
        /// </summary>
        protected override void GameObjectUpdatedDelegate(GameObjectType type)
        {
            if (type == GameObjectType.Pet)
            {
                InitEditor();
                if (mEditorItem != null && !PetBase.Lookup.Values.Contains(mEditorItem))
                {
                    mEditorItem = null;
                    UpdateEditor();
                }
            }
        }

        /// <summary>
        /// Botón Cancelar: restaura los cambios no guardados y cierra el formulario.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            foreach (var item in mChanged)
            {
                item.RestoreBackup();
                item.DeleteBackup();
            }
            Hide();
            Globals.CurrentEditor = -1;
            Dispose();
        }

        /// <summary>
        /// Botón Guardar: envía al servidor los objetos modificados.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (var item in mChanged)
            {
                // Se puede ordenar (por ejemplo, immunities) para mantener la consistencia
                item.Immunities.Sort();
                PacketSender.SendSaveObject(item);
                item.DeleteBackup();
            }
            Hide();
            Globals.CurrentEditor = -1;
            Dispose();
        }

        /// <summary>
        /// Al cargar el formulario, se inicializan los controles necesarios.
        /// </summary>
        private void FrmPet_Load(object sender, EventArgs e)
        {
            // Inicializar el ComboBox de Sprite
            cmbSprite.Items.Clear();
            cmbSprite.Items.Add(Strings.General.None);
            cmbSprite.Items.AddRange(GameContentManager.GetSmartSortedTextureNames(GameContentManager.TextureType.Entity));

            // Inicializar el ComboBox de Spells
            cmbSpell.Items.Clear();
            cmbSpell.Items.AddRange(SpellBase.Names);

            // Aquí se pueden inicializar otros controles específicos de mascotas,
            // por ejemplo, para la especie, comportamiento, etc.

            InitLocalization();
            UpdateEditor();
        }

        /// <summary>
        /// Inicializa las cadenas de localización (textos, etiquetas, etc.) para el editor de mascotas.
        /// </summary>
        private void InitLocalization()
        {
            Text = Strings.PetEditor.title;
            toolStripItemNew.Text = Strings.PetEditor.New;
            toolStripItemDelete.Text = Strings.PetEditor.delete;
            toolStripItemCopy.Text = Strings.PetEditor.copy;
            toolStripItemPaste.Text = Strings.PetEditor.paste;
            toolStripItemUndo.Text = Strings.PetEditor.undo;

            grpPets.Text = Strings.PetEditor.pets;
            grpGeneral.Text = Strings.PetEditor.general;
            lblName.Text = Strings.PetEditor.name;
            lblPic.Text = Strings.PetEditor.sprite;

            // Propiedades específicas de mascotas:
            lblSpecies.Text = Strings.PetEditor.species;
            lblRequiredMaturity.Text = Strings.PetEditor.requiredmaturity;
            lblRequiredEnergy.Text = Strings.PetEditor.requiredenergy;
            lblRequiredMood.Text = Strings.PetEditor.requiredmood;
            lblMaxBreedCount.Text = Strings.PetEditor.maxbreedcount;

            grpStats.Text = Strings.PetEditor.stats;
          //  lblHP.Text = Strings.PetEditor.hp;
          //  lblMana.Text = Strings.PetEditor.mana;
            lblStr.Text = Strings.PetEditor.attack;
            lblDef.Text = Strings.PetEditor.defense;
            lblSpd.Text = Strings.PetEditor.speed;
            lblMag.Text = Strings.PetEditor.abilitypower;
            lblMR.Text = Strings.PetEditor.magicresist;

            grpCombat.Text = Strings.PetEditor.combat;
            lblDamage.Text = Strings.PetEditor.basedamage;
            lblCritChance.Text = Strings.PetEditor.critchance;
            lblCritMultiplier.Text = Strings.PetEditor.critmultiplier;
            lblDamageType.Text = Strings.PetEditor.damagetype;
            lblScalingStat.Text = Strings.PetEditor.scalingstat;
            lblScaling.Text = Strings.PetEditor.scalingamount;
            lblAttackAnimation.Text = Strings.PetEditor.attackanimation;

            // Se pueden agregar más grupos (por ejemplo, para reproducción, comportamiento, etc.)

          //  btnSave.Text = Strings.PetEditor.save;
         //  btnCancel.Text = Strings.PetEditor.cancel;
        }

        /// <summary>
        /// Actualiza los controles del editor para reflejar el objeto en edición.
        /// </summary>
        private void UpdateEditor()
        {
            if (mEditorItem != null)
            {
                pnlContainer.Show();

                // Grupo General
                txtName.Text = mEditorItem.Name;
                cmbFolder.Text = mEditorItem.Folder;
                cmbSprite.SelectedIndex = cmbSprite.FindString(TextUtils.NullToNone(mEditorItem.Sprite));
                txtSpecies.Text = mEditorItem.Species;
                nudRequiredMaturity.Value = mEditorItem.RequiredMaturity;
                nudRequiredEnergy.Value = mEditorItem.RequiredEnergy;
                nudRequiredMood.Value = mEditorItem.RequiredMood;
                nudMaxBreedCount.Value = mEditorItem.MaxBreedCount;

                // Grupo Stats (suponiendo que PetStats es un arreglo de int con la longitud de Stat)
                nudStr.Value = mEditorItem.PetStats[(int)Stat.Attack];
                nudMag.Value = mEditorItem.PetStats[(int)Stat.AbilityPower];
                nudDef.Value = mEditorItem.PetStats[(int)Stat.Defense];
                nudMR.Value = mEditorItem.PetStats[(int)Stat.MagicResist];
                nudSpd.Value = mEditorItem.PetStats[(int)Stat.Speed];

                // Grupo Combate
                nudDamage.Value = mEditorItem.Damage;
                nudCritChance.Value = mEditorItem.CritChance;
                nudCritMultiplier.Value = (decimal)mEditorItem.CritMultiplier;
                nudScaling.Value = mEditorItem.Scaling;
                cmbDamageType.SelectedIndex = mEditorItem.DamageType;
                cmbScalingStat.SelectedIndex = mEditorItem.ScalingStat;
                cmbAttackAnimation.SelectedIndex = AnimationBase.ListIndex(mEditorItem.AttackAnimationId) + 1;
                cmbAttackSpeedModifier.SelectedIndex = mEditorItem.AttackSpeedModifier;
                nudAttackSpeedValue.Value = mEditorItem.AttackSpeedValue;

                // Grupo Regen
                nudHpRegen.Value = mEditorItem.VitalRegen[(int)Vital.Health];
                nudMpRegen.Value = mEditorItem.VitalRegen[(int)Vital.Mana];

                // Add the spells to the list
                lstSpells.Items.Clear();
                for (var i = 0; i < mEditorItem.Spells.Count; i++)
                {
                    if (mEditorItem.Spells[i] != Guid.Empty)
                    {
                        lstSpells.Items.Add(SpellBase.GetName(mEditorItem.Spells[i]));
                    }
                    else
                    {
                        lstSpells.Items.Add(Strings.General.None);
                    }
                }

                if (lstSpells.Items.Count > 0)
                {
                    lstSpells.SelectedIndex = 0;
                    cmbSpell.SelectedIndex = SpellBase.ListIndex(mEditorItem.Spells[lstSpells.SelectedIndex]);
                }


                // Si se han agregado propiedades de comportamiento de combate (por ejemplo, DefaultBehavior y CombatRange)
                cmbDefaultBehavior.SelectedIndex = (int)mEditorItem.DefaultBehavior;
                nudCombatRange.Value = mEditorItem.SightRange;

                // Agregar el objeto a la lista de cambios si aún no está allí
                if (!mChanged.Contains(mEditorItem))
                {
                    mChanged.Add(mEditorItem);
                    mEditorItem.MakeBackup();
                }
            }
            else
            {
                pnlContainer.Hide();
            }
            UpdateToolStripItems();
        }

        /// <summary>
        /// Inicializa la lista de carpetas y repuebla el control de objetos.
        /// </summary>
        private void InitEditor()
        {
            //Collect folders
            var mFolders = new List<string>();
            foreach (var itm in PetBase.Lookup)
            {
                if (!string.IsNullOrEmpty(((PetBase)itm.Value).Folder) &&
                    !mFolders.Contains(((PetBase)itm.Value).Folder))
                {
                    mFolders.Add(((PetBase)itm.Value).Folder);
                    if (!mKnownFolders.Contains(((PetBase)itm.Value).Folder))
                    {
                        mKnownFolders.Add(((PetBase)itm.Value).Folder);
                    }
                }
            }

            mFolders.Sort();
            mKnownFolders.Sort();
            cmbFolder.Items.Clear();
            cmbFolder.Items.Add("");
            cmbFolder.Items.AddRange(mKnownFolders.ToArray());

            var items = PetBase.Lookup.OrderBy(p => p.Value?.Name).Select(pair => new KeyValuePair<Guid, KeyValuePair<string, string>>(pair.Key,
                new KeyValuePair<string, string>(((PetBase)pair.Value)?.Name ?? Models.DatabaseObject<PetBase>.Deleted, ((PetBase)pair.Value)?.Folder ?? ""))).ToArray();
            lstGameObjects.Repopulate(items, mFolders, btnAlphabetical.Checked, CustomSearch(), txtSearch.Text);
        }

        private bool CustomSearch()
        {
            return !string.IsNullOrWhiteSpace(txtSearch.Text) && txtSearch.Text != Strings.PetEditor.searchplaceholder;
        }

        private void UpdateToolStripItems()
        {
            toolStripItemCopy.Enabled = mEditorItem != null && lstGameObjects.Focused;
            toolStripItemPaste.Enabled = mEditorItem != null && mCopiedItem != null && lstGameObjects.Focused;
            toolStripItemDelete.Enabled = mEditorItem != null && lstGameObjects.Focused;
            toolStripItemUndo.Enabled = mEditorItem != null && lstGameObjects.Focused;
        }

        // Manejadores de eventos para cambios en los controles (ejemplo: txtName, cmbSprite, etc.)
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (mEditorItem != null)
            {
                mEditorItem.Name = txtName.Text;
                lstGameObjects.UpdateText(txtName.Text);
            }
        }

        private void cmbSprite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mEditorItem != null)
            {
                mEditorItem.Sprite = TextUtils.SanitizeNone(cmbSprite.Text);
                DrawPetSprite();
            }
        }

        private void DrawPetSprite()
        {
            // Dibuja el sprite de la mascota en el PictureBox (p. ej. picPet)
            Bitmap picSpriteBmp = new Bitmap(picPet.Width, picPet.Height);
            using (Graphics gfx = Graphics.FromImage(picSpriteBmp))
            {
                gfx.FillRectangle(Brushes.Black, new Rectangle(0, 0, picPet.Width, picPet.Height));
                if (cmbSprite.SelectedIndex > 0)
                {
                    Image img = Image.FromFile("resources/entities/" + cmbSprite.Text);
                    gfx.DrawImage(img, new Rectangle(0, 0, picPet.Width, picPet.Height),
                        0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                    img.Dispose();
                }
            }
            picPet.BackgroundImage = picSpriteBmp;
        }

        // Se agregan los métodos de manejo de carpetas, búsqueda, copiar/pegar y de la ToolStrip
        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            string folderName = "";
            DialogResult result = DarkInputBox.ShowInformation(Strings.PetEditor.folderprompt, Strings.PetEditor.foldertitle, ref folderName, DarkDialogButton.OkCancel);
            if (result == DialogResult.OK && !string.IsNullOrEmpty(folderName))
            {
                if (!cmbFolder.Items.Contains(folderName))
                {
                    mEditorItem.Folder = folderName;
                    lstGameObjects.UpdateText(folderName);
                    InitEditor();
                    cmbFolder.Text = folderName;
                }
            }
        }

        private void cmbFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mEditorItem != null)
            {
                mEditorItem.Folder = cmbFolder.Text;
                InitEditor();
            }
        }

        private void btnAlphabetical_Click(object sender, EventArgs e)
        {
            btnAlphabetical.Checked = !btnAlphabetical.Checked;
            InitEditor();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            InitEditor();
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = Strings.PetEditor.searchplaceholder;
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            txtSearch.SelectAll();
            txtSearch.Focus();
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = Strings.PetEditor.searchplaceholder;
        }

        // Manejadores de la ToolStrip
        private void toolStripItemNew_Click(object sender, EventArgs e)
        {
            PacketSender.SendCreateObject(GameObjectType.Pet);
        }

        private void toolStripItemDelete_Click(object sender, EventArgs e)
        {
            if (mEditorItem != null && lstGameObjects.Focused)
            {
                if (DarkMessageBox.ShowWarning(Strings.PetEditor.deleteprompt, Strings.PetEditor.deletetitle, DarkDialogButton.YesNo, Icon) == DialogResult.Yes)
                {
                    PacketSender.SendDeleteObject(mEditorItem);
                }
            }
        }

        private void toolStripItemCopy_Click(object sender, EventArgs e)
        {
            if (mEditorItem != null && lstGameObjects.Focused)
            {
                mCopiedItem = mEditorItem.JsonData;
                toolStripItemPaste.Enabled = true;
            }
        }

        private void toolStripItemPaste_Click(object sender, EventArgs e)
        {
            if (mEditorItem != null && mCopiedItem != null && lstGameObjects.Focused)
            {
                mEditorItem.Load(mCopiedItem, true);
                UpdateEditor();
            }
        }

        private void toolStripItemUndo_Click(object sender, EventArgs e)
        {
            if (mChanged.Contains(mEditorItem) && mEditorItem != null)
            {
                if (DarkMessageBox.ShowWarning(Strings.PetEditor.undoprompt, Strings.PetEditor.undotitle, DarkDialogButton.YesNo, Icon) == DialogResult.Yes)
                {
                    mEditorItem.RestoreBackup();
                    UpdateEditor();
                }
            }
        }

        // Manejadores para los controles de combate (por ejemplo, el Attack Animation, Damage Type, etc.)
        private void cmbAttackAnimation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mEditorItem != null)
            {
                mEditorItem.AttackAnimation = AnimationBase.Get(AnimationBase.IdFromList(cmbAttackAnimation.SelectedIndex - 1));
            }
        }

        private void cmbDamageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mEditorItem != null)
            {
                mEditorItem.DamageType = cmbDamageType.SelectedIndex;
            }
        }

        private void cmbScalingStat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mEditorItem != null)
            {
                mEditorItem.ScalingStat = cmbScalingStat.SelectedIndex;
            }
        }

        private void lstSpells_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSpells.SelectedIndex > -1)
            {
                cmbSpell.SelectedIndex = SpellBase.ListIndex(mEditorItem.Spells[lstSpells.SelectedIndex]);
            }
        }

        private void cmbSpell_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSpells.SelectedIndex > -1 && lstSpells.SelectedIndex < mEditorItem.Spells.Count)
            {
                mEditorItem.Spells[lstSpells.SelectedIndex] = SpellBase.IdFromList(cmbSpell.SelectedIndex);
            }
            int n = lstSpells.SelectedIndex;
            lstSpells.Items.Clear();
            for (int i = 0; i < mEditorItem.Spells.Count; i++)
            {
                lstSpells.Items.Add(SpellBase.GetName(mEditorItem.Spells[i]));
            }
            lstSpells.SelectedIndex = n;
        }

        private void nudScaling_ValueChanged(object sender, EventArgs e)
        {
            if (mEditorItem != null)
            {
                mEditorItem.Scaling = (int)nudScaling.Value;
            }
        }

     

        private void nudHpRegen_ValueChanged(object sender, EventArgs e)
        {
            if (mEditorItem != null)
            {
                mEditorItem.VitalRegen[(int)Vital.Health] = (int)nudHpRegen.Value;
            }
        }

        private void nudMpRegen_ValueChanged(object sender, EventArgs e)
        {
            if (mEditorItem != null)
            {
                mEditorItem.VitalRegen[(int)Vital.Mana] = (int)nudMpRegen.Value;
            }
        }

        // Se pueden agregar más manejadores para los demás controles (por ejemplo, para las propiedades de reproducción, etc.)

        // Aquí se incluirían también los manejadores de teclas y demás eventos globales.
    }
}
