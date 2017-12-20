﻿using System;
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

namespace PlateBall.Server
{
    class PlateBallWorld : World
    {
        public Body Ball { get; }
        private Server _server;
        public PlateBallWorld(Vector2 gravity, Server server) : base(gravity)
        {
            _server = server;
            Ball = BodyFactory.CreateCircle(this, 0.3f, 0.2f, new Vector2(1, 1));
            Ball.Move(new Vector2(1f, 3f));
            Ball.BodyType = BodyType.Dynamic;
            Ball.Restitution = 1.01f;

            BodyFactory.CreateEdge(this, new Vector2((float)900 / 100, 0), new Vector2((float)900 / 100, (float)900 / 100));
            BodyFactory.CreateEdge(this, new Vector2(0, 0), new Vector2(0, (float)900 / 100));
        }
        public void Update(GameTime gameTime)
        {
            if (!_server.IsReady)
                return;
            this.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
        }
    }
}