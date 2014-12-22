/*
 * Rohan Aras
 * 
 * This class contains all the logic to play liars dice given a list of players,
 * their hands/dice, and a user interface.  
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Client
    {
        public static void Main(string[] args)
        {
            UserInterface UI = new ConsoleUI(); //the user interface to be used
            LinkedList<Player> playerOrder = UI.Intro(); //player order.count must be > 2
            Stack<Player> losers = new Stack<Player>();

            int totalDiceLeft = playerOrder.Count * playerOrder.First().ShowHand().DiceLeft();
            Dictionary<string, int> playerDiceLeft = DiceLeft(playerOrder);

            while (playerOrder.Count > 1) //while there is no champion
            {
                //get everyone's rolls and count the dice for round
                EveryoneRolls(playerOrder);
                int[] allDice = AllDice(playerOrder);

                //First player in round fencepost
                List<int[]> betList = new List<int[]>();
                List<string> playerList = new List<string>();
                UI.WhosTurn(playerOrder.First().Name());
                playerOrder.First().GameState(betList, playerList, playerDiceLeft, totalDiceLeft);
                int[] lastBet = new int[2];
                int[] bet = GetBet(playerOrder.First(), lastBet);

                while (bet[0] != -1) { //while no player has called
                    UI.HasBet(playerOrder.First().Name(), bet[1], bet[0]);
                    lastBet = MovePlayerToEnd(playerOrder, bet);
                    UI.WhosTurn(playerOrder.First().Name());
                    UpdatePlayerGameState(betList, bet, playerList, playerOrder, playerDiceLeft,
                        totalDiceLeft);
                    bet = GetBet(playerOrder.First(), lastBet);
                }

                UI.HasCalled(playerOrder.First().Name(), playerOrder.Last().Name());
                UI.AllHands(AllHands(playerOrder));

                totalDiceLeft--;

                //sequence to figure out who won
                int totalDiceIncludeWild = allDice[lastBet[1] - 1] + allDice[0];
                if (lastBet[0] <= totalDiceIncludeWild)
                {//the number of dice bet was less than the actual amount
                    UI.CorrectBet(playerOrder.Last().Name(), totalDiceIncludeWild, allDice[0],
                        lastBet[1]);
                    CurrentPlayerLost(UI, playerOrder, losers, playerDiceLeft);
                }
                else
                {//the number of dice bet was greater than the actual amount
                    UI.CorrectCall(playerOrder.First().Name(), totalDiceIncludeWild, allDice[0],
                        lastBet[1]);
                    LastPlayerLost(UI, playerOrder, losers, playerDiceLeft);
                }
            }
            UI.GameOver(losers);
            UI.Champion(playerOrder.First().Name());
        }

        /// <summary>
        /// creates a dictionary linking players and the number of dice they have left
        /// </summary>
        /// <param name="playerOrder">the players</param>
        /// <returns>the dictionary</returns>
        public static Dictionary<string, int> DiceLeft(LinkedList<Player> playerOrder)
        {
            Dictionary<string, int> diceLeft = new Dictionary<string, int>();
            foreach (Player p in playerOrder)
                diceLeft.Add(p.Name(), p.ShowHand().DiceLeft()); 
            return diceLeft;
        }

        /// <summary>
        /// Rolls all the dice for all the players
        /// </summary>
        /// <param name="playerOrder">all the players</param>
        public static void EveryoneRolls(LinkedList<Player> playerOrder)
        {
            foreach (Player p in playerOrder)
            {
                p.Roll();
            }
        }
        /// <summary>
        /// Sums up the number of each facevalue
        /// </summary>
        /// <param name="allPlayers"></param>
        /// <returns>an array detailing the number of each facevalue at index 
        /// [face value - 1]</returns>
        public static int[] AllDice(LinkedList<Player> allPlayers)
        {
            int[] allDice = new int[6]; //todo should fix this so that it works with all dice types
            foreach (Player p in allPlayers)
            {
                int[] nextHand = p.ShowHand().ArrayCountHand();
                for (int i = 0; i < allDice.Length; i++)
                    allDice[i] += nextHand[i];
            }
            return allDice;
        }

        /// <summary>
        /// Moves player at front of list to the end of the list. Denotes that player's bet as
        /// the last bet
        /// </summary>
        /// <param name="playerOrder">The list of players to be rotated</param>
        /// <param name="bet">an int array with two elements [NOD, FV]</param>
        /// <returns>what was the current bet [NOD, FV]</returns>
        public static int[] MovePlayerToEnd(LinkedList<Player> playerOrder, int[] bet)
        {
            playerOrder.AddLast(playerOrder.First());
            playerOrder.RemoveFirst();
            return bet;
        }

        /// <summary>
        /// updates player on the current gamestate so that the player can make an informed 
        /// move
        /// </summary>
        /// <param name="betList">A list of the pervious bets in the round, newest at 
        /// end</param>
        /// <param name="bet">an array of the last bets [NOD, FV]</param>
        /// <param name="playerList">A list of the pervious players in the round, newest at 
        /// end</param>
        /// <param name="playerOrder">The list of all the current (not lost) players</param>
        /// <param name="diceLeft">a dictionary of all the player's name and how many dice
        /// they have left</param>
        public static void UpdatePlayerGameState(List<int[]> betList, int[] bet, List<String>
            playerList, LinkedList<Player> playerOrder, Dictionary<string, int> diceLeft, int totalDiceLeft)
        {
            betList.Add(bet);
            playerList.Add(playerOrder.Last().Name());
            playerOrder.First().GameState(betList, playerList, diceLeft, totalDiceLeft);
        }

        /// <summary>
        /// Gets the call from a player and tests legality
        /// </summary>
        /// <param name="player">the player</param>
        /// <param name="lastBet">the last call for legality purposes</param>
        /// <returns>an int array with two elements [NOD, FV]</returns>
        public static int[] GetBet(Player player, int[] lastBet)
        {
            int[] bet = null;
            do //while bet is illegal
            {
                bet = player.Bet();
            } while (!LegalBet(bet, lastBet));
            return bet;
        }

        /// <summary>
        /// determines whether the call is legal based on the last call
        /// </summary>
        /// <param name="bet">requires an array with two elements, [NOD, FV]</param>
        /// <param name="lastBet">requires an array with two elements, [NOD, FV]</param>
        /// <returns>the legality</returns>
        public static bool LegalBet(int[] bet, int[] lastBet)
        {
            if (bet.Length > 2) //if wrong input
                throw new ArgumentOutOfRangeException("Bet Array too big");
            if (bet[0] == -1 && lastBet[0] == 0)
            {
                //TODO add a illegalCall method to Player Class and call it here
                return false;
            }
            if ((bet[0] != -1 && bet[0] < lastBet[0]) || bet[0] == 0 || bet[0] < -1)
            {
                //TODO call player.illegal NOD
                return false; 
            }
            if ((bet[0] != -1 && bet[0] == lastBet[0] && bet[1] <= lastBet[1]) ||
                bet[1] < 2 || bet[1] > 6)
            {
                //TODO call player.illegal FV if lastBet[1] != 6
                //TODO call player.illegal NOD if lastBet[1] == 6
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns a Dictionary of all the hands for the round
        /// </summary>
        /// <param name="allPlayers">All the players</param>
        /// <returns>The Dictionary</returns>
        public static Dictionary<String, String> AllHands(LinkedList<Player> allPlayers)
        {
            Dictionary<String, String> allHands = new Dictionary<String, String>();
            foreach (Player p in allPlayers)
                allHands.Add(p.Name(), p.ShowHand().CurrentHandString());
            return allHands;
        }

        /// <summary>
        /// reorganizes playerOrder if the current player lost the call. moves people to loser
        /// list as needed. also updates the number of dice each player has.
        /// Informs UI if players have lost die or lost
        /// </summary>
        /// <param name="playerOrder">the player list</param>
        /// <param name="losers">the loser list</param>
        /// <param name="diceLeft">the number of dice each player has</param>
        public static void CurrentPlayerLost(UserInterface UI, LinkedList<Player> playerOrder,
            Stack<Player> losers, Dictionary<string, int> diceLeft)
        {
            playerOrder.First().LoseDie();
            diceLeft[playerOrder.First().Name()]--;
            UI.HasLostDie(playerOrder.First().Name(), diceLeft[playerOrder.First().Name()]);
            if (playerOrder.First().HasLost())
            {
                losers.Push(playerOrder.First());
                playerOrder.RemoveFirst();
                UI.HasLostGame(losers.Peek().Name());
            }
            playerOrder.AddFirst(playerOrder.Last());
            playerOrder.RemoveLast();
        }

        /// <summary>
        /// reorganizes playerOrder if the last player lost the call. moves people to loser
        /// list as needed. also updates the number of dice each player has.
        /// Informs UI if players have lost die or lost
        /// </summary>
        /// <param name="playerOrder">the player list</param>
        /// <param name="losers">the loser list</param>
        /// <param name="diceLeft">the number of dice each player has</param>
        public static void LastPlayerLost(UserInterface UI, LinkedList<Player> playerOrder, 
            Stack<Player> losers, Dictionary<string, int> diceLeft)
        {
            playerOrder.Last().LoseDie();
            diceLeft[playerOrder.Last().Name()]--;
            UI.HasLostDie(playerOrder.Last().Name(), diceLeft[playerOrder.Last().Name()]);
            if (playerOrder.Last().HasLost())
            {
                losers.Push(playerOrder.Last());
                playerOrder.RemoveLast();
                UI.HasLostGame(losers.Peek().Name());
            }
        }
    }
}
