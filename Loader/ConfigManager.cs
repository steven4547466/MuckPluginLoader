using MuckPluginLoader.API.Extensions;
using MuckPluginLoader.API.Features;
using MuckPluginLoader.API.Interfaces;
using MuckPluginLoader.Loader;
using MuckPluginLoader.Loader.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

namespace MuckPluginLoader
{
    public static class ConfigManager
	{
        public static ISerializer Serializer { get; } = new SerializerBuilder()
            .WithTypeInspector(inner => new CommentGatheringTypeInspector(inner))
            .WithEmissionPhaseObjectGraphVisitor(args => new CommentsObjectGraphVisitor(args.InnerVisitor))
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreFields()
            .Build();

        public static IDeserializer Deserializer { get; } = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .WithNodeDeserializer(inner => new ValidatingNodeDeserializer(inner), deserializer => deserializer.InsteadOf<ObjectNodeDeserializer>())
                .IgnoreFields()
                .IgnoreUnmatchedProperties()
                .Build();

        public static bool Save(string path, IConfig config)
        {
            try
            {
                string serialized = Serializer.Serialize(config);
                File.WriteAllText(path, serialized);
                return true;
            }
            catch (YamlException yamlException)
            {
                Log.Error($"[MPL] An error has occurred while serializing configs: {yamlException}");

                return false;
            }
        }

        public static IConfig LoadConfig(string raw, IPlugin<IConfig> plugin, bool isLoader = false)
		{
            try
            {
                if (!isLoader)
                    return (IConfig)Deserializer.Deserialize(raw, plugin.Config.GetType());
                else
                    return Deserializer.Deserialize<Config>(raw);
            } 
            catch (Exception exception)
			{
                Log.Error($"[MPL] An error has occurred deserializing configs: {exception}.");
                return null;
            }
        }

        public static string Read(string path)
        {
            try
            {
                if (File.Exists(path))
                    return File.ReadAllText(path);
                else
                    return null;
            }
            catch (Exception exception)
            {
                Log.Error($"[MPL] An error has occurred while reading configs from {path}: {exception}");
            }

            return string.Empty;
        }

        public static void Reload()
		{
			try
			{
                string loaderPath = Path.Combine(Paths.Configs, "MuckPluginLoader.Loader.yml");
                if (!File.Exists(loaderPath))
                {
                    Log.Warn("[MPL] MuckPluginLoader.Loader does not have default configs, generating");

                    Save(loaderPath, Loader.Loader.Config);
				}
                else
				{
                    var config = LoadConfig(Read(loaderPath), null, true);

                    Loader.Loader.Config.CopyProperties(config);

                    Save(loaderPath, config);
				}

                foreach(var plugin in Loader.Loader.Plugins)
				{
                    string path = Path.Combine(Paths.Configs, plugin.ShortName + ".yml");
                    try
                    {
                        if (!File.Exists(path))
                        {
                            Log.Warn($"[MPL] {plugin.Name} does not have default configs, generating");

                            Save(path, plugin.Config);
                        }
                        else
                        {
                            var config = LoadConfig(Read(path), plugin);

                            plugin.Config.CopyProperties(config);

                            Save(path, config);
                        }
                    }
                    catch (Exception exception)
					{
                        Log.Error($"[MPL] {plugin.Name} configs could not be loaded. Default configs will be used instead. {exception}");
                        
                        Save(path, plugin.Config);
                    }
                }
			}
            catch(Exception exception)
			{
                Log.Error($"[MPL] An error has occurred while reloading configs: {exception}");
            }
		}
    }
}
