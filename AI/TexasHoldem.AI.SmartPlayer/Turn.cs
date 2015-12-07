namespace TexasHoldem.AI.SmartPlayer
{
    using TexasHoldem.Logic;
    using PreCalculations;
    using Helpers;

    public class Turn
    {
        public int Fold { get; set; }

        public int Call { get; set; }

        public int Raise { get; set; }

        public int AllIn { get; set; }

        public Turn DecideTurn(string ourCards, string communityCards, GameRoundType round, int gap)
        {
            var output = new Turn();
            var handourHand = new Hand(ourCards, communityCards);
            int chance = 0;

            switch (round)
            {
                case GameRoundType.PreFlop:
                    if (gap >= 4)
                    {
                        this.Call++;
                        this.Fold++;
                    }
                    else if (gap >= 2)
                    {
                        this.Raise++;
                        this.Call++;
                    }
                    else
                    {
                        this.AllIn++;
                        this.Raise++;
                    }

                    var handTypeValue = handourHand.HandTypeValue;
                    switch (handTypeValue)
                    {
                        case Hand.HandTypes.HighCard:
                            this.Raise++;
                            this.Call++;
                            break;
                        case Hand.HandTypes.Pair:
                            this.AllIn++;
                            this.Raise++;
                            this.Call++;
                            break;
                        default:
                            this.Raise++;
                            this.Call++;
                            this.Fold++;
                            break;
                    }
                    break;
                case GameRoundType.Flop:
                    chance = MonteCarloAnalysis.WinOddsMonteCarlo(ourCards, communityCards);
                    if (chance > 16 && chance <= 34)
                    {
                        this.Call++;
                        this.Fold++;
                    }
                    else if (chance > 34 && chance <= 40)
                    {
                        this.Raise++;
                        this.Call++;
                    }
                    else if (chance > 40 && chance <= 51)
                    {
                        this.AllIn++;
                        this.Raise++;
                        this.Call++;
                    }
                    else if (chance > 51)
                    {
                        this.AllIn++;
                        this.Raise++;
                    }
                    else
                    {
                        this.Fold++;
                    }
                    break;
                case GameRoundType.Turn:
                    chance = MonteCarloAnalysis.WinOddsMonteCarlo(ourCards, communityCards);
                    if (chance > 18 && chance <= 34)
                    {
                        this.Call++;
                        this.Fold++;
                    }
                    else if (chance > 34 && chance <= 41)
                    {
                        this.Raise++;
                        this.Call++;
                    }
                    else if (chance > 41 && chance <= 55)
                    {
                        this.AllIn++;
                        this.Raise++;
                        this.Call++;
                    }
                    else if (chance > 55)
                    {
                        this.AllIn++;
                        this.Raise++;
                    }
                    else
                    {
                        this.Fold++;
                    }
                    break;
                case GameRoundType.River:
                    chance = MonteCarloAnalysis.WinOddsMonteCarlo(ourCards, communityCards);
                    if (chance > 20 && chance <= 36)
                    {
                        this.Call++;
                        this.Fold++;
                    }
                    else if (chance > 36 && chance <= 45)
                    {
                        this.Raise++;
                        this.Call++;
                    }
                    else if (chance > 45 && chance <= 62)
                    {
                        this.AllIn++;
                        this.Raise++;
                        this.Call++;
                    }
                    else if (chance > 62)
                    {
                        this.AllIn++;
                        this.Raise++;
                    }
                    else
                    {
                        this.Fold++;
                    }
                    break;
            }
            return output;
        }
    }
}
