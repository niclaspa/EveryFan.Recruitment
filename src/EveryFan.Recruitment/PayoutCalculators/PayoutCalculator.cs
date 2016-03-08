using System;
using System.Collections.Generic;
using System.Linq;

namespace EveryFan.Recruitment.PayoutCalculators
{
    public abstract class PayoutCalculator : IPayoutCalculator
    {
        protected abstract IReadOnlyList<PayingPosition> GetPayingPositions(Tournament tournament);

        public IReadOnlyList<TournamentPayout> Calculate(Tournament tournament)
        {
            IReadOnlyList<PayingPosition> payingPositions = this.GetPayingPositions(tournament);
            IReadOnlyList<TournamentEntry> orderedEntries = tournament.Entries.OrderByDescending(p => p.Chips).ToList();

            List<TournamentPayout> payouts = new List<TournamentPayout>();
            payouts.AddRange(payingPositions.Select((p, i) => new TournamentPayout()
            {
                Payout = p.Payout,
                UserId = orderedEntries[i].UserId
            }));

            return payouts;
        }

        protected static HashSet<int> GetDistinctRandomValues(int count, int min, int max)
        {
            // sanity check - should never happen
            int numberOfPossibleValues = max - min + 1;
            if (numberOfPossibleValues < count)
            {
                throw new ArgumentException("count is bigger than interval");
            }

            // not optimal in terms of performance - but should be good enough
            Random rand = new Random();
            HashSet<int> res = new HashSet<int>();
            while (res.Count < count)
            {
                res.Add(rand.Next(min, max));
            }

            return res;
        }
    }
}
