using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Vintasoft.Imaging.Codecs.ImageFiles;

namespace TAXI
{
    internal class Program
    {
        static void Main(string[] args)
        {


            ///Check if game launcher has fed in a player name and difficulty level
            if (args.Length > 0)
            {
                string userName = args[0];
                string difficultyLevelString = args[1];

               //Console.WriteLine() used here as a debugging tool for MAUI launcher, commented out when not in use
               // Console.WriteLine(args[0]);
               // Console.WriteLine(args[1]);

                Game game = new Game(userName);
                int difficultyLevel = game.DifficultyLevel(difficultyLevelString);
                game.Run(difficultyLevel);

            }
            ///If not, default difficulty setting chosen and player name remains unchanged from default value
            else
            {
                string userName = "Lauren";
                Game game = new Game(userName);
                int difficultyLevel = 30;
                game.Run(difficultyLevel);
            }
        }
    }

    /// <summary>
    /// Game class created to hold game setup and running methods
    /// </summary>
    public class Game
    {
        public Player player;
        public Driver driver;
        public Car car;
        ///Constructor method for Game instantiates player, driver and car when called
        public Game(string userName)
        {
            player = new Player(userName);
            driver = new Driver("Dai");
            car = new Car("Taxi", 100);
        }
        /// <summary>
        /// Run() contains the introduction/exposition of the game and when called, automatically runs the combat method and contains the game loop.
        /// </summary>
        /// <param name="difficultyLevel"></param>
        public void Run(int difficultyLevel)
        {
            while (true)
            {
                Console.WriteLine($"After a long night in Heron Bay, you climb into the first taxi you find and meet your driver, {driver.Name}. \n{player.Name}: 'Alright Drive? Bryn Road please.'" +
                    $"\n{driver.Name}: 'Aye okay. You sure you're alright mate? Looking a bit green around the gills, is all.'\n{player.Name}: 'I'm not going to spew, if that's what you're asking.'" +
                    $"\n{driver.Name}: 'Not the end of the world if you do. Fifty quid, mind. Doesn't bother me though. You have to, you have to, innit?'" +
                    $"\n{player.Name}: 'It's fine, I won't.'\n{driver.Name}: 'You know, I've been starting to wonder if it even matters. If anything matters.'" +
                    $"\n{driver.Name}: 'You know what I mean? Puke in the car, don't puke in the car, life continues on uncaring. Tomorrow the post will still be delivered and the shops will be open. It's the same with anything.'" +
                    $"\n{player.Name}: 'I suppose so mate...' \n{driver.Name}: 'You could say that about anything though. Suppose you or I drove off a cliff. A few people might shed a tear " +
                    $"but when all is said and done, life goes on. It is what it is.' \n{player.Name}: 'What?' \n{driver.Name}: 'Makes you wonder...' " +
                    $"\n{player.Name}: 'Sorry, what? I think you missed a turning by the way Drive, do you want directions?' \n{driver.Name}: 'Doesn't matter. It is what it is.'" +
                    $"\n More wrong turns. You see a sign for the beach. The driver's gone wrong. What will you do? You can try talking some sense into him or escape the car. " +
                    $"\n The more he talks, the less sure of yourself you feel. Is he right? No... that can't be..." +
                    $"\n Try to raise the driver's sanity level in time to stop him driving you off a cliff, or damage the taxi enough to escape!");
                Combat(difficultyLevel);
                if (!PlayAgain())
                {
                    break;
                }
            }

        }
        ///Combat() manages the turn-based combat of the game, while continually checking if any of the win/lose conditions have been met
        public void Combat(int difficultyLevel)
        {

            while (!GameOver())
            {
                player.Attack(player, driver, car, difficultyLevel);
                driver.Attack(driver, player, difficultyLevel);
            }
        }
        ///GameOver method checks conditions for winning or losing the game and returns a boolean to say whether the game is over or not
        public bool GameOver()
        {
            if (player.IHaveSanity() <= 0 || driver.IHaveSanity() >= 100 || car.IHaveHealth() <= 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// PlayAgain() method allows user to restart the game loop to play again or exit the game completely. It returns as a boolean so that it can be used as a run condition for other methods.
        /// </summary>
  
        private bool PlayAgain()
        {
            string imageViewerPath= @"C:\Users\Lauren\OneDrive\Documents\GitHub\Foundry-Phase1-Lauren\TAXI\imageviewer\fsviewer.exe";
            string winImagePath = @"C:\Users\Lauren\OneDrive\Documents\GitHub\Foundry-Phase1-Lauren\TAXI\winscreen.jpg";
            string loseImagePath = @"C:\Users\Lauren\OneDrive\Documents\GitHub\Foundry-Phase1-Lauren\TAXI\losescreen.jpg";

            ProcessStartInfo winInfo = new ProcessStartInfo();
            winInfo.FileName = imageViewerPath;
            winInfo.Arguments = winImagePath;


            ProcessStartInfo loseInfo = new ProcessStartInfo();
            loseInfo.FileName = imageViewerPath;
            loseInfo.Arguments = loseImagePath;

            if (player.IHaveSanity() <= 0)
            {
                Console.WriteLine($"The more the driver talks, the more you realise he has a point. Nothing in this life makes sense, and life is a relentless struggle. You allow the cruel unforgiving nature of the world to wash over you. {player.Name}: 'Drive, I'm ready. You're right. Take me with you.' \nThe car reaches the cliff. You feel an overwhelming sense of calm as the water rises up around you.\n GAME OVER.  \n1. Play again\n2. Quit");

                try
                {
                    Process.Start(loseInfo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while opening the image viewer");
                }

                //Try/Catch used in each if statement to ensure invalid user input does not cause game to crash
                try
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    if (choice == 1) 
                    {
                        player.ResetSanity();
                        driver.ResetSanity();
                        car.ResetHealth();
                        return true;

                    }
                    else if (choice == 2)
                    {
                        Console.WriteLine($"Thanks for playing, {player.Name}.");
                        return false;
                    }
                }
                catch (Exception e)
                { Console.WriteLine("Invalid entry"); }
            }
            else if (driver.IHaveSanity() >= 100)
            {
                Console.WriteLine($"The driver seems to shudder a little. His brow furrows, and his expression changes. {driver.Name}: 'What am I doing?'\n{driver.Name}: '{player.Name}, it was {player.Name} wasn't it? I'm sorry. You've reminded me today that as bad as things are, it's not all lost. Maybe tomorrow will be better. Let's get us both home.' \nYOU SURVIVED! \n1. Play again\n2. Quit");

                try
                {
                    Process.Start(winInfo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while opening the image viewer");
                }

                try
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    if (choice == 1)
                    {
                        player.ResetSanity();
                        driver.ResetSanity();
                        car.ResetHealth();
                        return true;

                    }
                    else if (choice == 2)
                    {
                        Console.WriteLine($"Thanks for playing, {player.Name}.");
                        return false;
                    }
                }
                catch (Exception f)
                { Console.WriteLine("Invalid entry"); }
            }
            else if (car.IHaveHealth() <= 0)
            {
                Console.WriteLine($"The {car.Name} window finally gives way! You heave yourself up and out of the window, and escape to safety. YOU SURVIVED.\n1. Play again\n2. Quit");

                try
                {
                    Process.Start(winInfo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while opening the image viewer");
                }

                try
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    if (choice == 1)
                    {
                        player.ResetSanity();
                        driver.ResetSanity();
                        car.ResetHealth();
                        return true;

                    }
                    else if (choice == 2)
                    {
                        Console.WriteLine($"Thanks for playing, {player.Name}.");
                        return false;
                    }
                }
                catch (Exception g)
                { Console.WriteLine("Invalid entry"); }
            }
            return false;
        }
        ///DifficultyLevel() method set up to take player's difficulty selection from the launcher, which is entered through a drop down box, and convert it into an integer that can be operated on to change combat difficulty accordingly.
        public int DifficultyLevel(string difficultyLevel)
        {
            switch (difficultyLevel)
            {
                case "I don't touch the stuff, mate (easy)":
                    return 10;
                case "Only a shandy":
                    return 20;
                case "I might have had a couple (normal)":
                    return 30;
                case "Who's counting?":
                    return 40;
                case "I don't feel so good (hard)":
                    return 50;
                default:
                    return 30;
            }
        }
    }
    /// <summary>
    /// An abstract class has been used as the parent class because no objects of type Entity will ever need to be instantiated, but having the child classes Driver, Player and Car inherit attributes from a parent class will save on repetition of code.
    /// </summary>

    public abstract class Entity
    {
        public string Name { get; set; }
        protected Random rand = new Random();

        public Entity(string Name)
        {
            this.Name = Name;
        }


    }
    ///Interface IHaveSanity is used for Player and Driver to inherit from, as these use Sanity as their 'health' metric. They could not inherit this directly from the parent class Entity because Car uses a standard Health metric to record damage, so interfaces are used as an additional source of inheritance because child classes cannot inherit from more than one parent class. 
    interface IHaveSanity
    {

    }
    ///class Player inherits from parent class Entity and interface IHaveSanity
    public class Player : Entity, IHaveSanity
    {
        public int Sanity = 100;
        internal int MaxSanity = 100;
        internal int MinSanity = 0;
        ///Constructor for Player class object instantiation
        public Player(string Name) : base(Name)
        {
        }
        ///Getter method to return player Sanity
        public int IHaveSanity()
        {
            return Sanity;
        }
        ///Setter method to reset sanity to its level at the start of the game - used when player chooses to play again
        public void ResetSanity()
        {
            Sanity = MaxSanity;
        }
        /// <summary>
        /// Attack method which allows player to choose whether to attack the driver (normal or strong attack) or the car. Success is based on a random number generator, with strong attack less likely to succeed, but causing more damage when it does. Attacking the car gives the player a chance at an additional turn, also decided by a random number generation.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="driver"></param>
        /// <param name="car"></param>
        /// <param name="difficultyLevel"></param>
        public void Attack(Player player, Driver driver, Car car, int difficultyLevel)
        {
            int normalAttack = 2*(difficultyLevel/ 5);
            int strongAttack = 6*(difficultyLevel / 5);
            //Change console colours for when a player choice is needed, to differentiate and break up the text on screen
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("1. Try to reason with driver (normal attack)\n2. Scream for help (strong attack)\n3. Kick the car window (chance to distract)");
            try
            {
                int input = Convert.ToInt32(Console.ReadLine());
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                ///difficulty level added as a modifier for random number generator to make successful actions less likely at higher difficulty settings
                if (input == 1 && (rand.Next(1, 101)) - (difficultyLevel / 10) > 5)
                {
                    driver.Sanity += normalAttack;
                    Console.WriteLine($"You reason with {driver.Name}. He seems to pause for thought. \nDriver sanity: {driver.Sanity}/{driver.MaxSanity}");

                }
                else if (input == 2 && (rand.Next(1, 101)) - (difficultyLevel / 10) > 40)
                {
                    driver.Sanity += strongAttack;
                    Console.WriteLine($"Your cries for help seem to get through to {driver.Name}. He looks deep in thought. \nDriver sanity: {driver.Sanity}/{driver.MaxSanity}");
                }
                else if (input == 3)
                {
                    car.Health -= normalAttack;
                    Console.WriteLine($"You lean back and kick the {car.Name} window, as hard as you can! \nCar: {car.Health}/{car.MaxHealth}");


                    if (car.Health >= 1 && (rand.Next(1, 101)) - (difficultyLevel / 10) > 70)
                    {

                        Console.WriteLine($"Kicking the window has distracted {driver.Name} from his rant! Now's your chance!");
                        Attack(player, driver, car, difficultyLevel);
                    }
                }

                else
                {
                    Console.WriteLine($"Nothing you do seems to work... \n Driver sanity: {driver.Sanity}/{driver.MaxSanity}\nCar: {car.Health}/{car.MaxHealth}");
                }
            }
            catch (Exception a)
            { Console.WriteLine("In your confusion you didn't know what to do, and you missed your chance!"); }
        }

    }
    ///class Driver inherits from parent class Entity and interface IHaveSanity
    public class Driver : Entity, IHaveSanity
    {
        public int Sanity = 0;
        internal int MaxSanity = 100;
        internal int MinSanity = 0;
        /// <summary>
        /// Getter method to return driver Sanity
        /// </summary>
        /// <returns>int Sanity</returns>

        public int IHaveSanity()
        {
            return Sanity;
        }
        ///Constructor for Driver class object instantiation
        public Driver(string Name) : base(Name)
        {

        }
        ///Attack method which uses a random number generator to determine whether the driver's attack on the player is a strong attack, normal attack, or fails to hit.
        public void Attack(Driver driver, Player player, int difficultyLevel)
        {
            int normalAttack = 2*(difficultyLevel/5);
            int strongAttack = 7*(difficultyLevel/5);
            if (player.IHaveSanity() > 0 && (rand.Next(1, 101))+(difficultyLevel/10) < 85)
            {
                player.Sanity -= normalAttack;
                Console.WriteLine($"{driver.Name} is saying some pretty outlandish things, but you can understand where he's coming from. \nCurrent sanity: {player.Sanity}/{player.MaxSanity}");
            }
            else if (player.IHaveSanity() > 0 && (rand.Next(1, 101)) + (difficultyLevel / 10) < 25)
            {
                player.Sanity -= strongAttack;
                Console.WriteLine($"A lot of what {driver.Name} was saying really hit home. Maybe he's right... \nCurrent sanity: {player.Sanity}/{player.MaxSanity}");
            }
            else if (player.IHaveSanity() > 0)
            {
                Console.WriteLine($"{driver.Name} was ranting on about something. Thankfully you couldn't hear him over the noise from the road. \nCurrent sanity: {player.Sanity}/{player.MaxSanity}");
            }
        }
        /// <summary>
        /// Setter method to reset sanity to its level at the start of the game - used when player chooses to play again
        /// </summary>
        public void ResetSanity()
        {
            Sanity = MinSanity;
        }
    }
    /// <summary>
    /// interface IHaveHealth set up because Car uses a different health metric to the other child classes inheriting from parent class Entity
    /// </summary>
    interface IHaveHealth
    { }
    ///Car class inherits from parent class Entity, interface IHaveHealth
    public class Car : Entity, IHaveHealth
    {
        public int Health { get; set; }
        internal int MaxHealth = 100;
        internal int MinHealth = 0;
        ///Constructor for instantiation of objects in Car class
        public Car(string Name, int Health) : base(Name)
        {
            this.Health = Health;
        }
        ///Getter method to return Car Health
        public int IHaveHealth()
        {
            return Health;
        }
        /// <summary>
        /// Setter method to return Car Health to original level if the player restarts the game
        /// </summary>
        public void ResetHealth()
        {
            Health = MaxHealth;
        }



    }
}
