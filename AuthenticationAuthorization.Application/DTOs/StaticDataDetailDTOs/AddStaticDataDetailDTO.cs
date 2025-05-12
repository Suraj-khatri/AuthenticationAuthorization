namespace AuthenticationAuthorization.Application.DTOs.StaticDataDetailDTOs
{
    public class AddStaticDataDetailDTO
    {
        public string DetailName { get; set; } = null!;
        public string DetailDesc { get; set; } = null!;
        public int TypeId { get; set; }
    }
}
