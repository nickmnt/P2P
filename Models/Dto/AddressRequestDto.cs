using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P2P.Models.Dto
{
    public class AddressRequestDto
    {
        public int RequesterName { get; set; }
        public int TargetName { get; set; }
        public List<int> Visited { get; set; }
    }
}
