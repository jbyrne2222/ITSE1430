/* 
 * ITSE 1430
 * Jonathan Byrne
 * Lab 1
 * 2/7/2018
*/

using System;

namespace JonathanByrne.MovieLib.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            bool quit = false;
            while (!quit)
            {
                bool isEqual = quit.Equals(10);

                //Display the menu
                char choice = DisplayMenu();

                //Process user's menu choice
                switch (Char.ToUpper(choice))
                {
                    case 'L': ListMovies(); break;

                    case 'A': AddMovie(); break;

                    case 'R': RemoveMovie(); break;

                    case 'Q': quit = true; break;
                };
            };
        }
        
        //Gets information from user to add to movie database
        static void AddMovie ()
        {
            _title = ReadString("Please enter movie title: ", true);

            _description = ReadString("Enter an optional description: ", false);

            _length = ReadInt("Enter length of movie in minutes (optional): ", 0);

            _owned = ReadBool("Do you own this movie? (Y/N) ", true);
        }

        //Reads user input string for movie title and description
        private static string ReadString(string message, bool isRequired)
        {
            do
            {
                Console.Write(message);

                string value = Console.ReadLine();

                //If not required or not empty
                if (!isRequired || value != "")
                    return value;

                Console.WriteLine("Value is required");
            } while (true);
        }

        //Reads user input int for movie length in minutes
        private static int ReadInt( string message, int minValue)
        {
            do
            {
                Console.Write(message);

                string value = Console.ReadLine();

                if(value == "")
                {
                    return 0;
                }
                else if (Int32.TryParse(value, out int result))
                {
                    if (result >= minValue)
                        return result;
                };

                string msg = String.Format("Value must be >= {0}", minValue);
                Console.WriteLine(msg);

            } while (true);
        }

        //Reads user input "Y" or "N" as true or false respectively
        private static bool ReadBool(string message, bool isRequired)
        {
            do
            {
                Console.Write(message);

                string input = Console.ReadLine();

                input = input.Trim();
                input = input.ToUpper();

                bool owned = true;

                if (input == "Y")
                {
                    return owned;
                }
                else if (input == "N")
                {
                    owned = false;
                    return owned;
                }
                else if(input != "Y" || input != "N")
                {
                    Console.WriteLine("You must enter Y or N");
                }
                
            } while (true);
        }

        //Displays menu in program
        private static char DisplayMenu()
        {
            do
            {
                Console.WriteLine("L)ist Movies");
                Console.WriteLine("A)dd Movie");
                Console.WriteLine("R)emove Movie");
                Console.WriteLine("Q)uit");

                string input = Console.ReadLine();

                input = input.Trim();

                input = input.ToUpper();

                if (String.Compare(input, "L", true) == 0)
                    return input[0];
                else if (input == "A")
                    return input[0];
                else if (input == "R")
                    return input[0];
                else if (input == "Q")
                    return input[0];

                Console.WriteLine("Please choose a valid option");
            } while (true);
        }

        //Removes Movie from database by replacing with null value
        static void RemoveMovie()
        {
            if(!String.IsNullOrEmpty(_title))
            {
                Console.WriteLine("Are you sure you want to delete the movie (Y/N)? ");
                string input = Console.ReadLine();

                input = input.Trim();
                input = input.ToUpper();

                if (input == "Y")
                {
                    _title = null;
                    _description = null;
                    _length = 0;
                } else if (input != "Y")
                    return;

            }
        }

        //Lists saved user input data on movies
        static void ListMovies()
        {
            if(!String.IsNullOrEmpty(_title))
            {
                Console.WriteLine(_title);

                if (!String.IsNullOrEmpty(_description))
                    Console.WriteLine(_description);

                Console.WriteLine("Length of Movie: " + _length);
                if(_owned == true)
                {
                    Console.WriteLine("Status: Owned");
                }
                else if (_owned == false)
                {
                    Console.WriteLine("Status: On Wishlist");
                }
            }
            else if(String.IsNullOrEmpty(_title))
            {
                Console.WriteLine("No movies available.");
            }
        }

        //Movie data
        static string _title;
        static string _description;
        static int _length;
        static bool _owned;
    }

 
}
