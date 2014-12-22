/*
 * Rohan Aras
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project1
{
    class TestPlayer : Player
    {

        public TestPlayer(Hand hand) : base(hand) { }

        public override int[] Bet()
        {
            Random r = new Random();
            int betOrCall = r.Next(1, 5);
            if (betOrCall == 1)
            {
                int[] call = { -1, 2 }; //HACK might be unnecesarry now
                return call;
            }
            if (betsThisRound.Count != 0)
            {
                int[] myBet = new int[2];
                myBet[0] = betsThisRound[betsThisRound.Count - 1][0] + 1;
                myBet[1] = betsThisRound[betsThisRound.Count - 1][1] + 1;
                if (myBet[1] <= 6)
                    return myBet;
                else
                {
                    myBet[1] = 2;
                    return myBet;
                }
            }
            else
            {
                int[] bet = {1, 2};
                return bet;
            } 
        }
    }
}
