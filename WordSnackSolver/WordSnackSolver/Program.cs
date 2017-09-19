using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace WordSnackSolver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var userCommand = new ConsoleKeyInfo();
            var lettersToSearch = GetSearchedLetters();
            var letterCount = GetNumberOfLetters();
            DoSearch(lettersToSearch, letterCount);

            do
            {
                Console.WriteLine("\n Press");
                Console.WriteLine("c : for change searched word expected letter count");
                Console.WriteLine("l : for change letters to be searched");
                Console.WriteLine("n : to start a new search");
                Console.WriteLine("e : to exit");

                userCommand = Console.ReadKey();

                switch (userCommand.Key)
                {
                    case ConsoleKey.C:
                        {
                            var newLetterCount = GetNumberOfLetters();
                            letterCount = newLetterCount;
                            DoSearch(lettersToSearch, letterCount);
                        }
                        break;

                    case ConsoleKey.L:
                        {
                            var newLetters = GetSearchedLetters();
                            lettersToSearch = newLetters;
                            DoSearch(lettersToSearch, letterCount);
                        }
                        break;

                    case ConsoleKey.N:
                        DoNewSearch();
                        break;

                    default:
                        DoNewSearch();
                        break;
                }
            }
            while (userCommand.Key != ConsoleKey.E);
        }

        private static void DoNewSearch()
        {
            var lettersToSearch = GetSearchedLetters();
            var letterCount = GetNumberOfLetters();
            DoSearch(lettersToSearch, letterCount);
        }

        private static void DoSearch(List<char> lettersToSearch, int letterCount)
        {
            var results = GetResults(lettersToSearch, letterCount);

            if (results.Count > 0)
            {
                PrintResults(results);
            }
            else
            {
                Console.WriteLine("no words found :(");
            }
        }

        private static string DictionaryWordsPath
        {
            get
            {
                var fileName = "sk.txt";
                var folderPath = Environment.CurrentDirectory;
                return Path.Combine(folderPath, fileName);
            }
        }

        private static List<char> GetSearchedLetters()
        {
            Console.Clear();
            Console.WriteLine("Provide letters separated by comma");
            var userInput = Console.ReadLine().Split(',');
            var lettersToSearch = new List<char>();

            foreach (var item in userInput)
            {
                var trimmed = item.Trim();
                if (trimmed.Length == 1)
                    lettersToSearch.Add(trimmed[0]);
            }

            return lettersToSearch;
        }

        private static int GetNumberOfLetters()
        {
            Console.Clear();
            Console.WriteLine("Provide number of letters in word");
            return Int32.Parse(Console.ReadLine());
        }

        private static List<string> GetResults(List<char> lettersToSearch, int letterCount)
        {
            var wordsFromDictionary = GetDataFromDictionaryFile(DictionaryWordsPath);
            var results = new List<string>();
            var wordsWithMatchingLetterCount = wordsFromDictionary.Where(a => a.Count() == letterCount);

            foreach (var word in wordsWithMatchingLetterCount)
            {
                var isCandidate = true;

                //check if every character is contained in word
                foreach (var wordCh in word)
                {
                    if (lettersToSearch.Contains(wordCh))
                    {
                        isCandidate = true;
                    }
                    else
                    {
                        isCandidate = false;
                        break;
                    }
                }

                if (isCandidate)
                {
                    var isFound = true;

                    //check if every char is in the word only once
                    foreach (var ch in word)
                    {
                        if (word.Count(w => w == ch) == 1)
                        {
                            isFound = true;
                        }
                        else
                        {
                            isFound = false;
                            break;
                        }
                    }

                    if (isFound)
                    {
                        //got ya
                        results.Add(word);
                    }
                }
            }

            return results;
        }

        private static void PrintResults(List<string> words)
        {
            foreach (var word in words)
            {
                Console.WriteLine(word);
            }
        }

        private static List<string> GetDataFromDictionaryFile(string path)
        {
            String[] csv = File.ReadAllLines(path);
            var res = new List<string>();

            foreach (string csvrow in csv)
            {
                var fields = csvrow.Split(' ');
                res.Add(fields[0]);
            }

            return res;
        }
    }
}