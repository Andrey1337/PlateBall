using Microsoft.Xna.Framework;

namespace PlateBall.Server.PackageFormat
{
    public class GameStatePackage
    {
        public Vector2 BallPosition { get; set; }
        //public int Plate1Position { get; set; }
        //public int Plate2Position { get; set; }
        public GameStatePackage(Vector2 ballPosition)
        {
            BallPosition = ballPosition;
        }
    }
}