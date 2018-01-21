using FarseerPhysics.Samples.Demos;
using FarseerPhysics.Samples.ScreenSystem;
using Microsoft.Xna.Framework;
using PlateBall.Client.Screens;

namespace PlateBall.Client
{
    public class PlateBallGame : Game
    {
        public ScreenManager ScreenManager { get; set; }
        private readonly GraphicsDeviceManager _graphics;

        public PlateBallGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 900;
            _graphics.PreferredBackBufferHeight = 900;

            ScreenManager = new ScreenManager(this);
            Components.Add(ScreenManager);

            FrameRateCounter frameRateCounter = new FrameRateCounter(ScreenManager);
            frameRateCounter.DrawOrder = 101;
            Components.Add(frameRateCounter);
        }

        protected override void Initialize()
        {
            MenuScreen menuScreen = new MenuScreen("Plate Ball");

            menuScreen.AddMenuItem("", EntryType.Separator, null);
            menuScreen.AddMenuItem("Host game", EntryType.Screen, new PlateBallGameScreen(Content, _graphics));
            menuScreen.AddMenuItem("ConnectRequest to game", EntryType.Screen, new SimpleDemo8());
            menuScreen.AddMenuItem("", EntryType.Separator, null);
            menuScreen.AddMenuItem("Exit", EntryType.ExitItem, null);


            ScreenManager.AddScreen(new BackgroundScreen());
            ScreenManager.AddScreen(menuScreen);


            base.Initialize();
        }
    }
}
