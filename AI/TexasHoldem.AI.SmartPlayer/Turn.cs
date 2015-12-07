namespace TexasHoldem.AI.SmartPlayer
{
    using Helpers;
    using PreCalculations;
    using TexasHoldem.Logic;

    /// <summary>
    /// Class that holds the values, which to be used for decisionmaking for the Bot
    /// </summary>
    public class Turn
    {
        public int Fold { get; set; }

        public int Call { get; set; }

        public int Raise { get; set; }

        public int AllIn { get; set; }

        /// <summary>
        /// <para>This method calculates chances of our hand to win and based on the results returns values, which to be used for decisionmaking</para>
        /// <para>Used methods to calculate chances:</para>
        ///     <para>-> for pre-flop: uses the gap parameter and checkes wether we have High Card or a Pair</para>
        ///     <para>-> for all other rounds: uses MonteCarlo algorythm on 250 game trials, which is with diff: (3 to 2.5)</para>
        /// </summary>
        /// <param name="ourCards"><c>string</c> that contains our cards <example>example: "2h 3h"</example></param>
        /// <param name="communityCards"><c>string</c> that contains the community cards <example>example: "Jh Qh 2d"</example></param>
        /// <param name="currentRound"><c>GameRoundType</c> is enum that informs us in which round are we currently playing <example>example: GameRoundType.PreFlop</example></param>
        /// <param name="gapBetweenOurCards"><c>int</c> that informs us what is the gap between the cards in our hand
        /// <example>example: If we have Jack and 8 -> gap = 2, 
        /// If we have King and 6 -> gap = 6</example></param>
        /// <returns><c>Turn</c> object that holds the chance values for Fold, Call, Raise and AllIn</returns>
        /// <seealso cref="http://www.codeproject.com/Articles/19092/More-Texas-Holdem-Analysis-in-C-Part-2"/>
        public Turn DecideChanceForAction(string ourCards, string communityCards, GameRoundType currentRound, int gapBetweenOurCards)
        {
            var output = new Turn();
            var ourHand = new Hand(ourCards, communityCards);
            int chance = 0;

            switch (currentRound)
            {
                case GameRoundType.PreFlop:
                    if (gapBetweenOurCards >= 4)
                    {
                        this.Call++;
                        this.Fold++;
                    }
                    else if (gapBetweenOurCards >= 2)
                    {
                        this.Raise++;
                        this.Call++;
                    }
                    else
                    {
                        this.AllIn++;
                        this.Raise++;
                    }

                    var handTypeValue = ourHand.HandTypeValue;
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