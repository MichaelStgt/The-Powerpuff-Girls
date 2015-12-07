namespace TexasHoldem.AI.SmartPlayer.Helpers
{
    using TexasHoldem.AI.SmartPlayer.PreCalculations;

    public static class MonteCarloAnalysis
    {
        private const int GameTrials = 250; // 1000 or 500 or 250

        public static int WinOddsMonteCarlo(string myCards, string openCards)
        {
            ulong pocketmask = Hand.ParseHand(myCards);
            ulong board = Hand.ParseHand(openCards);
            ulong dead = board | pocketmask;

            // Keep track of stats
            double win = 0.0, count = 0;

            // Loop for specified time duration
            while (count < GameTrials)
            {
                // Player and board info
                ulong boardmask = Hand.RandomHand(board, dead | pocketmask, 5);
                uint playerHandVal = Hand.Evaluate(pocketmask | boardmask);

                // Ensure that dead, board, and pocket cards are not
                // available to opponent hands.
                ulong deadmask = dead | boardmask | pocketmask;

                // Comparison Results
                bool greaterthan = true;
                bool greaterthanequal = true;

                // Get Opponent hand info
                ulong oppmask = Hand.RandomHand(deadmask, 2);
                uint oppHandVal = Hand.Evaluate(oppmask | boardmask);

                // Remove these opponent cards from future opponents
                deadmask |= oppmask;

                // Determine compare status
                if (playerHandVal < oppHandVal)
                {
                    greaterthan = greaterthanequal = false;
                    break;
                }
                else if (playerHandVal <= oppHandVal)
                {
                    greaterthan = false;
                }

                // Calculate stats
                if (greaterthan)
                    win += 1.0;
                else if (greaterthanequal)
                    win += 0.5;

                count += 1.0;
            }

            var result = (int)((win / count) * 100);
            // Return stats
            return (count == 0 ? 0 : result);
        }
    }
}