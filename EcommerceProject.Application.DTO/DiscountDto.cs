using EcommerceProject.Application.DTO.Enums;

namespace EcommerceProject.Application.DTO
{
    public record DiscountDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Percent { get; set; }
        public DiscountStatusDto Status { get; set; }
    }
}
