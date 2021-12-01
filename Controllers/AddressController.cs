using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using P2P.Utility;
using P2P.Models.Dto;
using P2P.Models;

namespace P2P.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly IConfigLoader _configLoader;

        public AddressController(ILogger<FileController> logger, IConfigLoader configLoader)
        {
            this._logger = logger;
            this._configLoader = configLoader;
        }

        [HttpPost("find")]
        public IActionResult Find(AddressRequestDto request)
        {
            Console.WriteLine($"Request received from node {request.RequesterName} for {request.TargetName}'s address");

            var searchResult = _configLoader.Config.FriendNodes.FirstOrDefault(x => x.NodeName == request.TargetName);
            if (searchResult != null)
            {
                return Ok("success " + searchResult.NodePort);
            }

            var validNeighbors = new List<Node>();
            foreach(var n in _configLoader.Config.FriendNodes)
            {
                if(!request.Visited.Contains(n.NodeName))
                {
                    validNeighbors.Add(n);
                }
            }

            if (validNeighbors.Count == 0)
                return Ok("empty");

            validNeighbors.Sort((x,y) =>
            {
                return Math.Abs(x.NodeName - _configLoader.Config.NodeNumber).CompareTo(Math.Abs(x.NodeName - _configLoader.Config.NodeNumber));
            });

            return Ok($"try {validNeighbors[0].NodeName} {validNeighbors[0].NodePort}");
        }
    }
}
