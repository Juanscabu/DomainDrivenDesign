namespace EcommerceProject.Domain.Common
{
    public abstract class BaseAuditableEntity: BaseEntity
    {
        public DateTime Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModify { get; set; }
        public string? LastModifyBy { get; set; }
    }
}
