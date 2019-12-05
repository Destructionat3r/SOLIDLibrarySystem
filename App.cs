using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;

namespace library_system
{
    class App
    {
        private string filetype = "JSON";
        private LibraryHelper libraryHelper = new LibraryHelper();
        private List<Book> books;

        public App()
        {

        }

        public void Run()
        {
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
                    Console.Clear();
                    Console.WriteLine("Select a category:");
                    for (int i = 0; i < libraryHelper.Categories.Count; i++)
                    {
                        Console.WriteLine(i + ": " + libraryHelper.Categories[i]);
                    }

                    int selectedCategoryID = 0;
                    bool validID = false;
                    do
                    {
                        try
                        {
                            selectedCategoryID = Convert.ToInt32(Console.ReadLine());
                            if (selectedCategoryID >= 0 && selectedCategoryID < libraryHelper.Categories.Count)
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

                    string selectedCategory = libraryHelper.Categories[selectedCategoryID];
                    Console.WriteLine("You have sected {0}", selectedCategory);

                    string title = Input("Title");
                    string author = Input("Author");
                    string publisher = Input("Publisher");
                    string dateOfPublication = Input("Date of publication");

                    books.Add(new Book(title, author, publisher, dateOfPublication, selectedCategory));

                    another = Input("Add another? y/n");
                    if (another == "n")
                    {
                        done = true;
                    }

                };

                Console.Clear();
                Console.WriteLine("All books in library\n");
                foreach (var book in books)
                {
                    book.Display();
                }


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

