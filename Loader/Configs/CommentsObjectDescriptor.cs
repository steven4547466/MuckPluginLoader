using System;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace MuckPluginLoader.Loader.Configs
{
    /// <summary>
    /// Source: https://dotnetfiddle.net/8M6iIE.
    /// </summary>
    public sealed class CommentsObjectDescriptor : IObjectDescriptor
    {
        private readonly IObjectDescriptor innerDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsObjectDescriptor"/> class.
        /// </summary>
        /// <param name="innerDescriptor">The inner descriptor instance.</param>
        /// <param name="comment">The comment to be written.</param>
        public CommentsObjectDescriptor(IObjectDescriptor innerDescriptor, string comment)
        {
            this.innerDescriptor = innerDescriptor;
            Comment = comment;
        }

        /// <summary>
        /// Gets the comment to be written.
        /// </summary>
        public string Comment { get; private set; }

        /// <inheritdoc/>
        public object Value => innerDescriptor.Value;

        /// <inheritdoc/>
        public Type Type => innerDescriptor.Type;

        /// <inheritdoc/>
        public Type StaticType => innerDescriptor.StaticType;

        /// <inheritdoc/>
        public ScalarStyle ScalarStyle => innerDescriptor.ScalarStyle;
    }
}
