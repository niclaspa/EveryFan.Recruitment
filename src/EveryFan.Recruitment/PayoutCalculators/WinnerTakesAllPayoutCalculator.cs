using System.Collections.Generic;
using System.Linq;

namespace EveryFan.Recruitment.PayoutCalculators
{
    /// <summary>
    /// Winner takes all payout calculator, the winner recieves the entire prize pool. In the event of a tie for the winning position the
    /// prize pool is split equally between the tied players.
    /// </summary>
    public class WinnerTakesAllPayoutCalculator : PayoutCalculator
    {
        protected override IReadOnlyList<PayingPosition> GetPayingPositions(Tournament tournament)
        {
            long winningChipsAmount = tournament.Entries.Max(x => x.Chips);
            int numberOfWinners = tournament.Entries.Count(x => x.Chips == winningChipsAmount);
            int payout = tournament.PrizePool / numberOfWinners;
            int payoutRemainder = tournament.PrizePool % numberOfWinners;

            // randomly decide who's going to recieve + 1
            HashSet<int> payoutRemainderRecipients = GetDistinctRandomValues(payoutRemainder, 0, numberOfWinners - 1);

            // winners get payout
            List<PayingPosition> res = tournament.Entries
                .Where(x => x.Chips == winningChipsAmount)
                .Select((x, i) => new PayingPosition { Payout = payout, Position = i })
                .ToList();

            // add on the +1
            foreach (int index in payoutRemainderRecipients)
            {
                res[index].Payout += 1;
            }

            return res;
        }
    }
}
