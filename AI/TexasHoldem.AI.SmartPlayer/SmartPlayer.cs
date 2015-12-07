namespace TexasHoldem.AI.SmartPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helpers;
    using PreCalculations;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Players;

    public class SmartPlayer : BasePlayer
    {
        private IList<int> opponentActions = new List<int>();

        public SmartPlayer()
            : base()
        {
        }

        public override string Name { get; } = "The_Powerpuff_Girls_Player" + Guid.NewGuid();

        public override void EndRound(EndRoundContext context)
        {
            var opponentMoves = context.RoundActions.Where(x => x.PlayerName != this.Name);

            foreach (var oponentAction in opponentMoves)
            {
                this.opponentActions.Add((int)oponentAction.Action.Type);
            }

            base.EndRound(context);
        }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            var ourCards = ParseHandToString.GenerateStringFromCard(this.FirstCard) + " " + ParseHandToString.GenerateStringFromCard(this.SecondCard);
            var gap = Hand.GapCount(Hand.ParseHand(ourCards));
            var openedCards = this.CommunityCards;
            var round = context.RoundType;
            var openedCardsString = string.Empty;
            var turn = new Turn();
            var myMoney = context.MoneyLeft;
            var allIn = PlayerAction.Raise(myMoney);
            var call = PlayerAction.CheckOrCall();
            var fold = PlayerAction.Fold();
            var smallBlind = context.SmallBlind;
            var moneyToCall = context.MoneyToCall;

            if (round != GameRoundType.PreFlop)
            {
                for (int i = 0; i < openedCards.Count; i++)
                {
                    if (i != 0)
                    {
                        openedCardsString = openedCardsString + " " + ParseHandToString.GenerateStringFromCard(openedCards.ElementAt(i));
                    }
                    else
                    {
                        openedCardsString = ParseHandToString.GenerateStringFromCard(openedCards.ElementAt(i));
                    }
                }

                turn.DecideTurn(ourCards, openedCardsString, round, gap);
                var previous = context.PreviousRoundActions.LastOrDefault().Action;

                if (previous == null)
                {
                    if (turn.Call >= 2)
                    {
                        return call;
                    }
                    else if (turn.Raise == 2)
                    {
                        return PlayerAction.Raise(smallBlind);
                    }
                    else if (turn.Raise >= 3)
                    {
                        return PlayerAction.Raise(smallBlind * 2);
                    }
                    else if (turn.AllIn >= 2)
                    {
                        return allIn;
                    }
                    else if (turn.Fold >= 2)
                    {
                        return fold;
                    }
                }
                else if (previous == call)
                {
                    if (turn.Call >= 2 && (moneyToCall / myMoney < 0.3))
                    {
                        return call;
                    }
                    else if (turn.Raise >= 2 && (moneyToCall / myMoney < 0.50))
                    {
                        return PlayerAction.Raise(smallBlind);
                    }
                    else if (turn.AllIn >= 2)
                    {
                        return PlayerAction.Raise(smallBlind * 2);
                    }
                    else if (turn.Fold >= 2)
                    {
                        return fold;
                    }
                }
                else if (previous.Type == PlayerActionType.Raise)
                {
                    if (turn.Call >= 2 && (moneyToCall / myMoney < 0.50))
                    {
                        return call;
                    }
                    else if (turn.AllIn >= 2)
                    {
                        return PlayerAction.Raise(smallBlind * 2);
                    }
                    else if (turn.Fold >= 2)
                    {
                        return fold;
                    }
                }
                else if (context.IsAllIn)
                {
                    if (turn.Call >= 2 && (moneyToCall / myMoney < 0.5))
                    {
                        return call;
                    }
                    else if (turn.Raise >= 2 && (moneyToCall / myMoney < 0.75))
                    {
                        return call;
                    }
                    else if (turn.AllIn >= 2)
                    {
                        return call;
                    }
                    else if (turn.Fold >= 2)
                    {
                        return fold;
                    }
                }
            }
            else
            {
                if (!context.IsAllIn)
                {
                    if (turn.Call >= 2)
                    {
                        return call;
                    }
                    else if (turn.Raise >= 2)
                    {
                        return PlayerAction.Raise(smallBlind);
                    }
                    else if (turn.AllIn >= 2)
                    {
                        return allIn;
                    }
                    else if (turn.Fold >= 2)
                    {
                        return fold;
                    }
                }
                else
                {
                    if (turn.Call >= 2 && (moneyToCall / myMoney < 0.5))
                    {
                        return call;
                    }
                    else if (turn.Raise >= 2 && (moneyToCall / myMoney < 0.75))
                    {
                        return call;
                    }
                    else if (turn.AllIn >= 2)
                    {
                        return call;
                    }
                    else if (turn.Fold >= 2)
                    {
                        return fold;
                    }
                }
            }

            return call;
        }
    }
}