using s2industries.ZUGFeRD;

namespace FredsWorkmate.Database
{
    public enum Currency
    {
        USD = 0,
        EUR,
    }

    public static class CurrencyUtil
    {
        public static CurrencyCodes ToCurrencyCodes(Currency currency)
        {
            return currency switch
            {
                Currency.EUR => CurrencyCodes.EUR,
                Currency.USD => CurrencyCodes.USD,
                _ => throw new ArgumentException("Unknown currency: " + currency),
            };
        }
    }
}
