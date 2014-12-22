/*
 * Rohan Aras
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Project1
{
    class ConsoleUI : UserInterface
    {
        private String savedText;

        public ConsoleUI()
        {
            savedText = "";
        }

        public LinkedList<Player> Intro()
        {
            LinkedList<Player> playerOrder = new LinkedList<Player>();
            Console.Write("How many dice do you wish to use? ");
            //int numberOfDice = Console.Read() - 48;
            //Console.ReadLine();
            int numberOfDice = Convert.ToInt32(Console.ReadLine());
            String type = "";
            int count = 0;
            //TODO: update this as I create more AIs
            Console.WriteLine("\nWhich of the following users do you wish to add to your game?");
            while (type != "done" || count <= 1)
            {
                Hand hand = new Hand(numberOfDice, 6); //TODO: allow for other dice types
                Console.WriteLine("[Human, TestPlayer]");
                type = Console.ReadLine().Trim().ToLower();
                if (type == "human")
                {
                    Console.Write("What is your name? ");
                    string userName = Console.ReadLine();
                    playerOrder.AddLast(new Human(hand, userName, this));
                    count++;
                }
                else if (type == "testplayer" || type == "tp")
                {
                    playerOrder.AddLast(new TestPlayer(hand));
                    count++;
                }
                else if (type == "onethirdrule" || type == "otr")
                {
                    playerOrder.AddLast(new OneThirdRule(hand));
                    count++;
                }
                else
                    Console.WriteLine("That player type does not exist. Please try again");
                Console.WriteLine();
                if (count <= 1)
                    Console.WriteLine("You now have " + count
                        + " players. You need at least 2 players. Please add another of type:");
                else
                    Console.WriteLine("You now have " + count
                        + " players. If you wish, you may add another of type: \nEnter \"done\" to continue.");
            }
            //END TODO
            Console.Clear();
            return playerOrder;
        }

        public void WhosTurn(string playerName)
        {
            Thread.Sleep(1000);
            string whosTurn = "It is now " + playerName + "'s turn\n";
            Console.Write(whosTurn);
            savedText += whosTurn;
        }

        public void HasBet(string playerName, int faceValue, int NOD)
        {
            string hasBet = playerName + " has bet " + NOD + ", " + faceValue + "'s.\n\n";
            Console.Write(hasBet);
            savedText += hasBet;
        }

        public void DiceTooLow()
        {
            Console.WriteLine("You didn't bet enough dice.");
        }

        public void FaceTooLow()
        {
            Console.WriteLine("You didn't bet a high enough value.");
        }

        public void HasCalled(string playerName, string previousPlayer)
        {
            string hasCalled = playerName + " has called " + previousPlayer +
                "\n\n+-----------------------------------------------------+\n\n";
            Console.Write(hasCalled);
            savedText += hasCalled;
        }

        public void AllHands(Dictionary<string, string> playersDice)
        {
            string allHands = "People's hands for the last round:\n";
            foreach (string s in playersDice.Keys)
            {
                string singleHand = "\t" + s + ": " + playersDice[s] + "\n";
                allHands += singleHand;
            }
            allHands += "\n";
            savedText += allHands;
            Console.Write(allHands);
        }

        public void CorrectBet(string playerName, int actualAmount, int wilds, int betFV)
        {
            string correctBet = "There were " + actualAmount + ", " + betFV +
                "'s including " + wilds + " wilds. " + playerName + " bet correctly.\n\n";
            Console.Write(correctBet);
            savedText += correctBet;
        }

        public void CorrectCall(string playerName, int actualAmount, int wilds, int betFV)
        {
            string correctCall = "There were " + actualAmount + ", " + betFV +
                "'s including " + wilds + " wilds. " + playerName + " called correctly.\n\n";
            Console.Write(correctCall);
            savedText += correctCall;
        }

        public void HasLostDie(string playerName, int numberLeft)
        {
            string hasLostDie = playerName + " has lost a die and now has " + numberLeft
                + " left.\n\n";
            if (numberLeft != 0)
                hasLostDie += "+-----------------------------------------------------+\n\n";
            savedText += hasLostDie;
            Console.Write(hasLostDie);
        }

        public void HasLostGame(string playerName)
        {
            string hasLostGame = playerName + " has lost the game."
                + "\n\n+-----------------------------------------------------+\n\n";
            savedText += hasLostGame;
            Console.WriteLine(hasLostGame);
        }

        public void GameOver(Stack<Player> players)
        {
            string playerNames = "";
            foreach (Player p in players)
            {
                playerNames += p.Name() + "\n";
            }
            string gameOver = "The game is now over. The following players lost: \n" + playerNames;
            savedText += gameOver;
            Console.WriteLine(gameOver);
        }

        public bool NewRound()
        {
            throw new NotImplementedException();
        }

        public void Champion(string playerName)
        {
            string winner = playerName + " is the WINNER!!";
            savedText += winner;
            Console.WriteLine(winner);
            System.IO.File.WriteAllText(@"C:\Users\Public\Documents\autosave.dg",
                savedText);
            Console.WriteLine(
                "autosave saved to C:\\Users\\Public\\Documents\\autosave.dg");
            Console.ReadKey(); //todo get rid of later?
        }

        public int FaceValueBet()
        {
            Console.WriteLine("What is the face value that you wish to bet?");
            int FV = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Console.Write(savedText);
            return FV;
        }

        public int NumberOfDiceBet()
        {
            Console.WriteLine("How many dice do you wish to bet? Type \"call\" to call.");
            string bet = Console.ReadLine().Trim().ToLower();
            if (bet == "call")
            {
                Console.Clear();
                Console.Write(savedText);
                return -1;
            }
            return Convert.ToInt32(bet);
        }

        public void Rules()
        {
            throw new NotImplementedException();
        }

        public void IllegalFVBet(int faceValue)
        {
            throw new NotImplementedException();
        }

        public void IllegalNODBet(int NOD)
        {
            if (NOD == -1)
                Console.WriteLine("There is no previous player to call.");
            else
                Console.WriteLine("You have bet fewer dice than the previous player.");
            Console.WriteLine("Please try again.");
        }

        public void HumanHand(String hand)
        {
            Console.WriteLine("Your hand currently contains the following: " + hand);
        }
    }
}
