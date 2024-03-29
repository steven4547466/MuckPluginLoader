﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MuckPluginLoader.Bootstrap
{
	public sealed class Bootstrap
	{
		public static bool IsLoaded { get; set; }

		public static object Plugins { get; set; } 

		public static Version Version { get; set; }

		public static void Load() 
		{ 
			if (IsLoaded)
			{
				UnityEngine.Debug.LogWarning("[MPL] MuckPluginLoader is already loaded!");
				return;
			}
			try
			{
				UnityEngine.Debug.Log("[MPL] MuckPluginLoader is loading!");
				string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				string path = Path.Combine(appData, "MuckPluginLoader");
				string pluginsPath = Path.Combine(path, "Plugins");
				string depsPath = Path.Combine(pluginsPath, "dependencies");
				if (!Directory.Exists(depsPath))
				{
					Directory.CreateDirectory(depsPath);
					Directory.CreateDirectory(Path.Combine(path, "Configs"));
				}
				if (File.Exists(Path.Combine(path, "MuckPluginLoader.Loader.dll")))
				{
					if (File.Exists(Path.Combine(depsPath, "MuckPluginLoader.API.dll")))
					{
						if (File.Exists(Path.Combine(depsPath, "YamlDotNet.dll")))
						{
							Assembly assembly = Assembly.Load(File.ReadAllBytes(Path.Combine(path, "MuckPluginLoader.Loader.dll")));
							MethodInfo method = assembly.GetType("MuckPluginLoader.Loader.Loader").GetMethod("Run");
							if (method != null)
							{
								var plugins = method.Invoke(null, new object[]
								{
									new Assembly[]
									{
										Assembly.Load(File.ReadAllBytes(Path.Combine(depsPath, "MuckPluginLoader.API.dll"))),
										Assembly.Load(File.ReadAllBytes(Path.Combine(depsPath, "YamlDotNet.dll")))
									}
								});
								Plugins = plugins;
							}
							IsLoaded = true;
							Version = assembly.GetName().Version;
							return;
						}
						else
						{
							UnityEngine.Debug.LogError("[MPL] YamlDotNet.dll was not found. Make sure you installed MPL correctly!");
						}
					}
					else
					{
						UnityEngine.Debug.LogError("[MPL] MuckPluginLoader.API.dll not found. Make sure you installed MPL correctly!");
					}
				}
				else
				{
					UnityEngine.Debug.LogError("[MPL] MuckPluginLoader.Loader.dll not found. Make sure you installed MPL correctly!");
				}
			}
			catch(Exception e)
			{
				UnityEngine.Debug.LogError(e);
			}
		}
	}
}
