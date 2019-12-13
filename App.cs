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
        private LibraryHelper libraryHelper = new LibraryHelper();
        public List<Book> books = new List<Book>();
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
                //time.Update();
                //time.Display();
                int filetype;

                do
                {
                    //Ask the user if they want to import a JSON, XML file or neither
                    Console.Clear();
                    Console.WriteLine("Would you like to import a file?");
                    Console.WriteLine("0: JSON");
                    Console.WriteLine("1: XML");
                    Console.WriteLine("2: No import");
                    while(!int.TryParse(Console.ReadLine(), out filetype))
                    {
                        Console.WriteLine("Option not available. Please try again");
                    }
                } while(filetype != 0 && filetype != 1 && filetype != 2);
                Console.Clear();
                switch (filetype)
                {
                    case 0:
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
                    case 1:
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
                    case 2:
                        books = new List<Book>();
                        break;
                }
                bool done = false;
                string another = "";
                do
                {
                    another = Input("Add a book y/n");
                    another = another.ToUpper();
                    
                } while(another != "Y" && another != "N");

                if (another == "N")
                {
                    done = true;
                }

                while (!done)
                {
                    int typeOfBook;
                    string typeChoice = "";
                    int categoryCounter = 0;
                    int displayCategory = 0;
                    string selectedCategory = "";

                    do
                    {
                        //Allow the user to enter non-fiction books into the system
                        Console.Clear();
                        Console.WriteLine("Do you want to enter a fiction or non fiction book?");
                        Console.WriteLine("0: Non-fiction");
                        Console.WriteLine("1: Fiction");
                        while(!int.TryParse(Console.ReadLine(), out typeOfBook))
                        {
                            Console.WriteLine("Option not available. Please try again");
                        }
                    }while(typeOfBook != 0 && typeOfBook != 1);

                    switch(typeOfBook)
                    {
                        case 0:
                            categoryCounter = libraryHelper.NonFictionCategories.Count;
                            break;
                        case 1:
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
                        //Allow for multiple authors to be entered
                        string authors = Input("Author");
                        authorList.Add(new Author(authors));
                    }
                    string publisher = Input("Publisher");
                    string dateOfPublication = Input("Date of publication");
                    //Put all of the author list into a single string seperated by a comma
                    string author = string.Join(", ", authorList.Select( o => o.Name).ToArray<string>());

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
                bool firstPass = true;
                int counter = 0;
                
                //Print all the non-fiction and then the fiction books
                foreach (Book book in books)
                {                    
                    if ((int)books[counter].bookType == 0)
                    {
                        if (firstPass == true)
                        {
                            Console.WriteLine("Non-Fiction Books");
                            firstPass = false;
                        }
                        book.Display();
                    }
                    counter++;
                }

                firstPass = true;
                counter = 0;
                Console.WriteLine();

                foreach (Book book in books)
                {                    
                    if ((int)books[counter].bookType == 1)
                    {
                        if (firstPass == true)
                        {
                            Console.WriteLine("Fiction Books");
                            firstPass = false;
                        }
                        book.Display();
                    }
                    counter++;
                }
                
                if (filetype == 0)
                {
                    using (StreamWriter file = File.CreateText(@"library.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(file, books);
                    }
                }

                if (filetype == 1)
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

