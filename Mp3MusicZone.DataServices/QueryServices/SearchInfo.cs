namespace Mp3MusicZone.DomainServices.QueryServices
{
    using System;

    public class SearchInfo
    {
        private string searchTerm;

        public SearchInfo(string searchTerm)
        {
            this.SearchTerm = searchTerm;
        }

        public string SearchTerm
        {
            get => this.searchTerm is null ? string.Empty : this.searchTerm;

            set => this.searchTerm = value;
        }
    }
}
