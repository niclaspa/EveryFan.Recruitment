using EveryFan.Recruitment.PayoutCalculators;

namespace EveryFan.Recruitment
{
    public interface IPayoutCalculatorFactory
    {
        T Create<T>() where T : PayoutCalculator, new();
    }

    public class PayoutCalculatorFactory : IPayoutCalculatorFactory
    {
        public T Create<T>() where T : PayoutCalculator, new()
        {
            return new T();
        }
    }
}
