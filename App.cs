using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;

namespace SOLIDLibrarySystem
{
    class App
    {
        private string filetype = "JSON";
        private LibraryHelper libraryHelper = new LibraryHelper();
        private List<Book> books = new List<Book>();
        private List<Author> authorList = new List<Author>();

        public App()
        {

        }

        public void Run()
        {
            libraryHelper.SetNonFictionCategories();
            libraryHelper.SetFictionCategories();
            CurrentTime time = new CurrentTime();
            while (true)
            {
                Console.Clear();
                time.Update();
                time.Display();

                switch (filetype)
                {
                    case "JSON":
                        if (File.Exists(@"library.json"))
                        {
                            string exisitingData;
                            using (StreamReader reader = new StreamReader(@"library.json", Encoding.Default))
                            {
                                exisitingData = reader.ReadToEnd();
                            }
                            books = JsonConvert.DeserializeObject<List<Book>>(exisitingData);
                        }
                        else
                        {
                            books = new List<Book>();
                        }
                        break;
                    case "XML":
                        if (File.Exists(@"library.xml"))
                        {
                            var serializer = new XmlSerializer(typeof(List<Book>));
                            using (var reader = new StreamReader(@"library.xml"))
                            {
                                try
                                {
                                    books = (List<Book>)serializer.Deserialize(reader);
                                }
                                catch
                                {
                                    Console.WriteLine("Could not load file");
                                } // Could not be deserialized to this type.
                            }
                        }
                        else
                        {
                            books = new List<Book>();
                        }
                        break;
                }
                bool done = false;

                string another = Input("Add a book y/n");
                if (another == "n")
                {
                    done = true;
                }

                while (!done)
                {
                    int typeOfBook;
                    string typeChoice = "";
                    int categoryCounter = 0;
                    int displayCategory = 0;
                    string type = "";
                    string selectedCategory = "";

                    Console.Clear();
                    Console.WriteLine("Do you want to enter a fiction or non fiction book?");
                    Console.WriteLine("0: Non-fiction");
                    Console.WriteLine("1: Fiction");
                    while(!int.TryParse(Console.ReadLine(), out typeOfBook))
                    {
                        Console.WriteLine("Option not available. Please try again");
                    }

                    switch(typeOfBook)
                    {
                        case 0:
                            type = "NonFiction";
                            categoryCounter = libraryHelper.NonFictionCategories.Count;
                            break;
                        case 1:
                            type = "Fiction";
                            categoryCounter = libraryHelper.FictionCategories.Count;
                            break;
                    }
                    Console.Clear();

                    Console.WriteLine("Select a category:");
                    for (displayCategory = 0; displayCategory < categoryCounter; displayCategory++)
                    {
                        if (typeOfBook == 0)
                        {
                            typeChoice = libraryHelper.NonFictionCategories[displayCategory];
                        }
                        if (typeOfBook == 1)
                        {
                            typeChoice = libraryHelper.FictionCategories[displayCategory];
                        }
                        Console.WriteLine(displayCategory + ": " + typeChoice);
                    }

                    int selectedCategoryID = 0;
                    bool validID = false;
                    do
                    {
                        try
                        {
                            selectedCategoryID = Convert.ToInt32(Console.ReadLine());
                            if (selectedCategoryID >= 0 && selectedCategoryID < categoryCounter)
                            {
                                validID = true;
                            }
                            else
                            {
                                Console.WriteLine("Option not available. Please try again");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            Console.WriteLine("Please try again");
                        }
                    } while (!validID);

                    if (typeOfBook == 0)
                    {
                        selectedCategory = libraryHelper.NonFictionCategories[selectedCategoryID];
                    }
                    if (typeOfBook == 1)
                    {
                        selectedCategory = libraryHelper.FictionCategories[selectedCategoryID];
                    }
                    Console.WriteLine("You have selected {0}", selectedCategory);

                    string title = Input("Title");
                    int noOfAuthors = 0;
                    Console.Write("Number of authors: ");
                    while(!int.TryParse(Console.ReadLine(), out noOfAuthors))
                    {
                        Console.WriteLine("Please enter a number");
                    }
                    for (int i = 0; i < noOfAuthors; i++)
                    {
                        string authors = Input("Author");
                        authorList.Add(new Author(authors));
                    }
                    string publisher = Input("Publisher");
                    string dateOfPublication = Input("Date of publication");

                    string author = string.Join(", ", authorList.Select( o => o.Name).ToArray<string>());
                    Console.WriteLine(author);

                    if (typeOfBook == 0)
                    {
                        NonFictionBook nonFictionBook = new NonFictionBook(BookType.NonFiction, title, author, publisher, dateOfPublication, selectedCategory);
                        books.Add(nonFictionBook);
                    }
                    if (typeOfBook == 1)
                    {
                        FictionBook fictionBook = new FictionBook(BookType.Fiction, title, author, publisher, dateOfPublication, selectedCategory);
                        books.Add(fictionBook);
                    }

                    another = Input("Add another? y/n");
                    if (another == "n")
                    {
                        done = true;
                    }

                };

                Console.Clear();
                Console.WriteLine("All books in library\n");            
                //bool firstPass = true;
                
                foreach (Book book in books)
                {                    
                    book.Display();
                }
                
                /*
                Book book = new Book();

                for(int i = 0; i < books.Count; i++)
                {
                    if ((int)books[i].bookType == 0)
                    {
                        if (firstPass == true)
                        {
                            Console.WriteLine("Non-Fiction Books");
                            firstPass = false;
                        }
                        book.Display();
                    }
                }

                for(int i = 0; i < books.Count; i++)
                {
                    if ((int)books[i].bookType == 1)
                    {
                        if (firstPass == true)
                        {
                            Console.WriteLine("Fiction Books");
                            firstPass = false;
                        }
                        book.Display();
                    }
                }
                */
                
                if (filetype == "JSON")
                {
                    using (StreamWriter file = File.CreateText(@"library.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(file, books);
                    }
                }

                if (filetype == "XML")
                {
                    var serializer = new XmlSerializer(typeof(List<Book>));
                    using (var writer = new StreamWriter(@"library.xml"))
                    {
                        serializer.Serialize(writer, books);
                    }

                }

                //Console.WriteLine(itemsSerialized);
                Console.ReadKey(true);
            }
        }
        public static string Input(string prompt)
        {
            Console.Write(prompt + ": ");
            return Console.ReadLine();
        }
    }
}

