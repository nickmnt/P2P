using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace P2P.Models
{
    public class Node: IComparable<Node>
    {
        public int NodeName { get; set; }
        public int NodePort { get; set; }

        public int CompareTo([AllowNull] Node other)
        {
            return this.NodeName.CompareTo(other.NodeName);
        }
    }
}
