namespace AuthenticationAuthorization.Application.DTOs.PermissionDTOs
{
    public class GetPermissionDTO
    {
        public int Id { get; set; }
        public string PermissionName { get; set; } = null!;
        public string? Description { get; set; }

    }
}
