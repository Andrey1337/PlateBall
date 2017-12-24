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
    public class PlateBallGameScreen : GameScreen
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
            GameWorld = new PlateBallWorld(this);

            Server.Server server = new Server.Server(11000);
            server.StartListen();

            Thread connectThread = new Thread(() =>
            {
                Client client1 = new Client("Andrey", 64064,
                    new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000));
                client1.Connect();

                Thread.Sleep(1000);
                client1.StartGame();
            });
            connectThread.Start();


            GameWorld.Load(_contentManager);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(InputHelper input, GameTime gameTime)
        {
            if (input.KeyboardState.IsKeyDown(Keys.Escape))
            {
                Server.UdpServer.Close();
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
