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
    public class Hand
    {
        private List<Dice> HandList;
        private int diceType;

        /// <summary>
        /// creates a standard starting hand with 5 d6's
        /// </summary>
        public Hand() : this(5, 6) { }

        /// <summary>
        /// Creates a new hand object
        /// </summary>
        /// <param name="handSize">number of starting dice in hand</param>
        /// <param name="diceType">the type of dice used</param>
        public Hand(int handSize, int diceType)
        {
            this.diceType = diceType;
            HandList = new List<Dice>();
            Random random = new Random();
            for (int i = 0; i < handSize; i++)
                HandList.Add(new Dice(random, diceType));
        }

        /// <summary>
        /// Removes a die from the hand
        /// Throws ArgumentException if hand doesn't have dice
        /// </summary>
        public void RemoveDie()
        {
            if (HandList.Count <= 0)
                throw new ArgumentException("Hand has no more dice");
            HandList.RemoveAt(0);
        }

        /// <summary>
        /// removes dice from the hand
        /// Throws ArgumentException if hand doesn't have enough dice
        /// </summary>
        /// <param name="number">the number of dice to be removed</param>
        public void RemoveDice(int number)
        {
            if (HandList.Count < number)
                throw new ArgumentException("Hand has not enough dice");
            for (int i = 0; i < number; i++)
                RemoveDie();
        }

        /// <summary>
        /// Rolls all dice in hand
        /// </summary>
        public void Roll()
        {
            foreach (Dice d in HandList)
                d.Roll();
        }

        /// <returns>the current hand of dice as a list of dice</returns>
        public List<Dice> CurrentListHand()
        {
            return HandList;
        }

        /// <summary>
        /// Creates an array with an element for each possible FV.
        /// Each element contains the amount of dice in hand with given FV.
        /// </summary>
        /// <returns>an integer array with Length == Dice FV()</returns>
        public int[] ArrayCountHand()
        {
            int[] count = new int[HandList.First().DiceType()];
            foreach (Dice d in HandList)
                count[d.CurrentFace() - 1]++;
            return count;
        }

        /// <summary>
        /// Creates and array with an element for each dice
        /// </summary>
        /// <returns></returns>
        public int[] CurrentHandArray()
        {
            int[] hand = new int[HandList.Count];
            for (int i = 0; i < HandList.Count; i++)
                hand[i] = HandList[i].CurrentFace();
            return hand;
        }

        /// <summary>
        /// Returns a string of the current face of all dice in the hand
        /// </summary>
        /// <returns>a string with numbers divided by ", "</returns>
        public String CurrentHandString()
        {
            return String.Join(", ", CurrentHandArray());
        }

        /// <summary>
        /// Returns the number of dice left in the hand 
        /// </summary>
        /// <returns>an integer >= 0 </returns>
        public int DiceLeft()
        {
            return HandList.Count;
        }

        /// <summary>
        /// returns the number of faces of a die in this hand
        /// </summary>
        /// <returns>an int representing the number of faces</returns>
        public int DiceType()
        {
            return diceType;
        }
    }
}
