using BankingInformatics.Frontend.Models;
using BankingInformatics.Frontend.Models.Parameters;
using DecimalMath;

namespace BankingInformatics.Frontend.Calculators;

public static class CreditCalculator
{
    public static PaymentCollection CalculatePayments(CreditParameters parameters) => parameters.PaymentType switch
    {
        PaymentType.Differentiated => CalculateDifferentiatedPayments(parameters),
        PaymentType.Annuity => CalculateAnnuityPayments(parameters),
        _ => throw new ArgumentOutOfRangeException(nameof(parameters))
    };

    private static PaymentCollection CalculateAnnuityPayments(CreditParameters parameters)
    {
        var monthRate = parameters.Percent / (100 * 12);
        var periods = (parameters.Months * -1);

        var monthPayment = decimal.Round(parameters.Sum * (monthRate / (1 - DecimalEx.Pow(1 + monthRate, periods))));
        var overpayment = decimal.Round((monthPayment * parameters.Months) - parameters.Sum);

        var paymentCollection = new PaymentCollection
        {
            Payments = Extensions.EnumeratePaymentDates(parameters.Date, parameters.Months)
                .Select(date => new Payment { Date = date, Sum = monthPayment })
                .ToArray(),
            Overpayment = overpayment
        };

        return paymentCollection;
    }

    private static PaymentCollection CalculateDifferentiatedPayments(CreditParameters parameters)
    {
        var bodyPaymentPart = decimal.Round(parameters.Sum / parameters.Months);

        var payments = Extensions.EnumeratePaymentDates(parameters.Date, parameters.Months)
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
            Overpayment = payments.Sum(payment => payment.PercentSum!.Value)
        };

        return paymentCollection;
    }
}
