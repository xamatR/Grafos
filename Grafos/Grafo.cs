using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafos {
    internal class Grafo {
        public int order { get; private set; }
        public int conterFinal { get; private set; }
        public List<Node> nodes { get; private set; }
        public bool directed { get; private set; }
        public bool weighted_edge { get; private set; }
        public bool weighted_node { get; private set; }

        public Grafo(bool directed, bool weighted_edge, bool weighted_node) {
            this.order = 0;
            this.conterFinal = 0;
            this.nodes = new List<Node>();
            this.directed = directed;
            this.weighted_edge = weighted_edge;
            this.weighted_node = weighted_node;
        }

        public void addNode(float weight) {
            Node node = new Node(this.conterFinal, weight);
            this.nodes.Add(node);
            this.order++;
            this.conterFinal++;
        }

        private Node findNode(int id) {
            foreach (var node in nodes) {
                if (node.id == id) {
                    return node;
                }
            }
            return null;
        }


        public bool hasNode(int id) {
            foreach (var node in nodes) {
                if (node.id == id) {
                    return true;
                }
            }
            return false;
        }

        public void removeNode(int id) {
            if (this.hasNode(id)) {
                Node node = findNode(id);
                foreach (var edge in node.edges.ToList()) {
                    this.removeEdge(edge.idOrigin, edge.idTarget);
                }
                this.nodes.Remove(node);
                this.order--;
            }
        }

        public void addEdge(int idOrigin, int idTarget, float weight) {
            bool originInsert = false;
            bool targetInsert = false;
            if (!hasNode(idOrigin) && !hasNode(idTarget)) {
                Console.WriteLine("Not founded nodes.");
                return;
            }

            if (this.directed) {
                foreach (var node in nodes) {
                    if (node.id == idOrigin) {
                        node.addEdge(idTarget, idOrigin, weight);
                        return;
                    }
                }
            }

            foreach (var node in nodes) {
                if (node.id == idOrigin) {
                    node.addEdge(idTarget, idOrigin, weight);
                    originInsert = true;
                } else if (node.id == idTarget) {
                    node.addEdge(idOrigin, idTarget, weight);
                    targetInsert = true;
                }

                if (originInsert && targetInsert) {
                    return;
                }
            }
        }

        public void removeEdge(int idOrigin, int idTarget) {

            if (!hasNode(idTarget) && !hasNode(idOrigin)) {
                Console.WriteLine("Not founded nodes.");
                return;
            }

            if (this.directed) {
                foreach (var node in nodes) {
                    if (node.id == idOrigin) {
                        foreach (var edge in node.edges) {
                            if (edge.idTarget == idTarget) {
                                node.edges.Remove(edge);
                                return;
                            }
                        }
                    }
                }
            }

            foreach (var node in nodes) {
                if (node.id == idOrigin) {
                    foreach (var edge in node.edges) {
                        if (edge.idTarget == idTarget) {
                            node.edges.Remove(edge);
                            break;
                        }
                    }
                }
            }

            foreach (var node in nodes) {
                if (node.id == idTarget) {
                    foreach (var edge in node.edges) {
                        if (edge.idTarget == idOrigin) {
                            node.edges.Remove(edge);
                            return;
                        }
                    }
                }
            }

        }


        public void toAjacentList() {
            foreach (var node in nodes) {
                Console.Write(node.id + " -> ");
                foreach (var edge in node.edges) {
                    Console.Write(edge.idTarget + " ");
                }
                Console.WriteLine();
            }
        }

        public void toAjacentMatrix() {
            int[,] matrix = new int[this.order, this.order];
            foreach (var node in nodes) {
                foreach (var edge in node.edges) {
                    matrix[node.id, edge.idTarget] = 1;
                }
            }

            for (int i = 0; i < this.order; i++) {
                for (int j = 0; j < this.order; j++) {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }


        public void toIncidenceMatrix() {

            int edgesSum = this.nodes.Sum(x => x.edges.Count()) / 2;

            int[,] matrix = new int[this.order, edgesSum];

            int counter = 0;

            for (int i = 0; i < this.order; i++) {
                //sort the edges by idTarget to avoid add the same edge twice
                nodes[i].edges.Sort((x, y) => x.idTarget.CompareTo(y.idTarget));
                for (int j = 0; j < nodes[i].edges.Count(); j++) {
                    //add the edge only if the idTarget is bigger than the idOrigin
                    if (nodes[i].id < nodes[i].edges[j].idTarget) {
                        matrix[i, counter] = 1;
                        matrix[nodes[i].edges[j].idTarget, counter] = 1;
                        counter++;
                    }

                }
            }

            for (int i = 0; i < this.order; i++) {
                for (int j = 0; j < edgesSum; j++) {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }

        }

        private bool hasEdge(int idOrigin, int idTarget) {
            return (findNode(idOrigin).edges.Find(x => x.idTarget == idTarget) != null) ? true: false;
        }
        //Prim's algorithm
        public void minimumSpanningTree() {
            Console.WriteLine("Minimun Spanning Tree(Prim)");
            List<int> nodesInSolution = new List<int>();
            List<Edge> frontierEdges = new List<Edge>();
            List<Edge> tree = new List<Edge>();
            nodesInSolution.Add(this.nodes.First().id);
            foreach (var edge in this.nodes.First().edges) {
                frontierEdges.Add(edge);
            }

            Edge auxEdge;

            while (frontierEdges.Count != 0) {
                //find the edge with the minimum weight
                auxEdge = frontierEdges.Find(x => x.weight == frontierEdges.Min(y => y.weight));
                //add the target node to the nodesInSolution
                nodesInSolution.Add(auxEdge.idTarget);
                tree.Add(auxEdge);
                //update the frontier
                foreach (var edge in findNode(auxEdge.idTarget).edges.ToList()) {
                    if(!nodesInSolution.Contains(edge.idTarget))
                        frontierEdges.Add(edge);                 
                }
                //remove the edge from the frontier
                foreach(var edge in frontierEdges.ToList()) {
                    if (edge.idTarget == auxEdge.idTarget) {
                        frontierEdges.Remove(edge);
                    }
                }

                //foreach (var edge in frontierEdges.ToList()) {
                //    if (nodesInSolution.Contains(edge.idTarget)) {
                //        frontierEdges.Remove(edge);
                //    }
                //}
            }

            //print the list like a nodesInSolution

            foreach (var item in tree) {
                Console.WriteLine("{" + item.ToString() + "}");
            }

            foreach (var item in nodesInSolution) {
                Console.Write(item + " ");
            }

            float weight = totalWheightOfEdges(tree);

            Console.WriteLine("\nTotal weight of minimun spannig tree: " + weight);
        }

        //Kruskal's algorithm
        public void MSTKruskal() {
            Console.WriteLine("Minimun Spaning Tree (Kruskal)");

            //TODO: implement Kruskal's algorithm

        }


        private float totalWheightOfEdges(List<Edge> edges) {
            float total = 0;
            foreach (var edge in edges) {
                total += edge.weight;
            }
            return total;
        }

        
    }
}
