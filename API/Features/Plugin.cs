using MuckPluginLoader.API.Features;
using MuckPluginLoader.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MuckPluginLoader.API.Features
{
	public abstract class Plugin<TConfig> : IPlugin<TConfig>
		where TConfig : IConfig, new()
	{

        public Assembly Assembly { get; protected set; }

        public virtual string Name { get; }
        
        public virtual string ShortName { get; }

        public virtual string Author { get; }

        public virtual Version Version { get; }

        public virtual TConfig Config { get; }

        public Plugin()
        {
            Assembly = Assembly.GetCallingAssembly();
            Name = Assembly.GetName().Name;
            ShortName = Regex.Replace(string.Concat(Name.Select((ch, i) => i > 0 && char.IsUpper(ch) ? "_" + ch.ToString() : ch.ToString())).ToLower(), @"[^0-9a-zA-Z_]+", string.Empty);
            Author = Assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
            Version = Assembly.GetName().Version;
        }

        public virtual void OnEnable()
        {
            var attribute = Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            Log.Debug($"{Name} v{(attribute == null ? $"{Version.Major}.{Version.Minor}.{Version.Build}" : attribute.InformationalVersion)} by {Author} has been enabled!");
        }
        public virtual void OnDisable() => Log.Debug($"{Name} has been disabled!");
    }
}
