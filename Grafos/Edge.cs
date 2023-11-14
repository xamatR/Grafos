using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafos {
    internal class Edge :IComparable<Edge> {

        public int idTarget { get; private set; }
        public int idOrigin { get; private set; }
        public float weight { get; private set; }

        public Edge(int idTarget, int idOrigin, float weight) {
            this.idTarget = idTarget;
            this.idOrigin = idOrigin;
            this.weight = weight;
        }

        public int CompareTo(Edge other) {
            return this.weight.CompareTo(other.weight);
        }

        public override string ToString() {
            return "Origin: "  + this.idOrigin + " Target: " + this.idTarget + " Weight: " + this.weight;
        }
    }
}
