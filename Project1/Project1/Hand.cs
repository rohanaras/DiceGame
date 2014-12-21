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
            HandList = new List<Dice>();
            Random random = new Random();
            for (int i = 0; i < handSize; i++)
                HandList.Add(new Dice(random, diceType));
        }

        /// <summary>
        /// Removes a die from the hand
        /// </summary>
        public void RemoveDie()
        {
            if (HandList.Count <= 0)
                throw new ArgumentException("Hand has no more dice");
            HandList.RemoveAt(0);
        }

        /// <summary>
        /// removes dice from the hand
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

        public int[] ArrayCountHand()
        {
            int[] count = new int[HandList.First().DiceType()];
            foreach (Dice d in HandList)
                count[d.CurrentFace() - 1]++;
            return count;
        }

        public int[] CurrentHandArray()
        {
            int[] hand = new int[HandList.Count];
            for (int i = 0; i < HandList.Count; i++)
                hand[i] = HandList[i].CurrentFace();
            return hand;
        }

        public String CurrentHandString()
        {
            return String.Join(", ", CurrentHandArray());
        }

        public int DiceLeft()
        {
            return HandList.Count;
        }
    }
}
