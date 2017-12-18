using System;
using System.Collections.Generic;

namespace FarseerPhysics.Dynamics
{
    public class NearbyEventArg : EventArgs
    {
        public Dictionary<int, HashSet<Body>> Dictionary { get; }
        public NearbyEventArg(Dictionary<int, HashSet<Body>> dictionary)
        {
            Dictionary = dictionary;
        }
    }
}