using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuckPluginLoader.API.Interfaces;

namespace MuckPluginLoader.Events
{
	public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		public bool EnableDebug { get; set; } = false;
	}
}
