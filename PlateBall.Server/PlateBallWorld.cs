using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace PlateBall.Server
{
    class PlateBallWorld : World
    {
        public Body Ball { get; }
        public PlateBallWorld(Vector2 gravity, Server server) : base(gravity)
        {
            Ball = BodyFactory.CreateCircle(this, 0.1f, 0.2f, new Vector2(1, 1));
            Ball.Move(new Vector2(0.3f, 0.1f));
            Ball.BodyType = BodyType.Dynamic;
            Ball.CollidesWith = Category.All;
            Ball.CollisionCategories = Category.All;
            //Ball.Restitution = 1.01f;
            Ball.Restitution = 5f;

            BodyFactory.CreateEdge(this, new Vector2((float)900 / 100, 0), new Vector2((float)900 / 100, (float)900 / 100));
            BodyFactory.CreateEdge(this, new Vector2(0, 0), new Vector2(0, (float)900 / 100));
        }
        public void Update(double gameTime)
        {
            this.Step(Math.Min((float)gameTime, (1f / 30f)));
        }
    }
}
