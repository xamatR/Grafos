using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafos {
    internal class Edge {

        public int idTarget { get; private set; }
        public int idOrigin { get; private set; }
        public float weight { get; private set; }

        public Edge(int idTarget, int idOrigin, float weight) {
            this.idTarget = idTarget;
            this.idOrigin = idOrigin;
            this.weight = weight;
        }   

    }
}
