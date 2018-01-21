using FarseerPhysics.Dynamics;

namespace PlateBall.Client.Player
{
    public class Player
    {
        public string Name { get; }
        private GameWorld _world;
        public bool StartTheGame { get; set; }
        public Player(string name, GameWorld world)
        {
            Name = name;
            _world = world;
        }

    }
}
