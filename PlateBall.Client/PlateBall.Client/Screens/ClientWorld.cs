using FarseerPhysics.Samples.DrawingSystem;
using FarseerPhysics.Samples.ScreenSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlateBall.Client.Screens
{
    public class ClientWorld
    {
        private Texture2D _ballTexture2D;

        public ClientWorld(GameScreen gameScreen)
        {

        }

        public void Load(ContentManager content)
        {
            _ballTexture2D = content.Load<Texture2D>("index");
        }
    }
}