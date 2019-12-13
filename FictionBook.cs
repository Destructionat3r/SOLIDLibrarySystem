namespace SOLIDLibrarySystem
{
    //Fiction book class to seperate fiction books from the non-fiction
    class FictionBook : Book
    {
        public FictionBook( BookType bookType, string title, string author, string publisher, string dateOfPublication, string category) : base (bookType, title, author, publisher, dateOfPublication, category)
        {

        }
    }
}