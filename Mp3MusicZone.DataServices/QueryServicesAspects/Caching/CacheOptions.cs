namespace Mp3MusicZone.DomainServices.QueryServicesAspects.Caching
{
    using System;

    public class CacheOptions
    {
        public CacheOptions(bool varyByUser, int absoluteDurationInSeconds)
        {
            this.VaryByUser = varyByUser;
            this.AbsoluteDurationInSeconds = absoluteDurationInSeconds;
        }

        public int AbsoluteDurationInSeconds { get; set; }

        public bool VaryByUser { get; set; }
    }
}
