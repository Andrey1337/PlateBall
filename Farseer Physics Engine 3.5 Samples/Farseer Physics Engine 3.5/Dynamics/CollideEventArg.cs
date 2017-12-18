using System;
using System.Collections.Generic;

namespace FarseerPhysics.Dynamics
{
    public class CollideEventArg : EventArgs
    {
        public Dictionary<int, HashSet<Body>> Dictionary;

        public CollideEventArg(Dictionary<int, HashSet<Body>> dictionary)
        {
            Dictionary = dictionary;
        }
    }
}