using ETicaretAPI.Application.Abstractions.Services.Authentications;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IAuthService : IExternalAuthentication, IInternalAuthentication
    {
        Task PasswordResetAsnyc(string email);
        Task<bool> VerifyResetTokenAsync(string resetToken, string userId);
    }
}
