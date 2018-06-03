namespace Mp3MusicZone.Common.Mappings
{
    using AutoMapper;
    using System;

    public interface IHaveCustomMappings
    {
        void Configure(Profile config);
    }
}
