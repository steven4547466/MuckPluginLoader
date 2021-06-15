﻿using System;
using System.ComponentModel;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace MuckPluginLoader.Loader.Configs
{
    /// <summary>
    /// Source: https://dotnetfiddle.net/8M6iIE.
    /// </summary>
    public sealed class CommentsPropertyDescriptor : IPropertyDescriptor
    {
        private readonly IPropertyDescriptor baseDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsPropertyDescriptor"/> class.
        /// </summary>
        /// <param name="baseDescriptor">The base descriptor instance.</param>
        public CommentsPropertyDescriptor(IPropertyDescriptor baseDescriptor)
        {
            this.baseDescriptor = baseDescriptor;
            Name = baseDescriptor.Name;
        }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public Type Type => baseDescriptor.Type;

        /// <inheritdoc/>
        public Type TypeOverride
        {
            get => baseDescriptor.TypeOverride;
            set => baseDescriptor.TypeOverride = value;
        }

        /// <inheritdoc/>
        public int Order { get; set; }

        /// <inheritdoc/>
        public ScalarStyle ScalarStyle
        {
            get => baseDescriptor.ScalarStyle;
            set => baseDescriptor.ScalarStyle = value;
        }

        /// <inheritdoc/>
        public bool CanWrite => baseDescriptor.CanWrite;

        /// <inheritdoc/>
        public void Write(object target, object value)
        {
            baseDescriptor.Write(target, value);
        }

        /// <inheritdoc/>
        public T GetCustomAttribute<T>()
            where T : Attribute
        {
            return baseDescriptor.GetCustomAttribute<T>();
        }

        /// <inheritdoc/>
        public IObjectDescriptor Read(object target)
        {
            var description = baseDescriptor.GetCustomAttribute<DescriptionAttribute>();
            return description != null
                ? new CommentsObjectDescriptor(baseDescriptor.Read(target), description.Description)
                : baseDescriptor.Read(target);
        }
    }
}
