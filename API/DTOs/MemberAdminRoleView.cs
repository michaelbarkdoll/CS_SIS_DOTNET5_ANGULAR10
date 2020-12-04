using System.Collections.Generic;
using API.Entities;

namespace API.DTOs
{
    public class MemberAdminRoleView
    {
        public MemberAdminRoleView()
        {
        }

        public MemberAdminRoleView(int userID, int roleID, string userName)
        {
            this.UserID = userID;
            this.RoleID = roleID;
            this.UserName = userName;

        }
        // public int Id { get; set; }     // Primary key of our database - requires name Id for Entity Framework (not case sensitive)
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public List<string> Roles { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; } 
    }
}