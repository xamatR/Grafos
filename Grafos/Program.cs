// See https://aka.ms/new-console-template for more information

using Grafos;

void readGraphFile(string path, Grafo grafo) {
    string[] lines = System.IO.File.ReadAllLines(path);
    int nNodes = int.Parse(lines[0]);

    // Add nodes
    for (int i = 0; i < nNodes; i++) {
        grafo.addNode(i);
    }

    // Add edges
    for (int i = 1; i < nNodes; i++) {
        string[] values = lines[i].Split(' ');
        for (int j = 0; j < nNodes; j++) {
            if (grafo.directed) {
                if (float.Parse(values[j]) != 999f)
                    grafo.addEdge(i, j, float.Parse(values[j]));
            } else {
                if (j > i) {
                    if (float.Parse(values[j]) != 999f)
                        grafo.addEdge(i, j, float.Parse(values[j]));
                }
            }
        }
    }
}

void writeGraphFile(string path, Grafo grafo) {
    string[] lines = new string[grafo.nodes.Count + 1];
    lines[0] = grafo.nodes.Count.ToString();
    int[,] matrix = grafo.toAdjacencyMatrixWithReturn();
    for (int i = 0; i < grafo.nodes.Count; i++) {
        string line = "";
        for (int j = 0; j < grafo.nodes.Count; j++) {
            line += matrix[i, j].ToString() + " ";
        }
        lines[i + 1] = line;
    }
    System.IO.File.WriteAllLines(path, lines);
    Console.WriteLine("Printed");
}

Grafo grafo = new Grafo(false, true, false);

grafo.addNode(0);
grafo.addNode(1);
grafo.addNode(2);
grafo.addNode(3);
grafo.addNode(4);
grafo.addNode(5);

grafo.addEdge(0, 1, 1);
grafo.addEdge(0, 2, 2);
grafo.addEdge(1, 2, 3);
grafo.addEdge(1, 3, 4);
grafo.addEdge(2, 3, 5);
grafo.addEdge(2, 5, 6);
grafo.addEdge(5, 4, 2);
grafo.addEdge(3, 4, 3);
grafo.addEdge(0, 4, 0.5f);


Console.WriteLine("Grafo: ");

grafo.toAjacentList();
Console.WriteLine("-----------");

grafo.toAjacentMatrix();

Console.WriteLine("------------");

grafo.toIncidenceMatrix();

Console.WriteLine("-------------");

grafo.minimumSpanningTree();

Console.WriteLine("-------------");


grafo.breadthSearch();

Console.WriteLine("\n-------------");

grafo.depthSearch();

Console.WriteLine("\n-------------");

grafo.Dijkstra(0);

Console.WriteLine("\n-------------");

grafo.eulerianCycle();

Console.WriteLine("\n-------------");

grafo.orderTopological();





