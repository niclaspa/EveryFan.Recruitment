using System;
using System.Collections.Generic;
using System.Linq;

namespace EveryFan.Recruitment.PayoutCalculators
{
    /// <summary>
    /// FiftyFifty payout calculator. The 50/50 payout scheme returns double the tournament buyin to people
    /// who finish in the top half of the table. If the number of runners is odd the player in the middle position
    /// should get their stake back. Any tied positions should have the sum of the amount due to those positions
    /// split equally among them.
    /// </summary>
    public class FiftyFiftyPayoutCalculator : PayoutCalculator
    {
        protected override IReadOnlyList<PayingPosition> GetPayingPositions(Tournament tournament)
        {
            IReadOnlyList<TournamentEntry> orderedEntries = tournament.Entries.OrderByDescending(p => p.Chips).ToList();

            // work out the chips value of the last paying position
            int lastPayoutPosition = (int)Math.Ceiling((double)orderedEntries.Count/2 - 1);
            long lastPayoutValue = orderedEntries[lastPayoutPosition].Chips;

            // anything greater than that, get the full payout, anything less gets nothing
            List<PayingPosition> res = orderedEntries
                .Where(x => x.Chips > lastPayoutValue)
                .Select((x, i) => new PayingPosition { Payout = tournament.BuyIn*2, Position = i })
                .ToList();

            // the amount to share among the last payout position
            int prizePoolRemainder = tournament.PrizePool - tournament.BuyIn*2*res.Count;

            // the number of players to share it between
            int lastPayoutShareCount = orderedEntries.Count(x => x.Chips == lastPayoutValue);

            // the remainder after splitting the last payout evenly across the last positions
            int lastPayoutRemainder = prizePoolRemainder % lastPayoutShareCount;

            // randomly decide who's going to recieve +1
            HashSet<int> lastPayoutRemainderRecipients = GetDistinctRandomValues(lastPayoutRemainder, 0, lastPayoutShareCount - 1);

            // add on the last position(s), sharing the remainder of the prize pool
            int resCount = res.Count;
            res.AddRange(
                Enumerable.Range(0, lastPayoutShareCount)
                    .Select((x, i) => new PayingPosition
                    {
                        Payout = prizePoolRemainder / lastPayoutShareCount + (lastPayoutRemainderRecipients.Contains(i) ? 1 : 0),
                        Position = resCount + i
                    }));

            return res;
        }
    }
}
