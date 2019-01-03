namespace Mp3MusicZone.Web.ViewModels.Shared
{
    using System; 

    public class SearchViewModel<TModel> : ISearchViewModel
        where TModel : class
    {
        public SearchViewModel(
            TModel decoratedModel,
            string searchTerm,
            string searchIn)
        {
            if (decoratedModel is null)
                throw new ArgumentNullException(nameof(decoratedModel));

            this.DecoratedModel = decoratedModel;
            this.SearchTerm = searchTerm;
            this.SearchIn = searchIn;
        }

        public TModel DecoratedModel { get; set; }

        public string SearchTerm { get; set; }

        public string SearchIn { get; set; }
    }
}
