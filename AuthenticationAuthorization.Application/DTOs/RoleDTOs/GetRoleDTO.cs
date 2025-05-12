namespace AuthenticationAuthorization.Application.DTOs.RoleDTOs
{
    public class GetRoleDTO
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
