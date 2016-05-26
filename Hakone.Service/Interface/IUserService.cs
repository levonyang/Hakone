using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Domain;
using Hakone.Domain.Enum;

namespace Hakone.Service
{
    public interface IUserService
    {
        UserLoginResult ValidateUser(string userName, string password);
        User GetUserByUserNameOrEmail(string userNameOrEmail);
        User GetUserByUserName(string userName);
        User GetUserByUserNameNoCache(string userName);
        User GetUserByEmail(string email);

        User InsertGuestUser();

        UserRegistrationResult RegisterUser(User user);
        bool IsRegistered(string userName);
        bool IsEmailExists(string email);
        bool IsEmptyPassword(int userId);

        void InsertUser(User user);

        bool ChangePassword(int userId, string newPassword);

        bool EditUserProfile(int userId, string email, int? bornYear, int? bornMonth, int? bornDay, bool? gender);

        User GetUserByGuid(string guid);
    }
}
