namespace Mp3MusicZone.DomainServices.QueryServices
{
    using Contracts;
    using System;

    public class SearchInfo
    {
        private string searchTerm;

        public string SearchTerm
        {
            get => this.searchTerm is null ? string.Empty : this.searchTerm;

            set => this.searchTerm = value;
        }
    }
}
