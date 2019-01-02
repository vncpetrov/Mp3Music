namespace Mp3MusicZone.Web.ViewModels.Shared
{
    using System;
     
    public interface ISearchViewModel
    {
        string SearchTerm { get; set; }

        string SearchIn { get; set; }
    }
}
