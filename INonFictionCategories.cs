using System.Collections.Generic;

namespace SOLIDLibrarySystem
{
    public interface INonFictionCategories
    {
        List<string> FictionCategories { get; set; }
        void SetNonFictionCategories();        
    }
}