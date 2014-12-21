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
        private List<int[]> calls;

        public TestPlayer(Hand hand) : base(hand) { }

        public override void GameState(List<int[]> calls, List<string> names, Dictionary<string, int> diceLeft)
        {
            this.calls = calls;
        }

        public override int[] Call()
        {
            if (calls.Count != 0)
            {
                int[] previous = new int[2];
                previous[0] = calls[calls.Count - 1][0] + 1;
                previous[1] = calls[calls.Count - 1][1] + 1;
                if (previous[1] <= 6)
                    return previous;
                else
                {
                    previous[1] = 2;
                    return previous;
                }
            }
            else
            {
                int[] thisCall = {1, 2};
                return thisCall;
            } 
        }
    }
}
