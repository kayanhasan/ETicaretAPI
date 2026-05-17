using ETicaretAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        DTOs.Token CreateAccessToken(int second, AppUser appUser);
        string CreateRefreshToken();
    }
}
