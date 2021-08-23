using CAF.Core.Entities;
using CAF.Core.Enums;
using CAF.Core.ViewModel.Common.Response;
using CAF.Core.ViewModel.User.Request;
using CAF.Core.ViewModel.User.Response;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Data;

namespace CAF.Core.Service
{
    public interface IUserService
    {
        LoginUserResponse Login(LoginUserRequest userRequest, string secret);
        List<UserGridVM> GetAllUsers(GetUserGridRequest request);
        long AddUser(AddUserRequest request);
        UserVM GetUser(long id);
        void DeleteUser(long id);
        void UpdateUser(UpdateUserRequest request);
        /// <summary>
        /// Oturum açan kullanıcının hedeflenen kullanıcı rolünde yetkisi varmı kontrol etmektedir.
        /// </summary>
        /// <param name="targetRole"></param>
        void UserRoleControl(UserRole targetRole);
        List<KeyValueVM<UserRole, string>> GetRoles();
        List<UserAutoCompleteResponse> SearchAutoComplete(SearchUserRequest request);
        NotifyMessageResponse<Guid> ForgetPassword(ForgetPasswordRequest request);
        NotifyMessageResponse<Guid> NewPasswordByResetCode(NewPasswordByResetCodeRequest request);
        void SendMailForgetPassword(User user);
        void Logout();
        bool DatabaseInitializer(IConfiguration configuration, IServiceCollection services);
        public DataTable GetQuery(string query);
        void SendCreatePasswordMail(User user);
        /// <summary>
        /// Personelin profil resmini kaldırma
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        NotifyMessageResponse<long> RemoveProfileImage(long userId);
    }
}
