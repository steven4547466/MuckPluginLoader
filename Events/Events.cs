using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuckPluginLoader.API.Features;
using HarmonyLib;

namespace MuckPluginLoader.Events
{
    public class Events : Plugin<Config>
    {
		public override string Author { get; } = "MPL Team";

		public static Events Instance { get; set; }

		public Harmony Harmony { get; private set; }

		private int harmonyPatches = 0;

		public delegate void CustomEventHandler<TEventArgs>(TEventArgs ev)
			where TEventArgs : System.EventArgs;

		public delegate void CustomEventHandler();

		public override void OnEnable()
		{
			base.OnEnable();
			Instance = this;
			try
			{
				Harmony.DEBUG = true;
				Harmony = new Harmony($"MPL-Events-{++harmonyPatches}");
				Harmony.PatchAll();
				Log.Debug("[MPL] Events patched successfully");
			} 
			catch(Exception exception)
			{
				Log.Error($"[MPL] Events patching failed: {exception}");
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();
			Harmony.UnpatchAll();
			Log.Debug("[MPL] Events unpatched");
		}
	}
}
