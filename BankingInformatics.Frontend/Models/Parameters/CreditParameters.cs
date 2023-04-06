using System.ComponentModel.DataAnnotations;
using BankingInformatics.Frontend.Calculators.Credit;

namespace BankingInformatics.Frontend.Models.Parameters;

public record CreditParameters
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    public DateOnly Date { get; set; }

    [Required]
    [Range(1, double.MaxValue)]
    public decimal Sum { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Months { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal Percent { get; set; }

    [Required]
    [EnumDataType(typeof(PaymentType))]
    public PaymentType PaymentType { get; set; }
}
