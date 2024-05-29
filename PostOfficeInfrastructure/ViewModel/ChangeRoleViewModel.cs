using Microsoft.AspNetCore.Identity;

namespace PostOfficeInfrastructure.ViewModel
{
    public class ChangeRoleViewModel
    {
        public string UserId { get; set; }
        public string UserContactNumber { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
        public ChangeRoleViewModel()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }

    }
}
