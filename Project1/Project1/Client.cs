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

            Dictionary<string, int> diceLeft = DiceLeft(playerOrder);

            while (playerOrder.Count > 1) //while there is no champion
            {
                //get everyone's rolls and count the dice for round
                EveryoneRolls(playerOrder);
                int[] allDice = AllDice(playerOrder);

                //First player in round fencepost
                List<int[]> callList = new List<int[]>();
                List<string> playerList = new List<string>();
                UI.WhosTurn(playerOrder.First().Name());
                playerOrder.First().GameState(callList, playerList, diceLeft);
                int[] lastCall = new int[2];
                int[] call = GetCall(playerOrder.First(), lastCall);

                while (call[0] != -1) { //while no player has called
                    UI.HasBet(playerOrder.First().Name(), call[1], call[0]);
                    lastCall = MovePlayerToEnd(playerOrder, call);
                    UI.WhosTurn(playerOrder.First().Name());
                    UpdatePlayerGameState(callList, call, playerList, playerOrder, diceLeft);
                    call = GetCall(playerOrder.First(), lastCall);
                }

                UI.HasCalled(playerOrder.First().Name(), playerOrder.Last().Name());
                UI.AllHands(AllHands(playerOrder));

                //sequence to figure out who won
                int totalDiceIncludeWild = allDice[lastCall[1] - 1] + allDice[0];
                if (lastCall[0] <= totalDiceIncludeWild)
                {//the number of dice called was less than the actual amount
                    UI.CorrectBet(playerOrder.Last().Name(), totalDiceIncludeWild, allDice[0],
                        lastCall[1]);
                    CurrentPlayerLost(UI, playerOrder, losers, diceLeft);
                }
                else
                {//the number of dice called was greater than the actual amount
                    UI.CorrectCall(playerOrder.First().Name(), totalDiceIncludeWild, allDice[0],
                        lastCall[1]);
                    LastPlayerLost(UI, playerOrder, losers, diceLeft);
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
        /// Moves player at front of list to the end of the list. Denotes that player's call as
        /// the last call
        /// </summary>
        /// <param name="playerOrder">The list of players to be rotated</param>
        /// <param name="lastCall">an int array with two elements [NOD, FV]</param>
        /// <param name="call">an int array with two elements [NOD, FV]</param>
        public static int[] MovePlayerToEnd(LinkedList<Player> playerOrder, int[] call)
        {
            playerOrder.AddLast(playerOrder.First());
            playerOrder.RemoveFirst();
            return call;
        }

        /// <summary>
        /// updates player on the current gamestate so that the player can make an informed 
        /// move
        /// </summary>
        /// <param name="callList">A list of the pervious calls in the round, newest at 
        /// end</param>
        /// <param name="call">an array of the last call [NOD, FV]</param>
        /// <param name="playerList">A list of the pervious players in the round, newest at 
        /// end</param>
        /// <param name="playerOrder">The list of all the current (not lost) players</param>
        /// <param name="diceLeft">a dictionary of all the player's name and how many dice
        /// they have left</param>
        public static void UpdatePlayerGameState(List<int[]> callList, int[] call, List<String>
            playerList, LinkedList<Player> playerOrder, Dictionary<string, int> diceLeft)
        {
            callList.Add(call);
            playerList.Add(playerOrder.Last().Name());
            playerOrder.First().GameState(callList, playerList, diceLeft);
        }

        /// <summary>
        /// Gets the call from a player and tests legality
        /// </summary>
        /// <param name="player">the player</param>
        /// <param name="lastCall">the last call for legality purposes</param>
        /// <returns>an int array with two elements [NOD, FV]</returns>
        public static int[] GetCall(Player player, int[] lastCall)
        {
            int[] call = null;
            do //while call is illegal
            {
                call = player.Call();
            } while (!LegalCall(call, lastCall));
            return call;
        }

        /// <summary>
        /// determines whether the call is legal based on the last call
        /// </summary>
        /// <param name="call">requires an array with two elements, [NOD, FV]</param>
        /// <param name="lastCall">requires an array with two elements, [NOD, FV]</param>
        /// <returns>the legality</returns>
        public static bool LegalCall(int[] call, int[] lastCall)
        {
            if (call.Length > 2) //if wrong input
                throw new ArgumentOutOfRangeException("Call Array too big");
            if (call[0] == -1 && lastCall[0] == 0)
            {
                //TODO add a illegalCall method to Player Class and call it here
                return false;
            }
            if ((call[0] != -1 && call[0] < lastCall[0]) || call[0] == 0 || call[0] < -1)
            {
                //TODO call player.illegal NOD
                return false; 
            }
            if ((call[0] != -1 && call[0] == lastCall[0] && call[1] <= lastCall[1]) ||
                call[1] < 2 || call[1] > 6)
            {
                //TODO call player.illegal FV if lastCall[1] != 6
                //TODO call player.illegal NOD if lastCall[1] == 6
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
