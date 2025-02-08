using Intersect.Client.Core;
using Intersect.Client.Entities;
using Intersect.Client.Framework.File_Management;
using Intersect.Client.Framework.Gwen;
using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.General;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Enums;
using Intersect.GameObjects;

namespace Intersect.Client.Interface.Game.Pets
{
    public class PetWindow
    {
        private WindowControl mPetWindow;
        private Label mPetName;
        private Label mPetLevel;
        private Label mPetGender;
        private Label mPetEnergy;
        private Label mPetMood;
        private Label mPetPersonality;
        private Label mPetRarity;
        private Button mSummonButton;
        private Button mFeedButton;
        private Button mPlayButton;
        private Button mReleaseButton;
        private Button mBreedButton;
        private ImagePanel mPetIcon;
        private ImagePanel mStatsPanel;
        private Label mPetStats;
        private Label mPetVitals;

        public PetWindow(Canvas gameCanvas)
        {
            mPetWindow = new WindowControl(gameCanvas, Strings.Pets.WindowTitle, false, "PetWindow");
            mPetWindow.SetSize(400, 400);
            mPetWindow.SetPosition(50, 50);
            mPetWindow.DisableResizing();

            // Panel Izquierdo
            mPetName = new Label(mPetWindow, "PetNameLabel") { X = 20, Y = 30, Width = 260, Height = 20 };
            mPetLevel = new Label(mPetWindow, "PetLevelLabel") { X = 20, Y = 60, Width = 260, Height = 20 };
            mPetGender = new Label(mPetWindow, "PetGenderLabel") { X = 20, Y = 90, Width = 260, Height = 20 };
            mPetEnergy = new Label(mPetWindow, "PetEnergyLabel") { X = 20, Y = 120, Width = 260, Height = 20 };
            mPetMood = new Label(mPetWindow, "PetMoodLabel") { X = 20, Y = 150, Width = 260, Height = 20 };
            mPetPersonality = new Label(mPetWindow, "PetPersonalityLabel") { X = 20, Y = 180, Width = 260, Height = 20 };
            mPetRarity = new Label(mPetWindow, "PetRarityLabel") { X = 20, Y = 210, Width = 260, Height = 20 };

            mPetIcon = new ImagePanel(mPetWindow, "PetIconPanel") { X = 110, Y = 240, Width = 80, Height = 80 };

            // Panel Derecho - Stats y Vitals
            mStatsPanel = new ImagePanel(mPetWindow, "StatsPanel") { X = 300, Y = 30, Width = 100, Height = 300 };
            mPetStats = new Label(mStatsPanel, "PetStatsLabel") { X = 10, Y = 10, Width = 80, Height = 20 };
            mPetVitals = new Label(mStatsPanel, "PetVitalsLabel") { X = 10, Y = 40, Width = 80, Height = 20 };

            mSummonButton = new Button(mPetWindow, "SummonPetButton") { X = 20, Y = 330, Width = 260, Height = 30 };
            mSummonButton.Text = Strings.Pets.Summon;
            mSummonButton.Clicked += (sender, args) => ToggleSummonPet();

            mFeedButton = new Button(mPetWindow, "FeedPetButton") { X = 20, Y = 370, Width = 125, Height = 30 };
            mFeedButton.Text = Strings.Pets.Feed;
            mFeedButton.Clicked += (sender, args) => FeedPet();

            mPlayButton = new Button(mPetWindow, "PlayPetButton") { X = 155, Y = 370, Width = 125, Height = 30 };
            mPlayButton.Text = Strings.Pets.Play;
            mPlayButton.Clicked += (sender, args) => PlayWithPet();

            mReleaseButton = new Button(mPetWindow, "ReleasePetButton") { X = 20, Y = 410, Width = 125, Height = 30 };
            mReleaseButton.Text = Strings.Pets.Release;
            mReleaseButton.Clicked += (sender, args) => ReleasePet();

            mBreedButton = new Button(mPetWindow, "BreedPetButton") { X = 155, Y = 410, Width = 125, Height = 30 };
            mBreedButton.Text = Strings.Pets.Breed;
            mBreedButton.Clicked += (sender, args) => BreedPet();

            mPetWindow.LoadJsonUi(GameContentManager.UI.InGame, Graphics.Renderer.GetResolutionString());
        }
        public void Update()
        {
            var pet = Globals.Me.EquippedPet;
            if (pet == null)
            {
                mPetWindow.IsHidden = true;
                return;
            }

            mPetName.SetText(pet.Name);
            mPetLevel.SetText(Strings.Pets.Level.ToString(pet.Level));
            mPetGender.SetText(pet.Gender == Gender.Male ? Strings.Pets.Male : Strings.Pets.Female);
            mPetEnergy.SetText(Strings.Pets.Energy.ToString(pet.Hunger));
            mPetMood.SetText(Strings.Pets.Mood.ToString(pet.Mood));
            mPetPersonality.SetText(Strings.Pets.Personality.ToString(pet.Personality));
            mPetRarity.SetText(Strings.Pets.Rarity.ToString(pet.Rarity));
            mPetStats.SetText("Stats: " + pet.GetStatsSummary());
            mPetVitals.SetText("Vitals: " + pet.GetVitalsSummary());

            mSummonButton.Text = pet.IsSummoned ? Strings.Pets.Return : Strings.Pets.Summon;
            mBreedButton.IsHidden = !CanBreed(pet);
        }

        private void ToggleSummonPet()
        {
            PacketSender.SendTogglePetSummon();
        }

        private void FeedPet()
        {
            PacketSender.SendFeedPet();
        }

        private void PlayWithPet()
        {
            PacketSender.SendPlayWithPet();
        }

        private void ReleasePet()
        {
            PacketSender.SendReleasePet();
        }

        private void BreedPet()
        {
            PacketSender.SendBreedPet();
        }

        private bool CanBreed(Pet pet)
        {
            return pet.Maturity >= 100 && pet.BreedCount < 10;
        }

        public void Show()
        {
            mPetWindow.IsHidden = false;
        }

        public void Hide()
        {
            mPetWindow.IsHidden = true;
        }
    }
}
