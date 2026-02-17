using System;

namespace AdventureGame.Core
{
    // Interface for character
    public interface ICharacter
    {
        string Name { get; set; }
        int Health { get; set; }
        void TakeDamage(int amount);
        void Attack(ICharacter target);
    }

    public class Character : ICharacter
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int AttackPower { get; set; }

        public Character(string name, int health, int attackPower)
        {
            Name = name;
            Health = health;
            AttackPower = attackPower;
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health < 0) Health = 0;
            Console.WriteLine($"{Name} takes {amount} damage. Remaining health: {Health}");
        }

        public void Attack(ICharacter target)
        {
            Console.WriteLine($"{Name} attacks {target.Name} for {AttackPower} damage!");
            target.TakeDamage(AttackPower);
        }
    }

    // monster class
    public class Monster : Character
    {
        public Monster(string name, int health, int attackPower)
            : base(name, health, attackPower)
        {
        }
    }

    //potion class
    public class Potion
    {
        public string Name { get; set; }
        public int HealAmount { get; set; }

        public Potion(string name, int healAmount)
        {
            Name = name;
            HealAmount = healAmount;
        }

        public void Use(ICharacter target)
        {
            target.Health += HealAmount;
            Console.WriteLine($"{target.Name} uses {Name} and heals for {HealAmount} HP!");
        }
    }

    // health class
    public class HealthBar
    {
        public static void Display(ICharacter character)
        {
            Console.WriteLine($"{character.Name} Health: {character.Health}");
        }
    }

    //class for Maze
    public class Maze
    {
        private const int Size = 10;
        private char[,] grid = new char[Size, Size];
        private int playerRow = 1;
        private int playerCol = 1;
        private Random random = new Random();

        public void GenerateMaze()
        {
            // Build maze
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (i == 0 || i == Size - 1 || j == 0 || j == Size - 1)
                        grid[i, j] = '#';
                    else
                        grid[i, j] = random.Next(100) < 25 ? '#' : '.';
                }
            }

            // creates a path
            for (int i = 1; i < Size - 1; i++)
                grid[i, 1] = '.';

            for (int j = 1; j < Size - 1; j++)
                grid[Size - 2, j] = '.';

            // Start
            grid[1, 1] = '.';

            // Exit
            grid[Size - 2, Size - 1] = 'X';
        }

        // Maze Display Method
        public void DisplayMaze()
        {
            Console.Clear();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (i == playerRow && j == playerCol)
                        Console.Write("@ ");
                    else
                        Console.Write(grid[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        // W,A, S, D movement method
        public bool MovePlayer(ConsoleKey key)
        {
            int newRow = playerRow;
            int newCol = playerCol;

            if (key == ConsoleKey.W) newRow--;
            if (key == ConsoleKey.S) newRow++;
            if (key == ConsoleKey.A) newCol--;
            if (key == ConsoleKey.D) newCol++;

            if (newRow < 0 || newRow >= Size || newCol < 0 || newCol >= Size)
                return false;

            if (grid[newRow, newCol] != '#')
            {
                playerRow = newRow;
                playerCol = newCol;

                if (grid[playerRow, playerCol] == 'X')
                    return true;
            }

            return false;
        }
    }

    //Main Game Class
    class Game
    {
        static void Main(string[] args)
        {
            Character player = new Character("Player", 100, 15);
          

            Maze maze = new Maze();
            maze.GenerateMaze();

            bool isRunning = true;

            Console.WriteLine("Game Starting...\n");
            HealthBar.Display(player);
            Console.WriteLine("Press any key to enter the maze...");
            Console.ReadKey();

            while (isRunning)
            {
                maze.DisplayMaze();
                Console.WriteLine("\nMove with W/A/S/D or press Q to quit");

                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Q)
                {
                    isRunning = false;
                }
                else
                {
                    bool reachedExit = maze.MovePlayer(key);

                    if (reachedExit)
                    {
                        maze.DisplayMaze();
                        Console.WriteLine("\nYou reached the exit! You win!");
                        break;
                    }
                }
            }

            // Game ends and prints game over message
            Console.WriteLine("\nGame Over.");
            Console.ReadKey();
        }
    }
}
