using System.ComponentModel.DataAnnotations;

namespace BankingInformatics.Frontend.Models.Parameters;

public class DepositParameters
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    public DateOnly Date { get; set; }

    [Required]
    [Range(1, double.MaxValue)]
    public decimal SourceSum { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Months { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal Percent { get; set; }
}
