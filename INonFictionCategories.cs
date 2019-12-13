using System.Collections.Generic;

namespace SOLIDLibrarySystem
{
    //Interface to deal with all the fiction categories
    public interface INonFictionCategories
    {
        List<string> FictionCategories { get; set; }
        void SetNonFictionCategories();        
    }
}