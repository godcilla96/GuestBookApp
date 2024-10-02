using System;
using Newtonsoft.Json; //Newtonsoft installterat för att läsa in jsondata 

namespace GuestBookApp
{
    class Program
    {
        static List<GuestBookEntry> guestBookEntries = new List<GuestBookEntry>();
        static string fileName = "guestbook.json";

        static void Main(string[] args)
        {
            //laddar in menyn med olika alternativ
            LoadEntriesFromFile();
            bool running = true;
            while (running)
            {
                //menyn
                Console.Clear();
                Console.WriteLine("Välkommen till Gästboken");
                Console.WriteLine("1. Visa alla inlägg");
                Console.WriteLine("2. Lägg till ett inlägg");
                Console.WriteLine("3. Ta bort ett inlägg");
                Console.WriteLine("4. Avsluta");
                Console.Write("Välj ett alternativ: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowEntries(true); // Här vill vi att användaren ska trycka på en tangent
                        break;
                    case "2":
                        AddEntry();
                        break;
                    case "3":
                        RemoveEntry();
                        break;
                    case "4":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }
            }
        }

        //koden för att visa alla sparade inlägg
        static void ShowEntries(bool waitForKeyPress)
        {
            Console.Clear();
            Console.WriteLine("Gästbokens inlägg:\n");
            if (guestBookEntries.Count == 0)
            {
                Console.WriteLine("Inga inlägg att visa.");
            }
            else
            {
                for (int i = 0; i < guestBookEntries.Count; i++)
                {
                    Console.WriteLine($"{i}. {guestBookEntries[i].Owner}: {guestBookEntries[i].Message}");
                }
            }

            //väntar bara på en tangenttryckning om waitForKeyPress är true
            if (waitForKeyPress)
            {
                Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn.");
                Console.ReadKey();
            }
        }

        //lägg till inlägg
        static void AddEntry()
        {
            Console.Clear();
            
            //validering och felhantering genom while-loop, inläggsägare
            string owner;
            do
            {
                Console.Write("Ange ägare till inlägget: ");
                owner = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(owner))
                {
                    Console.WriteLine("Fel: Ägaren får inte vara tom. Försök igen.");
                }
            } while (string.IsNullOrWhiteSpace(owner));

            //validering och felhantering genom while-loop, inläggsinnehåll
            string message;
            do
            {
                Console.Write("Ange inlägg: ");
                message = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(message))
                {
                    Console.WriteLine("Fel: Inlägget får inte vara tomt. Försök igen.");
                }
            } while (string.IsNullOrWhiteSpace(message));

            guestBookEntries.Add(new GuestBookEntry { Owner = owner, Message = message });
            SaveEntriesToFile();
            Console.WriteLine("Inlägget har sparats. Tryck på valfri tangent för att återgå till menyn.");
            Console.ReadKey();
        }

        //ta bort ett sparat inlägg från json-filen
        static void RemoveEntry()
        {
            Console.Clear();
            ShowEntries(false);  //skickar med false för att inte vänta på tangenttryckning

            if (guestBookEntries.Count == 0)
            {
                Console.WriteLine("\nInga inlägg att ta bort. Tryck på valfri tangent för att återgå till menyn.");
                Console.ReadKey();  //använder detta för att jag vill att användaren ska se att listan är tom
                return;
            }

            int index;
            bool validIndex = false;

            //loop för att säkerställa att användaren anger ett giltigt index, felhantering
            do
            {
                Console.Write("\nAnge index för inlägget som ska tas bort: ");
                if (int.TryParse(Console.ReadLine(), out index) && index >= 0 && index < guestBookEntries.Count)
                {
                    validIndex = true;
                }
                else
                {
                    Console.WriteLine("Fel: Ogiltigt index. Försök igen.");
                }
            } while (!validIndex);

            guestBookEntries.RemoveAt(index);
            SaveEntriesToFile();
            Console.WriteLine("Inlägget har tagits bort. Tryck på valfri tangent för att återgå till menyn.");
            Console.ReadKey();
        }

        //laddar in json-filen med alla gästboksinlägg
        static void LoadEntriesFromFile()
        {
            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                guestBookEntries = JsonConvert.DeserializeObject<List<GuestBookEntry>>(json);
            }
        }

        //sparar inläggen till json-filen
        static void SaveEntriesToFile()
        {
            string json = JsonConvert.SerializeObject(guestBookEntries, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }
    }

    class GuestBookEntry
    {
        public string Owner { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}