using System;
using System.Collections.Generic;
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
                if(node.id == id) {
                    return node;
                }
            }
            return null;
        }


        public bool hasNode(int id) {
            foreach (var node in nodes) {
                if(node.id == id) {
                    return true;
                }
            }
            return false;
        }

        public void removeNode(int id) {
            if(this.hasNode(id)) {
                Node node = findNode(id);
                foreach(var edge in node.edges) {
                    this.removeEdge(edge.idOrigin, edge.idTarget);
                }
                this.nodes.Remove(node);
                this.order--;
            }
        }

        public void addEdge(int idOrigin, int idTarget, float weight) {
            bool originInsert = false;
            bool targetInsert = false;
            if(!hasNode(idOrigin) &&!hasNode(idTarget)) {
                Console.WriteLine("Not founded nodes.");
                return;
            }

            if (this.directed) {
                foreach (var node in nodes)
                {
                    if (node.id == idOrigin) {
                        node.addEdge(idTarget,idOrigin, weight);
                        return;
                    }
                }
            }

            foreach (var node in nodes)
            {
                if(node.id == idOrigin) {
                    node.addEdge(idTarget, idOrigin, weight);
                    originInsert = true;
                }else if(node.id == idTarget) {
                    node.addEdge(idOrigin,idTarget , weight);
                    targetInsert = true;
                }

                if(originInsert && targetInsert) {
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
                            if(edge.idTarget == idTarget) {
                                node.edges.Remove(edge);
                                return;
                            }
                        }
                    }
                }
            }

            foreach (var node in nodes)
            {
                if(node.id == idOrigin) {
                    foreach(var edge in node.edges) {
                        if(edge.idTarget == idTarget) {
                            node.edges.Remove(edge);
                            break;
                        }
                    }
                }
            }

            foreach (var node in nodes)
            {
                if(node.id == idTarget) {
                    foreach(var edge in node.edges) {
                        if(edge.idTarget == idOrigin) {
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
                    Console.Write(matrix[i,j] + " ");
                }
                Console.WriteLine();
            }
        }


        public void toIncidenceMatrix() {
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

    }
}
