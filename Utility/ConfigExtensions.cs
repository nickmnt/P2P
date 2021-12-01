using P2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2P.Utility
{
    public static class ConfigExtensions
    {
        public static Node FindBestNeighbor(this Config config)
        {
            int best_val = 999999999;
            Node best = null;

            foreach(var n in config.FriendNodes)
            {
                if(Math.Abs(n.NodeName - config.NodeNumber) < best_val)
                {
                    best_val = n.NodeName;
                    best = n;
                }
            }

            return best;
        }

        public static Node FindBestNeighbor(this Config config, int excludedName)
        {
            int best_val = 999999999;
            Node best = null;

            foreach (var n in config.FriendNodes)
            {
                if (Math.Abs(n.NodeName-config.NodeNumber) < best_val && n.NodeName != excludedName)
                {
                    best_val = n.NodeName;
                    best = n;
                }
            }

            return best;
        }
    }
}
