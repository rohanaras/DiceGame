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

        public override void GameState(List<int[]> calls, List<string> names,
            Dictionary<string, int> diceLeft)
        {
        }

        public override int[] Call()
        {
            UI.HumanHand(base.VisibleHand.CurrentHandString());
            int[] call = new int[2];
            call[0] = UI.NumberOfDiceBet();
            if (call[0] == -1)
            {
                call[1] = 2; //HACK 
                return call;
            }
            call[1] = UI.FaceValueBet(); 
            return call;
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
