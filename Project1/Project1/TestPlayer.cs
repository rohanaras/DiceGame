using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project1
{
    class TestPlayer : Player
    {

        public TestPlayer(Hand hand) : base(hand) { }

        public override void GameState(List<int[]> calls, List<string> names, Dictionary<string, int> diceLeft)
        {
        }

        public override int[] Call()
        {
            throw new NotImplementedException();
        }
    }
}
