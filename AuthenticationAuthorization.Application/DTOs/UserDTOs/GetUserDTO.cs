namespace AuthenticationAuthorization.Application.DTOs.UserDTOs
{
    public class GetUserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsTemporary { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }
        public DateTime? LastPasswordChangedOn { get; set; }
        public string? Role { get; set; }
    }
}
