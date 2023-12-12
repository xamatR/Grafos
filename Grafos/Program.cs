// See https://aka.ms/new-console-template for more information

using Grafos;
using System.Collections.Immutable;

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
            if (values[j] == "0" || values[j] == "999") continue;

            grafo.addEdge(i - 1, j, float.Parse(values[j]));
            
        }
    }

    //grafo.toMatrixWithWeight();
}



int menu1() {
    Console.WriteLine("1. Ler grafo de arquivo");
    Console.WriteLine("0. Sair");

    int option = Console.ReadKey().KeyChar - 48;

    return option;
}

int subMenu() {
    Console.WriteLine("1. Ordem topologica");
    Console.WriteLine("2. Print lista de adjacencia");
    Console.WriteLine("3. Print matriz de adjacencia");
    Console.WriteLine("4. Print matriz de incidencia");
    Console.WriteLine("5. Arvore geradora minima");
    Console.WriteLine("6. Busca em largura");
    Console.WriteLine("7. Busca em profundidade");
    Console.WriteLine("8. Dijkstra");
    Console.WriteLine("9. Ciclo euleriano");
    Console.WriteLine("0. Sair");


    int option = Console.ReadKey().KeyChar -48;
    return option;
}

void writeGraphFile(string path, Grafo grafo) {
    string[] lines = new string[grafo.nodes.Count + 1];
    lines[0] = grafo.nodes.Count.ToString();
    int[,] matrix = grafo.toMatrixWithWeightAndInfinity();
    for (int i = 0; i < grafo.nodes.Count; i++) {
        string line = "";
        for (int j = 0; j < grafo.nodes.Count; j++) {
            line += matrix[i, j].ToString() + " ";
        }
        lines[i + 1] = line;
    }
    System.IO.File.WriteAllLines(path, lines);
    Console.WriteLine("Arquivo salvo na pasta" + System.IO.Directory.GetCurrentDirectory() + " com o nome de " + path + ".");
}

bool menuGraphType() {
    Console.WriteLine("1. Grafo direcionado");
    Console.WriteLine("2. Grafo não direcionado");
    int type = Console.ReadKey().KeyChar -48;

    return (type != 1) ? false : true;
}




int option1 = menu1();
if (option1 == 1) {
    Console.Clear();
    Grafo grafo = new Grafo(menuGraphType(), true, false);
    readGraphFile("input.txt", grafo);
    int option = 1;
    while (option != 0) {
        Console.Clear();
        option = 0;
        option = subMenu();
        switch (option) {
            case 1:
                Console.WriteLine("");
                grafo.orderTopological();
                break;
            case 2:
                Console.WriteLine("");
                grafo.toAjacentList();
                break;
            case 3:
                Console.WriteLine("");
                grafo.toAjacentMatrix();
                break;
            case 4:
                Console.WriteLine("");
                grafo.toIncidenceMatrix();
                break;
            case 5:
                Console.WriteLine("");
                grafo.minimumSpanningTree();
                break;
            case 6:
                Console.WriteLine("");
                grafo.breadthSearch();
                break;
            case 7:
                Console.WriteLine("");
                grafo.depthSearch();
                break;
            case 8:
                Console.WriteLine("");
                Console.WriteLine("Digite o vertice inicial: ");
                int v = Console.ReadKey().KeyChar - 48;
                Console.WriteLine("");
                grafo.Dijkstra(v);
                break;
            case 9:
                Console.WriteLine("");
                grafo.eulerianCycle();
                break;            
            case 0:
                Console.WriteLine("");
                Console.WriteLine("Saindo...");
                break;
            default:
                Console.WriteLine("");
                break;
        }
        if (option != 0) {
            Console.WriteLine("\n --------------------------------");
            Console.WriteLine("Pressione qualquer tecla para escolher uma nova opção.");
            Console.ReadKey();
        }
    }
    Console.WriteLine("Deseja salvar o grafo? (s/n)");
    
    char save = Console.ReadKey().KeyChar;
    Console.WriteLine("");
    if (save == 's') {
        Console.WriteLine("Digite o nome do arquivo:");
        string path = Console.ReadLine() + ".txt";
        if(path == ".txt") path = "output.txt";
        Console.WriteLine("");
        writeGraphFile(path, grafo);
    }
}



//Grafo grafo = new Grafo(false, true, false);

//grafo.addNode(0);
//grafo.addNode(1);
//grafo.addNode(2);
//grafo.addNode(3);
//grafo.addNode(4);
//grafo.addNode(5);

//grafo.addEdge(0, 1, 1);
//grafo.addEdge(0, 2, 2);
//grafo.addEdge(1, 2, 3);
//grafo.addEdge(1, 3, 4);
//grafo.addEdge(2, 3, 5);
//grafo.addEdge(2, 5, 6);
//grafo.addEdge(5, 4, 2);
//grafo.addEdge(3, 4, 3);
//grafo.addEdge(0, 4, 0.5f);


//Console.WriteLine("Grafo: ");

//grafo.toAjacentList();
//Console.WriteLine("-----------");

//grafo.toAjacentMatrix();

//Console.WriteLine("------------");

//grafo.toIncidenceMatrix();

//Console.WriteLine("-------------");

//grafo.minimumSpanningTree();

//Console.WriteLine("-------------");

//grafo.breadthSearch();

//Console.WriteLine("\n-------------");

//grafo.depthSearch();

//Console.WriteLine("\n-------------");

//grafo.Dijkstra(0);

//Console.WriteLine("\n-------------");

//grafo.eulerianCycle();

//Console.WriteLine("\n-------------");

//grafo.orderTopological();





