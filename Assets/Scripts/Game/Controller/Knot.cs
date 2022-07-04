using System;
using UnityEngine;
using Model;

namespace KnotGameController
{
    [Serializable]
    public class Knot
    {
        public ConductorInfo leftConductor;
        public ConductorInfo rightConductor;
        public Vector3 position;
        public int posInt;
        public ConductorInfo topConductor;

        public Knot(ConductorInfo LeftConductor, ConductorInfo RightConductor, bool isDefault)
        {
            leftConductor = LeftConductor;
            rightConductor = RightConductor;
            topConductor = leftConductor;
            GameController.Nodes.Add(this);
            leftConductor.Intersections.AddLast(this);
            rightConductor.Intersections.AddLast(this);
            if (!isDefault)
                topConductor = rightConductor;
        }

        public Knot(int leftConductorNumber, int rightConductorNumber, int PosInt)
        {
            leftConductor = GameController.Conductors[leftConductorNumber];
            rightConductor = GameController.Conductors[rightConductorNumber];
            topConductor = leftConductor;
            GameController.Nodes.Add(this);
            posInt = PosInt;
            leftConductor.Intersections.AddLast(this);
            rightConductor.Intersections.AddLast(this);
        }

        public void UntieKnot()
        {
            rightConductor.Intersections.Remove(this);
            leftConductor.Intersections.Remove(this);
            GameController.Nodes.Remove(this);
        }

        public bool IsBackConductor(ConductorInfo conductorInfo)
        {
            if (topConductor == conductorInfo)
                return true;
            return false;
        }
    }
}
