using CAF.Core.Validation.User;
using CAF.Core.ViewModel.User.Request;

using FluentValidation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CAF.Application.Dependency
{
    internal static class ValidationInjection
    {
        internal static IServiceCollection AddValidations(this IServiceCollection services, IConfiguration configuration)
        {

            #region User
            services.AddTransient<IValidator<LoginUserRequest>, LoginUserRequestValidation>();
            services.AddTransient<IValidator<AddUserRequest>, AddUserRequestValidation>();
            services.AddTransient<IValidator<GetUserGridRequest>, GetUserGridRequestValidation>();
            services.AddTransient<IValidator<UpdateUserRequest>, UpdateUserRequestValidation>();
            services.AddTransient<IValidator<SearchUserRequest>, SearchUserRequestValidation>();
            services.AddTransient<IValidator<NewPasswordByResetCodeRequest>, NewPasswordByResetCodeRequestValidation>();
            services.AddTransient<IValidator<ForgetPasswordRequest>, ForgetPasswordRequestValidation>();
            #endregion

            #region Public
            #endregion

            return services;
        }
    }
}
