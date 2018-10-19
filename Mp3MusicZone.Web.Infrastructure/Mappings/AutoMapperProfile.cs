namespace Mp3MusicZone.Web.Infrastructure.Mappings
{
    using AutoMapper;
    using Mp3MusicZone.Common.Mappings;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            IEnumerable<Type> allTypes = this.GetTypes();

            this.RegisterToMappings(allTypes);
            this.RegisterFromMappings(allTypes);
            this.RegisterCustomMappings(allTypes);
        }

        private void RegisterToMappings(IEnumerable<Type> types)
        {
            var toMappings =
                types.Where(t => t.IsClass
                                    && !t.IsAbstract
                                    && t.GetInterfaces()
                                        .Any(i => i.IsGenericType
                                                  && i.GetGenericTypeDefinition()
                                                         == typeof(IMapTo<>)))
                         .Select(t => new
                         {
                             Source = t,
                             Destination = t.GetInterfaces()
                                .First(i => i.GetGenericTypeDefinition() == typeof(IMapTo<>))
                                .GetGenericArguments()
                                .First()
                         });

            foreach (var mapping in toMappings)
            {
                this.CreateMap(mapping.Source, mapping.Destination);
            }
        }

        private void RegisterFromMappings(IEnumerable<Type> types)
        {
            var fromMappings =
                types.Where(t => t.IsClass
                            && !t.IsAbstract
                            && t.GetInterfaces()
                                .Any(i => i.IsGenericType
                                     && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                      .Select(t => new
                      {
                          Destination = t,
                          Source = t.GetInterfaces()
                            .First(i => i.GetGenericTypeDefinition() == typeof(IMapFrom<>))
                            .GetGenericArguments().First()
                      });

            foreach (var mapping in fromMappings)
            {
                this.CreateMap(mapping.Source, mapping.Destination);
            }
        }

        private void RegisterCustomMappings(IEnumerable<Type> types)
        {
            var customMappings =
                types.Where(t => t.IsClass
                            && !t.IsAbstract
                            && typeof(IHaveCustomMappings).IsAssignableFrom(t))
                      .Select(t => (IHaveCustomMappings)Activator.CreateInstance(t));

            foreach (var mapping in customMappings)
            {
                mapping.Configure(this);
            }
        }

        private IEnumerable<Type> GetTypes()
        {
            List<Type> types = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.GetName().Name.Contains("Mp3MusicZone"))
                .SelectMany(a => a.GetTypes())
                .ToList();

            return types;
        }
    }
}
