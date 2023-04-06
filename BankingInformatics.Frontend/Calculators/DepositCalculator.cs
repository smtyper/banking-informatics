using BankingInformatics.Frontend.Models;
using BankingInformatics.Frontend.Models.Parameters;
using DecimalMath;

namespace BankingInformatics.Frontend.Calculators;

public class DepositCalculator
{
    public static DepositPaymentCollection CalculateDepositPayments(DepositParameters parameters)
    {
        var payments = Extensions.EnumeratePaymentDates(parameters.Date, parameters.Months)
            .Select((date, index) =>
            {
                var monthNumber = index + 1;
                var sum = decimal.Round(parameters.SourceSum *
                                        DecimalEx.Pow(1 + (parameters.Percent / (100 * 12)), monthNumber));
                var percentSum = decimal.Round(sum - parameters.SourceSum);

                var payment = new DepositPayment { Date = date, PercentSum = percentSum, Sum = sum };

                return payment;
            })
            .ToArray();

        var paymentCollection = new DepositPaymentCollection
        {
            TotalPercentSum = payments.Last().PercentSum,
            TotalSum = payments.Last().Sum,
            DepositPayments = payments
        };

        return paymentCollection;
    }
}
