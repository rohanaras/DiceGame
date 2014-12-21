/*
 * Rohan Aras
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public interface UserInterface
    {
        /// <summary>
        /// Queries user for information on number and types of players 
        /// </summary>
        /// <returns>a queue with players and their hands</returns>
        LinkedList<Player> Intro();

        /// <summary>
        /// notifies players of who's turn it is
        /// </summary>
        /// <param name="playerName">the player currently up</param>
        void WhosTurn(String playerName);

        /// <summary>
        /// Notifies players that a player has bet something
        /// </summary>
        /// <param name="playerName">the player's name</param>
        /// <param name="bet">the bet</param>
        void HasBet(String playerName, int faceValue, int NOD);

        /// <summary>
        /// Notifies the players that the number of dice bet is
        /// too low
        /// For human users.
        /// </summary>
        void DiceTooLow();

        /// <summary>
        /// notifies the players that the face number of the bet
        /// is too low
        /// For human users.
        /// </summary>
        void FaceTooLow();

        /// <summary>
        /// notifies players that a player has called
        /// </summary>
        /// <param name="playerName">the player's name</param>
        void HasCalled(String playerName, String previousPlayer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playersDice">Every player and their Dice
        /// from the last round [player name, dice] </param>
        void AllHands(Dictionary<String, String> playersDice);

        /// <summary>
        /// notifies players that the bet was correct
        /// </summary>
        /// <param name="playerName">The better's name</param>
        void CorrectBet(String playerName, int actualAmount, int wilds, int betFV);

        /// <summary>
        /// notifies the players that the call was correct
        /// </summary>
        /// <param name="playerName">the caller's name</param>
        void CorrectCall(String playerName, int actualAmount, int wilds, int betFV);

        /// <summary>
        /// notifies everyone that the player has lost a die
        /// </summary>
        /// <param name="playerName">the player's name</param>
        void HasLostDie(String playerName, int numberLeft);

        /// <summary>
        /// notifies the players that someone has lost
        /// </summary>
        /// <param name="playerName">the player's name</param>
        void HasLostGame(String playerName);

        /// <summary>
        /// notifies players that game is over
        /// </summary>
        /// <param name="playerNames">the players that lost</param>
        void GameOver(Stack<Player> players);

        /// <summary>
        /// asks if wants to play again
        /// </summary>
        bool NewRound();

        /// <summary>
        /// notifies players of champion
        /// </summary>
        /// <param name="playerName">the champion</param>
        void Champion(String playerName);

        /// <summary>
        /// Querries player about what value they want to bet.
        /// For Human players.
        /// </summary>
        /// <returns>the face value bet</returns>
        int FaceValueBet();

        /// <summary>
        /// Querries player about how many dice they want to bet.
        /// For Human players.
        /// </summary>
        /// <returns>the number of dice bet</returns>
        int NumberOfDiceBet();

        /// <summary>
        /// List of the dice game Rules
        /// </summary>
        void Rules();

        /// <summary>
        /// the face value does not exist
        /// For Human players.
        /// </summary>
        /// <param name="faceValue">the facevalue</param>
        void IllegalFVBet(int faceValue);

        /// <summary>
        /// too few dice were bet
        /// For Human players.
        /// </summary>
        /// <param name="faceValue">the facevalue</param>
        void IllegalNODBet(int NOD);

        /// <summary>
        /// Displays the hand of a human player
        /// </summary>
        /// <param name="hand">the hand</param>
        void HumanHand(String hand);
    }
}
