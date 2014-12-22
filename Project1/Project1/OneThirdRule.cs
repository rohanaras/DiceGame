using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class OneThirdRule : Player
    {
        public OneThirdRule(Hand hand) : base(hand) { }

        public override int[] Bet()
        {
            if (playersThisRound.Count() == 0)
            {
                int[] firstBet = { totalDiceLeft / 3, MostCommonInHand() };
                return firstBet;
            }
            if (betsThisRound[betsThisRound.Count - 1][0] > totalDiceLeft / 3)
            {
                int[] call = { -1, 2 }; //hack might not be needed
                return call;
            }
            if (betsThisRound[betsThisRound.Count - 1][0] == totalDiceLeft / 3)
            {
                int[] raise = { betsThisRound[betsThisRound.Count - 1][0] + 1, MostCommonBet() };
                return raise;
            }
            int[] bet = { totalDiceLeft / 3, MostCommonBet() };
            return bet;
        }

        /// <summary>
        /// Finds the most common bet FV in the last round of bets
        /// </summary>
        /// <returns>an int respresenting the facevalue</returns>
        private int MostCommonBet()
        {
            int[] faceValues = new int[VisibleHand.DiceType() - 1];
            faceValues[MostCommonInHand() - 2]++;
            foreach (int[] i in betsThisRound)
                faceValues[i[1] - 2]++;
            int mostCommon = -1;
            int numberOfBets = -1;
            for (int i = 0; i < faceValues.Length; i++)
                if (faceValues[i] >= numberOfBets)
                {
                    mostCommon = i + 2;
                    numberOfBets = faceValues[i];
                }
            return mostCommon;
        }

        /// <summary>
        /// returns the most common face value in the player's hand
        /// </summary>
        /// <returns>returns an int representing the facevalue</returns>
        private int MostCommonInHand()
        {
            int[] hand = VisibleHand.ArrayCountHand();
            int mostCommon = -1;
            int occurances = -1;
            for (int i = 1; i < hand.Length; i++)
                if (hand[i] >= occurances)
                {
                    mostCommon = i + 1;
                    occurances = hand[i];
                }
            return mostCommon;
        }
    }
}
