namespace MicroservicePermissions.Application.DTOs
{
    public class PermissionElasticDto
    {
        public int Id { get; set; }
        public string EmployeeForename { get; set; } = default!;
        public string EmployeeSurname { get; set; } = default!;
        public int PermissionTypeId { get; set; }
        public DateTime PermissionDate { get; set; }
    }
}
