using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using P2P.Utility;

namespace P2P.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly IConfigLoader _configLoader;

        public FileController(ILogger<FileController> logger, IConfigLoader configLoader)
        {
            this._logger = logger;
            this._configLoader = configLoader;
        }

        [HttpGet("download/{fileName}/{nodeId}/{port}")]
        public IActionResult Download(string fileName, int nodeId, int port)
        {
            Console.WriteLine($"Request received from node {nodeId} with port {port} for file {fileName}");
            var fileAddress = $"./{_configLoader.Config.OwnedFilesDir}{fileName}";
            
            if (System.IO.File.Exists(fileAddress) && _configLoader.Config.OwnedFiles.Contains(fileName))
            {
                var stream = System.IO.File.OpenRead(fileAddress);

                return new FileStreamResult(stream, "application/octet-stream");
            }

            var best = _configLoader.Config.FindBestNeighbor(nodeId);

            return NotFound(best.NodePort);
        }
    }
}
