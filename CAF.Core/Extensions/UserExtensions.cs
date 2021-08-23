using CAF.Core.Entities;
using CAF.Core.Enums;

namespace CAF.Core.Extensions
{
    public static class UserExtensions
    {
        public static string GetUserName(this User user)
        {
            if (user == null) return string.Empty;

            return $"{user.FirstName} {user.LastName}";
        }
    }
}
