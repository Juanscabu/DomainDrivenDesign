namespace EcommerceProject.Service.WebApi.Helpers
{
    public record AppSettings
    {
        public string OriginCors { get; set; }
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

    }
}
