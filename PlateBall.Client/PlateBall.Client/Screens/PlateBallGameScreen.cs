using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using FarseerPhysics.Samples.ScreenSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;


namespace PlateBall.Client.Screens
{
    class PlateBallGameScreen : GameScreen
    {
        private readonly ContentManager _contentManager;
        public readonly GraphicsDeviceManager Graphics;

        public PlateBallWorld GameWorld;

        public Player.Player Player1 { get; set; }
        public Player.Player Player2 { get; set; }
        public Server.Server Server { get; set; }

        public bool IsGameStarted { get; }
        public PlateBallGameScreen(ContentManager contentManager, GraphicsDeviceManager graphics)
        {
            _contentManager = contentManager;
            Graphics = graphics;
            HasCursor = true;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            Server.Server server = new Server.Server(11000);
            server.StartListen();

            Thread connectThread = new Thread(() =>
            {
                Server.Client client1 = new Server.Client("Andrey", 64064,
                    new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000));
                client1.Connect();

                Thread.Sleep(1000);
                client1.StartGame();
            });
            connectThread.Start();             

            GameWorld = new PlateBallWorld(new Vector2(0, 0), this);
            GameWorld.Load(_contentManager);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            GameWorld.Update(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(InputHelper input, GameTime gameTime)
        {
            if (input.KeyboardState.IsKeyDown(Keys.Escape))
            {
                Server.udpServer.Close();
                ExitScreen();
            }
            base.HandleInput(input, gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            Graphics.GraphicsDevice.Clear(Color.White);
            GameWorld.Draw(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
