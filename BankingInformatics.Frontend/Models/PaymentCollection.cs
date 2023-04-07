namespace BankingInformatics.Frontend.Models;

public record PaymentCollection
{
    public IReadOnlyCollection<Payment> Payments { get; init; } = null!;

    public decimal Overpayment { get; init; }
}

public record Payment
{
    public DateOnly Date { get; init; }

    public decimal? BodySum { get; init; }

    public decimal? PercentSum { get; init; }

    public decimal Sum { get; init; }
}
