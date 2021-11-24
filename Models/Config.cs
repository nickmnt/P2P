using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2P.Models
{
    public class Config
    {
        public int NodeNumber { get; set; }
        public int NodePort { get; set; }
        public string OwnedFilesDir { get; set; }
        public string NewFilesDir { get; set; }
        public List<string> OwnedFiles { get; set; }
        public List<Node> FriendNodes { get; set; }
    }
}
