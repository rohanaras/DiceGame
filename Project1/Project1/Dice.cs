using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Dice
    {
        private Random number;
        private int numberOfFaces;
        private int currentFace;

        /// <summary>
        /// creates a new d6 dice object
        /// </summary>
        public Dice() : this(new Random(), 6) { }

        /// <summary>
        /// creates a dice object 
        /// </summary>
        /// <param name="numberOfFaces">the number of faces the die should have</param>
        public Dice(int numberOfFaces) : this(new Random(), numberOfFaces) { }

        /// <summary>
        /// creates a dice object
        /// </summary>
        /// <param name="number">Random Object for chosing a dice face</param>
        /// <param name="numberOfFaces">the number of faces the die should have</param>
        public Dice(Random number, int numberOfFaces)
        {
            this.number = number;
            this.numberOfFaces = numberOfFaces;
            currentFace = number.Next(1, numberOfFaces + 1);
        }

        /// <summary>
        /// rolls the dice
        /// </summary>
        /// <returns>the aquired face</returns>
        public int Roll()
        {
            currentFace = number.Next(1, numberOfFaces + 1);
            return currentFace;
        }


        /// <returns>the current face</returns>
        public int CurrentFace()
        {
            return currentFace;
        }

        public int DiceType()
        {
            return numberOfFaces;
        }
    }
}
