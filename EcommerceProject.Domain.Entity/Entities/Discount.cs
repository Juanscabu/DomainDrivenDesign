using EcommerceProject.Domain.Common;
using EcommerceProject.Domain.Enums;

namespace EcommerceProject.Domain.Entities
{
    public class Discount : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Percent { get; set; }
        public DiscountStatus Status { get; set; }
    }
}
