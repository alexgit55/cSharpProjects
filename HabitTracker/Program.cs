using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker
{
    public class Habit
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }

    internal class Program
    {
        static string connectionString = @"Data Source=habit_tracker.db";

        static void Main(string[] args)
        {

            RunCommandOnDatabase(@"CREATE TABLE IF NOT EXISTS habits (id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, Quantity INTEGER)");

            GetUserInput();
        }

        public static void RunCommandOnDatabase(string commandText)
        // Create a connection to the database and execute a command
        {

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = commandText;
            command.ExecuteNonQuery();

            connection.Close();
        }

        static void GetUserInput()
        // Display the main menu and get user input
        {
            Console.Clear();

            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\n0. Close Application");
                Console.WriteLine("1. View All Records");
                Console.WriteLine("2. Add New Record");
                Console.WriteLine("3. Delete A Record");
                Console.WriteLine("4. Update a Record");
                Console.WriteLine("------------------------------\n");

                var userInput = Console.ReadKey();
                Console.WriteLine("\n");

                switch (userInput.KeyChar)
                {
                    case '0':
                        Console.WriteLine("Closing Application. Goodbye!");
                        closeApp = true;
                        break;
                    case '1':
                        ViewAllRecords();
                        break;
                    case '2':
                        InsertRecord();
                        break;
                    case '3':
                        DeleteRecord();
                        break;
                    case '4':
                        UpdateRecord();
                        break;
                    default:
                        Console.WriteLine("\nInvalid input. Please try again.");
                        break;
                }
                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();
                Console.Clear();

            }
        }

        private static void ViewAllRecords()
        // Display all records in the database
        {
            Console.Clear();
            Console.WriteLine("Viewing all records\n");
            string commandText = $"SELECT * FROM habits";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = commandText;

            List<Habit> habits = new();

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    habits.Add(new Habit
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd", new CultureInfo("en-us")),
                        Quantity = reader.GetInt32(2)
                    });
                }

            }
            else
                Console.WriteLine("No records found");

            connection.Close();
            Console.WriteLine("--------------------------------");
            Console.WriteLine("ID\tDate\t\tQuantity");
            Console.WriteLine("--------------------------------");
            foreach (var habit in habits)
            {
                Console.WriteLine($"{habit.Id}\t{habit.Date:yyyy-MM-dd}\t{habit.Quantity}");
            }
        }

        private static string GetDateInput()
        // Get a date input from the user. Return to main menu if 0 is entered, otherwise return the date
        {
            bool validDate = false;
            DateTime date = new();
            while (validDate == false)
            {
                Console.WriteLine("Please enter the date: (Format: yyyy-MM-dd). Type 0 to return to main menu.");
                var userInput = Console.ReadLine();
                if (userInput == "0")
                    return "0";
                validDate = DateTime.TryParseExact(userInput, "yyyy-MM-dd", new CultureInfo("en-us"), DateTimeStyles.None, out date);

                if (validDate == true)
                    break;

                Console.WriteLine("Invalid Date format, please try again.");
            }
            
            
            return date.ToString("yyyy-MM-dd");

        }

        private static int GetNumberInput(string message)
        // Get a number input from the user, keep asking until a valid number is entered. Return to main menu if 0 is entered, otherwise return the number
        {
            Console.WriteLine(message);
            int quantity = -1;
            bool validInput = false;

            while (validInput == false)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out quantity))
                    validInput = true;
                else
                    Console.WriteLine("Invalid input. Please try again.");
            }

            return quantity;
        }

        private static void InsertRecord()
        // Insert a new record into the database
        {
            Console.Clear();
            Console.WriteLine("\nAdd New Record to Database\n");
            Console.WriteLine("----------------------------\n\n");
            string date = GetDateInput();
            if (date == "0")
            {
                Console.WriteLine("No records will be added.");
                return;
            }

            int quantity = GetNumberInput("Please insert number of glasses of water drank (no decimals allowed, type 0 to return to main menu): ");

            if (quantity == 0)
            {
                Console.WriteLine("No records will be added.");
                return;
            }

            string commandText = $"INSERT INTO habits (Date, Quantity) VALUES ('{date}', {quantity})";
            RunCommandOnDatabase(commandText);

            Console.WriteLine("Record added successfully.\n\n");
        }
        private static void DeleteRecord()
        // Delete a record from the database
        {
            Console.Clear();
            ViewAllRecords();

            int id = GetNumberInput("\nPlease enter the ID of the record you want to delete (type 0 to return to main menu): ");

            string commandText = $"DELETE FROM habits WHERE id = {id}";

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = commandText;
            int rowCount = command.ExecuteNonQuery();

            if (rowCount == 0)
                Console.WriteLine($"No record with ID {id} found. No records were deleted.\n\n");
            else
                Console.WriteLine($"Record with ID {id} was deleted.\n\n");

            connection.Close();
        }

        private static void UpdateRecord()
        // Update a record in the database
        {
            Console.Clear();
            ViewAllRecords();

            int id = GetNumberInput("\nPlease enter the ID of the record you want to update (type 0 to return to main menu): ");

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE Id = {id})";
            int checkQuery = Convert.ToInt32(checkCommand.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"No record with ID {id} found. No records were updated.\n\n");
                connection.Close();
                return;
            }

            string date = GetDateInput();
            int quantity = GetNumberInput("\n\nPlease insert number of glasses of water drank (no decimals allowed, type 0 to return to main menu): ");

            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText = $"UPDATE habits SET Date = '{date}', Quantity = {quantity} WHERE Id = {id}";

            tableCommand.ExecuteNonQuery();

            Console.WriteLine($"Record with id of {id} has been updated.");

            connection.Close();

        }
    }
}
