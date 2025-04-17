using System;
using System.Threading;

namespace ConsoleProjects
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Calculator();
            //  RandomNumberGame();
            GuessNumber();

        }

        private static void GuessNumber()
        {
            Console.WriteLine("Welcome to Guess Number!!!!");

          

            Random random = new Random();
            int randomeNumber = random.Next(1,random.Next(1, 100));

          
            int chances = 10;
            while(chances > 0)
            {
                Console.WriteLine("Enter a number");
                int number = Convert.ToInt32(Console.ReadLine());
                if (number == randomeNumber)
                {
                    Console.WriteLine($"You Won!!! with chances left are {chances}");
                    return;
                }
                else if (number > randomeNumber)
                {
                    Console.WriteLine("TOO HIGH");
                    
                }
                else
                {
                    Console.WriteLine("TOO LOW!!!");
                    //break;

                }
                chances--;
                Console.WriteLine($"Chances left: {chances}");
            }
            Console.WriteLine("You lost! The correct number was: " + randomeNumber);
        }

        private static void RandomNumberGame()
        {
            Console.WriteLine("Welcome to Dice!");
            Random random = new Random();
            int RandomNumber;
            int EnemyRandomNumber;
            int MyNumber = 0;
            int EnemyNumber = 0;

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("\nPress any key to roll the dice...");
                Console.ReadKey();

                RandomNumber = random.Next(1, 7);
                Console.WriteLine("\nYour Number is: " + RandomNumber);
                Console.WriteLine("Rolling enemy's dice...");
                Thread.Sleep(1000);

                EnemyRandomNumber = random.Next(1, 7);
                Console.WriteLine("Enemy Number is: " + EnemyRandomNumber);

                if (RandomNumber > EnemyRandomNumber)
                {
                    MyNumber++;
                    Console.WriteLine("You Win this round!");
                }
                else if (RandomNumber < EnemyRandomNumber)
                {
                    EnemyNumber++;
                    Console.WriteLine("Enemy Wins this round!");
                }
                else
                {
                    Console.WriteLine("It's a Draw for this round!");
                }
            }

            Console.WriteLine("\nGame Over!");
            if (MyNumber > EnemyNumber)
            {
                Console.WriteLine("🎉 YOU ARE THE WINNER! 🎉");
            }
            else if (EnemyNumber > MyNumber)
            {
                Console.WriteLine("😈 ENEMY WINS! 😈");
            }
            else
            {
                Console.WriteLine("🤝 It's a DRAW!");
            }

            Console.WriteLine($"\nYour Wins: {MyNumber}\nEnemy Wins: {EnemyNumber}");
        }

        private static void Calculator()
        {
            Console.WriteLine("Welcome to Calculator Game!");
            bool Play = true;
            while (Play)
            {
                Console.WriteLine("Enter Number 1: ");
                int Num1 = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter Number 2: ");
                int Num2 = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("\n+: Addition");
                Console.WriteLine("-: Subtraction");
                Console.WriteLine("*: Multiplication");
                Console.WriteLine("/: Division\n");

                string Choice = Console.ReadLine();

                switch (Choice)
                {
                    case "+":
                        Console.WriteLine("Sum : " + (Num1 + Num2));
                        break;
                    case "-":
                        Console.WriteLine("Subtraction : " + (Num1 - Num2));
                        break;
                    case "/":
                        if (Num2 == 0)
                        {
                            Console.WriteLine("Cannot divide by zero!");
                        }
                        else
                        {
                            Console.WriteLine("Division : " + (Num1 / Num2));
                        }
                        break;
                    case "*":
                        Console.WriteLine("Multiplication : " + (Num1 * Num2));
                        break;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }

                Console.WriteLine("\nDo you want to continue? (Y/N)");
                var options = Console.ReadLine();

                if (options.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    Play = true;
                }
                else
                {
                    Console.WriteLine("Thanks for playing!");
                    Play = false;
                }
            }
            Console.ReadKey();
        }
    }
}
