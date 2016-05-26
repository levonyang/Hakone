using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Cube;
using CacheAspect.Attributes;
using Hakone.Data.LinqUtility;
using Hakone.Domain;
using Hakone.Domain.Enum;
using Hakone.Service;

namespace Hakone.Service
{
    public class UserService : GenericController<User, Hakone.Domain.HakoneDBDataContext>, IUserService
    {
        public Domain.Enum.UserLoginResult ValidateUser(string usernameOrEmail, string password)
        {
            var user = GetUserByUserNameOrEmail(usernameOrEmail);

            if (user == null)
                return UserLoginResult.UserNotExist;
            if (user.IsLocked)
                return UserLoginResult.Locked;
            if (!user.IsActivated)
                return UserLoginResult.NotActive;

            var passwordHash = Cipher.Hash(password, user.PasswordSalt);
            var isValid = passwordHash.Equals(user.Password);
            
            if (!isValid)
                return UserLoginResult.WrongPassword;

            //save last login date
            user.LastLoginDate = DateTime.Now;
            user.LastActiveIP = XRequest.GetIP();
            user.TryCount = 0;
            UserService.SubmitChanges();
            return UserLoginResult.Successful;
        }

        public User GetUserByUserNameOrEmail(string userNameOrEmail)
        {
            User user = null;


            user = UserService.SelectAll().FirstOrDefault(u => u.UserName == userNameOrEmail);

            if (user == null && userNameOrEmail.Contains("@"))
            {
                user = UserService.SelectAll().FirstOrDefault(u => u.Email == userNameOrEmail);
            }

            return user;
        }

        [Cache.Cacheable(CacheKey.GetUserByUserName)]
        public User GetUserByUserName(string userName)
        {
            return SelectAll().FirstOrDefault(r => r.UserName == userName);
        }

        public User GetUserByUserNameNoCache(string userName)
        {
            return SelectAll().FirstOrDefault(r => r.UserName == userName);
        }

        public User GetUserByEmail(string email)
        {
            return SelectAll().FirstOrDefault(r => r.Email == email);
        }


        public User InsertGuestUser()
        {
            var guest = new User
            {
                UserName = Guid.NewGuid().ToString(),
                IsActivated = true,
                LastLoginDate = DateTime.Now,
            };

            return guest;
        }


        public UserRegistrationResult RegisterUser(User user)
        {
            if (user == null)
                throw new ArgumentException("当前用户为空");

            var result = new UserRegistrationResult();  

            if (IsRegistered(user.UserName))
            {
                result.AddError("用户名已经存在");
                return result;
            }
            if (String.IsNullOrEmpty(user.Email))
            {
                result.AddError("邮箱不能为空");
                return result;
            }
            //if (user.Email.IsValidEmail())
            //{
            //    result.AddError("邮件格式错误");
            //    return result;
            //}
            //if (user.Password.IsNullOrEmpty())
            //{
            //    result.AddError("密码不能为空");
            //    return result;
            //}

            InsertUser(user);

            return result;
        }


        public bool IsRegistered(string userName)
        {
            return SelectAll().FirstOrDefault(r => r.UserName == userName) != null;
        }

        public bool IsEmailExists(string email)
        {
            return SelectAll().FirstOrDefault(r => r.Email == email) != null;
        }


        public void InsertUser(User user)
        {
            var passwordSalt = Cipher.GenerateSalt();
            var passwordHash = Cipher.Hash(user.Password, passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.Password = passwordHash;
            user.BindAccount = user.BindAccount ?? string.Empty;

            //setting default value:
            user.RoleID = EnumUserRole.NormalUser.ToInt();
            user.Grade = 1;
            user.EntryDate = DateTime.Now;
            user.LastLoginDate = DateTime.Now;
            user.IsActivated = true;
            user.IsLocked = false;
            user.IsAutoAdded = false;
            user.LoginTimes = 1;
            user.LastActiveIP = XRequest.GetIP();
            user.GUID = Guid.NewGuid();
            UserService.Insert(user);
            UserService.SubmitChanges();
        }


        public bool ChangePassword(int userId, string newPassword)
        {
            var user = SelectAll().FirstOrDefault(u => u.UserID == userId);
            if (user == null) return false;

            string passwordSalt = Cipher.GenerateSalt();
            string passwordHash = Cipher.Hash(newPassword, passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.Password = passwordHash;
            UserService.Update(user,true);
            return true;
        }

        public bool EditUserProfile(int userId, string email, int? bornYear, int? bornMonth, int? bornDay, bool? gender)
        {
            var user = SelectAll().Single(r => r.UserID == userId);

            user.Email = email;
            user.BornYear = bornYear;
            user.BornMonth = bornMonth;
            user.BornDay = bornDay;

            user.Gender = gender;
            UserService.Update(user,true);

            return true;

        }


        public User GetUserByGuid(string guid)
        {
            Guid userGuid = Guid.Parse(guid);
            return SelectAll().FirstOrDefault(r => r.GUID == userGuid);
        }


        public bool IsEmptyPassword(int userId)
        {
            var user = GetEntity(userId);
            if (user == null) return false;

            var password = string.Empty;
            var passwordHash = Cipher.Hash(password, user.PasswordSalt);
            return passwordHash.Equals(user.Password);
        }
    }
}
