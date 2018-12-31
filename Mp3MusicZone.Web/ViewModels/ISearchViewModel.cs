namespace Mp3MusicZone.Web.ViewModels
{
    using System;
     
    public interface ISearchViewModel
    {
        string SearchTerm { get; set; }

        string SearchIn { get; set; }
    }
}
