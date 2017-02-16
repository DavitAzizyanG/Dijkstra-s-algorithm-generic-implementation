using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphShortestPath
{
    class Program
    {
        static void Main(string[] args)
        {
            GraphTesting();
        }

        static void GraphTesting()
        {
            Graph graph = new Graph();
            //Nodes
            graph.AddNode("A");
            graph.AddNode("B");
            graph.AddNode("C");
            graph.AddNode("D");
            graph.AddNode("E");
            graph.AddNode("F");
            graph.AddNode("G");
            graph.AddNode("H");
            graph.AddNode("I");
            graph.AddNode("J");
            graph.AddNode("Z");
            //Connections
            graph.AddConnection("A", "B", 14, true);
            graph.AddConnection("A", "C", 10, true);
            graph.AddConnection("A", "D", 14, true);
            graph.AddConnection("A", "E", 21, true);
            graph.AddConnection("B", "C", 9, true);
            graph.AddConnection("B", "E", 10, true);
            graph.AddConnection("B", "F", 14, true);
            graph.AddConnection("C", "D", 9, false);
            graph.AddConnection("D", "G", 10, false);
            graph.AddConnection("E", "H", 11, true);
            graph.AddConnection("F", "C", 10, false);
            graph.AddConnection("F", "H", 10, true);
            graph.AddConnection("F", "I", 9, true);
            graph.AddConnection("G", "F", 8, false);
            graph.AddConnection("G", "I", 9, true);
            graph.AddConnection("H", "J", 9, true);
            graph.AddConnection("I", "J", 10, true);
            var calculator = new DistanceCalculator();
            var distances = calculator.CalculateDistances(graph, "G");  // Start from "G"
            foreach (var d in distances)
            {
                Console.WriteLine("{0}, {1}", d.Key, d.Value);
            }

            Stack<string> path = graph.ExtractShortestPath("G", "B");
            Console.WriteLine("Path from G to B is:");
            while (path.Count != 0)
            {
                Console.Write(path.Pop() + "  ");
            }
        }
    }
}

