namespace Mp3MusicZone.Web.ViewModels
{
    using System;

    public class SearchViewModel<T> : ISearchViewModel
        where T : class
    {
        public SearchViewModel(
            T decoratedModel,
            string searchTerm,
            string searchIn)
        {
            if (decoratedModel is null)
                throw new ArgumentNullException(nameof(decoratedModel));

            this.DecoratedModel = decoratedModel;
            this.SearchTerm = searchTerm;
            this.SearchIn = searchIn;
        }

        public T DecoratedModel { get; set; }

        public string SearchTerm { get; set; }

        public string SearchIn { get; set; }
    }
}
