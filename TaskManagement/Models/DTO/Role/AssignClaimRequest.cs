namespace TaskManagement.Models.DTO.Role
{
    public class AssignClaimRequest
    {
        public string RoleId { get; set; }
        public List<string> Permissions { get; set; }
    }
}
