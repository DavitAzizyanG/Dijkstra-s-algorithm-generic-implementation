using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphShortestPath
{

    internal class NodeConnection
    {
        public Node Target { get; set; }
        public double Distance { get; set; }
        public NodeConnection(Node target, double dist)
        {
            Target = target;
            Distance = dist;
        }
    }
    internal class Node
    {
        IList<NodeConnection> _connections;
        internal string Name { get; private set; }
        internal double DistanceFromStart { get; set; }
        internal string NearestVertex { get; set; }
        internal IEnumerable<NodeConnection> Connections
        {
            get { return _connections; }
        }
        internal void AddConnection(Node targetNode, double distance, bool twoWay)
        {
            if (targetNode == null) throw new ArgumentNullException("targetNode");
            if (targetNode == this)
                throw new ArgumentException("Node may not connect to itself.");
            if (distance <= 0) throw new ArgumentException("Distance must be positive.");
            _connections.Add(new NodeConnection(targetNode, distance));
            if (twoWay) targetNode.AddConnection(this, distance, false);
        }
        public Node(string name)
        {
            Name = name;
            _connections = new List<NodeConnection>();
        }
    }

    public class Graph
    {
        internal IDictionary<string, Node> Nodes { get; private set; }
        public Graph()
        {
            Nodes = new Dictionary<string, Node>();
        }
        public void AddNode(string name)
        {
            var node = new Node(name);
            Nodes.Add(name, node);
        }
        public void AddConnection(string fromNode, string toNode, int distance, bool twoWay)
        {
            Nodes[fromNode].AddConnection(Nodes[toNode], distance, twoWay);
        }
        public Stack<string> ExtractShortestPath(string startVertex, string endVertex)
        {
            Stack<string> path = new Stack<string>();
            string lastVertex = endVertex;
            Node lastNode = Nodes[lastVertex];
            path.Push(lastVertex);
            while (lastVertex != startVertex)
            {
                lastNode = Nodes[lastNode.NearestVertex];
                lastVertex = lastNode.Name;
                path.Push(lastVertex);
            }
            return path;
        }
    }
    public class DistanceCalculator
    {
        public IDictionary<string, double> CalculateDistances(Graph graph, string startingNode)
        {
            if (!graph.Nodes.Any(n => n.Key == startingNode))
                throw new ArgumentException("Starting node must be in graph.");
            InitialiseGraph(graph, startingNode);
            ProcessGraph(graph, startingNode);
            return ExtractDistances(graph);
        }
        private void InitialiseGraph(Graph graph, string startingNode)
        {
            foreach (Node node in graph.Nodes.Values)
                node.DistanceFromStart = double.PositiveInfinity;
            graph.Nodes[startingNode].DistanceFromStart = 0;
        }
        private IDictionary<string, double> ExtractDistances(Graph graph)
        {
            return graph.Nodes.ToDictionary(n => n.Key, n => n.Value.DistanceFromStart);
        }
        private void ProcessGraph(Graph graph, string startingNode)
        {
            bool finished = false;
            var queue = graph.Nodes.Values.ToList();
            while (!finished)
            {
                Node nextNode = queue.OrderBy(n => n.DistanceFromStart).FirstOrDefault(
                                        nd => !double.IsPositiveInfinity(nd.DistanceFromStart));
                if (nextNode == null)
                {
                    finished = true;
                }
                else
                {
                    var connections = nextNode.Connections.Where(c => queue.Contains(c.Target));
                    foreach (var connection in connections)
                    {
                        double distance = nextNode.DistanceFromStart + connection.Distance;
                        if (distance < connection.Target.DistanceFromStart)
                        {
                            connection.Target.DistanceFromStart = distance;
                            connection.Target.NearestVertex = nextNode.Name;
                        }
                    }
                    queue.Remove(nextNode);
                }
            }
        }
    }
}
