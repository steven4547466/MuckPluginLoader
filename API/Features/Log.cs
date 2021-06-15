using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MuckPluginLoader.API.Features
{
	public static class Log
	{
		public static void Debug(object message, bool shouldShow = true)
		{
			if (shouldShow)
				UnityEngine.Debug.Log(message);
		}

		public static void Error(object message) => UnityEngine.Debug.LogError(message);

		public static void Warn(object message) => UnityEngine.Debug.LogWarning(message);
	}
}
