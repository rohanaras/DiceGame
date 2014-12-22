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
    class Human : Player
    {
        protected UserInterface UI;

        public Human(Hand hand, string playerName, UserInterface UI)
            : base(hand, playerName)
        {
            this.UI = UI;
        }

        public override int[] Bet()
        {
            UI.HumanHand(base.VisibleHand.CurrentHandString());
            int[] bet = new int[2];
            bet[0] = UI.NumberOfDiceBet();
            if (bet[0] == -1)
            {
                bet[1] = 2; //HACK 
                return bet;
            }
            bet[1] = UI.FaceValueBet(); 
            return bet;
        }

        public override void RedoNODBet(int NOD)
        {
            UI.IllegalNODBet(NOD);
        }

        public override void RedoFaceValueBet(int FV)
        {
            UI.IllegalFVBet(FV);
        }
    }
}
