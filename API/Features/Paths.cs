using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuckPluginLoader.API.Features
{
	public static class Paths
	{
		static Paths() => LoadPaths();

		public static string AppData { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		public static string MPL { get; set; }

		public static string Plugins { get; set; }

		public static string Dependencies { get; set;}

		public static string Configs { get; set; }

		public static void LoadPaths()
		{
			MPL = Path.Combine(AppData, "MuckPluginLoader");
			Plugins = Path.Combine(MPL, "Plugins");
			Dependencies = Path.Combine(Plugins, "dependencies");
			Configs = Path.Combine(MPL, "Configs");

		}
	}
}
