using System;
using System.Collections.Generic;

namespace SOLIDLibrarySystem
{
    //Class to deal with all the categories for both fiction and non-fiction books
    public class LibraryHelper : INonFictionCategories, IFictionCategories
    {
        protected List<string> nonFictionCategories = new List<string>();
        protected List<string> fictionCategories = new List<string>();
        public List<String> NonFictionCategories 
        { 
            get {return nonFictionCategories;} 
            set {nonFictionCategories = value;} 
        }
        public List<String> FictionCategories 
        { 
            get {return fictionCategories;} 
            set {fictionCategories = value;} 
        }

        public void SetNonFictionCategories()
        {
            NonFictionCategories.Add("Programming");
            NonFictionCategories.Add("Systems Analysis");
            NonFictionCategories.Add("E - Commerce");
            NonFictionCategories.Add("Interaction Design");
            NonFictionCategories.Add("Web Design");
        }
        public void SetFictionCategories()
        {
            FictionCategories.Add("Horror");
            FictionCategories.Add("Romance");
            FictionCategories.Add("Fantasy");
        }
    }
}