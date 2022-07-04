using System;
using System.Collections.Generic;
using KnotGameController;

namespace Model
{
    [Serializable]
    public class ConductorInfo
    {
        public int startConnectionPoint;
        public int connectionPoint;
        private LinkedList<Knot> _intersections;

        public LinkedList<Knot> Intersections
        {
            get
            {
                if (_intersections == null)
                    _intersections = new LinkedList<Knot>();
                return _intersections;
            }
        }

        public ConductorInfo(int StartConnectionPoint, int ConnectionPoint)
        {
            startConnectionPoint = StartConnectionPoint;
            connectionPoint = ConnectionPoint;
            _intersections = new LinkedList<Knot>();
        }

        public ConductorInfo() { }
    }
}