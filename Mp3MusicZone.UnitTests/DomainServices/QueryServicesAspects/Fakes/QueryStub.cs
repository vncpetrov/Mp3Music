namespace Mp3MusicZone.UnitTests.DomainServices.QueryServicesAspects.Fakes
{
    using Mp3MusicZone.Domain.Attributes;
    using Mp3MusicZone.DomainServices.Contracts;
    using System;

    [Permission("SomeId")]
    public class QueryStub : IQuery<object>
    {
    }
}
