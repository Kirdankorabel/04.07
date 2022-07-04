using System;
using System.Collections.Generic;
using KnotGameController;

namespace Model
{
    [Serializable]
    public class LevelInfo
    {
        public int startConnectionCount;
        public int connectionCount;
        public List<ConductorInfo> conductors = new List<ConductorInfo>();
        public List<KnotInfo> knotsInfo = new List<KnotInfo>();
        [NonSerialized] public List<Knot> knots = new List<Knot>();
    }
}
