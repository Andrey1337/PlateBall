using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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

        private Client _client;

        public GameWorld GameWorld;

        private Server.Server _server;
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
            GameWorld = new GameWorld(this);
            GameWorld.Load(_contentManager);

            _server = new Server.Server(11000);

            Task connectThread = new Task(() =>
            {
                _client = new Client("Andrey", 64064, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000), this);
                _client.ConnectRequest();
            });
            connectThread.Start();
        }

        public override void HandleInput(InputHelper input, GameTime gameTime)
        {
            if (input.KeyboardState.IsKeyDown(Keys.Escape))
            {
                _client.Exit();
                _server.Exit();

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
