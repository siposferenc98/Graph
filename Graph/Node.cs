using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class Node
    {
        public int ertek;
        public int x, y;
        public int parent;
        public List<int> neigh = new();
        public int status = 0;
    }
}
