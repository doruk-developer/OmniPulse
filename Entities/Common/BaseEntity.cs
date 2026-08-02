namespace OmniPulse.Entities.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public System.DateTime CreatedDate { get; set; } = System.DateTime.UtcNow;
    public string CreatedBy { get; set; } = "System";
    public System.DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false; 
}