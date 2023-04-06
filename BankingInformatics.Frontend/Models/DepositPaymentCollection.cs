namespace BankingInformatics.Frontend.Models;

public record DepositPaymentCollection
{
    public decimal TotalPercentSum { get; init; }

    public decimal TotalSum { get; init; }

    public IReadOnlyCollection<DepositPayment> DepositPayments { get; init; } = null!;
}

public record DepositPayment
{
    public DateOnly Date { get; init; }

    public decimal PercentSum { get; init; }

    public decimal Sum { get; init; }
}
