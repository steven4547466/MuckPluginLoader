using MuckPluginLoader.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuckPluginLoader.Loader
{
	public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
	}
}
