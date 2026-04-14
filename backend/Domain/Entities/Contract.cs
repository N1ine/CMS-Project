using Domain.Enums;

namespace Domain.Entities;

public class Contract
{
    public int Id { get; set; }

    public int CompanyId { get; set; }
    public int EmployeeId { get; set; }

    public string Position { get; set; } = null!;
    public string Description { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public decimal Wage { get; set; }

    public int ContractStatusId { get; set; }

    public ContractStatus Status => CalculateStatus();

    private ContractStatus CalculateStatus()
    {
        var now = DateTime.UtcNow.Date;
        if (StartDate > now)
            return ContractStatus.NotStarted;

        if (EndDate.HasValue && EndDate.Value.Date < now)
            return ContractStatus.Finished;

        return ContractStatus.Active;
    }
}