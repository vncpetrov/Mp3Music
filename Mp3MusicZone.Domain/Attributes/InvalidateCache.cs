namespace Mp3MusicZone.Domain.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InvalidateCacheForAttribute : Attribute
    {
        public InvalidateCacheForAttribute(params string[] keys)
        {
            this.Keys = keys;
        }

        public string[] Keys { get; set; }
    }
}
