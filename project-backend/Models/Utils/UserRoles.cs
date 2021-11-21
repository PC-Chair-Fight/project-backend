namespace project_backend.Models.Utils
{
    public enum UserRoles
    {
        User,
        Worker,
        Admin
    }


    public static class UserRolesExtension
    {
        public static string ToString(this UserRoles claim)
        {
            return claim switch
            {
                UserRoles.User => "User",
                UserRoles.Worker => "Worker",
                UserRoles.Admin => "Admin",
                _ => ""
            };
        }

    }
}
