using CAF.Core.Constant;
using CAF.Core.Entities;
using CAF.Core.Enums;
using CAF.Core.Exception;
using CAF.Core.Extensions;
using CAF.Core.Helper;
using CAF.Core.Helper.Interface;
using CAF.Core.Repository;
using CAF.Core.Service;
using CAF.Core.ViewModel.BlobStorage.Request;
using CAF.Core.ViewModel.Common.Response;
using CAF.Core.ViewModel.User.Request;
using CAF.Core.ViewModel.User.Response;
using CAF.Service.Services.Base;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;

namespace CAF.Service.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IServiceProvider serviceProvider) : base(httpContextAccessor, serviceProvider)
        {
            this.userRepository = userRepository;
        }
        private string CreateToken(Dictionary<string, string> values, string secret)
        {
            return Core.Helper.TokenHelper.CreateToken(values, secret, DateTime.UtcNow.AddDays(7));
        }



        public LoginUserResponse Login(LoginUserRequest userRequest, string secret)
        {            
            var user = userRepository.Login(userRequest.Email, userRequest.Password);

            if (user != null)
            {

                //if (user.UserType == UserType.QuitJob)
                //{
                //    throw new BadRequestException("İşten ayrılmış kişi giriş yapamaz.");
                //}
                Dictionary<string, string> tokenValues = new Dictionary<string, string>();
                tokenValues.Add(ClaimTypes.Name, user.Id.ToString());
                tokenValues.Add(ClaimTypes.Role, user.RoleType.ToString());
                tokenValues.Add(UserRoleConstant.LoginId, Guid.NewGuid().ToString());

                #region Agency
                //Guid? agencyId = null;
                //if (user.AgencyId.HasValue)
                //{
                //    agencyId = user.AgencyId.Value;
                //    tokenValues.Add(UserRoleConstant.AgencyIdKey, agencyId.ToString());
                //}
                //else tokenValues.Add(UserRoleConstant.AgencyIdKey, "");
                #endregion

                #region Customer Login
                //if (user.CustomerForId.HasValue)
                //{
                //    tokenValues.Add(UserRoleConstant.CustomerId, user.CustomerForId.Value.ToString());
                //}
                #endregion

                //var image = string.IsNullOrEmpty(user.Agency?.LogoUrl) ? "https://deryasigorta.com/assets/images/logo/favicon.ico" : user.Agency?.LogoUrl;
                return new LoginUserResponse()
                {
                    Email = user.Email,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Token = CreateToken(tokenValues, secret),
                    UserId = user.Id,
                    UserRole = user.RoleType,
                    ProfileImageUrl = user.ProfileImageUrl
                };
            }
            else throw new BadRequestException("Kullanıcı bulunamadı. Bilgileri kontrol ediniz", "", Core.Enums.ErrorType.Warning);

        }

        #region CRUD

        public List<UserGridVM> GetAllUsers(GetUserGridRequest request)
        {
            var queryableList = userRepository.GetAllUsersWithJoins().Where(x => x.RoleType <= CurrentUser.Role);

            return queryableList
                .Select(x => new UserGridVM(x)).ToList();
        }

        public long AddUser(AddUserRequest request)
        {
            //if (!IsTopAdmin)
            //    request.AgencyId = CurrentUser.AgencyId;

            //Kendi yetkisine göre mi ekliyor.
            UserRoleControl(request.RoleType);

            #region Otomatik Amir Atama
            //if (request.RoleType == UserRole.AgencyUser && !request.ManagerUserId.HasValue && request.AgencyId.HasValue)
            //{
            //    var admins = userRepository
            //                     .Where(x => x.RoleType == UserRole.AgencyAdmin && x.AgencyId == request.AgencyId.Value).ToList();
            //    if (admins.Count == 1)
            //    {
            //        request.ManagerUserId = admins[0].Id; // Otomatik amir atamak
            //    }
            //}
            #endregion
            //Aynı kullanıcı var mı?
            var user = userRepository.Get(x => x.Email == request.Email);
            if (user != null)
                throw new BadRequestException($"{request.Email} mail adresi zaten kullanılmaktadır.");

            var userEntity = request.ToEntity();
            if (string.IsNullOrEmpty(userEntity.Password))
                userEntity.Password = "NoneSet";

            var userId = userRepository.Add(userEntity);


            if (!string.IsNullOrEmpty(request.ProfileImageBase64))
            {
                var storageService = base.ResolveService<IStorageService>();

                //Dokümanın uplaod işlemleri
                var uploadRequest = new StorageUploadBytesRequest()
                {
                    FileBytes = Convert.FromBase64String(request.ProfileImageBase64.RemovePrefixBase64()),
                    FileName = $"{userId}.png",
                    FolderName = StorageFolderConstant.UserProfileImage,
                    ExistFileAction = Core.Enums.ExistFileAction.Overwrite
                };
                var response = storageService.FileUpload(uploadRequest);

                userEntity.ProfileImageUrl = response.FileUrl;

                userRepository.Update(userEntity);
            }

            if (userEntity.Password == "NoneSet")
                SendCreatePasswordMail(userEntity);

            ClearOutputCache(CachePath.UsersLogin, CachePath.SearchAutoComplete);
            return userId;
        }

        public UserVM GetUser(long id)
        {
            var user = userRepository.FindAndIncludes(id);

            return new UserVM(user);
        }

        public void DeleteUser(long id)
        {
            var user = userRepository.FindOrThrow(id);

            //Kendi yetkisine göre mi ekliyor.
            UserRoleControl(user.RoleType);

            userRepository.Delete(user);

            ClearOutputCache(CachePath.UsersLogin, CachePath.SearchAutoComplete);
        }

        public void UpdateUser(UpdateUserRequest request)
        {
            //Kendi yetkisine göre mi ekliyor.
            UserRoleControl(request.RoleType);

            //Aynı kullanıcı var mı?
            //1- Aynı mail kontorlü =>  x.Email == request.Email
            //2- Kendi kullanıcısı dışındaki kullanıcılara bak =>  x.Id != request.Id 

            var exist = userRepository.Get(x => x.Email == request.Email && x.Id != request.Id);
            if (exist != null)
            {
                throw new BadRequestException($"{request.Email} kullanıcısı zaten sistemde mevcut.");
            }

            var user = userRepository.FindOrThrow(request.Id);

            //if (user.RoleType == UserRole.Customer && request.RoleType != UserRole.Customer)
            //    throw new BadRequestException($"{user.GetUserName()} müşterinin rolleri değişemez.");

            //if (user.RoleType != UserRole.Customer && request.RoleType == UserRole.Customer)
            //    throw new BadRequestException($"{user.GetUserName()} kullanıcının rolü müşteri olamaz.");

            user.DbState = request.DbState;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.RoleType = request.RoleType;
            user.SubRoles = request.SubRoles;
            //user.UserType = request.UserType;
            //user.ManagerUserId = request.ManagerUserId;
            //user.AgencyDimensionId = request.AgencyDimensionId;

            //if (IsTopAdmin) //Sadece topadmin değiştirebilir. Aksi tartirde ne gönderildiği önemli değildir.
            //{
            //    user.AgencyId = request.AgencyId;
            //}

            if (!string.IsNullOrEmpty(request.Password))
            {
                user.Password = request.Password.ToMD5Hash();
            }

            if (!string.IsNullOrEmpty(request.ProfileImageBase64))
            {
                var storageService = base.ResolveService<IStorageService>();

                //Dokümanın uplaod işlemleri
                var uploadRequest = new StorageUploadBytesRequest()
                {
                    FileBytes = Convert.FromBase64String(request.ProfileImageBase64.RemovePrefixBase64()),
                    FileName = $"{user.Id}.png",
                    FolderName = StorageFolderConstant.UserProfileImage,
                    ExistFileAction = Core.Enums.ExistFileAction.Overwrite
                };
                var response = storageService.FileUpload(uploadRequest);

                user.ProfileImageUrl = response.FileUrl;

            }
            userRepository.Update(user);

            ClearOutputCache(CachePath.UsersLogin, CachePath.SearchAutoComplete);
        }

        #endregion

        public void UserRoleControl(UserRole targetRole)
        {
            var currentRole = Core.Utilities.AppStatic.Session.Role;
            switch (currentRole)
            {
                case UserRole.Standard:
                    throw new BadRequestException("Standart kullanıcı bu kullanıcıya işlem yapamaz.");
                case UserRole.SystemAdmin:
                    break;
                default:
                    throw new BadRequestException("Rolü olmayan hesap kullanıcılara işlem yapamaz.");
            }
        }

        public List<KeyValueVM<UserRole, string>> GetRoles()
        {
            var currentRole = Core.Utilities.AppStatic.Session.Role;
            var result = new List<KeyValueVM<UserRole, string>>();
            switch (currentRole)
            {
                case UserRole.Standard:
                    break;
                case UserRole.SystemAdmin:
                    result.Add(new KeyValueVM<UserRole, string>(UserRole.SystemAdmin, UserRole.SystemAdmin.GetDisplayNameValue()));
                    result.Add(new KeyValueVM<UserRole, string>(UserRole.Standard, UserRole.Standard.GetDisplayNameValue()));
                    break;
                default:
                    break;
            }

            return result;
        }

        public List<UserAutoCompleteResponse> SearchAutoComplete(SearchUserRequest request)
        {
            return userRepository.Where(x => x.RoleType <= CurrentUser.Role).Select(x => new UserAutoCompleteResponse(x)).ToList();
        }

        public DashboardContainer GetDashboard(DashboardRequest request)
        {
            #region Calculate Date
            var now = Core.Helper.UtilitiesHelper.DateTimeNow();
            switch (request.Type)
            {
                case DashboardRequestType.Today:
                    request.StartDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                    request.EndDate = request.StartDate.Value.AddDays(1).AddMilliseconds(-1);
                    break;
                case DashboardRequestType.ThisWeek:
                    request.StartDate = now.StartOfWeek(DayOfWeek.Monday);
                    request.EndDate = request.StartDate.Value.AddDays(7).AddMilliseconds(-1);
                    break;
                case DashboardRequestType.ThisMonth:
                    request.StartDate = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
                    request.EndDate = request.StartDate.Value.AddMonths(1).AddMilliseconds(-1);
                    break;
                case DashboardRequestType.LastThreeMonths:
                    request.StartDate = new DateTime(now.Year, now.Month - 2, 1, 0, 0, 0);
                    request.EndDate = request.StartDate.Value.AddMonths(3).AddMilliseconds(-1);
                    break;
                case DashboardRequestType.LastSixMonths:
                    request.StartDate = new DateTime(now.Year, now.Month - 5, 1, 0, 0, 0);
                    request.EndDate = request.StartDate.Value.AddMonths(6).AddMilliseconds(-1);
                    break;
                case DashboardRequestType.ThisYear:
                    request.StartDate = new DateTime(now.Year, 1, 1, 0, 0, 0);
                    request.EndDate = request.StartDate.Value.AddYears(1).AddMilliseconds(-1);
                    break;
            }
            #endregion
            var response = new DashboardContainer();
            response.Title = "Dashboard";

            return response;
        }

        public NotifyMessageResponse<Guid> ForgetPassword(ForgetPasswordRequest request)
        {
            User user = null;
            if (!string.IsNullOrEmpty(request.Email))
                user = userRepository.Get(x => x.Email == request.Email);
            //else if (!string.IsNullOrEmpty(request.Tckn))
            //    user = userRepository.Get(x => x.TCKN == request.Tckn);

            if (user != null)
            {
                user.ResetPasswordCode = Guid.NewGuid().ToString().Replace("-", "");

                userRepository.Update(user);

                //Şifre belirleme maili
                SendMailForgetPassword(user);

                return new NotifyMessageResponse<Guid>(Guid.NewGuid(), $"{user.Email} adresine şifre sıfırlama maili gönderilmiştir.");
            }
            else
                throw new BadRequestException("Kullanıcı bulunamadı");

        }

        public NotifyMessageResponse<Guid> NewPasswordByResetCode(NewPasswordByResetCodeRequest request)
        {
            var user = userRepository.Get(x => x.ResetPasswordCode == request.ResetCode);

            if (user != null)
            {
                if (request.NewPassword == request.ConfirmNewPassword)
                {
                    user.Password = request.NewPassword.ToMD5Hash();
                    user.ResetPasswordCode = null; //aynı kod tekrar kullanılmamalıdır.

                    userRepository.Update(user);

                    ClearOutputCache(CachePath.UsersLogin);
                    //TODO: şifreniz değişti maili atılabilir
                    return new NotifyMessageResponse<Guid>(Guid.NewGuid(), $"Sayın {user.FirstName} {user.LastName} şifreni başarıyla değiştirilmiştir.");
                }
                else
                {
                    throw new BadRequestException("Yeni şifrenizi doğrulamasını kontrol ediniz.");
                }
            }
            else
            {
                throw new BadRequestException("Şifre sıfırlama bilgileri yanlış.");
            }
        }

        public void SendMailForgetPassword(User user)
        {
            //change route
            //reset route
            var model = new Core.ViewModel.Mail.SendMailModel()
            {
                AppealUserName = $"{user.FirstName} {user.LastName}",
                To = new List<string>() { user.Email },
                Subject = "Şifremi Unuttum",
                HtmlContent = Core.Extensions.CommonExtensions.GetMailTemplateString("ForgetPasswordMail.html"),
                ButtonName = "Yeni Şifreni Belirle",
                ButtonLink = "change?code=" + System.Web.HttpUtility.UrlEncode(user.ResetPasswordCode)
            };
            var _mailService = base.ResolveService<IMailHelper>();
            _mailService.SendMail(model);
        }

        public void Logout()
        {
        }


        public bool DatabaseInitializer(IConfiguration configuration, IServiceCollection services)
        {
            return userRepository.DatabaseInitializer(configuration, services);
        }

        public DataTable GetQuery(string query)
        {
            return userRepository.GetQuery(query);
        }

        public void SendCreatePasswordMail(User user)
        {
            user.Password = "NonSet";
            user.ResetPasswordCode = Guid.NewGuid().ToString().Replace("-", "");
            userRepository.Update(user);

            var model = new Core.ViewModel.Mail.SendMailModel()
            {
                AppealUserName = $"{user.FirstName} {user.LastName}",
                To = new List<string>() { user.Email },
                Subject = "Şifre Belirleme",
                HtmlContent = Core.Extensions.CommonExtensions.GetMailTemplateString("CustomerNewUserMail.html"),
                ButtonName = "Yeni Şifreni Belirle",
                ButtonLink = "change?code=" + System.Web.HttpUtility.UrlEncode(user.ResetPasswordCode)
            };
            var _mailService = base.ResolveService<IMailHelper>();
            _mailService.SendMail(model);
        }


        public NotifyMessageResponse<long> RemoveProfileImage(long userId)
        {

            if (IsTopAdmin || CurrentUser.Id == userId)
            {
                var user = userRepository.FindOrThrow(userId);

                user.ProfileImageUrl = null;
                userRepository.Update(user);

                return new NotifyMessageResponse<long>(userId, $"{user.GetUserName()} profil resmi kaldırılmıştır.");
            }
            else
            {
                return new NotifyMessageResponse<long>(userId, $"Yekiniz olmayan personelin fotoğrafını kaldıramazsınız.");
            }
        }
    }
}
