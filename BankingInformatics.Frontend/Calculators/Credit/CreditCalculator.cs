using BankingInformatics.Frontend.Models;
using BankingInformatics.Frontend.Models.Parameters;

namespace BankingInformatics.Frontend.Calculators.Credit;

public static class CreditCalculator
{
    public static PaymentCollection CalculatePayments(CreditParameters parameters) => parameters.PaymentType switch
    {
        PaymentType.Differentiated => CalculateDifferentiatedPayments(parameters),
        _ => throw new ArgumentOutOfRangeException(nameof(parameters))
    };

    private static PaymentCollection CalculateDifferentiatedPayments(CreditParameters parameters)
    {
        var bodyPaymentPart = decimal.Round(parameters.Sum / parameters.Months);

        var payments = EnumeratePaymentDates(parameters.Date, parameters.Months)
            .Select((date, index) =>
            {
                var remainingSum = parameters.Sum - (index * bodyPaymentPart);
                var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
                var daysInYear = DateTime.IsLeapYear(date.Year) ?
                    366 :
                    365;

                var percentPaymentPart = decimal.Round(
                    remainingSum * (parameters.Percent / 100) * daysInMonth / daysInYear, 2);

                var payment = new Payment
                {
                    Date = date,
                    BodySum = bodyPaymentPart,
                    PercentSum = percentPaymentPart,
                    Sum = bodyPaymentPart + percentPaymentPart,
                };

                return payment;
            })
            .ToArray();

        var paymentCollection = new PaymentCollection
        {
            Payments = payments,
            Overpayment = payments.Sum(payment => payment.PercentSum)
        };

        return paymentCollection;
    }

    private static IEnumerable<DateOnly> EnumeratePaymentDates(DateOnly date, int months)
    {
        var currentDate = date;

        foreach (var _ in Enumerable.Range(0, months))
        {
            currentDate = currentDate.AddMonths(1);

            yield return currentDate;
        }
    }
}
