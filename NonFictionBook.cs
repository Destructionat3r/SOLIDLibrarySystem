namespace SOLIDLibrarySystem
{
    //Non-fiction book class to seperate non-fiction books from the fiction
    class NonFictionBook : Book
    {
        public NonFictionBook(BookType bookType, string title, string author, string publisher, string dateOfPublication, string category) : base (bookType, title, author, publisher, dateOfPublication, category)
        {

        }
    }
}