using System.Collections.Generic;

namespace SOLIDLibrarySystem
{
    public interface IFictionCategories
    {
        List<string> FictionCategories { get; set; }
        void SetFictionCategories();        
    }
}