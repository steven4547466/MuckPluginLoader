using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MuckPluginLoader.API.Interfaces
{
	public interface IPlugin<out TConfig>
		where TConfig : IConfig
	{
		Assembly Assembly { get; }
		
		string Name { get; }
		
		string ShortName { get; }

		string Author { get; }

		Version Version { get; }

		TConfig Config { get; }

		void OnEnable();

		void OnDisable();
	}
}
