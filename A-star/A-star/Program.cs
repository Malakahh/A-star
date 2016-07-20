using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using Node = Pathfinding.AStar<Pathfinding.Vector, string>.Node;
using Edge = Pathfinding.AStar<Pathfinding.Vector, string>.EdgeData;

namespace Pathfinding
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Graphs.DirectedGraph<Vector, string> graph = new Graphs.DirectedGraph<Vector, string>();
            //graph.AddNode(new Node(new Vector(1, 2)));
            //graph.AddNode(new Node(new Vector(2, 3)));
            //graph.AddEdge(graph.nodes[0], graph.nodes[1], new Edge("1,2 -> 2,3", 1));
            //graph.AddEdge(graph.nodes[1], graph.nodes[0], new Edge("2,3 -> 1,2", 2));
            //Console.WriteLine(graph.ToString());
            //Console.ReadKey();







            //graph.AddNode(new Vector(1, 2), "This is 1-2");
            //graph.AddNode(new Vector(2, 3), "This is 2-3");
            //graph.AddEdge(graph.GetNode(new Vector(1,2)), graph.GetNode(new Vector(2, 3)), 1.2);
            //graph.AddEdge(graph.GetNode(new Vector(2, 3)), graph.GetNode(new Vector(1, 2)), 2.3);
            //Console.Write(graph.ToString());

            GraphicsUnit gu = GraphicsUnit.Pixel;
            Vector start = new Vector(0, 0);
            Vector goal = new Vector(0, 0);

            string file = Directory.GetCurrentDirectory() + "\\test";

            Bitmap map = new Bitmap(file + ".png");
            int width = (int)map.GetBounds(ref gu).Width;
            int height = (int)map.GetBounds(ref gu).Height;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color c = map.GetPixel(x, y);

                    if (c == Color.FromArgb(0, 255, 0)) // start
                    {
                        start = new Vector(x, y);
                        graph.AddNode(new Node(start));
                    }
                    else if (c == Color.FromArgb(255, 0, 0)) // goal
                    {
                        goal = new Vector(x, y);
                        graph.AddNode(new Node(goal));
                    }
                    else if (c == Color.FromArgb(255, 255, 255)) //blank
                    {
                        graph.AddNode(new Node(new Vector(x, y)));
                    }
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector c = new Vector(x, y);
                    Node n = (Node)graph.GetNode(c);

                    if (n == null)
                        continue;

                    // x 
                    if (y + 1 < height)
                    {
                        var m = graph.GetNode(new Vector(x, y + 1));
                        if (m != null)
                            graph.AddEdge(n, m, new Edge(n + "->" + m, 1));
                    }

                    if (y - 1 >= 0)
                    {
                        var m = graph.GetNode(new Vector(x, y - 1));
                        if (m != null)
                            graph.AddEdge(n, m, new Edge(n + "->" + m, 1));
                    }

                    // x + 1
                    if (x + 1 < width)
                    {
                        var m = graph.GetNode(new Vector(x + 1, y));
                        if (m != null)
                            graph.AddEdge(n, m, new Edge(n + "->" + m, 1));

                        if (y + 1 < height)
                        {
                            var o = graph.GetNode(new Vector(x + 1, y + 1));
                            if (o != null)
                                graph.AddEdge(n, o, new Edge(n + "->" + o, n.data.DistanceTo(o.data)));
                        }

                        if (y - 1 >= 0)
                        {
                            var o = graph.GetNode(new Vector(x + 1, y - 1));
                            if (o != null)
                                graph.AddEdge(n, o, new Edge(n + "->" + o, n.data.DistanceTo(o.data)));
                        }
                    }

                    // x - 1
                    if (x - 1 >= 0)
                    {
                        var m = graph.GetNode(new Vector(x - 1, y));
                        if (m != null)
                            graph.AddEdge(n, m, new Edge(n + "->" + m, 1));

                        if (y + 1 < height)
                        {
                            var o = graph.GetNode(new Vector(x - 1, y + 1));
                            if (o != null)
                                graph.AddEdge(n, o, new Edge(n + "->" + o, n.data.DistanceTo(o.data)));
                        }

                        if (y - 1 >= 0)
                        {
                            var o = graph.GetNode(new Vector(x - 1, y - 1));
                            if (o != null)
                                graph.AddEdge(n, o, new Edge(n + "->" + o, n.data.DistanceTo(o.data)));
                        }
                    }
                }
            }

            Node[] path = AStar<Vector, string>.FindPath((Node)graph.GetNode(start), (Node)graph.GetNode(goal), graph, h);

            foreach (Node n in path)
            {
                if (!(n.data.x == start.x && n.data.y == start.y) &&
                    !(n.data.x == goal.x && n.data.y == goal.y))
                {
                    map.SetPixel((int)n.data.x, (int)n.data.y, Color.FromArgb(0, 0, 255));
                }
            }

            map.Save(file + "2.png");
            Console.WriteLine("DonnoDK");
            Console.ReadKey();

        }

        static double h(Node n, Node goal)
        {
            return n.data.DistanceTo(goal.data);
        }
    }

    struct Vector
    {
        public float x;
        public float y;

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public double DistanceTo(Vector v)
        {
            return Math.Sqrt(Math.Pow(x - v.x, 2) + Math.Pow(y - v.y, 2));
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }

        public override bool Equals(object obj)
        {
            Vector v = (Vector)obj;

            return v.x == x && v.y == y;
        }

    }
}
