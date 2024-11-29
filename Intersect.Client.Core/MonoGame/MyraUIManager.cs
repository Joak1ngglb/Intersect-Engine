using Intersect.Client.General;
using Intersect.Logging;
using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.ColorPicker;

namespace Intersect.Client.MonoGame
{
    public class MyraUIManager
    {
        public Desktop Desktop { get; private set; }

        public MyraUIManager(Game game)
        {
            MyraEnvironment.Game = game;
            Desktop = new Desktop();
        }

        public void LoadUI(string filePath)
        {
            // Read the MML file
            string data = File.ReadAllText(filePath);

            // Load the project from the MML data
            Project project = Project.LoadFromXml(data);

            // Set the root widget of the desktop to the root of the project
            Desktop.Root = project.Root;

            // Attach event handlers based on the file being loaded
            switch (Path.GetFileNameWithoutExtension(filePath))
            {
                case "MainMenu":
                    AttachMainMenuEventHandlers(project);
                    break;
                case "Settings":
                    AttachSettingsEventHandlers(project);
                    break;
                // Add more cases here as needed
            }
        }
        
        private void AttachMainMenuEventHandlers(Project project)
        {
            // Find the username field by its ID and attach event handlers
            var usernameField = (TextBox)project.Root.FindChildById("usernameField");
            if (usernameField != null)
            {
                //usernameField.SubmitPressed += (s, e) => TryLogin();
                //usernameField.PasswordField = true;
            }

            // Find the password field by its ID and attach event handlers
            var passwordField = (TextBox)project.Root.FindChildById("passwordField");
            if (passwordField != null)
            {
                passwordField.PasswordField = true;
            }
            
            // Find the forgot password button by its ID and attach an event handler
            var forgotPasswordButton = (TextButton)project.Root.FindChildById("forgotPasswordButton");
            if (forgotPasswordButton != null)
            {
                forgotPasswordButton.Click += (s, a) =>
                {
                    // Open forgot password window logic here
                };
            }
            
            // Find the save password checkbox by its ID
            var savePassCheckbox = (CheckButton)project.Root.FindChildById("savePassCheckButton");
            if (savePassCheckbox != null)
            {
                // Attach any necessary event handlers here
            }
            
            // Find the login button by its ID and attach an event handler
            var loginButton = (TextButton)project.Root.FindChildById("loginButton");
            if (loginButton != null)
            {
                loginButton.Click += (s, a) =>
                {
                    // Unload the current UI and load the login window UI
                    // try login > LoadUI("resources/interface/menu/CharacterSelect.xmmp");
                };
            }

            // Find the register button by its ID and attach an event handler
            var registerButton = (TextButton)project.Root.FindChildById("registerButton");
            if (registerButton != null)
            {
                registerButton.Click += (s, a) =>
                {
                    // Open register window logic here
                };
            }

            // Find the settings button by its ID and attach an event handler
            var settingsButton = (TextButton)project.Root.FindChildById("settingsButton");
            if (settingsButton != null)
            {
                settingsButton.Click += (s, a) =>
                {
                    // Open settings window logic here
                    LoadUI("resources/interface/menu/Settings.xmmp");
                };
            }
            
            // Find the credits button by its ID and attach an event handler
            var creditsButton = (TextButton)project.Root.FindChildById("creditsButton");
            if (creditsButton != null)
            {
                creditsButton.Click += (s, a) =>
                {
                    // Load the credits window UI
                    LoadUI("resources/interface/menu/Credits.xmmp");
                };
            }

            // Find the exit button by its ID and attach an event handler
            var exitButton = (TextButton)project.Root.FindChildById("exitButton");
            if (exitButton != null)
            {
                exitButton.Click += (s, a) =>
                {
                    MyraEnvironment.Game.Exit();
                    Log.Info("User clicked exit button.");
                    Globals.IsRunning = false;
                };
            }
        }

        private void AttachSettingsEventHandlers(Project project)
        {
            // Attach event handler for color button
            var backgroundColorButton = (TextButton)project.Root.FindChildById("backgroundColorPickerButton");
            if (backgroundColorButton != null)
            {
                backgroundColorButton.Click += (sender, args) =>
                {
                    // Open color picker dialog and change the Myra UI color
                    var colorDialog = new ColorPickerDialog();
                    colorDialog.ColorPickerPanel.TouchUp += (s, e) =>
                    {
                        // Update the color of the root widget in real time
                        project.Root.Background = new SolidBrush(colorDialog.Color);
                        //project.Root.Desktop.Background = new SolidBrush(colorDialog.Color); weird
                    };
                    colorDialog.ShowModal(project.Root.Desktop);
                };
            }
            var borderColorButton = (TextButton)project.Root.FindChildById("borderColorPickerButton");
            if (borderColorButton != null)
            {
                borderColorButton.Click += (sender, args) =>
                {
                    // Open color picker dialog and change the Myra UI color
                    var colorDialog = new ColorPickerDialog();
                    colorDialog.ColorPickerPanel.TouchUp += (s, e) =>
                    {
                        // Update the color of the root widget in real time
                        project.Root.Border = new SolidBrush(colorDialog.Color);
                    };
                    colorDialog.ShowModal(project.Root.Desktop);
                };
            }
            
            var borderThicknessSlider = (HorizontalSlider)project.Root.FindChildById("borderThicknessButton");
            if (borderThicknessSlider != null)
            {
                borderThicknessSlider.MouseMoved += (sender, args) =>
                {
                    // 
                    project.Root.BorderThickness = new Thickness((int)borderThicknessSlider.Value);
                };
            }
        }
        

        public void Render()
        {
            Desktop.Render();
        }
    }
}