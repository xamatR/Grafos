﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
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
            foreach (Node node in nodes) {
                if (node.id == id) return node;
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

            if (!this.directed) {
                for (int i = 0; i < this.order; i++) {
                    for (int j = 0; j < this.order; j++) {
                        if (j <= i) {
                            matrix[i, j] = 0;
                        }
                    }
                }
            }



            for (int i = 0; i < this.order; i++) {
                for (int j = 0; j < this.order; j++) {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }

        }

        public int[,] toAdjacencyMatrixWithReturn() {
            int[,] matrix = new int[this.order, this.order];
            foreach (var node in nodes) {
                foreach (var edge in node.edges) {
                    matrix[node.id, edge.idTarget] = 1;
                }
            }

            if (!this.directed) {
                for (int i = 0; i < this.order; i++) {
                    for (int j = 0; j < this.order; j++) {
                        if (j <= i) {
                            matrix[i, j] = 0;
                        }
                    }
                }
            }

            return matrix;
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
            return (findNode(idOrigin).edges.Find(x => x.idTarget == idTarget) != null) ? true : false;
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
                    //if the node is already in the solution then dont add the edge
                    if (!nodesInSolution.Contains(edge.idTarget))
                        frontierEdges.Add(edge);
                }
                //remove the edge from the frontier
                foreach (var edge in frontierEdges.ToList()) {
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

        public void depthSearch() {
            Console.WriteLine("Depth Search");
            List<int> visitedNodes = new List<int>();
            Stack<int> stack = new Stack<int>();

            stack.Push(this.nodes.First().id);
            visitedNodes.Add(this.nodes.First().id);
            int aux;
            while (stack.Count != 0) {
                aux = stack.Pop();
                foreach (var edge in findNode(aux).edges) {
                    if (!visitedNodes.Contains(edge.idTarget)) {
                        stack.Push(edge.idTarget);
                        visitedNodes.Add(edge.idTarget);
                    }
                }
            }

            foreach (var item in visitedNodes) {
                Console.Write(item + " ");
            }
        }

        public void breadthSearch() {
            Console.WriteLine("Breadth Search");
            List<int> visitedNodes = new List<int>();
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(this.nodes.First().id);
            visitedNodes.Add(this.nodes.First().id);
            int aux;
            while (queue.Count != 0) {
                aux = queue.Dequeue();
                foreach (var edge in findNode(aux).edges) {
                    if (!visitedNodes.Contains(edge.idTarget)) {
                        queue.Enqueue(edge.idTarget);
                        visitedNodes.Add(edge.idTarget);
                    }
                }
            }

            foreach (var item in visitedNodes) {
                Console.Write(item + " ");
            }
        }

        //Dijkstra's algorithm
        public void Dijkstra(int idOrigin) {
            Console.WriteLine("Dijkstra");

            if (!hasNode(idOrigin)) {
                Console.WriteLine("Not founded node.");
                return;
            }

            bool[] visitedNodes = new bool[this.order];
            float[] distance = new float[this.order];
            Stack<int>[] pathStack = new Stack<int>[this.order];

            for (int i = 0; i < this.order; i++) {
                distance[i] = float.MaxValue;
                visitedNodes[i] = false;
                pathStack[i] = new Stack<int>();
            }
            //distance of the origin node to itself is 0
            distance[idOrigin] = 0;

            for (int i = 0; i < this.order; i++) {
                //find the node with the minimum distance
                int aux = minDistance(distance, visitedNodes);
                visitedNodes[aux] = true;

                //update the distance of the adjacent nodes
                foreach (var edge in findNode(aux).edges) {
                    if (edge == null) continue;

                    if (!visitedNodes[edge.idTarget]
                        && edge.weight != 0
                        && (distance[aux] + edge.weight < distance[edge.idTarget])) {
                        distance[edge.idTarget] = distance[aux] + edge.weight;
                        pathStack[edge.idTarget].Clear();
                        pathStack[edge.idTarget].Push(aux);

                        foreach (var node in pathStack[aux]) {
                            pathStack[edge.idTarget].Push((int)node);
                        }
                    }
                }
            }
            for (int i = 0; i < this.order; i++) {
                Console.Write($"Distance from {idOrigin} to {i}: {distance[i]}, Path: ");
                PrintPath(pathStack[i]);
                Console.WriteLine();
            }
        }

        private void PrintPath(Stack<int> pathStack) {
            while (pathStack.Count > 0) {
                Console.Write(pathStack.Pop() + " ");
            }
        }

        private int minDistance(float[] distance, bool[] visitedNodes) {
            float min = float.MaxValue;
            int minIndex = -1;

            for (int i = 0; i < distance.Length; i++) {
                if (!visitedNodes[i] && distance[i] <= min) {
                    min = distance[i];
                    minIndex = i;
                }
            }
            if (minIndex == -1) {
                Console.WriteLine("No valid minIndex found. All remaining nodes are unreachable.");
            }

            return minIndex;
        }

        public void orderTopological() {
            Console.WriteLine("Order Topological");
            List<int> visitedNodes = new List<int>();
            Stack<int> stack = new Stack<int>();

            stack.Push(this.nodes.First().id);
            visitedNodes.Add(this.nodes.First().id);
            int aux;
            while (stack.Count != 0) {
                aux = stack.Pop();
                foreach (var edge in findNode(aux).edges) {
                    if (!visitedNodes.Contains(edge.idTarget)) {
                        stack.Push(edge.idTarget);
                        visitedNodes.Add(edge.idTarget);
                    }
                }
            }

            foreach (var item in visitedNodes) {
                Console.Write(item + " ");
            }
        }

        public void eulerianCycle() {
            Console.WriteLine("Eulerian Cycle");

            if (!eulerianGraph()) {
                Console.WriteLine("Not eulerian graph.");
                return;
            }

            // Create a copy of the graph
            Grafo auxGraph = this;

            Stack<int> stack = new Stack<int>();
            List<int> cycle = new List<int>();

            int current = auxGraph.nodes.First().id;
            eulerianCycleUtil(current, stack, cycle, auxGraph);

            foreach (var item in cycle) {
                Console.Write(item + " ");
            }
        }

        private void eulerianCycleUtil(int current, Stack<int> stack, List<int> cycle, Grafo auxGraph) {
            while (auxGraph.findNode(current).edges.Count > 0) {
                int next_node = auxGraph.findNode(current).edges.First().idTarget;
                // Remove edge from the auxiliary graph
                auxGraph.findNode(current).edges.Remove(auxGraph.findNode(current).edges.First());
                eulerianCycleUtil(next_node, stack, cycle, auxGraph);
            }
            cycle.Insert(0, current);
        }



        public bool eulerianGraph() {
            foreach (var node in nodes) {
                if (node.edges.Count % 2 != 0) {
                    return false;
                }
            }
            return true;
        }

        public void eulerianTour() {

        }


        

        private float totalWheightOfEdges(List<Edge> edges) {
            float total = 0;
            foreach (var edge in edges) {
                total += edge.weight;
            }
            return total;
        }

        public void toMatrixWithWeight() {
            int[,] matrix = new int[this.order, this.order];
            foreach (var node in nodes) {
                foreach (var edge in node.edges) {
                    matrix[node.id, edge.idTarget] = (int)edge.weight;
                }
            }

            if (!this.directed) {
                for (int i = 0; i < this.order; i++) {
                    for (int j = 0; j < this.order; j++) {
                        if (j <= i) {
                            matrix[i, j] = 0;
                        }
                    }
                }
            }

            for (int i = 0; i < this.order; i++) {
                for (int j = 0; j < this.order; j++) {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }


        }

        public int[,] toMatrixWithWeightAndInfinity() {
            int[,] matrix = new int[this.order, this.order];

            for (int i = 0; i < this.order; i++) {
                for (int j = 0; j < this.order; j++) {
                    matrix[i, j] = 0;
                }
            }

            foreach (var node in nodes) {
                foreach (var edge in node.edges) {
                    matrix[node.id, edge.idTarget] = (int)edge.weight;
                }
            }

            if (this.directed) {
                for (int i = 0; i < this.order; i++) {
                    for (int j = 0; j < this.order; j++) {
                        //if the node is in superior triangular and the value is 0 then change to infinity
                        if (i != j && matrix[i, j] == 0) {
                            matrix[i, j] = 999;
                        }
                    }
                }
            } else {
                for (int i = 0; i < this.order; i++) {
                    for (int j = 0; j < this.order; j++) {
                        if (j <= i) {
                            matrix[i, j] = 0;
                        } else if (matrix[i, j] == 0) {
                            matrix[i, j] = 999;
                        }
                    }
                }

            }

            return matrix;


        }


    }
}
