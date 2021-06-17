using MuckPluginLoader.API.Features;
using System;

namespace MuckPluginLoader.API.Extensions
{
	public static class Reflection
	{
        public static void CopyProperties(this object target, object source)
        {
            Type type = target.GetType();

            if (type != source.GetType())
                throw new Exception("Target and source type mismatch!");

            foreach (var sourceProperty in type.GetProperties())
                type.GetProperty(sourceProperty.Name)?.SetValue(target, sourceProperty.GetValue(source, null), null);
        }
    }
}
