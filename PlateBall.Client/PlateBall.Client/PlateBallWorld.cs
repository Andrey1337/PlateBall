using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Samples.DrawingSystem;
using FarseerPhysics.Samples.ScreenSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PlateBall.Client.Screens;

namespace PlateBall.Client
{
    class PlateBallWorld : World
    {
        private readonly PlateBallGameScreen _gameScreen;
        public Body Ball { get; }
        private Sprite _ballSprite;

        public PlateBallWorld(Vector2 gravity, PlateBallGameScreen gameScreen) : base(gravity)
        {
            _gameScreen = gameScreen;
            Ball = BodyFactory.CreateCircle(this, 0.3f, 0.2f, new Vector2(1, 1));
            Ball.Move(new Vector2(2f, 0f));
            Ball.BodyType = BodyType.Dynamic;
            Ball.Restitution = 1.01f;

            _ballSprite = new Sprite(_gameScreen.ScreenManager.Assets.TextureFromShape(Ball.FixtureList[0].Shape, MaterialType.Waves, Color.Brown, 1f));
            //border    
            BodyFactory.CreateEdge(this, new Vector2((float)900 / 100, 0), new Vector2((float)900 / 100, (float)900 / 100));
            BodyFactory.CreateEdge(this, new Vector2((float)0, 0), new Vector2(0, (float)900 / 100));
        }

        public void Load(ContentManager contentManager)
        {

        }

        public void Update(GameTime gameTime)
        {
            //if (!_gameScreen.IsGameStarted) return;
            Debug.WriteLine(gameTime.ElapsedGameTime);
            this.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_ballSprite.Texture, ConvertUnits.ToDisplayUnits(Ball.Position),
                null, Color.White, Ball.Rotation, _ballSprite.Origin, 1f, SpriteEffects.None, 0f);
        }

    }
}
