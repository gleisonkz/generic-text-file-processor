using ConsoleUI.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleUI
{
    class Program
    {
        static void Main()
        {
            Console.Write("Pressione alguma tecla para iniciar...");
            Console.ReadLine();

            DemonstrateTextFileStorage();

            Console.WriteLine();
            Console.Write("Pressione alguma tecla para finalizar...");
            Console.ReadLine();
        }

        private static void DemonstrateTextFileStorage()
        {
            var baseDirectory = @"C:\Temp";
            var peopleList = new List<Person>();
            var logList = new List<Log>();
            var bookList = new List<Book>();

            string peoplesFilePath = $@"{baseDirectory}\people.csv";
            string logsFilePath = $@"{baseDirectory}\logs.csv";
            string booksFilePath = $@"{baseDirectory}\books.csv";

            CreateBaseFolderDirectory(baseDirectory);
            PopulateLists(peopleList, logList, bookList);

            GenericTextFileProcessor.SaveToTextFile<Person>(peopleList, peoplesFilePath);
            GenericTextFileProcessor.SaveToTextFile<Log>(logList, logsFilePath);
            GenericTextFileProcessor.SaveToTextFile<Book>(bookList, booksFilePath);

            var peopleListFromFile = GenericTextFileProcessor.LoadFromTextFile<Person>(peoplesFilePath);
            var logListFromFile = GenericTextFileProcessor.LoadFromTextFile<Log>(logsFilePath);
            var bookListFromFile = GenericTextFileProcessor.LoadFromTextFile<Book>(booksFilePath);

            foreach (var p in peopleListFromFile)
            {
                Console.WriteLine($"{ p.FirstName } { p.LastName } (IsAlive = { p.IsAlive })");
            }

            foreach (var log in logListFromFile)
            {
                Console.WriteLine($"{ log.ErrorCode }: { log.Message } at { log.TimeOfEvent.ToShortTimeString() }");
            }

            foreach (var book in bookListFromFile)
            {
                Console.WriteLine($"{ book.Name }: { book.Genre } at { book.Year }");
            }
        }

        private static void CreateBaseFolderDirectory(string folderPath)
        {
            Directory.CreateDirectory(folderPath);
        }

        private static void PopulateLists(List<Person> peoples, List<Log> logs, List<Book> books)
        {
            peoples.Add(new Person { FirstName = "Tim", LastName = "Corey" });
            peoples.Add(new Person { FirstName = "Sue", LastName = "Storm", IsAlive = false });
            peoples.Add(new Person { FirstName = "Greg", LastName = "Olsen" });

            logs.Add(new Log { Message = "I blew up", ErrorCode = 9999 });
            logs.Add(new Log { Message = "I'm too awesome", ErrorCode = 1337 });
            logs.Add(new Log { Message = "I was tired", ErrorCode = 2222 });

            books.Add(new Book { Name = "Harry Potter", Genre = "Magic", Year = 1993 });
            books.Add(new Book { Name = "Lord of Rings", Genre = "Action", Year = 2015 });
            books.Add(new Book { Name = "Alice in the worldland", Genre = "Adventure", Year = 2000 });
        }
    }
}
