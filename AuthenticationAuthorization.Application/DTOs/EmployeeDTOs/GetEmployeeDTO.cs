namespace AuthenticationAuthorization.Application.DTOs.EmployeeDTOs
{
    public class GetEmployeeDTO
    {
        public string EmpCode { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? OfficialEmail { get; set; }
        public string? Gender { get; set; }
        public string? Department { get; set; }
        public string? Branch { get; set; }
        public string UserName { get; set; } = null!;
        public string? EmpStatus { get; set; }
    }
}
