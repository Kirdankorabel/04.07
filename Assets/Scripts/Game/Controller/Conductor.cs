using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnotGameController
{
    public class Conductor : MonoBehaviour
    {
        public Color color;
        public int leght = 10;
        public Vector3 endPos;
        public List<Segment> segments;
        public int oldConnection;

        private LinkedList<(Vector3, int)> positions = new LinkedList<(Vector3, int)>();
        private Vector3 TargrtPos;

        public ConductorInfo ConductorInfo { get; set; }
        public int PositionsCount => positions.Count;
        public LinkedListNode<(Vector3, int)> LastPos
        {
            get => positions.Last;
            set
            {
                positions.AddLast(value);
            }
        }
        private void Start()
        {
            foreach (var segment in segments)
                segment.Activate += () =>
                    GameController.ActiveConductor = this;
        }

        private void FixedUpdate()
        {
            if (TargrtPos != Vector3.zero)
                segments[leght - 1].transform.position = Vector3.MoveTowards(segments[leght - 1].transform.position, TargrtPos, 0.1f);
        }

        public void Connect(Connection connection)
        {
            if (!connection.isFree)
                return;
            TargrtPos = connection.gameObject.transform.position + Vector3.up;
            oldConnection = ConductorInfo.connectionPoint;
            ConductorInfo.connectionPoint = connection.Number;
            GameController.ActiveConductor = this;
            connection.isFree = false;

            foreach (var cond in GameController.conductors)
            {
                if (cond.ConductorInfo.connectionPoint < oldConnection && cond.ConductorInfo.connectionPoint > ConductorInfo.connectionPoint
                        && ChackIntersections(ConductorInfo, cond.ConductorInfo) != null)
                    ChackIntersections(ConductorInfo, cond.ConductorInfo).UntieKnot();
                else if (cond.ConductorInfo.connectionPoint > oldConnection && cond.ConductorInfo.connectionPoint < ConductorInfo.connectionPoint
                    && ChackIntersections(cond.ConductorInfo, ConductorInfo) != null)
                    ChackIntersections(cond.ConductorInfo, ConductorInfo).UntieKnot();
                else if (cond.ConductorInfo.connectionPoint < oldConnection && cond.ConductorInfo.connectionPoint > ConductorInfo.connectionPoint
                    && ChackIntersections(ConductorInfo, cond.ConductorInfo) == null)
                {
                    bool isDefault = true;
                    if (cond.ConductorInfo.Intersections.Last != null &&
                        ConductorInfo.Intersections.Last.Value.IsBackConductor(ConductorInfo) == cond.ConductorInfo.Intersections.Last.Value.IsBackConductor(cond.ConductorInfo))
                        isDefault = false;
                    new Knot(cond.ConductorInfo, ConductorInfo, isDefault);
                }
                else if (cond.ConductorInfo.connectionPoint > oldConnection && cond.ConductorInfo.connectionPoint < ConductorInfo.connectionPoint
                    && ChackIntersections(cond.ConductorInfo, ConductorInfo) == null)
                {
                    bool isDefault = true;
                    if (cond.ConductorInfo.Intersections.Last != null &&
                        ConductorInfo.Intersections.Last.Value.IsBackConductor(ConductorInfo) == cond.ConductorInfo.Intersections.Last.Value.IsBackConductor(cond.ConductorInfo))
                        isDefault = false;
                    new Knot(ConductorInfo, cond.ConductorInfo, isDefault);
                }
            }

            StartCoroutine(Waiter(() => GameController.Check()));
        }

        public void Connect()
        {
            TargrtPos = GameController.connections[ConductorInfo.connectionPoint].gameObject.transform.position + Vector3.up;
            GameController.connections[ConductorInfo.connectionPoint].isFree = false;
        }

        public void Unconnect()
        {
            if (ConductorInfo.Intersections.Count == 0 || (ConductorInfo.Intersections.Last.Value.IsBackConductor(ConductorInfo)
                && ConductorInfo.Intersections.Last.Value.rightConductor == ConductorInfo
                && (ConductorInfo.Intersections.Count > 1 && !ConductorInfo.Intersections.Last.Previous.Value.IsBackConductor(ConductorInfo))))
                return;
            var value = Vector3.back;

            if (ConductorInfo.Intersections.Last.Value.IsBackConductor(ConductorInfo))
                value = -value;
            TargrtPos = GameController.connections[ConductorInfo.connectionPoint].gameObject.transform.position + 2 * Vector3.up + 2f * value;
            GameController.connections[ConductorInfo.connectionPoint].isFree = true;
        }

        private Knot ChackIntersections(ConductorInfo LeftConductor, ConductorInfo RightConductor)
        {
            Knot result = null;
            foreach (var knot in ConductorInfo.Intersections)
                if (knot.leftConductor == LeftConductor && knot.rightConductor == RightConductor)
                    result = knot;
            return result;
        }

        private IEnumerator Waiter(Action action)
        {
            while (true)
            {
                yield return new WaitForSeconds(3f);
                action.Invoke();
            }
        }
    }
}