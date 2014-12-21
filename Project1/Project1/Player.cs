using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public abstract class Player
    {
        private Hand Hand;
        private string PlayerName;
        protected Hand VisibleHand;

        /// <summary>
        /// creates a player object with an auto generated name
        /// </summary>
        /// <param name="hand">the player's hand</param>
        public Player(Hand hand) : this(hand, "") 
        {
            List<String> names = new List<String>();
            StreamReader sr = new StreamReader("namelist.txt");
            String name = sr.ReadLine();
            while (name != null)
            {
                names.Add(name);
                name = sr.ReadLine();
            }
            Random r = new Random();
            PlayerName = names[r.Next(0, names.Count)];
        }

        /// <summary>
        /// creates a player object
        /// </summary>
        /// <param name="hand">the given hand for the player</param>
        /// <param name="playerName">gives the player object the given name</param>
        public Player(Hand hand, string playerName)
        {
            this.Hand = hand;
            this.VisibleHand = hand;
            this.PlayerName = playerName;
        }

        /// <returns>returns the hand</returns>
        public Hand ShowHand()
        {
            return Hand;
        }

        /// <returns>returns the player's name</returns>
        public string Name()
        {
            return PlayerName;
        }

        /// <summary>
        /// Rolls the dice in the hand
        /// </summary>
        public void Roll()
        {
            Hand.Roll();
            VisibleHand = Hand;
        }

        /// <summary>
        /// provides player with game state
        /// </summary>
        public abstract void GameState(List<int[]> calls, List<string> names,
            Dictionary<string, int> diceLeft);

        /// <summary>
        /// removes a die from the player's hand
        /// </summary>
        public void LoseDie()
        {
            Hand.RemoveDie();
            VisibleHand = Hand;
        }

        /// <summary>
        /// checks to see if player has no dice left
        /// </summary>
        /// <returns>returns true if so</returns>
        public bool HasLost()
        {
            return Hand.DiceLeft() == 0;
        }

        /// <summary>
        /// Prompts the player to make a call
        /// </summary>
        /// <returns>returns the player's call in the form [NOD,FV]</returns>
        public abstract int[] Call();

        public virtual void RedoFaceValueBet(int FV)
        {
            throw new WarningException("Face value error");
        }

        public virtual void RedoNODBet(int NOD)
        {
            throw new WarningException("Number of Dice error");
        }
    }
}
