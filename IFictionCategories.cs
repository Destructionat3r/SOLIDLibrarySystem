using System.Collections.Generic;

namespace SOLIDLibrarySystem
{
    //Interface to deal with all the fiction categories
    public interface IFictionCategories
    {
        List<string> FictionCategories { get; set; }
        void SetFictionCategories();        
    }
}