using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafos {
    internal class Node {

        public int id { get; private set; }
        public int degreeIn { get; private set; }
        public int degreeOut { get; private set; }
        public float weight { get; private set; }
        public List<Edge> edges { get; private set; }
        public Node(int id, float weight) {
            this.id = id;
            this.weight = weight;
            this.degreeIn = 0;
            this.degreeOut = 0;
            this.edges = new List<Edge>();
        }
        
        public void addEdge(int idTarget,int idOrigin, float weight) {
            Edge edge = new Edge(idTarget,idOrigin, weight);
            this.edges.Add(edge);
            this.degreeOut++;
        }   



    }
}
