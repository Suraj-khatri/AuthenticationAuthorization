namespace AuthenticationAuthorization.Application.DTOs.EmployeeDTOs
{
    public class AddEmployeeDTO
    {
        public string EmpCode { get; set; } = null!;
        public string UserName { get; set; }

        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string LastName { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? OfficialEmail { get; set; }

        public int GenderId { get; set; }

        public int DepartmentId { get; set; }

        public int BranchId { get; set; }

        public int? CompanyId { get; set; }

        public bool IsActive { get; set; }

        public bool IsTemporary { get; set; }

        public string? EmpStatus { get; set; }
    }
}
