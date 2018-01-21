using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Samples.DrawingSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlateBall.Client.Screens;
using PlateBall.Server.PackageFormat;

namespace PlateBall.Client
{
    public class GameWorld
    {
        private readonly PlateBallGameScreen _gameScreen;
        private Texture2D _ballTexture;

        public GameStatePackage GameInfo { get; set; }

        public GameWorld(PlateBallGameScreen gameScreen)
        {
            _gameScreen = gameScreen;
        }

        public void Load(ContentManager content)
        {
            _ballTexture = content.Load<Texture2D>("index");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (GameInfo == null)
                return;
            spriteBatch.Draw(_ballTexture, GameInfo.BallPosition, Color.White);

            //Debug.WriteLine(GameInfo.BallPosition);
        }
    }
}
