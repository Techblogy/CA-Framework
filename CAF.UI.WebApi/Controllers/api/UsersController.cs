using System;
using System.Collections.Generic;
using System.Data;

using CAF.Core.Enums;
using CAF.Core.Helper.Interface;
using CAF.Core.Service;
using CAF.Core.Utilities;
using CAF.Core.ViewModel.Common.Response;
using CAF.Core.ViewModel.Notification.Request;
using CAF.Core.ViewModel.User.Request;
using CAF.Core.ViewModel.User.Response;
using CAF.UI.WebApi.Filters;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CAF.UI.WebApi.Controllers.api
{
    [Route(route)]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMailHelper _mailService;

        public UsersController(IUserService userService, IMailHelper mailService, IConfiguration configuration, IAppSettings appSettings) : base(configuration, appSettings)
        {
            _userService = userService;
            _mailService = mailService;
        }
        [HttpPost]
        [AllowAnonymous]
        public virtual ActionResult<LoginUserResponse> Login([FromBody] LoginUserRequest request, [FromServices] IActionLogService actionLogService)
        {
            var result = _userService.Login(request, base.AppSettings.Secret);
            actionLogService.AddLog(Core.Enums.ActionLogType.Login, "", result.UserId);
            return result;
        }

        #region CRUD
        [HttpPost]
        public virtual ActionResult<List<UserGridVM>> GetUserGridData([FromBody] GetUserGridRequest request)
        {
            return _userService.GetAllUsers(request);
        }

        [HttpPost]
        public virtual ActionResult<long> AddUser([FromBody] AddUserRequest request)
        {
            return _userService.AddUser(request);
        }

        [HttpPost]
        [Route("{id:long}", Name = "GetUser")]
        public virtual ActionResult<UserVM> GetUser([FromRoute] long id)
        {
            return _userService.GetUser(id);
        }

        [HttpPost]
        public virtual ActionResult<long> UpdateUser([FromBody] UpdateUserRequest request)
        {
            _userService.UpdateUser(request);
            return request.Id;
        }

        [HttpPost]
        [Route("{id:long}")]
        public virtual ActionResult DeleteUser([FromRoute] long id)
        {
            _userService.DeleteUser(id);
            return NoContent();
        }
        #endregion

        [HttpPost]
        public virtual ActionResult<List<KeyValueVM<UserRole, string>>> GetUserRoles()
        {
            return _userService.GetRoles();
        }

        [HttpPost]
        [OutputCacheFilter(ExpiryHours = 20)]
        public virtual ActionResult<List<UserAutoCompleteResponse>> SearchAutoComplete([FromBody] SearchUserRequest request)
        {
            return _userService.SearchAutoComplete(request);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual ActionResult<NotifyMessageResponse<Guid>> NewPasswordByResetCode([FromBody] NewPasswordByResetCodeRequest request)
        {
            return _userService.NewPasswordByResetCode(request);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual ActionResult<NotifyMessageResponse<Guid>> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            return _userService.ForgetPassword(request);
        }

        [HttpPost]
        [HttpGet]
        public virtual ActionResult<bool> Logout()
        {
            _userService.Logout();
            return Ok(true);
        }

        [HttpPost]
        [Authorize(Roles = Core.Constant.UserRoleConstant.Admin)]
        public virtual ActionResult<bool> Initializer([FromServices] IServiceCollection services)
        {
            return Ok(_userService.DatabaseInitializer(base.Configuration, services));
        }

        [HttpPost]
        public virtual ActionResult<DataTable> GetQuery([FromBody] string query)
        {
            return Ok(_userService.GetQuery(query));
        }

        [HttpPost]
        [Authorize(Roles = Core.Constant.UserRoleConstant.Admin)]
        [AllowAnonymous]
        public virtual ActionResult<AppSettings> GetAppSettings()
        {
            return Ok((AppSettings)base.AppSettings);
        }

        [HttpPost, HttpGet]
        [Route("{userId:long}")]
        public virtual ActionResult<NotifyMessageResponse<long>> RemoveProfileImage([FromRoute] long userId)
        {
            _userService.Logout();
            return Ok(_userService.RemoveProfileImage(userId));
        }

        #region SignalR
        [HttpPost]
        public virtual ActionResult<bool> SendNotification([FromServices] INotificationService service, [FromBody] BasiNotificationRequest request)
        {
            service.SendNotification(request);
            return Ok(true);
        }
        #endregion
    }
}
