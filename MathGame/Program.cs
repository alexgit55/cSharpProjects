/*
 * This is a simple math game that asks the user to solve a simple addition problem.
 * The user is asked if they want to play again after each round.
 */


bool keepPlaying = true;
int PlayerScore = 0;
int lowerRange = 1;
int upperRange = 11;
int TotalQuestions = 10;

while (keepPlaying)
{   
    Console.Clear();
    Console.WriteLine("Welcome to the Math Game!\n");
    char userSelection = GameMenu();
    int menuSelection = userSelection - '0';

    switch (menuSelection)
    {
        case 1:
            Console.WriteLine("You selected Addition");
            GameRound(1);
            break;
        case 2:
            Console.WriteLine("You selected Subtraction");
            GameRound(2);
            break;
        case 3:
            Console.WriteLine("You selected Multiplication");
            GameRound(3);
            break;
        case 4:
            Console.WriteLine("You selected Division");
            GameRound(4);
            break;
        case 5:
            Console.WriteLine("You selected Random");
            break;
        case 6:
            Console.WriteLine("You selected History");
            break;
        case 7:
            Console.WriteLine("Thanks for Playing! Goodbye!");
            keepPlaying = false;
            break;
    }
}

Console.ReadKey();

char GameMenu()
    // Display the game menu and return the user's selection
{
    char selection='0';
    char[] validSelections = {'1','2','3','4','5','6','7'};

    while (Array.IndexOf(validSelections, selection) == -1)
    {
        Console.WriteLine("Please select the math operation(s) you want to focus on: ");
        Console.WriteLine("1. Addition");
        Console.WriteLine("2. Subtraction");
        Console.WriteLine("3. Multiplication");
        Console.WriteLine("4. Division");
        Console.WriteLine("5. Random");
        Console.WriteLine("6. History");
        Console.WriteLine("7. Exit");

        selection = Console.ReadKey().KeyChar;
        Console.WriteLine();
    }
    return selection;
}

void GameRound(int selection, int difficulty=1, int totalQuestions=10 )
{
    int lowerRange = 1;
    int upperRange = 21;
    int PlayerScore = 0;
    for (int i = 1; i <= totalQuestions; i++)
    {
        Console.WriteLine($"Question {i} of {totalQuestions}");
        PlayerScore += MathQuestion(lowerRange, upperRange, selection);
    }
    Console.WriteLine($"Your final score was {PlayerScore} out of {totalQuestions} correct.");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();

}

int MathQuestion(int lowerRange, int upperRange, int selection)
{
    bool correct = false;
    switch (selection)
    {
        case 1:
            correct = Addition(lowerRange, upperRange);
            break;
        case 2:
            correct = Subtraction(lowerRange, upperRange);
            break;
        case 3:
            correct = Multiplication(lowerRange, upperRange);
            break;
        case 4:
            correct = Division(lowerRange, upperRange);
            break;
        case 5:
            Console.WriteLine("You selected Random");
            break;
    }

    if (correct)
        return 1;
    else
        return 0;
}

bool Addition(int lowerRange, int upperRange)
{
    int userAnswer;
    // Generate two random numbers between lower range and upper range
    Random rand = new Random();
    int num1 = rand.Next(lowerRange, upperRange);
    int num2 = rand.Next(lowerRange, upperRange);

    //Ask the user to solve the addition problem
    Console.WriteLine($"What is the sum of {num1} + {num2}?");

    while (!int.TryParse(Console.ReadLine(), out userAnswer))
    {
        Console.WriteLine("Please enter a valid number.");
    }

    // Check if the user's answer is correct
    int correctAnswer = num1 + num2;
    if (userAnswer == correctAnswer)
    {
        Console.WriteLine("Correct! Good job!\n");
        return true;
    }
    else
    {
        Console.WriteLine($"Incorrect. The correct answer is {correctAnswer}\n");
        return false;
    }
    
}

bool Subtraction(int lowerRange, int upperRange)
{
    // Generate two random numbers between 1 and 10
    int num1;
    int num2;
    int userAnswer;
    do {
        Random rand = new Random();
        num1 = rand.Next(lowerRange, upperRange);
        num2 = rand.Next(lowerRange, upperRange);
    } while (num1 <= num2);

    // Ask the user to solve the subtraction problem
    Console.WriteLine($"What is the difference of {num1} - {num2}?");

    while (!int.TryParse(Console.ReadLine(), out userAnswer))
    {
        Console.WriteLine("Please enter a valid number.");
    }

    // Check if the user's answer is correct
    int correctAnswer = num1 - num2;
    if (userAnswer == correctAnswer)
    {
        Console.WriteLine("Correct! Good job!\n");
        return true;
    }
    else
    {
        Console.WriteLine($"Incorrect. The correct answer is {correctAnswer}\n");
        return false;
    }
}

bool Multiplication(int lowerRange, int upperRange)
{
    int userAnswer;
    // Generate two random numbers between lowerRange and upperRange

    Random rand = new Random();
    int num1 = rand.Next(lowerRange, upperRange);
    int num2 = rand.Next(lowerRange, upperRange);

    // Ask the user to solve the multiplication problem
    Console.WriteLine($"What is the product of {num1} * {num2}?");

    while (!int.TryParse(Console.ReadLine(), out userAnswer))
    {
        Console.WriteLine("Please enter a valid number.");
    }

    // Check if the user's answer is correct
    int correctAnswer = num1 * num2;
    if (userAnswer == correctAnswer)
    {
        Console.WriteLine("Correct! Good job!\n");
        return true;
    }
    else
    {
        Console.WriteLine($"Incorrect. The correct answer is {correctAnswer}\n");
        return false;
    }
}

bool Division(int lowerRange, int upperRange)
{
    int userAnswer;
    int num1 = 0;
    int num2 = 1;
    // Generate two random numbers between lowerRange and upperRange
    // Ensure that num1 is divisible by num2
    do
    {
        Random rand = new Random();
        num1 = rand.Next(1, 11);
        num2 = rand.Next(1, 11);
    } while (num1%num2 != 0);

    // Ask the user to solve the addition problem
    Console.WriteLine($"What is the quotient of {num1} / {num2}?");

    while (!int.TryParse(Console.ReadLine(), out userAnswer))
    {
        Console.WriteLine("Please enter a valid number.");
    }

    // Check if the user's answer is correct
    int correctAnswer = num1 / num2;
    if (userAnswer == correctAnswer)
    {
        Console.WriteLine("Correct! Good job!\n");
        return true;
    }
    else
    {
        Console.WriteLine($"Incorrect. The correct answer is {correctAnswer}\n");
        return false;
    }
}