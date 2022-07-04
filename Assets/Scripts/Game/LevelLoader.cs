using System.Collections.Generic;
using UnityEngine;
using Model;

namespace KnotGameController
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private Connection startConnectionPrefab;
        [SerializeField] private Connection ConnectionPrefab;
        [SerializeField] private Conductor ConductorPrefab;
        [SerializeField] private List<Color> colors;
        [SerializeField] private Segment segment1Prefab;
        [SerializeField] private Segment2 segment2Prefab;
        [SerializeField] private Segment connectionSegmentPrefab;

        private void Awake()
        {
            GameController.levelLoader = this;
            GameController.Start();
        }

        private void Start()
        {
            foreach (var conductor in GameController.conductors)
                CreateConductor(conductor);
            foreach (var node in GameController.Nodes)
                InitNode(node);
            foreach (var conductor in GameController.conductors)
                InstantiateConductor(conductor);
        }

        public void LoadLevel(LevelInfo levelInfo)
        {
            GameController.startConnections = new Connection[levelInfo.startConnectionCount];
            GameController.connections = new Connection[levelInfo.connectionCount];
            GameController.conductors = new List<Conductor>();
            levelInfo.knots = new List<Knot>();

            for (var i = 0; i < levelInfo.startConnectionCount; i++)
            {
                var connection = Instantiate(startConnectionPrefab, new Vector3(i - levelInfo.startConnectionCount / 2f + 2.5f, 5, 0), Quaternion.identity);
                GameController.startConnections[i] = connection;
            }
            for (var i = 0; i < levelInfo.connectionCount; i++)
            {
                var connection = Instantiate(ConnectionPrefab, new Vector3((i - levelInfo.connectionCount / 2f + 2.5f), -4, 0), Quaternion.identity);
                GameController.connections[i] = connection;
                connection.Number = i;
            }

            var number = 0;
            foreach (var conductor in levelInfo.conductors)
                LoadConductor(conductor, ref number);

            foreach (var knot in levelInfo.knotsInfo)
                new Knot(knot.leftConductorNumber, knot.rightConductorNumber, knot.posInt);

        }
        private void InitNode(Knot knot)
        {
            var leftConductor = GameController.GetConductor(knot.leftConductor.startConnectionPoint);
            var rightConductor = GameController.GetConductor(knot.rightConductor.startConnectionPoint);
            var posInt = knot.posInt;
            knot.position = (leftConductor.segments[posInt].transform.position + rightConductor.segments[posInt].transform.position) / 2;
            knot.position = new Vector3(((leftConductor.LastPos.Value.Item1 + rightConductor.LastPos.Value.Item1).x / 2), knot.position.y, knot.position.z);

            leftConductor.LastPos = new LinkedListNode<(Vector3, int)>((knot.position - Vector3.left / 4 + Vector3.forward / 2, posInt - 2));
            leftConductor.LastPos = new LinkedListNode<(Vector3, int)>((knot.position - Vector3.left / 4 - Vector3.forward / 2, posInt + 2));
            rightConductor.LastPos = new LinkedListNode<(Vector3, int)>((knot.position - Vector3.right / 4 - Vector3.forward / 2, posInt - 2));
            rightConductor.LastPos = new LinkedListNode<(Vector3, int)>((knot.position - Vector3.right / 4 + Vector3.forward / 2, posInt + 2));
        }

        private void LoadConductor(ConductorInfo conductorInfo, ref int number)
        {
            var c = Instantiate(ConductorPrefab);

            GameController.conductors.Add(c);
            c.ConductorInfo = conductorInfo;
            c.color = colors[number];
            c.LastPos = new LinkedListNode<(Vector3, int)>((GameController.startConnections[conductorInfo.startConnectionPoint].gameObject.transform.position + Vector3.down, 0));
            number++;
        }

        private void CreateConductor(Conductor conductor)
        {
            conductor.segments = new List<Segment>();
            var startPos = GameController.startConnections[conductor.ConductorInfo.startConnectionPoint].gameObject.transform.position + Vector3.down;
            conductor.endPos = GameController.connections[conductor.ConductorInfo.connectionPoint].gameObject.transform.position + Vector3.up;
            GameController.connections[conductor.ConductorInfo.connectionPoint].isFree = false;
            Vector3 step = (startPos - conductor.endPos) / conductor.leght;

            var firstSegment = Instantiate(connectionSegmentPrefab, startPos + Vector3.up / 2, Quaternion.identity, transform);
            conductor.segments.Add(firstSegment);

            for (var i = 1; i < conductor.leght - 1; i++)
            {
                var segment = Instantiate(segment1Prefab, startPos - step * i, Quaternion.identity, transform);
                conductor.segments.Add(segment);
            }
            var lastSegment = Instantiate(connectionSegmentPrefab, startPos - step * (conductor.leght - 1) + Vector3.down / 2, Quaternion.identity, transform);
            conductor.segments.Add(lastSegment);
            for (var i = 0; i < conductor.leght; i++)
            {
                var segment = conductor.segments[i];
                segment.transform.rotation = Quaternion.identity;
                segment.SetColor = conductor.color;
                segment.gameObject.name = i.ToString();
                SetConnect(conductor, i, segment);

                if (i > 1)
                {
                    var segment2 = Instantiate(segment2Prefab);
                    segment2.SetPoints(conductor.segments[i - 1].transform, conductor.segments[i].transform);
                    segment2.SetColor = conductor.color;
                }
            }
        }

        private void InstantiateConductor(Conductor conductor)
        {
            var endPos = conductor.endPos;
            var endSegment = conductor.leght - 1;
            var node = conductor.LastPos;
            for (var i = 0; i < conductor.PositionsCount; i++)
            {
                CaclulatePositions(conductor, node.Value.Item1, endPos, node.Value.Item2, endSegment);
                endPos = node.Value.Item1;
                endSegment = node.Value.Item2;
                node = node.Previous;
            }

            if (conductor.ConductorInfo.Intersections.Last.Value.leftConductor.connectionPoint <
                conductor.ConductorInfo.Intersections.Last.Value.rightConductor.connectionPoint)
                new Knot(conductor.ConductorInfo.Intersections.Last.Value.rightConductor,
                    conductor.ConductorInfo.Intersections.Last.Value.leftConductor, true);

            if (conductor.ConductorInfo.Intersections.First.Value.rightConductor.startConnectionPoint <
                conductor.ConductorInfo.Intersections.First.Value.leftConductor.startConnectionPoint)
                conductor.ConductorInfo.Intersections.AddFirst(
                    new Knot(conductor.ConductorInfo.Intersections.First.Value.leftConductor,
                    conductor.ConductorInfo.Intersections.First.Value.rightConductor, true));
        }

        private void CaclulatePositions(Conductor conductor, Vector3 startPos, Vector3 endPos, int start, int end)
        {
            var step = -(endPos - startPos) / (float)(end - start);
            for (var i = start; i < end; i++)
                conductor.segments[i].transform.position = startPos + (start - i) * step;
        }

        private void SetConnect(Conductor conductor, int number, Segment segment)
        {
            if (number == 0)
                segment.Rigidbody.isKinematic = true;
            else if (number > 0 && number < conductor.leght - 1)
                conductor.segments[number - 1].SetConnectedBody(segment.Rigidbody);
            else if (number == conductor.leght - 1)
            {
                conductor.segments[number - 1].SetConnectedBody(segment.Rigidbody);
                segment.SetConnectedBody(GameController.connections[conductor.ConductorInfo.connectionPoint].gameObject.GetComponent<Rigidbody>());
                segment.Rigidbody.isKinematic = true;
            }
        }
    }
}