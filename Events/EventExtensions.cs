using MuckPluginLoader.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuckPluginLoader.Events
{
	public static class EventExtensions
	{
        public static void InvokeSafely<T>(this Events.CustomEventHandler<T> ev, T arg)
            where T : System.EventArgs
        {
            if (ev == null)
                return;

            foreach (Events.CustomEventHandler<T> handler in ev.GetInvocationList())
            {
                try
                {
                    handler(arg);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }

        public static void InvokeSafely(this Events.CustomEventHandler ev)
        {
            if (ev == null)
                return;

            foreach (Events.CustomEventHandler handler in ev.GetInvocationList())
            {
                try
                {
                    handler();
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }
    }
}
