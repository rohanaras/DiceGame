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
        /// creates a dice object with a given random object and a set number of faces
        /// </summary>
        /// <param name="number">Random Object for chosing a dice face</param>
        /// <param name="numberOfFaces">must be > 1 else ArgumentOutOfRangeException</param>
        public Dice(Random number, int numberOfFaces)
        {
            if (numberOfFaces <= 1)
                throw new ArgumentOutOfRangeException("must have more than one side");
            this.number = number;
            this.numberOfFaces = numberOfFaces;
            currentFace = number.Next(1, numberOfFaces + 1);
        }

        /// <summary>
        /// rolls the dice
        /// </summary>
        /// <returns>a number between 1 and the max FV</returns>
        public int Roll()
        {
            currentFace = number.Next(1, numberOfFaces + 1);
            return currentFace;
        }


        /// <summary>
        /// returns the current face value
        /// </summary>
        /// <returns>a number between 1 and the max FV</returns>
        public int CurrentFace()
        {
            return currentFace;
        }

        
        /// <summary>
        /// Returns the number of faces that this die object has
        /// </summary>
        /// <returns>a number greater than 1</returns>
        public int DiceType()
        {
            return numberOfFaces;
        }
    }
}
