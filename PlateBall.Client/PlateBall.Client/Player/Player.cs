using FarseerPhysics.Dynamics;

namespace PlateBall.Client.Player
{
    class Player
    {
        public string Name { get; }
        private PlateBallWorld _world;
        public bool StartTheGame { get; set; }
        public Player(string name, PlateBallWorld world)
        {
            Name = name;
            _world = world;
        }

    }
}
