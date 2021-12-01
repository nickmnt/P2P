using P2P.Models;

namespace P2P.Utility
{
    public interface IConfigLoader
    {
        public Config Config { get; set; }
        public NodeFilesInfo NodeFilesInfo { get; set; } 
    }
}
