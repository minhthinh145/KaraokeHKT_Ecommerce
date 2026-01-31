﻿using QLQuanKaraokeHKT.Application.DTOs.AuthDTOs;

namespace QLQuanKaraokeHKT.Application.Services.Interfaces.Auth
{
    public interface IVerifyAuthService
    {
        Task<ServiceResult> VerifyAccountByEmail(VerifyAccountDTO verifyAccountDto);
    }
}
