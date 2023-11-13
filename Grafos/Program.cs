// See https://aka.ms/new-console-template for more information

using Grafos;

Grafo grafo =  new Grafo(false, true,false);

grafo.addNode(0);
grafo.addNode(1);
grafo.addNode(2);
grafo.addNode(3);
grafo.addNode(4);
grafo.addNode(5);

grafo.addEdge(0,1,1);
grafo.addEdge(0,2,2);
grafo.addEdge(1, 2, 3);
grafo.addEdge(1, 3, 4);
grafo.addEdge(2, 3, 5);
grafo.addEdge(2, 5, 6);
grafo.addEdge(5, 4, 2);

Console.WriteLine("Grafo: ");

grafo.toAjacentList();
Console.WriteLine("-----------");

grafo.toAjacentMatrix();

Console.WriteLine("------------");

grafo.toIncidenceMatrix();




