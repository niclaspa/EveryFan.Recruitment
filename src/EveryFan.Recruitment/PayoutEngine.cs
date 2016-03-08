using System;
using System.Collections.Generic;
using EveryFan.Recruitment.PayoutCalculators;

namespace EveryFan.Recruitment
{
    public class PayoutEngine
    {
        private readonly IPayoutCalculatorFactory factory;

        public PayoutEngine()
        {
            this.factory = new PayoutCalculatorFactory();
        }

        public IReadOnlyList<TournamentPayout> Calculate(Tournament tournament)
        {
            IPayoutCalculator calculator;

            switch (tournament.PayoutScheme)
            {
                case PayoutScheme.FIFTY_FIFY:
                {
                    calculator = this.factory.Create<FiftyFiftyPayoutCalculator>();
                    break;
                }

                case PayoutScheme.WINNER_TAKES_ALL:
                {
                    calculator = this.factory.Create<WinnerTakesAllPayoutCalculator>();
                    break;
                }

                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(tournament.PayoutScheme));
                }
            }

            return calculator.Calculate(tournament);
        }
    }
}
