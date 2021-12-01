using P2P.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.IO;

namespace P2P.Utility
{
    public class ConfigLoader : IConfigLoader
    {
        public Config Config { get; set; }
        public NodeFilesInfo NodeFilesInfo { get; set; }

        public ConfigLoader()
        {
            var result = ReadFromFiles();
            Config = result.Config;
            NodeFilesInfo = result.NodeFilesInfo;
        }

        private static ConfigPair ReadFromFiles()
        {
            //Read Yaml
            var configYml = File.ReadAllText("Config.yml");
            var nodeFilesYml = File.ReadAllText("NodeFiles.yml");

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            var config = deserializer.Deserialize<Config>(configYml);
            var nodeFilesInfo = deserializer.Deserialize<NodeFilesInfo>(nodeFilesYml);

            return new ConfigPair { Config = config, NodeFilesInfo = nodeFilesInfo };
        }
    }
}
